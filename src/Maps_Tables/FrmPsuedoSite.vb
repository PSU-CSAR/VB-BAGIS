Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase

Public Class FrmPsuedoSite

    Private m_analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
    Private m_representedArea As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
    Private m_precipFolder As String
    Private m_precipFile As String
    Private m_elevLayer As String = "ps_elev"
    Private m_siteFileName As String = "ps_site"
    Private m_proximityLayer As String = "ps_proximity"
    Private m_precipLayer As String = "ps_precip"
    Private m_locationLayer As String = "ps_location"
    Private m_demInMeters As Boolean    'Inherited from Site Scenario form; Controls elevation display/calculation
    Private m_usingElevMeters As Boolean    'Inherited from Site Scenario form; Controls elevation display/calculation
    Private m_usingXYUnits As esriUnits  'Inerited from Site Scenario form; Controls proximity display/calculation  
    Private m_aoiBoundary As String = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
    Private m_lastAnalysis As PseudoSite = Nothing
    Private m_formLoaded As Boolean = False
    Private m_cellSize As Double
    Private m_demMax As Double
    Private m_demMin As Double
    Private m_idxLayer As Int16 = 0
    Private m_idxValues As Int16 = 1
    Private m_idxFullPaths As Int16 = 2
    Private m_sep As String = ","
    'These 2 collections hold the values for the location layer(s) in memory; The key is the layer path which should be unique
    Private m_dictLocationAllValues As IDictionary(Of String, IList(Of String))
    Private m_dictLocationIncludeValues As IDictionary(Of String, IList(Of String))


    Public Sub New(ByVal demInMeters As Boolean, ByVal useMeters As Boolean, ByVal usingXYUnits As esriUnits, _
                   ByVal siteScenarioToolTimeStamp As DateTime)

        ' This call is required by the designer.
        InitializeComponent()

        'Populate class-level variables
        m_usingElevMeters = useMeters
        m_demInMeters = demInMeters
        m_usingXYUnits = usingXYUnits

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
        Dim inputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) + BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim pRasterStats As IRasterStatistics2 = BA_GetRasterStatsGDB(inputPath, m_cellSize)
        'Determine if Display ZUnit is the same as DEM ZUnit
        'AOI_DEMMin and AOI_DEMMax use internal system unit, i.e., meters
        Dim Conversion_Factor As Double = BA_SetConversionFactor(m_usingElevMeters, m_demInMeters) 'i.e., meters to meters
        'm_demMin = Math.Round(pRasterStats.Minimum * Conversion_Factor - 0.005, 2)
        'Cheat up so min is never outside of the actual range
        m_demMin = pRasterStats.Minimum * Conversion_Factor - 0.005
        m_demMax = pRasterStats.Maximum * Conversion_Factor + 0.005

        'Populate Boxes
        txtMinElev.Text = Convert.ToString(Math.Ceiling(m_demMin))
        TxtMaxElev.Text = Convert.ToString(Math.Floor(m_demMax))
        TxtRange.Text = Val(TxtMaxElev.Text) - Val(txtMinElev.Text)
        txtLower.Text = txtMinElev.Text
        TxtUpperRange.Text = TxtMaxElev.Text

        'Set DEM label; Default is meters when form loads
        If m_usingElevMeters = False Then
            lblElevation.Text = "DEM Elevation (Feet)"
            LblElevRange.Text = "Desired Range (Feet)"
        End If

        'Set proximity label; Default is meters when form loads
        Select Case m_usingXYUnits
            Case esriUnits.esriFeet
                LblBufferDistance.Text = "Buffer Distance (Feet):"
            Case esriUnits.esriKilometers
                LblBufferDistance.Text = "Buffer Distance (Km):"
            Case esriUnits.esriMiles
                LblBufferDistance.Text = "Buffer Distance (Miles):"
        End Select

        'Set label of form
        Me.Text = "Add Pseudo Site: " + BA_GetBareName(AOIFolderBase)

        SuggestSiteName()
        LoadLayers()
        BA_SetDefaultProjection(My.ArcMap.Application)

        'Only reload previous run if it completed successfully and ps_site exists
        If BA_File_Exists(m_analysisFolder + "\" + m_siteFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
        'Check for previously saved scenario and load those values as defaults
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.PseudoSiteXml)

        ' Open analysis file if there is one
        If BA_File_ExistsWindowsIO(xmlOutputPath) Then
            m_lastAnalysis = BA_LoadPseudoSiteFromXml(AOIFolderBase)
            ReloadLastAnalysis(siteScenarioToolTimeStamp)
            BtnMap.Enabled = True
            BtnFindSite.Enabled = False
        End If
        End If

        m_formLoaded = True
    End Sub

    Private Sub BtnFindSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnFindSite.Click
        '1. Check to make sure npactual exists before going further; It's a required layer
        If Not BA_File_Exists(m_analysisFolder + "\" + m_representedArea, WorkspaceType.Geodatabase, _
                              ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then

            MessageBox.Show("Unable to locate the Scenario 1 represented area. Calculate Scenario 1 using the Site Scenario tool and try again.", _
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Delete any layers from the previous run
        DeletePreviousRun()

        If CkElev.Checked Then
            'Validate lower and upper elevations
            Dim sbElev As StringBuilder = New StringBuilder()
            Dim comps As Double
            Dim minElev As Double = CDbl(txtMinElev.Text)
            Dim upperElev As Double = 99999
            If Not String.IsNullOrEmpty(TxtUpperRange.Text) Then
                Double.TryParse(TxtUpperRange.Text, upperElev)
            End If
            'tryparse fails, doesn't get into comps < 0 comparison
            If Double.TryParse(txtLower.Text, comps) Then
                If comps < minElev Then
                    sbElev.Append("Desired range lower: Value greater than minimum elevation is required!" + vbCrLf)
                ElseIf comps > upperElev Then
                    sbElev.Append("Desired range lower: Value less than upper desired range is required!" + vbCrLf)
                End If
            Else
                sbElev.Append("Desired range lower: Numeric value required!" + vbCrLf)
            End If
            Dim maxElev As Double = CDbl(TxtMaxElev.Text)
            Dim lowerRange As Double = 0
            If Not String.IsNullOrEmpty(txtLower.Text) Then
                Double.TryParse(txtLower.Text, lowerRange)
            End If
            'tryparse fails, doesn't get into comps < 0 comparison
            If Double.TryParse(TxtUpperRange.Text, comps) Then
                If comps < lowerRange Then
                    sbElev.Append("Desired range upper: Value greater than the lower desired range is required!" + vbCrLf)
                ElseIf comps > maxElev Then
                    sbElev.Append("Desired range upper: Value less than maximum elevation is required!" + vbCrLf)
                End If
            Else
                sbElev.Append("Desired range upper: Numeric value required!" + vbCrLf)
            End If

            If sbElev.Length > 0 Then
                Dim errMsg As String = "You selected the Elevation option but one or more of the parameters are invalid: " + vbCrLf + vbCrLf + _
                    sbElev.ToString + vbCrLf +
                    "Click 'No' to fix the parameters, or 'Yes' to find a site without using the Elevation option"
                Dim res As DialogResult = MessageBox.Show(errMsg, "Invalid elevation values", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                If res <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    CkElev.Checked = False
                End If
            End If
        End If

        'If user selected proximity layer, did they choose a layer?
        If CkProximity.Checked Then
            If LstVectors.SelectedItem Is Nothing Then
                Dim res As DialogResult = MessageBox.Show("You selected the Proximity option but failed to select a layer. Do you wish to " + _
                                                          "find a site without using the Proximity option", "Missing layer", MessageBoxButtons.YesNo, _
                                                          MessageBoxIcon.Question)
                If res <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    CkProximity.Checked = False
                End If
            End If
            'If proximity still selected, validate buffer distance
            If CkProximity.Checked Then
                Dim errorMsg As String = ValidBufferDistance()
                If Not String.IsNullOrEmpty(errorMsg) Then
                    txtBufferDistance.Select(0, txtBufferDistance.Text.Length)
                    Dim errorMsg2 As String = "You selected the Proximity option but one or more of the parameters are invalid: " + vbCrLf + vbCrLf + _
                                              errorMsg + vbCrLf +
                                             "Click 'No' to fix the parameter, or 'Yes' to find a site without using the Proximity option ?"
                    Dim res As DialogResult = MessageBox.Show(errorMsg2, "Invalid proximity buffer", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                    If res <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        CkProximity.Checked = False
                    End If
                End If
            End If
        End If

        'If user selected PRISM layer, did they enter a valid range?
        If CkPrecip.Checked Then
            Dim sbPrism As StringBuilder = New StringBuilder()
            Dim comps As Double
            If Not Double.TryParse(txtMinPrecip.Text, comps) Then
                Dim errMsg As String = "You selected the Precipitation option but have not configured a valid range." + vbCrLf + vbCrLf + _
                    "Click 'No' to add the desired range, or 'Yes' to find a site without using the Precipitation option."
                Dim res As DialogResult = MessageBox.Show(errMsg, "Invalid precipitation values", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                If res <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    CkPrecip.Checked = False
                End If
            End If
            'Is Precip option still selected?
            If CkPrecip.Checked Then
                Dim minPrecip As Double = CDbl(txtMinPrecip.Text)
                Dim upperPrecip As Double = 99999
                If Not String.IsNullOrEmpty(TxtPrecipUpper.Text) Then
                    Double.TryParse(TxtPrecipUpper.Text, upperPrecip)
                End If
                'tryparse fails, doesn't get into comps < 0 comparison
                If Double.TryParse(TxtPrecipLower.Text, comps) Then
                    If comps < minPrecip Then
                        sbPrism.Append("Desired range lower: Value greater than minimum precipitation is required!" + vbCrLf)
                    ElseIf comps > upperPrecip Then
                        sbPrism.Append("Desired range lower: Value less than upper desired range is required!" + vbCrLf)
                    End If
                Else
                    sbPrism.Append("Desired range lower: Numeric value required!" + vbCrLf)
                End If
                Dim maxPrecip As Double = CDbl(txtMaxPrecip.Text)
                Dim lowerRange As Double = comps
                'tryparse fails, doesn't get into comps < 0 comparison
                If Double.TryParse(TxtPrecipUpper.Text, comps) Then
                    If comps < lowerRange Then
                        sbPrism.Append("Desired range upper: Value greater than the lower desired range is required!" + vbCrLf)
                    ElseIf comps > maxPrecip Then
                        sbPrism.Append("Desired range upper: Value less than maximum precipitation is required!" + vbCrLf)
                    End If
                Else
                    sbPrism.Append("Desired range upper: Numeric value required!" + vbCrLf)
                End If
                If sbPrism.Length > 0 Then
                    Dim errMsg As String = "You selected the Precipitation option but one or more of the parameters are invalid: " + vbCrLf + vbCrLf + _
                        sbPrism.ToString + vbCrLf +
                        "Click 'No' to fix the parameters, or 'Yes' to find a site without using the Precipitation option."
                    Dim res As DialogResult = MessageBox.Show(errMsg, "Invalid precipitation values", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                    If res <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        CkPrecip.Checked = False
                    End If
                End If
            End If
        End If

        'User selected location layer; Are required fields populated?
        If CkLocation.Checked Then
            Dim sbLoc As StringBuilder = New StringBuilder()
            If GrdLocation.Rows.Count = 0 Then
                sbLoc.Append("No layers have been configured")
            End If
            For Each row As DataGridViewRow In GrdLocation.Rows
                Dim layerName As String = Convert.ToString(row.Cells(m_idxLayer).Value)
                Dim layerLocation As String = Convert.ToString(row.Cells(m_idxFullPaths).Value)
                If row.Cells(m_idxFullPaths).Value Is Nothing Then
                    sbLoc.Append("Missing layer path for layer " + layerName + vbCrLf)
                End If
                If row.Cells(m_idxValues).Value Is Nothing Then
                    sbLoc.Append("Missing selected values for layer " + layerName + vbCrLf)
                Else
                    Dim lstValues As IList(Of String) = m_dictLocationIncludeValues(layerLocation)
                    If lstValues.Count < 1 Then
                        sbLoc.Append("Missing selected values for layer " + layerName + vbCrLf)
                    End If
                End If
            Next
            If sbLoc.Length > 0 Then
                Dim errMsg As String = "You selected the Location option but one or more of the parameters are invalid: " + vbCrLf + vbCrLf + _
                    sbLoc.ToString + vbCrLf +
                    "Click 'No' to fix the parameters, or 'Yes' to find a site without using the Location option."
                Dim res As DialogResult = MessageBox.Show(errMsg, "Invalid location values", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                If res <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    CkLocation.Checked = False
                End If
            End If
        End If

        Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        Dim maskPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & m_aoiBoundary

        BtnFindSite.Enabled = False

        'Use this to hold the list of layers that we send to the cell statistics tool
        Dim sb As StringBuilder = New StringBuilder()

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Locating pseudo-site", "Locating...")
        progressDialog2.ShowDialog()

        Dim success As BA_ReturnCode = BA_ReturnCode.Success

        '2. Identify cells that are furthest from the represented area (Euclidean distance tool)
        Dim distanceFileName As String = "ps_distance"
        Dim furthestPixelInputFile As String = distanceFileName
        If success = BA_ReturnCode.Success Then
            pStepProg.Message = "Executing Euclidean distance tool"
            pStepProg.Step()
            '@ToDo: Verify what cell size should be; Currently comes from filled_dem
            success = BA_EuclideanDistance(m_analysisFolder + "\" + m_representedArea, m_analysisFolder + "\" + distanceFileName, _
                                           CStr(m_cellSize), maskPath, snapRasterPath, maskPath)
        End If
        If CkElev.Checked = True Then
            success = GenerateElevationLayer(pStepProg, snapRasterPath)
            If success = BA_ReturnCode.Success Then
                sb.Append(m_analysisFolder + "\" + m_elevLayer + "; ")
            End If
        End If

        If CkPrecip.Checked = True Then
            success = GeneratePrecipitationLayer(pStepProg, snapRasterPath)
            If success = BA_ReturnCode.Success Then
                sb.Append(m_analysisFolder + "\" + m_precipLayer + "; ")
            End If
        End If

        If CkProximity.Checked = True Then
            Dim errorMsg As String = ValidBufferDistance()
            If Not String.IsNullOrEmpty(errorMsg) Then
                txtBufferDistance.Select(0, txtBufferDistance.Text.Length)
                errorMsg = errorMsg + " The proximity layer will not be used in analysis."
                MessageBox.Show(errorMsg)
            Else
                success = GenerateProximityLayer(pStepProg, snapRasterPath)
                If success = BA_ReturnCode.Success Then
                    sb.Append(m_analysisFolder + "\" + m_proximityLayer + "; ")
                End If
            End If
        End If

        If CkLocation.Checked = True Then
            success = GenerateLocationLayer(pStepProg, snapRasterPath)
            If success = BA_ReturnCode.Success Then
                sb.Append(m_analysisFolder + "\" + m_locationLayer + "; ")
            End If
        End If

        Dim cellStatFileName As String = "ps_cellStat"
        Dim timesFileName As String = "ps_times"
        If sb.Length > 0 Then
            If success = BA_ReturnCode.Success Then
                '6. Get minimum for all of the constraint layers
                pStepProg.Message = "Calculating cell statistics for all constraint layers"
                pStepProg.Step()
                sb.Remove(sb.ToString().LastIndexOf("; "), "; ".Length)
                success = BA_GetCellStatistics(sb.ToString, snapRasterPath, "MINIMUM", _
                                               m_analysisFolder + "\" + cellStatFileName, "false")
            End If

            If BA_IsRasterEmpty(m_analysisFolder, cellStatFileName) Then
                Dim errMsg As String = "The entire area of the AOI was excluded using the constraints you selected. " +
                    "No suitable site location could be found. "
                MessageBox.Show(errMsg, "No site location found", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                If progressDialog2 IsNot Nothing Then
                    progressDialog2.HideDialog()
                End If
                BtnFindSite.Enabled = True
                Exit Sub
            End If

            furthestPixelInputFile = timesFileName
            If success = BA_ReturnCode.Success Then
                pStepProg.Message = "Executing Times tool with distance and cell statistics layers"
                pStepProg.Step()
                success = BA_Times(m_analysisFolder + "\" + distanceFileName, m_analysisFolder + "\" + cellStatFileName, _
                    m_analysisFolder + "\" + timesFileName)
            End If
        End If

        Dim furthestPixelFileName As String = "ps_furthest"
        If success = BA_ReturnCode.Success Then
            pStepProg.Message = "Finding furthest pixel"
            pStepProg.Step()
            'Get the maximum pixel value
            'Set everything to null that is smaller than that; should leave one pixel
            'Expression can be more precise; Rounding down works for now
            Dim cellSize As Double = -1
            Dim pRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(m_analysisFolder + "\" + furthestPixelInputFile, cellSize)
            Dim pExpression As String = Nothing
            If pRasterStats IsNot Nothing Then
                'sample expression: SetNull('C:\Docs\Lesley\animas_AOI_prms_3\analysis.gdb\ps_distance' < 6259,'C:\Docs\Lesley\animas_AOI_prms_3\analysis.gdb\ps_distance')
                Dim targetPath As String = m_analysisFolder + "\" + furthestPixelInputFile
                pExpression = "SetNull('" + targetPath + "' < " + _
                    CStr(Math.Floor(pRasterStats.Maximum)) + _
                    ",'" + targetPath + "')"
                success = BA_RasterCalculator(m_analysisFolder + "\" + furthestPixelFileName, pExpression, _
                                              snapRasterPath, maskPath)
            End If
        End If

        If success = BA_ReturnCode.Success Then
            success = BA_RasterToPoint(m_analysisFolder + "\" + furthestPixelFileName, _
                                       m_analysisFolder + "\" + m_siteFileName, BA_FIELD_VALUE)
        End If

        Dim siteObjectId As Integer = -1
        If success = BA_ReturnCode.Success Then
            Dim numSites As Int16 = BA_CountPolygons(m_analysisFolder, m_siteFileName, BA_FIELD_GRIDCODE_GDB)
            If numSites < 1 Then
                MessageBox.Show("No psuedo-sites were found. Please double-check your selection criteria")
            ElseIf numSites > 1 Then
                MessageBox.Show(numSites & " pseudo-sites were found. Currently BAGIS only knows how to deal with one, so it will randomly pick one.")
                Dim r As New Random()
                Dim keepSite As Integer = r.Next(1, numSites + 1)
                'Delete all sites except the first one
                Dim strSelect As String = " " & BA_FIELD_OBJECT_ID & " <> " & keepSite
                success = BA_DeleteFeatures(m_analysisFolder, m_siteFileName, strSelect)
            End If

            'Create new psuedo_sites file or append auto-site to existing
            pStepProg.Message = "Integrating new pseudo-site into site selection layers"
            pStepProg.Step()
            Dim newSite As Site = PreparePointFileToAppend(snapRasterPath)
            If newSite IsNot Nothing Then
                Dim pseudoPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) + BA_EnumDescription(MapsFileName.Pseudo)
                If BA_File_Exists(pseudoPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    success = BA_AppendFeatures(m_analysisFolder + "\" + m_siteFileName, pseudoPath)
                Else
                    success = BA_CopyFeatures(m_analysisFolder + "\" + m_siteFileName, pseudoPath)
                End If

                If success = BA_ReturnCode.Success Then
                    'Query the OID of the new site
                    siteObjectId = GetNewSiteObjectId(newSite.Elevation)
                    If siteObjectId > 0 Then
                        newSite.ObjectId = siteObjectId
                        'Adds the sites to 'existing sites' on the form
                        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteScenario.AddinImpl)(My.ThisAddIn.IDs.frmSiteScenario)
                        Dim siteScenarioForm As frmSiteScenario = dockWindowAddIn.UI
                        siteScenarioForm.AddNewPseudoSite(newSite)

                        'Set the global variable for pseudo-sites to true
                        AOI_HasPseudoSite = True
                    Else
                        MessageBox.Show("Unable to add psuedo-site to Site Scenario Tool. Reload Site Scenario Tool")
                    End If
                End If
            Else
                MessageBox.Show("An error occurred while trying to process the new pseudo-site layer!")
            End If

            'Delete the layers we don't need to keep for the map
            BA_RemoveRasterFromGDB(m_analysisFolder, distanceFileName)
            If BA_File_Exists(m_analysisFolder + "\" + cellStatFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(m_analysisFolder, cellStatFileName)
            End If
            If BA_File_Exists(m_analysisFolder + "\" + timesFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(m_analysisFolder, timesFileName)
            End If
            BA_RemoveRasterFromGDB(m_analysisFolder, furthestPixelFileName)
        End If

        If success = BA_ReturnCode.Success Then
            pStepProg.Message = "Saving pseudo-site log"
            pStepProg.Step()
            SavePseudoSiteLog(siteObjectId)
        End If

        If progressDialog2 IsNot Nothing Then
            progressDialog2.HideDialog()
        End If
        MessageBox.Show("The new pseudo-site has been added to Scenario 1 in the Site Scenario Tool")
        BtnMap.Enabled = True
        progressDialog2 = Nothing
        pStepProg = Nothing
    End Sub

    Private Sub CkElev_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkElev.CheckedChanged
        GrpElevation.Enabled = CkElev.Checked
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub CkPrecip_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkPrecip.CheckedChanged
        GrpPrecipitation.Enabled = CkPrecip.Checked
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub CkProximity_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkProximity.CheckedChanged
        GrpProximity.Enabled = CkProximity.Checked
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Function GenerateElevationLayer(ByVal pStepProg As IStepProgressor, ByVal snapRasterPath As String) As BA_ReturnCode
        '1. Reclass elevation raster according to upper and lower ranges
        pStepProg.Message = "Reclass DEM for elevation layer"
        pStepProg.Step()
        Dim sb As StringBuilder = New StringBuilder()
        'Set min/max of reclass to actual dem values
        Dim strMinElev As String = Convert.ToString(m_demMin)
        Dim strLower As String = txtLower.Text
        Dim strUpperRange As String = TxtUpperRange.Text
        Dim strMaxElev As String = Convert.ToString(m_demMax)
        'Convert the values to the DEM value, before composing the reclass string, if we need to
        If m_demInMeters <> m_usingElevMeters Then
            Dim converter As IUnitConverter = New UnitConverter
            Dim toElevUnits As esriUnits = esriUnits.esriMeters
            If Not m_demInMeters Then _
                toElevUnits = esriUnits.esriFeet
            Dim fromElevUnits As esriUnits = esriUnits.esriFeet
            If m_usingElevMeters Then _
                fromElevUnits = esriUnits.esriMeters
            strMinElev = Convert.ToString(Math.Round(converter.ConvertUnits(m_demMin, fromElevUnits, toElevUnits)))
            strLower = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(txtLower.Text), fromElevUnits, toElevUnits)))
            strUpperRange = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(TxtUpperRange.Text), fromElevUnits, toElevUnits)))
            strMaxElev = Convert.ToString(Math.Round(converter.ConvertUnits(m_demMax, fromElevUnits, toElevUnits)))
        End If
        sb.Append(strMinElev + " " + strLower + " NoData;")
        sb.Append(strLower + " " + strUpperRange + " 1;")
        sb.Append(strUpperRange + " " + strMaxElev + " NoData")
        Dim inputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) + BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim reclassElevPath As String = m_analysisFolder & "\" & m_elevLayer
        Dim success As BA_ReturnCode = BA_ReclassifyRasterFromString(inputPath, BA_FIELD_VALUE, sb.ToString, _
                                                                     reclassElevPath, snapRasterPath)
        'Add 'NAME' field to be used as label for map
        If success = BA_ReturnCode.Success Then
            success = BA_AddUserFieldToRaster(m_analysisFolder, m_elevLayer, BA_FIELD_NAME, esriFieldType.esriFieldTypeString, _
                                          100, BA_MAPS_PS_ELEVATION)
        End If
        Return success
    End Function

    Private Function GeneratePrecipitationLayer(ByVal pStepProg As IStepProgressor, ByVal snapRasterPath As String) As BA_ReturnCode
        '1. Reclass precip raster according to upper and lower ranges
        pStepProg.Message = "Reclass precipitation layer"
        pStepProg.Step()
        Dim sb As StringBuilder = New StringBuilder()
        Dim strMinPrecip As String = txtMinPrecip.Text
        Dim strLowerRange As String = TxtPrecipLower.Text
        Dim strUpperRange As String = TxtPrecipUpper.Text
        Dim strMaxPrecip As String = txtMaxPrecip.Text
        sb.Append(strMinPrecip + " " + strLowerRange + " NoData;")
        sb.Append(strLowerRange + " " + strUpperRange + " 1;")
        sb.Append(strUpperRange + " " + strMaxPrecip + " NoData")

        Dim inputFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True)
        Dim prismRasterName As String
        If CmboxPrecipType.SelectedIndex = 0 Then
            prismRasterName = AOIPrismFolderNames.annual.ToString    'read direct Annual PRISM raster
        ElseIf CmboxPrecipType.SelectedIndex > 0 And CmboxPrecipType.SelectedIndex < 5 Then 'read directly Quarterly PRISM raster
            prismRasterName = BA_GetPrismFolderName(CmboxPrecipType.SelectedIndex + 12)
        Else 'sum individual monthly PRISM rasters
            Dim response As Integer = BA_PRISMCustom(My.Document, AOIFolderBase, Val(CmboxBegin.SelectedItem), Val(CmboxEnd.SelectedItem))
            If response = 0 Then
                MsgBox("Unable to generate custom PRISM layer! Program stopped.")
                Return BA_ReturnCode.UnknownError
            End If
            inputFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            prismRasterName = BA_TEMP_PRISM
        End If
        Dim inputPath As String = inputFolder + prismRasterName
        Dim reclassPrismPath As String = m_analysisFolder & "\" & m_precipLayer
        Dim success As BA_ReturnCode = BA_ReclassifyRasterFromString(inputPath, BA_FIELD_VALUE, sb.ToString, _
                                                                             reclassPrismPath, snapRasterPath)
        'Add 'NAME' field to be used as label for map
        If success = BA_ReturnCode.Success Then
            success = BA_AddUserFieldToRaster(m_analysisFolder, m_precipLayer, BA_FIELD_NAME, esriFieldType.esriFieldTypeString, _
                                          100, BA_MAPS_PS_PRECIPITATION)
        End If
        Return success
    End Function

    Private Function GenerateLocationLayer(ByVal pStepProg As IStepProgressor, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim layerCount As Int16 = GrdLocation.Rows.Count
        Dim success As BA_ReturnCode
        Dim outputFolderPath As String = m_analysisFolder + "\" + m_locationLayer
        Dim inRasterPath2 As String = Nothing
        Dim timesOutputFolderPath As String = Nothing
        Dim lstRastersToDelete As IList(Of String) = New List(Of String)
        For i As Int16 = 0 To layerCount - 1
            pStepProg.Message = "Processing location layer " + Convert.ToString(GrdLocation.Rows(i).Cells(m_idxLayer).Value)
            pStepProg.Step()
            'Build reclassItem array
            Dim layerLocation As String = Convert.ToString(GrdLocation.Rows(i).Cells(m_idxFullPaths).Value)
            Dim lstAllValues As IList(Of String) = m_dictLocationAllValues(layerLocation)
            Dim lstIncludeValues As IList(Of String) = m_dictLocationIncludeValues(layerLocation)
            Dim reclassItems(lstAllValues.Count - 1) As ReclassItem
            For j As Integer = 0 To lstAllValues.Count - 1
                Dim nextItem As ReclassItem = New ReclassItem()
                Dim pValue As String = lstAllValues(j)
                nextItem.FromValue = pValue
                nextItem.ToValue = pValue
                If lstIncludeValues.Contains(pValue) Then
                    nextItem.OutputValue = 1
                Else
                    nextItem.OutputValue = -9999
                End If
                reclassItems(j) = nextItem
            Next
            If layerCount > 1 Then
                outputFolderPath = m_analysisFolder + "\tempLocation" + CStr(i)
                lstRastersToDelete.Add(outputFolderPath)
                timesOutputFolderPath = m_analysisFolder + "\timesLocation" + CStr(i)
            End If
            success = BA_ReclassifyRasterFromTableWithNoData(layerLocation, BA_FIELD_VALUE, reclassItems, _
                                                             outputFolderPath, snapRasterPath)
            If success = BA_ReturnCode.Success AndAlso i > 0 Then
                'inRasterPath1 always outputFolderPath
                'inRasterPath2 see case statement below
                'outRasterPath always timesOutputFolderPath
                Select Case i
                    Case 1
                        'Multiplying first 2 reclass layers
                        inRasterPath2 = m_analysisFolder + "\tempLocation" + CStr(i - 1)
                    Case Is > 1
                        'Multiplying by previous times output layer
                        inRasterPath2 = m_analysisFolder + "\timesLocation" + CStr(i - 1)
                End Select
                success = BA_Times(outputFolderPath, inRasterPath2, timesOutputFolderPath)
                lstRastersToDelete.Add(timesOutputFolderPath)
            End If
            ' Stop processing if there is an error
            If success <> BA_ReturnCode.Success Then
                Return success
            End If
        Next
        ' Need to rename the final layer to the location output layer
        If layerCount > 1 Then
            success = BA_RenameRasterInGDB(m_analysisFolder, timesOutputFolderPath, m_locationLayer)
        End If
        If success = BA_ReturnCode.Success Then
            For Each layerPath As String In lstRastersToDelete
                Dim layerFolder As String = "PleaseReturn"
                Dim layerName As String = BA_GetBareName(layerPath, layerFolder)
                Dim retVal As Short = BA_RemoveRasterFromGDB(layerFolder, layerName)
            Next
        End If
        '@ToDo: Add the layer name to the raster for display
        Return success
    End Function

    Private Sub txtLower_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtLower.Validating
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub TxtUpperRange_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtUpperRange.Validating
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub CmdPrism_Click(sender As System.Object, e As System.EventArgs) Handles CmdPrism.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating PRISM precipitation values", "Calculating...")
        progressDialog2.ShowDialog()

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
        TxtPrecipLower.Text = txtMinPrecip.Text
        TxtPrecipUpper.Text = txtMaxPrecip.Text

        If progressDialog2 IsNot Nothing Then
            progressDialog2.HideDialog()
        End If
        progressDialog2 = Nothing
        pStepProg = Nothing

    End Sub

    Private Sub CmboxPrecipType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxPrecipType.SelectedIndexChanged
        RaiseEvent FormInputChanged()
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

    Private Sub BtnMap_Click(sender As System.Object, e As System.EventArgs) Handles BtnMap.Click
        'Ensure default map frame name is set before trying to build map
        Dim response As Integer = BA_SetDefaultMapFrameName(BA_MAPS_DEFAULT_MAP_NAME, My.Document)
        response = BA_SetMapFrameDimension(BA_MAPS_DEFAULT_MAP_NAME, 1, 2, 7.5, 9, True)
        BA_RemoveAutoSiteLayersfromMapFrame(My.Document)
        AddLayersToMapFrame(My.ThisApplication, My.Document)
        Dim Basin_Name As String
        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = cboSelectedBasin.getValue
        End If
        Dim aoiName As String = BA_GetBareName(AOIFolderBase)
        Dim mapTitle As String = aoiName & Basin_Name
        BA_AddMapElements(My.Document, mapTitle, "Subtitle BAGIS")

        'Toggle the layers we want to see
        Dim LayerNames(10) As String
        LayerNames(1) = BA_MAPS_PS_REPRESENTED
        LayerNames(2) = BA_EnumDescription(MapsLayerName.NewPseudoSite)
        LayerNames(3) = BA_MAPS_PS_INDICATOR
        LayerNames(4) = BA_MAPS_AOI_BASEMAP
        LayerNames(5) = BA_MAPS_PS_PROXIMITY
        LayerNames(6) = BA_MAPS_PS_ELEVATION
        LayerNames(7) = BA_MAPS_PS_PRECIPITATION
        LayerNames(8) = BA_MAPS_PS_LOCATION
        LayerNames(9) = BA_MAPS_HILLSHADE
        response = BA_ToggleLayersinMapFrame(My.Document, LayerNames)

        BA_RemoveLayersfromLegend(My.Document)
        'Note: these functions are called in BA_DisplayMap if we end up adding buttons
        Dim UnitText As String = Nothing    'Textbox above scale bar
        Dim subtitle As String = "PROPOSED PSEUDO SITE LOCATION"
        BA_MapUpdateSubTitle(My.Document, mapTitle, subtitle, UnitText)
        Dim keyLayerName As String = Nothing
        BA_SetLegendFormat(My.Document, keyLayerName)

        'Clip data frame to aoi border
        ClipDataFrameToAoiBorder()

        MessageBox.Show("Use ArcMap Table of Contents to view map.", "Map", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AddLayersToMapFrame(ByVal pApplication As ESRI.ArcGIS.Framework.IApplication, _
                                    ByVal pMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument)
        Dim pColor As IColor = New RgbColor
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim retVal As Integer = -1

        Try
            'Scenario 1 Represented area
            Dim filepathname As String = m_analysisFolder & "\" & m_representedArea
            If Not BA_File_Exists(filepathname, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                MessageBox.Show("Unable to locate the represented area from the site scenario tool. Cannot load map.", "Error", _
                     MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            pColor.RGB = RGB(255, 0, 0) 'red
            success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_REPRESENTED, pColor, 30)

            'add pseudo site
            filepathname = m_analysisFolder & "\" & m_siteFileName
            If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                pColor.RGB = RGB(169, 0, 230)    'Purple
                success = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.NewPseudoSite, pColor, MapsMarkerType.PseudoSite)
            End If

            'draw circle around pseudo site
            Dim siteLayerName As String = BA_EnumDescription(MapsLayerName.NewPseudoSite)
            Dim tempLayer As ILayer
            Dim pseudoSrc As IFeatureLayer = Nothing
            'Reset layer count in case layers were removed
            Dim nlayers As Int16 = pMxDoc.FocusMap.LayerCount

            For i = nlayers To 1 Step -1
                tempLayer = CType(pMxDoc.FocusMap.Layer(i - 1), ILayer)   'Explicit cast
                If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = siteLayerName Then
                    pseudoSrc = CType(tempLayer, IFeatureLayer)
                    Exit For
                End If
            Next

            Dim pActualColor As IColor = New RgbColor
            pActualColor.RGB = RGB(169, 0, 230)    'Purple
            Dim actualRenderer As ISimpleRenderer = BA_BuildRendererForPoints(pActualColor, 25)

            If pseudoSrc IsNot Nothing Then
                Dim pFSele As IFeatureSelection = TryCast(pseudoSrc, IFeatureSelection)
                Dim pQFilter As IQueryFilter = New QueryFilter
                pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                Dim fLayerDef As IFeatureLayerDefinition = CType(pseudoSrc, IFeatureLayerDefinition)
                Dim pseudoCopy As IFeatureLayer = fLayerDef.CreateSelectionLayer(BA_MAPS_PS_INDICATOR, True, Nothing, Nothing)
                Dim pGFLayer As IGeoFeatureLayer = CType(pseudoCopy, IGeoFeatureLayer)
                pGFLayer.Renderer = actualRenderer
                My.Document.FocusMap.AddLayer(pGFLayer)
                pFSele.Clear()
            End If

            'add aoib as base layer for difference of representation maps
            filepathname = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & BA_BufferedAOIExtentRaster
            retVal = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_AOI_BASEMAP, _
                                                MapsDisplayStyle.Cyan_Light_to_Blue_Dark, 30, WorkspaceType.Geodatabase)

            'Proximity if it exists
            If m_lastAnalysis.UseProximity Then
                filepathname = m_analysisFolder & "\" & m_proximityLayer
                If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    retVal = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_PS_PROXIMITY, _
                                                        MapsDisplayStyle.Yellows, 30, WorkspaceType.Geodatabase)
                End If
            End If

            'Elevation if it exists
            filepathname = m_analysisFolder & "\" & m_elevLayer
            If m_lastAnalysis.UseElevation Then
                If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    retVal = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_PS_ELEVATION, _
                                   MapsDisplayStyle.Slope, 30, WorkspaceType.Geodatabase)
                End If
            End If

            'Precipitation if used
            filepathname = m_analysisFolder & "\" & m_precipLayer
            If m_lastAnalysis.UsePrism Then
                If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    retVal = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_PS_PRECIPITATION, _
                                MapsDisplayStyle.Purple_Blues, 30, WorkspaceType.Geodatabase)
                End If
            End If

            'add hillshade
            filepathname = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & _
                BA_GetBareName(BA_EnumDescription(PublicPath.Hillshade))
            retVal = BA_MapDisplayRaster(pMxDoc, filepathname, BA_MAPS_HILLSHADE, 0)
            'Move hillshade to bottom
            Dim layerCount As Int16 = My.Document.FocusMap.LayerCount
            Dim idxHillshade As Integer = BA_GetLayerIndexByName(My.Document, BA_MAPS_HILLSHADE)
            Dim hLayer As ILayer = My.Document.FocusMap.Layer(idxHillshade)
            My.Document.FocusMap.MoveLayer(hLayer, layerCount)

            'zoom to the aoi boundary layer
            BA_ZoomToAOI(pMxDoc, AOIFolderBase)

        Catch ex As Exception
            Debug.Print("AddLayersToMapFrame Exception: " & ex.Message)
            MessageBox.Show("An error occurred while trying to load the map!")
        Finally

        End Try

    End Sub

    Private Sub LoadLayers()
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)

        'display feature layers
        Dim FeatureClassCount As Integer = UBound(AOIVectorList)
        If FeatureClassCount > 0 Then
            For i = 1 To FeatureClassCount
                Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, True)
                LstVectors.Items.Add(item)
            Next
        End If

        'display raster layers
        Dim rasterCount As Integer = UBound(AOIRasterList)
        If rasterCount > 0 Then
            For i = 1 To rasterCount
                Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                If BA_IsIntegerRasterGDB(fullLayerPath) Then
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, True)
                    LstRasters.Items.Add(item)
                End If
            Next
        End If

        'display zonal layers
        Dim lstZoneLayers As IList(Of String) = New List(Of String)
        lstZoneLayers.Add(BA_RasterElevationZones)
        lstZoneLayers.Add(BA_RasterPrecipitationZones)
        lstZoneLayers.Add(BA_RasterSlopeZones)
        lstZoneLayers.Add(BA_RasterAspectZones)
        lstZoneLayers.Add(BA_RasterSNOTELZones)
        lstZoneLayers.Add(BA_RasterSnowCourseZones)
        For Each zoneLayer As String In lstZoneLayers
            Dim zonePath As String = m_analysisFolder + "\" + zoneLayer
            If BA_File_Exists(zonePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                Dim item As LayerListItem = New LayerListItem(zoneLayer, zonePath, LayerType.Raster, True)
                LstRasters.Items.Add(item)
            End If
        Next

    End Sub

    Private Function GenerateProximityLayer(ByVal pStepProg As IStepProgressor, ByVal snapRasterPath As String) As BA_ReturnCode
        pStepProg.Message = "Generating proximity layer"
        pStepProg.Step()

        '--- Calculate correct buffer distance based on XY units ---
        Dim comps As Double = -1
        Dim bufferDistance As Double = 0
        Dim isNumber As Boolean = Double.TryParse(txtBufferDistance.Text, comps)
        If isNumber Then
            bufferDistance = comps
        End If
        Dim strBuffer As String = Convert.ToString(bufferDistance) + " "
        Select Case m_usingXYUnits
            Case esriUnits.esriFeet
                strBuffer = strBuffer + MeasurementUnit.Feet.ToString
            Case esriUnits.esriKilometers
                strBuffer = strBuffer + MeasurementUnit.Kilometers.ToString
            Case esriUnits.esriMiles
                strBuffer = strBuffer + MeasurementUnit.Miles.ToString
            Case Else
                strBuffer = strBuffer + MeasurementUnit.Meters.ToString
        End Select

        Dim item As LayerListItem = LstVectors.SelectedItem
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim tempProximity As String = "ps_prox_v"
        Dim outFeaturesPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + tempProximity
        If item IsNot Nothing Then
            success = BA_Buffer(item.Value, outFeaturesPath, strBuffer, "ALL")
        End If
        If success = BA_ReturnCode.Success Then
            success = BA_AddUserFieldToVector(m_analysisFolder, tempProximity, BA_FIELD_PSITE, esriFieldType.esriFieldTypeInteger, _
                                              -1, "1")
        End If
        If success = BA_ReturnCode.Success Then
            success = BA_Feature2RasterGP(outFeaturesPath, m_analysisFolder + "\" + m_proximityLayer, BA_FIELD_PSITE, m_cellSize, snapRasterPath)
            BA_Remove_ShapefileFromGDB(m_analysisFolder, tempProximity)
        End If
        If success = BA_ReturnCode.Success Then
            'Add 'NAME' field to be used as label for map
            success = BA_AddUserFieldToRaster(m_analysisFolder, m_proximityLayer, BA_FIELD_NAME, esriFieldType.esriFieldTypeString, _
                                          100, BA_MAPS_PS_PROXIMITY)
        End If

        If Not success = BA_ReturnCode.Success Then
            MessageBox.Show("An error occurred while generating the proximity layer. It will not be used in analysis")
        End If
        Return success
    End Function

    Private Sub SuggestSiteName()
        Dim psuedoList As IList(Of Site) = BA_ReadSiteAttributes(SiteType.Pseudo)
        Dim pSitePrefix As String = "auto_site_"
        Dim pSiteId As Short = 0
        Dim bName As Boolean = False
        If psuedoList.Count > 0 Then
            Do While bName = False
                pSiteId += 1
                bName = True
                For Each pSite As Site In psuedoList
                    If pSite.Name.Equals(pSitePrefix & pSiteId) Then
                        bName = False
                        Exit For
                    End If
                Next
            Loop
        Else
            pSiteId += 1
        End If
        TxtSiteName.Text = pSitePrefix & pSiteId
    End Sub

    Private Sub SavePseudoSiteLog(ByVal objectId As Integer)
        m_lastAnalysis = New PseudoSite(objectId, TxtSiteName.Text, CkElev.Checked, CkPrecip.Checked, CkProximity.Checked, _
                                        CkLocation.Checked)
        'Save Elevation data
        If m_lastAnalysis.UseElevation Then
            Dim elevUnits As esriUnits = esriUnits.esriMeters
            If m_usingElevMeters = False Then _
                elevUnits = esriUnits.esriFeet
            m_lastAnalysis.ElevationProperties(elevUnits, CDbl(txtLower.Text), CDbl(TxtUpperRange.Text))
        End If
        'Save Prism settings
        If m_lastAnalysis.UsePrism Then
            m_lastAnalysis.PrismProperties(CmboxPrecipType.SelectedIndex, CmboxBegin.SelectedIndex, CmboxEnd.SelectedIndex, _
                                  CDbl(TxtPrecipUpper.Text), CDbl(TxtPrecipLower.Text))
        End If
        'Save Proximity settings
        If m_lastAnalysis.UseProximity Then
            Dim item As LayerListItem = LstVectors.SelectedItem
            Dim comps As Double = -1
            Dim isNumber As Boolean = Double.TryParse(txtBufferDistance.Text, comps)
            If isNumber Then
                m_lastAnalysis.ProximityProperties(m_usingXYUnits, item.Name, comps)
            Else
                m_lastAnalysis.ProximityProperties(m_usingXYUnits, item.Name, 0)
            End If
        End If
        'Save Location settings
        If m_lastAnalysis.UseLocation Then
            For Each pRow As DataGridViewRow In GrdLocation.Rows
                Dim filePath As String = Convert.ToString(pRow.Cells(m_idxFullPaths).Value)
                Dim selectedList As IList(Of String) = m_dictLocationIncludeValues(filePath)
                Dim layerName As String = Convert.ToString(pRow.Cells(m_idxLayer).Value)
                m_lastAnalysis.AddLocationProperties(layerName, filePath, BA_FIELD_VALUE, selectedList)
            Next
        End If
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.PseudoSiteXml)
        m_lastAnalysis.Save(xmlOutputPath)
    End Sub

    Private Sub ReloadLastAnalysis(ByVal scenarioTimeStamp As DateTime)
        If m_lastAnalysis IsNot Nothing Then
            Dim sb As StringBuilder = New StringBuilder()
            If m_lastAnalysis.DateCreated < scenarioTimeStamp Then
                'Don't reload form because represented area could be wrong for previous analysis
                Exit Sub
            Else
                sb.Append("BAGIS has loaded details for the previously generated pseudo-site. Use the ‘map’ button to view the pseudo-site and its supporting layers. ")
                sb.Append(vbCrLf + vbCrLf)
                sb.Append("If you wish to generate another auto pseudo-site, close this window and return to the Site Scenario Analysis tool. ")
                sb.Append("You must select the sites to be included in Scenario 1 based on your preferences and click the calculate button ")
                sb.Append("to generate a new analysis for the represented area, then rerun the auto pseudo site tool.")
                sb.Append(vbCrLf + vbCrLf)
                sb.Append("Check the checkbox for this pseudo-site in the Scenario 1 grid in the Site Scenario Analysis tool ")
                sb.Append("to include the newly generated site in future calculations.")
            End If
            TxtSiteName.Text = m_lastAnalysis.SiteName
            If m_lastAnalysis.UseElevation Then
                CkElev.Checked = True
                If m_lastAnalysis.ElevUnits = esriUnits.esriFeet Then
                    LblElevRange.Text = "Desired Range (Feet)"
                End If
                txtLower.Text = CStr(m_lastAnalysis.LowerElev)
                TxtUpperRange.Text = CStr(m_lastAnalysis.UpperElev)
            End If
            If m_lastAnalysis.UsePrism Then
                CkPrecip.Checked = True
                CmboxPrecipType.SelectedIndex = m_lastAnalysis.PrecipTypeIdx
                If CmboxPrecipType.SelectedIndex = 5 Then
                    lblBeginMonth.Enabled = True
                    CmboxBegin.Enabled = True
                    CmboxBegin.SelectedIndex = m_lastAnalysis.PrecipBeginIdx
                    lblEndMonth.Enabled = True
                    CmboxEnd.Enabled = True
                    CmboxEnd.SelectedIndex = m_lastAnalysis.PrecipEndIdx
                Else
                    lblBeginMonth.Enabled = False
                    CmboxBegin.Enabled = False
                    lblEndMonth.Enabled = False
                    CmboxEnd.Enabled = False
                End If
                TxtPrecipUpper.Text = CStr(m_lastAnalysis.LowerPrecip)
                TxtPrecipLower.Text = CStr(m_lastAnalysis.UpperPrecip)
                CmdPrism_Click(Me, EventArgs.Empty)
            End If
            If m_lastAnalysis.UseProximity = True Then
                CkProximity.Checked = True
                For Each item As LayerListItem In LstVectors.Items
                    If item.Name.Equals(m_lastAnalysis.ProximityLayer) Then
                        LstVectors.SelectedItem = item
                        Exit For
                    End If
                Next
                If m_lastAnalysis.BufferUnits = esriUnits.esriFeet Then
                    LblElevRange.Text = "Buffer Distance (Feet):"
                End If
                txtBufferDistance.Text = CStr(m_lastAnalysis.BufferDistance)
            End If
            If sb.Length > 0 Then
                MessageBox.Show(sb.ToString, "BAGIS Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Public Function PreparePointFileToAppend(ByVal snapRasterPath As String) As Site
        Dim fClass As IFeatureClass = Nothing
        Dim aField As IField = Nothing
        Dim aCursor As IFeatureCursor = Nothing
        Dim aFeature As IFeature = Nothing
        Try
            '1. Delete any fields that aren't shape or objectid
            fClass = BA_OpenFeatureClassFromGDB(m_analysisFolder, m_siteFileName)
            If fClass IsNot Nothing Then
                For i As Short = fClass.Fields.FieldCount - 1 To 0 Step -1
                    aField = fClass.Fields.Field(i)
                    Select Case aField.Name
                        Case BA_FIELD_OBJECT_ID
                            'Do nothing
                        Case BA_FIELD_SHAPE
                            'Do nothing
                        Case Else
                            fClass.DeleteField(aField)
                    End Select
                Next
            End If
            '2. Calculate site elevation: Use extract values to points
            Dim filledDemPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) + BA_EnumDescription(MapsFileName.filled_dem_gdb)
            Dim tempFileName As String = "tmpExtract"
            Dim success As BA_ReturnCode = BA_ExtractValuesToPoints(m_analysisFolder + "\" + m_siteFileName, filledDemPath, _
                                                                    m_analysisFolder + "\" + tempFileName, snapRasterPath, False)
            Dim newSite As Site = Nothing
            If success = BA_ReturnCode.Success Then
                Dim elev As Double = 9999.0
                fClass = BA_OpenFeatureClassFromGDB(m_analysisFolder, tempFileName)
                Dim idxElev As Short = fClass.Fields.FindField(BA_RasterValu)
                If idxElev > -1 Then
                    aCursor = fClass.Search(Nothing, False)
                    aFeature = aCursor.NextFeature
                    If aFeature IsNot Nothing Then
                        elev = Convert.ToDouble(aFeature.Value(idxElev))
                    End If
                End If
                BA_Remove_ShapefileFromGDB(m_analysisFolder, tempFileName)
                '3. Updates the site attributes
                'Only 1 site; Site id is always 1
                newSite = New Site(1, TxtSiteName.Text, SiteType.Pseudo, elev, False)
                success = BA_UpdatePseudoSiteAttributes(m_analysisFolder, m_siteFileName, 1, newSite)
            End If
            Return newSite
        Catch ex As Exception
            Debug.Print("PreparePointFileToAppend: " & ex.Message)
            Return Nothing
        Finally
            fClass = Nothing
            aField = Nothing
            aCursor = Nothing
            aFeature = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function GetNewSiteObjectId(ByVal siteElev As Double) As Integer
        Dim fClass As IFeatureClass = Nothing
        Dim aCursor As IFeatureCursor = Nothing
        Dim aFeature As IFeature = Nothing
        Dim aQueryFilter As IQueryFilter = New QueryFilter()
        Try
            Dim objectId As Integer = -1
            fClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), BA_EnumDescription(MapsFileName.Pseudo))
            Dim idxOid As Short = fClass.Fields.FindField(BA_FIELD_OBJECT_ID)
            aQueryFilter.WhereClause = " " & BA_SiteNameField & " = '" & TxtSiteName.Text & _
                                       " ' and " & BA_SiteElevField & " = " & siteElev
            If idxOid > -1 Then
                aCursor = fClass.Search(Nothing, False)
                aFeature = aCursor.NextFeature
                Do While aFeature IsNot Nothing
                    If aFeature IsNot Nothing Then
                        objectId = Convert.ToInt16(aFeature.Value(idxOid))
                    End If
                    aFeature = aCursor.NextFeature
                Loop
            End If
            Return objectId
        Catch ex As Exception
            Debug.Print("GetNewSiteObjectId: " & ex.Message)
            Return -1
        End Try
    End Function

    Public Event FormInputChanged()

    Protected Sub Form_InputChanged() Handles Me.FormInputChanged
        BtnMap.Enabled = False
        BtnFindSite.Enabled = True
    End Sub

    Private Sub CmboxBegin_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxBegin.SelectedIndexChanged
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub CmboxEnd_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxEnd.SelectedIndexChanged
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub TxtPrecipUpper_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtPrecipUpper.Validating
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub TxtPrecipLower_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtPrecipLower.Validating
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub LstVectors_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstVectors.SelectedIndexChanged
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub txtBufferDistance_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtBufferDistance.Validating
        RaiseEvent FormInputChanged()
    End Sub

    Private Function ValidBufferDistance() As String
        Dim item As LayerListItem = LstVectors.SelectedItem
        Dim sb As StringBuilder = New StringBuilder
        If item IsNot Nothing Then
            Dim fClass As IFeatureClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), item.Name)
            If fClass IsNot Nothing Then
                If Not fClass.ShapeType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
                    Dim comps As Double = -1
                    If Double.TryParse(txtBufferDistance.Text, comps) Then
                        If comps <= 0 Then
                            sb.Append("Value greater than 0 required for features that are not polygons!" + vbCrLf)
                        End If
                    Else
                        sb.Append("Numeric value required for features that are not polygons!" + vbCrLf)
                    End If
                End If
            End If
        End If
        Return sb.ToString
    End Function

    Private Sub BtnClear_Click(sender As System.Object, e As System.EventArgs) Handles BtnClear.Click
        SuggestSiteName()
        CkElev.Checked = False
        txtLower.Text = txtMinElev.Text
        TxtUpperRange.Text = TxtMaxElev.Text
        CkPrecip.Checked = False
        CmboxPrecipType.SelectedIndex = 0
        CmboxBegin.SelectedIndex = 0
        CmboxEnd.SelectedIndex = 0
        txtMinPrecip.Text = "-"
        txtMaxPrecip.Text = "-"
        TxtPrecipUpper.Text = Nothing
        TxtPrecipLower.Text = Nothing
        CkProximity.Checked = False
        LstVectors.ClearSelected()
        LstRasters.ClearSelected()
        txtBufferDistance.Text = Nothing
    End Sub

    Private Sub TxtSiteName_TextChanged(sender As Object, e As System.EventArgs) Handles TxtSiteName.TextChanged
        If m_formLoaded = True Then
            RaiseEvent FormInputChanged()
        End If
    End Sub

    Private Sub txtMinPrecip_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMinPrecip.TextChanged
        ManagePrecipRange()
    End Sub

    Private Sub txtMaxPrecip_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMaxPrecip.TextChanged
        ManagePrecipRange()
    End Sub

    Private Sub ManagePrecipRange()
        Dim comps As Double = -1
        If Double.TryParse(txtMinPrecip.Text, comps) Then
            If Double.TryParse(txtMaxPrecip.Text, comps) Then
                TxtPrecipUpper.Enabled = True
                TxtPrecipLower.Enabled = True
            Else
                TxtPrecipUpper.Enabled = False
                TxtPrecipLower.Enabled = False
            End If
        Else
            TxtPrecipUpper.Enabled = False
            TxtPrecipLower.Enabled = False
        End If
    End Sub

    Private Sub ClipDataFrameToAoiBorder()
        Try
            Dim clipFc As IFeatureClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi), m_aoiBoundary)
            Dim clipCursor As IFeatureCursor = clipFc.Search(Nothing, True)
            Dim clipFeature As IFeature = clipCursor.NextFeature
            If clipFeature IsNot Nothing Then
                Dim mOptions As IMapClipOptions = CType(My.Document.FocusMap, IMapClipOptions)
                mOptions.ClipGeometry = clipFeature.Shape
                'Create border object
                Dim pBorder As IBorder = New SymbolBorder
                Dim pStyleGallery As IStyleGallery = My.Document.StyleGallery
                Dim pEnumStyleGallery As IEnumStyleGalleryItem = pStyleGallery.Items("Borders", "ESRI.style", "Default")
                pEnumStyleGallery.Reset()
                Dim pStyleItem As IStyleGalleryItem2 = pEnumStyleGallery.Next
                Do Until pStyleItem Is Nothing
                    If pStyleItem.Name = "3.0 Point" Then
                        pBorder = pStyleItem.Item
                        'Apply border object
                        mOptions.ClipBorder = pBorder
                        Exit Do
                    End If
                    pStyleItem = pEnumStyleGallery.Next
                Loop
            End If
        Catch ex As Exception
            Debug.Print("ClipDataFrameToAoiBorder Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub CkLocation_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkLocation.CheckedChanged
        GrpLocation.Enabled = CkLocation.Checked
        RaiseEvent FormInputChanged()
    End Sub

    Private Sub LstRasters_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstRasters.SelectedIndexChanged
        LstValues.Items.Clear()
        If LstRasters.SelectedIndex > -1 Then
            Dim item As LayerListItem = LstRasters.SelectedItem
            Dim folderName As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(item.Value, folderName)
            Dim pEnum As IEnumerator = BA_QueryUniqueValuesFromRasterGDB(folderName, fileName, BA_FIELD_VALUE)
            If pEnum IsNot Nothing Then
                While pEnum.MoveNext
                    LstValues.Items.Add(Convert.ToString(pEnum.Current))
                End While
            End If
        End If
    End Sub

    Private Sub ToggleLocationButtons(ByVal enabled As Boolean)
        BtnAddLocation.Enabled = enabled
        BtnDeleteLocation.Enabled = enabled
        BtnEditLocation.Enabled = enabled
        If GrdLocation.SelectedRows.Count = 0 Then
            BtnDeleteLocation.Enabled = False
            BtnEditLocation.Enabled = False
        End If
    End Sub

    Private Sub BtnAddLocation_Click(sender As System.Object, e As System.EventArgs) Handles BtnAddLocation.Click
        PnlLocation.Visible = True
    End Sub

    Private Sub PnlLocation_VisibleChanged(sender As Object, e As System.EventArgs) Handles PnlLocation.VisibleChanged
        ToggleLocationButtons(Not PnlLocation.Visible)
    End Sub

    Private Sub BtnCancelLocation_Click(sender As System.Object, e As System.EventArgs) Handles BtnDoneLocation.Click
        PnlLocation.Visible = False
        LstRasters.ClearSelected()
    End Sub

    Private Sub BtnDoneLocation_Click(sender As System.Object, e As System.EventArgs) Handles BtnSaveLocation.Click
        Dim sb As StringBuilder = New StringBuilder()
        Dim lstAllValues As IList(Of String) = New List(Of String)
        Dim lstSelectValues As IList(Of String) = New List(Of String)
        Dim rasterItem As LayerListItem = LstRasters.SelectedItem
        If LstValues.SelectedItems.Count < 1 Then
            MessageBox.Show("You must select at least one value to use this layer in the analysis")
            Exit Sub
        Else
            For i As Integer = 0 To LstValues.Items.Count - 1
                lstAllValues.Add(LstValues.Items(i))
                If LstValues.GetSelected(i) Then
                    sb.Append(LstValues.Items(i) + m_sep)
                    lstSelectValues.Add(LstValues.Items(i))
                End If
            Next
            sb.Remove(sb.ToString().LastIndexOf(m_sep), m_sep.Length)
            If m_dictLocationAllValues Is Nothing Then
                m_dictLocationAllValues = New Dictionary(Of String, IList(Of String))
                m_dictLocationIncludeValues = New Dictionary(Of String, IList(Of String))
            End If
            '@ToDo: Need to make sure only one row per layer location, otherwise this will fail
            m_dictLocationAllValues.Add(rasterItem.Value, lstAllValues)
            m_dictLocationIncludeValues.Add(rasterItem.Value, lstSelectValues)
        End If
        Dim item As New DataGridViewRow
        item.CreateCells(GrdLocation)
        With item
            .Cells(m_idxLayer).Value = rasterItem.Name
            .Cells(m_idxValues).Value = sb.ToString
            .Cells(m_idxFullPaths).Value = rasterItem.Value
        End With
        '---add the row---
        GrdLocation.Rows.Add(item)
        LstRasters.ClearSelected()
    End Sub

    Private Sub GrdLocation_SelectionChanged(sender As Object, e As System.EventArgs) Handles GrdLocation.SelectionChanged
        If PnlLocation.Visible = False Then
            ToggleLocationButtons(True)
        End If
    End Sub

    Private Sub BtnDeleteLocation_Click(sender As System.Object, e As System.EventArgs) Handles BtnDeleteLocation.Click
        Dim res As DialogResult = MessageBox.Show("You are about to delete a row from the Location constraint list. " + _
                                                  "This cannot be undone." + vbCrLf + vbCrLf + "Do you wish to continue ?",
                                                  "Delete", MessageBoxButtons.YesNo)
        If res = Windows.Forms.DialogResult.Yes Then
            GrdLocation.Rows.Remove(GrdLocation.SelectedRows(0))
        End If
    End Sub

    Private Sub DeletePreviousRun()
        Dim fileToDelete As String = m_analysisFolder + "\" + m_elevLayer
        If BA_File_Exists(fileToDelete, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            BA_RemoveRasterFromGDB(m_analysisFolder, m_elevLayer)
        End If
        fileToDelete = m_analysisFolder + "\" + m_precipLayer
        If BA_File_Exists(fileToDelete, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            BA_RemoveRasterFromGDB(m_analysisFolder, m_precipLayer)
        End If
        fileToDelete = m_analysisFolder + "\" + m_proximityLayer
        If BA_File_Exists(fileToDelete, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            BA_RemoveRasterFromGDB(m_analysisFolder, m_proximityLayer)
        End If
        '@ToDo: Delete location rasters when they are added
        fileToDelete = m_analysisFolder + "\" + m_siteFileName
        If BA_File_Exists(fileToDelete, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            BA_Remove_ShapefileFromGDB(m_analysisFolder, m_siteFileName)
        End If
    End Sub
End Class