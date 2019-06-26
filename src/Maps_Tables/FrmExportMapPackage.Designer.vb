<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmExportMapPackage
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtCurrentMap = New System.Windows.Forms.TextBox()
        Me.TxtExportSingleDescr = New System.Windows.Forms.TextBox()
        Me.BtnExportCurrent = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TxtPublisher = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtComments = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 5)
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
        Me.txtExportFolder.Location = New System.Drawing.Point(139, 2)
        Me.txtExportFolder.Name = "txtExportFolder"
        Me.txtExportFolder.ReadOnly = True
        Me.txtExportFolder.Size = New System.Drawing.Size(416, 27)
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 55)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 20)
        Me.Label1.TabIndex = 77
        Me.Label1.Text = "Selected Map"
        '
        'TxtCurrentMap
        '
        Me.TxtCurrentMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCurrentMap.ForeColor = System.Drawing.Color.Black
        Me.TxtCurrentMap.Location = New System.Drawing.Point(139, 52)
        Me.TxtCurrentMap.Name = "TxtCurrentMap"
        Me.TxtCurrentMap.ReadOnly = True
        Me.TxtCurrentMap.Size = New System.Drawing.Size(416, 27)
        Me.TxtCurrentMap.TabIndex = 78
        Me.TxtCurrentMap.Text = "ELEVATION DISTRIBUTION"
        '
        'TxtExportSingleDescr
        '
        Me.TxtExportSingleDescr.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtExportSingleDescr.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtExportSingleDescr.ForeColor = System.Drawing.Color.Black
        Me.TxtExportSingleDescr.Location = New System.Drawing.Point(16, 87)
        Me.TxtExportSingleDescr.Name = "TxtExportSingleDescr"
        Me.TxtExportSingleDescr.ReadOnly = True
        Me.TxtExportSingleDescr.Size = New System.Drawing.Size(683, 20)
        Me.TxtExportSingleDescr.TabIndex = 79
        Me.TxtExportSingleDescr.Text = "Export ONLY the currently selected map to the export folder"
        '
        'BtnExportCurrent
        '
        Me.BtnExportCurrent.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExportCurrent.Location = New System.Drawing.Point(572, 49)
        Me.BtnExportCurrent.Name = "BtnExportCurrent"
        Me.BtnExportCurrent.Size = New System.Drawing.Size(127, 33)
        Me.BtnExportCurrent.TabIndex = 80
        Me.BtnExportCurrent.Text = "Export Map"
        Me.BtnExportCurrent.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(17, 340)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(671, 44)
        Me.TextBox1.TabIndex = 81
        Me.TextBox1.Text = "Export ALL maps and charts to the export folder; May include individual maps prev" &
    "iously exported."
        '
        'TxtPublisher
        '
        Me.TxtPublisher.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPublisher.ForeColor = System.Drawing.Color.Black
        Me.TxtPublisher.Location = New System.Drawing.Point(139, 135)
        Me.TxtPublisher.Name = "TxtPublisher"
        Me.TxtPublisher.Size = New System.Drawing.Size(416, 27)
        Me.TxtPublisher.TabIndex = 83
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 138)
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
        Me.TxtComments.Location = New System.Drawing.Point(200, 173)
        Me.TxtComments.Multiline = True
        Me.TxtComments.Name = "TxtComments"
        Me.TxtComments.Size = New System.Drawing.Size(488, 71)
        Me.TxtComments.TabIndex = 85
        Me.TxtComments.Text = "Now is the time for all good men to come to the aid of their country. Now is the " &
    "time for all good men to come to the aid of their country"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 173)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(183, 20)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "Publisher Comments"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(351, 388)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(100, 33)
        Me.BtnCancel.TabIndex = 86
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'FrmExportMapPackage
        '
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.TxtComments)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtPublisher)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.BtnExportCurrent)
        Me.Controls.Add(Me.TxtExportSingleDescr)
        Me.Controls.Add(Me.TxtCurrentMap)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CmdPublish)
        Me.Controls.Add(Me.txtExportFolder)
        Me.Controls.Add(Me.Label2)
        Me.Name = "FrmExportMapPackage"
        Me.Size = New System.Drawing.Size(715, 450)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtExportFolder As Windows.Forms.TextBox
    Friend WithEvents CmdPublish As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TxtCurrentMap As Windows.Forms.TextBox
    Friend WithEvents TxtExportSingleDescr As Windows.Forms.TextBox
    Friend WithEvents BtnExportCurrent As Windows.Forms.Button
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents TxtPublisher As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents TxtComments As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents BtnCancel As Windows.Forms.Button
End Class
