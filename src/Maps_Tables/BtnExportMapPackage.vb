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
            MessageBox.Show("You must select an AOI before exporting!!")
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

        Dim pRasterStats As ESRI.ArcGIS.DataSourcesRaster.IRasterStatistics =
            BAGIS_ClassLibrary.BA_GetDemStatsGDB(AOIFolderBase)
        Dim DisplayConversion_Factor As Double = BAGIS_ClassLibrary.BA_SetConversionFactor(oMapsSettings.ZMeters, True)
        Dim dblMinElev As Double = Math.Round(pRasterStats.Minimum * DisplayConversion_Factor - 0.005, 2)  'adjust value to include the actual min, max
        Dim dblMaxElev As Double = Math.Round(pRasterStats.Maximum * DisplayConversion_Factor + 0.005, 2)

        BA_GenerateTables(oMapsSettings, dblMaxElev, dblMinElev)

        'Dim parentPath As String = "C:\Docs\animas_AOI_prms\maps\"
        'Dim files As String() = {"title_page.pdf", "ElevDist.pdf", "ElevSnotel.pdf", "ElevSnow.pdf",
        '    "PrecipMap.pdf", "ElevAspect.pdf", "ElevSlope.pdf", "Tables_Resize.pdf", "TablePrecip.pdf"}

        '' Open the output document
        'Dim outputDocument As PdfDocument = New PdfDocument()

        '' Iterate through files
        'For Each strFileName As String In files
        '    Dim fullPath As String = parentPath + strFileName
        '    'Open the document to import pages from it.
        '    Dim inputDocument As PdfDocument = PdfReader.Open(fullPath, PdfDocumentOpenMode.Import)
        '    'Iterate pages
        '    Dim count As Int16 = inputDocument.PageCount
        '    For idx As Int16 = 0 To count - 1
        '        'Get the page from the external document...
        '        Dim page As PdfPage = inputDocument.Pages(idx)
        '        '...And add it to the output document.
        '        outputDocument.AddPage(page)
        '    Next
        'Next

        ''Save the document...
        'Dim concatFileName As String = parentPath + "animas_AOI_prms.pdf"
        'outputDocument.Save(concatFileName)
        'MessageBox.Show("Document saved!")
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
