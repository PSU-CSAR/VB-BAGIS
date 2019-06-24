Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmExportMapPackage

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook

        If String.IsNullOrEmpty(AOIFolderBase) Then
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.FrmExportMapPackage
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

    Public Sub InitializeForm(ByVal strExportFolder As String, ByVal strCurrentMapTitle As String)
        txtExportFolder.Text = strExportFolder
        TxtCurrentMap.Text = strCurrentMapTitle
    End Sub

    Public WriteOnly Property SelectedMap(ByVal strSelectedMap) As String
        Set(value As String)
            TxtCurrentMap.Text = strSelectedMap
        End Set
    End Property

    ''' <summary>
    ''' Implementation class of the dockable window add-in. It is responsible for
    ''' creating and disposing the user interface class for the dockable window.
    ''' </summary>
    Public Class AddinImpl
        Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As FrmExportMapPackage

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmExportMapPackage(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        Protected Friend ReadOnly Property UI() As FrmExportMapPackage
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

        Dim files As String() = {BA_ExportChartAreaElevPdf, BA_ExportChartAreaElevPrecipPdf, BA_ExportChartAreaElevPrecipSitePdf,
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
End Class