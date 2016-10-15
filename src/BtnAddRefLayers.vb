Imports ESRI.ArcGIS.Desktop.AddIns
Imports System.IO
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Carto

Public Class BtnAddRefLayers
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button
    Public settingsform As frmSettings = New frmSettings
    Public Sub New()
        BA_SetSettingPath()
        If Len(BA_Settings_Filepath) = 0 Then
            MsgBox("ERROR! BA_Read_Settings: Cannot retrieve the file path and name of the definition file.")
        End If
        Dim settings_message As String = BA_Read_Settings(settingsform)
        BA_SystemSettings.listCount = settingsform.lstLayers.Items.Count
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnClick()
        Dim terrainRef As String = BA_SystemSettings.Ref_Terrain
        Dim DrainageRef As String = BA_SystemSettings.Ref_Drainage
        Dim watershedRef As String = BA_SystemSettings.Ref_Watershed
        Dim pourpointRef As String = BA_SystemSettings.PourPointLayer
        Dim ppointpath As String = "Please Return"
        Dim layertype As String = ""
        Dim pplayername As String = BA_GetBareNameAndExtension(pourpointRef, ppointpath, layertype)
        pourpointRef = ppointpath & pplayername

        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pourpointRef)
        Dim checkedUrls As IDictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
        Dim valid1 As Boolean = BA_VerifyUrl(settingsform.txtGaugeStation.Text, checkedUrls)
        Dim FileExists As Boolean = False
        If valid1 Then
            If Not String.IsNullOrEmpty(settingsform.txtGaugeStation.Text) Then 'it's OK to not have a specified reference layer
                If wType = WorkspaceType.Raster Then
                    Dim File_Path As String = "PleaseReturn"
                    Dim File_Name As String = BA_GetBareNameAndExtension(settingsform.txtGaugeStation.Text, File_Path, layertype)
                    Dim TempPathName As String = File_Path & File_Name
                    FileExists = BA_Shapefile_Exists(TempPathName)
                ElseIf wType = WorkspaceType.FeatureServer Then
                    FileExists = BA_File_Exists(settingsform.txtGaugeStation.Text, wType, esriDatasetType.esriDTFeatureClass)
                End If
            End If
            If Not FileExists Then
                MsgBox("Pourpoint layer does not exist: " & pourpointRef)
                pourpointRef = ""
            End If
        Else
            MsgBox("Pourpoint layer does not exist: " & pourpointRef)
            pourpointRef = ""
        End If

        BA_LoadReferenceLayers(terrainRef, DrainageRef, watershedRef, pourpointRef)
        If String.IsNullOrEmpty(DrainageRef) And Not String.IsNullOrEmpty(watershedRef) And Not String.IsNullOrEmpty(terrainRef) Then
            MsgBox("No reference layer is specified in the settings")
        End If
        'Dim SaveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        'SaveAOIMXDButton.selectedProperty = True
    End Sub
    
    Protected Overrides Sub OnUpdate()
    End Sub

End Class
