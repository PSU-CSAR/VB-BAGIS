﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmWebservices
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
        Me.TxtWebService = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSet = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboFields = New System.Windows.Forms.ComboBox()
        Me.BtnFields = New System.Windows.Forms.Button()
        Me.BtnLayerFile = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TxtWebService
        '
        Me.TxtWebService.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtWebService.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtWebService.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtWebService.Location = New System.Drawing.Point(41, 12)
        Me.TxtWebService.Name = "TxtWebService"
        Me.TxtWebService.ReadOnly = True
        Me.TxtWebService.Size = New System.Drawing.Size(459, 22)
        Me.TxtWebService.TabIndex = 64
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(7, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 16)
        Me.Label5.TabIndex = 63
        Me.Label5.Text = "Url:"
        '
        'BtnSet
        '
        Me.BtnSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSet.Location = New System.Drawing.Point(508, 8)
        Me.BtnSet.Name = "BtnSet"
        Me.BtnSet.Size = New System.Drawing.Size(108, 30)
        Me.BtnSet.TabIndex = 65
        Me.BtnSet.Text = "Set"
        Me.BtnSet.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "Fields:"
        '
        'cboFields
        '
        Me.cboFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFields.FormattingEnabled = True
        Me.cboFields.Location = New System.Drawing.Point(61, 94)
        Me.cboFields.Name = "cboFields"
        Me.cboFields.Size = New System.Drawing.Size(135, 24)
        Me.cboFields.TabIndex = 68
        '
        'BtnFields
        '
        Me.BtnFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFields.Location = New System.Drawing.Point(207, 91)
        Me.BtnFields.Name = "BtnFields"
        Me.BtnFields.Size = New System.Drawing.Size(108, 30)
        Me.BtnFields.TabIndex = 69
        Me.BtnFields.Text = "Get Fields"
        Me.BtnFields.UseVisualStyleBackColor = True
        '
        'BtnLayerFile
        '
        Me.BtnLayerFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnLayerFile.Location = New System.Drawing.Point(508, 46)
        Me.BtnLayerFile.Name = "BtnLayerFile"
        Me.BtnLayerFile.Size = New System.Drawing.Size(108, 30)
        Me.BtnLayerFile.TabIndex = 72
        Me.BtnLayerFile.Text = "Set"
        Me.BtnLayerFile.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TextBox1.Location = New System.Drawing.Point(82, 50)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(418, 22)
        Me.TextBox1.TabIndex = 71
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 16)
        Me.Label2.TabIndex = 70
        Me.Label2.Text = "Layer File:"
        '
        'FrmWebservices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 262)
        Me.Controls.Add(Me.BtnLayerFile)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnFields)
        Me.Controls.Add(Me.cboFields)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnSet)
        Me.Controls.Add(Me.TxtWebService)
        Me.Controls.Add(Me.Label5)
        Me.Name = "FrmWebservices"
        Me.ShowIcon = False
        Me.Text = "Webservices testing"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtWebService As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSet As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboFields As System.Windows.Forms.ComboBox
    Friend WithEvents BtnFields As System.Windows.Forms.Button
    Friend WithEvents BtnLayerFile As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
