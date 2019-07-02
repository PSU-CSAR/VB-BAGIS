Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Output

Public Class BtnPublishMap
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmPublishMapPackage.AddinImpl)(My.ThisAddIn.IDs.FrmPublishMapPackage)
        Dim frmMapPackage As FrmPublishMapPackage = dockWindowAddIn.UI

        Dim sOutputDir As String = AOIFolderBase + BA_DefaultMapPackageFolder
        If Not IO.Directory.Exists(sOutputDir) Then
            IO.Directory.CreateDirectory(sOutputDir)
        End If
        Dim ResampleRatio As Integer = 1
        Dim OutputResolution As Integer = 300
        ExportActiveViewParameterized(sOutputDir, frmMapPackage.CurrentMap, OutputResolution, ResampleRatio, False)
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Public Sub ExportActiveViewParameterized(ByVal sOutputDir As String, ByVal sFileName As String, ByVal iOutputResolution As Long,
                                             ByVal lResampleRatio As Long, ByVal bClipToGraphicsExtent As Boolean)

        'Export the active view using the specified parameters
        Dim docActiveView As IActiveView
        Dim docExport As IExport = New ExportPDF
        Dim docPrintAndExport As IPrintAndExport = New PrintAndExport
        Dim RasterSettings As IOutputRasterSettings

        Try

            docActiveView = My.ArcMap.Document.ActiveView

            ' Output Image Quality of the export.  The value here will only be used if the export
            '  object is a format that allows setting of Output Image Quality, i.e. a vector exporter.
            '  The value assigned to ResampleRatio should be in the range 1 to 5.
            '  1 corresponds to "Best", 5 corresponds to "Fast"

            If TypeOf docExport Is IOutputRasterSettings Then
                ' for vector formats, assign a ResampleRatio to control drawing of raster layers at export time
                RasterSettings = docExport
                RasterSettings.ResampleRatio = lResampleRatio

                ' NOTE: for raster formats output quality of the DISPLAY is set to 1 for image export 
                ' formats by default which is what should be used
            End If

            'assign the output path and filename.  We can use the Filter property of the export object to
            ' automatically assign the proper extension to the file.

            docExport.ExportFileName = sOutputDir + "\" + sFileName

            docPrintAndExport.Export(docActiveView, docExport, iOutputResolution, bClipToGraphicsExtent, Nothing)

            Windows.Forms.MessageBox.Show("Finished Exporting " + docExport.ExportFileName, "BAGIS")
            'cleanup for the exporter
            docExport.Cleanup()

        Catch ex As Exception
            Debug.Print("ExportActiveViewParameterized Exception: " + ex.Message)
        End Try
    End Sub

End Class
