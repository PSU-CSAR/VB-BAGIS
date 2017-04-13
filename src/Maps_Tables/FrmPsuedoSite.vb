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
    Private m_precipFolder As String
    Private m_precipFile As String
    Private m_elevLayer As String = "ps_elev"
    Private m_siteFileName As String = "ps_site"
    Private m_proximityLayer As String = "ps_proximity"
    Private m_demInMeters As Boolean
    Private m_usingElevMeters As Boolean    'Inherited from Site Scenario form; Controls elevation display
    Private m_demXYUnits As esriUnits = esriUnits.esriMeters
    Private m_usingXYMeters As Boolean  'Inerited from Site Scenario form; Controls buffer display  


    Public Sub New(ByVal demInMeters As Boolean, ByVal useMeters As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        'Populate class-level variables
        m_usingElevMeters = useMeters
        m_demInMeters = demInMeters

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

        LoadLstLayers()

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

        If CkProximity.Checked = True Then
            success = GenerateProximityLayer(pStepProg)
            If success = BA_ReturnCode.Success Then
                sb.Append(m_analysisFolder + "\" + m_proximityLayer + "; ")
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

        If success = BA_ReturnCode.Success Then
            Dim numSites As Int16 = BA_CountPolygons(m_analysisFolder, m_siteFileName, BA_FIELD_GRIDCODE_GDB)
            If numSites < 1 Then
                MessageBox.Show("No psuedo-sites were found. Please double-check your selection criteria")
            ElseIf numSites > 1 Then
                MessageBox.Show(numSites & " pseudo-sites were found. Right now BAGIS only knows how to deal with one so it will pick the first one.")
            End If

            'Delete the layers we don't need to keep for the map
            BA_Remove_ShapefileFromGDB(m_analysisFolder, unionFileName)
            BA_RemoveRasterFromGDB(m_analysisFolder, distanceFileName)
            BA_RemoveRasterFromGDB(m_analysisFolder, furthestPixelFileName)
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
            If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                pColor.RGB = RGB(65, 105, 225) 'royal blue
                success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_ELEVATION, pColor)
            End If

            'Represented area
            filepathname = m_analysisFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea)
            If Not BA_File_Exists(filepathname, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                MessageBox.Show("Unable to locate the represented area from the site scenario tool. Cannot load map.", "Error", _
                     MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            pColor.RGB = RGB(255, 0, 0) 'red
            success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_PS_REPRESENTED, pColor)

            'add aoi boundary and zoom to AOI
            filepathname = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
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
                Dim pseudoCopy As IFeatureLayer = fLayerDef.CreateSelectionLayer(siteLayerName, True, Nothing, Nothing)
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
        Dim bufferDistance As Double = Convert.ToDouble(txtBufferDistance.Text)
        Dim strBuffer As String = Convert.ToString(bufferDistance) + " "
        If m_demXYUnits = esriUnits.esriMeters Then
            strBuffer = strBuffer + MeasurementUnit.Meters.ToString
        Else
            strBuffer = strBuffer + MeasurementUnit.Feet.ToString
        End If

        Dim item As LayerListItem = LstVectors.SelectedItem
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim outFeaturesPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + "tmpBuffer"
        If item IsNot Nothing Then
            success = BA_Buffer(item.Value, outFeaturesPath, strBuffer)
        End If
        If success = BA_ReturnCode.Success Then
            success = BA_Erase(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & BA_BASIN_DEM_EXTENT_SHAPEFILE, _
                               outFeaturesPath, BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & _
                               m_proximityLayer)
        End If
        If Not success = BA_ReturnCode.Success Then
            MessageBox.Show("An error occurred while generating the proximity layer. It will not be used in analysis")
        End If
        Return success
    End Function
End Class