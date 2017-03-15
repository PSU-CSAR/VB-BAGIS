<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmElevPrecip
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
        Me.LblHeading = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LstRasters = New System.Windows.Forms.ListBox()
        Me.CmdClear = New System.Windows.Forms.Button()
        Me.CmdClose = New System.Windows.Forms.Button()
        Me.CmdCreateRaster = New System.Windows.Forms.Button()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LblHeading
        '
        Me.LblHeading.AutoSize = True
        Me.LblHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHeading.Location = New System.Drawing.Point(3, 9)
        Me.LblHeading.Name = "LblHeading"
        Me.LblHeading.Size = New System.Drawing.Size(269, 16)
        Me.LblHeading.TabIndex = 8
        Me.LblHeading.Text = "Elevation-Precipitation Attribute Layer"
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
        Me.CmdClose.Location = New System.Drawing.Point(287, 250)
        Me.CmdClose.Name = "CmdClose"
        Me.CmdClose.Size = New System.Drawing.Size(106, 28)
        Me.CmdClose.TabIndex = 12
        Me.CmdClose.Text = "Close"
        Me.CmdClose.UseVisualStyleBackColor = True
        '
        'CmdCreateRaster
        '
        Me.CmdCreateRaster.Enabled = False
        Me.CmdCreateRaster.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCreateRaster.Location = New System.Drawing.Point(288, 215)
        Me.CmdCreateRaster.Name = "CmdCreateRaster"
        Me.CmdCreateRaster.Size = New System.Drawing.Size(106, 28)
        Me.CmdCreateRaster.TabIndex = 20
        Me.CmdCreateRaster.Text = "Create Raster"
        Me.CmdCreateRaster.UseVisualStyleBackColor = True
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(299, 6)
        Me.BtnAbout.Margin = New System.Windows.Forms.Padding(2)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(97, 28)
        Me.BtnAbout.TabIndex = 37
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'FrmElevPrecip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(405, 281)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.CmdCreateRaster)
        Me.Controls.Add(Me.CmdClose)
        Me.Controls.Add(Me.CmdClear)
        Me.Controls.Add(Me.LstRasters)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.LblHeading)
        Me.Name = "FrmElevPrecip"
        Me.ShowIcon = False
        Me.Text = "AOI: "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblHeading As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LstRasters As System.Windows.Forms.ListBox
    Friend WithEvents CmdClear As System.Windows.Forms.Button
    Friend WithEvents CmdClose As System.Windows.Forms.Button
    Friend WithEvents CmdCreateRaster As System.Windows.Forms.Button
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
End Class
