<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPartitionRaster
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
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LstRasters = New System.Windows.Forms.ListBox()
        Me.CmdClear = New System.Windows.Forms.Button()
        Me.CmdClose = New System.Windows.Forms.Button()
        Me.LstFields = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LstValues = New System.Windows.Forms.ListBox()
        Me.CmdClearValues = New System.Windows.Forms.Button()
        Me.CmdSelectAll = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CmdCreateRaster = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(266, 16)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Precipitation Representation Partition"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.75!)
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(3, 35)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(94, 14)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Raster Layers"
        '
        'LstRasters
        '
        Me.LstRasters.FormattingEnabled = True
        Me.LstRasters.Location = New System.Drawing.Point(6, 55)
        Me.LstRasters.Name = "LstRasters"
        Me.LstRasters.Size = New System.Drawing.Size(197, 147)
        Me.LstRasters.TabIndex = 10
        '
        'CmdClear
        '
        Me.CmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClear.Location = New System.Drawing.Point(45, 211)
        Me.CmdClear.Name = "CmdClear"
        Me.CmdClear.Size = New System.Drawing.Size(106, 28)
        Me.CmdClear.TabIndex = 11
        Me.CmdClear.Text = "Clear selection"
        Me.CmdClear.UseVisualStyleBackColor = True
        '
        'CmdClose
        '
        Me.CmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClose.Location = New System.Drawing.Point(639, 222)
        Me.CmdClose.Name = "CmdClose"
        Me.CmdClose.Size = New System.Drawing.Size(106, 28)
        Me.CmdClose.TabIndex = 12
        Me.CmdClose.Text = "Close"
        Me.CmdClose.UseVisualStyleBackColor = True
        '
        'LstFields
        '
        Me.LstFields.FormattingEnabled = True
        Me.LstFields.Location = New System.Drawing.Point(253, 55)
        Me.LstFields.Name = "LstFields"
        Me.LstFields.Size = New System.Drawing.Size(98, 147)
        Me.LstFields.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.75!)
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(254, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 14)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Raster Fields"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.75!)
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(373, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 14)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Raster Values"
        '
        'LstValues
        '
        Me.LstValues.FormattingEnabled = True
        Me.LstValues.Location = New System.Drawing.Point(372, 55)
        Me.LstValues.Name = "LstValues"
        Me.LstValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstValues.Size = New System.Drawing.Size(98, 147)
        Me.LstValues.TabIndex = 15
        '
        'CmdClearValues
        '
        Me.CmdClearValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClearValues.Location = New System.Drawing.Point(476, 105)
        Me.CmdClearValues.Name = "CmdClearValues"
        Me.CmdClearValues.Size = New System.Drawing.Size(106, 28)
        Me.CmdClearValues.TabIndex = 17
        Me.CmdClearValues.Text = "Clear selection"
        Me.CmdClearValues.UseVisualStyleBackColor = True
        '
        'CmdSelectAll
        '
        Me.CmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelectAll.Location = New System.Drawing.Point(476, 70)
        Me.CmdSelectAll.Name = "CmdSelectAll"
        Me.CmdSelectAll.Size = New System.Drawing.Size(106, 28)
        Me.CmdSelectAll.TabIndex = 18
        Me.CmdSelectAll.Text = "Select All"
        Me.CmdSelectAll.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.75!)
        Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label3.Location = New System.Drawing.Point(373, 209)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(238, 51)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Click to select; Ctrl-click to de-select" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Selected values will be included in ana" & _
    "lysis"
        '
        'CmdCreateRaster
        '
        Me.CmdCreateRaster.Enabled = False
        Me.CmdCreateRaster.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCreateRaster.Location = New System.Drawing.Point(639, 188)
        Me.CmdCreateRaster.Name = "CmdCreateRaster"
        Me.CmdCreateRaster.Size = New System.Drawing.Size(106, 28)
        Me.CmdCreateRaster.TabIndex = 20
        Me.CmdCreateRaster.Text = "Create Raster"
        Me.CmdCreateRaster.UseVisualStyleBackColor = True
        '
        'FrmPartitionRaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(759, 262)
        Me.Controls.Add(Me.CmdCreateRaster)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CmdSelectAll)
        Me.Controls.Add(Me.CmdClearValues)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LstValues)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LstFields)
        Me.Controls.Add(Me.CmdClose)
        Me.Controls.Add(Me.CmdClear)
        Me.Controls.Add(Me.LstRasters)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Name = "FrmPartitionRaster"
        Me.ShowIcon = False
        Me.Text = "AOI: "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LstRasters As System.Windows.Forms.ListBox
    Friend WithEvents CmdClear As System.Windows.Forms.Button
    Friend WithEvents CmdClose As System.Windows.Forms.Button
    Friend WithEvents LstFields As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LstValues As System.Windows.Forms.ListBox
    Friend WithEvents CmdClearValues As System.Windows.Forms.Button
    Friend WithEvents CmdSelectAll As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CmdCreateRaster As System.Windows.Forms.Button
End Class
