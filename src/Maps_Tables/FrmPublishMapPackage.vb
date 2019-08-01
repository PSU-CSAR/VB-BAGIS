Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports TheArtOfDev.HtmlRenderer.PdfSharp
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Framework

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

    Dim m_charts_all As String()
    Dim m_maps_all As String() = {BA_ExportMapElevPdf, BA_ExportMapElevStelPdf, BA_ExportMapElevScPdf,
                                  BA_ExportMapElevPrecipPdf, BA_ExportMapAspectPdf, BA_ExportMapSlopePdf}
    Dim m_mapsSettings As MapsSettings
    Dim m_currentMap As String

    Public Sub InitializeForm()
        txtExportFolder.Text = AOIFolderBase + BA_ExportMapPackageFolder
        m_mapsSettings = BA_ReadMapSettings()
        If m_mapsSettings.UseSubRange = False Then
            m_charts_all = {BA_ExportChartAreaElevPdf, BA_ExportChartAreaElevPrecipPdf, BA_ExportChartAreaElevPrecipSitePdf,
            BA_ExportChartAreaElevSnotelPdf, BA_ExportChartAreaElevScosPdf, BA_ExportChartSlopePdf, BA_ExportChartAspectPdf,
            BA_ExportChartElevPrecipCorrelPdf}
        Else
            m_charts_all = {BA_ExportChartAreaElevPdf, BA_ExportChartAreaElevPrecipPdf, BA_ExportChartAreaElevPrecipSitePdf,
            BA_ExportChartAreaElevSnotelPdf, BA_ExportChartAreaElevScosPdf, BA_ExportChartSlopePdf, BA_ExportChartAspectPdf,
            BA_ExportChartElevPrecipCorrelPdf, BA_ExportChartAreaElevSubrangePdf, BA_ExportChartAreaElevPrecipSubrangePdf, BA_ExportChartAreaElevPrecipSiteSubrangePdf,
            BA_ExportChartAreaElevSnotelSubrangePdf, BA_ExportChartAreaElevScosSubrangePdf}
        End If
        LoadDataGridView()
    End Sub

    Protected Friend Property CurrentMap() As String
        Get
            Return m_currentMap
        End Get
        Set(ByVal Value As String)
            m_currentMap = Value
        End Set
    End Property

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

    Private Sub CmdPublish_Click(sender As Object, e As EventArgs) Handles CmdPublish.Click
        Dim sOutputDir As String = AOIFolderBase + BA_ExportMapPackageFolder
        If Not System.IO.Directory.Exists(sOutputDir) Then
            System.IO.Directory.CreateDirectory(sOutputDir)
        End If

        'Check for existence of map package product; If it exists, ask if we should overwrite
        Dim success As BA_ReturnCode = BA_ReturnCode.OtherError
        If System.IO.File.Exists(sOutputDir + "\" + BA_ExportAllMapsChartsPdf) Then
            Dim strMessage As String = "A map package has already been created for this AOI at " +
                sOutputDir + "\" + BA_ExportAllMapsChartsPdf + "." + vbCrLf + "Do you wish to overwrite the existing " +
                "map package ?"
            Dim res As DialogResult = MessageBox.Show(strMessage, "BAGIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        success = BA_DeleteMapPackageElements()
        If success <> BA_ReturnCode.Success Then
            MessageBox.Show("An error occurred while deleting the map package!!", "BAGIS")
            Exit Sub
        End If

        'Check to see if 1 or more maps exist in the output directory
        Dim bMapsExist As Boolean = False
        For Each strMapFile As String In m_maps_all
            If System.IO.File.Exists(sOutputDir + "\" + strMapFile) Then
                bMapsExist = True
                Exit For
            End If
        Next
        'If they do, ask the user if they want to overwrite ?
        If bMapsExist = True Then
            Dim strMessage As String = "At least one map .pdf exists in  " + sOutputDir +
                "." + vbCrLf + "Do you wish to overwrite the existing maps ?"
            Dim res As DialogResult = MessageBox.Show(strMessage, "BAGIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res = DialogResult.Yes Then
                For Each strMapFile As String In m_maps_all
                    If System.IO.File.Exists(sOutputDir + "\" + strMapFile) Then
                        Try
                            System.IO.File.Delete(sOutputDir + "\" + strMapFile)
                        Catch ex As System.IO.IOException
                            MessageBox.Show("Unable to delete " + sOutputDir + "\" + strMapFile + "! The most likely cause is that you have the file open. " +
                                        "Please check and try again.", "BAGIS")
                            Exit Sub
                        End Try
                    End If
                    'Also check for/delete any associated .mxd
                    Dim fileNameBase As String = System.IO.Path.GetFileNameWithoutExtension(strMapFile)
                    Dim sMxdFullPath As String = sOutputDir + "\" + fileNameBase + ".mxd"
                    If System.IO.File.Exists(sMxdFullPath) Then
                        Try
                            System.IO.File.Delete(sMxdFullPath)
                        Catch ex As System.IO.IOException
                            MessageBox.Show("Unable to delete " + sMxdFullPath +
                                            "!" + vbCrLf + "The most likely cause is that you are using the .mxd in ArcMap. " +
                                            "Use the 'Save As' menu item to save the .mxd under a different name and try again.", "BAGIS")
                            Exit Sub
                        End Try
                    End If
                Next
            End If
        End If

        'Declare progress indicator variables
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Publishing map pdf documents", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()

            ' Publish the maps
            success = PublishMaps(AOIFolderBase + BA_ExportMapPackageFolder, pStepProg)
            pStepProg.Hide()
            progressDialog2.HideDialog()

            ' Open the output document
            Dim outputDocument As PdfDocument = New PdfDocument()
            PublishCharts(AOIFolderBase + BA_ExportMapPackageFolder, outputDocument)

            'Re-initialize the step progressor
            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Publishing map package", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Message = "Publishing title page..."
            pStepProg.Step()

            PublishTitlePage(AOIFolderBase + BA_ExportMapPackageFolder)

            pStepProg.Message = "Assembling full document..."
            pStepProg.Step()

            'Check for files to prepare list for concatenation
            Dim lstFoundFiles As IList(Of String) = New List(Of String)
            'title page
            If BA_File_ExistsWindowsIO(AOIFolderBase + BA_ExportMapPackageFolder + "\" + BA_TitlePagePdf) Then
                lstFoundFiles.Add(AOIFolderBase + BA_ExportMapPackageFolder + "\" + BA_TitlePagePdf)
            End If

            'maps
            For Each strFile In m_maps_all
                Dim fullPath As String = AOIFolderBase + BA_ExportMapPackageFolder + "\" + strFile
                If BA_File_ExistsWindowsIO(fullPath) Then
                    lstFoundFiles.Add(fullPath)
                End If
            Next

            'charts
            For Each strFile In m_charts_all
                Dim fullPath As String = AOIFolderBase + BA_ExportMapPackageFolder + "\" + strFile
                If BA_File_ExistsWindowsIO(fullPath) Then
                    lstFoundFiles.Add(fullPath)
                End If
            Next

            ' Iterate through files
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

            'Save the document...
            Dim concatFileName As String = AOIFolderBase + BA_ExportMapPackageFolder + "\" + BA_ExportAllMapsChartsPdf
            outputDocument.Save(concatFileName)

            'Reload the datagrid
            LoadDataGridView()
            MessageBox.Show("Document published!")
        Catch ex As Exception
            Debug.Print("CmdPublish_Click" + ex.Message)
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
        End Try
    End Sub

    Private Sub PublishCharts(ByVal parentPath As String, ByRef outputDocument As PdfDocument)
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
        Dim DisplayConversion_Factor As Double = BAGIS_ClassLibrary.BA_SetConversionFactor(m_mapsSettings.ZMeters, True)
        Dim dblMinElev As Double = Math.Round(pRasterStats.Minimum * DisplayConversion_Factor - 0.005, 2)  'adjust value to include the actual min, max
        Dim dblMaxElev As Double = Math.Round(pRasterStats.Maximum * DisplayConversion_Factor + 0.005, 2)

        'Delete old .pdf files from previous runs (if they exist)
        For i As Integer = 1 To m_charts_all.Length - 1
            Dim fullPath As String = parentPath + m_charts_all(i)
            If BA_File_ExistsWindowsIO(fullPath) Then
                System.IO.File.Delete(fullPath)
            End If
        Next

        BA_GenerateTables(m_mapsSettings, dblMaxElev, dblMinElev, False)
    End Sub

    Private Sub CmdCancel_Click(sender As Object, e As EventArgs) Handles CmdCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmPublishMapPackage
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub PublishTitlePage(ByVal parentPath As String)
        Dim comboBox = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim aoiName As String = comboBox.getValue().ToUpper

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

    Private Function PublishMaps(ByVal parentPath As String, ByRef pStepProg As IStepProgressor) As BA_ReturnCode
        Dim document As IDocument = My.ArcMap.Document
        Dim uid As UID
        Dim success As BA_ReturnCode = BA_ReturnCode.OtherError
        For Each strMap As String In m_maps_all
            Dim bPublishMap As Boolean = False
            If Not System.IO.File.Exists(parentPath + "\" + strMap) Then
                pStepProg.Message = "Publishing " + strMap + "..."
                pStepProg.Step()
                Select Case strMap
                    Case BA_ExportMapElevPdf
                        Dim ElevDistButton As BtnElevationDist = AddIn.FromID(Of BtnElevationDist)(My.ThisAddIn.IDs.BtnElevationDist)
                        If ElevDistButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnElevationDist"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                    Case BA_ExportMapElevStelPdf
                        Dim ElevSNOTELButton As BtnElevationSNOTEL = AddIn.FromID(Of BtnElevationSNOTEL)(My.ThisAddIn.IDs.BtnElevationSNOTEL)
                        If ElevSNOTELButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnElevationSNOTEL"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                        bPublishMap = True
                    Case BA_ExportMapElevScPdf
                        Dim ElevScosButton As BtnElevSnowCourse = AddIn.FromID(Of BtnElevSnowCourse)(My.ThisAddIn.IDs.BtnElevSnowCourse)
                        If ElevScosButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnElevSnowCourse"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                        bPublishMap = True
                    Case BA_ExportMapElevPrecipPdf
                        Dim PrecipButton As BtnPrecipitationDist = AddIn.FromID(Of BtnPrecipitationDist)(My.ThisAddIn.IDs.BtnPrecipitationDist)
                        If PrecipButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnPrecipitationDist"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                        bPublishMap = True
                    Case BA_ExportMapSlopePdf
                        Dim SlopeButton As BtnSlopeDist = AddIn.FromID(Of BtnSlopeDist)(My.ThisAddIn.IDs.BtnSlopeDist)
                        If SlopeButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnSlopeDist"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                        bPublishMap = True
                    Case BA_ExportMapAspectPdf
                        Dim AspectButton As BtnAspectDist = AddIn.FromID(Of BtnAspectDist)(My.ThisAddIn.IDs.BtnAspectDist)
                        If AspectButton.SelectedProperty = True Then
                            uid = New UIDClass()
                            uid.Value = "Microsoft_BAGIS_BtnAspectDist"
                            Dim commandItem As ICommandItem = document.CommandBars.Find(uid)
                            commandItem.Execute()
                            bPublishMap = True
                        End If
                        bPublishMap = True
                End Select
                If bPublishMap = True Then
                    success = BA_ExportActiveViewAsPdf(parentPath, strMap, BA_MapPdfOutputResolution, BA_MapPdfResampleRatio, False)
                End If
            End If
        Next
        Return success
    End Function

    Private Sub LoadDataGridView()
        DataGridView1.Rows.Clear()
        For Each strFile In m_maps_all
            Dim rowId As Int16 = DataGridView1.Rows.Add()
            Dim row As DataGridViewRow = DataGridView1.Rows.Item(rowId)
            With row
                .Cells("file_name").Value = strFile
                If System.IO.File.Exists(AOIFolderBase + BA_ExportMapPackageFolder + "\" + strFile) Then
                    Dim datePublished As DateTime =
                        System.IO.File.GetLastWriteTime(AOIFolderBase + BA_ExportMapPackageFolder + "\" + strFile)
                    .Cells("Published").Value = datePublished.ToString("MM/dd/yy H:mm:ss")
                End If
            End With
        Next
        ' Clear any selected cells
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing
    End Sub
End Class