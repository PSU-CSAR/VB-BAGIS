Imports BAGIS_ClassLibrary

Public Class MapsSettings

    Public ZMeters As Boolean
    Public ElevationInterval As String
    Public IdxPrecipType As String
    Public UseSubRange As Boolean
    Public SubRangeFromElev As String
    Public SubRangeToElev As String
    Public MinimumPrecip As String
    Public MaximumPrecip As String
    Public AspectDirections As String = "8"  'Default

    '** Note: this logic is duplicated frmGenerateMaps.SetPrecipPathInfo() **
    Public ReadOnly Property PrecipPath() As String
        Get
            If IdxPrecipType = 0 Then  'read direct Annual PRISM raster
                Return BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
            ElseIf IdxPrecipType > 0 And IdxPrecipType < 5 Then 'read directly Quarterly PRISM raster
                Return BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
            Else 'sum individual monthly PRISM rasters
                Return BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            End If
        End Get
    End Property

    Public ReadOnly Property PRISMRasterName() As String
        Get
            If IdxPrecipType = 0 Then  'read direct Annual PRISM raster
                Return AOIPrismFolderNames.annual.ToString
            ElseIf IdxPrecipType > 0 And IdxPrecipType < 5 Then 'read directly Quarterly PRISM raster
                Return BA_GetPrismFolderName(IdxPrecipType + 12)
            Else 'sum individual monthly PRISM rasters
                Return BA_TEMP_PRISM
            End If
        End Get
    End Property


End Class
