Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports Microsoft.Office.Interop.Excel

Public Class BtnRepresentPrecip
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        'Declare progress indicator variables
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            Dim mapsFilePath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) + "\" + BA_MapParameterFile

            If Not BA_File_ExistsWindowsIO(mapsFilePath) Then
                MessageBox.Show("Unable to read map parameters. Please use the Generate Maps tool to set the map parameters.")
                Exit Sub
            End If

            progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating Represented Precipitation Chart", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()

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

            pStepProg.Message = "Resampling DEM to PRISM resolution..."
            pStepProg.Step()
            Dim resampleDemPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis) + "\" + BA_RasterPrecMeanElev
            Dim success As BA_ReturnCode = BA_CreateElevPrecipLayer(AOIFolderBase, PRISMFolderName, PRISMRasterName, resampleDemPath)
            Dim objExcel As New Microsoft.Office.Interop.Excel.Application
            Dim bkWorkBook As Workbook = objExcel.Workbooks.Add 'a file in excel
            If success = BA_ReturnCode.Success Then
                Dim sampleTableFile As String = "sampleTbl"
                Dim sampleTablePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis) + "\" + sampleTableFile
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.Append(PRISMFolderName + "\" + PRISMRasterName & ";")
                sb.Append(resampleDemPath)
                pStepProg.Message = "Extracting DEM and PRISM values..."
                pStepProg.Step()
                success = BA_Sample(sb.ToString, PRISMFolderName + "\" + PRISMRasterName, sampleTablePath, _
                          PRISMFolderName + "\" + PRISMRasterName, BA_Resample_Nearest, 935)
                If success = BA_ReturnCode.Success Then
                    '@ToDo: Measurement units will come from frmGenerateMaps
                    Dim demUnits As MeasurementUnit = MeasurementUnit.Meters
                    Dim precipUnits As MeasurementUnit = MeasurementUnit.Inches

                    'Create Elevation Distribution Worksheet
                    Dim pPrecipDemElevWorksheet As Worksheet = bkWorkBook.Sheets.Add
                    pPrecipDemElevWorksheet.Name = "Precip-DEMElev"

                    'Create Elevation Distribution Worksheet
                    Dim pChartsWorksheet As Worksheet = bkWorkBook.Sheets.Add
                    pPrecipDemElevWorksheet.Name = "Charts"

                    'Create Elevation Distribution Worksheet
                    Dim pPrecipSiteWorksheet As Worksheet = bkWorkBook.Sheets.Add
                    pPrecipDemElevWorksheet.Name = "Precip-SiteElev"



                    success = BA_CreateRepresentPrecipTable(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis), sampleTableFile, _
                                PRISMRasterName + "_1", BA_RasterPrecMeanElev, BA_Aspect, "Partition", pPrecipDemElevWorksheet, demUnits, _
                                precipUnits, "Partition")
                    If success = BA_ReturnCode.Success Then
                        '@ToDo: Min axis values will come from frmGenerateMaps when this is integrated
                        BA_CreateRepresentPrecipChart(bkWorkBook, pPrecipDemElevWorksheet, pPrecipSiteWorksheet, _
                                                      pChartsWorksheet, demUnits, _
                                                      precipUnits, 1900, 4)
                    End If
                End If
            End If
            objExcel.Visible = True
        Catch ex As Exception
            Debug.Print("BtnRepresentPrecip.OnClick Exception: " + ex.Message)
            MessageBox.Show("Unable to calculate represented precipitation areas")
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
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
