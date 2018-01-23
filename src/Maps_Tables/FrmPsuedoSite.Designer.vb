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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPsuedoSite))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LblBufferDistance = New System.Windows.Forms.Label()
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
        Me.PnlProximity = New System.Windows.Forms.Panel()
        Me.LblAddBufferDistance = New System.Windows.Forms.Label()
        Me.txtBufferDistance = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSaveProximity = New System.Windows.Forms.Button()
        Me.BtnCancelProximity = New System.Windows.Forms.Button()
        Me.LstVectors = New System.Windows.Forms.ListBox()
        Me.BtnDeleteProximity = New System.Windows.Forms.Button()
        Me.BtnAddProximity = New System.Windows.Forms.Button()
        Me.GrdProximity = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnEditProximity = New System.Windows.Forms.Button()
        Me.CkProximity = New System.Windows.Forms.CheckBox()
        Me.BtnFindSite = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMap = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtSiteName = New System.Windows.Forms.TextBox()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CkLocation = New System.Windows.Forms.CheckBox()
        Me.GrpLocation = New System.Windows.Forms.GroupBox()
        Me.PnlLocation = New System.Windows.Forms.Panel()
        Me.BtnToggle = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BtnSaveLocation = New System.Windows.Forms.Button()
        Me.BtnCancelLocation = New System.Windows.Forms.Button()
        Me.LstValues = New System.Windows.Forms.ListBox()
        Me.LstRasters = New System.Windows.Forms.ListBox()
        Me.BtnDeleteLocation = New System.Windows.Forms.Button()
        Me.BtnAddLocation = New System.Windows.Forms.Button()
        Me.GrdLocation = New System.Windows.Forms.DataGridView()
        Me.Layer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Values = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FullPath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnEditLocation = New System.Windows.Forms.Button()
        Me.BtnRecalculate = New System.Windows.Forms.Button()
        Me.BtnCopy = New System.Windows.Forms.Button()
        Me.CkConstraints = New System.Windows.Forms.CheckBox()
        Me.GrpElevation.SuspendLayout()
        Me.GrpPrecipitation.SuspendLayout()
        Me.GrpProximity.SuspendLayout()
        Me.PnlProximity.SuspendLayout()
        CType(Me.GrdProximity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpLocation.SuspendLayout()
        Me.PnlLocation.SuspendLayout()
        CType(Me.GrdLocation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblBufferDistance
        '
        Me.LblBufferDistance.AutoSize = True
        Me.LblBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblBufferDistance.Location = New System.Drawing.Point(256, 18)
        Me.LblBufferDistance.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblBufferDistance.Name = "LblBufferDistance"
        Me.LblBufferDistance.Size = New System.Drawing.Size(49, 16)
        Me.LblBufferDistance.TabIndex = 72
        Me.LblBufferDistance.Text = "Meters"
        '
        'CkElev
        '
        Me.CkElev.AutoSize = True
        Me.CkElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkElev.Location = New System.Drawing.Point(19, 149)
        Me.CkElev.Name = "CkElev"
        Me.CkElev.Size = New System.Drawing.Size(15, 14)
        Me.CkElev.TabIndex = 74
        Me.CkElev.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(0, 109)
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
        Me.CkPrecip.Location = New System.Drawing.Point(19, 256)
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
        Me.GrpElevation.Enabled = False
        Me.GrpElevation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpElevation.Location = New System.Drawing.Point(57, 124)
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
        Me.lblElevation.Location = New System.Drawing.Point(5, 28)
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
        Me.Label23.Location = New System.Drawing.Point(177, 28)
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
        Me.Label24.Location = New System.Drawing.Point(264, 28)
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
        Me.txtMinElev.Location = New System.Drawing.Point(213, 28)
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
        Me.Label1.Location = New System.Drawing.Point(369, 28)
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
        Me.Label4.Location = New System.Drawing.Point(169, 53)
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
        Me.TxtMaxElev.Location = New System.Drawing.Point(307, 28)
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
        Me.txtLower.Location = New System.Drawing.Point(218, 52)
        Me.txtLower.Margin = New System.Windows.Forms.Padding(2)
        Me.txtLower.Name = "txtLower"
        Me.txtLower.Size = New System.Drawing.Size(75, 20)
        Me.txtLower.TabIndex = 79
        Me.txtLower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtRange
        '
        Me.TxtRange.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtRange.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRange.ForeColor = System.Drawing.Color.Blue
        Me.TxtRange.Location = New System.Drawing.Point(422, 28)
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
        Me.Label3.Location = New System.Drawing.Point(303, 53)
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
        Me.LblElevRange.Location = New System.Drawing.Point(5, 53)
        Me.LblElevRange.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblElevRange.Name = "LblElevRange"
        Me.LblElevRange.Size = New System.Drawing.Size(152, 16)
        Me.LblElevRange.TabIndex = 76
        Me.LblElevRange.Text = "Desired Range (Meters)"
        '
        'TxtUpperRange
        '
        Me.TxtUpperRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtUpperRange.Location = New System.Drawing.Point(356, 52)
        Me.TxtUpperRange.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtUpperRange.Name = "TxtUpperRange"
        Me.TxtUpperRange.Size = New System.Drawing.Size(75, 20)
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
        Me.GrpPrecipitation.Location = New System.Drawing.Point(57, 210)
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
        Me.TxtPrecipLower.Size = New System.Drawing.Size(75, 20)
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
        Me.Label14.Location = New System.Drawing.Point(310, 118)
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
        Me.TxtPrecipUpper.Location = New System.Drawing.Point(356, 116)
        Me.TxtPrecipUpper.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtPrecipUpper.Name = "TxtPrecipUpper"
        Me.TxtPrecipUpper.Size = New System.Drawing.Size(75, 20)
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
        Me.CmboxEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.CmboxBegin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmboxBegin.FormattingEnabled = True
        Me.CmboxBegin.Location = New System.Drawing.Point(96, 54)
        Me.CmboxBegin.Name = "CmboxBegin"
        Me.CmboxBegin.Size = New System.Drawing.Size(67, 24)
        Me.CmboxBegin.TabIndex = 13
        '
        'CmboxPrecipType
        '
        Me.CmboxPrecipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxPrecipType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.GrpProximity.Controls.Add(Me.PnlProximity)
        Me.GrpProximity.Controls.Add(Me.BtnDeleteProximity)
        Me.GrpProximity.Controls.Add(Me.BtnAddProximity)
        Me.GrpProximity.Controls.Add(Me.GrdProximity)
        Me.GrpProximity.Controls.Add(Me.BtnEditProximity)
        Me.GrpProximity.Controls.Add(Me.LblBufferDistance)
        Me.GrpProximity.Enabled = False
        Me.GrpProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpProximity.Location = New System.Drawing.Point(57, 371)
        Me.GrpProximity.Name = "GrpProximity"
        Me.GrpProximity.Size = New System.Drawing.Size(487, 149)
        Me.GrpProximity.TabIndex = 81
        Me.GrpProximity.TabStop = False
        Me.GrpProximity.Text = "Proximity - Search limited to intersection of conditions listed below"
        '
        'PnlProximity
        '
        Me.PnlProximity.Controls.Add(Me.LblAddBufferDistance)
        Me.PnlProximity.Controls.Add(Me.txtBufferDistance)
        Me.PnlProximity.Controls.Add(Me.Label5)
        Me.PnlProximity.Controls.Add(Me.BtnSaveProximity)
        Me.PnlProximity.Controls.Add(Me.BtnCancelProximity)
        Me.PnlProximity.Controls.Add(Me.LstVectors)
        Me.PnlProximity.Location = New System.Drawing.Point(453, 18)
        Me.PnlProximity.Name = "PnlProximity"
        Me.PnlProximity.Size = New System.Drawing.Size(475, 131)
        Me.PnlProximity.TabIndex = 95
        Me.PnlProximity.Visible = False
        '
        'LblAddBufferDistance
        '
        Me.LblAddBufferDistance.AutoSize = True
        Me.LblAddBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAddBufferDistance.Location = New System.Drawing.Point(234, 40)
        Me.LblAddBufferDistance.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblAddBufferDistance.Name = "LblAddBufferDistance"
        Me.LblAddBufferDistance.Size = New System.Drawing.Size(153, 16)
        Me.LblAddBufferDistance.TabIndex = 94
        Me.LblAddBufferDistance.Text = "Buffer Distance (Meters):"
        '
        'txtBufferDistance
        '
        Me.txtBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBufferDistance.Location = New System.Drawing.Point(388, 39)
        Me.txtBufferDistance.Margin = New System.Windows.Forms.Padding(2)
        Me.txtBufferDistance.Name = "txtBufferDistance"
        Me.txtBufferDistance.Size = New System.Drawing.Size(75, 20)
        Me.txtBufferDistance.TabIndex = 93
        Me.txtBufferDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 6)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(222, 16)
        Me.Label5.TabIndex = 91
        Me.Label5.Text = "Limit search to selected values"
        '
        'BtnSaveProximity
        '
        Me.BtnSaveProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSaveProximity.Location = New System.Drawing.Point(398, 98)
        Me.BtnSaveProximity.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnSaveProximity.Name = "BtnSaveProximity"
        Me.BtnSaveProximity.Size = New System.Drawing.Size(65, 25)
        Me.BtnSaveProximity.TabIndex = 89
        Me.BtnSaveProximity.Text = "Save"
        Me.BtnSaveProximity.UseVisualStyleBackColor = True
        '
        'BtnCancelProximity
        '
        Me.BtnCancelProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancelProximity.Location = New System.Drawing.Point(327, 98)
        Me.BtnCancelProximity.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnCancelProximity.Name = "BtnCancelProximity"
        Me.BtnCancelProximity.Size = New System.Drawing.Size(65, 25)
        Me.BtnCancelProximity.TabIndex = 74
        Me.BtnCancelProximity.Text = "Cancel"
        Me.BtnCancelProximity.UseVisualStyleBackColor = True
        '
        'LstVectors
        '
        Me.LstVectors.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstVectors.FormattingEnabled = True
        Me.LstVectors.ItemHeight = 16
        Me.LstVectors.Location = New System.Drawing.Point(6, 38)
        Me.LstVectors.Name = "LstVectors"
        Me.LstVectors.Size = New System.Drawing.Size(197, 52)
        Me.LstVectors.Sorted = True
        Me.LstVectors.TabIndex = 71
        '
        'BtnDeleteProximity
        '
        Me.BtnDeleteProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteProximity.Location = New System.Drawing.Point(383, 93)
        Me.BtnDeleteProximity.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnDeleteProximity.Name = "BtnDeleteProximity"
        Me.BtnDeleteProximity.Size = New System.Drawing.Size(65, 22)
        Me.BtnDeleteProximity.TabIndex = 93
        Me.BtnDeleteProximity.Text = "Delete"
        Me.BtnDeleteProximity.UseVisualStyleBackColor = True
        '
        'BtnAddProximity
        '
        Me.BtnAddProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddProximity.Location = New System.Drawing.Point(383, 37)
        Me.BtnAddProximity.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnAddProximity.Name = "BtnAddProximity"
        Me.BtnAddProximity.Size = New System.Drawing.Size(65, 22)
        Me.BtnAddProximity.TabIndex = 92
        Me.BtnAddProximity.Text = "Add"
        Me.BtnAddProximity.UseVisualStyleBackColor = True
        '
        'GrdProximity
        '
        Me.GrdProximity.AllowUserToAddRows = False
        Me.GrdProximity.AllowUserToDeleteRows = False
        Me.GrdProximity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdProximity.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3})
        Me.GrdProximity.Location = New System.Drawing.Point(10, 37)
        Me.GrdProximity.MultiSelect = False
        Me.GrdProximity.Name = "GrdProximity"
        Me.GrdProximity.ReadOnly = True
        Me.GrdProximity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdProximity.Size = New System.Drawing.Size(356, 83)
        Me.GrdProximity.TabIndex = 91
        Me.GrdProximity.TabStop = False
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn1.HeaderText = "Layer"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn2.HeaderText = "Buffer"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "FullPath"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Visible = False
        Me.DataGridViewTextBoxColumn3.Width = 5
        '
        'BtnEditProximity
        '
        Me.BtnEditProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditProximity.Location = New System.Drawing.Point(383, 65)
        Me.BtnEditProximity.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnEditProximity.Name = "BtnEditProximity"
        Me.BtnEditProximity.Size = New System.Drawing.Size(65, 22)
        Me.BtnEditProximity.TabIndex = 94
        Me.BtnEditProximity.Text = "Edit"
        Me.BtnEditProximity.UseVisualStyleBackColor = True
        '
        'CkProximity
        '
        Me.CkProximity.AutoSize = True
        Me.CkProximity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkProximity.Location = New System.Drawing.Point(19, 400)
        Me.CkProximity.Name = "CkProximity"
        Me.CkProximity.Size = New System.Drawing.Size(15, 14)
        Me.CkProximity.TabIndex = 82
        Me.CkProximity.UseVisualStyleBackColor = True
        '
        'BtnFindSite
        '
        Me.BtnFindSite.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFindSite.Location = New System.Drawing.Point(438, 716)
        Me.BtnFindSite.Name = "BtnFindSite"
        Me.BtnFindSite.Size = New System.Drawing.Size(93, 23)
        Me.BtnFindSite.TabIndex = 83
        Me.BtnFindSite.Text = "Find Site"
        Me.BtnFindSite.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(466, 749)
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
        Me.BtnMap.Location = New System.Drawing.Point(333, 716)
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
        Me.Label2.Location = New System.Drawing.Point(0, 85)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 16)
        Me.Label2.TabIndex = 85
        Me.Label2.Text = "Site name:"
        '
        'TxtSiteName
        '
        Me.TxtSiteName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSiteName.Location = New System.Drawing.Point(85, 83)
        Me.TxtSiteName.Margin = New System.Windows.Forms.Padding(2)
        Me.TxtSiteName.MaxLength = 49
        Me.TxtSiteName.Name = "TxtSiteName"
        Me.TxtSiteName.Size = New System.Drawing.Size(168, 22)
        Me.TxtSiteName.TabIndex = 86
        '
        'BtnClear
        '
        Me.BtnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClear.Location = New System.Drawing.Point(390, 749)
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
        Me.TextBox1.Size = New System.Drawing.Size(541, 70)
        Me.TextBox1.TabIndex = 88
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'CkLocation
        '
        Me.CkLocation.AutoSize = True
        Me.CkLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkLocation.Location = New System.Drawing.Point(19, 585)
        Me.CkLocation.Name = "CkLocation"
        Me.CkLocation.Size = New System.Drawing.Size(15, 14)
        Me.CkLocation.TabIndex = 89
        Me.CkLocation.UseVisualStyleBackColor = True
        '
        'GrpLocation
        '
        Me.GrpLocation.Controls.Add(Me.PnlLocation)
        Me.GrpLocation.Controls.Add(Me.BtnDeleteLocation)
        Me.GrpLocation.Controls.Add(Me.BtnAddLocation)
        Me.GrpLocation.Controls.Add(Me.GrdLocation)
        Me.GrpLocation.Controls.Add(Me.BtnEditLocation)
        Me.GrpLocation.Enabled = False
        Me.GrpLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpLocation.Location = New System.Drawing.Point(57, 521)
        Me.GrpLocation.Name = "GrpLocation"
        Me.GrpLocation.Size = New System.Drawing.Size(487, 157)
        Me.GrpLocation.TabIndex = 90
        Me.GrpLocation.TabStop = False
        Me.GrpLocation.Text = "Location - Search limited to intersection of conditions listed below"
        '
        'PnlLocation
        '
        Me.PnlLocation.Controls.Add(Me.BtnToggle)
        Me.PnlLocation.Controls.Add(Me.Label6)
        Me.PnlLocation.Controls.Add(Me.BtnSaveLocation)
        Me.PnlLocation.Controls.Add(Me.BtnCancelLocation)
        Me.PnlLocation.Controls.Add(Me.LstValues)
        Me.PnlLocation.Controls.Add(Me.LstRasters)
        Me.PnlLocation.Location = New System.Drawing.Point(461, 41)
        Me.PnlLocation.Name = "PnlLocation"
        Me.PnlLocation.Size = New System.Drawing.Size(475, 131)
        Me.PnlLocation.TabIndex = 73
        Me.PnlLocation.Visible = False
        '
        'BtnToggle
        '
        Me.BtnToggle.Enabled = False
        Me.BtnToggle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnToggle.Location = New System.Drawing.Point(259, 6)
        Me.BtnToggle.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnToggle.Name = "BtnToggle"
        Me.BtnToggle.Size = New System.Drawing.Size(135, 25)
        Me.BtnToggle.TabIndex = 92
        Me.BtnToggle.Text = "Toggle Selection"
        Me.BtnToggle.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(5, 6)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(222, 16)
        Me.Label6.TabIndex = 91
        Me.Label6.Text = "Limit search to selected values"
        '
        'BtnSaveLocation
        '
        Me.BtnSaveLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSaveLocation.Location = New System.Drawing.Point(329, 97)
        Me.BtnSaveLocation.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnSaveLocation.Name = "BtnSaveLocation"
        Me.BtnSaveLocation.Size = New System.Drawing.Size(65, 25)
        Me.BtnSaveLocation.TabIndex = 89
        Me.BtnSaveLocation.Text = "Save"
        Me.BtnSaveLocation.UseVisualStyleBackColor = True
        '
        'BtnCancelLocation
        '
        Me.BtnCancelLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancelLocation.Location = New System.Drawing.Point(258, 97)
        Me.BtnCancelLocation.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnCancelLocation.Name = "BtnCancelLocation"
        Me.BtnCancelLocation.Size = New System.Drawing.Size(65, 25)
        Me.BtnCancelLocation.TabIndex = 74
        Me.BtnCancelLocation.Text = "Cancel"
        Me.BtnCancelLocation.UseVisualStyleBackColor = True
        '
        'LstValues
        '
        Me.LstValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstValues.FormattingEnabled = True
        Me.LstValues.ItemHeight = 16
        Me.LstValues.Location = New System.Drawing.Point(212, 38)
        Me.LstValues.Name = "LstValues"
        Me.LstValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstValues.Size = New System.Drawing.Size(182, 52)
        Me.LstValues.TabIndex = 72
        '
        'LstRasters
        '
        Me.LstRasters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstRasters.FormattingEnabled = True
        Me.LstRasters.ItemHeight = 16
        Me.LstRasters.Location = New System.Drawing.Point(6, 38)
        Me.LstRasters.Name = "LstRasters"
        Me.LstRasters.Size = New System.Drawing.Size(197, 52)
        Me.LstRasters.Sorted = True
        Me.LstRasters.TabIndex = 71
        '
        'BtnDeleteLocation
        '
        Me.BtnDeleteLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteLocation.Location = New System.Drawing.Point(382, 87)
        Me.BtnDeleteLocation.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnDeleteLocation.Name = "BtnDeleteLocation"
        Me.BtnDeleteLocation.Size = New System.Drawing.Size(65, 22)
        Me.BtnDeleteLocation.TabIndex = 89
        Me.BtnDeleteLocation.Text = "Delete"
        Me.BtnDeleteLocation.UseVisualStyleBackColor = True
        '
        'BtnAddLocation
        '
        Me.BtnAddLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddLocation.Location = New System.Drawing.Point(382, 31)
        Me.BtnAddLocation.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnAddLocation.Name = "BtnAddLocation"
        Me.BtnAddLocation.Size = New System.Drawing.Size(65, 22)
        Me.BtnAddLocation.TabIndex = 88
        Me.BtnAddLocation.Text = "Add"
        Me.BtnAddLocation.UseVisualStyleBackColor = True
        '
        'GrdLocation
        '
        Me.GrdLocation.AllowUserToAddRows = False
        Me.GrdLocation.AllowUserToDeleteRows = False
        Me.GrdLocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdLocation.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Layer, Me.Values, Me.FullPath})
        Me.GrdLocation.Location = New System.Drawing.Point(9, 31)
        Me.GrdLocation.MultiSelect = False
        Me.GrdLocation.Name = "GrdLocation"
        Me.GrdLocation.ReadOnly = True
        Me.GrdLocation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdLocation.Size = New System.Drawing.Size(356, 83)
        Me.GrdLocation.TabIndex = 70
        Me.GrdLocation.TabStop = False
        '
        'Layer
        '
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Layer.DefaultCellStyle = DataGridViewCellStyle7
        Me.Layer.HeaderText = "Layer"
        Me.Layer.Name = "Layer"
        Me.Layer.ReadOnly = True
        Me.Layer.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Layer.Width = 200
        '
        'Values
        '
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Values.DefaultCellStyle = DataGridViewCellStyle8
        Me.Values.HeaderText = "Values"
        Me.Values.Name = "Values"
        Me.Values.ReadOnly = True
        '
        'FullPath
        '
        Me.FullPath.HeaderText = "FullPath"
        Me.FullPath.Name = "FullPath"
        Me.FullPath.ReadOnly = True
        Me.FullPath.Visible = False
        Me.FullPath.Width = 5
        '
        'BtnEditLocation
        '
        Me.BtnEditLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditLocation.Location = New System.Drawing.Point(382, 59)
        Me.BtnEditLocation.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnEditLocation.Name = "BtnEditLocation"
        Me.BtnEditLocation.Size = New System.Drawing.Size(65, 22)
        Me.BtnEditLocation.TabIndex = 90
        Me.BtnEditLocation.Text = "Edit"
        Me.BtnEditLocation.UseVisualStyleBackColor = True
        '
        'BtnRecalculate
        '
        Me.BtnRecalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecalculate.Location = New System.Drawing.Point(3, 716)
        Me.BtnRecalculate.Name = "BtnRecalculate"
        Me.BtnRecalculate.Size = New System.Drawing.Size(322, 23)
        Me.BtnRecalculate.TabIndex = 91
        Me.BtnRecalculate.Text = "Add new site to Scenario 1 and recalculate"
        Me.BtnRecalculate.UseVisualStyleBackColor = True
        '
        'BtnCopy
        '
        Me.BtnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCopy.Location = New System.Drawing.Point(78, 683)
        Me.BtnCopy.Name = "BtnCopy"
        Me.BtnCopy.Size = New System.Drawing.Size(232, 23)
        Me.BtnCopy.TabIndex = 92
        Me.BtnCopy.Text = "Copy constraints for new site"
        Me.BtnCopy.UseVisualStyleBackColor = True
        '
        'CkConstraints
        '
        Me.CkConstraints.AutoSize = True
        Me.CkConstraints.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkConstraints.Location = New System.Drawing.Point(320, 685)
        Me.CkConstraints.Name = "CkConstraints"
        Me.CkConstraints.Size = New System.Drawing.Size(180, 20)
        Me.CkConstraints.TabIndex = 93
        Me.CkConstraints.Text = "Re-use layers for new site"
        Me.CkConstraints.UseVisualStyleBackColor = True
        '
        'FrmPsuedoSite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(567, 787)
        Me.Controls.Add(Me.CkConstraints)
        Me.Controls.Add(Me.BtnRecalculate)
        Me.Controls.Add(Me.GrpLocation)
        Me.Controls.Add(Me.BtnCopy)
        Me.Controls.Add(Me.CkLocation)
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
        Me.Text = " "
        Me.GrpElevation.ResumeLayout(False)
        Me.GrpElevation.PerformLayout()
        Me.GrpPrecipitation.ResumeLayout(False)
        Me.GrpPrecipitation.PerformLayout()
        Me.GrpProximity.ResumeLayout(False)
        Me.GrpProximity.PerformLayout()
        Me.PnlProximity.ResumeLayout(False)
        Me.PnlProximity.PerformLayout()
        CType(Me.GrdProximity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpLocation.ResumeLayout(False)
        Me.PnlLocation.ResumeLayout(False)
        Me.PnlLocation.PerformLayout()
        CType(Me.GrdLocation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblBufferDistance As System.Windows.Forms.Label
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
    Friend WithEvents CkLocation As System.Windows.Forms.CheckBox
    Friend WithEvents GrpLocation As System.Windows.Forms.GroupBox
    Friend WithEvents GrdLocation As System.Windows.Forms.DataGridView
    Friend WithEvents BtnEditLocation As System.Windows.Forms.Button
    Friend WithEvents PnlLocation As System.Windows.Forms.Panel
    Friend WithEvents LstValues As System.Windows.Forms.ListBox
    Friend WithEvents LstRasters As System.Windows.Forms.ListBox
    Friend WithEvents BtnDeleteLocation As System.Windows.Forms.Button
    Friend WithEvents BtnAddLocation As System.Windows.Forms.Button
    Friend WithEvents BtnSaveLocation As System.Windows.Forms.Button
    Friend WithEvents BtnCancelLocation As System.Windows.Forms.Button
    Friend WithEvents Layer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Values As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FullPath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnToggle As System.Windows.Forms.Button
    Friend WithEvents BtnDeleteProximity As System.Windows.Forms.Button
    Friend WithEvents BtnAddProximity As System.Windows.Forms.Button
    Friend WithEvents GrdProximity As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BtnEditProximity As System.Windows.Forms.Button
    Friend WithEvents PnlProximity As System.Windows.Forms.Panel
    Friend WithEvents txtBufferDistance As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSaveProximity As System.Windows.Forms.Button
    Friend WithEvents BtnCancelProximity As System.Windows.Forms.Button
    Friend WithEvents LstVectors As System.Windows.Forms.ListBox
    Friend WithEvents LblAddBufferDistance As System.Windows.Forms.Label
    Friend WithEvents BtnRecalculate As System.Windows.Forms.Button
    Friend WithEvents BtnCopy As System.Windows.Forms.Button
    Friend WithEvents CkConstraints As System.Windows.Forms.CheckBox
End Class
