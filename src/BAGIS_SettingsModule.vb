Imports BAGIS_ClassLibrary
Imports System.IO
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports System.Windows.Forms



Module BAGIS_SettingsModule
    Public Function BA_Save_Settings(Optional ByVal OutputPathName As String = "") As BA_ReturnCode
        Dim FileName As String
        Dim i As Integer
        Dim nlistitems As Integer

        If Len(OutputPathName) = 0 Then 'user did not specify an output path+name
            If Len(BA_Settings_Filepath) = 0 Then
                Return BA_ReturnCode.NotSupportedOperation
            End If

            FileName = BA_Settings_Filepath & "\" & BA_Settings_Filename
        Else
            FileName = OutputPathName
        End If

        Try
            'open file for output
            Using sw As StreamWriter = File.CreateText(FileName) 'if Filename exists, it will be overwritten
                sw.WriteLine(BA_VersionText)
                sw.WriteLine(BA_SystemSettings.Ref_Terrain)
                sw.WriteLine(BA_SystemSettings.Ref_Drainage)
                sw.WriteLine(BA_SystemSettings.Ref_Watershed)

                'the following are required fields
                sw.WriteLine(BA_SystemSettings.DEM10M)
                sw.WriteLine(BA_SystemSettings.DEM30M)
                sw.WriteLine(BA_SystemSettings.DEM10MPreferred)
                sw.WriteLine(BA_SystemSettings.DEM_ZUnit_IsMeter)

                sw.WriteLine(BA_SystemSettings.PourPointLayer)
                sw.WriteLine(BA_SystemSettings.PourPointField)
                sw.WriteLine(BA_SystemSettings.PourAreaField)
                sw.WriteLine(BA_SystemSettings.PourAreaUnit)

                sw.WriteLine(BA_SystemSettings.SNOTELLayer)
                sw.WriteLine(BA_SystemSettings.SNOTEL_ElevationField)
                sw.WriteLine(BA_SystemSettings.SNOTEL_NameField)
                sw.WriteLine(BA_SystemSettings.SNOTEL_ZUnit_IsMeter)

                sw.WriteLine(BA_SystemSettings.SCourseLayer)
                sw.WriteLine(BA_SystemSettings.SCourse_ElevationField)
                sw.WriteLine(BA_SystemSettings.SCourse_NameField)
                sw.WriteLine(BA_SystemSettings.SCourse_ZUnit_IsMeter)

                sw.WriteLine(BA_SystemSettings.PRISMFolder)
                'required field ends here
                If BA_SystemSettings.listCount > 0 Then
                    nlistitems = UBound(BA_SystemSettings.OtherLayers)
                    For i = 0 To nlistitems
                        sw.WriteLine(BA_SystemSettings.OtherLayers(i))
                    Next
                End If

                sw.Flush() 'finish writing
                sw.Close() 'close the file
            End Using

            Return BA_ReturnCode.Success
        Catch
            Return BA_ReturnCode.UnknownError
        End Try

    End Function

    'read the settings in the settings file to the frmSettings
    Public Function BA_Read_Settings(ByRef SettingsForm As frmSettings) As String
        Dim fileno As Integer = 0
        Dim FileName As String = "", layertype As String = ""
        Dim return_message As String = ""
        Dim TempPathName As String
        Dim FileExists As Boolean = True
        Dim linestring As String = "", linestring1 As String = "", linestring2 As String = ""

        Dim SNOTELDataExist As Boolean = False
        Dim SnowCourseDataExist As Boolean = False
        Dim PRISMDataExist As Boolean = False

        If Len(BA_Settings_Filepath) = 0 Then
            Return "ERROR! BA_Read_Settings: Cannot retrieve the file path and name of the definition file."
        End If

        FileName = BA_Settings_Filepath & "\" & BA_Settings_Filename

        'check if file exist
        If Not File.Exists(FileName) Then
            'check if file exists
            Return "ERROR! BA_Read_Settings: The definition file does not exist."
        End If

        Try

            ' This Dictionary keeps track of all the checked urls so that BAGIS doesn't hang
            ' trying to connect to the same server for each textbox if the server is down. Currently we
            ' only have one server but this could change.
            Dim checkedUrls As IDictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)

            Using sr As StreamReader = File.OpenText(FileName)
                linestring = sr.ReadLine() 'read the keyword line                                                                                   '1

                'check version and compatible version text
                If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                    Return "Version ERROR! BA_Read_Settings: The definition file's version doesn't match the version of the model." & vbCrLf & _
                    "Software version: " & BA_VersionText & "   Definition file version: " & linestring
                End If

                return_message = "WARNING!"
                'check for network connectivity
                If Not BA_IsNetworkAvailable(0) Then
                    return_message = return_message & vbCrLf & "Your computer is not connected to the network. You may be unable to access some of the data layers."
                End If
                linestring = sr.ReadLine() 'read the terrain reference layer name                                                                   '2

                TempPathName = Trim(linestring)
                BA_SystemSettings.Ref_Terrain = TempPathName
                SettingsForm.txtTerrain.Text = BA_SystemSettings.Ref_Terrain
                If Len(TempPathName) > 0 Then 'it's OK to not have a specified reference layer
                    'check if file exists
                    If Not File.Exists(TempPathName) Then
                        return_message = return_message & vbCrLf & "Reference File Missing: " & TempPathName
                    End If
                End If

                ' WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass
                linestring = sr.ReadLine() 'read the drainage reference layer name                                                                  '3
                TempPathName = Trim(linestring)
                BA_SystemSettings.Ref_Drainage = TempPathName
                SettingsForm.txtDrainage.Text = BA_SystemSettings.Ref_Drainage
                If Len(TempPathName) > 0 Then 'it's OK to not have a specified reference layer
                    'check if file exists
                    If Not File.Exists(TempPathName) Then
                        return_message = return_message & vbCrLf & "Reference File Missing: " & TempPathName
                    End If
                End If

                linestring = sr.ReadLine() 'read the watershed reference layer name                                                                 '4
                TempPathName = Trim(linestring)
                BA_SystemSettings.Ref_Watershed = TempPathName
                SettingsForm.txtWatershed.Text = BA_SystemSettings.Ref_Watershed
                If Len(TempPathName) > 0 Then 'it's OK to not have a specified reference layer
                    'check if file exists
                    If Not File.Exists(TempPathName) Then
                        return_message = return_message & vbCrLf & "Reference File Missing: " & TempPathName
                    End If
                End If

                linestring = sr.ReadLine() 'read the 10 meters DEM layer name                                                                       '5
                TempPathName = Trim(linestring)
                BA_SystemSettings.DEM10M = TempPathName
                SettingsForm.txtDEM10.Text = BA_SystemSettings.DEM10M
                'check if file exists
                Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(TempPathName)
                Dim valid1 As Boolean = BA_VerifyUrl(SettingsForm.txtDEM10.Text, checkedUrls)
                If valid1 Then
                    If Not BA_File_Exists(TempPathName, wType, esriDatasetType.esriDTRasterDataset) Then
                        return_message = return_message & vbCrLf & "10 Meters DEM Data Missing: " & TempPathName
                    End If
                Else
                    return_message = return_message & vbCrLf & "10 Meters DEM Data Missing: " & TempPathName
                End If

                linestring = sr.ReadLine() 'read the 30 meter DEM layer name                                                                        '6
                TempPathName = Trim(linestring)
                BA_SystemSettings.DEM30M = TempPathName
                SettingsForm.txtDEM30.Text = BA_SystemSettings.DEM30M
                valid1 = BA_VerifyUrl(SettingsForm.txtDEM30.Text, checkedUrls)
                If valid1 Then
                    'check if file exists
                    wType = BA_GetWorkspaceTypeFromPath(TempPathName)
                    If Not BA_File_Exists(TempPathName, wType, esriDatasetType.esriDTRasterDataset) Then
                        return_message = return_message & vbCrLf & "30 Meters DEM Data Missing: " & TempPathName
                    End If
                Else
                    return_message = return_message & vbCrLf & "30 Meters DEM Data Missing: " & TempPathName
                End If

                linestring = sr.ReadLine() 'read preferred DEM setting                                                                              '7
                If UCase(Trim(linestring)) = "TRUE" Then '10 meter is the preferred DEM
                    SettingsForm.Opt10M.Checked = True
                Else
                    SettingsForm.Opt30M.Checked = True
                End If

                linestring = sr.ReadLine() 'read DEM Z unit setting                                                                                 '8
                If UCase(Trim(linestring)) = "TRUE" Then 'meter is the Z unit
                    SettingsForm.OptMeter.Checked = True
                Else
                    SettingsForm.OptFoot.Checked = True
                End If

                'check gauge station shapefile
                linestring = sr.ReadLine()                                                                                                          '9
                TempPathName = Trim(linestring)
                BA_SystemSettings.PourPointLayer = TempPathName
                SettingsForm.txtGaugeStation.Text = BA_SystemSettings.PourPointLayer

                linestring = sr.ReadLine() 'the linestring contains the selected field name                                                         '10
                linestring1 = sr.ReadLine() 'Reads Area Field Index from Def File                                                                   '11

                Dim File_Path As String = ""
                Dim File_Name As String = ""
                Dim qType As esriFieldType
                Dim nFields As Integer
                Dim iCount As Integer
                Dim pWksFactory As IWorkspaceFactory
                Dim pFeatWorkspace As IFeatureWorkspace
                Dim pFeatClass As IFeatureClass
                Dim pFields As IFields = Nothing
                Dim aField As IField
                Dim FieldIndex As Integer
                Dim aoiIdField As String = BA_AOI_IDField

                wType = BA_GetWorkspaceTypeFromPath(SettingsForm.txtGaugeStation.Text)
                valid1 = BA_VerifyUrl(SettingsForm.txtGaugeStation.Text, checkedUrls)
                If valid1 Then
                    'Test to see if TempPathName is empty because BA_VerifyUrl only tests web services
                    If Not String.IsNullOrEmpty(SettingsForm.txtGaugeStation.Text) Then
                        If wType = WorkspaceType.Raster Then
                            File_Name = BA_GetBareNameAndExtension(SettingsForm.txtGaugeStation.Text, File_Path, layertype)
                            TempPathName = File_Path & File_Name
                            FileExists = BA_Shapefile_Exists(TempPathName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            FileExists = BA_File_Exists(SettingsForm.txtGaugeStation.Text, wType, esriDatasetType.esriDTFeatureClass)
                            aoiIdField = BA_AOI_IDFieldFeatService
                        End If
                    End If
                Else
                    FileExists = False
                End If

                If FileExists Then  'text exists for the setting of this layer
                    'set name field
                    SettingsForm.CmboxStationAtt.Items.Clear()
                    FieldIndex = -1
                    iCount = 0
                    Dim idxFieldId = -1
                    'Object for feature service fields
                    Dim allFields As IList(Of FeatureServiceField) = New List(Of FeatureServiceField)

                    If wType = WorkspaceType.Raster Then
                        pFeatClass = BA_OpenFeatureClassFromFile(File_Path, File_Name)

                        'get fields
                        pFields = pFeatClass.Fields
                        nFields = pFields.FieldCount

                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeInteger Or _
                                 qType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.CmboxStationAtt.Items.Add(aField.Name)
                                If aField.Name = linestring Then FieldIndex = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        allFields = BA_QueryAllFeatureServiceFieldNames(SettingsForm.txtGaugeStation.Text)
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType <= esriFieldType.esriFieldTypeInteger Or _
                                fField.fieldType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.CmboxStationAtt.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring, True) = 0 Then FieldIndex = iCount
                                'Check for the index field; It is a String
                                'Raster datasets use findField method down below
                                If String.Compare(fField.alias, aoiIdField, True) = 0 Then idxFieldId = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If FieldIndex < 0 Then
                        return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring & " is not in " & TempPathName
                        SettingsForm.CmboxStationAtt.SelectedIndex = 0
                    Else
                        SettingsForm.CmboxStationAtt.SelectedIndex = FieldIndex
                    End If

                    'set area field
                    SettingsForm.ComboStationArea.Items.Clear()
                    SettingsForm.ComboStationArea.Items.Add("No data")

                    FieldIndex = -1
                    iCount = 0

                    If wType = WorkspaceType.Raster Then
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then 'numeric data types
                                SettingsForm.ComboStationArea.Items.Add(aField.Name)
                                If aField.Name = linestring1 Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next

                        'Ver1E Update - check awdb_id field in the forecast point layer
                        '@ToDo: How to handle this for feature service layer. Different field name? usgs_id  
                        idxFieldId = pFields.FindField(aoiIdField)
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        'Populate the area dropdown
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                                SettingsForm.ComboStationArea.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring1, True) = 0 Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If linestring1 = "No data" Then
                        SettingsForm.ComboStationArea.SelectedIndex = 0
                    Else
                        If FieldIndex <= 0 Then
                            return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring1 & " is not in " & TempPathName
                            SettingsForm.ComboStationArea.SelectedIndex = 0
                        Else
                            SettingsForm.ComboStationArea.SelectedIndex = FieldIndex
                        End If
                    End If

                    If idxFieldId <= 0 Then
                        return_message = return_message & vbCrLf & "Attribute Field Missing: " & aoiIdField & " is not in " & TempPathName
                    End If

                    aField = Nothing
                    pFields = Nothing
                    pFeatClass = Nothing
                    pFeatWorkspace = Nothing
                    pWksFactory = Nothing
                Else
                    return_message = return_message & vbCrLf & "Gauge station data Missing: " & TempPathName
                End If

                'Read Area Unit
                linestring = sr.ReadLine() 'Reads Area Unit Index from Def File                                                                             '12

                'Determine what idex to set list to:
                Dim UnitIndex As Integer
                If linestring = "Sq. Km" Then
                    UnitIndex = 1
                ElseIf linestring = "Acre" Then
                    UnitIndex = 2
                ElseIf linestring = "Sq. Miles" Then
                    UnitIndex = 3
                Else
                    UnitIndex = 0 'Nothing
                    If SettingsForm.ComboStationArea.SelectedIndex > 0 Then 'a valid area field was specified
                        return_message = return_message & vbCrLf & "Information Missing: Unknown AOI areal unit (" & linestring & ")"
                    End If
                End If

                'Display Area Unit
                SettingsForm.ComboStation_Value.SelectedIndex = UnitIndex

                'set snotel file
                BA_SystemSettings.AnalysisSourceNotSpecified = False 'Reset module-level variable
                linestring = sr.ReadLine()                                                                                                                  '13
                SettingsForm.txtSNOTEL.Text = Trim(linestring)

                wType = BA_GetWorkspaceTypeFromPath(SettingsForm.txtSNOTEL.Text)
                If Trim(linestring) = "" Then
                    TempPathName = "SNOTEL"
                    FileExists = False
                    BA_SystemSettings.AnalysisSourceNotSpecified = True
                Else
                    valid1 = BA_VerifyUrl(SettingsForm.txtSNOTEL.Text, checkedUrls)
                    If valid1 Then
                        If wType = WorkspaceType.Raster Then
                            File_Name = BA_GetBareNameAndExtension(SettingsForm.txtSNOTEL.Text, File_Path, layertype)
                            TempPathName = File_Path & File_Name
                            FileExists = BA_Shapefile_Exists(TempPathName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            FileExists = BA_File_Exists(SettingsForm.txtSNOTEL.Text, wType, esriDatasetType.esriDTFeatureClass)
                        End If
                    Else
                        FileExists = valid1
                    End If
                End If

                'set snotel field
                linestring = sr.ReadLine()  'elevation field                                                                                                '14
                linestring1 = sr.ReadLine() 'name field                                                                                                     '15
                linestring2 = sr.ReadLine() 'elevation unit                                                                                                 '16

                If FileExists Then  'text exists for the setting of this layer

                    'set elevation field
                    SettingsForm.ComboSNOTEL_Elevation.Items.Clear()
                    FieldIndex = -1
                    iCount = 0
                    'Object for feature service fields
                    Dim allFields As IList(Of FeatureServiceField) = New List(Of FeatureServiceField)

                    If wType = WorkspaceType.Raster Then
                        pFeatClass = BA_OpenFeatureClassFromFile(File_Path, File_Name)

                        'get fields
                        pFields = pFeatClass.Fields
                        nFields = pFields.FieldCount

                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then
                                SettingsForm.ComboSNOTEL_Elevation.Items.Add(aField.Name)
                                If String.Compare(aField.Name, linestring, True) = 0 Then FieldIndex = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        allFields = BA_QueryAllFeatureServiceFieldNames(SettingsForm.txtSNOTEL.Text)
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                                SettingsForm.ComboSNOTEL_Elevation.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring, True) Then FieldIndex = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If FieldIndex < 0 Then
                        return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring & " is not in " & TempPathName
                        SettingsForm.ComboSNOTEL_Elevation.SelectedIndex = 0
                    Else
                        SettingsForm.ComboSNOTEL_Elevation.SelectedItem = linestring
                    End If

                    'set name field
                    SettingsForm.ComboSNOTEL_Name.Items.Clear()
                    SettingsForm.ComboSNOTEL_Name.Items.Add("None")

                    FieldIndex = -1
                    iCount = 0

                    If wType = WorkspaceType.Raster Then
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.ComboSNOTEL_Name.Items.Add(aField.Name)
                                If aField.Name = linestring1 Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.ComboSNOTEL_Name.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring1, True) Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If FieldIndex < 0 Then
                        If linestring1 <> "None" Then
                            return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring1 & " is not in " & TempPathName
                        End If
                        SettingsForm.ComboSNOTEL_Name.SelectedItem = "None"
                    Else
                        SettingsForm.ComboSNOTEL_Name.SelectedItem = linestring1
                    End If

                    If UCase(Trim(linestring2)) = "TRUE" Then 'meter is the Z unit
                        SettingsForm.OptSTMeter.Checked = "True"
                    Else
                        SettingsForm.OptSTFoot.Checked = "True"
                    End If

                    SNOTELDataExist = True

                    aField = Nothing
                    pFields = Nothing
                    pFeatClass = Nothing
                    pFeatWorkspace = Nothing
                    pWksFactory = Nothing
                End If

                'read snow course
                linestring = sr.ReadLine()                                                                                                                  '13
                SettingsForm.txtSnowCourse.Text = Trim(linestring)

                wType = BA_GetWorkspaceTypeFromPath(SettingsForm.txtSnowCourse.Text)
                If Trim(linestring) = "" Then
                    TempPathName = "Snow Course"
                    FileExists = False
                    BA_SystemSettings.AnalysisSourceNotSpecified = True
                Else
                    valid1 = BA_VerifyUrl(SettingsForm.txtSnowCourse.Text, checkedUrls)
                    If valid1 Then
                        If wType = WorkspaceType.Raster Then
                            File_Name = BA_GetBareNameAndExtension(SettingsForm.txtSnowCourse.Text, File_Path, layertype)
                            TempPathName = File_Path & File_Name
                            FileExists = BA_Shapefile_Exists(TempPathName)
                        ElseIf wType = WorkspaceType.FeatureServer Then
                            FileExists = BA_File_Exists(SettingsForm.txtSnowCourse.Text, wType, esriDatasetType.esriDTFeatureClass)
                        End If
                    Else
                        FileExists = False
                    End If
                End If

                'set snow course field
                linestring = sr.ReadLine()  'elevation field                                                                                                '14
                linestring1 = sr.ReadLine() 'name field                                                                                                     '15
                linestring2 = sr.ReadLine() 'elevation unit                                                                                                 '16

                If FileExists Then  'text exists for the setting of this layer

                    'set elevation field
                    SettingsForm.ComboSC_Elevation.Items.Clear()
                    FieldIndex = -1
                    iCount = 0
                    'Object for feature service fields
                    Dim allFields As IList(Of FeatureServiceField) = New List(Of FeatureServiceField)

                    If wType = WorkspaceType.Raster Then
                        pFeatClass = BA_OpenFeatureClassFromFile(File_Path, File_Name)

                        'get fields
                        pFields = pFeatClass.Fields
                        nFields = pFields.FieldCount

                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType <= esriFieldType.esriFieldTypeDouble Then
                                SettingsForm.ComboSC_Elevation.Items.Add(aField.Name)
                                If aField.Name = linestring Then FieldIndex = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        allFields = BA_QueryAllFeatureServiceFieldNames(SettingsForm.txtSnowCourse.Text)
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType <= esriFieldType.esriFieldTypeDouble Then
                                SettingsForm.ComboSC_Elevation.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring, True) = 0 Then FieldIndex = iCount
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If FieldIndex < 0 Then
                        return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring & " is not in " & TempPathName
                        SettingsForm.ComboSC_Elevation.SelectedIndex = 0
                    Else
                        SettingsForm.ComboSC_Elevation.SelectedItem = linestring
                    End If

                    'set name field
                    SettingsForm.ComboSC_Name.Items.Clear()
                    SettingsForm.ComboSC_Name.Items.Add("None")

                    FieldIndex = -1
                    iCount = 0

                    If wType = WorkspaceType.Raster Then
                        For i = 0 To nFields - 1
                            aField = pFields.Field(i)
                            qType = aField.Type
                            If qType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.ComboSC_Name.Items.Add(aField.Name)
                                If aField.Name = linestring1 Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next
                    ElseIf wType = WorkspaceType.FeatureServer Then
                        For Each fField As FeatureServiceField In allFields
                            If fField.fieldType = esriFieldType.esriFieldTypeString Then
                                SettingsForm.ComboSC_Name.Items.Add(fField.alias)
                                If String.Compare(fField.alias, linestring1, True) Then FieldIndex = iCount + 1
                                iCount = iCount + 1
                            End If
                        Next
                    End If

                    If FieldIndex < 0 Then
                        If linestring1 <> "None" Then
                            return_message = return_message & vbCrLf & "Attribute Field Missing: " & linestring1 & " is not in " & TempPathName
                        End If
                        SettingsForm.ComboSC_Name.SelectedItem = "None"
                    Else
                        SettingsForm.ComboSC_Name.SelectedItem = linestring1
                    End If

                    If UCase(Trim(linestring2)) = "TRUE" Then 'meter is the Z unit
                        SettingsForm.OptSCMeter.Checked = "True"
                    Else
                        SettingsForm.OptSCFoot.Checked = "True"
                    End If

                    SnowCourseDataExist = True

                    aField = Nothing
                    pFields = Nothing
                    pFeatClass = Nothing
                    pFeatWorkspace = Nothing
                    pWksFactory = Nothing
                End If


                linestring = sr.ReadLine() 'read the prism layer name                                                                                       '21
                SettingsForm.txtPRISM.Text = Trim(linestring)

                If Trim(linestring) = "" Then
                    TempPathName = "PRISM folder"
                    FileExists = False
                    BA_SystemSettings.AnalysisSourceNotSpecified = True
                Else
                    valid1 = BA_VerifyUrl(SettingsForm.txtPRISM.Text, checkedUrls)
                    If valid1 Then
                        wType = BA_GetWorkspaceTypeFromPath(SettingsForm.txtPRISM.Text)
                        If wType = WorkspaceType.ImageServer Then
                            TempPathName = linestring & "/" & PrismServiceNames.PRISM_Precipitation_Q4.ToString & _
                             "/" & BA_Url_ImageServer
                            FileExists = BA_File_ExistsImageServer(TempPathName)
                        Else
                            TempPathName = linestring & "\Q4\grid"
                            FileExists = BA_Workspace_Exists(TempPathName)
                        End If
                    Else
                        FileExists = valid1
                    End If
                End If

                'check if file exists
                If FileExists Then
                    PRISMDataExist = True
                End If

                SettingsForm.lstLayers.Items.Clear()

                'omitted?
                'Just Read the line specified to the lstLayers list count and do nothing. 
                'This Line is only because there is one line in the .def file which only shows the number of items in the list
                ' linestring = sr.ReadLine()
                BA_SystemSettings.listCount = 0
                TempPathName = ""

                Do While sr.Peek() >= 0
                    linestring = Trim(sr.ReadLine()) 'read a line to the lineString variable                                                                          '22 to (21+listcount)
                    If Len(linestring) > 0 Then
                        SettingsForm.lstLayers.Items.Add(linestring)
                        'If (Len(Trim(linestring)) <> 0) Then SettingsForm.lstLayers.Items.Add(Trim(linestring))
                        File_Name = BA_GetBareNameAndExtension(linestring, File_Path, layertype)
                        TempPathName = File_Path & File_Name
                        wType = BA_GetWorkspaceTypeFromPath(TempPathName)
                        Select Case layertype
                            Case "(Shapefile)" 'shapefile
                                If wType = WorkspaceType.Raster Then
                                    FileExists = BA_Shapefile_Exists(TempPathName)
                                Else
                                    FileExists = BA_File_Exists(TempPathName, wType, esriDatasetType.esriDTFeatureClass)
                                End If
                            Case "(Raster)" 'raster
                                FileExists = BA_File_Exists(TempPathName, wType, esriDatasetType.esriDTRasterDataset)
                            Case Else
                                return_message = return_message & vbCrLf & "Participating Data Unknown Type: " & TempPathName & " " & layertype
                        End Select
                        If Not FileExists Then 'text exists for the setting of this layer
                            return_message = return_message & vbCrLf & "Participating Data Missing: " & TempPathName
                        End If
                        BA_SystemSettings.listCount = BA_SystemSettings.listCount + 1
                    End If
                Loop

                If SettingsForm.lstLayers.Items.Count > 0 Then SettingsForm.lstLayers.SelectedIndex = SettingsForm.lstLayers.Items.Count - 1
                sr.Close()
                'reset object variables
                aField = Nothing
                    pFields = Nothing
                    pFeatClass = Nothing
                    pFeatWorkspace = Nothing
                    pWksFactory = Nothing
            End Using

            If SNOTELDataExist And SnowCourseDataExist And PRISMDataExist Then
                SettingsForm.ChkboxAOIOnly.Checked = False
                BA_SystemSettings.GenerateAOIOnly = False
            Else
                SettingsForm.ChkboxAOIOnly.Checked = True
                BA_SystemSettings.GenerateAOIOnly = True
                'MsgBox("Missing required data for performing basin analysis!" & vbCrLf & "The Generate AOI Only option is turned on automatically for BAGIS.")
            End If

            'Append any server connection errors to the return_message
            For Each key As String In checkedUrls.Keys
                If checkedUrls(key) = False Then
                    return_message = return_message & vbCrLf & "BAGIS was unable to connect to " & key & ". Data cannot currently be used from this server"
                End If
            Next

            If return_message = "WARNING!" Then return_message = "" 'i.e., no issue found in the definition file
        Catch ex As Exception
            Debug.Print("BA_Read_Settings Exception: " & ex.Message)
        End Try

        Return return_message
    End Function

    Public Sub BA_SetSettingPath()
        'BA_Settings_Filepath = Environ(BA_Settings_PathVariable) 'try the BAGIS folder first

        'If Len(BA_Settings_Filepath) = 0 Then 'then, try the TMP folder
        '    BA_Settings_Filepath = Environ("TMP")
        'End If

        'If Len(BA_Settings_Filepath) = 0 Then 'then, try the TEMP folder
        '    BA_Settings_Filepath = Environ("TEMP")
        'End If

        'If Len(BA_Settings_Filepath) = 0 Then 'lastly, try the ARCGISHOME folder
        '    BA_Settings_Filepath = Environ("ARCGISHOME")
        'End If

        ''Ver1E update - 3 lines added
        'If Len(BA_Settings_Filepath) = 0 Then 'lastly, try the AGSDESKTOPJAVA folder for ArcGIS 10
        '    BA_Settings_Filepath = Environ("AGSDESKTOPJAVA")
        'End If

        ''remove the slash if it's in the string
        'If Right(BA_Settings_Filepath, 1) = "\" Then BA_Settings_Filepath = Left(BA_Settings_Filepath, Len(BA_Settings_Filepath) - 1)
        BA_Settings_Filepath = BA_GetSettingsPath()
    End Sub

    'read the basinanalysis.def file into the BA_SystemSettings variable
    'This version reads but does not validate the data sources
    Public Function BA_ReadBAGISSettings(ByVal AOI_Folderpath As String) As Integer
        Dim fileno As Integer = 0
        Dim return_value As Integer = -1
        Dim linestring As String
        Dim source_namepath As String

        If Len(AOI_Folderpath) = 0 Then
            Return -1
        End If

        source_namepath = AOI_Folderpath & "\" & BA_Settings_Filename

        'check if file exist
        If Len(Dir(source_namepath, vbNormal)) = 0 Then 'file doesn't exist
            Return -1
        End If

        Try
            Using sr As StreamReader = File.OpenText(source_namepath)

                linestring = sr.ReadLine() 'read the keyword line                                     '1

                'check version
                If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                    MsgBox("The version of setting file is different from BAGIS version!" & vbCrLf & _
                           "Please save setting using settings form to update the version.", vbOKOnly, "BAGIS Settings Version Error")
                    Return -1
                End If

                'Check for network connectivity
                If Not BA_IsNetworkAvailable(0) Then
                    MessageBox.Show("Your computer is not connected to the network. You may be unable to access some of the data layers.")
                End If

                linestring = sr.ReadLine() 'three lines for reference layers                            '2
                BA_SystemSettings.Ref_Terrain = Trim(linestring) 'terrain
                linestring = sr.ReadLine()                                                              '3
                BA_SystemSettings.Ref_Drainage = Trim(linestring) 'drainage
                linestring = sr.ReadLine()                                                              '4
                BA_SystemSettings.Ref_Watershed = Trim(linestring) 'watershed

                linestring = sr.ReadLine()  '10 meters DEM layer                                        '5
                BA_SystemSettings.DEM10M = Trim(linestring)
                linestring = sr.ReadLine() '30 meters DEM layer                                         '6
                BA_SystemSettings.DEM30M = Trim(linestring)

                linestring = sr.ReadLine()  'read preferred DEM setting                                 '7
                'if True then 10 meter is preferred, else 30 meter
                If UCase(Trim(linestring)) = "TRUE" Then
                    BA_SystemSettings.DEM10MPreferred = True
                Else
                    BA_SystemSettings.DEM10MPreferred = False
                End If

                linestring = sr.ReadLine() 'read DEM Z unit setting                                     '8
                BA_SystemSettings.DEM_ZUnit_IsMeter = Trim(linestring)

                linestring = sr.ReadLine()                                                              '9
                BA_SystemSettings.PourPointLayer = Trim(linestring)

                linestring = sr.ReadLine() 'the linestring contains the selected field name             '10
                BA_SystemSettings.PourPointField = Trim(linestring)

                linestring = sr.ReadLine() 'area field                                                  '11
                BA_SystemSettings.PourAreaField = Trim(linestring)

                linestring = sr.ReadLine() 'area unit                                                   '12
                BA_SystemSettings.PourAreaUnit = Trim(linestring)

                'set snotel file
                linestring = sr.ReadLine()                                                              '13
                BA_SystemSettings.GenerateAOIOnly = False
                BA_SystemSettings.SNOTELLayer = Trim(linestring)
                If String.IsNullOrEmpty(BA_SystemSettings.SNOTELLayer) Then
                    BA_SystemSettings.GenerateAOIOnly = True
                End If

                'set snotel field
                linestring = sr.ReadLine()  'elevation field                                            '14
                BA_SystemSettings.SNOTEL_ElevationField = Trim(linestring)

                linestring = sr.ReadLine() 'name field                                                  '15
                BA_SystemSettings.SNOTEL_NameField = Trim(linestring)

                linestring = sr.ReadLine() 'elevation unit                                              '16
                BA_SystemSettings.SNOTEL_ZUnit_IsMeter = Trim(linestring)

                'read snow course
                linestring = sr.ReadLine()                                                              '17
                BA_SystemSettings.SCourseLayer = Trim(linestring)
                If String.IsNullOrEmpty(BA_SystemSettings.SCourseLayer) Then
                    BA_SystemSettings.GenerateAOIOnly = True
                End If

                'set snow course fields
                'set snotel field
                linestring = sr.ReadLine()  'elevation field                                            '18
                BA_SystemSettings.SCourse_ElevationField = Trim(linestring)

                linestring = sr.ReadLine()  'name field                                                 '19
                BA_SystemSettings.SCourse_NameField = Trim(linestring)

                linestring = sr.ReadLine()  'elevation unit                                             '20
                BA_SystemSettings.SCourse_ZUnit_IsMeter = Trim(linestring)

                'PRISM data
                linestring = sr.ReadLine()                                                              '21
                BA_SystemSettings.PRISMFolder = Trim(linestring)
                If String.IsNullOrEmpty(BA_SystemSettings.PRISMFolder) Then
                    BA_SystemSettings.GenerateAOIOnly = True
                End If

                BA_SystemSettings.listCount = 0
                Do While sr.Peek() >= 0
                    linestring = sr.ReadLine() 'read a line to the lineString variable               '21+nlayers    '22 to (21+listcount)
                    If Len(Trim(linestring)) > 0 Then 'a blank line was read
                        BA_SystemSettings.listCount = BA_SystemSettings.listCount + 1
                    End If
                Loop

                'BA_SystemSettings.GenerateAOIOnly = False 'the definition file doesn't contain information on Generate AOI Only option, but sets it to true when SNOTEL, Snow Course, or PRISM data is missing. 
                BA_SystemSettings.Status = 1
                sr.Close()
            End Using

            If BA_SystemSettings.listCount > 0 Then
                Array.Resize(BA_SystemSettings.OtherLayers, BA_SystemSettings.listCount)

                'reopen the file and skip the first 21 lines to read the list of participating layers 
                Using sr As StreamReader = File.OpenText(source_namepath)
                    'skip lines that before the list of other layers
                    For iCount As Integer = 1 To 21 Step 1
                        linestring = sr.ReadLine()
                    Next

                    Dim i As Integer = 0
                    linestring = sr.ReadLine()

                    Do While Not linestring Is Nothing

                        If Len(Trim(linestring)) > 0 Then 'a blank line was read
                            BA_SystemSettings.OtherLayers(i) = linestring
                            i += 1
                        End If
                        linestring = sr.ReadLine()
                    Loop

                    sr.Close()
                End Using
            End If
            Return 1
        Catch
            Return -1
        End Try
    End Function

    ''read the other participating layers in the basinanalysis.def file into the BA_SystemSettings.OtherLayers variable
    ''BA_ReadBAGISSettings function must be called first"
    ''return the number of layers in the list, a negative value indicates error
    'Public Function BA_ReadBAGISSettingsOtherLayers(ByVal AOI_Folderpath As String) As Integer

    '    Dim fileno As Integer = 0
    '    Dim return_value As Integer = -1
    '    Dim linestring As String
    '    Dim source_namepath As String

    '    If BA_SystemSettings.Status <> 1 Then
    '        BA_ReadBAGISSettingsOtherLayers = return_value
    '        MsgBox("BA_ReadBAGISSettingsOtherLayers Error! BA_ReadBAGISSettings function must be called first.")
    '        Exit Function
    '    End If

    '    If Len(AOI_Folderpath) = 0 Then
    '        BA_ReadBAGISSettingsOtherLayers = return_value
    '        Exit Function
    '    End If

    '    source_namepath = AOI_Folderpath & "\" & BA_Settings_Filename

    '    'check if file exist
    '    If Len(Dir(source_namepath, vbNormal)) = 0 Then 'file doesn't exist
    '        BA_ReadBAGISSettingsOtherLayers = return_value
    '        Exit Function
    '    End If

    '    If BA_SystemSettings.listCount <= 0 Then 'no other layers listed in the settings window
    '        return_value = 0
    '        BA_ReadBAGISSettingsOtherLayers = return_value
    '        Exit Function
    '    End If

    '    Dim nLayers As Integer = BA_SystemSettings.listCount
    '    Array.Resize(BA_SystemSettings.OtherLayers, nLayers)

    '    Try
    '        Using sr As StreamReader = File.OpenText(source_namepath)
    '            'skip lines that before the list of other layers
    '            linestring = sr.ReadLine() 'read the keyword line                                     '1

    '            linestring = sr.ReadLine() 'three lines for reference layers                            '2
    '            linestring = sr.ReadLine()                                                              '3
    '            linestring = sr.ReadLine()                                                              '4

    '            linestring = sr.ReadLine() '10 meters DEM layer                                        '5
    '            linestring = sr.ReadLine() '30 meters DEM layer                                         '6
    '            linestring = sr.ReadLine() 'read preferred DEM setting                                 '7
    '            linestring = sr.ReadLine() 'read DEM Z unit setting                                     '8
    '            linestring = sr.ReadLine()                                                              '9
    '            linestring = sr.ReadLine() 'the linestring contains the selected field name             '10
    '            linestring = sr.ReadLine() 'area field                                                  '11
    '            linestring = sr.ReadLine() 'area unit                                                   '12

    '            'set snotel file
    '            linestring = sr.ReadLine()                                                              '13
    '            'set snotel field
    '            linestring = sr.ReadLine() 'elevation field                                            '14
    '            linestring = sr.ReadLine() 'name field                                                  '15
    '            linestring = sr.ReadLine() 'elevation unit                                              '16

    '            'read snow course
    '            linestring = sr.ReadLine()                                                              '17
    '            'set snow course fields
    '            'set snotel field
    '            linestring = sr.ReadLine()  'elevation field                                            '18
    '            linestring = sr.ReadLine()  'name field                                                 '19
    '            linestring = sr.ReadLine()  'elevation unit                                             '20

    '            'PRISM data
    '            linestring = sr.ReadLine()                                                              '21

    '            Dim i As Integer = 0
    '            Do While Not linestring Is Nothing
    '                linestring = sr.ReadLine()
    '                If Not linestring Is Nothing Then
    '                    If Len(Trim(linestring)) > 0 Then 'a blank line was read
    '                        BA_SystemSettings.OtherLayers(i) = linestring
    '                        i += 1
    '                    End If
    '                Else
    '                    Exit Do
    '                End If
    '            Loop

    '            return_value = i
    '            sr.Close()
    '            'Print_BAGISSettings
    '        End Using
    '    Catch
    '    End Try

    '    BA_ReadBAGISSettingsOtherLayers = return_value
    'End Function

    Public Sub SetValue(ByVal value As Object, ByVal index As Integer)
        BA_SystemSettings.OtherLayers(index) = value.ToString
    End Sub

    'copy the basinanalysis.def file to the AOI folder
    Public Function BA_CopyBAGISSettings(ByVal target_folder As String) As Integer
        'check if the process is for creating AOI folders
        'Dim commandstr As String
        'Dim comspec_string As String = ""
        'Dim ReturnCode As Integer = -1
        Dim sourcefilepathname As String = BA_Settings_Filepath & "\" & BA_Settings_Filename
        Dim targetfilepathname As String = target_folder & "\" & BA_Settings_Filename

        Try
            File.Copy(sourcefilepathname, targetfilepathname)
            Return 1
        Catch ex As Exception
            MsgBox("BA_CopyBAGISSetting Exception: " & ex.Message)
            Return -1
        End Try

        'If Len(Dir(sourcefilepathname, vbNormal)) <> 0 Then 'i.e., file exists
        '    'copy the source file to the target folder
        '    commandstr = Environ("COMSPEC") & " /c copy /A /Y " & Chr(34) & sourcefilepathname & Chr(34) & " " & Chr(34) & targetfilepathname & Chr(34)
        '    If Not bShellAndWait(commandstr, vbMinimizedFocus) Then
        '        Err.Raise(9999)
        '    Else
        '        ReturnCode = 1
        '    End If
        'End If
    End Function

    Public Function BA_ReadDefaultSettingsFromJson(ByVal filePath As String) As Settings

        Try
            Dim bytes() As Byte = New Byte(0) {}
            Dim settings As Settings = Nothing
            Using fsSource As FileStream = New FileStream(filePath, _
                FileMode.Open, FileAccess.Read)
                ' Read the source file into a byte array.
                ReDim bytes((fsSource.Length) - 1)
                Dim numBytesToRead As Integer = CType(fsSource.Length, Integer)
                Dim numBytesRead As Integer = 0

                While (numBytesToRead > 0)
                    ' Read may return anything from 0 to numBytesToRead.
                    Dim n As Integer = fsSource.Read(bytes, numBytesRead, _
                        numBytesToRead)
                    ' Break when the end of the file is reached.
                    If (n = 0) Then
                        Exit While
                    End If
                    numBytesRead = (numBytesRead + n)
                    numBytesToRead = (numBytesToRead - n)
                End While
            End Using

            If bytes.Length > 0 Then
                Using memStream As MemoryStream = New MemoryStream(bytes)
                    settings = New Settings()
                    Dim ser As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(settings.[GetType]())
                    Return CType(ser.ReadObject(memStream), Settings)
                End Using
            End If
            Return settings
        Catch ex As Exception
            Debug.Print("BA_ReadDefaultSettingsFromJson Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function BA_QueryDefaultSettings(ByVal webserviceUrl As String) As Settings
        webserviceUrl = webserviceUrl & "/api/rest/desktop/settings/"
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(webserviceUrl & "?f=pjson")
        Try
            Dim settings = New Settings()
            Using resp As System.Net.WebResponse = req.GetResponse()
                Dim ser As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(settings.[GetType]())
                settings = CType(ser.ReadObject(resp.GetResponseStream), Settings)
            End Using
            Return settings
        Catch ex As Exception
            Debug.Print("BA_ReadDefaultSettingsFromJson Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

End Module
