Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports TheArtOfDev.HtmlRenderer.PdfSharp
Imports System.Windows.Forms

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmPublishMapPackage

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook

        If String.IsNullOrEmpty(AOIFolderBase) Then
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.FrmPublishMapPackage
            dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
            dockWindow.Show(False)
        End If

    End Sub


    Private m_hook As Object
    ''' <summary>
    ''' Host object of the dockable window
    ''' </summary> 
    Public Property Hook() As Object
        Get
            Return m_hook
        End Get
        Set(ByVal value As Object)
            m_hook = value
        End Set
    End Property

    Public Sub InitializeForm(ByVal strExportFolder As String)
        txtExportFolder.Text = strExportFolder
        Dim rowId As Int16 = DataGridView1.Rows.Add()
        Dim row As DataGridViewRow = DataGridView1.Rows.Item(rowId)
        With row
            .Cells("file_name").Value = "map_elevation.pdf"
            .Cells("Published_YN").Value = "Y"
        End With
        rowId = DataGridView1.Rows.Add()
        Dim row1 As DataGridViewRow = DataGridView1.Rows.Item(rowId)
        With row1
            .Cells("file_name").Value = "map_elevation_snotel.pdf"
            .Cells("Published_YN").Value = "N"
        End With
        rowId = DataGridView1.Rows.Add()
        Dim row2 As DataGridViewRow = DataGridView1.Rows.Item(rowId)
        With row2
            .Cells("file_name").Value = "map_elevation_sc.pdf"
            .Cells("Published_YN").Value = "N"
        End With
    End Sub

    ''' <summary>
    ''' Implementation class of the dockable window add-in. It is responsible for
    ''' creating and disposing the user interface class for the dockable window.
    ''' </summary>
    Public Class AddinImpl
        Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As FrmPublishMapPackage

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmPublishMapPackage(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        Protected Friend ReadOnly Property UI() As FrmPublishMapPackage
            Get
                Return m_windowUI
            End Get
        End Property

    End Class

    Private Sub CmdExport_Click(sender As Object, e As EventArgs) Handles CmdPublish.Click
        BA_ExportMapPackageFolder = txtExportFolder.Text

        If Not String.IsNullOrEmpty(BA_ExportMapPackageFolder) Then
            If Not System.IO.Directory.Exists(BA_ExportMapPackageFolder) Then
                System.IO.Directory.CreateDirectory(BA_ExportMapPackageFolder)
            End If
        End If
        '' Open the output document
        Dim outputDocument As PdfDocument = New PdfDocument()
        GenerateCharts(BA_ExportMapPackageFolder, outputDocument)

        'Save the document...
        Dim concatFileName As String = BA_ExportMapPackageFolder + "\sample_charts.pdf"
        outputDocument.Save(concatFileName)
        Windows.Forms.MessageBox.Show("Document saved!")
    End Sub

    Private Sub GenerateCharts(ByVal parentPath As String, ByRef outputDocument As PdfDocument)
        Dim oMapsSettings As MapsSettings = BA_ReadMapSettings()
        'Check for Snotel and snow course layers
        If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) +
                          BA_EnumDescription(MapsFileName.Snotel), WorkspaceType.Geodatabase,
                          ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
            AOI_HasSNOTEL = True
        End If
        If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) +
                          BA_EnumDescription(MapsFileName.SnowCourse), WorkspaceType.Geodatabase,
                          ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
            AOI_HasSnowCourse = True
        End If

        Dim pRasterStats As ESRI.ArcGIS.DataSourcesRaster.IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)
        Dim DisplayConversion_Factor As Double = BAGIS_ClassLibrary.BA_SetConversionFactor(oMapsSettings.ZMeters, True)
        Dim dblMinElev As Double = Math.Round(pRasterStats.Minimum * DisplayConversion_Factor - 0.005, 2)  'adjust value to include the actual min, max
        Dim dblMaxElev As Double = Math.Round(pRasterStats.Maximum * DisplayConversion_Factor + 0.005, 2)

        Dim files As String() = {BA_TitlePagePdf, BA_ExportChartAreaElevPdf, BA_ExportChartAreaElevPrecipPdf, BA_ExportChartAreaElevPrecipSitePdf,
            BA_ExportChartAreaElevSnotelPdf, BA_ExportChartAreaElevScosPdf, BA_ExportChartSlopePdf, BA_ExportChartAspectPdf,
            BA_RangeChartsPdf, BA_ExportChartElevPrecipCorrelPdf}
        'Delete old .pdf files from previous runs (if they exist)
        For i As Integer = 1 To files.Length - 1
            Dim fullPath As String = parentPath + files(i)
            If BA_File_ExistsWindowsIO(fullPath) Then
                System.IO.File.Delete(fullPath)
            End If
        Next

        BA_GenerateTables(oMapsSettings, dblMaxElev, dblMinElev, False)

        PublishTitlePage(parentPath)

        'Check for files to prepare list for concatenation
        Dim lstFoundFiles As IList(Of String) = New List(Of String)
        For Each strFile In files
            Dim fullPath As String = parentPath + "\" + strFile
            If BA_File_ExistsWindowsIO(fullPath) Then
                lstFoundFiles.Add(fullPath)
            End If
        Next

        '' Iterate through files
        For Each strFullPath As String In lstFoundFiles
            'Open the document to import pages from it.
            Dim inputDocument As PdfDocument = PdfReader.Open(strFullPath, PdfDocumentOpenMode.Import)
            'Iterate pages
            Dim count As Int16 = inputDocument.PageCount
            For idx As Int16 = 0 To count - 1
                'Get the page from the external document...
                Dim page As PdfPage = inputDocument.Pages(idx)
                '...And add it to the output document.
                outputDocument.AddPage(page)
            Next
        Next
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmPublishMapPackage
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub PublishTitlePage(ByVal parentPath As String)
        Dim comboBox = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim aoiName As String = comboBox.getValue()

        'Save the values for the title page in an .xml file
        Dim titlePage As ExportTitlePage = New ExportTitlePage
        With titlePage
            .aoi_name = aoiName
            .publisher = TxtPublisher.Text.Trim
            .comments = TxtComments.Text.Trim
            .local_path = AOIFolderBase.Trim
            .date_created = DateAndTime.Now
        End With
        Dim xmlOutputPath As String = parentPath + "\title_page.xml"
        titlePage.Save(xmlOutputPath)

        'Format the xml file using an xsl style sheet to produce an html document
        Dim xslTemplate As String = BA_GetAddInDirectory() & BA_EnumDescription(PublicPath.TitlePageXsl)
        Dim xslFileExists As Boolean = BA_File_ExistsWindowsIO(xslTemplate)
        Dim htmlFile As String = parentPath + "\title_page.html"
        If xslFileExists Then
            Dim inputFile As String = xmlOutputPath
            Dim success As BA_ReturnCode = BA_XSLTransformToHtml(inputFile, xslTemplate, htmlFile)
        End If

        'Read the contents of the html file into a string and use the PDFGenerator to generate the title page
        'in .pdf format
        If System.IO.File.Exists(htmlFile) Then
            Dim htmlText As String = System.IO.File.ReadAllText(htmlFile)
            Dim titlePagDoc As PdfDocument = PdfGenerator.GeneratePdf(htmlText, PdfSharp.PageSize.Letter)
            titlePagDoc.Save(parentPath + "\" + BA_TitlePagePdf)
        End If
    End Sub
End Class