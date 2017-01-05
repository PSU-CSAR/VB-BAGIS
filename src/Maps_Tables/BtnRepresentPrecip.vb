Imports BAGIS_ClassLibrary
Imports System.Windows.Forms

Public Class BtnRepresentPrecip
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim mapsFilePath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) + "\" + BA_MapParameterFile

            If Not BA_File_ExistsWindowsIO(mapsFilePath) Then
                MessageBox.Show("Unable to read map parameters. Please use the Generate Maps tool to set the map parameters.")
                Exit Sub
            End If

            Dim strNext As String = Nothing
            Dim intSelectedIndex As Int16 = -1
            Dim intBeginIndex As Int16 = -1
            Dim intEndIndex As Int16 = -1
            Using sr As New System.IO.StreamReader(mapsFilePath)
                For i As Int16 = 1 To 11 'The index we want is on lines 9 -11
                    strNext = sr.ReadLine
                    Select Case i
                        Case 9
                            intSelectedIndex = Integer.Parse(strNext)
                        Case 10
                            intBeginIndex = Integer.Parse(strNext)
                        Case 11
                            intEndIndex = Integer.Parse(strNext)
                    End Select
                Next
            End Using

            'Based on frmGenerateMaps.CmboxPrecipType.SelectedIndex
            Dim PRISMRasterName As String = Nothing
            Dim PRISMFolderName As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True)
            Dim response As Integer
            If intSelectedIndex = 0 Then  'read direct Annual PRISM raster
                PRISMRasterName = AOIPrismFolderNames.annual.ToString
            ElseIf intSelectedIndex > 0 And intSelectedIndex < 5 Then 'read directly Quarterly PRISM raster
                PRISMRasterName = BA_GetPrismFolderName(intSelectedIndex + 12)
            Else 'sum individual monthly PRISM rasters
                response = BA_PRISMCustom(My.Document, AOIFolderBase, intBeginIndex, intEndIndex)
                If response = 0 Then
                    MessageBox.Show("Unable to generate custom PRISM layer! Program stopped.")
                    Exit Sub
                End If
                PRISMRasterName = BA_TEMP_PRISM
                PRISMFolderName = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            End If

            Dim success As BA_ReturnCode = BA_CreateElevPrecipLayer(AOIFolderBase, PRISMFolderName, PRISMRasterName)
            MessageBox.Show("Finished!")
        Catch ex As Exception
            Debug.Print("BtnRepresentPrecip.OnClick Exception: " + ex.Message)
            MessageBox.Show("Unable to calculate represented precipitation areas")
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            'Check to make sure the precip zone layer exists before enabling represented precip tool
            'Precip data is selected in Maps dialog but is a required layer for represented precip  tool
            Dim folderPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            Dim fullPath As String = folderPath & BA_EnumDescription(MapsFileName.PrecipZone)
            If value = True Then
                If Not BA_File_Exists(fullPath, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                    Me.Enabled = False
                    Exit Property
                End If
            End If
            Me.Enabled = value
        End Set
    End Property
End Class
