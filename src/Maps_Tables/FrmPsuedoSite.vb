Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmPsuedoSite

    Private m_analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
    Private m_precipFolder As String
    Private m_precipFile As String
    Private m_elevLayer As String = "ps_elev"

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
        If Not BA_File_Exists(m_analysisFolder + "\" + BA_EnumDescription(MapsFileName.ActualRepresentedArea), WorkspaceType.Geodatabase, _
                              ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then

            MessageBox.Show("Unable to locate the represented area from the site scenario tool. Cannot generate pseudo site.", "Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        Dim maskPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & BA_BASIN_DEM_EXTENT_SHAPEFILE

        'Use this to hold the list of layers that we send to the union tool
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(m_analysisFolder + "\" + BA_EnumDescription(MapsFileName.ActualRepresentedArea) + "; ")

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Locating pseudo-site", "Locating...")
        progressDialog2.ShowDialog()

        Dim success As BA_ReturnCode = BA_ReturnCode.Success
        If CkElev.Checked = True Then
            success = GenerateElevationLayer(pStepProg, snapRasterPath)
            If success = BA_ReturnCode.Success Then
                sb.Append(m_analysisFolder + "\" + m_elevLayer + "; ")
            End If
        End If

        Dim unionFileName As String = "ps_union"
        If success = BA_ReturnCode.Success Then
            'union all our layers to prepare for Euclidean distance tool
            pStepProg.Message = "Union all participating layers"
            pStepProg.Step()
            sb.Remove(sb.ToString().LastIndexOf("; "), "; ".Length)
            success = BA_Union(sb.ToString, m_analysisFolder + "\" + unionFileName)
        End If

        Dim distanceFileName As String = "ps_distance"
        If success = BA_ReturnCode.Success Then
            'Run the Euclidean distance tool
            pStepProg.Message = "Executing Euclidean distance tool"
            pStepProg.Step()
            '@ToDo: Verify what cell size should be
            Dim cellSize As Double = BA_CellSize(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces), BA_EnumDescription(MapsFileName.filled_dem_gdb))
            success = BA_EuclideanDistance(m_analysisFolder + "\" + unionFileName, m_analysisFolder + "\" + distanceFileName, _
                                           CStr(cellSize), maskPath, snapRasterPath)
        End If


        If progressDialog2 IsNot Nothing Then
            progressDialog2.HideDialog()
        End If
        progressDialog2 = Nothing
        pStepProg = Nothing
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

    Private Function GenerateElevationLayer(ByVal pStepProg As IStepProgressor, ByVal snapRasterPath As String) As BA_ReturnCode
        '1. Reclass elevation raster according to upper and lower ranges
        pStepProg.Message = "Reclass DEM for elevation layer"
        pStepProg.Step()
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(txtMinElev.Text + " " + txtLower.Text + " 1;")
        sb.Append(txtLower.Text + " " + TxtUpperRange.Text + " 2;")
        sb.Append(TxtUpperRange.Text + " " + TxtMaxElev.Text + " 3 ")
        Dim inputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) + BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim reclassElevFile As String = "elevrecl"
        Dim reclassElevPath As String = m_analysisFolder & "\" & reclassElevFile
        Dim success As BA_ReturnCode = BA_ReclassifyRasterFromString(inputPath, BA_FIELD_VALUE, sb.ToString, _
                                                                     reclassElevPath, snapRasterPath)
        pStepProg.Message = "Convert elevation raster to feature class"
        pStepProg.Step()
        '2. Convert raster to polygon
        Dim reclassElevFc As String = "elevrecl_v"
        Dim reclassElevFcPath As String = m_analysisFolder & "\" & reclassElevFc
        If success = BA_ReturnCode.Success Then
            success = BA_Raster2Polygon_GP(reclassElevPath, reclassElevFcPath, snapRasterPath)
        End If

        '3. Dissolve on grid code
        If success = BA_ReturnCode.Success Then
            Dim elevLayerPath As String = m_analysisFolder & "\" & m_elevLayer
            success = BA_Dissolve(reclassElevFcPath, BA_FIELD_GRIDCODE, elevLayerPath)
        End If

        '4. Delete the polygon in the desired elevation range; This should always be #2
        If success = BA_ReturnCode.Success Then
            Dim selectQuery As String = " " + BA_FIELD_GRIDCODE + " = 2"
            success = BA_DeleteFeatures(m_analysisFolder, m_elevLayer, selectQuery)
        End If
        Return success
    End Function

    Private Sub txtLower_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtLower.Validating
        Dim sb As StringBuilder = New StringBuilder()
        Dim comps As Double
        Dim minElev As Double = CDbl(txtMinElev.Text)
        Dim upperElev As Double = CDbl(TxtUpperRange.Text)
        'tryparse fails, doesn't get into comps < 0 comparison
        If Double.TryParse(txtLower.Text, comps) Then
            If comps < minElev Then
                sb.Append("Value greater than minimum elevation is required!")
            ElseIf comps > upperElev Then
                sb.Append("Value less than upper desired range is required!")
            End If
        Else
            sb.Append("Numeric value required!")
        End If
        If sb.Length > 0 Then
            e.Cancel = True
            txtLower.Select(0, txtLower.Text.Length)
            MessageBox.Show(sb.ToString)
        End If
    End Sub

    Private Sub TxtUpperRange_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtUpperRange.Validating
        Dim sb As StringBuilder = New StringBuilder()
        Dim comps As Double
        Dim maxElev As Double = CDbl(TxtMaxElev.Text)
        Dim lowerRange As Double = CDbl(txtLower.Text)
        'tryparse fails, doesn't get into comps < 0 comparison
        If Double.TryParse(TxtUpperRange.Text, comps) Then
            If comps < lowerRange Then
                sb.Append("Value greater than the lower desired range is required!")
            ElseIf comps > maxElev Then
                sb.Append("Value less than maximum elevation is required!")
            End If
        Else
            sb.Append("Numeric value required!")
        End If
        If sb.Length > 0 Then
            e.Cancel = True
            TxtUpperRange.Select(0, TxtUpperRange.Text.Length)
            MessageBox.Show(sb.ToString)
        End If
    End Sub

    Private Sub CmdPrism_Click(sender As System.Object, e As System.EventArgs) Handles CmdPrism.Click
        m_precipFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True)
        If CmboxPrecipType.SelectedIndex = 0 Then  'read direct Annual PRISM raster
            m_precipFile = AOIPrismFolderNames.annual.ToString
        ElseIf CmboxPrecipType.SelectedIndex > 0 And CmboxPrecipType.SelectedIndex < 5 Then 'read directly Quarterly PRISM raster
            m_precipFile = BA_GetPrismFolderName(CmboxPrecipType.SelectedIndex + 12)
        Else 'sum individual monthly PRISM rasters
            Dim response As Integer = BA_PRISMCustom(My.Document, AOIFolderBase, Val(CmboxBegin.SelectedItem), Val(CmboxEnd.SelectedItem))
            If response = 0 Then
                MessageBox.Show("Unable to generate custom PRISM layer! Program stopped.")
                Exit Sub
            End If
            m_precipFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            m_precipFile = BA_TEMP_PRISM
        End If

        Dim raster_res As Double
        Dim pRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(m_precipFolder & m_precipFile, raster_res)

        'Populate Boxes
        txtMinPrecip.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
        txtMaxPrecip.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
        txtRangePrecip.Text = Val(txtMaxPrecip.Text) - Val(txtMinPrecip.Text)
    End Sub

    Private Sub CmboxPrecipType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxPrecipType.SelectedIndexChanged
        If CmboxPrecipType.SelectedIndex = 5 Then
            lblBeginMonth.Enabled = True
            CmboxBegin.Enabled = True
            lblEndMonth.Enabled = True
            CmboxEnd.Enabled = True
        Else
            lblBeginMonth.Enabled = False
            CmboxBegin.Enabled = False
            lblEndMonth.Enabled = False
            CmboxEnd.Enabled = False
        End If

        'reset the PRISM Dialog window
        txtMinPrecip.Text = "-"
        txtMaxPrecip.Text = "-"
        txtRangePrecip.Text = "-"
    End Sub
End Class