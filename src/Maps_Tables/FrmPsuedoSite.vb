Public Class FrmPsuedoSite

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CmboxPrecipType.Items.Clear()
        With CmboxPrecipType
            .Items.Add("Annual Precipitation")
            .Items.Add("Jan - Mar Precipitation")
            .Items.Add("Apr - Jun Precipitation")
            .Items.Add("Jul - Sep Precipitation")
            .Items.Add("Oct - Dec Precipitation")
            .Items.Add("Custom")
            .SelectedIndex = 0
        End With

        CmboxBegin.Items.Clear()
        With CmboxBegin
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 0
        End With

        CmboxEnd.Items.Clear()
        With CmboxEnd
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 11
        End With
    End Sub

    Private Sub BtnFindSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnFindSite.Click

    End Sub

    Private Sub CkElev_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkElev.CheckedChanged
        GrpElevation.Enabled = CkElev.Checked
    End Sub

    Private Sub CkPrecip_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkPrecip.CheckedChanged
        GrpPrecipitation.Enabled = CkPrecip.Checked
    End Sub

    Private Sub CkProximity_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkProximity.CheckedChanged
        GrpProximity.Enabled = CkProximity.Checked
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
End Class