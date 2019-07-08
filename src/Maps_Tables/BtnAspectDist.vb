Public Class BtnAspectDist
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim Basin_Name As String = ""
        BA_DisplayMap(My.Document, 5, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, "ASPECT DISTRIBUTION")
        BAGIS_ClassLibrary.BA_ZoomToAOI(My.Document, AOIFolderBase)

        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmPublishMapPackage.AddinImpl)(My.ThisAddIn.IDs.FrmPublishMapPackage)
        Dim frmMapPackage As FrmPublishMapPackage = dockWindowAddIn.UI
        frmMapPackage.CurrentMap = BAGIS_ClassLibrary.BA_ExportMapAspectPdf

    End Sub

    Public Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
        Get
            Return Me.Enabled
        End Get
    End Property
    Protected Overrides Sub OnUpdate()

  End Sub
End Class
