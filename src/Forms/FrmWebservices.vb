Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.GISClient
Imports System.Text
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary


Public Class FrmWebservices



    Private Sub BtnSet_Click(sender As System.Object, e As System.EventArgs) Handles BtnSet.Click
        Dim pGxDialog As IGxDialog = New GxDialog
        pGxDialog.AllowMultiSelect = False
        pGxDialog.Title = "Browse For AGIS Map Service"
        'Dim pGxFilter As IGxObjectFilter = New GxFilterMapServers
        Dim pGxFilter As IGxObjectFilter = New GxFilterMapDatasetsLayersAndResults
        pGxDialog.ObjectFilter = pGxFilter
        Dim pGxObjects As IEnumGxObject = Nothing
        If pGxDialog.DoModalOpen(0, pGxObjects) Then
            pGxObjects.Reset()
            Dim pGxObj As IGxObject = pGxObjects.Next
            Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
            Dim sName As IAGSServerObjectName = agsObj.AGSServerObjectName
            Dim url As String = agsObj.AGSServerObjectName.URL
            Dim propertySet As IPropertySet = agsObj.AGSServerObjectName.AGSServerConnectionName.ConnectionProperties()
            Dim names(propertySet.Count - 1) As Object
            Dim values(propertySet.Count - 1) As Object
            propertySet.GetAllProperties(names, values)
            Dim sb As StringBuilder = New StringBuilder()
            For i As Integer = 0 To propertySet.Count - 1
                sb.Append(CStr(names(i)) & vbCrLf)
                sb.Append(values(i).ToString & vbCrLf)
            Next
            Windows.Forms.MessageBox.Show(sb.ToString)
        End If


    End Sub

    Private Sub BtnFields_Click(sender As System.Object, e As System.EventArgs) Handles BtnFields.Click
        cboFields.Items.Clear()
        Dim fieldNames As IList(Of String) = BA_QueryFeatureServiceFieldNames(TxtWebService.Text, ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
        For Each fName As String In fieldNames
            cboFields.Items.Add(fName)
        Next
        cboFields.SelectedIndex = 0
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        BtnFields.Focus()
    End Sub

    Private Sub BtnLayerFile_Click(sender As System.Object, e As System.EventArgs) Handles BtnLayerFile.Click

    End Sub
End Class