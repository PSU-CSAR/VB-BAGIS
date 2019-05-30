Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class BtnExportMapPackage
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        If String.IsNullOrEmpty(AOIFolderBase) Then
            MessageBox.Show("You must select an AOI before exporting!!", "BAGIS")
            Exit Sub
        End If
        If Not BA_File_ExistsWindowsIO(BA_GetPath(AOIFolderBase, PublicPath.Maps) +
            "\" + BA_MapParameterFile) Then
            MessageBox.Show("The map settings have not been configured for this AOI. Use the Generate Maps button to complete the configuration!!", "BAGIS")
            Exit Sub
        End If
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

        Dim parentPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) + "\"
        Dim files As String() = {"title_page.pdf", BA_ChartsPdf, BA_RangeChartsPdf, BA_ElevPrecipPdf}
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
            Dim fullPath As String = parentPath + strFile
            If BA_File_ExistsWindowsIO(fullPath) Then
                lstFoundFiles.Add(fullPath)
            End If
        Next

        '' Open the output document
        Dim outputDocument As PdfDocument = New PdfDocument()

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

        'Save the document...
        Dim concatFileName As String = parentPath + BA_GetBareName(AOIFolderBase) + ".pdf"
        outputDocument.Save(concatFileName)
        MessageBox.Show("Document saved!")
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
