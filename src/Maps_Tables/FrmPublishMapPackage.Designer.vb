<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPublishMapPackage
    Inherits System.Windows.Forms.UserControl

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtExportFolder = New System.Windows.Forms.TextBox()
        Me.CmdPublish = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TxtPublisher = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtComments = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CmdCancel = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.file_name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Published = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 5)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 20)
        Me.Label2.TabIndex = 73
        Me.Label2.Text = "Publish Folder"
        '
        'txtExportFolder
        '
        Me.txtExportFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExportFolder.ForeColor = System.Drawing.Color.Black
        Me.txtExportFolder.Location = New System.Drawing.Point(144, 2)
        Me.txtExportFolder.Name = "txtExportFolder"
        Me.txtExportFolder.ReadOnly = True
        Me.txtExportFolder.Size = New System.Drawing.Size(544, 27)
        Me.txtExportFolder.TabIndex = 74
        '
        'CmdPublish
        '
        Me.CmdPublish.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdPublish.Location = New System.Drawing.Point(457, 388)
        Me.CmdPublish.Name = "CmdPublish"
        Me.CmdPublish.Size = New System.Drawing.Size(231, 33)
        Me.CmdPublish.TabIndex = 76
        Me.CmdPublish.Text = "Publish Map Package"
        Me.CmdPublish.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(17, 356)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(671, 44)
        Me.TextBox1.TabIndex = 81
        Me.TextBox1.Text = "Publish ALL maps and charts to the export folder; May include individual maps pre" &
    "viously exported."
        '
        'TxtPublisher
        '
        Me.TxtPublisher.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPublisher.ForeColor = System.Drawing.Color.Black
        Me.TxtPublisher.Location = New System.Drawing.Point(144, 37)
        Me.TxtPublisher.Name = "TxtPublisher"
        Me.TxtPublisher.Size = New System.Drawing.Size(544, 27)
        Me.TxtPublisher.TabIndex = 83
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 40)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 20)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "Published By"
        '
        'TxtComments
        '
        Me.TxtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtComments.ForeColor = System.Drawing.Color.Black
        Me.TxtComments.Location = New System.Drawing.Point(17, 94)
        Me.TxtComments.Multiline = True
        Me.TxtComments.Name = "TxtComments"
        Me.TxtComments.Size = New System.Drawing.Size(671, 71)
        Me.TxtComments.TabIndex = 85
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 72)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(183, 20)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "Publisher Comments"
        '
        'CmdCancel
        '
        Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCancel.Location = New System.Drawing.Point(351, 388)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.Size = New System.Drawing.Size(100, 33)
        Me.CmdCancel.TabIndex = 86
        Me.CmdCancel.Text = "Cancel"
        Me.CmdCancel.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.file_name, Me.Published})
        Me.DataGridView1.Location = New System.Drawing.Point(17, 183)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(671, 150)
        Me.DataGridView1.TabIndex = 87
        '
        'file_name
        '
        Me.file_name.HeaderText = "File Name"
        Me.file_name.Name = "file_name"
        Me.file_name.ReadOnly = True
        Me.file_name.Width = 375
        '
        'Published
        '
        Me.Published.HeaderText = "Published"
        Me.Published.Name = "Published"
        Me.Published.ReadOnly = True
        Me.Published.Width = 225
        '
        'FrmPublishMapPackage
        '
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.CmdPublish)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.TxtComments)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtPublisher)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.txtExportFolder)
        Me.Controls.Add(Me.Label2)
        Me.Name = "FrmPublishMapPackage"
        Me.Size = New System.Drawing.Size(715, 450)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtExportFolder As Windows.Forms.TextBox
    Friend WithEvents CmdPublish As Windows.Forms.Button
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents TxtPublisher As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents TxtComments As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents CmdCancel As Windows.Forms.Button
    Friend WithEvents DataGridView1 As Windows.Forms.DataGridView
    Friend WithEvents file_name As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Published As Windows.Forms.DataGridViewTextBoxColumn
End Class
