Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System.ComponentModel
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.IO
Imports System.Text

Public Class frmAOIInfo
    Private AOIRasterList() As String
    Private AOIVectorList() As String
    Private m_aoi As Aoi
    Private m_snotelClipLayer As String = BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub CmdClearSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearSelected.Click
        LstRasters.SelectedIndex = -1
        LstVectors.SelectedIndex = -1
    End Sub
    Dim m_version As String
    Private m_PRISMClipBuffer As Double = -1

    Private Sub CmdSetAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetAOI.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim tempAOIFolderBase As String = ""
        Dim response As Integer

        If BA_Enable_SAExtension(My.ArcMap.Application) <> ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for BAGIS and is not available. Program stopped.")
            Exit Sub
        End If

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select AOI Folder"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then
            Exit Sub
        End If

        'get the name of the selected folder
        Dim pGxDataFolder As IGxFile = pGxObject.Next
        tempAOIFolderBase = pGxDataFolder.Path
        pGxDialog = Nothing
        pGxDataFolder = Nothing

        If String.IsNullOrEmpty(tempAOIFolderBase) Then Exit Sub 'user cancelled the action

        'check if the selected folder is a valid AOI
        Dim success As BA_ReturnCode = BA_CheckAoiStatus(tempAOIFolderBase, My.ArcMap.Application.hWnd, My.ArcMap.Document)
        If success <> BA_ReturnCode.Success Then 'the folder is not an AOI or has missing layers
            Exit Sub
        End If

        success = BA_SetDefaultProjection(My.ArcMap.Application)
        If success <> BA_ReturnCode.Success Then    'unable to set the default projection
            Exit Sub
        End If

        'set mapframe name to default name
        response = BA_SetDefaultMapFrameName(BA_MAPS_DEFAULT_MAP_NAME, My.Document)
        Dim response1 As Integer = BA_SetMapFrameDimension(BA_MAPS_DEFAULT_MAP_NAME, 1, 2, 7.5, 9, True)

        'global variables: pSelectColor and pDisplayColor
        pSelectColor = New RgbColor
        'pSelectColor determines the color for AOI and Pourpoint graphics
        pSelectColor.RGB = RGB(0, 255, 0)

        'pDisplayColor determines the color for AOI and Pourpoint shapfiles
        pDisplayColor = New RgbColor
        pDisplayColor.RGB = RGB(255, 0, 0)

        FrameBAGISLayers.Enabled = True
        CmdAddLayer.Enabled = True
        FrameUserLayers.Enabled = True

        'check Snotel, snow course, and PRISM data and determine the value for BA_SystemSettings.GenerateAOIOnly value
        'reset their selected checkboxes
        ChkPRISMSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkSnowCourseSelected.Checked = False
        CmdReClip.Enabled = False

        BA_SystemSettings.GenerateAOIOnly = False

        'PRISM
        Dim temppathname As String = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\" & AOIPrismFolderNames.annual.ToString
        If Not BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            ChkPRISMExist.Checked = False
            BA_SystemSettings.GenerateAOIOnly = True
        Else
            ChkPRISMExist.Checked = True
            Dim depthUnits As MeasurementUnit = BA_GetDepthUnit(tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism), AOIPrismFolderNames.annual.ToString)
            Select Case depthUnits
                Case MeasurementUnit.Inches
                    rbtnDepthInch.Checked = True
                Case MeasurementUnit.Millimeters
                    rbtnDepthMM.Checked = True
            End Select
            Dim dblPrismBuffer As Double = BA_GetBufferDistance(tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism), _
                                                                AOIPrismFolderNames.annual.ToString, esriDatasetType.esriDTRasterDataset)
            If dblPrismBuffer > 0 Then _
                BA_PRISMClipBuffer = dblPrismBuffer
            txtPrismBufferDist.Text = CStr(BA_PRISMClipBuffer)
            ' This variable keeps track of whether the PRISM clip buffer is changed; If changed, we need to recreate p_aoi_v and p_aoi
            m_PRISMClipBuffer = BA_PRISMClipBuffer
        End If

        'SNOTEL
        temppathname = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SNOTELSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSNOTELExist.Checked = True
            Dim dblSnotelBuffer As Double = BA_GetBufferDistance(tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), _
                                                                 BA_SNOTELSites, esriDatasetType.esriDTFeatureClass)
            If dblSnotelBuffer > 0 Then _
                BA_SnotelClipBuffer = dblSnotelBuffer
            TxtSnotelBuffer.Text = CStr(BA_SnotelClipBuffer)
        Else
            ChkSNOTELExist.Checked = False
            TxtSnotelBuffer.Text = Nothing
        End If

        'Snow Courses
        temppathname = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SnowCourseSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSnowCourseExist.Checked = True
            Dim dblSCBuffer As Double = BA_GetBufferDistance(tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), _
                                                             BA_SnowCourseSites, esriDatasetType.esriDTFeatureClass)
            If dblSCBuffer > 0 Then _
                BA_SnowCourseClipBuffer = dblSCBuffer
            TxtSnowCourseBuffer.Text = CStr(BA_SnowCourseClipBuffer)
        Else
            ChkSnowCourseExist.Checked = False
            TxtSnowCourseBuffer.Text = Nothing
        End If

        'BA_SetAOI activates/deactivates the Analysis and Maps menu items based on the presence of SNOTEL, Snow Course, and PRISM data in the AOI.
        'It overwrites the Options setttings.
        BA_SetAOI(tempAOIFolderBase)
        AOIFolderBase = tempAOIFolderBase

        If String.IsNullOrEmpty(AOIFolderBase) Then
            'MsgBox "Please set an AOI first!"
            FrameBAGISLayers.Enabled = False
            CmdAddLayer.Enabled = False
            FrameUserLayers.Enabled = False
            'Exit Sub
        End If

        'display folder paths
        txtDEMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        txtPRISMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        txtLayersFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)

        'display aoi area
        BA_ReadPPAttributes(AOIFolderBase, AOI_ShapeArea, AOI_ShapeUnit, AOI_ReferenceArea, AOI_ReferenceUnit)

        If AOI_ShapeArea <= 0 Then
            MsgBox("Unable to read the pour point layer of the AOI!" & vbCrLf & "AOI folder: " & AOIFolderBase & _
            vbCrLf & "Featureclass: " & BA_POURPOINTCoverage)
            Exit Sub
            Me.Close()
        End If

        txtArea.Text = Format(AOI_ShapeArea, "#0.00")
        txtAreaAcre.Text = Format(AOI_ShapeArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(AOI_ShapeArea * 0.3861022, "#0.00")

        txtRefArea.Text = Format(AOI_ReferenceArea, "#0.00")
        lblRefUnit.Text = AOI_ReferenceUnit

        'Get DEM statistics
        Dim pRasterStats As IRasterStatistics = Nothing
        Dim fullfilepath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        pRasterStats = BA_GetDemStatsGDB(AOIFolderBase)

        If pRasterStats IsNot Nothing Then
            txtMinElev.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
            txtMaxElev.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
            txtRangeElev.Text = Math.Round((pRasterStats.Maximum - pRasterStats.Minimum) + 0.005, 2)

            pRasterStats = Nothing
        Else
            MsgBox("Unable to read the filled DEM! The AOI surface.gdb is corrupted.")
        End If

        Dim aoiName As String = BA_GetBareName(tempAOIFolderBase)
        m_aoi = New Aoi(aoiName, tempAOIFolderBase, Nothing, m_version)
        'update caption
        Me.Text = "AOI: " & aoiName
        LoadLstLayers()

        BasinFolderBase = Nothing
        Dim cboSelectedBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        cboSelectedBasin.setValue(BA_GetBareName(BasinFolderBase))
        MsgBox(aoiName & " is set as the current AOI!", MsgBoxStyle.Information)
        'set the value of cbotargeted basin to the name of the aoi since an aoi should also be a basin
        'Dim cboselectbasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        'cboselectbasin.setValue(BA_GetBareName(tempAOIFolderBase))
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Call CoFreeUnusedLibraries()
    End Sub

    Private Sub frmAOIInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If String.IsNullOrEmpty(AOIFolderBase) Then
            'MsgBox "Please set an AOI first!"
            FrameBAGISLayers.Enabled = False
            CmdAddLayer.Enabled = False
            CmdReClip.Enabled = False
            FrameUserLayers.Enabled = False
            Exit Sub
        End If

        'global variables: pSelectColor and pDisplayColor
        pSelectColor = New RgbColor
        'pSelectColor determines the color for AOI and Pourpoint graphics
        pSelectColor.RGB = RGB(0, 255, 0)

        'pDisplayColor determines the color for AOI and Pourpoint shapfiles
        pDisplayColor = New RgbColor
        pDisplayColor.RGB = RGB(255, 0, 0)

        FrameBAGISLayers.Enabled = True
        CmdAddLayer.Enabled = True
        FrameUserLayers.Enabled = True

        'display folder paths
        txtDEMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        txtPRISMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        txtLayersFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)

        'display aoi area
        BA_ReadPPAttributes(AOIFolderBase, AOI_ShapeArea, AOI_ShapeUnit, AOI_ReferenceArea, AOI_ReferenceUnit)

        If AOI_ShapeArea <= 0 Then
            MsgBox("Unable to read the pour point layer of the AOI!" & vbCrLf & "AOI folder: " & AOIFolderBase & _
            vbCrLf & "Featureclass: " & BA_POURPOINTCoverage)
            Exit Sub
            Me.Close()
        End If

        txtArea.Text = Format(AOI_ShapeArea, "#0.00")
        txtAreaAcre.Text = Format(AOI_ShapeArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(AOI_ShapeArea * 0.3861022, "#0.00")

        txtRefArea.Text = Format(AOI_ReferenceArea, "#0.00")
        lblRefUnit.Text = AOI_ReferenceUnit

        'Get DEM statistics
        Dim pRasterStats As IRasterStatistics = Nothing
        Dim fullfilepath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        pRasterStats = BA_GetDemStatsGDB(AOIFolderBase)

        If pRasterStats IsNot Nothing Then
            txtMinElev.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
            txtMaxElev.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
            txtRangeElev.Text = Math.Round((pRasterStats.Maximum - pRasterStats.Minimum) + 0.005, 2)
            pRasterStats = Nothing
        Else
            MsgBox("Unable to read the filled DEM! The AOI surface.gdb is corrupted.")
        End If

        'check Snotel, snow course, and PRISM data
        'reset their selected checkboxes
        ChkPRISMSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkSnowCourseSelected.Checked = False
        CmdReClip.Enabled = False

        'PRISM
        Dim temppathname As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\" & AOIPrismFolderNames.annual.ToString
        If Not BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            ChkPRISMExist.Checked = False
        Else
            ChkPRISMExist.Checked = True
            Dim depthUnits As MeasurementUnit = BA_GetDepthUnit(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism), AOIPrismFolderNames.annual.ToString)
            Select Case depthUnits
                Case MeasurementUnit.Inches
                    rbtnDepthInch.Checked = True
                Case MeasurementUnit.Millimeters
                    rbtnDepthMM.Checked = True
            End Select
            Dim dblPrismBuffer As Double = BA_GetBufferDistance(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism), _
                                                    AOIPrismFolderNames.annual.ToString, esriDatasetType.esriDTRasterDataset)
            If dblPrismBuffer > 0 Then _
                BA_PRISMClipBuffer = dblPrismBuffer
            txtPrismBufferDist.Text = CStr(BA_PRISMClipBuffer)
            ' This variable keeps track of whether the PRISM clip buffer is changed; If changed, we need to recreate p_aoi_v and p_aoi
            m_PRISMClipBuffer = BA_PRISMClipBuffer
        End If

        'SNOTEL
        temppathname = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SNOTELSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSNOTELExist.Checked = True
            Dim dblSnotelBuffer As Double = BA_GetBufferDistance(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), _
                                                     BA_SNOTELSites, esriDatasetType.esriDTFeatureClass)
            If dblSnotelBuffer > 0 Then _
                BA_SnotelClipBuffer = dblSnotelBuffer
            TxtSnotelBuffer.Text = CStr(BA_SnotelClipBuffer)
        Else
            ChkSNOTELExist.Checked = False
            TxtSnotelBuffer.Text = Nothing
        End If

        'Snow Courses
        temppathname = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SnowCourseSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSnowCourseExist.Checked = True
            Dim dblSCBuffer As Double = BA_GetBufferDistance(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), _
                                                 BA_SnowCourseSites, esriDatasetType.esriDTFeatureClass)
            If dblSCBuffer > 0 Then _
                BA_SnowCourseClipBuffer = dblSCBuffer
            TxtSnowCourseBuffer.Text = CStr(BA_SnowCourseClipBuffer)
        Else
            ChkSnowCourseExist.Checked = False
            TxtSnowCourseBuffer.Text = Nothing
        End If

        Dim aoiName As String = BA_GetBareName(AOIFolderBase)
        m_aoi = New Aoi(aoiName, AOIFolderBase, Nothing, m_version)
        'update caption
        Me.Text = "AOI: " & aoiName
        LoadLstLayers()
    End Sub

    Private Sub CmdReClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdReClip.Click
        'read the basinanalyst.def file to get the path/name of the required input layers
        'the value should be available in BA_SystemSettings

        Dim response As Short
        Dim j As Integer
        Dim DataName As String

        'Validation
        If ChkPRISMSelected.Checked = True Then
            If Not IsNumeric(txtPrismBufferDist.Text) Then
                MessageBox.Show("Numeric value required for Prism buffer distance!", "BAGIS")
                txtPrismBufferDist.Focus()
                Exit Sub
            End If
            If Len(txtPrismBufferDist.Text) > 0 Then
                BA_PRISMClipBuffer = Val(txtPrismBufferDist.Text)
            End If
        End If
        If ChkSNOTELSelected.Checked = True Then
            If Not IsNumeric(TxtSnotelBuffer.Text) Then
                MessageBox.Show("Numeric value required for SNOTEL buffer distance!", "BAGIS")
                TxtSnotelBuffer.Focus()
                Exit Sub
            End If
            If Len(TxtSnotelBuffer.Text) > 0 Then
                BA_SnotelClipBuffer = Val(TxtSnotelBuffer.Text)
            End If
        End If
        If ChkSnowCourseSelected.Checked = True Then
            If Not IsNumeric(TxtSnowCourseBuffer.Text) Then
                MessageBox.Show("Numeric value required for Snow Course buffer distance!", "BAGIS")
                TxtSnowCourseBuffer.Focus()
                Exit Sub
            End If
            If Len(TxtSnowCourseBuffer.Text) > 0 Then
                BA_SnowCourseClipBuffer = Val(TxtSnowCourseBuffer.Text)
            End If
        End If


        'remove all layers of the AOI from the data frame
        BA_SetSettingPath()
        BA_ReadBAGISSettings(BA_Settings_Filepath)
        response = BA_RemoveLayersInFolder(My.ArcMap.Document, AOIFolderBase)

        Dim nstep As Integer = 5
        'delete the files/folders that need to be re-created
        Dim prismgdbpath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        If ChkPRISMExist.Checked = True And ChkPRISMSelected.Checked = True Then 'delete PRISM Folder
            'response = BA_Remove_Folder(BA_GetPath(AOIFolderBase, GeodatabaseNames.Prism))
            If BA_Workspace_Exists(prismgdbpath) Then
                Dim gdbSuccess As BA_ReturnCode = BA_DeleteGeodatabase(prismgdbpath, My.ArcMap.Document)
                If gdbSuccess <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to delete Geodatabase '" & prismgdbpath & "'. Please restart ArcMap and try again", "Unable to delete Geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
                nstep = nstep + 1
            End If
        End If

        If ChkSNOTELExist.Checked = True And ChkSNOTELSelected.Checked = True Then 'backup or delete SNOTEL file
            'Backup the original snotel layer if it doesn't exist; Will only do this once per AOI
            If Not BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) + BA_Orig_SNOTELSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Dim success As BA_ReturnCode = BA_RenameFeatureClassInGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), BA_SNOTELSites, BA_Orig_SNOTELSites)
            Else
                response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), BA_SNOTELSites)
            End If
            'Delete old clip layer, if it existed
            If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + BA_SnotelClipAoi, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), BA_SnotelClipAoi)
            End If
            ChkSNOTELExist.Checked = False
            nstep = nstep + 1
        End If

        If ChkSnowCourseExist.Checked = True And ChkSnowCourseSelected.Checked = True Then 'backup or delete Snow Course file
            'Backup the original snow course layer if it doesn't exist; Will only do this once per AOI
            If Not BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) + BA_Orig_SnowCourseSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Dim success As BA_ReturnCode = BA_RenameFeatureClassInGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), BA_SnowCourseSites, BA_Orig_SnowCourseSites)
            Else
                response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), BA_SnowCourseSites)
            End If
            'Delete old clip layer, if it existed
            If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + BA_SnowCourseClipAoi, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), BA_SnowCourseClipAoi)
            End If
            ChkSnowCourseExist.Checked = False
            nstep = nstep + 1
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, nstep)
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping selected layers ", "Clipping...")
        pStepProg.Show()
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()
        Dim sb As StringBuilder = New StringBuilder
        Dim sbErrorMessage As StringBuilder = New StringBuilder

        'regenerate the files/folders
        If ChkPRISMSelected.Checked = True Then 'Clip PRISM data
            'set the PRISM folder variables: PRISMLayer()
            'BA_SetPRISMFolderNames()

            'create the PRISM Geodatabase
            Dim gdbName As String = BA_EnumDescription(GeodatabaseNames.Prism)
            Dim success As BA_ReturnCode = BA_CreateFileGdb(AOIFolderBase, gdbName)
            Dim InPRISMPath As String = BA_SystemSettings.PRISMFolder

            'Make sure units are selected
            If Not rbtnDepthInch.Checked And _
                Not rbtnDepthMM.Checked Then
                MessageBox.Show("Depth units for PRISM layers are required. Please select depth units.", "Missing units", _
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                rbtnDepthInch.Focus()
                Exit Sub
            End If

            'PRISM
            If String.IsNullOrEmpty(Trim(InPRISMPath)) Then
                sbErrorMessage.Append("Error: PRISM data source is not defined! Please use the Options dialog to define the data source." + vbCrLf)
            Else
                Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(InPRISMPath)
                Dim prismServices As System.Array = Nothing
                Dim prismExists As Boolean = False
                If wType = WorkspaceType.ImageServer Then
                    Dim tempPathName As String = InPRISMPath & "/" & PrismServiceNames.Prism_Precipitation_q4.ToString & _
                        "/" & BA_Url_ImageServer
                    prismExists = BA_File_ExistsImageServer(tempPathName)
                    prismServices = System.Enum.GetValues(GetType(PrismServiceNames))
                Else
                    Dim tempPathName As String = InPRISMPath & "\Q4\grid"
                    prismExists = BA_Workspace_Exists(tempPathName)
                End If
                If prismExists Then
                    Dim havePRISMClipLayer As Boolean = True
                    If BA_PRISMClipBuffer <> m_PRISMClipBuffer Then
                        'PRISM buffer changed, we need to recreate prism template files
                        success = ReclipPrismAoiFiles()
                        If success <> BA_ReturnCode.Success Then
                            sbErrorMessage.Append("Error: Unable to generate PRISM clip layer!" + vbCrLf)
                            havePRISMClipLayer = False
                         End If
                    End If
                    'there are 17 prism rasters to be clipped
                    If havePRISMClipLayer = True Then
                        BA_SetPRISMFolderNames()
                        For j = 0 To 16
                            DataName = PRISMLayer(j)
                            pStepProg.Step()
                            System.Windows.Forms.Application.DoEvents()

                            If wType = WorkspaceType.ImageServer Then
                                Dim clipFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.PrismClipAOIExtentCoverage)
                                Dim webServiceUrl As String = InPRISMPath & "/" & prismServices(j).ToString & _
                                     "/" & BA_Url_ImageServer
                                Dim newFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True) & DataName
                                response = BA_ClipAOIImageServer(AOIFolderBase, webServiceUrl, newFilePath, AOIClipFile.PrismClipAOIExtentCoverage)
                            Else
                                'input PRISM raster is in GRID format, output is in FGDB format 
                                Dim outputFolder As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
                                response = BA_ClipAOIRaster(AOIFolderBase, InPRISMPath & "\" & DataName & "\grid", DataName, outputFolder, AOIClipFile.PrismClipAOIExtentCoverage)
                            End If

                            If response <= 0 Then
                                sbErrorMessage.Append("Error: PRISM Clipping " & DataName & " failed! Return value = " & response & "." + vbCrLf)
                            End If
                        Next

                        'update the Z unit metadata of PRISM
                        'We need to update the depth units on new PRISM layers
                        Dim inputFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
                        Dim inputFile As String = AOIPrismFolderNames.annual.ToString

                        Dim unitText As String = BA_EnumDescription(MeasurementUnit.Inches)
                        If rbtnDepthMM.Checked Then
                            unitText = BA_EnumDescription(MeasurementUnit.Millimeters)
                        End If

                        sb.Append(BA_BAGIS_TAG_PREFIX)
                        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString & "; ")
                        sb.Append(BA_ZUNIT_VALUE_TAG & unitText & ";")
                        'Record buffer distance and units
                        sb.Append(BA_BUFFER_DISTANCE_TAG + CStr(BA_PRISMClipBuffer) + "; ")
                        sb.Append(BA_XUNIT_VALUE_TAG + txtPrismMeters.Text + ";")
                        sb.Append(BA_BAGIS_TAG_SUFFIX)
                        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                                          sb.ToString, BA_BAGIS_TAG_PREFIX.Length)

                        Me.ChkPRISMExist.Checked = True
                    End If
                Else
                    sbErrorMessage.Append("Error: The specified PRISM data source is missing! Please verify the data source information in the Options dialog." + vbCrLf)
                End If
            End If
        End If

        Dim InLayerString As String
        Dim LayerPath As String = ""
        Dim LayerName As String = "", strExtension As String = ""

        'SNOTEL
        If ChkSNOTELSelected.Checked = True Then 'clip SNOTEL data
            InLayerString = BA_SystemSettings.SNOTELLayer
            If String.IsNullOrEmpty(Trim(InLayerString)) Then
                sbErrorMessage.Append("Error: SNOTEL data source is not defined! Please use the Options dialog to define the data source." + vbCrLf)
            Else
                Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(InLayerString)
                Dim snotelExists As Boolean = False
                If wType = WorkspaceType.Raster Then
                    LayerName = BA_GetBareNameAndExtension(InLayerString, LayerPath, strExtension)
                    snotelExists = BA_Shapefile_Exists(LayerPath & LayerName)
                ElseIf wType = WorkspaceType.FeatureServer Then
                    snotelExists = BA_File_ExistsFeatureServer(InLayerString)
                End If

                If snotelExists Then
                    Dim snotelClipLayer As String = GetSnoClipLayer(True)
                    If String.IsNullOrEmpty(snotelClipLayer) Then
                        sbErrorMessage.Append("Error: Unable to generate SNOTEL clip layer!" + vbCrLf)
                    Else
                        If wType = WorkspaceType.Raster Then
                            response = BA_ClipAOISNOTEL(AOIFolderBase, LayerPath & LayerName, True, snotelClipLayer)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            response = BA_ClipAOISnoWebServices(AOIFolderBase, InLayerString, True, snotelClipLayer)
                        End If
                        If response <> 1 Then
                            Select Case response
                                Case -1 '-1: unknown error
                                    sbErrorMessage.Append("Error: Unable to clip the SNOTEL layer to the AOI!" + vbCrLf)
                                Case -2 '-2: output exists
                                    sbErrorMessage.Append("Error: Output SNOTEL target layer exists in the AOI. Unable to clip data to AOI!" + vbCrLf)
                                Case -3 '-3: missing parameters
                                    sbErrorMessage.Append("Error: Missing SNOTEL clipping parameters. Unable to clip data to AOI!" + vbCrLf)
                                Case -4 '-4: no input shapefile
                                    sbErrorMessage.Append("Error: Missing the SNOTEL clipping shapefile. Unable to clip data to AOI!" + vbCrLf)
                                Case 0 '0: no intersect between the input and the clip layers
                                    sbErrorMessage.Append("Warning: There are no SNOTEL sites within the AOI. The output SNOTEL layer was not created." + vbCrLf)
                            End Select
                        Else
                            Me.ChkSNOTELExist.Checked = True
                            'Record buffer units in metadata if snotel layer exists
                            If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) + BA_SNOTELSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                                sb.Clear()
                                sb.Append(BA_BAGIS_TAG_PREFIX)
                                sb.Append(BA_BUFFER_DISTANCE_TAG + CStr(BA_SnotelClipBuffer) + "; ")
                                sb.Append(BA_XUNIT_VALUE_TAG + txtSnotelMeters.Text + ";")
                                sb.Append(BA_BAGIS_TAG_SUFFIX)
                                BA_UpdateMetadata(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), BA_SNOTELSites, LayerType.Vector, BA_XPATH_TAGS, _
                                                  sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
                            End If
                            pStepProg.Step()
                            System.Windows.Forms.Application.DoEvents()
                        End If
                    End If

                Else
                    sbErrorMessage.Append("Error: The specified SNOTEL data source is missing! Please verify the data source information in the Options dialog." + vbCrLf)
                End If
            End If
        End If

        If ChkSnowCourseSelected.Checked = True Then 'clip Snow Course data
            InLayerString = BA_SystemSettings.SCourseLayer

            If String.IsNullOrEmpty(Trim(InLayerString)) Then
                sbErrorMessage.Append("Error: Snow Course data source is not defined! Please use the Options dialog to define the data source." + vbCrLf)
            Else
                Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(InLayerString)
                Dim scExists As Boolean = False
                If wType = WorkspaceType.Raster Then
                    LayerName = BA_GetBareNameAndExtension(InLayerString, LayerPath, strExtension)
                    scExists = BA_Shapefile_Exists(LayerPath & LayerName)
                ElseIf wType = WorkspaceType.FeatureServer Then
                    scExists = BA_File_ExistsFeatureServer(InLayerString)
                End If

                If scExists Then
                    Dim scClipLayer As String = GetSnoClipLayer(False)
                    If String.IsNullOrEmpty(scClipLayer) Then
                        sbErrorMessage.Append("Error: Unable to generate Snow Course clip layer!" + vbCrLf)
                    Else
                        If wType = WorkspaceType.Raster Then
                            response = BA_ClipAOISNOTEL(AOIFolderBase, LayerPath & LayerName, False, scClipLayer)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            response = BA_ClipAOISnoWebServices(AOIFolderBase, InLayerString, False, scClipLayer)
                        End If
                        If response <> 1 Then
                            Select Case response
                                Case -1 '-1: unknown error
                                    sbErrorMessage.Append("Error: Unable to clip the Snow Course layer to the AOI!" + vbCrLf)
                                Case -2 '-2: output exists
                                    sbErrorMessage.Append("Error: Output Snow Course target layer exists in the AOI. Unable to clip data to AOI!" + vbCrLf)
                                Case -3 '-3: missing parameters
                                    sbErrorMessage.Append("Error: Missing Snow Course clipping parameters. Unable to clip data to AOI!" + vbCrLf)
                                Case -4 '-4: no input shapefile
                                    sbErrorMessage.Append("Error: Missing the Snow Course clipping shapefile. Unable to clip data to AOI!" + vbCrLf)
                                Case 0 '0: no intersect between the input and the clip layers
                                    sbErrorMessage.Append("Warning: There are no Snow Course sites within the AOI. The output Snow Course layer was not created." + vbCrLf)
                            End Select
                        Else
                            Me.ChkSnowCourseExist.Checked = True
                            'Record buffer units in metadata if snow course layer exists
                            If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True) + BA_SnowCourseSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                                sb.Clear()
                                sb.Append(BA_BAGIS_TAG_PREFIX)
                                sb.Append(BA_BUFFER_DISTANCE_TAG + CStr(BA_SnowCourseClipBuffer) + "; ")
                                sb.Append(BA_XUNIT_VALUE_TAG + TxtSnowCourseMeters.Text + ";")
                                sb.Append(BA_BAGIS_TAG_SUFFIX)
                                BA_UpdateMetadata(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), BA_SnowCourseSites, LayerType.Vector, BA_XPATH_TAGS, _
                                                  sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
                            End If
                        End If
                        pStepProg.Step()
                        System.Windows.Forms.Application.DoEvents()
                    End If
                Else
                    sbErrorMessage.Append("Error: The specified Snow Course data source is missing! Please verify the data source information in the Options dialog." + vbCrLf)
                End If
            End If
        End If

        pStepProg.Hide()
        pStepProg = Nothing
        progressDialog2.HideDialog()
        progressDialog2 = Nothing

        If sbErrorMessage.Length < 1 Then
            MessageBox.Show("Re-clipping layer(s) completed!", "BAGIS", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Re-clipping layer(s) completed with the following warnings: " + vbCrLf + vbCrLf + sbErrorMessage.ToString, _
                            "BAGIS", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        ChkSnowCourseSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkPRISMSelected.Checked = False
    End Sub

    Private Sub LstRasters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstRasters.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstVectors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstVectors.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub ManageAoiLayerButtons()
        Dim lCount As Integer = LstRasters.SelectedItems.Count + _
                                    LstVectors.SelectedItems.Count

        If lCount > 0 Then
            CmdClearSelected.Enabled = True
            CmbAddSelectionsToMap.Enabled = True
            'Dim m_lstHruLayersItem As LayerListItem = Nothing
            'If m_lstHruLayersItem IsNot Nothing Then
            '    Panel1.Enabled = True
            'End If
        Else
            CmdClearSelected.Enabled = False
            CmbAddSelectionsToMap.Enabled = False
            'Panel1.Enabled = False
        End If
    End Sub

    Private Sub CmbAddSelectionsToMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbAddSelectionsToMap.Click
        Try
            Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
            ' Display raster layers
            If LstRasters.SelectedIndex > -1 Then
                Dim items As IList = LstRasters.SelectedItems
                For Each item As LayerListItem In items
                    If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
                        BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, _
                                                   symbology.DisplayStyle, symbology.Transparency, WorkspaceType.Geodatabase)
                    Else
                        BA_DisplayRaster(My.ArcMap.Application, item.Value, "", 1)
                    End If
                Next
            End If
            ' Display vector layers
            If LstVectors.SelectedIndex > -1 Then
                Dim items As IList = LstVectors.SelectedItems
                For Each item As LayerListItem In items
                    Dim strFileName As String = BA_GetBareName(item.Value)
                    If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                    Else
                        BA_DisplayVector(My.ArcMap.Document, item.Value, "", 1)
                    End If
                Next
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to view one or more of the layers you requested.", "Error", MessageBoxButtons.OK)
            Debug.Print("AOI path: " & m_aoi.FilePath)
            Debug.Print("AOI path length: " & m_aoi.FilePath.Length)
            Debug.Print("BtnViewAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub ChkPRISMSelected_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPRISMSelected.CheckedChanged
        If ChkPRISMSelected.Checked Or ChkSNOTELSelected.Checked Or ChkSnowCourseSelected.Checked = True Then
            CmdReClip.Enabled = True
        Else
            CmdReClip.Enabled = False
        End If
        grpboxPRISMUnit.Enabled = ChkPRISMSelected.Checked
        rbtnDepthInch.Enabled = ChkPRISMSelected.Checked
        rbtnDepthMM.Enabled = ChkPRISMSelected.Checked
        txtPrismBufferLbl.Enabled = ChkPRISMSelected.Checked
        txtPrismBufferDist.Enabled = ChkPRISMSelected.Checked
        txtPrismMeters.Enabled = ChkPRISMSelected.Checked
        txtDepthUnit.Enabled = ChkPRISMSelected.Checked
        'Populate buffer textbox with default values
        If ChkPRISMSelected.Checked AndAlso String.IsNullOrEmpty(txtPrismBufferDist.Text) Then
            txtPrismBufferDist.Text = CStr(BA_PRISMClipBuffer)
        End If
    End Sub

    Private Sub ChkSNOTELSelected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSNOTELSelected.CheckedChanged
        If ChkSNOTELSelected.Checked Or ChkPRISMSelected.Checked Or ChkSnowCourseSelected.Checked = True Then
            CmdReClip.Enabled = True
            'txtSiteBufferD.Enabled = True
        Else
            CmdReClip.Enabled = False
            'txtSiteBufferD.Enabled = False
        End If
        TxtSnotelBuffer.Enabled = ChkSNOTELSelected.Checked
        TxtSnotelClipDescr.Enabled = ChkSNOTELSelected.Checked
        txtSnotelMeters.Enabled = ChkSNOTELSelected.Checked
        'Populate buffer textbox with default values
        If ChkSNOTELSelected.Checked AndAlso String.IsNullOrEmpty(TxtSnotelBuffer.Text) Then
            TxtSnotelBuffer.Text = CStr(BA_SnotelClipBuffer)
        End If
    End Sub

    Private Sub ChkSnowCourseSelected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSnowCourseSelected.CheckedChanged
        If ChkSnowCourseSelected.Checked Or ChkPRISMSelected.Checked Or ChkSNOTELSelected.Checked = True Then
            CmdReClip.Enabled = True
        Else
            CmdReClip.Enabled = False
        End If
        TxtSnowCourseBuffer.Enabled = ChkSnowCourseSelected.Checked
        TxtSnowCourseClipDescr.Enabled = ChkSnowCourseSelected.Checked
        TxtSnowCourseMeters.Enabled = ChkSnowCourseSelected.Checked
        'Populate buffer textbox with default values
        If ChkSnowCourseSelected.Checked AndAlso String.IsNullOrEmpty(TxtSnowCourseBuffer.Text) Then
            TxtSnowCourseBuffer.Text = CStr(BA_SnowCourseClipBuffer)
        End If
    End Sub

    Private Sub LoadLstLayers()
        Dim ShapefileCount As Long, RasterCount As Long
        Dim i As Long

        ' Create/configure a step progressor
        Dim stepCount As Short = 6  'Loading 4 dropdowns
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading list layers", "loading...")

        Try
            LstVectors.Items.Clear()
            LstRasters.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            pStepProg.Step()

            'display shapefiles
            ShapefileCount = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstVectors.Items.Add(item)
                Next
            End If

            pStepProg.Step()

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstRasters.Items.Add(item)
                Next
            End If
            pStepProg.Step()

        Catch ex As Exception
            MessageBox.Show("LoadLstLayers() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    'Private Sub frmAOIInfo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
    '    LstRasters.Items.Clear()
    '    LstVectors.Items.Clear()

    '    Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
    '    BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)

    '    'display shapefiles
    '    Dim ShapefileCount As Long
    '    ShapefileCount = UBound(AOIVectorList)
    '    If ShapefileCount > 0 Then
    '        For i = 1 To ShapefileCount
    '            ' Vectors are always discrete
    '            Dim isDiscrete As Boolean = True
    '            Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
    '            Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
    '            LstVectors.Items.Add(item)
    '        Next
    '    End If


    '    'display raster layers
    '    Dim RasterCount As Long
    '    RasterCount = UBound(AOIRasterList)
    '    If RasterCount > 0 Then
    '        For i = 1 To RasterCount
    '            Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
    '            Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
    '            Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
    '            LstRasters.Items.Add(item)
    '        Next
    '    End If
    'End Sub







    'Back up for BA_ClipAOIRaster
    'Public Function BA_ClipAOIRaster(ByVal AOIFolder As String, ByVal InputRaster As String, ByVal OutputRasterName As String, _
    '                                 ByVal gdbName As GeodatabaseNames, ByVal AOIClipKey As AOIClipFile) As Short
    '    'prepare for data clipping
    '    'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
    '    Dim return_value As Short = 0
    '    Dim Data_Path As String = ""
    '    Dim Data_Name As String
    '    Dim OutputName As String
    '    Dim ClipShapeFile As String = Nothing
    '    Dim pClipFCursor As IFeatureCursor
    '    Dim pClipFeature As IFeature
    '    Dim pGeo As IGeometry
    '    Dim pAOIEnvelope As IEnvelope
    '    Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
    '    Dim tool As ESRI.ArcGIS.DataManagementTools.Clip = New ESRI.ArcGIS.DataManagementTools.Clip()

    '    If String.IsNullOrEmpty(InputRaster) Then
    '        Return -3
    '    End If

    '    Data_Name = BA_GetBareName(InputRaster, Data_Path)

    '    Dim pClipFeatureLayer As IFeatureLayer = New FeatureLayer
    '    Dim pClipFClass As IFeatureClass

    '    Try
    '        ClipShapeFile = BA_EnumDescription(AOIClipKey)

    '        'Get gdb Workspace path added by Momeni
    '        Dim outputFolder As String = AOIFolder & "\" & BA_EnumDescription(gdbName)

    '        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(outputFolder)
    '        If Not BA_Folder_ExistsWindowsIO(outputFolder) Then
    '            Throw New Exception("Output geodatabase folder " + outputFolder + " does not exist.")
    '        End If
    '        If String.IsNullOrEmpty(OutputRasterName) Then 'user didn't specify an output name
    '            OutputName = outputFolder & "\" & Data_Name
    '        Else

    '            If workspaceType = BAGIS_ClassLibrary.WorkspaceType.Raster And OutputRasterName.Length > BA_GRID_NAME_MAX_LENGTH Then
    '                Throw New Exception("Output raster name cannot exceed " + CStr(BA_GRID_NAME_MAX_LENGTH) + " characters")
    '            End If
    '            OutputName = outputFolder & "\" & OutputRasterName
    '        End If

    '        'check if a layer of the same name exists in the AOI
    '        If BA_File_Exists(OutputName, workspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
    '            Return -2
    '        End If

    Private Sub CmdAddLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAddLayer.Click
        'MsgBox("Test")

        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String = "", Data_Name As String, data_type As Object
        Dim data_type_code As Integer '1. shapefile, 2. Raster, 0. Unsupported format
        Dim data_fullname As String
        Dim importDone As Boolean = False

        Dim pFilter As IGxObjectFilter = New GxFilterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select a GIS dataset to add to AOI"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        Select Case data_type
            Case 4, 5 'shapefile
                data_type_code = 1
            Case 12, 13 'raster
                data_type_code = 2
            Case Else 'unsupported format
                data_type_code = 0
        End Select

        'pad a backslash to the path if it doesn't have one.
        'If String.(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name
        If String.IsNullOrEmpty(data_fullname) Then Exit Sub 'user cancelled the action

        'allow user to specify a different output name
        Dim outlayername As String
        outlayername = InputBox("Set output layer name (please don't use space in the name):", "Clip Layer to AOI", Data_Name)

        If String.IsNullOrEmpty(outlayername) Then 'user cancelled the action
            Exit Sub
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        If data_type_code = 1 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Adding the vector layer ", "Adding...")
        ElseIf data_type_code = 2 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Adding the raster layer ", "Adding...")
        End If
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Step()

        'check if a layer of the same name exist
        Try
            Dim Layer_Exist As Boolean = False
            If data_type_code = 1 Then 'shapefile
                For i As Integer = 0 To LstVectors.Items.Count - 1
                    If LstVectors.Items(i).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next

                'check if the selected layer is raster then check if the chosen name exists in the raster list or not
            ElseIf data_type_code = 2 Then
                For i As Integer = 0 To LstRasters.Items.Count - 1
                    If LstRasters.Items(i).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next
            Else
                MsgBox("The data type of " & Data_Name & " is not supported. No layer was added to the AOI.")
                Exit Sub
            End If

            If Layer_Exist Then
                'MsgBox(outlayername & " already exists in the AOI! Please Choose another name.")
                Throw New Exception(vbCrLf + outlayername + " is already exists")
            End If
            'confirm the selection
            'response = MsgBox("Clip " & Data_Name & " to the AOI?" & vbCrLf & "Output: " & outlayername, vbYesNo)
            'If response = vbNo Then Exit Sub

            'prepare for data clipping
            Dim response As Integer
            Dim outputFolder As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            If data_type_code = 1 Then 'clip shapefile
                response = BA_ClipAOIVector(m_aoi.FilePath, data_fullname, outlayername, outputFolder, True)
                If response <= 0 Then
                    Throw New Exception(vbCrLf + "Vector clipping failed! Return value = " & response & ".")
                End If
                pStepProg.Step()

                'Add vector to the list for display
                Dim fullLayerPath As String = Data_Path & outlayername
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Vector, True)
                LstVectors.Items.Add(item)

            ElseIf data_type_code = 2 Then 'raster clip
                Dim clipKey As AOIClipFile = 1
                response = BA_ClipAOIRaster(m_aoi.FilePath, data_fullname, outlayername, outputFolder, clipKey)
                If response <= 0 Then
                    Throw New Exception(vbCrLf + "Raster Clipping failed! Return value = " & response & ".")
                    Exit Sub
                End If
                importDone = True
                pStepProg.Step()
                'Add Raster to the list for display
                Dim fullLayerPath As String = Data_Path & outlayername
                Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Raster, isDiscrete)
                LstRasters.Items.Add(item)
            End If
            frmAOIInfo_Load(Me, Nothing)
            MsgBox("Clipping done!")

        Catch ex As Exception
            MessageBox.Show("Add new layer exception: " & ex.Message)
        Finally

            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub txtPrismBufferD_DoubleClick(sender As Object, e As System.EventArgs) Handles txtPrismBufferLbl.DoubleClick
        Dim response As String
        response = InputBox("Please enter a PRISM buffer distance in meters", "Set PRISM Buffer Distance", BA_PRISMClipBuffer)
        If Not IsNumeric(response) Then
            MsgBox("Numeric value required!")
            Exit Sub
        End If
        If Len(Trim(response)) > 0 Then
            BA_PRISMClipBuffer = Val(response)
            txtPrismBufferLbl.Text = "Clip Buffer Distance: " + response + " meters"
        End If
    End Sub

    Private Function ReclipPrismAoiFiles() As BA_ReturnCode
        'use Buffer GP to perform buffer and save the result as a shapefile
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim BufferTool As ESRI.ArcGIS.AnalysisTools.Buffer = New ESRI.ArcGIS.AnalysisTools.Buffer
        Try
            Dim aoiGdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi)
            With BufferTool
                .in_features = aoiGdbPath & "\" & BA_AOIExtentCoverage
                .buffer_distance_or_field = BA_PRISMClipBuffer
                .dissolve_option = "ALL"
                .out_feature_class = AOIFolderBase & "\" & BA_PRISMClipAOI & ".shp"
            End With
            GP.AddOutputsToMap = False
            GP.Execute(BufferTool, Nothing)

            'save the buffered AOI as a shapefile and then import it into the GDB
            'to prevent a bug when the buffer distance exceed the xy domain limits of the GDB
            'Copy the temporary line shape file to the aoi.gdb
            If BA_File_Exists(aoiGdbPath + "\" + BA_PRISMClipAOI, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                BA_Remove_ShapefileFromGDB(aoiGdbPath, BA_PRISMClipAOI)
            End If
            Dim success As BA_ReturnCode = BA_ConvertShapeFileToGDB(AOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, True, False), aoiGdbPath, BA_PRISMClipAOI)
            'create a raster version of the buffered AOI
            Dim DEMCellSize As Double = BA_CellSize(aoiGdbPath, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
            Dim snapRasterPath As String = aoiGdbPath + BA_EnumDescription(PublicPath.AoiGrid)
            success = BA_Feature2RasterGP(AOIFolderBase & BA_StandardizeShapefileName(BA_PRISMClipAOI, True, True), aoiGdbPath & BA_EnumDescription(PublicPath.AoiPrismGrid), "ID", DEMCellSize, snapRasterPath)
            BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, False))
            m_PRISMClipBuffer = BA_PRISMClipBuffer
            Return success
        Catch ex As Exception
            Debug.Print("ReclipPrismAoiFiles: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Private Function ReclipSnotelFiles(ByVal bufferDistance As Double, ByVal outputFeatureClassName As String, ByVal strUnits As String) As BA_ReturnCode
        Dim aoiGdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi)
        'Remove pre-existing file before we try to recreate
        If BA_File_Exists(aoiGdbPath + "\" + outputFeatureClassName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            BA_Remove_ShapefileFromGDB(aoiGdbPath, outputFeatureClassName)
        End If

        'use Buffer GP to perform buffer and save the result as a shapefile
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim BufferTool As ESRI.ArcGIS.AnalysisTools.Buffer = New ESRI.ArcGIS.AnalysisTools.Buffer
        Try
            With BufferTool
                .in_features = aoiGdbPath & "\" & BA_AOIExtentCoverage
                .buffer_distance_or_field = CStr(bufferDistance)
                .dissolve_option = "ALL"
                .out_feature_class = AOIFolderBase + "\" + outputFeatureClassName & ".shp"
            End With
            GP.AddOutputsToMap = False
            GP.Execute(BufferTool, Nothing)

            'save the buffered AOI as a shapefile and then import it into the GDB
            'to prevent a bug when the buffer distance exceed the xy domain limits of the GDB
            'Copy the temporary line shape file to the aoi.gdb
            Dim success As BA_ReturnCode = BA_ConvertShapeFileToGDB(AOIFolderBase, BA_StandardizeShapefileName(outputFeatureClassName, True, False), aoiGdbPath, outputFeatureClassName)
            If success = BA_ReturnCode.Success Then
                'Record buffer distance in metadata
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(BA_BAGIS_TAG_PREFIX)
                sb.Append(BA_BUFFER_DISTANCE_TAG + CStr(bufferDistance) + "; ")
                sb.Append(BA_XUNIT_VALUE_TAG + strUnits + ";")
                sb.Append(BA_BAGIS_TAG_SUFFIX)
                BA_UpdateMetadata(aoiGdbPath, outputFeatureClassName, LayerType.Vector, BA_XPATH_TAGS, _
                                  sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
            End If
            BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(outputFeatureClassName, False))
            Return success
        Catch ex As Exception
            Debug.Print("ReclipSnotelFiles: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Private Function GetSnoClipLayer(ByVal isSnotel As Boolean) As String
        '1. Is buffer distance the same as dem ? If so, we can use aoib_v
        Dim aoiBufferDistance As Double = BA_GetBufferDistance(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces),
                                                               BA_EnumDescription(MapsFileName.filled_dem_gdb), esriDatasetType.esriDTRasterDataset)
        If isSnotel = True Then
            If aoiBufferDistance = BA_SnotelClipBuffer Then
                Return m_snotelClipLayer
            End If
        Else
            If aoiBufferDistance = BA_SnowCourseClipBuffer Then
                Return m_snotelClipLayer
            End If
        End If

        If isSnotel = True Then
            Dim success As BA_ReturnCode = ReclipSnotelFiles(BA_SnotelClipBuffer, BA_SnotelClipAoi, txtSnotelMeters.Text)
            If success = BA_ReturnCode.Success Then
                m_snotelClipLayer = BA_SnotelClipAoi
                Return m_snotelClipLayer
            End If
        Else
            '2. Is buffer distance the same for snotel and snow course? If so, we can create the layer once for both
            If BA_SnowCourseClipBuffer = BA_SnotelClipBuffer Then
                If BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True) + BA_SnotelClipAoi, WorkspaceType.Geodatabase, _
                                  esriDatasetType.esriDTFeatureClass) Then
                    Return BA_SnotelClipAoi
                Else
                    ' May still need to create the clip layer if snotel clip layer creation failed
                    Dim success As BA_ReturnCode = ReclipSnotelFiles(BA_SnowCourseClipBuffer, BA_SnowCourseClipAoi, TxtSnowCourseMeters.Text)
                    If success = BA_ReturnCode.Success Then
                        Return BA_SnowCourseClipAoi
                    End If
                End If
            Else
                Dim success As BA_ReturnCode = ReclipSnotelFiles(BA_SnowCourseClipBuffer, BA_SnowCourseClipAoi, TxtSnowCourseMeters.Text)
                If success = BA_ReturnCode.Success Then
                    Return BA_SnowCourseClipAoi
                End If
            End If
        End If
        Return Nothing
    End Function

End Class