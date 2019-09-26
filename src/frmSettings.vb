Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports System.Windows.Forms
Imports System.Text
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.GISClient
Imports System.Net

Public Class frmSettings

    Private Sub CmdSetTerrainRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetTerrainRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Terrain Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile
        pGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtTerrain.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSetDrainageRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetDrainageRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Drainage Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtDrainage.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSetWatershedRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetWatershedRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Watershed Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtWatershed.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSet10MDEM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSet10MDEM.Click
        Dim bObjectSelected As Boolean = True
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pGxObject As IEnumGxObject = Nothing

        Dim rasFilter As IGxObjectFilter = New GxFilterRasterDatasets
        filterCollection.AddFilter(rasFilter, True)
        Dim imageFilter As IGxObjectFilter = New GxFilterImageServers
        filterCollection.AddFilter(imageFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select 10 Meters DEM"
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category.Equals(BA_EnumDescription(GxFilterCategory.ImageService)) Then
            'get the url of the selected image service
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            txtDEM10.Text = agsObj.AGSServerObjectName.URL
        Else
            'get the name of the selected folder and file
            Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
            Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
            Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
            Dim data_fullname As String = Data_Path & pDatasetName.Name
            If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
            If BA_GetWorkspaceTypeFromPath(data_fullname) = WorkspaceType.Geodatabase Then
                ShowGeodatabaseErrorMessage("DEM data")
                Exit Sub
            End If
            txtDEM10.Text = data_fullname
        End If

        If Not String.IsNullOrEmpty(txtDEM10.Text) Then CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSet30MDEM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSet30MDEM.Click
        Dim bObjectSelected As Boolean = True
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pGxObject As IEnumGxObject = Nothing

        Dim rasFilter As IGxObjectFilter = New GxFilterRasterDatasets
        filterCollection.AddFilter(rasFilter, True)
        Dim imageFilter As IGxObjectFilter = New GxFilterImageServers
        filterCollection.AddFilter(imageFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select 30 Meters DEM"
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category.Equals(BA_EnumDescription(GxFilterCategory.ImageService)) Then
            'get the url of the selected image service
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            txtDEM30.Text = agsObj.AGSServerObjectName.URL
        Else
            'get the name of the selected folder and file
            Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
            Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
            Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
            Dim data_fullname As String = Data_Path & pDatasetName.Name
            If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
            If BA_GetWorkspaceTypeFromPath(data_fullname) = WorkspaceType.Geodatabase Then
                ShowGeodatabaseErrorMessage("DEM data")
                Exit Sub
            End If
            txtDEM30.Text = data_fullname
        End If

        If Not String.IsNullOrEmpty(txtDEM30.Text) Then CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSetGaugeLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetGaugeLayer.Click
        Dim bObjectSelected As Boolean
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pointFilter As IGxObjectFilter = New GxFilterPointFeatureClasses
        filterCollection.AddFilter(pointFilter, True)
        Dim featureServiceFilter As IGxObjectFilter = New GxFilterFeatureServers
        filterCollection.AddFilter(featureServiceFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)
        Dim NO_DATA_ITEM As String = "No data"

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Gauge Station Layer"
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category = BA_EnumDescription(GxFilterCategory.FeatureService) Then
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            Dim sName As IAGSServerObjectName = agsObj.AGSServerObjectName
            Dim url As String = agsObj.AGSServerObjectName.URL
            Dim propertySet As IPropertySet = agsObj.AGSServerObjectName.AGSServerConnectionName.ConnectionProperties()
            'Build the REST url
            Dim prefix As String = propertySet.GetProperty(BA_Property_RestUrl)
            'Extract the selected service information
            Dim idxServices As Integer = url.IndexOf(BA_Url_Services)
            Dim idxMapServer As Integer = url.IndexOf(BA_Url_MapServer)
            Dim serviceText As String = url.Substring(idxServices, idxMapServer - idxServices - 1)   'subtract 1 to avoid trailing /
            txtGaugeStation.Text = prefix & serviceText & BA_EnumDescription(PublicPath.FeatureServiceUrl)

            'elevation field
            CmboxStationAtt.Items.Clear()

            Dim fsFields As IList(Of FeatureServiceField) = BA_QueryAllFeatureServiceFieldNames(txtGaugeStation.Text)
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType <= esriFieldType.esriFieldTypeInteger Or _
                    fField.fieldType = esriFieldType.esriFieldTypeString Then
                    CmboxStationAtt.Items.Add(fField.alias)
                End If
            Next
            CmboxStationAtt.SelectedIndex = 0
            If CmboxStationAtt.Items.Count = 0 Then
                MsgBox("No valid attribute field in the attribute table! Please check data." & vbCrLf & txtGaugeStation.Text)
            End If

            'Area field
            ComboStationArea.Items.Clear()
            ComboStationArea.Items.Add(NO_DATA_ITEM)
            ComboStationArea.SelectedItem = NO_DATA_ITEM
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                    ComboStationArea.Items.Add(fField.alias)
                End If
            Next
        Else
            'get the name of the selected folder
            Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
            Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
            Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
            Dim Data_Name As String = pDatasetName.Name
            Dim data_type As esriDatasetType = pDatasetName.Type
            Dim data_type_name As String

            'Set Data Type Name from Data Type
            If data_type = esriDatasetType.esriDTFeatureDataset Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTFeatureClass Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTRasterDataset Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTRasterBand Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTTin Then
                data_type_name = " (Tin)"
            Else
                data_type_name = " (Cannot Clip)"
            End If

            'pad a backslash to the path if it doesn't have one.
            Data_Path = BA_StandardizePathString(Data_Path, True)

            Dim data_fullname As String = Data_Path & Data_Name & data_type_name
            If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
            If BA_GetWorkspaceTypeFromPath(data_fullname) = WorkspaceType.Geodatabase Then
                ShowGeodatabaseErrorMessage("Gauge station data")
                Exit Sub
            End If
            txtGaugeStation.Text = data_fullname

            'read the fields in the attribute table and add to CmboxStationAtt
            Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromFile(Data_Path, Data_Name)

            'get fields
            Dim pFields As IFields = pFeatClass.Fields
            Dim aField As IField
            Dim i As Integer, nfields As Integer, qType As Integer
            nfields = pFields.FieldCount

            'elevation field
            CmboxStationAtt.Items.Clear()
            For i = 0 To nfields - 1 'Selects only numerical data types
                aField = pFields.Field(i)
                qType = aField.Type
                If qType <= esriFieldType.esriFieldTypeInteger Or _
                    qType = esriFieldType.esriFieldTypeString Then
                    CmboxStationAtt.Items.Add(aField.Name)
                End If
            Next
            CmboxStationAtt.SelectedIndex = 0

            'Area field
            ComboStationArea.Items.Clear()
            ComboStationArea.Items.Add(NO_DATA_ITEM)
            ComboStationArea.SelectedItem = NO_DATA_ITEM
            For i = 1 To nfields 'Selects only string data types
                aField = pFields.Field(i - 1)
                qType = aField.Type
                If qType <= esriFieldType.esriFieldTypeDouble Then 'numeric data types
                    ComboStationArea.Items.Add(aField.Name)
                End If
            Next

            'Release ArcObjects
            aField = Nothing
            pFields = Nothing
            pFeatClass = Nothing
            pDatasetName = Nothing
            pGxDataset = Nothing
        End If

        If Not String.IsNullOrEmpty(txtGaugeStation.Text) Then CmdUndo.Enabled = True
        'set area unit to unknown
        ComboStation_Value.SelectedIndex = 0

        pGxObject = Nothing
        pGxDialog = Nothing
    End Sub

    Private Sub CmdSetSNOTEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetSNOTEL.Click
        Dim bObjectSelected As Boolean
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pGxObject As IEnumGxObject = Nothing
        Const NONE_ITEM As String = "None"

        Dim pointFilter As IGxObjectFilter = New GxFilterPointFeatureClasses
        filterCollection.AddFilter(pointFilter, True)
        Dim featureServiceFilter As IGxObjectFilter = New GxFilterFeatureServers
        filterCollection.AddFilter(featureServiceFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)


        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select SNOTEL Layer"
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category = BA_EnumDescription(GxFilterCategory.FeatureService) Then
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            Dim sName As IAGSServerObjectName = agsObj.AGSServerObjectName
            Dim url As String = agsObj.AGSServerObjectName.URL
            Dim propertySet As IPropertySet = agsObj.AGSServerObjectName.AGSServerConnectionName.ConnectionProperties()
            'Build the REST url
            Dim prefix As String = propertySet.GetProperty(BA_Property_RestUrl)
            'Extract the selected service information
            Dim idxServices As Integer = url.IndexOf(BA_Url_Services)
            Dim idxMapServer As Integer = url.IndexOf(BA_Url_MapServer)
            Dim serviceText As String = url.Substring(idxServices, idxMapServer - idxServices - 1)   'subtract 1 to avoid trailing /
            txtSNOTEL.Text = prefix & serviceText & BA_EnumDescription(PublicPath.FeatureServiceUrl)

            'elevation field
            ComboSNOTEL_Elevation.Items.Clear()
            Dim fsFields As IList(Of FeatureServiceField) = BA_QueryAllFeatureServiceFieldNames(txtSNOTEL.Text)
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                    ComboSNOTEL_Elevation.Items.Add(fField.alias)
                End If
            Next
            ComboSNOTEL_Elevation.SelectedIndex = 0

            'name field
            ComboSNOTEL_Name.Items.Clear()
            ComboSNOTEL_Name.Items.Add(NONE_ITEM)
            ComboSNOTEL_Name.SelectedItem = NONE_ITEM
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType = esriFieldType.esriFieldTypeString Then
                    ComboSNOTEL_Name.Items.Add(fField.alias)
                End If
            Next
        Else
            'get the name of the selected folder
            Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
            Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
            Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
            Dim Data_Name As String = pDatasetName.Name
            Dim data_type As esriDatasetType = pDatasetName.Type
            Dim data_type_name As String

            'Set Data Type Name from Data Type
            If data_type = esriDatasetType.esriDTFeatureDataset Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTFeatureClass Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTRasterDataset Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTRasterBand Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTTin Then
                data_type_name = " (Tin)"
            Else
                data_type_name = " (Cannot Clip)"
            End If

            'pad a backslash to the path if it doesn't have one.
            Data_Path = BA_StandardizePathString(Data_Path, True)

            Dim data_fullname As String = Data_Path & Data_Name & data_type_name
            If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
            If BA_GetWorkspaceTypeFromPath(data_fullname) = WorkspaceType.Geodatabase Then
                ShowGeodatabaseErrorMessage("SNOTEL data")
                Exit Sub
            End If
            txtSNOTEL.Text = data_fullname

            'read the fields in the attribute table and add to CmboxStationAtt
            Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromFile(Data_Path, Data_Name)

            'get fields
            Dim pFields As IFields = pFeatClass.Fields
            Dim aField As IField
            Dim i As Integer, nfields As Integer, qType As Integer
            nfields = pFields.FieldCount

            'elevation field
            ComboSNOTEL_Elevation.Items.Clear()
            For i = 0 To nfields - 1 'Selects only numerical data types
                aField = pFields.Field(i)
                qType = aField.Type
                If qType <= esriFieldType.esriFieldTypeDouble Then 'numerical data types
                    ComboSNOTEL_Elevation.Items.Add(aField.Name)
                End If
            Next
            ComboSNOTEL_Elevation.SelectedIndex = 0

            'name field
            ComboSNOTEL_Name.Items.Clear()
            ComboSNOTEL_Name.Items.Add(NONE_ITEM)
            ComboSNOTEL_Name.SelectedItem = NONE_ITEM
            For i = 1 To nfields 'Selects only string data types
                aField = pFields.Field(i - 1)
                qType = aField.Type
                If qType = esriFieldType.esriFieldTypeString Then 'string data types
                    ComboSNOTEL_Name.Items.Add(aField.Name)
                End If
            Next

            'Release ArcObjects
            aField = Nothing
            pFields = Nothing
            pFeatClass = Nothing
            pDatasetName = Nothing
            pGxDataset = Nothing
        End If

        If Not String.IsNullOrEmpty(txtSNOTEL.Text) Then CmdUndo.Enabled = True

        pGxObject = Nothing
        pGxDialog = Nothing
    End Sub

    Private Sub CmdSetSnowC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetSnowC.Click
        Dim bObjectSelected As Boolean
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pGxObject As IEnumGxObject = Nothing
        Const NONE_ITEM As String = "None"

        Dim pointFilter As IGxObjectFilter = New GxFilterPointFeatureClasses
        filterCollection.AddFilter(pointFilter, True)
        Dim featureServiceFilter As IGxObjectFilter = New GxFilterFeatureServers
        filterCollection.AddFilter(featureServiceFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Snow Course Layer"
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category = BA_EnumDescription(GxFilterCategory.FeatureService) Then
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            Dim sName As IAGSServerObjectName = agsObj.AGSServerObjectName
            Dim url As String = agsObj.AGSServerObjectName.URL
            Dim propertySet As IPropertySet = agsObj.AGSServerObjectName.AGSServerConnectionName.ConnectionProperties()
            'Build the REST url
            Dim prefix As String = propertySet.GetProperty(BA_Property_RestUrl)
            'Extract the selected service information
            Dim idxServices As Integer = url.IndexOf(BA_Url_Services)
            Dim idxMapServer As Integer = url.IndexOf(BA_Url_MapServer)
            Dim serviceText As String = url.Substring(idxServices, idxMapServer - idxServices - 1)   'subtract 1 to avoid trailing /
            txtSnowCourse.Text = prefix & serviceText & BA_EnumDescription(PublicPath.FeatureServiceUrl)

            'elevation field
            ComboSC_Elevation.Items.Clear()
            Dim fsFields As IList(Of FeatureServiceField) = BA_QueryAllFeatureServiceFieldNames(txtSnowCourse.Text)
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                    ComboSC_Elevation.Items.Add(fField.alias)
                End If
            Next
            ComboSC_Elevation.SelectedIndex = 0

            'name field
            ComboSC_Name.Items.Clear()
            ComboSC_Name.Items.Add(NONE_ITEM)
            ComboSC_Name.SelectedItem = NONE_ITEM
            For Each fField As FeatureServiceField In fsFields
                If fField.fieldType = esriFieldType.esriFieldTypeString Then
                    ComboSC_Name.Items.Add(fField.alias)
                End If
            Next
        Else
            'get the name of the selected folder
            Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
            Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
            Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
            Dim Data_Name As String = pDatasetName.Name
            Dim data_type As esriDatasetType = pDatasetName.Type
            Dim data_type_name As String

            'Set Data Type Name from Data Type
            If data_type = esriDatasetType.esriDTFeatureDataset Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTFeatureClass Then
                data_type_name = " (Shapefile)"
            ElseIf data_type = esriDatasetType.esriDTRasterDataset Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTRasterBand Then
                data_type_name = " (Raster)"
            ElseIf data_type = esriDatasetType.esriDTTin Then
                data_type_name = " (Tin)"
            Else
                data_type_name = " (Cannot Clip)"
            End If

            'pad a backslash to the path if it doesn't have one.
            Data_Path = BA_StandardizePathString(Data_Path, True)

            Dim data_fullname As String = Data_Path & Data_Name & data_type_name
            If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
            If BA_GetWorkspaceTypeFromPath(data_fullname) = WorkspaceType.Geodatabase Then
                ShowGeodatabaseErrorMessage("Snow course data")
                Exit Sub
            End If
            txtSnowCourse.Text = data_fullname

            'read the fields in the attribute table and add to CmboxStationAtt
            Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromFile(Data_Path, Data_Name)

            'get fields
            Dim pFields As IFields = pFeatClass.Fields
            Dim aField As IField
            Dim i As Integer, nfields As Integer, qType As Integer
            nfields = pFields.FieldCount

            'elevation field
            ComboSC_Elevation.Items.Clear()
            For i = 0 To nfields - 1 'Selects only numerical data types
                aField = pFields.Field(i)
                qType = aField.Type
                If qType <= esriFieldType.esriFieldTypeDouble Then 'numerical data types
                    ComboSC_Elevation.Items.Add(aField.Name)
                End If
            Next
            ComboSC_Elevation.SelectedIndex = 0

            'name field
            ComboSC_Name.Items.Clear()
            ComboSC_Name.Items.Add(NONE_ITEM)
            ComboSC_Name.SelectedItem = NONE_ITEM
            For i = 1 To nfields 'Selects only string data types
                aField = pFields.Field(i - 1)
                qType = aField.Type
                If qType = esriFieldType.esriFieldTypeString Then 'string data types
                    ComboSC_Name.Items.Add(aField.Name)
                End If
            Next

            'Release ArcObjects
            aField = Nothing
            pFields = Nothing
            pFeatClass = Nothing
            pDatasetName = Nothing
            pGxDataset = Nothing
        End If

        If Not String.IsNullOrEmpty(txtSnowCourse.Text) Then CmdUndo.Enabled = True

        pGxObject = Nothing
        pGxDialog = Nothing

    End Sub

    Private Sub CmdSetPrecip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetPrecip.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String = ""

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select PRISM Data Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            Dim pGxObj As IGxObject = pGxObject.Next
            If pGxObj.Category.Equals(BA_EnumDescription(GxFilterCategory.ArcGisServerFolder)) Then
                'get the url of the selected image service
                Dim agsFolder As IGxAGSFolder = CType(pGxObj, IGxAGSFolder)
                Dim propertySet As IPropertySet = agsFolder.AGSServerConnectionName.ConnectionProperties
                Dim sb As StringBuilder = New StringBuilder
                sb.Append(propertySet.GetProperty(BA_Property_SoapUrl)) 'Image services use the SOAP url
                sb.Append("/" & pGxObj.BaseName)    'PRISM
                Data_Path = sb.ToString
                Dim TempPathName As String = sb.ToString & "/" & PrismServiceNames.PRISM_Precipitation_Q4.ToString
                TempPathName = TempPathName & "/" & BA_Url_ImageServer
                If Not BA_File_ExistsImageServer(TempPathName) Then
                    txtPRISM.Text = ""
                Else
                    txtPRISM.Text = Data_Path
                End If
            Else
                'get the name of the selected folder and file
                Dim pGxDataFolder As IGxFile = CType(pGxObj, IGxFile)
                Data_Path = pGxDataFolder.Path
                Dim TempPathName As String = Data_Path & "\Q4\grid"
                If Len(Trim(Data_Path)) = 0 Then Exit Sub 'user cancelled the action
                If BA_GetWorkspaceTypeFromPath(Data_Path) = WorkspaceType.Geodatabase Then
                    ShowGeodatabaseErrorMessage("PRISM data")
                    Exit Sub
                End If
                If Not BA_Workspace_Exists(TempPathName) Then
                    txtPRISM.Text = ""
                Else
                    txtPRISM.Text = Data_Path
                End If
            End If

            If String.IsNullOrEmpty(txtPRISM.Text) Then
                MsgBox(Data_Path & " is not a valid PRISM data folder!")
            Else
                CmdUndo.Enabled = True
            End If

        Catch ex As Exception
            Debug.Print("CmdSetPrecip_Click Exception: " & ex.Message)
            MsgBox("Please select a folder containing PRISM data!" & vbCrLf & Data_Path & " is not a valid PRISM data folder!")
        End Try
    End Sub


    Private Sub CmdDisplayReferenceLayers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdDisplayReferenceLayers.Click
        'check if pourpoint file exists
        Dim pourpointRef As String = BA_SystemSettings.PourPointLayer
        If Not BA_GetWorkspaceTypeFromPath(BA_SystemSettings.PourPointLayer) = WorkspaceType.FeatureServer Then
            Dim ppointpath As String = "Please Return"
            Dim layertype As String = ""
            Dim pplayername As String = BA_GetBareNameAndExtension(pourpointRef, ppointpath, layertype)
            pourpointRef = ppointpath & pplayername
            If Len(pourpointRef) > 0 Then 'it's OK to not have a specified reference layer
                If Not BA_Shapefile_Exists(pourpointRef) Then
                    MsgBox("Pourpoint layer does not exist: " & pourpointRef)
                    pourpointRef = ""
                End If
            End If
        End If


        Dim success As BA_ReturnCode = BA_SetDefaultProjection(My.ArcMap.Application)
        If success <> BA_ReturnCode.Success Then
            Exit Sub
        End If
        BA_LoadReferenceLayers(txtTerrain.Text, txtDrainage.Text, txtWatershed.Text, pourpointRef)
        'Dim SaveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        'Dim BasinInfoButton = AddIn.FromID(Of BtnBasinInfo)(My.ThisAddIn.IDs.BtnBasinInfo)
        'SaveAOIMXDButton.selectedProperty = True
        'BasinInfoButton.selectedProperty = True
    End Sub
    Private Sub txtTerrain_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTerrain.DoubleClick
        txtTerrain.Text = ""
        CmdUndo.Enabled = True
    End Sub
    Private Sub txtDrainage_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDrainage.DoubleClick
        txtDrainage.Text = ""
        CmdUndo.Enabled = True
    End Sub
    Private Sub txtWatershed_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWatershed.DoubleClick
        txtWatershed.Text = ""
        CmdUndo.Enabled = True
    End Sub


    Private Sub CmboxStationAtt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmboxStationAtt.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboStationArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboStationArea.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboStation_Value_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboStation_Value.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSNOTEL_Elevation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSNOTEL_Elevation.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSNOTEL_Name_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSNOTEL_Name.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSC_Elevation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSC_Elevation.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSC_Name_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSC_Name.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub


    Private Sub Opt10M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Opt10M.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub Opt30M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Opt30M.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptMeter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptFoot_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptFoot.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSTMeter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSTMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSTFoot_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSTFoot.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSCMeter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSCMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSCFoot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSCFoot.CheckedChanged
        CmdUndo.Enabled = True
    End Sub

    Private Sub lstLayers_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayers.Click
        If lstLayers.SelectedIndex >= 0 Then CmdRemoveLayer.Enabled = True
    End Sub

    Private Sub CmdAddLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAddLayer.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String, Data_Name As String, data_type As Object, data_type_name As String
        Dim data_fullname As String

        Dim pFilter As IGxObjectFilter = New GxFilterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select GIS Layers"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        Dim pGxObj As IGxObject = pGxObject.Next
        If pGxObj.Category = BA_EnumDescription(GxFilterCategory.FeatureService) Or _
            pGxObj.Category = BA_EnumDescription(GxFilterCategory.ImageService) Or _
            pGxObj.Category = BA_EnumDescription(GxFilterCategory.MapService) Then
            MessageBox.Show("Web services cannot be used in the participating layers section.", "Invalid layers", MessageBoxButtons.OK)
            Exit Sub
        End If

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset = CType(pGxObj, IGxDataset)
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = 4 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 5 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 12 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 13 Then
            data_type_name = " (Raster)"
        Else
            data_type_name = " (Cannot Clip)"
        End If

        'pad a backslash to the path if it doesn't have one.
        'If Right(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name & data_type_name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        lstLayers.Items.Add(data_fullname)
        lstLayers.SelectedIndex = lstLayers.Items.Count - 1
        CmdRemoveLayer.Enabled = True
        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdRemoveLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdRemoveLayer.Click
        lstLayers.Items.Remove(lstLayers.SelectedItem)
        If lstLayers.Items.Count = 0 Then
            CmdRemoveLayer.Enabled = False
        Else
            lstLayers.SelectedItem = lstLayers.Items.Count - 1
            CmdUndo.Enabled = True
        End If
    End Sub
    Private Sub CmdClearAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearAll.Click
        lstLayers.Items.Clear()
        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSaveSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSaveSettings.Click
        SaveSettingsButton()
    End Sub
    Public Sub SaveSettingsButton()
        Dim missingdata As Boolean = False
        Dim notForBasinAnalysis As Boolean = Me.ChkboxAOIOnly.Checked

        'check for required fields
        'DEM10 no longer required
        'If Len(Me.txtDEM10.Text) = 0 Then missingdata = True
        If Len(Me.txtDEM30.Text) = 0 Then missingdata = True
        If Len(Me.txtGaugeStation.Text) = 0 Then missingdata = True

        If Not notForBasinAnalysis Then
            If Len(Me.txtSNOTEL.Text) = 0 Then missingdata = True
            If Len(Me.txtSnowCourse.Text) = 0 Then missingdata = True
            If Len(Me.txtPRISM.Text) = 0 Then missingdata = True
        End If

        'MsgBox(Len(Me.txtTerrain.Text))
        If missingdata Then
            MsgBox("Missing information on the required fields!")
            Exit Sub
        End If

        'These layer paths can be directly edited to support ArcGIS Online since it is not
        'supported by ArcCatalog. Make sure the layer number is indicated and make sure the
        'url is valid
        If Not String.IsNullOrEmpty(txtGaugeStation.Text) Then
            If BA_GetWorkspaceTypeFromPath(txtGaugeStation.Text) = WorkspaceType.FeatureServer Then
                txtGaugeStation.Text = AddLayerId(txtGaugeStation.Text)
                If Not BA_File_ExistsFeatureServer(txtGaugeStation.Text) Then
                    MessageBox.Show("The Gauge Stations value is not a valid feature service. The settings cannot be saved!", "BAGIS")
                    txtGaugeStation.Focus()
                    Exit Sub
                End If
            End If
        End If
        If Not String.IsNullOrEmpty(txtSNOTEL.Text) Then
            If BA_GetWorkspaceTypeFromPath(txtSNOTEL.Text) = WorkspaceType.FeatureServer Then
                txtSNOTEL.Text = AddLayerId(txtSNOTEL.Text)
                If Not BA_File_ExistsFeatureServer(txtSNOTEL.Text) Then
                    MessageBox.Show("The SNOTEL Stations value is not a valid feature service. The settings cannot be saved!", "BAGIS")
                    txtSNOTEL.Focus()
                    Exit Sub
                End If
            End If
        End If
        If Not String.IsNullOrEmpty(txtSnowCourse.Text) Then
            If BA_GetWorkspaceTypeFromPath(txtSnowCourse.Text) = WorkspaceType.FeatureServer Then
                txtSnowCourse.Text = AddLayerId(txtSnowCourse.Text)
                If Not BA_File_ExistsFeatureServer(txtSnowCourse.Text) Then
                    MessageBox.Show("The Snow Course Stations value is not a valid feature service. The settings cannot be saved!", "BAGIS")
                    txtSnowCourse.Focus()
                    Exit Sub
                End If
            End If
        End If

        Dim demProjText As String = BA_GetProjectionString(Me.txtDEM30.Text)
        Dim projectionsToCheck = New SortedList
        projectionsToCheck.Add("Snotel", Me.txtSNOTEL.Text)
        projectionsToCheck.Add("Snow Course", Me.txtSnowCourse.Text)
        Dim mismatchList As New SortedList
        For Each key As String In projectionsToCheck.Keys
            Dim projPath As String = projectionsToCheck(key)
            If Not String.IsNullOrEmpty(projPath) Then
                Dim layerType As WorkspaceType = BA_GetWorkspaceTypeFromPath(projPath)
                If layerType <> WorkspaceType.FeatureServer Then
                    Dim parentPath As String = "Please Return"
                    'Although this is an optional argument, it's the only to strip the file type from the text field
                    Dim tempExt As String = "tempExt"
                    Dim fileName As String = BA_GetBareNameAndExtension(projPath, parentPath, tempExt)
                    If layerType = WorkspaceType.Raster And tempExt = "(Shapefile)" Then
                        fileName = BA_StandardizeShapefileName(fileName, True)
                    End If
                    projPath = parentPath & fileName
                End If
                Dim projString As String = BA_GetProjectionString(projPath)
                If demProjText <> projString Then
                    mismatchList.Add(key, projString)
                End If
            End If
        Next
        ' One or more of the projections didn't match
        If mismatchList.Count > 0 Then
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("Warning: One or more layers is in a different projection from the 30m DEM layer." & vbCrLf)
            sb.Append("30m DEM projection = " & demProjText & vbCrLf)
            For Each key As String In mismatchList.Keys
                sb.Append(key & " = " & mismatchList(key) & vbCrLf)
            Next
            sb.Append(vbCrLf)
            sb.Append("Do you still wish to save the settings ?")
            Dim res As DialogResult = MessageBox.Show(sb.ToString, "Projection mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        'MsgBox("Update variables")
        Update_BA_SystemSettings()

        Dim saveSuccess As BA_ReturnCode = BA_Save_Settings()
        If saveSuccess <> BA_ReturnCode.Success Then
            MsgBox("Cannot save to the basinanalyst.def file in " & BA_Settings_Filepath & "! Please contact your system administrator.")
        Else
            MsgBox("Basin Analyst settings are saved to the definition file!")
            'enable btnAddRefLayers
            CmdUndo.Enabled = False
        End If
    End Sub

    'Appends a layer id of 0 (the first layer) if url is missing layer id
    Private Function AddLayerId(ByVal url As String) As String
        Dim lastIdx As Integer = url.LastIndexOf("/")
        Dim layerId As String = url.Substring(lastIdx + 1)
        If Not IsNumeric(layerId) Then
            url = url + "/0"
        End If
        Return url
    End Function

    'get the name of the layers listed in the lstLayers and send them to an array for using in frmCreateAOI class
    'Private Sub UpdatelstLayersItemNames()
    '    Dim strArray(lstLayers.Items.Count - 1) As String
    '    For i = 0 To lstLayers.Items.Count - 1
    '        strArray(i) = lstLayers.Items.Item(i)
    '        frmSettingsListLayeritem = strArray
    '        'MsgBox(frmSettingsListLayeritem(i))
    '    Next
    'End Sub
    'Pass the index of comboboxes in the frmSetting into public integer variables for using in frmCreateAOI class
    'Private Sub UpdatefrmSettingsComboIndices()
    '    PourNameFieldIndex = CmboxStationAtt.SelectedIndex
    '    SNOTEL_NameFieldIndex = ComboSNOTEL_Name.SelectedIndex
    '    SCourse_NameFieldIndex = ComboSC_Name.SelectedIndex
    'End Sub

    Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
        Dim response As Integer
        If CmdUndo.Enabled Then 'user exits the dialog window without saving the change
            response = MsgBox("Save changes?", vbYesNo, "Exit Setting")
            If response = vbYes Then
                SaveSettingsButton()
            Else
                'BA_Read_Settings() 'reload the setting
                Me.Close()
            End If
        End If
        Me.Close()
    End Sub

    Private Sub Update_BA_SystemSettings()
        With BA_SystemSettings
            .GenerateAOIOnly = Me.ChkboxAOIOnly.Checked
            .Status = 1

            .Ref_Terrain = Trim(Me.txtTerrain.Text)
            .Ref_Drainage = Trim(Me.txtDrainage.Text)
            .Ref_Watershed = Trim(Me.txtWatershed.Text)

            'the following are required fields
            .DEM10M = Trim(Me.txtDEM10.Text)
            .DEM30M = Trim(Me.txtDEM30.Text)
            .DEM10MPreferred = Me.Opt10M.Checked
            .DEM_ZUnit_IsMeter = Me.OptMeter.Checked

            .PourPointLayer = Trim(Me.txtGaugeStation.Text)
            .PourPointField = Convert.ToString(Me.CmboxStationAtt.SelectedItem)
            .PourAreaField = Convert.ToString(Me.ComboStationArea.SelectedItem)
            .PourAreaUnit = Convert.ToString(Me.ComboStation_Value.SelectedItem)

            .SNOTELLayer = Trim(Me.txtSNOTEL.Text)
            .SNOTEL_ElevationField = Convert.ToString(Me.ComboSNOTEL_Elevation.SelectedItem)
            .SNOTEL_NameField = Convert.ToString(Me.ComboSNOTEL_Name.SelectedItem)
            .SNOTEL_ZUnit_IsMeter = Me.OptSTMeter.Checked

            .SCourseLayer = Trim(Me.txtSnowCourse.Text)
            .SCourse_ElevationField = Convert.ToString(Me.ComboSC_Elevation.SelectedItem)
            .SCourse_NameField = Convert.ToString(Me.ComboSC_Name.SelectedItem)
            .SCourse_ZUnit_IsMeter = Me.OptSCMeter.Checked

            .PRISMFolder = Trim(Me.txtPRISM.Text)
            'If lstLayers.Items.Count = 0 Then
            '.listCount = Nothing
            ' Else
            .listCount = lstLayers.Items.Count
            ' End If

            'other layers
            Dim layerNo As Integer = lstLayers.Items.Count
            If layerNo > 0 Then
                ReDim .OtherLayers(layerNo)
                For i As Integer = 0 To layerNo - 1
                    .OtherLayers(i) = lstLayers.Items(i)
                Next
            End If

        End With
    End Sub

    'we do not need this part because we have a similar block that works the same as this
    'Private Sub Display_BA_SystemSettings()
    '    With BA_SystemSettings
    '        .Ref_Terrain = Trim(Me.txtTerrain.Text)
    '        .Ref_Drainage = Trim(Me.txtDrainage.Text)
    '        .Ref_Watershed = Trim(Me.txtWatershed.Text)

    '        'the following are required fields
    '        '.DEM10M = (BA_SystemSettings.DEM10M)
    '        '.DEM30M = (BA_SystemSettings.DEM30M)

    '        .DEM10M = Trim(Me.txtDEM10.Text)
    '        .DEM30M = Trim(Me.txtDEM30.Text)

    '        If .DEM10MPreferred Then
    '            Me.Opt10M.Checked = True
    '        Else
    '            Me.Opt30M.Checked = True
    '        End If

    '        .DEM_ZUnit_IsMeter = Me.OptMeter.Checked

    '        .PourPointLayer = Trim(Me.txtGaugeStation.Text)
    '        .PourPointField = Me.CmboxStationAtt.SelectedItem.ToString
    '        .PourAreaField = Me.ComboStationArea.SelectedItem.ToString
    '        .PourAreaUnit = Me.ComboStation_Value.SelectedItem.ToString

    '        .SNOTELLayer = Trim(Me.txtSNOTEL.Text)
    '        .SNOTEL_ElevationField = Me.ComboSNOTEL_Elevation.SelectedIndex.ToString
    '        .SNOTEL_NameField = Me.ComboSC_Name.SelectedItem.ToString
    '        '.SNOTEL_ZUnit_IsMeter = Me.OptSTMeter.Checked
    '        If .DEM_ZUnit_IsMeter Then
    '            Me.OptSTMeter.Checked = True
    '        End If

    '        .SCourseLayer = Trim(Me.txtSnowCourse.Text)
    '        .SCourse_ElevationField = Me.ComboSC_Elevation.SelectedItem.ToString
    '        .SCourse_NameField = Me.ComboSC_Name.SelectedItem.ToString
    '        '.SCourse_ZUnit_IsMeter = Me.OptSCMeter.Checked
    '        If .SCourse_ZUnit_IsMeter Then
    '            Me.OptSCMeter.Checked = True
    '        End If

    '        .PRISMFolder = Trim(Me.txtPRISM.Text)

    '        'other layers
    '        Dim layerNo As Integer = Me.lstLayers.Items.Count
    '        If layerNo > 0 Then
    '            ReDim .OtherLayers(layerNo)
    '            For i As Integer = 0 To layerNo - 1
    '                .OtherLayers(i) = Me.lstLayers.Items(i)
    '            Next
    '        End If

    '    End With
    'End Sub

    Private Sub CmdUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdUndo.Click
        'check for network connectivity
        BA_Read_Settings(Me)
        CmdUndo.Enabled = False
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim settings_message As String
        BA_SetSettingPath() 'set the BA_Settings_Filepath global variable
        Me.ComboStation_Value.Items.Clear()
        Me.ComboStation_Value.Items.Add("Unknown")
        Me.ComboStation_Value.Items.Add("Sq. Km")
        Me.ComboStation_Value.Items.Add("Acre")
        Me.ComboStation_Value.Items.Add("Sq. Miles")
        Me.ComboStation_Value.SelectedIndex = 0

        Me.ComboSNOTEL_Name.Items.Clear()
        Me.ComboSNOTEL_Name.Items.Add("none")
        Me.ComboSNOTEL_Name.SelectedIndex = 0

        'reset Snow courses name field combobox
        Me.ComboSC_Name.Items.Clear()
        Me.ComboSC_Name.Items.Add("none")
        Me.ComboSC_Name.SelectedIndex = 0

        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Loading and validating BAGIS data settings", "Loading settings")
            progressDialog2.Animation = esriProgressAnimationTypes.esriProgressSpiral
            pStepProg.Hide()    'Don't use step progressor
            progressDialog2.ShowDialog()

            'set settings
            settings_message = BA_Read_Settings(Me)
            Dim msgPrefix As String = Nothing
            If BA_SystemSettings.AnalysisSourceNotSpecified = True Then
                'PRISM, Snotel, or snow course data is not specified
                msgPrefix = "The Generate AOI Only option is turned on for BAGIS because the sources for precipitation, SNOTEL, and snow course data are not specified." & vbCrLf & vbCrLf
             ElseIf BA_SystemSettings.GenerateAOIOnly = True Then
                msgPrefix = "The Generate AOI Only option is turned on for BAGIS because at least one of the required sources is missing. SNOTEL (and/or Snow Course and/or PRISM) data missing" & vbCrLf & vbCrLf
            End If

            If Len(settings_message) > 0 Then
                If Not String.IsNullOrEmpty(msgPrefix) Then
                    settings_message = msgPrefix & settings_message
                End If

                MsgBox(settings_message)

                If settings_message.Substring(0, 7) = "Version" Then
                    MsgBox("Please update Settings file using save settings in Settings form.", vbOKOnly, "BAGIS Settings Version Error")
                End If
                If settings_message.Substring(0, 6) = "ERROR!" Then
                    MsgBox("Please set and save the data layer settings first!")
                End If

                'SelectBasin_Flag = False 
                Dim cboSelectBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
                cboSelectBasin.selectedProperty = False
            ElseIf Not String.IsNullOrEmpty(msgPrefix) Then
                MsgBox(msgPrefix)
            End If

            CmdUndo.Enabled = False
            Me.Text = Me.Text & " " & BA_Settings_Filepath & "\" & BA_Settings_Filename

            'enable btnAddreferencelayers 
            'Dim AddRefLayersButton = AddIn.FromID(Of BtnAddRefLayers)(My.ThisAddIn.IDs.BtnAddRefLayers)
            'If Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Drainage) And _
            '    Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Watershed) And _
            '    Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Terrain) Then
            '    AddRefLayersButton.selectedProperty = True
            'End If

        Catch ex As Exception
            Debug.Print("frmSettings_Load Exception: " & ex.Message)
        Finally
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub ChkboxAOIOnly_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkboxAOIOnly.CheckedChanged
        Dim toggleStatus As Boolean = Me.ChkboxAOIOnly.Checked
        ToggleAOICreationOption(toggleStatus)
        CmdUndo.Enabled = True
    End Sub

    Private Sub ToggleAOICreationOption(ByVal status As Boolean)
        Dim not_Status As Boolean = Not status
        'if status is TRUE, then disable all none essential parameters for AOI creation
        lblSNOTEL.Enabled = not_Status
        lblSnowCourse.Enabled = not_Status
        lblPRISM.Enabled = not_Status

        Static SNOTELtxt As String
        Static SCtxt As String
        Static PRISMtxt As String

        If Trim(txtSNOTEL.Text) = "" Then
            txtSNOTEL.Text = SNOTELtxt
        Else
            SNOTELtxt = txtSNOTEL.Text
            txtSNOTEL.Text = ""
        End If

        If Trim(txtSnowCourse.Text) = "" Then
            txtSnowCourse.Text = SCtxt
        Else
            SCtxt = txtSnowCourse.Text
            txtSnowCourse.Text = ""
        End If

        If Trim(txtPRISM.Text) = "" Then
            txtPRISM.Text = PRISMtxt
        Else
            PRISMtxt = txtPRISM.Text
            txtPRISM.Text = ""
        End If

        txtSNOTEL.Enabled = not_Status
        txtSnowCourse.Enabled = not_Status
        txtPRISM.Enabled = not_Status
        CmdSetSNOTEL.Enabled = not_Status
        CmdSetSnowC.Enabled = not_Status
        CmdSetPrecip.Enabled = not_Status

        'hide the followings
        lblElevField.Visible = not_Status
        lblNameField.Visible = not_Status

        ComboSNOTEL_Elevation.Visible = not_Status
        ComboSNOTEL_Name.Visible = not_Status
        lblSNOTELUnit.Visible = not_Status
        GrpBoxSNOTELUnit.Visible = not_Status

        ComboSC_Elevation.Visible = not_Status
        ComboSC_Name.Visible = not_Status
        lblSnowCourseUnit.Visible = not_Status
        GrpBoxSnowCourseUnit.Visible = not_Status
    End Sub

    Private Sub ShowGeodatabaseErrorMessage(ByVal dataName As String)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(dataName)
        sb.Append(" cannot use data from a file geodatabase.")
        sb.Append(" Please select a different data source.")
        MessageBox.Show(sb.ToString, "Invalid data source", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnDefault_Click(sender As System.Object, e As System.EventArgs) Handles BtnDefault.Click
        'Dim settingsPath As String = BA_GetAddInDirectory() & "\defaultSettings.json"
        'Dim defaultSettings As Settings = BA_ReadDefaultSettingsFromJson(settingsPath)
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Loading and validating default BAGIS settings", "Loading default settings")
            progressDialog2.Animation = esriProgressAnimationTypes.esriProgressSpiral
            pStepProg.Hide()    'Don't use step progressor
            progressDialog2.ShowDialog()
            Dim defaultSettings As Settings = BA_QueryDefaultSettings(BA_WebServerName)
            If defaultSettings IsNot Nothing Then
                Dim warningSb As StringBuilder = New StringBuilder()
                warningSb.Append("WARNING!")

                txtTerrain.Text = Nothing
                'Check to see if settings file exists at default location
                Dim terrainPath As String = BA_Settings_Filepath & "\" & defaultSettings.terrain
                Dim copyFile As Boolean = True
                If BA_File_ExistsWindowsIO(terrainPath) Then
                    Dim result As DialogResult = MessageBox.Show("The terrain reference layer already exists at " & terrainPath & "." & _
                                                                 vbCrLf & "Do you wish to download the default layer and overwrite the existing layer definition?", _
                                                                 "Terrain layer", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result <> DialogResult.Yes Then
                        'Set the path to the file the user wants to keep
                        txtTerrain.Text = terrainPath
                        copyFile = False
                    End If
                End If
                If copyFile = True Then
                    Dim success As BA_ReturnCode = DownloadLyrFile(BA_WebServerName, terrainPath)
                    'If BA_File_ExistsWindowsIO(BA_GetAddInDirectory() & BA_EnumDescription(PublicPath.TerrainLayer)) Then

                    '    IO.File.Copy(BA_GetAddInDirectory() & BA_EnumDescription(PublicPath.TerrainLayer), BA_Settings_Filepath & BA_EnumDescription(PublicPath.TerrainLayer), True)
                    '    txtTerrain.Text = BA_Settings_Filepath & BA_EnumDescription(PublicPath.TerrainLayer)
                    'Else
                    '    MessageBox.Show("The default terrain reference layer could not be found. It will not be copied to " & _
                    '                    BA_Settings_Filepath & ".", "Missing terrain layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End If
                End If

                'As of 28-JAN-2016, all 3 terrain layers are included in txtTerrain the others are likely null
                txtDrainage.Text = defaultSettings.drainage
                txtWatershed.Text = defaultSettings.watershed
                'As of 28-JAN-2016, there is no 10m DEM so we set it to nothing
                txtDEM10.Text = defaultSettings.dem10
                'No default DEM until we know one exists
                Opt10M.Checked = False
                Opt30M.Checked = False
                txtDEM10.Text = Nothing

                ' This Dictionary keeps track of all the checked urls so that BAGIS doesn't hang
                ' trying to connect to the same server for each textbox if the server is down. Currently we
                ' only have one server but this could change.
                Dim checkedUrls As IDictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
                Dim valid1 As Boolean = False

                'Uncheck AOIOnly; Assume defaults will include correct layers
                ChkboxAOIOnly.Checked = False

                'check if file exists
                If Not String.IsNullOrEmpty(defaultSettings.dem10) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.dem10)
                    valid1 = BA_VerifyUrl(defaultSettings.dem10, checkedUrls)
                    If valid1 Then
                        If BA_File_Exists(defaultSettings.dem10, wType, esriDatasetType.esriDTRasterDataset) Then
                            txtDEM10.Text = defaultSettings.dem10
                            If defaultSettings.preferredDem = BA_Settings_dem10 Then Opt10M.Checked = True
                        End If
                    End If
                End If

                txtDEM30.Text = Nothing
                'check if file exists
                If Not String.IsNullOrEmpty(defaultSettings.dem30) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.dem30)
                    valid1 = BA_VerifyUrl(defaultSettings.dem30, checkedUrls)
                    If valid1 Then
                        If BA_File_Exists(defaultSettings.dem30, wType, esriDatasetType.esriDTRasterDataset) Then
                            txtDEM30.Text = defaultSettings.dem30
                            If defaultSettings.preferredDem = BA_Settings_dem30 Then Opt30M.Checked = True
                        End If
                    End If
                End If
                If Not String.IsNullOrEmpty(defaultSettings.demElevUnit) Then
                    Select Case defaultSettings.demElevUnit.ToLower
                        Case BA_EnumDescription(MeasurementUnit.Meters).ToLower
                            OptMeter.Checked = True
                            OptFoot.Checked = False
                        Case BA_EnumDescription(MeasurementUnit.Feet).ToLower
                            OptMeter.Checked = False
                            OptFoot.Checked = True
                        Case Else
                            OptMeter.Checked = False
                            OptFoot.Checked = False
                    End Select
                Else
                    OptMeter.Checked = False
                    OptFoot.Checked = False
                End If

                txtGaugeStation.Text = Nothing
                CmboxStationAtt.Items.Clear()
                ComboStationArea.Items.Clear()
                ComboStation_Value.SelectedIndex = 0
                If Not String.IsNullOrEmpty(defaultSettings.gaugeStation) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.gaugeStation)
                    valid1 = BA_VerifyUrl(defaultSettings.gaugeStation, checkedUrls)
                    Dim featureClass As IFeatureClass = Nothing
                    If valid1 Then
                        If wType = WorkspaceType.Raster Then
                            Dim filePath As String = "return"
                            Dim fileName As String = BA_GetBareName(defaultSettings.gaugeStation, filePath)
                            featureClass = BA_OpenFeatureClassFromFile(filePath, fileName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            featureClass = BA_OpenFeatureClassFromService(defaultSettings.gaugeStation, 0)
                        End If
                    End If
                    If featureClass IsNot Nothing Then
                        txtGaugeStation.Text = defaultSettings.gaugeStation
                        'Name field
                        'get fields
                        Dim pFields As IFields = featureClass.Fields
                        Dim nFields As Integer = pFields.FieldCount
                        Dim aField As IField = Nothing
                        Dim qType As esriFieldType
                        Dim foundIt As Boolean = False
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeInteger Or _
                                 qType = esriFieldType.esriFieldTypeString Then
                                CmboxStationAtt.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.gaugeStationName, True) = 0 Then
                                    CmboxStationAtt.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.gaugeStationName & " is not in gauge station data")
                        End If
                        foundIt = False
                        'Area field
                        ComboStationArea.Items.Add("No data")
                        ComboStationArea.SelectedItem = "No data"
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then 'numeric data types
                                ComboStationArea.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.gaugeStationArea, True) = 0 Then
                                    ComboStationArea.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.gaugeStationArea & " is not in gauge station data")
                        End If
                        foundIt = False

                        'Areal units; The combo box is loaded when the form loads
                        If Not String.IsNullOrEmpty(defaultSettings.gaugeStationUnits) Then
                            Select Case defaultSettings.gaugeStationUnits.ToLower
                                Case BA_EnumDescription(MeasurementUnit.SquareKilometers).ToLower
                                    ComboStation_Value.SelectedIndex = 1
                                Case BA_EnumDescription(MeasurementUnit.Acres).ToLower
                                    ComboStation_Value.SelectedIndex = 2
                                Case BA_EnumDescription(MeasurementUnit.SquareMiles).ToLower
                                    ComboStation_Value.SelectedIndex = 3
                                Case Else
                                    ComboStation_Value.SelectedIndex = 0
                            End Select
                        Else
                            ComboStation_Value.SelectedIndex = 0
                        End If
                    End If
                End If

                txtSNOTEL.Text = ""
                ComboSNOTEL_Elevation.Items.Clear()
                ComboSNOTEL_Name.Items.Clear()
                If Not String.IsNullOrEmpty(defaultSettings.snotel) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.snotel)
                    valid1 = BA_VerifyUrl(defaultSettings.snotel, checkedUrls)
                    Dim featureClass As IFeatureClass = Nothing
                    If valid1 Then
                        If wType = WorkspaceType.Raster Then
                            Dim filePath As String = "return"
                            Dim fileName As String = BA_GetBareName(defaultSettings.snotel, filePath)
                            featureClass = BA_OpenFeatureClassFromFile(filePath, fileName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            featureClass = BA_OpenFeatureClassFromService(defaultSettings.snotel, 0)
                        End If
                    End If
                    If featureClass IsNot Nothing Then
                        txtSNOTEL.Text = defaultSettings.snotel
                        'Elevation field
                        'get fields
                        Dim pFields As IFields = featureClass.Fields
                        Dim nFields As Integer = pFields.FieldCount
                        Dim aField As IField = Nothing
                        Dim qType As esriFieldType
                        Dim foundIt As Boolean = False
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then
                                ComboSNOTEL_Elevation.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.snotelElev, True) = 0 Then
                                    ComboSNOTEL_Elevation.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.snotelElev & " is not in SNOTEL data")
                        End If
                        foundIt = False
                        'Name field
                        ComboSNOTEL_Name.Items.Add("None")
                        ComboSNOTEL_Name.SelectedItem = "None"
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType = esriFieldType.esriFieldTypeString Then 'string data types
                                ComboSNOTEL_Name.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.snotelName, True) = 0 Then
                                    ComboSNOTEL_Name.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.snotelName & " is not in SNOTEL data")
                        End If
                    End If
                End If

                txtSnowCourse.Text = ""
                ComboSC_Elevation.Items.Clear()
                ComboSC_Name.Items.Clear()
                If Not String.IsNullOrEmpty(defaultSettings.snowCourse) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.snowCourse)
                    valid1 = BA_VerifyUrl(defaultSettings.snowCourse, checkedUrls)
                    Dim featureClass As IFeatureClass = Nothing
                    If valid1 Then
                        If wType = WorkspaceType.Raster Then
                            Dim filePath As String = "return"
                            Dim fileName As String = BA_GetBareName(defaultSettings.snowCourse, filePath)
                            featureClass = BA_OpenFeatureClassFromFile(filePath, fileName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            featureClass = BA_OpenFeatureClassFromService(defaultSettings.snowCourse, 0)
                        End If
                    End If
                    If featureClass IsNot Nothing Then
                        txtSnowCourse.Text = defaultSettings.snowCourse
                        'Elevation field
                        'get fields
                        Dim pFields As IFields = featureClass.Fields
                        Dim nFields As Integer = pFields.FieldCount
                        Dim aField As IField = Nothing
                        Dim qType As esriFieldType
                        Dim foundIt As Boolean = False
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then
                                ComboSC_Elevation.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.snowCourseElev, True) = 0 Then
                                    ComboSC_Elevation.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.snowCourseElev & " is not in Snow Course data")
                        End If
                        foundIt = False
                        'Name field
                        ComboSC_Name.Items.Add("None")
                        ComboSC_Name.SelectedItem = "None"
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType = esriFieldType.esriFieldTypeString Then 'string data types
                                ComboSC_Name.Items.Add(aField.Name)
                                If String.Compare(aField.Name, defaultSettings.snowCourseName, True) = 0 Then
                                    ComboSC_Name.SelectedItem = aField.Name
                                    foundIt = True
                                End If
                            End If
                        Next
                        If foundIt = False Then
                            warningSb.Append(vbCrLf & "Attribute Field Missing: " & defaultSettings.snowCourseName & " is not in Snow Course data")
                        End If
                    End If
                End If

                txtPRISM.Text = ""
                If Not String.IsNullOrEmpty(defaultSettings.prism) Then
                    Dim wType = BA_GetWorkspaceTypeFromPath(defaultSettings.prism)
                    valid1 = BA_VerifyUrl(defaultSettings.prism, checkedUrls)
                    Dim FileExists As Boolean = False
                    If valid1 Then
                        If wType = WorkspaceType.ImageServer Then
                            Dim TempPathName As String = defaultSettings.prism & "/" & PrismServiceNames.Prism_Precipitation_q4.ToString & _
                             "/" & BA_Url_ImageServer
                            FileExists = BA_File_ExistsImageServer(TempPathName)
                        Else
                            Dim TempPathName As String = defaultSettings.prism & "\Q4\grid"
                            FileExists = BA_Workspace_Exists(TempPathName)
                        End If
                    Else
                        FileExists = False
                    End If
                    If FileExists Then txtPRISM.Text = defaultSettings.prism
                End If

                'Remove all items from participating layers as there are none in the default settings
                lstLayers.Items.Clear()

                'Append any server connection errors to the warningsb
                For Each key As String In checkedUrls.Keys
                    If checkedUrls(key) = False Then
                        warningSb.Append(vbCrLf & "BAGIS was unable to connect to " & key & ". Data cannot currently be used from this server")
                    End If
                Next

                If Not String.Compare(warningSb.ToString, "WARNING!", False) = 0 Then
                    MessageBox.Show(warningSb.ToString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Else
                MessageBox.Show("The default settings could not be loaded", "Default Settings", MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Debug.Print("BtnDefault_Click: " & ex.Message)
        Finally
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
        End Try
    End Sub

    Private Function DownloadLyrFile(ByVal webserviceUrl As String, ByVal outputFilePath As String) As BA_ReturnCode
        Try
            Dim downloadUri As Uri = New Uri(webserviceUrl & "/api/rest/desktop/lyr/")
            Dim lyrDownload As New LayerDownload()
            lyrDownload.downLoadUrl = downloadUri.AbsoluteUri
            lyrDownload.downloadFolder = outputFilePath
            ' Using WebClient for built-in file download functionality
            Using myWebClient As New WebClient()
                'Put token in header
                AddHandler myWebClient.DownloadFileCompleted, AddressOf DownloadLyrFileCompleted
                myWebClient.DownloadFileAsync(downloadUri, outputFilePath, lyrDownload)
            End Using
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("DownloadLyrFile: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Private Sub DownloadLyrFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        Try
            Dim lyrDownload As LayerDownload = CType(e.UserState, LayerDownload)
            ' File download completed
            If e.Cancelled = True Then
                txtTerrain.Text = Nothing
                MessageBox.Show("Download of Terrain Ref. .lyr file was cancelled. The file will not be saved.", "Download cancelled", _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If e.Error IsNot Nothing Then
                MessageBox.Show("An error occurred while downloading the Terrain Ref. .lyr file from " & lyrDownload.downLoadUrl & ". The file will not be saved.", "Download error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

                Debug.Print("DownloadLyrFileCompleted error: " & e.Error.Message)
                Exit Sub
            End If
            ' The download succeeded !!
            If e.Cancelled = False And e.Error Is Nothing Then
                txtTerrain.Text = lyrDownload.downloadFolder
            End If
        Catch ex As Exception
            Debug.Print("DownloadFileCompleted: " & ex.Message)
        End Try
    End Sub
End Class