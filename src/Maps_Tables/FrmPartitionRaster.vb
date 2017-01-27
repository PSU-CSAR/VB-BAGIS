Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms

Public Class FrmPartitionRaster

    Private m_partitionRasterPath As String
    Private m_partitionField As String
    Private m_partitionValuesList As IList(Of String)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "AOI: " + BA_GetBareName(AOIFolderBase)

        LoadLstLayers()
    End Sub

    Private Sub LstRasters_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstRasters.SelectedIndexChanged
        LstFields.Items.Clear()
        LstValues.Items.Clear()
        If LstRasters.SelectedIndex > -1 Then
            Dim selItem As LayerListItem = LstRasters.SelectedItem
            m_partitionRasterPath = selItem.Value
            LoadFields()
            CmdClear.Enabled = True
        Else
            m_partitionRasterPath = Nothing
            CmdClear.Enabled = False
        End If
    End Sub

    Private Sub LoadLstLayers()
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
                Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                LstRasters.Items.Add(item)
            Next
        End If
    End Sub

    Private Sub LoadFields()
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim pField As IField
        Try
            Dim filePath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(m_partitionRasterPath, filePath)
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filePath)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(filePath, fileName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
            End If

            pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            pTable = pRasterBand.AttributeTable
            If pTable IsNot Nothing Then
                pFields = pTable.Fields
                For i As Integer = 0 To pFields.FieldCount - 1
                    ' Get the field at the given index.
                    pField = pFields.Field(i)
                    If Not pField.Name.Equals("OBJECTID") AndAlso Not pField.Name.Equals("Count") _
                        AndAlso Not pField.Name.Equals("COUNT") Then
                        LstFields.Items.Add(pField.Name)
                    End If
                Next i
            Else
                MessageBox.Show("Attribute table missing from raster. This raster cannot be used as a partition layer.")
            End If
        Catch ex As Exception
            Debug.Print("LoadFields Exception: " + ex.Message)
        Finally
            pGeoDataset = Nothing
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Public ReadOnly Property PartitionRasterPath As String
        Get
            Return m_partitionRasterPath
        End Get
    End Property

    Public ReadOnly Property PartitionValuesList As IList(Of String)
        Get
            Return m_partitionValuesList
        End Get
    End Property

    Public ReadOnly Property PartitionField As String
        Get
            Return m_partitionField
        End Get
    End Property

    Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
        Me.Close()
    End Sub

    Private Sub CmdClear_Click(sender As System.Object, e As System.EventArgs) Handles CmdClear.Click
        LstRasters.ClearSelected()
    End Sub

    Private Sub LstFields_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstFields.SelectedIndexChanged
        LstValues.Items.Clear()
        If LstFields.SelectedIndex > -1 Then
            m_partitionField = Convert.ToString(LstFields.SelectedItem)
            LoadValues()
            CmdClear.Enabled = True
        Else
            m_partitionRasterPath = Nothing
            CmdClear.Enabled = False
        End If
    End Sub

    Private Sub LoadValues()
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Try
            Dim filePath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(m_partitionRasterPath, filePath)
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filePath)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(filePath, fileName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
            End If
            pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            pTable = pRasterBand.AttributeTable
            If pTable IsNot Nothing Then
                pFields = pTable.Fields
                Dim idxValue As Integer = pFields.FindField(m_partitionField)
                pCursor = pTable.Search(Nothing, False)
                pRow = pCursor.NextRow
                Do While pRow IsNot Nothing
                    Dim nextValue As String = Convert.ToInt32(pRow.Value(idxValue))
                    LstValues.Items.Add(nextValue)
                    pRow = pCursor.NextRow
                Loop
                LstValues.Sorted = True
            Else
                MessageBox.Show("Attribute table missing from raster. This raster cannot be used as a partition layer.")
            End If
        Catch ex As Exception
            Debug.Print("LoadValues Exception: " + ex.Message)
        Finally
            pGeoDataset = Nothing
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pCursor = Nothing
            pRow = Nothing
            pTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
End Class