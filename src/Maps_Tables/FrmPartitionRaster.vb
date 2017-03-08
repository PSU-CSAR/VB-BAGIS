Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmPartitionRaster

    Private m_partitionRasterPath As String
    Private m_cellSize As Double
    Private m_snapRasterPath As String

    Public Sub New(ByVal partRasterPath As String, _
                   ByVal snapRasterpath As String, ByVal cellSize As Double)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "AOI: " + BA_GetBareName(AOIFolderBase)

        m_snapRasterPath = snapRasterpath
        m_cellSize = cellSize
        LoadLstLayers(partRasterPath)

    End Sub

    Private Sub LstRasters_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstRasters.SelectedIndexChanged
        If LstRasters.SelectedIndex > -1 Then
            Dim selItem As LayerListItem = LstRasters.SelectedItem
            CmdClear.Enabled = True
            CmdCreateRaster.Enabled = True
        Else
            m_partitionRasterPath = Nothing
            CmdClear.Enabled = False
            CmdCreateRaster.Enabled = False
        End If
    End Sub

    Private Sub LoadLstLayers(ByVal partRasterPath As String)
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)

        'display raster layers
        Dim RasterCount As Integer = UBound(AOIRasterList)
        If RasterCount > 0 Then
            For i = 1 To RasterCount
                Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                'Only discrete rasters can be used for partition
                If isDiscrete = True Then
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstRasters.Items.Add(item)
                    If Not String.IsNullOrEmpty(partRasterPath) Then
                        Dim fileName As String = BA_GetBareName(partRasterPath)
                        If item.Name.Equals(fileName.Substring(BA_RasterPartPrefix.Length)) Then
                            LstRasters.SetSelected(i - 1, True)
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    'Private Sub LoadFields()
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pRasterBandCollection As IRasterBandCollection = Nothing
    '    Dim pRasterBand As IRasterBand = Nothing
    '    Dim pTable As ITable = Nothing
    '    Dim pFields As IFields = Nothing
    '    Dim pField As IField
    '    Try
    '        Dim filePath As String = "PleaseReturn"
    '        Dim fileName As String = BA_GetBareName(m_partitionRasterPath, filePath)
    '        Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filePath)
    '        If pWorkspaceType = WorkspaceType.Raster Then
    '            pGeoDataset = BA_OpenRasterFromFile(filePath, fileName)
    '        ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
    '            pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
    '        End If

    '        pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
    '        pRasterBand = pRasterBandCollection.Item(0)
    '        pTable = pRasterBand.AttributeTable
    '        If pTable IsNot Nothing Then
    '            pFields = pTable.Fields
    '            For i As Integer = 0 To pFields.FieldCount - 1
    '                ' Get the field at the given index.
    '                pField = pFields.Field(i)
    '                If Not pField.Name.Equals("OBJECTID") AndAlso Not pField.Name.Equals("Count") _
    '                    AndAlso Not pField.Name.Equals("COUNT") Then
    '                    LstFields.Items.Add(pField.Name)
    '                    If pField.Name.Equals(m_partitionField) Then
    '                        LstFields.SetSelected(i - 1, True)
    '                    End If
    '                End If
    '            Next i
    '        Else
    '            MessageBox.Show("Attribute table missing from raster. This raster cannot be used as a partition layer.")
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("LoadFields Exception: " + ex.Message)
    '    Finally
    '        pGeoDataset = Nothing
    '        pRasterBand = Nothing
    '        pRasterBandCollection = Nothing
    '        pTable = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Sub

    Public ReadOnly Property PartitionRasterPath As String
        Get
            Return m_partitionRasterPath
        End Get
    End Property

    Public ReadOnly Property PartitionRasterName As String
        Get
            If String.IsNullOrEmpty(m_partitionRasterPath) Then
                Return Nothing
            Else
                Dim fileName As String = BA_GetBareName(m_partitionRasterPath)
                Return fileName.Substring(BA_RasterPartPrefix.Length)
            End If
        End Get
    End Property

    Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
        Me.Close()
    End Sub

    Private Sub CmdClear_Click(sender As System.Object, e As System.EventArgs) Handles CmdClear.Click
        LstRasters.ClearSelected()
    End Sub

    'Private Sub LoadValues()
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pRasterBandCollection As IRasterBandCollection = Nothing
    '    Dim pRasterBand As IRasterBand = Nothing
    '    Dim pTable As ITable = Nothing
    '    Dim pFields As IFields = Nothing
    '    Dim pCursor As ICursor = Nothing
    '    Dim pRow As IRow = Nothing
    '    Try
    '        Dim filePath As String = "PleaseReturn"
    '        Dim fileName As String = BA_GetBareName(m_partitionRasterPath, filePath)
    '        Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filePath)
    '        If pWorkspaceType = WorkspaceType.Raster Then
    '            pGeoDataset = BA_OpenRasterFromFile(filePath, fileName)
    '        ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
    '            pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
    '        End If
    '        pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
    '        pRasterBand = pRasterBandCollection.Item(0)
    '        pTable = pRasterBand.AttributeTable
    '        Dim prevSelectedValues As List(Of String) = New List(Of String)
    '        If m_partitionValuesList IsNot Nothing AndAlso m_partitionValuesList.Count > 0 Then
    '            prevSelectedValues.AddRange(m_partitionValuesList)
    '        End If
    '        Dim i As Int32 = 0
    '        If pTable IsNot Nothing Then
    '            pFields = pTable.Fields
    '            Dim idxValue As Integer = pFields.FindField(m_partitionField)
    '            pCursor = pTable.Search(Nothing, False)
    '            pRow = pCursor.NextRow
    '            Do While pRow IsNot Nothing
    '                Dim nextValue As String = Convert.ToString(pRow.Value(idxValue))
    '                LstValues.Items.Add(nextValue)
    '                If prevSelectedValues.Count > 0 Then
    '                    If prevSelectedValues.Contains(nextValue) Then
    '                        LstValues.SetSelected(i, True)
    '                    End If
    '                End If
    '                pRow = pCursor.NextRow
    '                i += 1
    '            Loop
    '            LstValues.Sorted = True
    '        Else
    '            MessageBox.Show("Attribute table missing from raster. This raster cannot be used as a partition layer.")
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("LoadValues Exception: " + ex.Message)
    '    Finally
    '        pGeoDataset = Nothing
    '        pRasterBand = Nothing
    '        pRasterBandCollection = Nothing
    '        pCursor = Nothing
    '        pRow = Nothing
    '        pTable = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Sub

    Private Sub CmdCreateRaster_Click(sender As System.Object, e As System.EventArgs) Handles CmdCreateRaster.Click
        If LstRasters.SelectedItem IsNot Nothing Then
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
            Dim progressDialog2 As IProgressDialog2 = Nothing

            Try
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Creating partition raster layer", "Running...")
                pStepProg.Show()
                progressDialog2.ShowDialog()
                pStepProg.Step()

                ' Delete any pre-existing partition rasters
                Dim AOIVectorList() As String = Nothing
                Dim AOIRasterList() As String = Nothing
                Dim layerPath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Analysis)
                BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
                Dim RasterCount As Integer = UBound(AOIRasterList)
                If RasterCount > 0 Then
                    For i = 1 To RasterCount
                        If AOIRasterList(i).IndexOf(BA_RasterPartPrefix) = 0 Then
                            Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                            BA_RemoveRasterFromGDB(layerPath, AOIRasterList(i))
                        End If
                    Next
                End If

                Dim selItem As LayerListItem = LstRasters.SelectedItem
                Dim partRasterPath As String = selItem.Value
                Dim outputRasterName As String = BA_RasterPartPrefix + selItem.Name
                Dim outputRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + outputRasterName
                Dim cellSize As Double = BA_CellSize(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), selItem.Name)
                Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
                'Only run statistics if the cell sizes are different
                If cellSize <> m_cellSize Then
                    Dim neighborhood As String = "Rectangle " & m_cellSize & " " & m_cellSize & " Map"
                    success = BA_FocalStatistics_CellSize(partRasterPath, outputRasterPath, neighborhood, _
                                                          StatisticsFieldName.MAJORITY.ToString, m_snapRasterPath, _
                                                          Convert.ToString(m_cellSize))
                Else
                    success = BA_Copy(partRasterPath, outputRasterPath)
                End If
                If success = BA_ReturnCode.Success Then
                    m_partitionRasterPath = outputRasterPath
                    MessageBox.Show("Partition raster has been created and will be used in analysis!")
                    Me.Close()
                Else
                    m_partitionRasterPath = Nothing
                    MessageBox.Show("Partition raster could not be created and cannot be used in analysis!")
                End If
            Catch ex As Exception
                Debug.Print("CmdCreateRaster_Click Exception" & ex.Message)
            Finally
                If pStepProg IsNot Nothing Then
                    pStepProg.Hide()
                    pStepProg = Nothing
                End If
                If progressDialog2 IsNot Nothing Then
                    progressDialog2.HideDialog()
                    progressDialog2 = Nothing
                End If
            End Try
        End If
    End Sub
End Class