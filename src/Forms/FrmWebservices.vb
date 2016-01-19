Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.GISClient
Imports System.Text
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase


Public Class FrmWebservices



    Private Sub BtnSet_Click(sender As System.Object, e As System.EventArgs) Handles BtnSet.Click
        Dim pGxDialog As IGxDialog = New GxDialog
        pGxDialog.AllowMultiSelect = False
        pGxDialog.Title = "Browse For Feature Service"
        'Dim pGxFilter As IGxObjectFilter = New GxFilterMapServers
        'Dim pGxFilter As IGxObjectFilter = New GxFilterMapDatasetsLayersAndResults
        Dim pGxFilter As IGxObjectFilter = New GxFilterFeatureServers
        pGxDialog.ObjectFilter = pGxFilter
        Dim pGxObjects As IEnumGxObject = Nothing
        If pGxDialog.DoModalOpen(0, pGxObjects) Then
            pGxObjects.Reset()
            Dim pGxObj As IGxObject = pGxObjects.Next
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            Dim sName As IAGSServerObjectName = agsObj.AGSServerObjectName
            Dim url As String = agsObj.AGSServerObjectName.URL
            Dim propertySet As IPropertySet = agsObj.AGSServerObjectName.AGSServerConnectionName.ConnectionProperties()
            'Build the REST url
            Dim prefix As String = propertySet.GetProperty(BA_Property_RestUrl)
            'Extract the selected service information
            Dim idxServices As Integer = url.IndexOf(BA_Url_Services)
            Dim idxMapServer As Integer = url.IndexOf(BA_Url_MapServer)
            Dim serviceText As String = url.Substring(idxServices, idxMapServer - idxServices - 1)   'subtract 1 to avoid trailing /
            'Example: http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB_ALL/AWDB_SNOTEL_ALL/FeatureServer/0
            TxtWebService.Text = prefix & serviceText & BA_EnumDescription(PublicPath.FeatureServiceUrl)
            cboFields.Items.Clear()
            'Dim names(propertySet.Count - 1) As Object
            'Dim values(propertySet.Count - 1) As Object
            'propertySet.GetAllProperties(names, values)
            'Dim sb As StringBuilder = New StringBuilder()
            'For i As Integer = 0 To propertySet.Count - 1
            '    sb.Append(CStr(names(i)) & vbCrLf)
            '    sb.Append(values(i).ToString & vbCrLf)
            'Next
            'Windows.Forms.MessageBox.Show(sb.ToString)
        End If


    End Sub

    Private Sub BtnFields_Click(sender As System.Object, e As System.EventArgs) Handles BtnFields.Click
        cboFields.Items.Clear()
        Dim fieldNames As IList(Of String) = BA_QueryFeatureServiceFieldNames(TxtWebService.Text, ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
        For Each fName As String In fieldNames
            cboFields.Items.Add(fName)
        Next
        cboFields.SelectedIndex = 0
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        BtnFields.Focus()
    End Sub

    Private Sub BtnImageFile_Click(sender As System.Object, e As System.EventArgs) Handles BtnImage.Click
        Dim pGxDialog As IGxDialog = New GxDialog
        pGxDialog.AllowMultiSelect = False
        pGxDialog.Title = "Browse For Image Service"
        Dim pGxFilter As IGxObjectFilter = New GxFilterImageServers

        pGxDialog.ObjectFilter = pGxFilter
        Dim pGxObjects As IEnumGxObject = Nothing
        If pGxDialog.DoModalOpen(0, pGxObjects) Then
            pGxObjects.Reset()
            Dim pGxObj As IGxObject = pGxObjects.Next
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            TxtImageUrl.Text = agsObj.AGSServerObjectName.URL
        End If
    End Sub

    Private Sub BtnClip_Click(sender As System.Object, e As System.EventArgs) Handles BtnClip.Click
        'Dim clipFilePath As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb\aoib"
        Dim clipFilePath As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb\aoi_v"
        Dim newFilePath As String = "C:\Docs\Lesley\teton_aoi\layers.gdb\dem_web"
        Dim success As BA_ReturnCode = BA_ClipImageServiceToVector(clipFilePath, TxtImageUrl.Text, newFilePath)
    End Sub

    Private Sub BtnTest_Click(sender As System.Object, e As System.EventArgs) Handles BtnTest.Click
        'Dim aoiPath As String = "C:\Docs\Lesley\New_Basin\Fern_Ridge_Lake_Inflow_01122016"
        'Dim clipFilePath As String = aoiPath & "\aoi.gdb\aoib_v"
        'Dim newFilePath As String = aoiPath & "\layers.gdb\snotel_sites_web"
        'BA_ClipFeatureService(clipFilePath, TxtWebService.Text, newFilePath, aoiPath)
        Dim cellSize As Double = BA_CellSize("C:\Docs\Lesley\teton_aoi\surfaces.gdb", "dem_filled")
        Dim xCols As Long = -1
        Dim yRows As Long = -1
        Dim extent As IEnvelope = Nothing
        BA_GetColumnRowCountFromVector("C:\Docs\Lesley\teton_aoi\aoi.gdb\p_aoi_v", cellSize, cellSize, _
                                       extent, xCols, yRows)
        Dim featClass As IFeatureClass = BA_OpenFeatureClassFromGDB("C:\Docs\Lesley\teton_aoi\aoi.gdb", "aoi_v")
        Dim snapRasterPath As String = "C:\Docs\Lesley\BASIN2\surfaces.gdb\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        'Dim snapRasterPath As String = TxtImageUrl.Text
        Dim newFileName As String = "snapTest2"
        Dim layersGdb As String = "C:\Docs\Lesley\teton_aoi\layers.gdb"
        If BA_File_Exists(layersGdb & "\" & newFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            BA_RemoveRasterFromGDB(layersGdb, newFileName)
        End If
        BA_ShapeFile2RasterGDB(featClass, layersGdb, newFileName, cellSize, BA_FIELD_AOI_NAME, snapRasterPath)
    End Sub
End Class