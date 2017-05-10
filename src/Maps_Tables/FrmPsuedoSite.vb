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
    Private m_demInMeters As Boolean    'Inherited from Site Scenario form; Controls elevation display/calculation
    Private m_usingElevMeters As Boolean    'Inherited from Site Scenario form; Controls elevation display/calculation
    Private m_usingXYUnits As esriUnits  'Inerited from Site Scenario form; Controls proximity display/calculation  
    Private m_aoiBoundary As String = BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
    Private m_lastAnalysis As PseudoSite = Nothing
    Private m_formLoaded As Boolean = False

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
        Dim pRasterStats As IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)
        'Determine if Display ZUnit is the same as DEM ZUnit
        'AOI_DEMMin and AOI_DEMMax use internal system unit, i.e., meters
        Dim Conversion_Factor As Double = BA_SetConversionFactor(m_usingElevMeters, m_demInMeters) 'i.e., meters to meters
        AOI_DEMMin = Math.Round(pRasterStats.Minimum * Conversion_Factor - 0.005, 2)
        AOI_DEMMax = Math.Round(pRasterStats.Maximum * Conversion_Factor + 0.005, 2)

        'Populate Boxes
        txtMinElev.Text = Convert.ToString(AOI_DEMMin)
        TxtMaxElev.Text = Convert.ToString(AOI_DEMMax)
        TxtRange.Text = Val(TxtMaxElev.Text) - Val(txtMinElev.Text)

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
        LoadLstLayers()
        BA_SetDefaultProjection(My.ArcMap.Application)

        'Check for previously saved scenario and load those values as defaults
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.PseudoSiteXml)

        ' Open analysis file if there is one
        If BA_File_ExistsWindowsIO(xmlOutputPath) Then
            m_lastAnalysis = BA_LoadPseudoSiteFromXml(AOIFolderBase)
            ReloadLastAnalysis(siteScenarioToolTimeStamp)
            BtnMap.Enabled = True
            BtnFindSite.Enabled = False
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

        'If user selected proximity layer, did they choose a layer?
        If CkProximity.Checked AndAlso LstVectors.SelectedItem Is Nothing Then
            Dim res As DialogResult = MessageBox.Show("You selected the Proximity option but failed to select a layer. Do you wish to " + _
                                                      "find a site without using the Proximity option ?", "Missing layer", MessageBoxButtons.YesNo, _
                                                      MessageBoxIcon.Question)
            If res <> Windows.Forms.DialogResult.Yes Then
                CkProximity.Checked = False
                Exit Sub
            End If
        End If

        Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        Dim maskPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & m_aoiBoundary

        BtnFindSite.Enabled = False

        'Use this to hold the list of layers that we send to the union tool
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(m_analysisFolder + "\" + m_representedArea + "; ")

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

        If CkProximity.Checked = True Then
            Dim errorMsg As String = ValidBufferDistance()
            If Not String.IsNullOrEmpty(errorMsg) Then
                txtBufferDistance.Select(0, txtBufferDistance.Text.Length)
                errorMsg = errorMsg + " The proximity layer will not be used in analysis."
                MessageBox.Show(errorMsg)
            Else
                success = GenerateProximityLayer(pStepProg)
                If success = BA_ReturnCode.Success Then
                    sb.Append(m_analysisFolder + "\" + m_proximityLayer + "; ")
                End If
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

        Dim furthestPixelFileName As String = "ps_furthest"
        If success = BA_ReturnCode.Success Then
            pStepProg.Message = "Finding furthest pixel"
            pStepProg.Step()
            'Get the maximum pixel value
            'Set everything to null that is smaller than that; should leave one pixel
            'Expression can be more precise; Rounding down works for now
            Dim cellSize As Double = -1
            Dim pRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(m_analysisFolder + "\" + distanceFileName, cellSize)
            Dim pExpression As String = Nothing
            If pRasterStats IsNot Nothing Then
                'sample expression: SetNull('C:\Docs\Lesley\animas_AOI_prms_3\analysis.gdb\ps_distance' < 6259,'C:\Docs\Lesley\animas_AOI_prms_3\analysis.gdb\ps_distance')
                Dim targetPath As String = m_analysisFolder + "\" + distanceFileName
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
                MessageBox.Show(numSites & " pseudo-sites were found. Right now BAGIS only knows how to deal with one so it will pick the first one.")
                'Delete all sites except the first one
                Dim strSelect As String = " " + BA_FIELD_OBJECT_ID + " > 1"
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
            BA_Remove_ShapefileFromGDB(m_analysisFolder, unionFileName)
            BA_RemoveRasterFromGDB(m_analysisFolder, distanceFileName)
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
        Dim strMinElev As String = txtMinElev.Text
        Dim strLower As String = txtLower.Text
        Dim strUpperRange As String = TxtUpperRange.Text
        Dim strMaxElev As String = TxtMaxElev.Text
        'Convert the values to the DEM value, before composing the reclass string, if we need to
        If m_demInMeters <> m_usingElevMeters Then
            Dim converter As IUnitConverter = New UnitConverter
            Dim toElevUnits As esriUnits = esriUnits.esriMeters
            If Not m_demInMeters Then _
                toElevUnits = esriUnits.esriFeet
            Dim fromElevUnits As esriUnits = esriUnits.esriFeet
            If m_usingElevMeters Then _
                fromElevUnits = esriUnits.esriMeters
            strMinElev = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(txtMinElev.Text), fromElevUnits, toElevUnits)))
            strLower = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(txtLower.Text), fromElevUnits, toElevUnits)))
            strUpperRange = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(TxtUpperRange.Text), fromElevUnits, toElevUnits)))
            strMaxElev = Convert.ToString(Math.Round(converter.ConvertUnits(Convert.ToDouble(TxtMaxElev.Text), fromElevUnits, toElevUnits)))
        End If
        sb.Append(strMinElev + " " + strLower + " 1;")
        sb.Append(strLower + " " + strUpperRange + " 2;")
        sb.Append(strUpperRange + " " + strMaxElev + " 3 ")
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
        Dim elevLayerPath As String = m_analysisFolder & "\" & m_elevLayer
        If success = BA_ReturnCode.Success Then
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
        RaiseEvent FormInputChanged()
        Dim sb As StringBuilder = New StringBuilder()
        Dim comps As Double
        Dim minElev As Double = CDbl(txtMinElev.Text)
        Dim upperElev As Double = 99999
        If Not String.IsNullOrEmpty(TxtUpperRange.Text) Then
            upperElev = CDbl(TxtUpperRange.Text)
        End If
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
        RaiseEvent FormInputChanged()
        Dim sb As StringBuilder = New StringBuilder()
        Dim comps As Double
        Dim maxElev As Double = CDbl(TxtMaxElev.Text)
        Dim lowerRange As Double = 0
        If Not String.IsNullOrEmpty(txtLower.Text) Then
            lowerRange = CDbl(txtLower.Text)
        End If
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

        BA_RemoveLayersfromLegend(My.Document)
        'Note: these functions are called in BA_DisplayMap if we end up adding buttons
        Dim UnitText As String = Nothing    'Textbox above scale bar
        Dim subtitle As String = "PROPOSED PSEUDO SITE LOCATION"
        BA_MapUpdateSubTitle(My.Document, mapTitle, subtitle, UnitText)
        Dim keyLayerName As String = Nothing
        BA_SetLegendFormat(My.Document, keyLayerName)
    End Sub

    Private Sub AddLayersToMapFrame(ByVal pApplication As ESRI.ArcGIS.Framework.IApplication, _
                                    ByVal pMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument)
        Dim pColor As IColor = New RgbColor
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim retVal As Integer = -1

        Try
            'Elevation if it exists
            Dim filepathname As String = m_analysisFolder & "\" & m_elevLayer
            If m_lastAnalysis.UseElevation Then
                If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    pColor.RGB = RGB(115, 178, 115) 'green
                    success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_ELEVATION, pColor, 30)
                End If
            End If

            'Scenario 2 Represented area
            filepathname = m_analysisFolder & "\" & m_representedArea
            If Not BA_File_Exists(filepathname, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                MessageBox.Show("Unable to locate the represented area from the site scenario tool. Cannot load map.", "Error", _
                     MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            pColor.RGB = RGB(255, 0, 0) 'red
            success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_REPRESENTED, pColor, 30)

            'Proximity if it exists
            If m_lastAnalysis.UseProximity Then
                filepathname = m_analysisFolder & "\" & m_proximityLayer
                If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    pColor.RGB = RGB(255, 165, 0) 'orange
                    success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_PROXIMITY, pColor, 30)
                End If
            End If

            'add aoi boundary and zoom to AOI
            filepathname = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + m_aoiBoundary
            success = BA_AddExtentLayer(pMxDoc, filepathname, Nothing, BA_MAPS_AOI_BOUNDARY, 0, 1.2, 2.0)

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
            retVal = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_PS_INCLUDE, _
                                                MapsDisplayStyle.Cyan_Light_to_Blue_Dark, 30, WorkspaceType.Geodatabase)

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

    Private Sub LoadLstLayers()
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
    End Sub

    Private Function GenerateProximityLayer(ByVal pStepProg As IStepProgressor) As BA_ReturnCode
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
        Dim outFeaturesPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + "tmpBuffer"
        If item IsNot Nothing Then
            success = BA_Buffer(item.Value, outFeaturesPath, strBuffer)
        End If
        If success = BA_ReturnCode.Success Then
            success = BA_Erase(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + m_aoiBoundary, _
                               outFeaturesPath, BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & _
                               m_proximityLayer)
            BA_Remove_ShapefileFromGDB(m_analysisFolder, "tmpBuffer")
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
        m_lastAnalysis = New PseudoSite(objectId, TxtSiteName.Text, CkElev.Checked, CkPrecip.Checked, CkProximity.Checked)
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
                                  CDbl(TxtPrecipLower.Text), CDbl(TxtPrecipUpper.Text))
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
                TxtPrecipLower.Text = CStr(m_lastAnalysis.LowerPrecip)
                TxtPrecipUpper.Text = CStr(m_lastAnalysis.UpperPrecip)
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
        If CkProximity.Checked Then
            Dim errorMsg As String = ValidBufferDistance()
            If Not String.IsNullOrEmpty(errorMsg) Then
                e.Cancel = True
                txtBufferDistance.Select(0, txtBufferDistance.Text.Length)
                MessageBox.Show(errorMsg)
            End If
        End If
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
                        If comps < 1 Then
                            sb.Append("Value greater than 0 required for features that are not polygons!")
                        End If
                    Else
                        sb.Append("Numeric value required for features that are not polygons!")
                    End If
                End If
            End If
        End If
        Return sb.ToString
    End Function

    Private Sub BtnClear_Click(sender As System.Object, e As System.EventArgs) Handles BtnClear.Click
        SuggestSiteName()
        CkElev.Checked = False
        txtLower.Text = Nothing
        TxtUpperRange.Text = Nothing
        CkPrecip.Checked = False
        CmboxPrecipType.SelectedIndex = 0
        CmboxBegin.SelectedIndex = 0
        CmboxEnd.SelectedIndex = 0
        txtMinPrecip.Text = "-"
        txtMaxPrecip.Text = "-"
        TxtPrecipLower.Text = Nothing
        TxtPrecipUpper.Text = Nothing
        CkProximity.Checked = False
        LstVectors.ClearSelected()
        txtBufferDistance.Text = Nothing
    End Sub

    Private Sub TxtSiteName_TextChanged(sender As Object, e As System.EventArgs) Handles TxtSiteName.TextChanged
        If m_formLoaded = True Then
            RaiseEvent FormInputChanged()
        End If
    End Sub
End Class