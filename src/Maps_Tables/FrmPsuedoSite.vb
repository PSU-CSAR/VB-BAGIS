Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster

Public Class FrmPsuedoSite

    Private analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
    Private reclassElevFile As String = "elevrecl"

    Public Sub New(ByVal useMeters As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CmboxPrecipType.Items.Clear()
        With CmboxPrecipType
            .Items.Add("Annual Precipitation")
            .Items.Add("Jan - Mar Precipitation")
            .Items.Add("Apr - Jun Precipitation")
            .Items.Add("Jul - Sep Precipitation")
            .Items.Add("Oct - Dec Precipitation")
            .Items.Add("Custom")
            .SelectedIndex = 0
        End With

        CmboxBegin.Items.Clear()
        With CmboxBegin
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 0
        End With

        CmboxEnd.Items.Clear()
        With CmboxEnd
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 11
        End With

        'read dem min, max everytime the form is activated
        'display dem elevation stats
        Dim pRasterStats As IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)
        Dim elevUnit As MeasurementUnit = BA_GetElevationUnitsForAOI(AOIFolderBase)
        Dim demInMeters As Boolean = False
        If elevUnit = MeasurementUnit.Meters Then
            demInMeters = True
        End If
        If useMeters = True Then
            'Determine if Display ZUnit is the same as DEM ZUnit
            'AOI_DEMMin and AOI_DEMMax use internal system unit, i.e., meters
            Dim Conversion_Factor As Double = BA_SetConversionFactor(True, demInMeters) 'i.e., meters to meters
            AOI_DEMMin = Math.Round(pRasterStats.Minimum * Conversion_Factor - 0.005, 2)
            AOI_DEMMax = Math.Round(pRasterStats.Maximum * Conversion_Factor + 0.005, 2)

            'Populate Boxes
            txtMinElev.Text = Math.Round(AOI_DEMMin * Conversion_Factor - 0.005, 2) 'adjust value to include the actual min, max
            TxtMaxElev.Text = Math.Round(AOI_DEMMax * Conversion_Factor + 0.005, 2)
            TxtRange.Text = Val(TxtMaxElev.Text) - Val(txtMinElev.Text)
        End If

    End Sub

    Private Sub BtnFindSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnFindSite.Click
        '1. Check to make sure npactual exists before going further; It's a required layer
        If Not BA_File_Exists(analysisFolder + "\" + BA_EnumDescription(MapsFileName.ActualRepresentedArea), WorkspaceType.Geodatabase, _
                              ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then

            MessageBox.Show("Unable to locate the represented area from the site scenario tool. Cannot generate pseudo site.", "Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If CkElev.Checked = True Then
            Dim success As BA_ReturnCode = GenerateElevationLayer()
        End If
    End Sub

    Private Sub CkElev_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkElev.CheckedChanged
        GrpElevation.Enabled = CkElev.Checked
    End Sub

    Private Sub CkPrecip_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkPrecip.CheckedChanged
        GrpPrecipitation.Enabled = CkPrecip.Checked
    End Sub

    Private Sub CkProximity_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkProximity.CheckedChanged
        GrpProximity.Enabled = CkProximity.Checked
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Function GenerateElevationLayer() As BA_ReturnCode
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(txtMinElev.Text + " " + txtLower.Text + " 1;")
        sb.Append(txtLower.Text + " " + TxtUpperRange.Text + " 2;")
        sb.Append(TxtUpperRange.Text + " " + TxtMaxElev.Text + " 3 ")
        Dim inputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) + BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim outputPath As String = analysisFolder & "\" & reclassElevFile
        Dim success As BA_ReturnCode = BA_ReclassifyRasterFromString(inputPath, BA_FIELD_VALUE, sb.ToString, _
                                                                     outputPath, Nothing)
        Return success
    End Function
End Class