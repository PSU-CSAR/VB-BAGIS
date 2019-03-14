
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms

Public Class BtnAddRefLayers
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        BA_SetSettingPath()
        If Len(BA_Settings_Filepath) = 0 Then
            MsgBox("ERROR! BA_Read_Settings: Cannot retrieve the file path and name of the definition file.")
        Else
            Dim retVal As Integer = BA_ReadBAGISSettings(BA_Settings_Filepath)
            If retVal <> 1 Then
                MessageBox.Show("The BAGIS settings were not loaded successfully!!", "BAGIS")
            End If
        End If
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

        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pourpointRef)
        Dim layertype As String = ""
        Dim valid1 As Boolean = True
        If Not BA_GetWorkspaceTypeFromPath(BA_SystemSettings.PourPointLayer) = WorkspaceType.FeatureServer Then
            Dim ppointpath As String = "Please Return"
            Dim pplayername As String = BA_GetBareNameAndExtension(pourpointRef, ppointpath, layertype)
            pourpointRef = ppointpath & pplayername
        Else
            Dim checkedUrls As IDictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
            valid1 = BA_VerifyUrl(pourpointRef, checkedUrls)
        End If

        Dim FileExists As Boolean = False
        If valid1 Then
            If Not String.IsNullOrEmpty(pourpointRef) Then 'it's OK to not have a specified reference layer
                If wType = WorkspaceType.Raster Then
                    FileExists = BA_Shapefile_Exists(pourpointRef)
                ElseIf wType = WorkspaceType.FeatureServer Then
                    FileExists = BA_File_Exists(pourpointRef, wType, esriDatasetType.esriDTFeatureClass)
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

        Dim success As BA_ReturnCode = BA_SetDefaultProjection(My.ArcMap.Application)
        If success <> BA_ReturnCode.Success Then
            Exit Sub
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
