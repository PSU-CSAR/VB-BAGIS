Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto

Public Class BtnDifferenceCondition
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim pMap As ESRI.ArcGIS.Carto.IMap = My.Document.FocusMap
            If BA_SiteScenarioValidMap(pMap) = False Then
                Exit Sub
            End If
            Dim pColor As IColor = New RgbColor
            pColor.RGB = RGB(197, 0, 255)   'Purple
            Dim pTempLayer As ILayer
            'Scenario 1 Representation
            For i = 0 To pMap.LayerCount - 1
                pTempLayer = pMap.Layer(i)
                If BA_MAPS_SCENARIO1_REPRESENTATION = pTempLayer.Name Then 'we have the right layer
                    If TypeOf pTempLayer Is IFeatureLayer Then
                        Dim gfLayer As IGeoFeatureLayer = CType(pTempLayer, IGeoFeatureLayer)
                        Dim renderer As IFeatureRenderer = gfLayer.Renderer
                        If TypeOf renderer Is ISimpleRenderer Then
                            Dim sRenderer As ISimpleRenderer = CType(renderer, ISimpleRenderer)
                            Dim pSymbol As ISymbol = sRenderer.Symbol
                            If TypeOf pSymbol Is ISimpleFillSymbol Then
                                Dim fSymbol As ISimpleFillSymbol = CType(pSymbol, ISimpleFillSymbol)
                                Dim fColor As IColor = fSymbol.Color
                                'Change display color to red, if it isn't already
                                If fColor.RGB <> pColor.RGB Then
                                    fSymbol.Color = pColor
                                    sRenderer.Symbol = pSymbol
                                End If
                            End If
                        End If
                    End If
                    Exit For
                End If
            Next
            'Scenario 2 representation
            pColor.RGB = RGB(255, 255, 0)   'Yellow
            For i = 0 To pMap.LayerCount - 1
                pTempLayer = pMap.Layer(i)
                If BA_MAPS_SCENARIO2_REPRESENTATION = pTempLayer.Name Then 'move the layer
                    If TypeOf pTempLayer Is IFeatureLayer Then
                        Dim gfLayer As IGeoFeatureLayer = CType(pTempLayer, IGeoFeatureLayer)
                        Dim renderer As IFeatureRenderer = gfLayer.Renderer
                        If TypeOf renderer Is ISimpleRenderer Then
                            Dim sRenderer As ISimpleRenderer = CType(renderer, ISimpleRenderer)
                            Dim pSymbol As ISymbol = sRenderer.Symbol
                            If TypeOf pSymbol Is ISimpleFillSymbol Then
                                Dim fSymbol As ISimpleFillSymbol = CType(pSymbol, ISimpleFillSymbol)
                                Dim fColor As IColor = fSymbol.Color
                                'Change display color to red, if it isn't already
                                If fColor.RGB <> pColor.RGB Then
                                    fSymbol.Color = pColor
                                    sRenderer.Symbol = pSymbol
                                End If
                            End If
                        End If
                    End If
                    Exit For
                End If
            Next
            'Reorder scenario layers so things are visible
            ''BA_MoveScenarioLayers()
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("An error occurred while trying to display the difference map.", "Error", Windows.Forms.MessageBoxButtons.OK)
            Debug.Print("OnClick" & ex.Message)
        End Try
        Dim Basin_Name As String = ""
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        BA_DisplayMap(My.Document, 9, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                                         "Difference of Representations")
        BA_ZoomToAOI(My.Document, AOIFolderBase)
    End Sub

    Public WriteOnly Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
