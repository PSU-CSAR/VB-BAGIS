Imports BAGIS_ClassLibrary
Imports System.Windows.Forms

Public Class BtnPublishMap
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmPublishMapPackage.AddinImpl)(My.ThisAddIn.IDs.FrmPublishMapPackage)
        Dim frmMapPackage As FrmPublishMapPackage = dockWindowAddIn.UI

        ' Check for the directory and create it if it doesn't exist
        Dim sOutputDir As String = AOIFolderBase + BA_ExportMapPackageFolder
        If Not IO.Directory.Exists(sOutputDir) Then
            IO.Directory.CreateDirectory(sOutputDir)
        End If
        Dim fileNameBase As String = IO.Path.GetFileNameWithoutExtension(frmMapPackage.CurrentMap)
        Dim sMxdFullPath As String = sOutputDir + "\" + fileNameBase + ".mxd"

        'Check for existence of map package product; If it exists, ask if we should overwrite
        Dim success As BA_ReturnCode = BA_ReturnCode.OtherError
        If IO.File.Exists(sOutputDir + "\" + BA_ExportAllMapsChartsPdf) Then
            Dim strMessage As String = "A map package has already been created for this AOI at " +
                sOutputDir + "\" + BA_ExportAllMapsChartsPdf + "." + vbCrLf + "Do you wish to overwrite the existing " +
                "map package ?"
            Dim res As DialogResult = MessageBox.Show(strMessage, "BAGIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res = DialogResult.Yes Then
                success = BA_DeleteMapPackageElements()
                If success <> BA_ReturnCode.Success Then
                    MessageBox.Show("An error occurred while deleting the map package!!", "BAGIS")
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
        End If
        Dim bOverwriteMapDocument As Boolean = False
        'Check for existence of map we want to export; If it exists, ask if we should overwrite
        If IO.File.Exists(sOutputDir + "\" + frmMapPackage.CurrentMap) Then
            Dim strMessage As String = "A .pdf version of this map already exists at " +
                sOutputDir + "\" + frmMapPackage.CurrentMap + "." + vbCrLf + "Do you wish to overwrite the existing " +
                "map ?"
            Dim res As DialogResult = MessageBox.Show(strMessage, "BAGIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res = DialogResult.Yes Then
                Try
                    IO.File.Delete(sOutputDir + "\" + frmMapPackage.CurrentMap)
                Catch ex As IO.IOException
                    MessageBox.Show("Unable to delete " + sOutputDir + "\" + frmMapPackage.CurrentMap +
                                    "!" + vbCrLf + "The most likely cause is that you have the file open. " +
                                    "Please check and try again.", "BAGIS")
                    Exit Sub
                End Try
                ' Manage saving of .mxd document
                If IO.File.Exists(sMxdFullPath) Then
                    Try
                        Dim pMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = My.ArcMap.Application.Document
                        Dim pMapDoc As ESRI.ArcGIS.Carto.MapDocument = CType(pMxDoc, ESRI.ArcGIS.Carto.IMapDocument)
                        Dim strMapName = pMapDoc.DocumentFilename
                        'Get the path of the current .mxd, if it is the same as the one we will momentarily save,
                        'we will use save instead of save as down below. Otherwise we delete the old copy
                        If sMxdFullPath.Equals(strMapName) Then
                            bOverwriteMapDocument = True
                        Else
                            IO.File.Delete(sMxdFullPath)
                        End If
                    Catch ex As IO.IOException
                        ' Hopefully will never need this but leaving it just in case
                        MessageBox.Show("Unable to delete " + sMxdFullPath +
                                        "!" + vbCrLf + "The most likely cause is that you are using the .mxd in ArcMap. " +
                                        "Use the 'Save As' menu item to save the .mxd under a different name and try again.", "BAGIS")
                        Exit Sub
                    End Try
                End If
            Else
                Exit Sub
            End If
        End If
        success = BA_ExportActiveViewAsPdf(sOutputDir, frmMapPackage.CurrentMap, BA_MapPdfOutputResolution,
                                                                 BA_MapPdfResampleRatio, False)
        If success = BA_ReturnCode.Success Then
            If bOverwriteMapDocument = False Then
                My.ArcMap.Application.SaveAsDocument(sMxdFullPath)
            Else
                My.ArcMap.Application.SaveDocument(sMxdFullPath)
            End If
        End If
            If success = BA_ReturnCode.Success Then
            MessageBox.Show("Finished publishing " + sOutputDir + "\" + frmMapPackage.CurrentMap + "!", "BAGIS")
        Else
            MessageBox.Show("Unable to publish " + sOutputDir + "\" + frmMapPackage.CurrentMap + "!", "BAGIS")
        End If
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

End Class
