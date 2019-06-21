<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExportMapPackage
    Inherits System.Windows.Forms.UserControl

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtExportFolder = New System.Windows.Forms.TextBox()
        Me.CmdFolder = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.CmdExport = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 14)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(122, 20)
        Me.Label2.TabIndex = 73
        Me.Label2.Text = "Export Folder"
        '
        'txtExportFolder
        '
        Me.txtExportFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportFolder.ForeColor = System.Drawing.Color.Black
        Me.txtExportFolder.Location = New System.Drawing.Point(139, 11)
        Me.txtExportFolder.Name = "txtExportFolder"
        Me.txtExportFolder.ReadOnly = True
        Me.txtExportFolder.Size = New System.Drawing.Size(416, 27)
        Me.txtExportFolder.TabIndex = 74
        '
        'CmdFolder
        '
        Me.CmdFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdFolder.Location = New System.Drawing.Point(562, 8)
        Me.CmdFolder.Name = "CmdFolder"
        Me.CmdFolder.Size = New System.Drawing.Size(127, 33)
        Me.CmdFolder.TabIndex = 75
        Me.CmdFolder.Text = "Select"
        Me.CmdFolder.UseVisualStyleBackColor = True
        '
        'CmdExport
        '
        Me.CmdExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdExport.Location = New System.Drawing.Point(263, 145)
        Me.CmdExport.Name = "CmdExport"
        Me.CmdExport.Size = New System.Drawing.Size(231, 33)
        Me.CmdExport.TabIndex = 76
        Me.CmdExport.Text = "Export Map Package"
        Me.CmdExport.UseVisualStyleBackColor = True
        '
        'FrmExportMapPackage
        '
        Me.Controls.Add(Me.CmdExport)
        Me.Controls.Add(Me.CmdFolder)
        Me.Controls.Add(Me.txtExportFolder)
        Me.Controls.Add(Me.Label2)
        Me.Name = "FrmExportMapPackage"
        Me.Size = New System.Drawing.Size(737, 300)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtExportFolder As Windows.Forms.TextBox
    Friend WithEvents CmdFolder As Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As Windows.Forms.FolderBrowserDialog
    Friend WithEvents CmdExport As Windows.Forms.Button
End Class
