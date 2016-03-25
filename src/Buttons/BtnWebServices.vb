Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary
Imports System.Web
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.Text
Imports ESRI.ArcGIS.GISClient

Public Class BtnWebServices
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        'Dim wForm As FrmWebservices = New FrmWebservices
        'wForm.ShowDialog()
        'AccessImageServerLayer()
        'AccessFeatureLayer()
        TestImageServices()
        MsgBox("Finish!")
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Protected Sub AccessImageServerLayer()
        Dim clipFilePath As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb\aoib"
        Dim imageUrl As String = "http://atlas.geog.pdx.edu/arcgis/services/30_Meters_DEM/westus_30m/ImageServer"
        Dim newFilePath As String = "C:\Docs\Lesley\teton_aoi\layers.gdb\dem_web"
        Dim success As BA_ReturnCode = BA_ClipImageService(clipFilePath, imageUrl, newFilePath)
    End Sub

    Protected Sub AccessFeatureLayer()
        Dim aoiPath As String = "C:\Docs\Lesley\teton_aoi"
        Dim clipFilePath As String = aoiPath & "\aoi.gdb\aoib_v"
        Dim webServiceUrl As String = "http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB/AWDB_COOP/FeatureServer/0"
        Dim newFilePath As String = aoiPath & "\layers.gdb\snotel_sites_web"
        Dim success As BA_ReturnCode = BA_ClipFeatureService(clipFilePath, webServiceUrl, newFilePath, aoiPath)
    End Sub

    Protected Sub QueryFields()
        Dim webServiceUrl As String = "http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB_ALL/AWDB_SNOTEL_ALL/FeatureServer/0"
        Dim fieldNames As IList(Of String) = BA_QueryFeatureServiceFieldNames(webServiceUrl, esriFieldType.esriFieldTypeString)
    End Sub

    Protected Sub TestImageServices()
        Dim AGSConnectionFactory As IAGSServerConnectionFactory = New AGSServerConnectionFactory
        Dim connectionProps As IPropertySet = New PropertySet
        Dim AGSConnection As IAGSServerConnection = Nothing
        'This works
        Dim fullPath As String = "http://basins.geog.pdx.edu/arcgis/services/BAGIS_GISLayers/DEM_westus_bc_30m/ImageServer"
        Dim layerExists As Boolean = BA_File_ExistsImageServer(fullPath)
        'This doesn't (DNS alias)
        fullPath = "http://webservices.geog.pdx.edu/arcgis/services/BAGIS_GISLayers/DEM_westus_bc_30m/ImageServer"
        layerExists = BA_File_ExistsImageServer(fullPath)
    End Sub

    Private Function File_ExistsImageServer(ByVal imageUrl As String) As Boolean
        Dim isLayer As IImageServerLayer = New ImageServerLayerClass
        Dim imageRaster As IRaster = Nothing
        Try
            isLayer.Initialize(imageUrl)
            imageRaster = isLayer.Raster
            Return True
        Catch ex As Exception
            ' An exception was thrown while trying to open the dataset, return false
            Return False
        End Try
    End Function

End Class

