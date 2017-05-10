<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPsuedoSite
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LstVectors = New System.Windows.Forms.ListBox()
        Me.LblBufferDistance = New System.Windows.Forms.Label()
        Me.txtBufferDistance = New System.Windows.Forms.TextBox()
        Me.CkElev = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CkPrecip = New System.Windows.Forms.CheckBox()
        Me.GrpElevation = New System.Windows.Forms.GroupBox()
        Me.lblElevation = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtMinElev = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtMaxElev = New System.Windows.Forms.TextBox()
        Me.txtLower = New System.Windows.Forms.TextBox()
        Me.TxtRange = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblElevRange = New System.Windows.Forms.Label()
        Me.TxtUpperRange = New System.Windows.Forms.TextBox()
        Me.GrpPrecipitation = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtMinPrecip = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtMaxPrecip = New System.Windows.Forms.TextBox()
        Me.TxtPrecipLower = New System.Windows.Forms.TextBox()
        Me.txtRangePrecip = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TxtPrecipUpper = New System.Windows.Forms.TextBox()
        Me.CmdPrism = New System.Windows.Forms.Button()
        Me.CmboxEnd = New System.Windows.Forms.ComboBox()
        Me.CmboxBegin = New System.Windows.Forms.ComboBox()
        Me.CmboxPrecipType = New System.Windows.Forms.ComboBox()
        Me.lblEndMonth = New System.Windows.Forms.Label()
        Me.lblBeginMonth = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GrpProximity = New System.Windows.Forms.GroupBox()
        Me.CkProximity = New System.Windows.Forms.CheckBox()
        Me.BtnFindSite = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMap = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtSiteName = New System.Windows.Forms.TextBox()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GrpElevation.SuspendLayout()
        Me.GrpPrecipitation.SuspendLayout()
        Me.GrpProximity.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(9, 18)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 16)
        Me.Label5.TabIndex = 69
        Me.Label5.Text = "Data Layer:"
        '
        'LstVectors
        '
        Me.LstVectors.FormattingEnabled = True
        Me.LstVectors.ItemHeight = 16
        Me.LstVectors.Location = New System.Drawing.Point(12, 40)
        Me.LstVectors.Name = "LstVectors"
        Me.LstVectors.Size = New System.Drawing.Size(197, 52)
        Me.LstVectors.TabIndex = 70
        '
        'LblBufferDistance
        '
        Me.LblBufferDistance.AutoSize = True
        Me.LblBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblBufferDistance.Location = New System.Drawing.Point(215, 42)
        Me.LblBufferDistance.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblBufferDistance.Name = "LblBufferDistance"
        Me.LblBufferDistance.Size = New System.Drawing.Size(153, 16)
        Me.LblBufferDistance.TabIndex = 72
        Me.LblBufferDistance.Text = "Buffer Distance (Meters):"
        '
        'txtBufferDistance
        '
        Me.txtBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBufferDistance.Location = New System.Drawing.Point(372, 42)
        Me.txtBufferDistance.Margin = New System.Windows.Forms.Padding(2)
        Me.txtBufferDistance.Name = "txtBufferDistance"
        Me.txtBufferDistance.Size = New System.Drawing.Size(41, 20)
        Me.txtBufferDistance.TabIndex = 71
        Me.txtBufferDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CkElev
        '
        Me.CkElev.AutoSize = True
        Me.CkElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkElev.Location = New System.Drawing.Point(19, 137)
        Me.CkElev.Name = "CkElev"
        Me.CkElev.Size = New System.Drawing.Size(15, 14)
        Me.CkElev.TabIndex = 74
        Me.CkElev.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(0, 97)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 16)
        Me.Label7.TabIndex = 75
        Me.Label7.Text = "Include"
        '
        'CkPrecip
        '
        Me.CkPrecip.AutoSize = True
        Me.CkPrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkPrecip.Location = New System.Drawing.Point(19, 244)
        Me.CkPrecip.Name = "CkPrecip"
        Me.CkPrecip.Size = New System.Drawing.Size(15, 14)
        Me.CkPrecip.TabIndex = 77
        Me.CkPrecip.UseVisualStyleBackColor = True
        '
        'GrpElevation
        '
        Me.GrpElevation.Controls.Add(Me.lblElevation)
        Me.GrpElevation.Controls.Add(Me.Label23)
        Me.GrpElevation.Controls.Add(Me.Label24)
        Me.GrpElevation.Controls.Add(Me.txtMinElev)
        Me.GrpElevation.Controls.Add(Me.Label1)
        Me.GrpElevation.Controls.Add(Me.Label4)
        Me.GrpElevation.Controls.Add(Me.TxtMaxElev)
        Me.GrpElevation.Controls.Add(Me.txtLower)
        Me.GrpElevation.Controls.Add(Me.TxtRange)
        Me.GrpElevation.Controls.Add(Me.Label3)
        Me.GrpElevation.Controls.Add(Me.LblElevRange)
        Me.GrpElevation.Controls.Add(Me.TxtUpperRange)
        Me.GrpElevation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpElevation.Location = New System.Drawing.Point(57, 112)
        Me.GrpElevation.Name = "GrpElevation"
        Me.GrpElevation.Size = New System.Drawing.Size(487, 80)
        Me.GrpElevation.TabIndex = 79
        Me.GrpElevation.TabStop = False
        Me.GrpElevation.Text = "Elevation"
        '
        'lblElevation
        '
        Me.lblElevation.AutoSize = True
        Me.lblElevation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElevation.Location = New System.Drawing.Point(5, 22)
        Me.lblElevation.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblElevation.Name = "lblElevation"
        Me.lblElevation.Size = New System.Drawing.Size(149, 16)
        Me.lblElevation.TabIndex = 70
        Me.lblElevation.Text = "DEM Elevation (Meters)"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(177, 22)
        Me.Label23.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(32, 16)
        Me.Label23.TabIndex = 69
        Me.Label23.Text = "Min:"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(264, 22)
        Me.Label24.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(36, 16)
        Me.Label24.TabIndex = 71
        Me.Label24.Text = "Max:"
        '
        'txtMinElev
        '
        Me.txtMinElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMinElev.Location = New System.Drawing.Point(213, 22)
        Me.txtMinElev.Margin = New System.Windows.Forms.Padding(2)
        Me.txtMinElev.Name = "txtMinElev"
        Me.txtMinElev.ReadOnly = True
        Me.txtMinElev.Size = New System.Drawing.Size(47, 15)
        Me.txtMinElev.TabIndex = 72
        Me.txtMinElev.Text = "1816"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(369, 22)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 16)
        Me.Label1.TabIndex = 73
        Me.Label1.Text = "Range:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(169, 47)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 16)
        Me.Label4.TabIndex = 80
        Me.Label4.Text = "Lower:"
        '
        'TxtMaxElev
        '
        Me.TxtMaxElev.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtMaxElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtMaxElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMaxElev.ForeColor = System.Drawing.Color.Blue
        Me.TxtMaxElev.Location = New System.Drawing.Point(307, 22)
        Me.TxtMaxElev.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtMaxElev.Name = "TxtMaxElev"
        Me.TxtMaxElev.ReadOnly = True
        Me.TxtMaxElev.Size = New System.Drawing.Size(53, 15)
        Me.TxtMaxElev.TabIndex = 74
        Me.TxtMaxElev.Text = "3536"
        '
        'txtLower
        '
        Me.txtLower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLower.Location = New System.Drawing.Point(218, 46)
        Me.txtLower.Margin = New System.Windows.Forms.Padding(2)
        Me.txtLower.Name = "txtLower"
        Me.txtLower.Size = New System.Drawing.Size(41, 20)
        Me.txtLower.TabIndex = 79
        Me.txtLower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtRange
        '
        Me.TxtRange.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtRange.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRange.ForeColor = System.Drawing.Color.Blue
        Me.TxtRange.Location = New System.Drawing.Point(422, 22)
        Me.TxtRange.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtRange.Name = "TxtRange"
        Me.TxtRange.ReadOnly = True
        Me.TxtRange.Size = New System.Drawing.Size(52, 15)
        Me.TxtRange.TabIndex = 75
        Me.TxtRange.Text = "2722"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(279, 47)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 16)
        Me.Label3.TabIndex = 78
        Me.Label3.Text = "Upper:"
        '
        'LblElevRange
        '
        Me.LblElevRange.AutoSize = True
        Me.LblElevRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblElevRange.Location = New System.Drawing.Point(5, 45)
        Me.LblElevRange.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblElevRange.Name = "LblElevRange"
        Me.LblElevRange.Size = New System.Drawing.Size(152, 16)
        Me.LblElevRange.TabIndex = 76
        Me.LblElevRange.Text = "Desired Range (Meters)"
        '
        'TxtUpperRange
        '
        Me.TxtUpperRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtUpperRange.Location = New System.Drawing.Point(332, 46)
        Me.TxtUpperRange.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtUpperRange.Name = "TxtUpperRange"
        Me.TxtUpperRange.Size = New System.Drawing.Size(41, 20)
        Me.TxtUpperRange.TabIndex = 77
        Me.TxtUpperRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GrpPrecipitation
        '
        Me.GrpPrecipitation.Controls.Add(Me.Label9)
        Me.GrpPrecipitation.Controls.Add(Me.Label10)
        Me.GrpPrecipitation.Controls.Add(Me.Label11)
        Me.GrpPrecipitation.Controls.Add(Me.txtMinPrecip)
        Me.GrpPrecipitation.Controls.Add(Me.Label12)
        Me.GrpPrecipitation.Controls.Add(Me.Label13)
        Me.GrpPrecipitation.Controls.Add(Me.txtMaxPrecip)
        Me.GrpPrecipitation.Controls.Add(Me.TxtPrecipLower)
        Me.GrpPrecipitation.Controls.Add(Me.txtRangePrecip)
        Me.GrpPrecipitation.Controls.Add(Me.Label14)
        Me.GrpPrecipitation.Controls.Add(Me.Label15)
        Me.GrpPrecipitation.Controls.Add(Me.TxtPrecipUpper)
        Me.GrpPrecipitation.Controls.Add(Me.CmdPrism)
        Me.GrpPrecipitation.Controls.Add(Me.CmboxEnd)
        Me.GrpPrecipitation.Controls.Add(Me.CmboxBegin)
        Me.GrpPrecipitation.Controls.Add(Me.CmboxPrecipType)
        Me.GrpPrecipitation.Controls.Add(Me.lblEndMonth)
        Me.GrpPrecipitation.Controls.Add(Me.lblBeginMonth)
        Me.GrpPrecipitation.Controls.Add(Me.Label8)
        Me.GrpPrecipitation.Enabled = False
        Me.GrpPrecipitation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpPrecipitation.Location = New System.Drawing.Point(57, 198)
        Me.GrpPrecipitation.Name = "GrpPrecipitation"
        Me.GrpPrecipitation.Size = New System.Drawing.Size(487, 146)
        Me.GrpPrecipitation.TabIndex = 80
        Me.GrpPrecipitation.TabStop = False
        Me.GrpPrecipitation.Text = "Precipitation"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(7, 94)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(132, 16)
        Me.Label9.TabIndex = 82
        Me.Label9.Text = "Precipitation (Inches)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(179, 94)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 16)
        Me.Label10.TabIndex = 81
        Me.Label10.Text = "Min:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(260, 94)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(36, 16)
        Me.Label11.TabIndex = 83
        Me.Label11.Text = "Max:"
        '
        'txtMinPrecip
        '
        Me.txtMinPrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinPrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinPrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinPrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtMinPrecip.Location = New System.Drawing.Point(215, 94)
        Me.txtMinPrecip.Margin = New System.Windows.Forms.Padding(2)
        Me.txtMinPrecip.Name = "txtMinPrecip"
        Me.txtMinPrecip.ReadOnly = True
        Me.txtMinPrecip.Size = New System.Drawing.Size(38, 15)
        Me.txtMinPrecip.TabIndex = 84
        Me.txtMinPrecip.Text = "-"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(345, 94)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(52, 16)
        Me.Label12.TabIndex = 85
        Me.Label12.Text = "Range:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(169, 117)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(47, 16)
        Me.Label13.TabIndex = 92
        Me.Label13.Text = "Lower:"
        '
        'txtMaxPrecip
        '
        Me.txtMaxPrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxPrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMaxPrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxPrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxPrecip.Location = New System.Drawing.Point(303, 94)
        Me.txtMaxPrecip.Margin = New System.Windows.Forms.Padding(2)
        Me.txtMaxPrecip.Name = "txtMaxPrecip"
        Me.txtMaxPrecip.ReadOnly = True
        Me.txtMaxPrecip.Size = New System.Drawing.Size(38, 15)
        Me.txtMaxPrecip.TabIndex = 86
        Me.txtMaxPrecip.Text = "-"
        '
        'TxtPrecipLower
        '
        Me.TxtPrecipLower.Enabled = False
        Me.TxtPrecipLower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPrecipLower.Location = New System.Drawing.Point(214, 115)
        Me.TxtPrecipLower.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtPrecipLower.Name = "TxtPrecipLower"
        Me.TxtPrecipLower.Size = New System.Drawing.Size(41, 20)
        Me.TxtPrecipLower.TabIndex = 91
        Me.TxtPrecipLower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRangePrecip
        '
        Me.txtRangePrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRangePrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRangePrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRangePrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtRangePrecip.Location = New System.Drawing.Point(398, 94)
        Me.txtRangePrecip.Margin = New System.Windows.Forms.Padding(2)
        Me.txtRangePrecip.Name = "txtRangePrecip"
        Me.txtRangePrecip.ReadOnly = True
        Me.txtRangePrecip.Size = New System.Drawing.Size(35, 15)
        Me.txtRangePrecip.TabIndex = 87
        Me.txtRangePrecip.Text = "-"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(279, 117)
        Me.Label14.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(49, 16)
        Me.Label14.TabIndex = 90
        Me.Label14.Text = "Upper:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(7, 117)
        Me.Label15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(150, 16)
        Me.Label15.TabIndex = 88
        Me.Label15.Text = "Desired Range (Inches)"
        '
        'TxtPrecipUpper
        '
        Me.TxtPrecipUpper.Enabled = False
        Me.TxtPrecipUpper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPrecipUpper.Location = New System.Drawing.Point(325, 115)
        Me.TxtPrecipUpper.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtPrecipUpper.Name = "TxtPrecipUpper"
        Me.TxtPrecipUpper.Size = New System.Drawing.Size(41, 20)
        Me.TxtPrecipUpper.TabIndex = 89
        Me.TxtPrecipUpper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdPrism
        '
        Me.CmdPrism.Location = New System.Drawing.Point(307, 20)
        Me.CmdPrism.Name = "CmdPrism"
        Me.CmdPrism.Size = New System.Drawing.Size(93, 30)
        Me.CmdPrism.TabIndex = 15
        Me.CmdPrism.Text = "Get Values"
        Me.CmdPrism.UseVisualStyleBackColor = True
        '
        'CmboxEnd
        '
        Me.CmboxEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxEnd.Enabled = False
        Me.CmboxEnd.FormattingEnabled = True
        Me.CmboxEnd.Location = New System.Drawing.Point(224, 54)
        Me.CmboxEnd.Name = "CmboxEnd"
        Me.CmboxEnd.Size = New System.Drawing.Size(67, 24)
        Me.CmboxEnd.TabIndex = 12
        '
        'CmboxBegin
        '
        Me.CmboxBegin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxBegin.Enabled = False
        Me.CmboxBegin.FormattingEnabled = True
        Me.CmboxBegin.Location = New System.Drawing.Point(96, 54)
        Me.CmboxBegin.Name = "CmboxBegin"
        Me.CmboxBegin.Size = New System.Drawing.Size(67, 24)
        Me.CmboxBegin.TabIndex = 13
        '
        'CmboxPrecipType
        '
        Me.CmboxPrecipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxPrecipType.FormattingEnabled = True
        Me.CmboxPrecipType.Location = New System.Drawing.Point(96, 22)
        Me.CmboxPrecipType.Name = "CmboxPrecipType"
        Me.CmboxPrecipType.Size = New System.Drawing.Size(195, 24)
        Me.CmboxPrecipType.TabIndex = 14
        '
        'lblEndMonth
        '
        Me.lblEndMonth.AutoSize = True
        Me.lblEndMonth.Enabled = False
        Me.lblEndMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndMonth.Location = New System.Drawing.Point(191, 59)
        Me.lblEndMonth.Name = "lblEndMonth"
        Me.lblEndMonth.Size = New System.Drawing.Size(28, 16)
        Me.lblEndMonth.TabIndex = 10
        Me.lblEndMonth.Text = "To:"
        '
        'lblBeginMonth
        '
        Me.lblBeginMonth.AutoSize = True
        Me.lblBeginMonth.Enabled = False
        Me.lblBeginMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBeginMonth.Location = New System.Drawing.Point(48, 59)
        Me.lblBeginMonth.Name = "lblBeginMonth"
        Me.lblBeginMonth.Size = New System.Drawing.Size(42, 16)
        Me.lblBeginMonth.TabIndex = 9
        Me.lblBeginMonth.Text = "From:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 16)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "PRISM Data"
        '
        'GrpProximity
        '
        Me.GrpProximity.Controls.Add(Me.Label5)
        Me.GrpProximity.Controls.Add(Me.LstVectors)
        Me.GrpProximity.Controls.Add(Me.txtBufferDistance)
        Me.GrpProximity.Controls.Add(Me.LblBufferDistance)
        Me.GrpProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpProximity.Location = New System.Drawing.Point(57, 359)
        Me.GrpProximity.Name = "GrpProximity"
        Me.GrpProximity.Size = New System.Drawing.Size(487, 100)
        Me.GrpProximity.TabIndex = 81
        Me.GrpProximity.TabStop = False
        Me.GrpProximity.Text = "Proximity"
        '
        'CkProximity
        '
        Me.CkProximity.AutoSize = True
        Me.CkProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkProximity.Location = New System.Drawing.Point(19, 388)
        Me.CkProximity.Name = "CkProximity"
        Me.CkProximity.Size = New System.Drawing.Size(15, 14)
        Me.CkProximity.TabIndex = 82
        Me.CkProximity.UseVisualStyleBackColor = True
        '
        'BtnFindSite
        '
        Me.BtnFindSite.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFindSite.Location = New System.Drawing.Point(260, 473)
        Me.BtnFindSite.Name = "BtnFindSite"
        Me.BtnFindSite.Size = New System.Drawing.Size(93, 23)
        Me.BtnFindSite.TabIndex = 83
        Me.BtnFindSite.Text = "Find Site"
        Me.BtnFindSite.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(441, 473)
        Me.BtnClose.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 22)
        Me.BtnClose.TabIndex = 73
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnMap
        '
        Me.BtnMap.Enabled = False
        Me.BtnMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMap.Location = New System.Drawing.Point(155, 473)
        Me.BtnMap.Name = "BtnMap"
        Me.BtnMap.Size = New System.Drawing.Size(93, 23)
        Me.BtnMap.TabIndex = 84
        Me.BtnMap.Text = "Map"
        Me.BtnMap.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 69)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 16)
        Me.Label2.TabIndex = 85
        Me.Label2.Text = "Site name:"
        '
        'TxtSiteName
        '
        Me.TxtSiteName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSiteName.Location = New System.Drawing.Point(85, 67)
        Me.TxtSiteName.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtSiteName.MaxLength = 49
        Me.TxtSiteName.Name = "TxtSiteName"
        Me.TxtSiteName.Size = New System.Drawing.Size(168, 22)
        Me.TxtSiteName.TabIndex = 86
        '
        'BtnClear
        '
        Me.BtnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClear.Location = New System.Drawing.Point(365, 473)
        Me.BtnClear.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(65, 22)
        Me.BtnClear.TabIndex = 87
        Me.BtnClear.Text = "Clear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(3, 8)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(541, 60)
        Me.TextBox1.TabIndex = 88
        Me.TextBox1.Text = "The auto Pseudo Site tool finds the most desirable site in the non-represented ar" & _
    "eas depicted in Scenario 1. Users can specify additional search constraints list" & _
    "ed below to narrow the search domain"
        '
        'FrmPsuedoSite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(561, 512)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.TxtSiteName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnMap)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnFindSite)
        Me.Controls.Add(Me.CkProximity)
        Me.Controls.Add(Me.GrpProximity)
        Me.Controls.Add(Me.GrpPrecipitation)
        Me.Controls.Add(Me.GrpElevation)
        Me.Controls.Add(Me.CkPrecip)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.CkElev)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "FrmPsuedoSite"
        Me.ShowIcon = False
        Me.Text = "Add Pseudo Site:"
        Me.GrpElevation.ResumeLayout(False)
        Me.GrpElevation.PerformLayout()
        Me.GrpPrecipitation.ResumeLayout(False)
        Me.GrpPrecipitation.PerformLayout()
        Me.GrpProximity.ResumeLayout(False)
        Me.GrpProximity.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LstVectors As System.Windows.Forms.ListBox
    Friend WithEvents LblBufferDistance As System.Windows.Forms.Label
    Friend WithEvents txtBufferDistance As System.Windows.Forms.TextBox
    Friend WithEvents CkElev As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CkPrecip As System.Windows.Forms.CheckBox
    Friend WithEvents GrpElevation As System.Windows.Forms.GroupBox
    Friend WithEvents lblElevation As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtMinElev As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxElev As System.Windows.Forms.TextBox
    Friend WithEvents txtLower As System.Windows.Forms.TextBox
    Friend WithEvents TxtRange As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LblElevRange As System.Windows.Forms.Label
    Friend WithEvents TxtUpperRange As System.Windows.Forms.TextBox
    Friend WithEvents GrpPrecipitation As System.Windows.Forms.GroupBox
    Friend WithEvents CmboxEnd As System.Windows.Forms.ComboBox
    Friend WithEvents CmboxBegin As System.Windows.Forms.ComboBox
    Friend WithEvents CmboxPrecipType As System.Windows.Forms.ComboBox
    Friend WithEvents lblEndMonth As System.Windows.Forms.Label
    Friend WithEvents lblBeginMonth As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtMinPrecip As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtMaxPrecip As System.Windows.Forms.TextBox
    Friend WithEvents TxtPrecipLower As System.Windows.Forms.TextBox
    Friend WithEvents txtRangePrecip As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TxtPrecipUpper As System.Windows.Forms.TextBox
    Friend WithEvents CmdPrism As System.Windows.Forms.Button
    Friend WithEvents GrpProximity As System.Windows.Forms.GroupBox
    Friend WithEvents CkProximity As System.Windows.Forms.CheckBox
    Friend WithEvents BtnFindSite As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnMap As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtSiteName As System.Windows.Forms.TextBox
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
