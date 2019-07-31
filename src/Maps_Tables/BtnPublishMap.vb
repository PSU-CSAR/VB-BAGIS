﻿Imports BAGIS_ClassLibrary
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
            Else
                Exit Sub
            End If
        End If
        success = BA_ExportActiveViewAsPdf(sOutputDir, frmMapPackage.CurrentMap, BA_MapPdfOutputResolution,
                                                                 BA_MapPdfResampleRatio, False)
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