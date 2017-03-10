Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class FrmHelp

    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal topic As BA_HelpTopics)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        SetForm(topic)
    End Sub

    Private Sub SetForm(ByVal topic As BA_HelpTopics)
        Dim description As String = ""
        Dim illustration As System.Drawing.Image = Nothing

        Try
            ' Note: These topics are specific to an add-in; The resources (illustrations) are stored in an individual project)
            Select Case topic
                'Case BA_HelpTopics.AspectTemplate
                '    description = "Aspect Template Rule converts aspect azimuth angle values into 4, 8, or 16 directions. A customizable majority filter can be applied to the direction map to remove isolated aspects within a kernel window. The more iterations the majority filter is applied the more generalized the final map is. See figure below."
                '    illustration = My.Resources.AspectTemplateIllustration
                Case BA_HelpTopics.ElevPrecipAttribLayer
                    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                    sb.Append("You can partition the elevation-precipitation data to examine factors that mediate the correlation. The dominant aspect values are attached to each elevation cell and monitoring station in the 'Elev-Precip AOI' and 'Elev-Precip Sites' spreadsheet tabs. Optionally, you can use a discrete (integer) raster layer to add attributes to the output spreadsheets for partitioning the data.")
                    sb.Append(vbCrLf + vbCrLf)
                    sb.Append("You can use any layer listed in the raster layers list as a partitioning attribute layer. To add a layer to this list, use the ‘Add a New Layer’ button in the AOI Utilities tool. If the spatial resolution of the aspect or Elevation-Precipitation attribute layers differs from the PRISM layer, the Zonal Statistics tool is used to resample the layer to the same resolution as the PRISM layer implementing the Majority rule.")
                    sb.Append(vbCrLf + vbCrLf)
                    sb.Append("The value in the raster value field of the attribute layer will be appended in the 'Elev-Precip AOI' and 'Elev-Precip Sites' spreadsheet tabs. Only the raster value field (i.e., 'VALUE') can be used due to limitations of the ArcMap Zonal Statistics tool. If the value is null for a particular elevation cell or monitoring station, the spreadsheet cell will contain ‘Unknown’.")
                    sb.Append(vbCrLf + vbCrLf)
                    sb.Append("The partition can be performed in Excel by using Excel’s filtering function.Search for 'Filtering Data in Excel' on the Internet to learn more about partitioning data in Excel.")
                    description = sb.ToString
                Case Else
                    description = "Help information not available."
            End Select
            LblDescription.Text = description
            If illustration IsNot Nothing Then
                PictureBox.Image = illustration
                PictureBox.SizeMode = PictureBoxSizeMode.Normal
            Else
                'No image means we have more room for text
                PictureBox.SendToBack()
                LblDescription.Height = LblDescription.Height + PictureBox.Height
                BtnOK.Top = BtnOK.Top + PictureBox.Height
            End If
        Catch ex As Exception
            MsgBox("An error has occurred" & Chr(13) & Chr(13) & ex.Message)
        End Try
    End Sub
End Class