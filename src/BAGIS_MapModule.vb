Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMapUI
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.esriSystem
Imports Microsoft.Office.Interop.Excel
Imports ESRI.ArcGIS.Framework
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Output

Module BAGIS_MapModule
    'resize the mapframe on the pagelayout
    Public Function BA_SetMapFrameDimension(ByVal mapname As String, ByVal xmin As Double, ByVal ymin As Double, ByVal xmax As Double, ByVal ymax As Double, ByVal No_Border As Boolean) As Integer
        Dim return_value As Integer

        'Set the page layout.
        Dim pMxDoc As IMxDocument = My.ArcMap.Application.Document
        Dim pPageLayout As IPageLayout = pMxDoc.PageLayout
        Dim pGraphicsContainer As IGraphicsContainer = pPageLayout
        Dim pActiveView As IActiveView = pPageLayout

        Dim pElem As IElement
        Dim pElemProp As IElementProperties2

        pGraphicsContainer.Reset()
        pElem = pGraphicsContainer.Next

        Do While Not pElem Is Nothing
            If TypeOf pElem Is IMapFrame Then
                pElemProp = pElem
                If pElemProp.Name = mapname Then
                    Exit Do
                End If
            End If
            pElem = pGraphicsContainer.Next
        Loop

        return_value = 0

        Dim pEnvelope As IEnvelope
        Dim pFProp As IFrameProperties
        Dim pBorder As ISymbolBorder
        Dim pLSymbol As ILineSymbol
        Dim pColor As IColor

        If Not pElem Is Nothing Then
            pEnvelope = New Envelope
            pEnvelope.PutCoords(xmin, ymin, xmax, ymax)
            pElem.Geometry = pEnvelope

            If No_Border Then
                'set frame border to null
                pFProp = pElem

                pColor = New RgbColor
                pLSymbol = New SimpleLineSymbol
                pBorder = New SymbolBorder
                pColor.NullColor = True
                pLSymbol.Color = pColor
                pBorder.LineSymbol = pLSymbol
                pFProp.Border = pBorder
            End If

            pActiveView.Refresh()
            return_value = 1
        End If

        pFProp = Nothing
        pElem = Nothing
        BA_SetMapFrameDimension = return_value
    End Function

    Public Function BA_ReadSiteAttributes(ByVal sType As SiteType) As IList(Of Site)
        Dim siteClass As IFeatureClass

        Dim nextFeature As IFeature
        Try
            Dim fileName As String = Nothing
            Select Case sType
                Case SiteType.Pseudo
                    fileName = BA_EnumDescription(MapsFileName.Pseudo)
                Case SiteType.Snotel
                    fileName = BA_EnumDescription(MapsFileName.Snotel)
                Case SiteType.SnowCourse
                    fileName = BA_EnumDescription(MapsFileName.SnowCourse)
            End Select
            siteClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers), fileName)
            Dim siteList As IList(Of Site) = New List(Of Site)
            If siteClass IsNot Nothing Then
                ' Create a ComReleaser for cursor management.
                Using comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
                    Dim fCursor As IFeatureCursor = siteClass.Search(Nothing, False)
                    comReleaser.ManageLifetime(fCursor)
                    nextFeature = fCursor.NextFeature
                    Dim idxOid = siteClass.FindField(BA_FIELD_OBJECT_ID)
                    Dim idxName = siteClass.FindField(BA_SiteNameField)
                    Dim idxElev = siteClass.FindField(BA_SiteElevField)
                    Do Until nextFeature Is Nothing
                        'Object Id: This is a built-in field so it should always exist
                        Dim oid As Integer = Convert.ToInt32(nextFeature.Value(idxOid))
                        'Site Name
                        Dim sName As String = Nothing
                        If idxName > -1 Then
                            sName = Convert.ToString(nextFeature.Value(idxName))
                        End If
                        'Site Elevation
                        Dim elev As Double = BA_9999
                        If idxElev > -1 Then
                            If nextFeature.Value(idxElev) IsNot DBNull.Value Then
                                elev = Convert.ToDouble(nextFeature.Value(idxElev))
                            End If

                        End If

                        Dim newSite As Site = New Site(oid, sName, sType, elev, False)
                        siteList.Add(newSite)
                        nextFeature = fCursor.NextFeature
                    Loop
                End Using
            End If
            Return siteList
        Catch ex As Exception
            Debug.Print("BA_ReadSiteAttributes Exception: " & ex.Message)
            Return Nothing
        Finally
            siteClass = Nothing
        End Try
    End Function

    'Adds a feature to the feature class and returns the OID
    Public Function BA_AddGraphic2Shapefile(ByVal FolderPath As String, ByVal shapefilename As String) As Integer

        'Create graphics container and set to focus map document
        Dim pGC As IGraphicsContainer = My.ArcMap.Document.FocusMap
        pGC.Reset()

        'Create element and assign it to the graphic element from other subroutine
        Dim pElem As IElement = pGC.Next

        'If there are no graphics in the active view, then abort the process
        If pElem Is Nothing Then
            Return -1
        End If

        Dim pGeom As IGeometry = pElem.Geometry
        'Determine the geometry type
        Dim inGeoType As Integer 'indicate the geometry type, point or polygon
        Select Case pGeom.GeometryType
            Case esriGeometryType.esriGeometryPoint 'point
                inGeoType = esriGeometryType.esriGeometryPoint
            Case esriGeometryType.esriGeometryPolygon  'polygon
                inGeoType = esriGeometryType.esriGeometryPolygon
            Case Else
                Return -1 'other geometry types are not supported
        End Select

        Try
            Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
            Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromGDB(FolderPath, shapefilename)
            comReleaser.ManageLifetime(pFeatClass)
            'Add the elements
            Dim pFeat As IFeature = pFeatClass.CreateFeature
            pFeat.Shape = pGeom
            pFeat.Store()
            Return pFeat.OID
        Catch ex As Exception
            Debug.Print("BA_AddGraphic2Shapefile Exception: " & ex.Message)
        End Try

    End Function

    Public Function BA_UpdatePseudoSiteAttributes(ByVal FolderPath As String, ByVal shapefilename As String,
                                                  ByVal OID As Integer, ByVal pSite As Site) As BA_ReturnCode
        Try
            Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
            Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromGDB(FolderPath, shapefilename)
            comReleaser.ManageLifetime(pFeatClass)
            'Name field
            Dim idxName As Integer = pFeatClass.FindField(BA_SiteNameField)
            Dim idxElev As Integer = pFeatClass.FindField(BA_SiteElevField)
            Dim idxType As Integer = pFeatClass.FindField(BA_SiteTypeField)
            If idxName < 0 Then
                Dim pFld As IFieldEdit = New Field
                pFld.Name_2 = BA_SiteNameField
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = 50
                pFld.Required_2 = 0
                pFeatClass.AddField(pFld)
                idxName = pFeatClass.FindField(BA_SiteNameField)
            End If
            If idxElev < 0 Then
                Dim pFld As IFieldEdit = New Field
                pFld.Name_2 = BA_SiteElevField
                pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                pFld.Required_2 = 0
                pFeatClass.AddField(pFld)
                idxElev = pFeatClass.FindField(BA_SiteElevField)
            End If
            If idxType < 0 Then
                Dim pFld As IFieldEdit = New Field
                pFld.Name_2 = BA_SiteTypeField
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = 5
                pFld.Required_2 = False
                ' Add field
                pFeatClass.AddField(pFld)
                idxType = pFeatClass.FindField(BA_SiteTypeField)
            End If
            Dim queryFilter As IQueryFilter = New QueryFilter
            queryFilter.WhereClause = "OBJECTID = " & OID
            Dim fCursor As IFeatureCursor = pFeatClass.Update(queryFilter, False)
            comReleaser.ManageLifetime(fCursor)
            Dim pFeature As IFeature = fCursor.NextFeature
            If pFeature IsNot Nothing Then
                pFeature.Value(idxName) = pSite.Name
                pFeature.Value(idxElev) = pSite.Elevation
                pFeature.Value(idxType) = BA_SitePseudo
                fCursor.UpdateFeature(pFeature)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_UpdatePseudoSiteAttributes: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try

    End Function

    'Deletes a site according to its object id
    Public Function BA_DeleteSite(ByVal folderPath As String, ByVal fileName As String,
                                  ByVal dSite As Site) As BA_ReturnCode
        Dim fClass As IFeatureClass
        Dim fCursor As IFeatureCursor
        Dim queryFilter As IQueryFilter
        Dim pFeature As IFeature
        Try
            fClass = BA_OpenFeatureClassFromGDB(folderPath, fileName)
            If fClass IsNot Nothing Then
                queryFilter = New QueryFilter
                queryFilter.WhereClause = "OBJECTID = " & dSite.ObjectId
                fCursor = fClass.Search(queryFilter, Nothing)
                pFeature = fCursor.NextFeature
                If pFeature IsNot Nothing Then
                    pFeature.Delete()
                    Return BA_ReturnCode.Success
                End If
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_DeleteSite Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            fClass = Nothing
            fCursor = Nothing
            queryFilter = Nothing
            pFeature = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_GetSiteScenarioFileName(ByVal parentFolder As String, ByVal pSite As Site) As String
        Dim sb As StringBuilder = New StringBuilder
        Dim fieldChecker As IFieldChecker = New FieldChecker
        Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim workspace As IWorkspace = Nothing
        Try
            'Replace spaces with underscores in site name
            Dim siteName As String = pSite.Name
            siteName = siteName.Replace(" ", "_")
            sb.Append(siteName)
            sb.Append("_")
            sb.Append(pSite.SiteType)
            sb.Append("_")
            sb.Append(pSite.ObjectId)
            sb.Append("_Rep")
            ' Strip trailing "\" if exists
            If parentFolder(Len(parentFolder) - 1) = "\" Then
                parentFolder = parentFolder.Remove(Len(parentFolder) - 1, 1)
            End If
            workspace = pWSFactory.OpenFromFile(parentFolder, 0)
            Dim validFileName As String = "Error"
            If workspace IsNot Nothing Then
                fieldChecker.ValidateWorkspace = workspace
                fieldChecker.ValidateTableName(sb.ToString, validFileName)
            End If
            Return validFileName
        Catch ex As Exception
            Debug.Print("BA_GetSiteScenarioFileName Exception: " & ex.Message)
            Return "Error"
        Finally
            workspace = Nothing
        End Try
    End Function

    Public Function BA_CreateBufferPolygonFile(ByVal pointFolder As String, ByVal pointFile As String,
                                               ByVal pointId As Integer, ByVal bufferDistance As Double,
                                               ByVal outputFolder As String, ByVal outputFile As String,
                                               ByVal pSpatialReference As ISpatialReference) As BA_ReturnCode
        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        Dim bufferPoly As IPolygon
        Dim pGeometry As IGeometry
        Dim pWorkspaceFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace
        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFeat As IFeature = Nothing
        Dim pFields As IFields = New Fields
        Dim pFieldsEdit As IFieldsEdit = Nothing
        comReleaser.ManageLifetime(pFieldsEdit)
        Dim pField As IField = New Field
        Dim pFieldEdit As IFieldEdit = Nothing
        comReleaser.ManageLifetime(pFieldEdit)
        Dim pGeomDef As IGeometryDef = New GeometryDef
        Dim pGeomDefEdit As IGeometryDefEdit = Nothing
        comReleaser.ManageLifetime(pGeomDefEdit)

        Try
            'Delete the existing featureClass if it exists
            Dim outputShapeFile As String = BA_StandardizeShapefileName(outputFile)
            If BA_Shapefile_Exists(outputFolder & "\" & outputShapeFile) Then
                Dim retVal As Integer = BA_Remove_Shapefile(outputFolder, outputFile)
                If retVal <> 1 Then
                    MsgBox("Couldn't delete shapefile!")
                End If
            End If

            bufferPoly = BA_BufferPoint(pointFolder, pointFile, pointId, bufferDistance)
            comReleaser.ManageLifetime(bufferPoly)
            pGeometry = CType(bufferPoly, IGeometry)
            comReleaser.ManageLifetime(pGeometry)

            'create the shapefile
            'Set up fields
            pFieldsEdit = CType(pFields, IFieldsEdit)

            ' Make the shape field
            ' it will need a geometry definition, with a spatial reference
            pFieldEdit = pField
            pFieldEdit.Name_2 = BA_FIELD_SHAPE
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry

            pGeomDefEdit = CType(pGeomDef, IGeometryDefEdit)

            'options are esriGeometryPolygon or esriGeometryPoint
            pGeomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon
            pGeomDefEdit.SpatialReference_2 = pSpatialReference

            pFieldEdit.GeometryDef_2 = pGeomDef
            pFieldsEdit.AddField(pField)

            ' Create the shapefile
            pFWS = pWorkspaceFactory.OpenFromFile(outputFolder, 0)
            'comReleaser.ManageLifetime(pFWS)
            pFeatClass = pFWS.CreateFeatureClass(BA_StandardizeShapefileName(outputFile), pFields, Nothing, Nothing, esriFeatureType.esriFTSimple, BA_FIELD_SHAPE, "")
            'comReleaser.ManageLifetime(pFeatClass)

            'Add the elements
            pFeat = pFeatClass.CreateFeature
            comReleaser.ManageLifetime(pFeat)
            pFeat.Shape = pGeometry
            pFeat.Store()

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_CreateBufferPolygonFile Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_CreateBufferPolygonFile2(ByVal pointFolder As String, ByVal pointFile As String,
                                               ByVal pointId As Integer, ByVal bufferDistance As Double,
                                               ByVal aoiFolder As String, ByVal aoiFile As String,
                                                ByVal outputFolder As String, ByVal outputFile As String) As BA_ReturnCode
        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        Dim bufferPoly As IPolygon
        Dim pGeometry As IGeometry
        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFeat As IFeature = Nothing

        Try
            'Delete the existing featureClass if it exists
            If BA_File_Exists(outputFolder & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                BA_Remove_ShapefileFromGDB(outputFolder, outputFile)
            End If

            'Make a copy of the aoi boundary polygon so we can append the buffer polygon to it
            If BA_File_Exists(aoiFolder & "\" & aoiFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Dim success = BA_CopyFeatures(aoiFolder & "\" & aoiFile, outputFolder & "\" & outputFile)
                bufferPoly = BA_BufferPoint(pointFolder, pointFile, pointId, bufferDistance)
                comReleaser.ManageLifetime(bufferPoly)
                pGeometry = CType(bufferPoly, IGeometry)
                comReleaser.ManageLifetime(pGeometry)

                If success = BA_ReturnCode.Success Then
                    pFeatClass = BA_OpenFeatureClassFromGDB(outputFolder, outputFile)
                    comReleaser.ManageLifetime(pFeatClass)
                    'Add the elements
                    pFeat = pFeatClass.CreateFeature
                    pFeat.Shape = pGeometry
                    pFeat.Store()
                    Dim bufferOid As Integer = pFeat.OID

                End If
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_CreateBufferPolygonFile2 Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_ExtractRepresentedArea(ByVal bufferFolder As String, ByVal bufferFile As String,
                                              ByVal demFolder As String, ByVal demFile As String,
                                              ByVal outputFolder As String, ByVal outputFile As String,
                                              ByVal pSite As Site, ByVal upperElev As Double,
                                              ByVal lowerElev As Double, ByVal demMax As Double,
                                              ByVal demMin As Double) As BA_ReturnCode
        Try
            Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            Dim rasBufferPath As String = "extMask"
            bufferFile = BA_StandardizeShapefileName(bufferFile, True, True)
            'Delete the old file if it exists
            If BA_File_Exists(bufferFolder & "\" & rasBufferPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_Remove_Raster(bufferFolder, rasBufferPath)
            End If
            Dim extent As String = BA_QueryFeatureClassExtent(bufferFolder, bufferFile)
            'Dim success As BA_ReturnCode = BA_ExtractByMask(bufferFolder & bufferFile, demFolder & "\" & demFile, _
            '                                                snapRasterPath, bufferFolder & "\" & rasBufferPath, extent)
            Dim success As BA_ReturnCode = BA_ExtractByFeatureClass(bufferFolder, bufferFile, demFolder, demFile,
                                                                    bufferFolder, rasBufferPath)
            If success = BA_ReturnCode.Success Then
                'Build list of reclass items
                Dim reclassList As New ArrayList()
                Dim minElev As Double = demMin - 0.005
                If lowerElev <> BA_9999 Then
                    minElev = pSite.Elevation - lowerElev
                    'Non-represented below
                    If minElev > demMin - 0.005 Then
                        'Dim belowItem As ReclassItem = New ReclassItem(demMin - 0.005, minElev, BA_ValNonRepresentedBelow)
                        Dim belowString As String = demMin - 0.005 & " " & minElev & " NoData; "
                        reclassList.Add(belowString)
                    Else
                        minElev = demMin - 0.005
                    End If
                End If
                Dim maxElev As Double = demMax + 0.005
                Dim hasNonRepresentedAbove As Boolean = False
                If upperElev <> BA_9999 Then
                    maxElev = pSite.Elevation + upperElev
                    hasNonRepresentedAbove = True
                    If maxElev > demMax + 0.005 Then
                        maxElev = demMax + 0.005
                        hasNonRepresentedAbove = False
                    End If
                End If
                'Dim representedItem As ReclassItem = New ReclassItem(minElev, maxElev, BA_ValRepresented)
                Dim representedString As String = minElev & " " & maxElev & " 1; "
                reclassList.Add(representedString)
                If hasNonRepresentedAbove = True Then
                    'Dim aboveItem As ReclassItem = New ReclassItem(maxElev, demMax + 0.05, BA_ValNonRepresentedAbove)
                    Dim aboveString As String = maxElev & " " & demMax + 0.05 & " NoData; "
                    reclassList.Add(aboveString)
                End If
                'Dim objItems() As Object = reclassList.ToArray
                'Dim items(objItems.GetUpperBound(0)) As ReclassItem
                'Dim i As Integer = 0
                'For Each obj As Object In objItems
                '    items(i) = CType(obj, ReclassItem)
                '    i += 1
                'Next
                Dim reclassString As String = ""
                For Each strItem As String In reclassList
                    reclassString = reclassString & strItem
                Next

                Dim tempOutputFile As String = "reclElev"
                'success = BA_ReclassifyRasterFromTable(outputFolder & "\" & rasBufferPath, BA_FIELD_VALUE, items, _
                '                                       outputFolder & "\" & tempOutputFile, ActionType.ReclCont, _
                '                                       snapRasterPath)
                success = BA_ReclassifyRasterFromString(bufferFolder & "\" & rasBufferPath, BA_FIELD_VALUE, reclassString,
                                                        bufferFolder & "\" & tempOutputFile, snapRasterPath)
                If success = BA_ReturnCode.Success Then
                    'Check to see if output file already exists and delete if it does
                    If BA_File_Exists(outputFolder & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                        BA_Remove_ShapefileFromGDB(outputFolder, outputFile)
                    End If

                    success = BA_Raster2Polygon_GP(bufferFolder & "\" & tempOutputFile,
                                                   outputFolder & "\" & outputFile, snapRasterPath)
                    'success = BA_ReplaceNoDataCellsGDB(outputFolder, tempOutputFile, outputFolder, outputFile, BA_ValNonRepresentedOutside, _
                    '                                   BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi), BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage))
                    If success = BA_ReturnCode.Success Then
                        'Remove the masked raster
                        If BA_File_Exists(bufferFolder & "\" & rasBufferPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                            BA_Remove_Raster(bufferFolder, rasBufferPath)
                        End If
                        'Remove the elev reclass file
                        If BA_File_Exists(bufferFolder & "\" & tempOutputFile, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                            BA_Remove_Raster(bufferFolder, tempOutputFile)
                        End If
                    End If
                End If
            End If
            Return success
        Catch ex As Exception
            Debug.Print("BA_ExtractRepresentedArea Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_ReclassNonRepresentedArea(ByVal outputFolder As String, ByVal inputFile As String,
                                                 ByVal outputFile As String, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim items(3) As ReclassItem
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Try
            Dim representedItem As ReclassItem = New ReclassItem(BA_ValRepresented, BA_ValRepresented, BA_ValRepresented)
            items(0) = representedItem
            Dim nonRepresentedBelow As ReclassItem = New ReclassItem(BA_ValNonRepresentedBelow, BA_ValNonRepresentedBelow, BA_ValNonRepresented)
            items(1) = nonRepresentedBelow
            Dim nonRepresentedAbove As ReclassItem = New ReclassItem(BA_ValNonRepresentedAbove, BA_ValNonRepresentedAbove, BA_ValNonRepresented)
            items(2) = nonRepresentedAbove
            Dim nonRepresentedOutside As ReclassItem = New ReclassItem(BA_ValNonRepresentedOutside, BA_ValNonRepresentedOutside, BA_ValNonRepresented)
            items(3) = nonRepresentedOutside
            success = BA_ReclassifyRasterFromTable(outputFolder & "\" & inputFile, BA_FIELD_VALUE, items,
                                       outputFolder & "\" & outputFile, ActionType.ReclDisc,
                                       snapRasterPath)

        Catch ex As Exception
            Debug.Print("BA_ReclassNonRepresentedArea Exception: " & ex.Message)
            Return success
        End Try
    End Function

    Public Function BA_UpdateAreaNames(ByVal folderName As String, ByVal rasterName As String, ByVal usedBuffer As Boolean) As BA_ReturnCode
        Dim pRasterDS As IRasterDataset
        Dim pBandCol As IRasterBandCollection
        Dim pBand As IRasterBand
        Dim attribTable As ITable
        Dim pCursor As ICursor
        Dim pRow As IRow
        Dim pFld As IFieldEdit = Nothing
        Try
            ' Get the number of rows from raster table
            pRasterDS = CType(BA_OpenRasterFromGDB(folderName, rasterName), IRasterDataset)
            pBandCol = CType(pRasterDS, IRasterBandCollection)
            pBand = pBandCol.Item(0)
            Dim TableExist As Boolean
            pBand.HasTable(TableExist)
            If Not TableExist Then Return BA_ReturnCode.UnknownError
            attribTable = pBand.AttributeTable
            Dim idxValue As Integer = attribTable.FindField(BA_FIELD_VALUE)
            Dim idxName As Integer = attribTable.FindField(BA_FIELD_NAME)
            If idxName < 1 Then
                'Define field name
                pFld = New Field
                pFld.Name_2 = BA_FIELD_NAME
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = BA_NAME_FIELD_WIDTH
                attribTable.AddField(pFld)
                idxName = attribTable.FindField(BA_FIELD_NAME)
            End If
            pCursor = attribTable.Update(Nothing, False)
            pRow = pCursor.NextRow
            Do While pRow IsNot Nothing
                Dim pValue As Integer = Convert.ToInt32(pRow.Value(idxValue))
                If usedBuffer = True Then
                    Select Case pValue
                        Case BA_ValRepresented
                            pRow.Value(idxName) = "Represented"
                        Case BA_ValNonRepresented
                            pRow.Value(idxName) = "Not Represented"
                    End Select
                Else
                    Select Case pValue
                        Case BA_ValRepresented
                            pRow.Value(idxName) = "Represented"
                        Case BA_ValNonRepresentedBelow
                            pRow.Value(idxName) = "Not Represented"
                        Case BA_ValNonRepresented
                            pRow.Value(idxName) = "Not Represented"
                    End Select
                End If
                pCursor.UpdateRow(pRow)
                pRow = pCursor.NextRow
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_UpdateAreaNames Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFld = Nothing
            pRow = Nothing
            pCursor = Nothing
            attribTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_UpdateSiteNames(ByVal folderName As String, ByVal rasterName As String) As BA_ReturnCode
        Dim pRasterDS As IRasterDataset
        Dim pBandCol As IRasterBandCollection
        Dim pBand As IRasterBand
        Dim attribTable As ITable
        Dim pCursor As ICursor
        Dim pRow As IRow
        Dim pFld As IFieldEdit = Nothing
        Try
            ' Get the number of rows from raster table
            pRasterDS = CType(BA_OpenRasterFromGDB(folderName, rasterName), IRasterDataset)
            pBandCol = CType(pRasterDS, IRasterBandCollection)
            pBand = pBandCol.Item(0)
            Dim TableExist As Boolean
            pBand.HasTable(TableExist)
            If Not TableExist Then Return BA_ReturnCode.UnknownError
            attribTable = pBand.AttributeTable
            Dim idxValue As Integer = attribTable.FindField(BA_FIELD_VALUE)
            Dim idxName As Integer = attribTable.FindField(BA_FIELD_NAME)
            If idxName < 1 Then
                'Define field name
                pFld = New Field
                pFld.Name_2 = BA_FIELD_NAME
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = BA_NAME_FIELD_WIDTH
                attribTable.AddField(pFld)
                idxName = attribTable.FindField(BA_FIELD_NAME)
            End If
            pCursor = attribTable.Update(Nothing, False)
            pRow = pCursor.NextRow
            Do While pRow IsNot Nothing
                Dim pValue As Integer = Convert.ToInt32(pRow.Value(idxValue))
                Select Case pValue
                    Case BA_ValRepresented
                        pRow.Value(idxName) = "Represented"
                    Case BA_ValNonRepresentedBelow
                        pRow.Value(idxName) = "Non-represented below"
                    Case BA_ValNonRepresentedAbove
                        pRow.Value(idxName) = "Non-represented above"
                End Select
                pCursor.UpdateRow(pRow)
                pRow = pCursor.NextRow
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_UpdateSiteNames Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFld = Nothing
            pRow = Nothing
            pCursor = Nothing
            attribTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Sub BA_AddScenarioLayersToMapFrame(ByVal pApplication As ESRI.ArcGIS.Framework.IApplication, ByVal pMxDoc As IMxDocument,
                                              ByVal aoiPath As String)
        BA_RemoveScenarioLayersfromMapFrame(pMxDoc)
        'add vector layers first

        'Scenario 1
        Dim filepath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis, True)
        Dim FileName As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
        Dim filepathname As String = filepath & FileName
        Dim pColor As IColor = New RgbColor
        pColor.RGB = RGB(255, 0, 0) 'red
        Dim success As BA_ReturnCode = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_SCENARIO1_REPRESENTATION, pColor, 0)

        'Scenario 2
        FileName = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
        filepathname = filepath & FileName
        If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_SCENARIO2_REPRESENTATION, pColor, 0)
        End If

        'Both scenarios
        FileName = BA_EnumDescription(MapsFileName.DifferenceRepresentedArea)
        filepathname = filepath & FileName
        pColor.RGB = RGB(48, 95, 207) 'red
        If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            success = BA_MapDisplayPolygon(pMxDoc, filepathname, BA_MAPS_BOTH_REPRESENTATION, pColor, 0)
        End If
        Dim response As Integer = -1

        'add aoi boundary and zoom to AOI
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
        FileName = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        filepathname = filepath & FileName
        response = BA_AddExtentLayer(pMxDoc, filepathname, Nothing, False, BA_MAPS_AOI_BOUNDARY, 0, 1.2, 2.0)

        'add aoi streams layer
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Layers, True)
        FileName = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiStreamsVector), False)
        filepathname = filepath & FileName
        If BA_File_Exists(filepathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            pColor.RGB = RGB(0, 0, 255)
            response = BA_AddLineLayer(pMxDoc, filepathname, BA_MAPS_STREAMS, pColor, 0)
        End If

        'add snotel, snow course, and pseudosite layers
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Layers, True)
        If AOI_HasSNOTEL Then
            FileName = BA_EnumDescription(MapsFileName.Snotel)
            filepathname = filepath & FileName
            pColor.RGB = RGB(0, 0, 255)
            response = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.Snotel, pColor, MapsMarkerType.Snotel)
        End If

        If AOI_HasSnowCourse Then
            FileName = BA_EnumDescription(MapsFileName.SnowCourse)
            filepathname = filepath & FileName
            pColor.RGB = RGB(0, 255, 255) 'cyan
            response = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.SnowCourse, pColor, MapsMarkerType.SnowCourse)
        End If

        If AOI_HasPseudoSite Then
            FileName = BA_EnumDescription(MapsFileName.Pseudo)
            filepathname = filepath & FileName
            pColor.RGB = RGB(255, 170, 0) 'electron gold
            response = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.pseudo_sites, pColor, MapsMarkerType.PseudoSite)
        End If

        'add hillshade
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True)
        FileName = BA_GetBareName(BA_EnumDescription(PublicPath.Hillshade))
        filepathname = filepath & FileName
        response = BA_MapDisplayRaster(pMxDoc, filepathname, BA_MAPS_HILLSHADE, 0)
        'Move hillshade to bottom
        Dim layerCount As Integer = My.Document.FocusMap.LayerCount
        Dim idxHillshade As Integer = BA_GetLayerIndexByName(My.Document, BA_MAPS_HILLSHADE)
        Dim hLayer As ILayer = My.Document.FocusMap.Layer(idxHillshade)
        My.Document.FocusMap.MoveLayer(hLayer, layerCount)

        'add filled DEM
        'filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True)
        'FileName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        'filepathname = filepath & FileName
        'response = BA_MapDisplayRaster(pMxDoc, filepath & FileName, BA_MAPS_FILLED_DEM, 0)

        'add Elevation Zones
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis, True)
        FileName = BA_EnumDescription(MapsFileName.ElevationZone)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_ELEVATION_ZONES, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)

        'If response < 0 Then
        '    ActualRepMap_Flag = False
        'Else
        '    ActualRepMap_Flag = True
        'End If

        'zoom to the aoi boundary layer
        BA_ZoomToAOI(pMxDoc, aoiPath)
    End Sub

    Public Function BA_RemoveScenarioLayersfromMapFrame(ByVal pMxDoc As IMxDocument) As Integer
        Dim LayerNames(0 To 14) As String
        LayerNames(1) = BA_MAPS_SNOTEL_SITES
        LayerNames(2) = BA_MAPS_SNOW_COURSE_SITES
        LayerNames(3) = BA_MAPS_PSEUDO_SITES
        LayerNames(4) = BA_MAPS_SCENARIO1_REPRESENTATION
        LayerNames(5) = BA_MAPS_SCENARIO2_REPRESENTATION
        LayerNames(6) = BA_MAPS_BOTH_REPRESENTATION
        LayerNames(7) = BA_MAPS_SNOTEL_SCENARIO1
        LayerNames(8) = BA_MAPS_SNOTEL_SCENARIO2
        LayerNames(9) = BA_MAPS_SNOW_COURSE_SCENARIO1
        LayerNames(10) = BA_MAPS_SNOW_COURSE_SCENARIO2
        LayerNames(11) = BA_MAPS_PSEUDO_SCENARIO1
        LayerNames(12) = BA_MAPS_PSEUDO_SCENARIO2
        LayerNames(13) = BA_MAPS_AOI_BASEMAP
        LayerNames(14) = BA_MAPS_ELEVATION_ZONES

        For j = 1 To 14
            BA_RemoveLayers(pMxDoc, LayerNames(j))
        Next

        'Remove any representation layers
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pTempLayer As ILayer
        Dim nlayers As Integer = pMap.LayerCount
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            Dim suffix As String = Right(pTempLayer.Name, 4)
            If suffix = "_Rep" Then 'remove the layer
                If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
                    Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
                    pDLayer.Disconnect()
                End If
                pMap.DeleteLayer(pTempLayer)
            End If
        Next

        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        pMxDoc.UpdateContents()
        Return 1
    End Function

    Public Function BA_ReclassByElevationOnly(ByVal demFolderPath As String, ByVal outputFolder As String, _
                                              ByVal snapRasterPath As String, _
                                              ByVal highestSite As Double, ByVal lowestSite As Double, _
                                              ByVal upperElev As Double, ByVal lowerElev As Double, _
                                              ByVal demMax As Double, ByVal demMin As Double) As BA_ReturnCode
        Try
            'Build list of reclass items
            Dim reclassList As New ArrayList()
            Dim minElev As Double = demMin - 0.005
            If lowerElev <> BA_9999 Then
                minElev = lowestSite - lowerElev
                'Non-represented below
                If minElev > demMin - 0.005 Then
                    'Dim belowItem As ReclassItem = New ReclassItem(demMin - 0.005, minElev, BA_ValNonRepresented)
                    Dim belowString As String = demMin - 0.005 & " " & minElev & " NoData; "
                    reclassList.Add(belowString)
                Else
                    minElev = demMin - 0.005
                End If
            End If
            Dim maxElev As Double = demMax + 0.005
            Dim hasNonRepresentedAbove As Boolean = False
            If upperElev <> BA_9999 Then
                maxElev = highestSite + upperElev
                hasNonRepresentedAbove = True
                If maxElev > demMax + 0.005 Then
                    maxElev = demMax + 0.005
                    hasNonRepresentedAbove = False
                End If
            End If
            'Dim representedItem As ReclassItem = New ReclassItem(minElev, maxElev, BA_ValRepresented)
            Dim representedString As String = minElev & " " & maxElev & " 1; "
            reclassList.Add(representedString)
            'Even though this is non-represented above, we don't care at the AOI level. This avoids having to reclass later
            If hasNonRepresentedAbove = True Then
                'Dim aboveItem As ReclassItem = New ReclassItem(maxElev, demMax + 0.05, BA_ValNonRepresented)
                Dim aboveString As String = maxElev & " " & demMax + 0.05 & " NoData; "
                reclassList.Add(aboveString)
            End If
            Dim reclassString As String = ""
            For Each strItem As String In reclassList
                reclassString = reclassString & strItem
            Next
            BA_ReclassifyRasterFromString(demFolderPath, BA_FIELD_VALUE, reclassString, outputFolder, snapRasterPath)

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ReclassByElevationOnly Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try

    End Function

    'Public Function BA_CreateRepresentationDifference(ByVal snapRasterPath As String, ByVal layerList As IList(Of String), _
    '                                                  ByVal outputFolder As String, ByVal outputName As String) As BA_ReturnCode
    '    Dim pRasterDS As IRasterDataset
    '    Dim pBandCol As IRasterBandCollection
    '    Dim pBand As IRasterBand
    '    Dim attribTable As ITable
    '    Dim qFilter As IQueryFilter = New QueryFilter
    '    Dim actualRepresentedCursor As ICursor
    '    Dim pseudoRepresentedCursor As ICursor
    '    Dim nonRepresentedCursor As ICursor
    '    Dim pRow As IRow
    '    Dim pFld As IFieldEdit = Nothing
    '    Try
    'Dim success As BA_ReturnCode = BA_ZoneOverlay(snapRasterPath, layerList, outputFolder, outputName, _
    '                                                      False, True, snapRasterPath, WorkspaceType.Geodatabase)
    '        If success = BA_ReturnCode.Success Then
    '            ' Get the number of rows from raster table
    '            pRasterDS = CType(BA_OpenRasterFromGDB(outputFolder, outputName), IRasterDataset)
    '            pBandCol = CType(pRasterDS, IRasterBandCollection)
    '            pBand = pBandCol.Item(0)
    '            Dim TableExist As Boolean
    '            pBand.HasTable(TableExist)
    '            If Not TableExist Then Return BA_ReturnCode.UnknownError
    '            attribTable = pBand.AttributeTable
    '            Dim idxName As Integer = attribTable.FindField(BA_FIELD_NAME)
    '            If idxName < 1 Then
    '                'Define field name
    '                pFld = New Field
    '                pFld.Name_2 = BA_FIELD_NAME
    '                pFld.Type_2 = esriFieldType.esriFieldTypeString
    '                pFld.Length_2 = BA_NAME_FIELD_WIDTH
    '                attribTable.AddField(pFld)
    '                idxName = attribTable.FindField(BA_FIELD_NAME)
    '            End If
    '            Dim actualLayer As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
    '            Dim pseudoLayer As String = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
    '            qFilter.WhereClause = actualLayer & " = " & CInt(BA_ValRepresented)
    '            actualRepresentedCursor = attribTable.Update(qFilter, False)
    '            pRow = actualRepresentedCursor.NextRow
    '            Do While pRow IsNot Nothing
    '                Dim idxNppseduo As Integer = attribTable.FindField(pseudoLayer)
    '                If idxNppseduo > 0 Then
    '                    Dim nppseduo As Integer = Convert.ToInt32(pRow.Value(idxNppseduo))
    '                    If nppseduo = BA_ValRepresented Then
    '                        pRow.Value(idxName) = "Represented in both scenarios"
    '                    Else
    '                        pRow.Value(idxName) = "Represented only in scenario 1"
    '                    End If
    '                End If
    '                actualRepresentedCursor.UpdateRow(pRow)
    '                pRow = actualRepresentedCursor.NextRow
    '            Loop
    '            qFilter.WhereClause = pseudoLayer & " = " & CInt(BA_ValRepresented)
    '            pseudoRepresentedCursor = attribTable.Update(qFilter, False)
    '            pRow = pseudoRepresentedCursor.NextRow
    '            Do While pRow IsNot Nothing
    '                Dim idxActual As Integer = attribTable.FindField(actualLayer)
    '                If idxActual > 0 Then
    '                    Dim npactual As Integer = Convert.ToInt32(pRow.Value(idxActual))
    '                    If npactual <> BA_ValRepresented Then
    '                        pRow.Value(idxName) = "Represented only in scenario 2"
    '                        pseudoRepresentedCursor.UpdateRow(pRow)
    '                    End If
    '                End If
    '                pRow = pseudoRepresentedCursor.NextRow
    '            Loop
    '            qFilter.WhereClause = pseudoLayer & " <> " & CInt(BA_ValRepresented) & " AND " & actualLayer & " <> " & CInt(BA_ValRepresented)
    '            nonRepresentedCursor = attribTable.Update(qFilter, False)
    '            pRow = nonRepresentedCursor.NextRow
    '            Do While pRow IsNot Nothing
    '                pRow.Value(idxName) = "Not represented in both scenarios"
    '                nonRepresentedCursor.UpdateRow(pRow)
    '                pRow = nonRepresentedCursor.NextRow
    '            Loop
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("BA_CreateRepresentationDifference Exception: " & ex.Message)
    '        Return BA_ReturnCode.UnknownError
    '    Finally
    '        pFld = Nothing
    '        pRow = Nothing
    '        actualRepresentedCursor = Nothing
    '        pseudoRepresentedCursor = Nothing
    '        nonRepresentedCursor = Nothing
    '        attribTable = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Function

    Public Function BA_CreateRepresentationDifference(ByVal snapRasterPath As String, ByVal layerList As IList(Of String), _
                                                      ByVal outputFolder As String, ByVal outputName As String) As BA_ReturnCode
        Dim sb As StringBuilder = New StringBuilder
        If layerList.Count > 0 Then
            For Each layerName In layerList
                sb.Append(layerName)
                sb.Append("; ")
            Next
        Else
            Return BA_ReturnCode.NotSupportedOperation
        End If
        Return BA_Intersect(sb.ToString, outputFolder & "\" & outputName)
    End Function

    Public Sub BA_RemoveAllLayersfromLegend(ByVal pMxDoc As IMxDocument)
        Try
            'Set the page layout.
            Dim pPageLayout As IPageLayout = pMxDoc.PageLayout
            Dim pGraphicsContainer As IGraphicsContainer = CType(pPageLayout, IGraphicsContainer)   'Explicit Cast
            pGraphicsContainer.Reset()
            Dim pMElem As IElement = pGraphicsContainer.Next
            Dim pElemProp As IElementProperties2
            Dim IsLegend As Boolean = False
            Do While Not pMElem Is Nothing
                pElemProp = CType(pMElem, IElementProperties2)
                If pElemProp.Name = "Legend" Then
                    IsLegend = True
                    Exit Do
                End If
                pMElem = pGraphicsContainer.Next
            Loop
            If IsLegend Then
                Dim pMapSurround As IMapSurround
                Dim pMapSurroundFrame As IMapSurroundFrame
                Dim pLegend As ESRI.ArcGIS.Carto.ILegend
                pMapSurroundFrame = CType(pMElem, IMapSurroundFrame)
                pMapSurround = pMapSurroundFrame.MapSurround
                pLegend = CType(pMapSurround, ESRI.ArcGIS.Carto.ILegend)
                pLegend.ClearItems()
            End If
        Catch ex As Exception
            Debug.Print("BA_RemoveAllLayersfromLegend Exception: " & ex.Message)
        End Try
    End Sub

    ' Populates an Analysis object from an XML file
    Public Function BA_LoadAnalysisFromXml(ByVal aoiPath As String) As Analysis
        Try
            Dim xmlInputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.AnalysisXml)
            If BA_File_ExistsWindowsIO(xmlInputPath) Then
                Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(Analysis))
                If obj IsNot Nothing Then
                    Dim analysis As Analysis = CType(obj, Analysis)
                    Return analysis
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Debug.Print("BA_LoadAnalysisFromXml Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    'Public Function BA_CalculateAreaStatistics(ByVal elevUnits As ESRI.ArcGIS.esriSystem.esriUnits, _
    '                                           ByVal countScenario2Sites As Integer, ByVal pStepProg As IStepProgressor) As AreaStatistics
    '    Dim aStats As AreaStatistics = New AreaStatistics
    '    Dim zonalOp As IZonalOp = New RasterZonalOp
    '    Dim extractionOp As IExtractionOp2 = New RasterExtractionOp
    '    Dim env As IRasterAnalysisEnvironment = Nothing
    '    Dim inputGds As IGeoDataset = Nothing
    '    Dim maskGds As IGeoDataset = Nothing
    '    Dim extractGds As IGeoDataset = Nothing
    '    Dim pTable As ITable = Nothing
    '    Dim s1Cursor As ICursor = Nothing
    '    Dim s2Cursor As ICursor = Nothing
    '    Dim diffCursor As ICursor = Nothing
    '    Dim pRow As IRow = Nothing
    '    Try
    '        Dim inputFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
    '        Dim inputFile As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
    '        inputGds = BA_OpenRasterFromGDB(inputFolder, inputFile)
    '        maskGds = BA_OpenRasterFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi), _
    '                                       BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiGrid), False))
    '        If inputGds IsNot Nothing And maskGds IsNot Nothing Then
    '            pStepProg.Message = "Calculate area totals for scenario 1"
    '            pStepProg.Step()
    '            System.Windows.Forms.Application.DoEvents()
    '            'It seems we have to extract the area before running the zonalOp because ArcObjects ignores the mask variable
    '            extractGds = extractionOp.Raster(inputGds, maskGds)
    '            env = CType(zonalOp, IRasterAnalysisEnvironment)
    '            env.Mask = maskGds
    '            ' Set the analysis extent to match the mask
    '            Dim extentProvider As Object = CType(maskGds.Extent, Object)
    '            env.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, extentProvider)
    '            pTable = zonalOp.ZonalGeometryAsTable(extractGds)
    '            Dim idxArea As Integer = pTable.FindField(BA_FIELD_AREA)
    '            Dim idxValue As Integer = pTable.FindField(BA_FIELD_VALUE)
    '            If idxArea > -1 And idxValue > -1 Then
    '                s1Cursor = pTable.Search(Nothing, False)
    '                pRow = s1Cursor.NextRow
    '                Do Until pRow Is Nothing
    '                    Dim pValue As Integer = Convert.ToInt16(pRow.Value(idxValue))
    '                    Select Case pValue
    '                        Case BA_ValRepresented
    '                            aStats.S1RepArea = Convert.ToDouble(pRow.Value(idxArea))
    '                        Case BA_ValNonRepresented
    '                            aStats.S1NonRepArea = Convert.ToDouble(pRow.Value(idxArea))
    '                    End Select
    '                    pRow = s1Cursor.NextRow
    '                Loop
    '            End If
    '            'Clear out variables to be re-used
    '            inputGds = Nothing
    '            extractGds = Nothing
    '            pTable = Nothing
    '            'Calculate area in meters (and feet)
    '            Dim s1RepAreaSqKm As Double
    '            Dim s1NonRepAreaSqKm As Double
    '            If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
    '                s1RepAreaSqKm = aStats.S1RepArea / 1000000
    '                aStats.S1RepAreaSqKm = Format(Math.Round(s1RepAreaSqKm, 3), "#0.000")
    '                s1NonRepAreaSqKm = aStats.S1NonRepArea / 1000000
    '                aStats.S1NonRepAreaSqKm = Format(Math.Round(s1NonRepAreaSqKm, 3), "#0.000")
    '                Dim s1RepAreaSqMiles As Double = s1RepAreaSqKm * BA_SQKm_To_SQMile
    '                aStats.S1RepAreaSqMi = Format(Math.Round(s1RepAreaSqMiles, 3), "#0.000")
    '                Dim s1NonRepAreaSqMiles As Double = s1NonRepAreaSqKm * BA_SQKm_To_SQMile
    '                aStats.S1NonRepAreaSqMi = Format(Math.Round(s1NonRepAreaSqMiles, 3), "#0.000")
    '                Dim s1RepAreaHectares As Double = s1RepAreaSqKm * BA_SQKm_To_HECTARE
    '                aStats.S1RepAreaHect = Format(Math.Round(s1RepAreaHectares, 2), "#0.00")
    '                Dim s1NonRepAreaHectares As Double = s1NonRepAreaSqKm * BA_SQKm_To_HECTARE
    '                aStats.S1NonRepAreaHect = Format(Math.Round(s1NonRepAreaHectares, 2), "#0.00")
    '                Dim s1RepAcres As Double = s1RepAreaSqKm * BA_SQKm_To_ACRE
    '                aStats.S1RepAreaAcres = Format(Math.Round(s1RepAcres, 2), "#0.00")
    '                Dim s1NonRepAcres As Double = s1NonRepAreaSqKm * BA_SQKm_To_ACRE
    '                aStats.S1NonRepAreaAcres = Format(Math.Round(s1NonRepAcres, 2), "#0.00")
    '            ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
    '                Dim s1RepSqMiles As Double = aStats.S1RepArea / BA_SQFeet_To_SQMile
    '                aStats.S1RepAreaSqMi = Format(Math.Round(s1RepSqMiles, 3), "#0.000")
    '                Dim s1NonRepSqMiles As Double = aStats.S1NonRepArea / BA_SQFeet_To_SQMile
    '                aStats.S1NonRepAreaSqMi = Format(Math.Round(s1NonRepSqMiles, 3), "#0.000")
    '                s1RepAreaSqKm = s1RepSqMiles * BA_SQMile_To_SQKm
    '                aStats.S1RepAreaSqKm = Format(Math.Round(s1RepAreaSqKm, 3), "#0.000")
    '                s1NonRepAreaSqKm = s1NonRepSqMiles * BA_SQMile_To_SQKm
    '                aStats.S1NonRepAreaSqKm = Format(Math.Round(s1NonRepAreaSqKm, 3), "#0.000")
    '                Dim s1RepAreaHectares As Double = s1RepAreaSqKm * BA_SQKm_To_HECTARE
    '                aStats.S1RepAreaHect = Format(Math.Round(s1RepAreaHectares, 2), "#0.00")
    '                Dim s1NonRepAreaHectares As Double = s1NonRepAreaSqKm * BA_SQKm_To_HECTARE
    '                aStats.S1NonRepAreaHect = Format(Math.Round(s1NonRepAreaHectares, 2), "#0.00")
    '                Dim s1RepAcres As Double = s1RepAreaSqKm * BA_SQKm_To_ACRE
    '                aStats.S1RepAreaAcres = Format(Math.Round(s1RepAcres, 2), "#0.00")
    '                Dim s1NonRepAcres As Double = s1NonRepAreaSqKm * BA_SQKm_To_ACRE
    '                aStats.S1NonRepAreaAcres = Format(Math.Round(s1NonRepAcres, 2), "#0.00")
    '            End If
    '            'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
    '            Dim s1AoiPctRep As Double = s1RepAreaSqKm / AOI_ShapeArea * 100
    '            aStats.S1AoiPctRep = Format(Math.Round(s1AoiPctRep, 2), "#0.00")
    '            Dim s1AoiPctNonRep As Double = s1NonRepAreaSqKm / AOI_ShapeArea * 100
    '            aStats.S1AoiPctNonRep = Format(Math.Round(s1AoiPctNonRep, 2), "#0.00")
    '            'Run the calculations for scenario 2 if applicable
    '            If countScenario2Sites > 0 Then
    '                pStepProg.Message = "Calculate area totals for scenario 2"
    '                pStepProg.Step()
    '                System.Windows.Forms.Application.DoEvents()
    '                inputFile = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
    '                inputGds = BA_OpenRasterFromGDB(inputFolder, inputFile)
    '                If inputGds IsNot Nothing Then
    '                    'It seems we have to extract the area before running the zonalOp because ArcObjects ignores the mask variable
    '                    extractGds = extractionOp.Raster(inputGds, maskGds)
    '                    pTable = zonalOp.ZonalGeometryAsTable(extractGds)
    '                    idxArea = pTable.FindField(BA_FIELD_AREA)
    '                    idxValue = pTable.FindField(BA_FIELD_VALUE)
    '                    If idxArea > -1 And idxValue > -1 Then
    '                        s2Cursor = pTable.Search(Nothing, False)
    '                        pRow = s2Cursor.NextRow
    '                        Do Until pRow Is Nothing
    '                            Dim pValue As Integer = Convert.ToInt16(pRow.Value(idxValue))
    '                            Select Case pValue
    '                                Case BA_ValRepresented
    '                                    aStats.S2RepArea = Convert.ToDouble(pRow.Value(idxArea))
    '                                Case BA_ValNonRepresented
    '                                    aStats.S2NonRepArea = Convert.ToDouble(pRow.Value(idxArea))
    '                            End Select
    '                            pRow = s2Cursor.NextRow
    '                        Loop
    '                    End If
    '                    'Clear out variables to be re-used
    '                    inputGds = Nothing
    '                    extractGds = Nothing
    '                    pTable = Nothing
    '                    'Calculate area in meters (and feet)
    '                    Dim s2RepAreaSqKm As Double
    '                    Dim s2NonRepAreaSqKm As Double
    '                    If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
    '                        s2RepAreaSqKm = aStats.S2RepArea / 1000000
    '                        aStats.S2RepAreaSqKm = Format(Math.Round(s2RepAreaSqKm, 3), "#0.000")
    '                        s2NonRepAreaSqKm = aStats.S2NonRepArea / 1000000
    '                        aStats.S2NonRepAreaSqKm = Format(Math.Round(s2NonRepAreaSqKm, 3), "#0.000")
    '                        Dim s2RepAreaSqMiles As Double = s2RepAreaSqKm * BA_SQKm_To_SQMile
    '                        aStats.S2RepAreaSqMi = Format(Math.Round(s2RepAreaSqMiles, 3), "#0.000")
    '                        Dim s2NonRepAreaSqMiles As Double = s2NonRepAreaSqKm * BA_SQKm_To_SQMile
    '                        aStats.S2NonRepAreaSqMi = Format(Math.Round(s2NonRepAreaSqMiles, 3), "#0.000")
    '                        Dim s2RepAreaHectares As Double = s2RepAreaSqKm * BA_SQKm_To_HECTARE
    '                        aStats.S2RepAreaHect = Format(Math.Round(s2RepAreaHectares, 2), "#0.00")
    '                        Dim s2NonRepAreaHectares As Double = s2NonRepAreaSqKm * BA_SQKm_To_HECTARE
    '                        aStats.S2NonRepAreaHect = Format(Math.Round(s2NonRepAreaHectares, 2), "#0.00")
    '                        Dim s2RepAcres As Double = s2RepAreaSqKm * BA_SQKm_To_ACRE
    '                        aStats.S2RepAreaAcres = Format(Math.Round(s2RepAcres, 2), "#0.00")
    '                        Dim s2NonRepAcres As Double = s2NonRepAreaSqKm * BA_SQKm_To_ACRE
    '                        aStats.S2NonRepAreaAcres = Format(Math.Round(s2NonRepAcres, 2), "#0.00")
    '                    ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
    '                        Dim s2RepSqMiles As Double = aStats.S2RepArea / BA_SQFeet_To_SQMile
    '                        aStats.S2RepAreaSqMi = Format(Math.Round(s2RepSqMiles, 3), "#0.000")
    '                        Dim s2NonRepSqMiles As Double = aStats.S2NonRepArea / BA_SQFeet_To_SQMile
    '                        aStats.S2NonRepAreaSqMi = Format(Math.Round(s2NonRepSqMiles, 3), "#0.000")
    '                        s2RepAreaSqKm = s2RepSqMiles * BA_SQMile_To_SQKm
    '                        aStats.S2RepAreaSqKm = Format(Math.Round(s2RepAreaSqKm, 3), "#0.000")
    '                        s2NonRepAreaSqKm = s2NonRepSqMiles * BA_SQMile_To_SQKm
    '                        aStats.S2NonRepAreaSqKm = Format(Math.Round(s2NonRepAreaSqKm, 3), "#0.000")
    '                        Dim s2RepAreaHectares As Double = s2RepAreaSqKm * BA_SQKm_To_HECTARE
    '                        aStats.S2RepAreaHect = Format(Math.Round(s2RepAreaHectares, 2), "#0.00")
    '                        Dim s2NonRepAreaHectares As Double = s2NonRepAreaSqKm * BA_SQKm_To_HECTARE
    '                        aStats.S2NonRepAreaHect = Format(Math.Round(s2NonRepAreaHectares, 2), "#0.00")
    '                        Dim s2RepAcres As Double = s2RepAreaSqKm * BA_SQKm_To_ACRE
    '                        aStats.S2RepAreaAcres = Format(Math.Round(s2RepAcres, 2), "#0.00")
    '                        Dim s2NonRepAcres As Double = s2NonRepAreaSqKm * BA_SQKm_To_ACRE
    '                        aStats.S2NonRepAreaAcres = Format(Math.Round(s2NonRepAcres, 2), "#0.00")
    '                    End If
    '                    'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
    '                    Dim s2AoiPctRep As Double = s2RepAreaSqKm / AOI_ShapeArea * 100
    '                    aStats.S2AoiPctRep = Format(Math.Round(s2AoiPctRep, 2), "#0.00")
    '                    Dim s2AoiPctNonRep As Double = s2NonRepAreaSqKm / AOI_ShapeArea * 100
    '                    aStats.S2AoiPctNonRep = Format(Math.Round(s2AoiPctNonRep, 2), "#0.00")
    '                End If
    '                inputFile = BA_EnumDescription(MapsFileName.DifferenceRepresentedArea)
    '                inputGds = BA_OpenRasterFromGDB(inputFolder, inputFile)
    '                If inputGds IsNot Nothing Then
    '                    pStepProg.Message = "Calculate difference values between scenarios"
    '                    pStepProg.Step()
    '                    System.Windows.Forms.Application.DoEvents()
    '                    'It seems we have to extract the area before running the zonalOp because ArcObjects ignores the mask variable
    '                    'We are accessing the scenario difference raster
    '                    extractGds = extractionOp.Raster(inputGds, maskGds)
    '                    pTable = zonalOp.ZonalGeometryAsTable(extractGds)
    '                    idxArea = pTable.FindField(BA_FIELD_AREA)
    '                    idxValue = pTable.FindField(BA_FIELD_VALUE)
    '                    If idxArea > -1 And idxValue > -1 Then
    '                        diffCursor = pTable.Search(Nothing, False)
    '                        pRow = diffCursor.NextRow
    '                        Do Until pRow Is Nothing
    '                            Dim pValue As Integer = Convert.ToInt16(pRow.Value(idxValue))
    '                            Select Case pValue
    '                                Case BA_ValRepresented
    '                                    '1-Represented only in scenario 1
    '                                    aStats.MapRepS1Only = Convert.ToDouble(pRow.Value(idxArea))
    '                                Case BA_ValNonRepresentedBelow
    '                                    '2-Not represented in both scenarios
    '                                    aStats.MapNotRep = Convert.ToDouble(pRow.Value(idxArea))
    '                                Case BA_ValNonRepresentedAbove
    '                                    '3-Represented in both scenarios
    '                                    aStats.MapRepBothScen = Convert.ToDouble(pRow.Value(idxArea))
    '                                Case (BA_ValNonRepresented)
    '                                    '4-Represented only in scenario 2
    '                                    aStats.MapRepS2Only = Convert.ToDouble(pRow.Value(idxArea))
    '                            End Select
    '                            pRow = diffCursor.NextRow
    '                        Loop
    '                        'Next we convert the area(s) from sq meters or sq feet to sq km; 
    '                        'Our future conversions will assume the values are stored in sq km
    '                        If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
    '                            aStats.MapRepS1Only = aStats.MapRepS1Only / 1000000
    '                            aStats.MapRepS2Only = aStats.MapRepS2Only / 1000000
    '                            aStats.MapNotRep = aStats.MapNotRep / 1000000
    '                            aStats.MapRepBothScen = aStats.MapRepBothScen / 1000000
    '                        ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
    '                            Dim repS1OnlySqMi As Double = aStats.MapRepS1Only / BA_SQFeet_To_SQMile
    '                            aStats.MapRepS1Only = repS1OnlySqMi * BA_SQMile_To_SQKm
    '                            Dim repS2OnlySqMi As Double = aStats.MapRepS2Only / BA_SQFeet_To_SQMile
    '                            aStats.MapRepS2Only = repS2OnlySqMi * BA_SQMile_To_SQKm
    '                            Dim diffNotRepSqMi As Double = aStats.MapNotRep / BA_SQFeet_To_SQMile
    '                            aStats.MapNotRep = diffNotRepSqMi * BA_SQMile_To_SQKm
    '                            Dim repBothScenSqMi As Double = aStats.MapRepBothScen / BA_SQFeet_To_SQMile
    '                            aStats.MapRepBothScen = repBothScenSqMi * BA_SQMile_To_SQKm
    '                        End If
    '                        'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
    '                        Dim mapAoiPctNotRep As Double = aStats.MapNotRep / AOI_ShapeArea * 100
    '                        aStats.MapAoiPctNotRep = Format(Math.Round(mapAoiPctNotRep, 2), "#0.00")
    '                        Dim mapAoiRepBothScen As Double = aStats.MapRepBothScen / AOI_ShapeArea * 100
    '                        aStats.MapAoiRepBothScen = Format(Math.Round(mapAoiRepBothScen, 2), "#0.00")
    '                        Dim mapRepS1Only As Double = aStats.MapRepS1Only / AOI_ShapeArea * 100
    '                        aStats.MapAoiRepS1Only = Format(Math.Round(mapRepS1Only, 2), "#0.00")
    '                        Dim mapRepS2Only As Double = aStats.MapRepS2Only / AOI_ShapeArea * 100
    '                        aStats.MapAoiRepS2Only = Format(Math.Round(mapRepS2Only, 2), "#0.00")
    '                    End If
    '                End If
    '            End If
    '        End If
    '        Return aStats
    '    Catch ex As Exception
    '        Debug.Print("BA_CalculateScenario1Represented Exception: " & ex.Message)
    '        Return Nothing
    '    Finally
    '        zonalOp = Nothing
    '        inputGds = Nothing
    '        pTable = Nothing
    '        s1Cursor = Nothing
    '        s2Cursor = Nothing
    '        diffCursor = Nothing
    '    End Try
    'End Function

    Public Function BA_CalculateAreaStatistics(ByVal elevUnits As ESRI.ArcGIS.esriSystem.esriUnits, _
                                           ByVal countScenario2Sites As Integer, ByVal pStepProg As IStepProgressor) As AreaStatistics
        Dim aStats As AreaStatistics = New AreaStatistics
        Try
            Dim inputFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            Dim inputFile As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
            pStepProg.Message = "Calculate area totals for scenario 1"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()
            aStats.S1RepArea = BA_CalculateTotalPolygonArea(inputFolder, inputFile)
            'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
            aStats.S1NonRepArea = (AOI_ShapeArea * 1000000) - aStats.S1RepArea
            'Calculate area in meters (and feet)
            Dim s1RepAreaSqKm As Double
            Dim s1NonRepAreaSqKm As Double
            If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
                s1RepAreaSqKm = aStats.S1RepArea / 1000000
                aStats.S1RepAreaSqKm = Format(Math.Round(s1RepAreaSqKm, 3), "#0.000")
                s1NonRepAreaSqKm = aStats.S1NonRepArea / 1000000
                aStats.S1NonRepAreaSqKm = Format(Math.Round(s1NonRepAreaSqKm, 3), "#0.000")
                Dim s1RepAreaSqMiles As Double = s1RepAreaSqKm * BA_SQKm_To_SQMile
                aStats.S1RepAreaSqMi = Format(Math.Round(s1RepAreaSqMiles, 3), "#0.000")
                Dim s1NonRepAreaSqMiles As Double = s1NonRepAreaSqKm * BA_SQKm_To_SQMile
                aStats.S1NonRepAreaSqMi = Format(Math.Round(s1NonRepAreaSqMiles, 3), "#0.000")
                Dim s1RepAreaHectares As Double = s1RepAreaSqKm * BA_SQKm_To_HECTARE
                aStats.S1RepAreaHect = Format(Math.Round(s1RepAreaHectares, 2), "#0.00")
                Dim s1NonRepAreaHectares As Double = s1NonRepAreaSqKm * BA_SQKm_To_HECTARE
                aStats.S1NonRepAreaHect = Format(Math.Round(s1NonRepAreaHectares, 2), "#0.00")
                Dim s1RepAcres As Double = s1RepAreaSqKm * BA_SQKm_To_ACRE
                aStats.S1RepAreaAcres = Format(Math.Round(s1RepAcres, 2), "#0.00")
                Dim s1NonRepAcres As Double = s1NonRepAreaSqKm * BA_SQKm_To_ACRE
                aStats.S1NonRepAreaAcres = Format(Math.Round(s1NonRepAcres, 2), "#0.00")
            ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
                Dim s1RepSqMiles As Double = aStats.S1RepArea / BA_SQFeet_To_SQMile
                aStats.S1RepAreaSqMi = Format(Math.Round(s1RepSqMiles, 3), "#0.000")
                Dim s1NonRepSqMiles As Double = aStats.S1NonRepArea / BA_SQFeet_To_SQMile
                aStats.S1NonRepAreaSqMi = Format(Math.Round(s1NonRepSqMiles, 3), "#0.000")
                s1RepAreaSqKm = s1RepSqMiles * BA_SQMile_To_SQKm
                aStats.S1RepAreaSqKm = Format(Math.Round(s1RepAreaSqKm, 3), "#0.000")
                s1NonRepAreaSqKm = s1NonRepSqMiles * BA_SQMile_To_SQKm
                aStats.S1NonRepAreaSqKm = Format(Math.Round(s1NonRepAreaSqKm, 3), "#0.000")
                Dim s1RepAreaHectares As Double = s1RepAreaSqKm * BA_SQKm_To_HECTARE
                aStats.S1RepAreaHect = Format(Math.Round(s1RepAreaHectares, 2), "#0.00")
                Dim s1NonRepAreaHectares As Double = s1NonRepAreaSqKm * BA_SQKm_To_HECTARE
                aStats.S1NonRepAreaHect = Format(Math.Round(s1NonRepAreaHectares, 2), "#0.00")
                Dim s1RepAcres As Double = s1RepAreaSqKm * BA_SQKm_To_ACRE
                aStats.S1RepAreaAcres = Format(Math.Round(s1RepAcres, 2), "#0.00")
                Dim s1NonRepAcres As Double = s1NonRepAreaSqKm * BA_SQKm_To_ACRE
                aStats.S1NonRepAreaAcres = Format(Math.Round(s1NonRepAcres, 2), "#0.00")
            End If
            'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
            Dim s1AoiPctRep As Double = s1RepAreaSqKm / AOI_ShapeArea * 100
            aStats.S1AoiPctRep = Format(Math.Round(s1AoiPctRep, 2), "#0.00")
            Dim s1AoiPctNonRep As Double = s1NonRepAreaSqKm / AOI_ShapeArea * 100
            aStats.S1AoiPctNonRep = Format(Math.Round(s1AoiPctNonRep, 2), "#0.00")
            'Run the calculations for scenario 2 if applicable
            If countScenario2Sites > 0 Then
                pStepProg.Message = "Calculate area totals for scenario 2"
                pStepProg.Step()
                System.Windows.Forms.Application.DoEvents()
                inputFile = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
                aStats.S2RepArea = BA_CalculateTotalPolygonArea(inputFolder, inputFile)
                'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
                aStats.S2NonRepArea = (AOI_ShapeArea * 1000000) - aStats.S2RepArea
                'Calculate area in meters (and feet)
                Dim s2RepAreaSqKm As Double
                Dim s2NonRepAreaSqKm As Double
                If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
                    s2RepAreaSqKm = aStats.S2RepArea / 1000000
                    aStats.S2RepAreaSqKm = Format(Math.Round(s2RepAreaSqKm, 3), "#0.000")
                    s2NonRepAreaSqKm = aStats.S2NonRepArea / 1000000
                    aStats.S2NonRepAreaSqKm = Format(Math.Round(s2NonRepAreaSqKm, 3), "#0.000")
                    Dim s2RepAreaSqMiles As Double = s2RepAreaSqKm * BA_SQKm_To_SQMile
                    aStats.S2RepAreaSqMi = Format(Math.Round(s2RepAreaSqMiles, 3), "#0.000")
                    Dim s2NonRepAreaSqMiles As Double = s2NonRepAreaSqKm * BA_SQKm_To_SQMile
                    aStats.S2NonRepAreaSqMi = Format(Math.Round(s2NonRepAreaSqMiles, 3), "#0.000")
                    Dim s2RepAreaHectares As Double = s2RepAreaSqKm * BA_SQKm_To_HECTARE
                    aStats.S2RepAreaHect = Format(Math.Round(s2RepAreaHectares, 2), "#0.00")
                    Dim s2NonRepAreaHectares As Double = s2NonRepAreaSqKm * BA_SQKm_To_HECTARE
                    aStats.S2NonRepAreaHect = Format(Math.Round(s2NonRepAreaHectares, 2), "#0.00")
                    Dim s2RepAcres As Double = s2RepAreaSqKm * BA_SQKm_To_ACRE
                    aStats.S2RepAreaAcres = Format(Math.Round(s2RepAcres, 2), "#0.00")
                    Dim s2NonRepAcres As Double = s2NonRepAreaSqKm * BA_SQKm_To_ACRE
                    aStats.S2NonRepAreaAcres = Format(Math.Round(s2NonRepAcres, 2), "#0.00")
                ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
                    Dim s2RepSqMiles As Double = aStats.S2RepArea / BA_SQFeet_To_SQMile
                    aStats.S2RepAreaSqMi = Format(Math.Round(s2RepSqMiles, 3), "#0.000")
                    Dim s2NonRepSqMiles As Double = aStats.S2NonRepArea / BA_SQFeet_To_SQMile
                    aStats.S2NonRepAreaSqMi = Format(Math.Round(s2NonRepSqMiles, 3), "#0.000")
                    s2RepAreaSqKm = s2RepSqMiles * BA_SQMile_To_SQKm
                    aStats.S2RepAreaSqKm = Format(Math.Round(s2RepAreaSqKm, 3), "#0.000")
                    s2NonRepAreaSqKm = s2NonRepSqMiles * BA_SQMile_To_SQKm
                    aStats.S2NonRepAreaSqKm = Format(Math.Round(s2NonRepAreaSqKm, 3), "#0.000")
                    Dim s2RepAreaHectares As Double = s2RepAreaSqKm * BA_SQKm_To_HECTARE
                    aStats.S2RepAreaHect = Format(Math.Round(s2RepAreaHectares, 2), "#0.00")
                    Dim s2NonRepAreaHectares As Double = s2NonRepAreaSqKm * BA_SQKm_To_HECTARE
                    aStats.S2NonRepAreaHect = Format(Math.Round(s2NonRepAreaHectares, 2), "#0.00")
                    Dim s2RepAcres As Double = s2RepAreaSqKm * BA_SQKm_To_ACRE
                    aStats.S2RepAreaAcres = Format(Math.Round(s2RepAcres, 2), "#0.00")
                    Dim s2NonRepAcres As Double = s2NonRepAreaSqKm * BA_SQKm_To_ACRE
                    aStats.S2NonRepAreaAcres = Format(Math.Round(s2NonRepAcres, 2), "#0.00")
                End If
                'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
                Dim s2AoiPctRep As Double = s2RepAreaSqKm / AOI_ShapeArea * 100
                aStats.S2AoiPctRep = Format(Math.Round(s2AoiPctRep, 2), "#0.00")
                Dim s2AoiPctNonRep As Double = s2NonRepAreaSqKm / AOI_ShapeArea * 100
                aStats.S2AoiPctNonRep = Format(Math.Round(s2AoiPctNonRep, 2), "#0.00")
                Dim s1Only As String = "s1_s2"
                Dim s2Only As String = "s2_s1"

                Dim success As BA_ReturnCode = BA_Erase(inputFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea), _
                                                        inputFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea), _
                                                        AOIFolderBase & "\" & s1Only)
                If success = BA_ReturnCode.Success Then
                    aStats.MapRepS1Only = BA_CalculateTotalPolygonArea(AOIFolderBase, s1Only)
                End If
                BA_Remove_Shapefile(AOIFolderBase, s1Only)

                success = BA_Erase(inputFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea), _
                                                        inputFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea), _
                                                        AOIFolderBase & "\" & s2Only)
                If success = BA_ReturnCode.Success Then
                    aStats.MapRepS2Only = BA_CalculateTotalPolygonArea(AOIFolderBase, s2Only)
                End If
                BA_Remove_Shapefile(AOIFolderBase, s2Only)

                aStats.MapRepBothScen = BA_CalculateTotalPolygonArea(inputFolder, BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
                aStats.MapNotRep = AOI_ShapeArea * 1000000 - aStats.MapRepBothScen

                'Next we convert the area(s) from sq meters or sq feet to sq km; 
                'Our future conversions will assume the values are stored in sq km
                If elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
                    aStats.MapRepS1Only = aStats.MapRepS1Only / 1000000
                    aStats.MapRepS2Only = aStats.MapRepS2Only / 1000000
                    aStats.MapNotRep = aStats.MapNotRep / 1000000
                    aStats.MapRepBothScen = aStats.MapRepBothScen / 1000000
                ElseIf elevUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
                    Dim repS1OnlySqMi As Double = aStats.MapRepS1Only / BA_SQFeet_To_SQMile
                    aStats.MapRepS1Only = repS1OnlySqMi * BA_SQMile_To_SQKm
                    Dim repS2OnlySqMi As Double = aStats.MapRepS2Only / BA_SQFeet_To_SQMile
                    aStats.MapRepS2Only = repS2OnlySqMi * BA_SQMile_To_SQKm
                    Dim diffNotRepSqMi As Double = aStats.MapNotRep / BA_SQFeet_To_SQMile
                    aStats.MapNotRep = diffNotRepSqMi * BA_SQMile_To_SQKm
                    Dim repBothScenSqMi As Double = aStats.MapRepBothScen / BA_SQFeet_To_SQMile
                    aStats.MapRepBothScen = repBothScenSqMi * BA_SQMile_To_SQKm
                End If
                'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
                Dim mapAoiPctNotRep As Double = aStats.MapNotRep / AOI_ShapeArea * 100
                aStats.MapAoiPctNotRep = Format(Math.Round(mapAoiPctNotRep, 2), "#0.00")
                Dim mapAoiRepBothScen As Double = aStats.MapRepBothScen / AOI_ShapeArea * 100
                aStats.MapAoiRepBothScen = Format(Math.Round(mapAoiRepBothScen, 2), "#0.00")
                Dim mapRepS1Only As Double = aStats.MapRepS1Only / AOI_ShapeArea * 100
                aStats.MapAoiRepS1Only = Format(Math.Round(mapRepS1Only, 2), "#0.00")
                Dim mapRepS2Only As Double = aStats.MapRepS2Only / AOI_ShapeArea * 100
                aStats.MapAoiRepS2Only = Format(Math.Round(mapRepS2Only, 2), "#0.00")
            End If
            Return aStats
        Catch ex As Exception
            Debug.Print("BA_CalculateAreaStatistics Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function BA_CalculateTotalPolygonArea(ByVal inputFolder As String, ByVal inputFile As String) As Double
        Dim inputGds As IGeoDataset = Nothing
        Dim inputFc As IFeatureClass = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim aFeature As IFeature = Nothing
        Dim aGeo As IGeometry = Nothing
        Dim aPoly As IPolygon = Nothing
        Dim aArea As IArea = Nothing
        Dim dblArea As Double = 0
        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFolder)
            If wType = WorkspaceType.Geodatabase Then
                inputGds = BA_OpenFeatureClassFromGDB(inputFolder, inputFile)
            Else
                inputGds = BA_OpenFeatureClassFromFile(inputFolder, inputFile)
            End If
            If inputGds IsNot Nothing Then
                inputFc = CType(inputGds, FeatureDataset)
                fCursor = inputFc.Search(Nothing, False)
                aFeature = fCursor.NextFeature
                Do While aFeature IsNot Nothing
                    aGeo = aFeature.Shape
                    aPoly = CType(aGeo, IPolygon)
                    aArea = CType(aPoly, IArea)
                    dblArea = dblArea + aArea.Area
                    aFeature = fCursor.NextFeature
                Loop
            End If
            Return dblArea
        Catch ex As Exception
            Debug.Print("BA_CalculateTotalPolygonArea Exception: " & ex.Message)
            Return -1
        Finally
            inputGds = Nothing
            inputFc = Nothing
            fCursor = Nothing
            aGeo = Nothing
            aPoly = Nothing
            aArea = Nothing
        End Try
    End Function

    Public Function BA_RemoveFilesBySuffix(ByVal workspaceName As String, ByVal tempSuffix As String) As BA_ReturnCode
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pRasterWs As IRasterWorkspaceEx = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Dim pDataset As IDataset = Nothing

        Try
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' Delete rasters
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                Dim startingPos As Integer = pName.Length - tempSuffix.Length
                If startingPos > 0 AndAlso pName.Substring(startingPos) = tempSuffix Then
                    pRasterWs = CType(pWorkspace, IRasterWorkspaceEx)
                    pDataset = pRasterWs.OpenRasterDataset(pName)
                    pDataset.Delete()
                End If
                pDSName = pEnumDSName.Next
            End While

            'Delete vectors
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureDataset)
            pDSName = pEnumDSName.Next
            pName = Nothing
            While Not pDSName Is Nothing
                Dim startingPos As Integer = pName.Length - tempSuffix.Length
                If startingPos > 0 AndAlso pName.Substring(startingPos) = tempSuffix Then
                    pFeatWSpace = CType(pWorkspace, IFeatureWorkspace)
                    pDataset = pFeatWSpace.OpenFeatureDataset(pName)
                    pDataset.Delete()
                End If
                pDSName = pEnumDSName.Next
            End While

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_RemoveFilesBySuffix & Exception: " & ex.Message)
            Return BA_ReturnCode.OtherError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatWSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspace)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    Public Function BA_BufferPoints(ByVal inputFolder As String, ByVal inputFile As String, ByVal outputFolder As String, _
                                    ByVal outputFile As String, ByVal queryString As String, _
                                    ByVal pSRef As ISpatialReference, ByVal pGraphicsContainer As IGraphicsContainer, _
                                    ByVal distance As Double) As BA_ReturnCode
        Dim pFClass As IFeatureClass = Nothing
        Dim pFCursor As IFeatureCursor = Nothing
        Dim pQueryFilter As IQueryFilter = New QueryFilter
        Dim pFCBuffer As IFeatureCursorBuffer2 = New FeatureCursorBuffer
        Dim enumGeo As IEnumGeometry = Nothing
        Dim pGeo As IGeometry = Nothing
        ' Create a name object for the target workspace and open it.
        Dim targetWorkspaceName As IWorkspaceName = Nothing
        Dim targetWorkspaceIName As ESRI.ArcGIS.esriSystem.IName = Nothing
        Dim targetWorkspace As IWorkspace = Nothing
        ' Create a name object for the target dataset.
        Dim targetFeatureClassName As IFeatureClassName = New FeatureClassNameClass()
        Dim targetDatasetName As IDatasetName = Nothing
        Try
            pFClass = BA_OpenFeatureClassFromGDB(inputFolder, inputFile)
            If pFClass IsNot Nothing Then
                pQueryFilter.WhereClause = queryString
                pFCursor = pFClass.Search(pQueryFilter, False)
                With pFCBuffer
                    .ValueDistance = distance
                    .FeatureCursor = pFCursor
                    .Dissolve = True
                    .BufferSpatialReference = pSRef
                    .DataFrameSpatialReference = pSRef
                    .SourceSpatialReference = pSRef
                    .TargetSpatialReference = pSRef
                End With
                targetWorkspaceName = New WorkspaceNameClass With _
                                                    {.WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory", _
                                                    .PathName = outputFolder}
                targetWorkspaceIName = CType(targetWorkspaceName, ESRI.ArcGIS.esriSystem.IName)
                targetWorkspace = CType(targetWorkspaceIName.Open(), IWorkspace)
                targetDatasetName = CType(targetFeatureClassName, IDatasetName)
                targetDatasetName.Name = outputFile
                targetDatasetName.WorkspaceName = targetWorkspaceName
                pFCBuffer.Buffer(targetFeatureClassName)

                'enumGeo = pFCBuffer.BufferedGeometry()
                'enumGeo.Reset()
                'pGeo = enumGeo.Next
                'Do While pGeo IsNot Nothing
                '    Dim simpleFillSymbol As ESRI.ArcGIS.Display.ISimpleFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()
                '    Dim pColor As IColor = New RgbColor
                '    pColor.RGB = RGB(0, 0, 255)
                '    simpleFillSymbol.Color = pColor
                '    simpleFillSymbol.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal
                '    Dim fillShapeElement As ESRI.ArcGIS.Carto.IFillShapeElement = New ESRI.ArcGIS.Carto.PolygonElementClass()
                '    fillShapeElement.Symbol = simpleFillSymbol
                '    Dim element As IElement = CType(fillShapeElement, IElement) ' Explicit Cast
                '    element.Geometry = pGeo
                '    pGraphicsContainer.AddElement(element, 0)
                '    pGeo = enumGeo.Next
                'Loop
            End If
        Catch ex As Exception
            Debug.Print("BA_BufferPoints Exception: " & ex.Message)
        Finally
            pFClass = Nothing
            pFCursor = Nothing
        End Try
    End Function

    Public Function BA_QueryFeatureClassExtent(ByVal folderPath As String, ByVal fileName As String) As String
        Dim bufferGds As IGeoDataset = Nothing
        Dim env As IEnvelope = Nothing
        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(folderPath)
            fileName = BA_StandardizeShapefileName(fileName, False)
            'Get processing extent from buffer
            If wType = WorkspaceType.Raster Then
                bufferGds = BA_OpenFeatureClassFromFile(folderPath, fileName)
            Else
                bufferGds = BA_OpenFeatureClassFromGDB(folderPath, fileName)
            End If
            env = bufferGds.Extent
            Dim urx, llx, lly, ury As Double
            env.QueryCoords(llx, lly, urx, ury)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append(llx & ", ")
            sb.Append(lly & ", ")
            sb.Append(urx & ", ")
            sb.Append(ury)
            Return sb.ToString
        Catch ex As Exception
            Debug.Print("BA_QueryFeatureClassExtent Exception: " & ex.Message)
            Return Nothing
        Finally
            bufferGds = Nothing
            env = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_ExtractByFeatureClass(ByVal inputFolder As String, ByVal inputFile As String, _
                                             ByVal rasterFolder As String, ByVal rasterFile As String, _
                                             ByVal outputFolder As String, ByVal outputFile As String) As BA_ReturnCode
        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(outputFolder)
        If wType = WorkspaceType.Geodatabase Then
            MessageBox.Show("BA_ExtractByFeatureClass does not work with file geodatabases as output!", "No FGDB", MessageBoxButtons.OK)
            Exit Function
        End If
        Dim exOp As IExtractionOp2 = New RasterExtractionOp
        Dim bufferFC As IFeatureClass = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim rasterGdb As IGeoDataset = Nothing
        Dim outputGdb As IGeoDataset = Nothing
        Dim pEnv As IRasterAnalysisEnvironment = CType(exOp, IRasterAnalysisEnvironment)
        Try
            inputFile = BA_StandardizeShapefileName(inputFile, False)
            wType = BA_GetWorkspaceTypeFromPath(inputFolder)
            If wType = WorkspaceType.Raster Then
                bufferFC = CType(BA_OpenFeatureClassFromFile(inputFolder, inputFile), IFeatureClass)
            Else
                bufferFC = CType(BA_OpenFeatureClassFromGDB(inputFolder, inputFile), IFeatureClass)
            End If
            wType = BA_GetWorkspaceTypeFromPath(rasterFolder)
            If wType = WorkspaceType.Geodatabase Then
                rasterGdb = BA_OpenRasterFromGDB(rasterFolder, rasterFile)
            Else
                rasterGdb = BA_OpenRasterFromFile(rasterFolder, rasterFile)
            End If
            If bufferFC IsNot Nothing AndAlso rasterGdb IsNot Nothing Then
                pCursor = bufferFC.Search(Nothing, False)
                pFeature = pCursor.NextFeature
                Dim pGeo As IGeometry = pFeature.Shape
                Dim pPoly As IPolygon = CType(pGeo, IPolygon)
                pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, pGeo.Envelope)
                outputGdb = exOp.Polygon(rasterGdb, pPoly, True)
                If outputGdb IsNot Nothing Then
                    If BA_File_ExistsRaster(outputFolder, outputFile) Then
                        BA_Remove_Raster(outputFolder, outputFile)
                    End If
                    Dim retVal As Integer = BA_SaveRasterDataset(outputGdb, outputFolder, outputFile)
                    If retVal = 1 Then
                        Return BA_ReturnCode.Success
                    End If
                End If
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_ExtractByFeatureClass Exception: " & ex.Message)
            Return Nothing
        Finally
            exOp = Nothing
            pCursor = Nothing
            pFeature = Nothing
            rasterGdb = Nothing
            outputGdb = Nothing
            pEnv = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_MapDisplayPolygon(ByVal pMxDoc As IMxDocument, ByVal filenamepath As String, _
                                         ByVal layername As String, ByVal PolyColor As IRgbColor, ByVal Transparency As Integer) As BA_ReturnCode
        Dim File_Path As String = "PleaseReturn"
        Dim File_Name As String

        File_Name = BA_GetBareName(filenamepath, File_Path)

        'exit if file_name is null
        If String.IsNullOrEmpty(File_Name) Then
            Return BA_ReturnCode.ReadError
        End If

        If BA_GetWorkspaceTypeFromPath(filenamepath) <> WorkspaceType.Geodatabase Then
            Return BA_ReturnCode.NotSupportedOperation
        End If

        Dim pFeatClass As IFeatureClass
        Dim pFLayer As IFeatureLayer = New FeatureLayer
        Dim pMap As IMap
        Dim pPolySym As ISimpleFillSymbol = New SimpleFillSymbol
        Dim pGFLayer As IGeoFeatureLayer
        Dim pRenderer As ISimpleRenderer = New SimpleRenderer
        Dim pLineSym As ILineSymbol = New SimpleLineSymbol
        Dim pEnv As IEnvelope
        Dim pTempLayer As ILayer

        Try
            pFeatClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)

            'add featureclass to current data frame
            pFLayer.FeatureClass = pFeatClass

            'check feature geometry type, only polygon layers can be used as an extent layer
            If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPolygon Then
                Return BA_ReturnCode.NotSupportedOperation
            End If

            'set layer name
            If String.IsNullOrEmpty(layername) Then
                pFLayer.Name = pFLayer.FeatureClass.AliasName
            Else
                pFLayer.Name = layername
            End If

            'add layer
            pMap = pMxDoc.FocusMap

            'set layer symbology - hollow
            'pLineSym.Color = PolyColor
            pLineSym.Width = 0
            pPolySym.Color = PolyColor
            pPolySym.Outline = pLineSym
            pPolySym.Style = esriSimpleFillStyle.esriSFSSolid
            pRenderer.Symbol = pPolySym

            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer

            If Transparency > 0 Then
                Dim pLayerEffects As ILayerEffects = CType(pGFLayer, ILayerEffects)
                pLayerEffects.Transparency = Transparency
            End If

            'check if a layer with the assigned name exists
            'search layer of the specified name, if found
            Dim nlayers As Long
            Dim i As Long
            nlayers = pMap.LayerCount
            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If layername = pTempLayer.Name Then 'remove the layer
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next

            pMap.AddLayer(pFLayer)

            'refresh the active view
            'pMxDoc.ActivatedView.Refresh()
            'pMxDoc.UpdateContents()

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_MapDisplayPolygon Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeatClass = Nothing
            pFLayer = Nothing
            pGFLayer = Nothing
            pEnv = Nothing
            pTempLayer = Nothing
            pRenderer = Nothing
            pMap = Nothing
            pPolySym = Nothing
            pLineSym = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    Public Sub BA_MoveScenarioLayers()
        Dim idxNew As Integer
        Dim pMap As IMap = My.Document.FocusMap
        Dim pTempLayer As ILayer
        ' We want to move the scenario layers directly on top of the streams layer
        For i = pMap.LayerCount To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If BA_MAPS_BOTH_REPRESENTATION = pTempLayer.Name Then
                idxNew = i - 1
                Exit For
            End If
        Next
        For i = 0 To pMap.LayerCount - 1
            pTempLayer = pMap.Layer(i)
            If BA_MAPS_SCENARIO1_REPRESENTATION = pTempLayer.Name Then 'move the layer
                pMap.MoveLayer(pTempLayer, idxNew)
                Exit For
            End If
        Next
        For i = 0 To pMap.LayerCount - 1
            pTempLayer = pMap.Layer(i)
            If BA_MAPS_SCENARIO2_REPRESENTATION = pTempLayer.Name Then 'move the layer
                pMap.MoveLayer(pTempLayer, idxNew)
                Exit For
            End If
        Next
    End Sub

    'map type:
    '1. Elevation Distribution
    '2. Elevation SNOTEL
    '3. Elevation Snow Course
    '4. Precipitation Distribution
    '5. Aspect Distribution
    Public Function BA_DisplayMap(ByVal pMxDocument As IMxDocument, ByVal maptype As Integer, ByVal selectedBasin As String, ByVal selectedAoi As String, _
                                  ByVal displayElevationInMeters As Boolean, ByVal subTitle As String) As Integer
        Dim LayerNames() As String
        Dim response As Integer
        Dim maptitle As String
        Dim KeyLayerName As String = Nothing
        Dim UnitText As String
        'Participated layers
        'Public Const BA_MapAOI = "AOI Boundary"
        'Public Const BA_MapElevationZone = "Elevation Zones"
        'Public Const BA_MapSNOTELZone = "SNOTEL Elevation Zones"
        'Public Const BA_MapSNOTELSite = "SNOTEL Sites"
        'Public Const BA_MapSnowCourseZone = "Snow Course Elevation Zones"
        'Public Const BA_MapSnowCourseSite = "Snow Courses"
        'Public Const BA_MapPrecipZone = "Precipitation Zones"
        'Public Const BA_MapHillshade = "Hillshade"
        'Public Const BA_MapStream = "AOI Streams"
        'Public Const BA_MapAspect = "Aspect"
        'Public Const BA_MapSlope = "Slope"
        Dim Basin_Name As String

        If Len(Trim(selectedBasin)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = vbCrLf & " at " & selectedBasin
        End If

        maptitle = selectedAoi & Basin_Name

        Select Case maptype
            Case 1 'elevation distribution
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_ELEVATION_ZONES
                If displayElevationInMeters = True Then
                    UnitText = "Elevation Units = Meters"
                Else
                    UnitText = "Elevation Units = Feet"
                End If
            Case 2 'elevation snotel
                ReDim LayerNames(0 To 5)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOTEL_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                KeyLayerName = BA_MAPS_SNOTEL_ZONES
                UnitText = "Area Represented = elevation zone" & vbCrLf & "between named site and next site below"
            Case 3 'elevation snow course
                ReDim LayerNames(0 To 5)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOW_COURSE_ZONES
                LayerNames(5) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_SNOW_COURSE_ZONES
                UnitText = "Area Represented = elevation zone" & vbCrLf & "between named site and next site below"
            Case 4 'precipitation distribution
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOTEL_SITES
                LayerNames(5) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(6) = BA_MAPS_PRECIPITATION_ZONES
                KeyLayerName = BA_MAPS_PRECIPITATION_ZONES
                UnitText = "Precipitation Units = Inches"
            Case 5 'aspect
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ASPECT
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_ASPECT
                UnitText = " "
            Case 6 'slope
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SLOPE
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_SLOPE
                UnitText = " "
            Case 7 'Scenario 1
                ReDim LayerNames(0 To 11)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO1
                LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO1
                LayerNames(8) = BA_MAPS_PSEUDO_SCENARIO1
                LayerNames(9) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(10) = BA_MAPS_PSEUDO_SITES
                LayerNames(11) = BA_MAPS_SCENARIO1_REPRESENTATION
                'KeyLayerName = BA_MAPS_SCENARIO1_REPRESENTATION
                UnitText = "Scenario 1 sites are circled in black"
            Case 8 'Scenario 2
                ReDim LayerNames(0 To 11)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO2
                LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO2
                LayerNames(8) = BA_MAPS_PSEUDO_SCENARIO2
                LayerNames(9) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(10) = BA_MAPS_PSEUDO_SITES
                LayerNames(11) = BA_MAPS_SCENARIO2_REPRESENTATION
                'KeyLayerName = BA_MAPS_SCENARIO2_REPRESENTATION
                UnitText = "Scenario 2 sites are circled in gold"
            Case 9 'Difference of Representations
                ReDim LayerNames(0 To 16)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO1
                LayerNames(7) = BA_MAPS_SNOTEL_SCENARIO2
                LayerNames(8) = BA_MAPS_SNOW_COURSE_SCENARIO1
                LayerNames(9) = BA_MAPS_SNOW_COURSE_SCENARIO2
                LayerNames(10) = BA_MAPS_PSEUDO_SCENARIO1
                LayerNames(11) = BA_MAPS_PSEUDO_SCENARIO2
                LayerNames(12) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(13) = BA_MAPS_PSEUDO_SITES
                LayerNames(14) = BA_MAPS_SCENARIO1_REPRESENTATION
                LayerNames(15) = BA_MAPS_SCENARIO2_REPRESENTATION
                LayerNames(16) = BA_MAPS_BOTH_REPRESENTATION
                KeyLayerName = Nothing
                UnitText = "Scenario 1 and scenario 2 sites are circled" & vbCrLf & "in black and gold respectively"
            Case Else
                MsgBox(maptype & " is not a valid option!")
                Return -1
        End Select
        'convert subtitle to uppercase
        BA_MapUpdateSubTitle(pMxDocument, maptitle, subTitle.ToUpper, UnitText)
        response = BA_ToggleLayersinMapFrame(pMxDocument, LayerNames)
        If response <> 1 Then
            'There was an error displaying the map; Disable the buttons
            'Site scenario maps that also use this function have problems prior to calling this function if some layers are missing
            'Their error handling is in BA_SiteScenarioValidMap
            Select Case maptype
                Case 1, 2, 3, 4, 5, 6
                    BA_Enable_MapFlags(False)
                    Dim errorMsg As String = "BAGIS is unable to display the selected map. Please reload the maps using the Generate Maps button."
                    MessageBox.Show(errorMsg, "Error loading map", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
        End If
        BA_ToggleView(pMxDocument, False) 'switch to the map layout view
        BA_SetLegendFormat(pMxDocument, KeyLayerName)
        Return response
    End Function

    Public Function BA_SiteScenarioValidMap(ByVal pMap As IMap) As Boolean
        Dim pTempLayer As ILayer
        Dim validMap As Boolean = True
        If pMap.LayerCount = 0 Then
            validMap = False
        Else
            Dim foundHillshade As Boolean = False
            For i = 0 To pMap.LayerCount - 1
                pTempLayer = pMap.Layer(i)
                If BA_MAPS_HILLSHADE = pTempLayer.Name Then 'hillshade is in map frame, we are likely good
                    foundHillshade = True
                    Exit For
                End If
            Next
            If Not foundHillshade Then
                validMap = False
            End If
        End If
        If validMap = False Then
            BA_Enable_ScenarioMapFlags(False)
            Dim errorMsg As String = "BAGIS is unable to display the selected map. Please reload the maps using the Maps button in the Site Scenarios tool."
            MessageBox.Show(errorMsg, "Error loading map", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Return validMap
    End Function

    Public Function BA_BuildRendererForPoints(ByVal markerColor As IColor, ByVal size As Integer) As ISimpleRenderer
        Dim pFillColor As IColor = New RgbColor
        'Dim pMSymbol As IMarkerSymbol = Nothing
        Dim pMSymbol As ISimpleMarkerSymbol = New SimpleMarkerSymbol
        Dim pMask As IMask = Nothing
        Dim pRenderer As ISimpleRenderer = New SimpleRenderer

        Try
            pMSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle
            pMSymbol.Size = size
            pMSymbol.OutlineColor = markerColor
            pMSymbol.Outline = True
            pMSymbol.OutlineSize = 3.0
            pFillColor.NullColor = True
            pMSymbol.Color = pFillColor
            pRenderer.Symbol = pMSymbol
            Return pRenderer
        Catch ex As Exception
            Debug.Print("BA_BuildRendererForPoints Exception: " & ex.Message)
            Return Nothing
        Finally
            pRenderer = Nothing
            pMSymbol = Nothing
        End Try
    End Function

    ' Populates an PseudoSite object from an XML file
    Public Function BA_LoadPseudoSitesFromXml(ByVal aoiPath As String) As PseudoSiteList
        Try
            Dim xmlInputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.PseudoSitesXml)
            If BA_File_ExistsWindowsIO(xmlInputPath) Then
                Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(PseudoSiteList))
                If obj IsNot Nothing Then
                    Dim analysis As PseudoSiteList = CType(obj, PseudoSiteList)
                    Return analysis
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Debug.Print("BA_LoadPseudoSitesFromXml Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function BA_FindElevPrecipRasterName(ByVal searchPrefix As String) As String
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Analysis)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
        Dim RasterCount As Integer = UBound(AOIRasterList)
        If RasterCount > 0 Then
            For i = 1 To RasterCount
                If AOIRasterList(i).IndexOf(searchPrefix) = 0 Then
                    Return AOIRasterList(i)
                End If
            Next
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BA_ReadMapSettings() As MapsSettings
        Dim retSettings As MapsSettings = Nothing
        Dim filePathName As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) +
            "\" + BA_MapParameterFile
        If BA_File_ExistsWindowsIO(filePathName) Then
            'This list corresponds to the values in frmGenerateMaps.CmboxElevInterval
            Dim lstElevationInterval As IList(Of String) =
                New List(Of String) From {"50", "100", "200", "250", "500", "1000", "2500", "5000"}
            Using sr As IO.StreamReader = New IO.StreamReader(filePathName)
                'read the version text
                Dim linestring As String = sr.ReadLine
                Dim errormessage As String = Nothing
                'check version
                If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                    sr.Close()
                    errormessage = "The map parameter file's version doesn't match the version of the model!"
                    MessageBox.Show(errormessage, "BAGIS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return retSettings
                End If
                'read the map unit text
                retSettings = New MapsSettings()
                linestring = sr.ReadLine
                If Trim(linestring) = "True" Then
                    retSettings.ZMeters = True
                Else
                    retSettings.ZMeters = False
                End If
                'prepare the elevation interval list
                linestring = sr.ReadLine
                Dim idxElevation As Integer = 0
                If BA_IsValidInteger(linestring) Then idxElevation = CInt(linestring)
                If lstElevationInterval.Count > (idxElevation + 1) Then
                    retSettings.ElevationInterval = lstElevationInterval.Item(idxElevation)
                End If
                linestring = sr.ReadLine    'Elevation class number
                Dim listCount As Integer = 0
                If BA_IsValidInteger(linestring) Then listCount = CInt(linestring)
                For i As Integer = 0 To listCount - 1
                    linestring = sr.ReadLine    'throw this away; we don't need it
                Next
                'prepare the PRISM list
                linestring = sr.ReadLine
                retSettings.IdxPrecipType = Val(linestring)
                'ignore rest of PRISM settings; Not used
                linestring = sr.ReadLine    'Begin date
                linestring = sr.ReadLine    'End date
                retSettings.MinimumPrecip = sr.ReadLine    'Min precip
                retSettings.MaximumPrecip = sr.ReadLine    'Max precip
                linestring = sr.ReadLine    'Precip range
                linestring = sr.ReadLine    'Precip interval
                linestring = sr.ReadLine    'Number of precip zones
                linestring = sr.ReadLine    'Precip zones listbox; throw away, not used
                listCount = Val(Trim(linestring))
                If listCount > 0 Then
                    For i = 1 To listCount
                        linestring = sr.ReadLine
                    Next
                End If
                linestring = sr.ReadLine    'number of subdivision
                'subrange analysis settings
                retSettings.UseSubRange = Convert.ToBoolean(sr.ReadLine)
                retSettings.SubRangeFromElev = sr.ReadLine
                retSettings.SubRangeToElev = sr.ReadLine
                If sr.Peek > -1 Then 'check if additional parameters were added after BAGIS Ver 1. Aspect was added in version 2
                    linestring = sr.ReadLine 'skip the REVISION text
                    linestring = sr.ReadLine 'aspect
                    Dim tokenstring() As String = linestring.Split(New Char(), " "c)
                    If tokenstring(0).ToUpper = "ASPECT" Then
                        retSettings.AspectDirections = tokenstring(1)
                    End If
                End If
            End Using
        Else
            MessageBox.Show("Unable to open the maps settings file. Please configure the settings from the Map Settings screen!",
                            "BAGIS", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Return retSettings
    End Function

    Public Function BA_IsValidInteger(ByVal fn As String) As Boolean
        ' Check for any value
        If String.IsNullOrEmpty(fn) Then Return False
        ' Check for numeric value
        If Not IsNumeric(fn) Then Return False
        ' Try to convert to Integer
        Dim iVal As Integer
        Try
            iVal = CInt(fn)
        Catch ex As Exception
            ' Could not convert to Integer, return false
            Return False
        End Try
        ' Don't allow negative numbers
        If iVal >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub BA_GenerateTables(ByVal oMapsSettings As MapsSettings, ByVal EMaxValue As Double, ByVal EMinValue As Double,
                                 ByVal bInteractive As Boolean)
        Dim response As Integer
        Dim SNOTELParentPath As String
        Dim SNOTELBareName As String
        Dim AOILayerPath As String
        Dim SnowCourseParentPath As String
        Dim SnowCourseBareName As String
        Dim bRepresentedPrecip As Boolean = False
        Dim bDemInMeters As Boolean = False

        'verify elevation range values if used
        If oMapsSettings.UseSubRange = True Then
            If Val(oMapsSettings.SubRangeFromElev) < EMinValue Or
                Val(oMapsSettings.SubRangeToElev) > EMaxValue Or
                Val(oMapsSettings.SubRangeFromElev) >= Val(oMapsSettings.SubRangeToElev) Then
                MsgBox("Invalid elevation range specified for localized analysis!")
                Exit Sub
            End If
        End If

        'Check for existence of elev-precip AOI table; If it doesn't exist, disable elevation-precip tool
        'This isn't perfect but we will assume if the maps config file was saved and the table doesn't exist,
        'Then the previous run didn't include elev-precip
        If oMapsSettings.IdxPrecipType > -1 Then
            bRepresentedPrecip = True
            If Not BA_File_Exists(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis) + "\" +
                    BA_TablePrecMeanElev, WorkspaceType.Geodatabase, esriDatasetType.esriDTTable) Then
                bRepresentedPrecip = False
            End If
        End If

        'convert unit to internal system unit, i.e., meters
        Dim elevUnit As MeasurementUnit = BA_GetElevationUnitsForAOI(AOIFolderBase)
        If elevUnit = MeasurementUnit.Missing Then
            Dim aoiName As String = BA_GetBareName(AOIFolderBase)
            Dim pAoi As Aoi = New Aoi(aoiName, AOIFolderBase, "", "")
            Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(pAoi)
            If frmDataUnits.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                elevUnit = frmDataUnits.NewElevationUnit
            Else
                'Set elevUnit to system default and warn the user
                elevUnit = MeasurementUnit.Meters
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("You did not define the elevation units for this" & vbCrLf)
                sb.Append("AOI. BAGIS assumes the elevation units are" & vbCrLf)
                sb.Append(BA_EnumDescription(MeasurementUnit.Meters) & ". If the elevation units are not " & BA_EnumDescription(MeasurementUnit.Meters) & vbCrLf)
                sb.Append("the results will be incorrect." & vbCrLf)
                MessageBox.Show(sb.ToString, "Elevation units not defined", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
        If elevUnit = MeasurementUnit.Meters Then
            bDemInMeters = True
        End If

        If oMapsSettings.ZMeters = True Then
            BA_ElevationUnitString = "Meters"
        Else
            BA_ElevationUnitString = "Feet"
        End If

        AOILayerPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)   'clipped SNOTEL and SnowCourse are in this folder
        SNOTELParentPath = AOILayerPath
        SNOTELBareName = BA_EnumDescription(MapsFileName.Snotel)
        SnowCourseParentPath = AOILayerPath
        SnowCourseBareName = BA_EnumDescription(MapsFileName.SnowCourse)

        'set the y axis values of the excel charts
        BA_Excel_SetYAxis(EMinValue, EMaxValue, CDbl(oMapsSettings.ElevationInterval))

        'Declare Excel object variables
        Dim objExcel As New Microsoft.Office.Interop.Excel.Application
        Dim bkWorkBook As Workbook 'a file in excel
        bkWorkBook = objExcel.Workbooks.Add

        'Declare progress indicator variables
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating Basin Analysis Tables", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()


            '=============================================
            ' Create Excel WorkSheets
            '=============================================
            pStepProg.Message = "Preparing Excel Spreadsheets..."
            pStepProg.Step()

            'Create Subdivided Elevation Distribution Worksheet for plotting curve
            Dim pSubElvWorksheet As Worksheet = bkWorkBook.ActiveSheet
            pSubElvWorksheet.Name = "Elevation Curve"

            'Create SNOTEL Distribution Worksheet
            Dim pSNOTELWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSNOTELWorksheet.Name = "SNOTEL"

            'Create Snow Course Distribution Worksheet
            Dim pSnowCourseWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSnowCourseWorksheet.Name = "Snow Course"

            'Create Snow Course Distribution Worksheet
            Dim pAspectWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pAspectWorksheet.Name = "Aspect"

            'Create Snow Course Distribution Worksheet
            Dim pSlopeWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSlopeWorksheet.Name = "Slope"

            'Create Snow Course Distribution Worksheet
            Dim pPRISMWorkSheet As Worksheet = bkWorkBook.Sheets.Add
            pPRISMWorkSheet.Name = "PRISM"

            'Create Elevation Distribution Worksheet
            Dim pAreaElvWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pAreaElvWorksheet.Name = "Area Elevations"

            'Dim variables for the range worksheets in case we need them later
            Dim pSCRangeWorksheet As Worksheet = Nothing
            Dim pElevationRangeWorksheet As Worksheet = Nothing
            Dim pPrecipitationRangeWorksheet As Worksheet = Nothing
            Dim pRangeChartWorksheet As Worksheet = Nothing
            Dim pChartsWorksheet As Worksheet = Nothing
            Dim pSTRangeWorksheet As Worksheet = Nothing
            If oMapsSettings.UseSubRange = True Then
                If AOI_HasSnowCourse Then
                    pSCRangeWorksheet = bkWorkBook.Sheets.Add
                    pSCRangeWorksheet.Name = "Snow Course Range"
                End If

                If AOI_HasSNOTEL Then
                    pSTRangeWorksheet = bkWorkBook.Sheets.Add
                    pSTRangeWorksheet.Name = "SNOTEL Range"
                End If

                pPrecipitationRangeWorksheet = bkWorkBook.Sheets.Add
                pPrecipitationRangeWorksheet.Name = "PRISM Range"

                pElevationRangeWorksheet = bkWorkBook.Sheets.Add
                pElevationRangeWorksheet.Name = "Elevation Range"

                pRangeChartWorksheet = bkWorkBook.Sheets.Add
                pRangeChartWorksheet.Name = "Range Charts"
            End If

            Dim pPrecipDemElevWorksheet As Worksheet = Nothing
            Dim pPrecipSiteWorksheet As Worksheet = Nothing
            Dim pPrecipChartWorksheet As Worksheet = Nothing
            If bRepresentedPrecip = True Then
                'Create Elevation Precipitation Worksheet
                pPrecipDemElevWorksheet = bkWorkBook.Sheets.Add
                pPrecipDemElevWorksheet.Name = "Elev-Precip AOI"

                'Create Site Precipitation Worksheet
                pPrecipSiteWorksheet = bkWorkBook.Sheets.Add
                pPrecipSiteWorksheet.Name = "Elev-Precip Sites"

                'Create Elev-Precip chart Worksheet
                pPrecipChartWorksheet = bkWorkBook.Sheets.Add
                pPrecipChartWorksheet.Name = "Elev-Precip Chart"
            End If

            'Create Snow Course Distribution Worksheet
            pChartsWorksheet = bkWorkBook.Sheets.Add
            pChartsWorksheet.Name = "Charts"

            '============================================
            '4. Create Elevation Excel Table
            '============================================
            Dim MaxPRISMValue As Double

            pStepProg.Message = "Creating Elevation Tables and Charts..."
            pStepProg.Step()

            'create elevation table for summary statistics
            Dim conversionFactor As Double
            If oMapsSettings.ZMeters = False Then 'Display = Feet
                If bDemInMeters = False Then 'DEM = Feet
                    conversionFactor = 1
                Else 'DEM = METERS
                    conversionFactor = 3.2808399 'Meters to Foot
                End If
            Else 'Display = Meters
                If bDemInMeters = False Then 'DEM = Feet
                    conversionFactor = 0.3048 'Foot to Meters
                Else 'DEM = Meters
                    conversionFactor = 1
                End If
            End If
            response = BA_Excel_CreateElevationTable(AOIFolderBase, pAreaElvWorksheet, conversionFactor, AOI_DEMMin, oMapsSettings.ZMeters)

            Dim positionLeftSecondColumn As Int16 = 650
            Dim row2TopPosition As Int16 = BA_ChartHeight + 80
            Dim row3TopPosition As Int16 = row2TopPosition + BA_ChartHeight + 80

            'populate the settings for the text description boxes
            LoadChartTextboxSettings(positionLeftSecondColumn, row2TopPosition, row3TopPosition)

            'create subdivided elevation table for plotting the curve
            response = BA_Excel_CreateSubElevationTable(AOIFolderBase, pSubElvWorksheet, conversionFactor, AOI_DEMMin, oMapsSettings.ZMeters)
            response = BA_Excel_CreateElevationChart(pSubElvWorksheet, pChartsWorksheet, BA_ChartSpacing, BA_ChartSpacing, Chart_YMinScale, Chart_YMaxScale,
                                                     Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)

            If AOI_HasSNOTEL Then
                pStepProg.Message = "Creating SNOTEL Table and Chart..."
                pStepProg.Step()
                response = BA_Excel_CreateSNOTELTable(AOIFolderBase, pSNOTELWorksheet, pSubElvWorksheet, BA_EnumDescription(MapsFileName.SnotelZone), conversionFactor)
                response = BA_Excel_CreateSNOTELChart(pSNOTELWorksheet, pSubElvWorksheet, pChartsWorksheet, True,
                    positionLeftSecondColumn, row2TopPosition,
                    Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)
            End If

            If AOI_HasSnowCourse Then
                pStepProg.Message = "Creating Snow Course Table and Chart..."
                pStepProg.Step()

                response = BA_Excel_CreateSNOTELTable(AOIFolderBase, pSnowCourseWorksheet, pSubElvWorksheet, BA_EnumDescription(MapsFileName.SnowCourseZone), conversionFactor)
                response = BA_Excel_CreateSNOTELChart(pSnowCourseWorksheet, pSubElvWorksheet, pChartsWorksheet, False,
                        positionLeftSecondColumn, row3TopPosition,
                        Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)
            End If

            pStepProg.Message = "Creating ASPECT Table and Chart..."
            pStepProg.Step()

            response = BA_Excel_CreateAspectTable(AOIFolderBase, pAspectWorksheet)
            response = BA_Excel_CreateAspectChart(pAspectWorksheet, pChartsWorksheet, row3TopPosition + BA_ChartHeight + 80)

            pStepProg.Message = "Creating SLOPE Table and Chart..."
            pStepProg.Step()

            response = BA_Excel_CreateSlopeTable(AOIFolderBase, pSlopeWorksheet)
            response = BA_Excel_CreateSlopeChart(pSlopeWorksheet, pChartsWorksheet, row3TopPosition)

            pStepProg.Message = "Creating Precipitation Table and Chart..."
            pStepProg.Step()

            'Calculate file path for prism based on the form
            response = BA_Excel_CreatePRISMTable(AOIFolderBase, pPRISMWorkSheet, pSubElvWorksheet, MaxPRISMValue,
                                                 oMapsSettings.PrecipPath & "\" + oMapsSettings.PRISMRasterName, AOI_DEMMin, conversionFactor, oMapsSettings.ZMeters)
            response = BA_Excel_CreatePRISMChart(pPRISMWorkSheet, pSubElvWorksheet, pChartsWorksheet,
                                                 positionLeftSecondColumn, BA_ChartSpacing,
                                                 Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, MaxPRISMValue, oMapsSettings.ZMeters,
                                                 Not oMapsSettings.ZMeters)

            pStepProg.Message = "Creating Combined Charts..."
            pStepProg.Step()

            response = BA_Excel_CreateCombinedChart(pPRISMWorkSheet, pSubElvWorksheet, pChartsWorksheet, pSnowCourseWorksheet,
                                                    pSNOTELWorksheet, Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, MaxPRISMValue,
                                                    oMapsSettings.ZMeters, Not oMapsSettings.ZMeters, AOI_HasSNOTEL, AOI_HasSnowCourse,
                                                    Nothing, False, row2TopPosition)

            'copy DEM area and %_area to the PRISM table
            'response = Excel_CopyCells(pAreaElvWorksheet, 3, pPRISMWorkSheet, 12)
            response = BA_Excel_CopyCells(pAreaElvWorksheet, 3, pPRISMWorkSheet, 12)
            'response = Excel_CopyCells(pAreaElvWorksheet, 10, pPRISMWorkSheet, 13)
            response = BA_Excel_CopyCells(pAreaElvWorksheet, 10, pPRISMWorkSheet, 13)
            response = BA_Excel_PrecipitationVolume(pPRISMWorkSheet, 12, 7, 14, 15)

            Dim demTitleUnit As MeasurementUnit = MeasurementUnit.Feet
            If oMapsSettings.ZMeters Then
                demTitleUnit = MeasurementUnit.Meters
            End If

            If bRepresentedPrecip = True Then
                pStepProg.Message = "Creating Elevation-Precipitation Correlation Charts..."
                pStepProg.Step()

                Dim partitionFieldName As String = BA_UNKNOWN
                Dim partitionFileName As String = BA_FindElevPrecipRasterName(BA_RasterPartPrefix)
                Dim partitionRasterPath As String = Nothing
                If Not String.IsNullOrEmpty(partitionFileName) Then
                    partitionRasterPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + partitionFileName
                End If
                Dim zonesFileName As String = BA_FindElevPrecipRasterName(BA_ZonesRasterPrefix)
                Dim zoneRasterPath As String = Nothing
                If Not String.IsNullOrEmpty(zonesFileName) Then
                    zoneRasterPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) + zonesFileName
                End If
                If Not String.IsNullOrEmpty(partitionRasterPath) Then
                    If Not String.IsNullOrEmpty(partitionFileName) Then _
                        partitionFieldName = partitionFileName.Substring(BA_RasterPartPrefix.Length)
                End If
                Dim zonesFieldName As String = Nothing
                If Not String.IsNullOrEmpty(zoneRasterPath) Then
                    zonesFileName = zonesFileName.Substring(BA_ZonesRasterPrefix.Length)
                    zonesFieldName = BA_FIELD_VALUE
                End If

                Dim success As BA_ReturnCode = BA_CreateRepresentPrecipTable(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis), BA_TablePrecMeanElev,
                    oMapsSettings.PRISMRasterName + "_1", BA_RasterPrecMeanElev, BA_FIELD_ASPECT, partitionFieldName, pPrecipDemElevWorksheet, demTitleUnit, conversionFactor,
                    MeasurementUnit.Inches, partitionFieldName, zonesFileName, zonesFieldName)
                If success = BA_ReturnCode.Success Then
                    success = BA_CreateSnotelPrecipTable(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis), BA_VectorSnotelPrec,
                                                         BA_FIELD_PRECIP, BA_SiteElevField, BA_SiteNameField,
                                                         BA_SiteTypeField, BA_FIELD_ASPECT, partitionFieldName,
                                                         pPrecipSiteWorksheet, MeasurementUnit.Inches, partitionFieldName,
                                                         zonesFileName, conversionFactor, Nothing)

                    If success = BA_ReturnCode.Success Then
                        Dim demChartMin As Integer = Math.Floor(EMinValue / 100) * 100
                        Dim prismChartMin As Integer = Math.Floor(Convert.ToDouble(oMapsSettings.MinimumPrecip)) - 1
                        success = BA_CreateRepresentPrecipChart(bkWorkBook, pPrecipDemElevWorksheet, pPrecipSiteWorksheet,
                                                                pPrecipChartWorksheet,
                                                                demTitleUnit, MeasurementUnit.Inches,
                                                                demChartMin, prismChartMin)
                    End If
                End If
            End If

            Dim bHasSnotelRange As Boolean = False
            Dim bHasSCRange As Boolean = False
            If oMapsSettings.UseSubRange = True Then
                pStepProg.Message = "Creating Elevation Range Tables and Charts..."
                pStepProg.Step()

                response = BA_Excel_CreateElevRangeTable(pElevationRangeWorksheet, pSubElvWorksheet, CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev))
                response = BA_Excel_CreatePrecipRangeTable(pPrecipitationRangeWorksheet, pPRISMWorkSheet, CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), MaxPRISMValue)

                response = BA_Excel_CreateElevationChart(pElevationRangeWorksheet, pRangeChartWorksheet, BA_ChartSpacing, BA_ChartSpacing,
                    CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)

                If AOI_HasSNOTEL Then
                    response = BA_Excel_CreateSNOTELRangeTable(pSTRangeWorksheet, pSNOTELWorksheet, pSubElvWorksheet, CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev))
                    If response = 1 Then
                        response = BA_Excel_CreateSNOTELChart(pSTRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, True,
                                                          positionLeftSecondColumn, row2TopPosition,
                                                          CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)
                        If response = 1 Then bHasSnotelRange = True
                    End If
                End If

                If AOI_HasSnowCourse Then
                    response = BA_Excel_CreateSNOTELRangeTable(pSCRangeWorksheet, pSnowCourseWorksheet, pSubElvWorksheet, CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev))
                    If response = 1 Then
                        response = BA_Excel_CreateSNOTELChart(pSCRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, False,
                               positionLeftSecondColumn, row3TopPosition,
                               CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), Chart_YMapUnit, oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)
                        If response = 1 Then bHasSCRange = True
                    End If
                End If

                response = BA_Excel_CreatePRISMChart(pPrecipitationRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet,
                                                     positionLeftSecondColumn, BA_ChartSpacing,
                                                     CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), Chart_YMapUnit, MaxPRISMValue,
                                                     oMapsSettings.ZMeters, Not oMapsSettings.ZMeters)

                response = BA_Excel_CreateCombinedChart(pPrecipitationRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, pSCRangeWorksheet,
                                                        pSTRangeWorksheet, CDbl(oMapsSettings.SubRangeFromElev), CDbl(oMapsSettings.SubRangeToElev), Chart_YMapUnit, MaxPRISMValue,
                                                        oMapsSettings.ZMeters, Not oMapsSettings.ZMeters, AOI_HasSNOTEL, AOI_HasSnowCourse, Nothing,
                                                        False, row2TopPosition)
            End If


            'Publish Charts Tab
            If bInteractive = False Then

                pStepProg.Message = "Publishing .pdf chart documents ..."
                pStepProg.Step()

                Dim sOutputFolder As String = AOIFolderBase + BA_ExportMapPackageFolder
                Dim pathToSave As String = sOutputFolder + "\" + BA_ExportChartAreaElevPdf
                Dim cboSelectAOI = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
                Dim aoiName As String = ""
                If Not String.IsNullOrEmpty(cboSelectAOI.getValue) Then
                    aoiName = cboSelectAOI.getValue.ToUpper
                End If

                Dim oPaperSize As XlPaperSize = XlPaperSize.xlPaperLetter
                Try
                    oPaperSize = pChartsWorksheet.PageSetup.PaperSize
                Catch ex As Exception
                    Debug.Print("BA_GenerateTables Exception: " + ex.Message)
                    MessageBox.Show("An error occurred while querying Excel's paper size! Please test printing from Excel and try again", "BAGIS")
                    Exit Sub
                End Try

                Dim oReqPaperSize As XlPaperSize = XlPaperSize.xlPaperLetter
                With pChartsWorksheet.PageSetup
                    .Zoom = False
                    .PaperSize = oReqPaperSize
                    .FitToPagesTall = 1
                    .FitToPagesWide = 1
                    .PrintArea = "$A$1:$M$27"
                    .CenterHeader = "&C&""Arial,Bold""&16 " + aoiName
                End With
                pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)

                pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevPrecipPdf
                With pChartsWorksheet.PageSetup
                    .PrintArea = "$N$1:$AA$27"
                End With
                pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)

                If AOI_HasSNOTEL Or AOI_HasSnowCourse Then
                    pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevPrecipSitePdf
                    With pChartsWorksheet.PageSetup
                        .PrintArea = "$A$29:$M$56"
                    End With
                    pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                End If

                If AOI_HasSNOTEL Then
                    pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevSnotelPdf
                    With pChartsWorksheet.PageSetup
                        .PrintArea = "$N$29:$AA$56"
                    End With
                    pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                End If

                If AOI_HasSnowCourse Then
                    pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevScosPdf
                    With pChartsWorksheet.PageSetup
                        .PrintArea = "$N$57:$AA$85"
                    End With
                    pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                End If
                pathToSave = sOutputFolder + "\" + BA_ExportChartSlopePdf
                With pChartsWorksheet.PageSetup
                    .PrintArea = "$A$57:$M$85"
                End With
                pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                pathToSave = sOutputFolder + "\" + BA_ExportChartAspectPdf
                With pChartsWorksheet.PageSetup
                    .PrintArea = "$A$86:$M$113"
                End With
                pChartsWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                pChartsWorksheet.PageSetup.PaperSize = oPaperSize

                'Subrange Chart Tab
                If pRangeChartWorksheet IsNot Nothing Then
                    pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevSubrangePdf
                    oPaperSize = pRangeChartWorksheet.PageSetup.PaperSize
                    With pRangeChartWorksheet.PageSetup
                        .Zoom = False
                        .PaperSize = oReqPaperSize
                        .FitToPagesTall = 1
                        .FitToPagesWide = 1
                        .PrintArea = "$A$1:$M$27"
                        .CenterHeader = "&C&""Arial,Bold""&16 " + aoiName
                    End With
                    pRangeChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)

                    pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevPrecipSubrangePdf
                    With pRangeChartWorksheet.PageSetup
                        .PrintArea = "$N$1:$AA$27"
                    End With
                    pRangeChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)

                    If bHasSCRange = True Or bHasSnotelRange = True Then
                        pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevPrecipSiteSubrangePdf
                        With pRangeChartWorksheet.PageSetup
                            .PrintArea = "$A$29:$M$56"
                        End With
                        pRangeChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                    End If

                    If bHasSnotelRange = True Then
                        pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevSnotelSubrangePdf
                        With pRangeChartWorksheet.PageSetup
                            .PrintArea = "$N$29:$AA$56"
                        End With
                        pRangeChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                    End If

                    If bHasSCRange = True Then
                        pathToSave = sOutputFolder + "\" + BA_ExportChartAreaElevScosSubrangePdf
                        With pRangeChartWorksheet.PageSetup
                            .PrintArea = "$N$57:$AA$85"
                        End With
                        pRangeChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                    End If
                    pRangeChartWorksheet.PageSetup.PaperSize = oPaperSize
                End If
                'Elev-Precip Chart Tab
                If pPrecipChartWorksheet IsNot Nothing Then
                    oPaperSize = pPrecipChartWorksheet.PageSetup.PaperSize
                    With pPrecipChartWorksheet.PageSetup
                        .Orientation = XlPageOrientation.xlLandscape
                        .Zoom = False
                        .PaperSize = oReqPaperSize
                        .FitToPagesTall = 1
                        .FitToPagesWide = 1
                        .CenterHeader = "&C&""Arial,Bold""&16 " + aoiName
                    End With
                    pathToSave = sOutputFolder + "\" + BA_ExportChartElevPrecipCorrelPdf
                    pPrecipChartWorksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pathToSave)
                    pPrecipChartWorksheet.PageSetup.PaperSize = oPaperSize
                End If
            End If

        Catch ex As Exception
            Debug.Print("CmdTables_Click Exception: " & ex.Message)
        Finally
            If bInteractive = True Then
                objExcel.Visible = True
                If pStepProg IsNot Nothing Then
                    pStepProg.Hide()
                    pStepProg = Nothing
                End If
                If progressDialog2 IsNot Nothing Then
                    progressDialog2.HideDialog()
                    progressDialog2 = Nothing
                End If
            Else
                bkWorkBook.Close(SaveChanges:=False)
                objExcel.Quit()
                objExcel = Nothing
                pStepProg.Hide()
                progressDialog2.HideDialog()
            End If
        End Try
    End Sub

    Private Sub LoadChartTextboxSettings(ByVal positionLeftSecondColumn As Single, ByVal row2TopPosition As Single,
                                         ByVal row3TopPosition As Single)
        BA_DictChartTextBoxSettings = New Dictionary(Of String, ChartTextBoxSettings)
        Dim areaElevSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With areaElevSettings
            .Left = BA_ChartSpacing
            .Top = BA_ChartHeight + 10
            .Message = "Area-Elevation Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAreaElevPdf, areaElevSettings)
        Dim areaElevPrecipSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With areaElevPrecipSettings
            .Left = positionLeftSecondColumn
            .Top = BA_ChartHeight + 10
            .Message = "Area-Elevation and Precipitation Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAreaElevPrecipPdf, areaElevPrecipSettings)
        Dim areaElevPrecipSiteSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With areaElevPrecipSiteSettings
            .Left = BA_ChartSpacing
            .Top = row2TopPosition + BA_ChartHeight + 10
            .Message = "Area-Elevation, Precipitation and Site Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAreaElevPrecipSitePdf, areaElevPrecipSiteSettings)
        Dim areaElevSnotelSiteSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With areaElevSnotelSiteSettings
            .Left = positionLeftSecondColumn
            .Top = row2TopPosition + BA_ChartHeight + 10
            .Message = "Area-Elevation and SNOTEL Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAreaElevSnotelPdf, areaElevSnotelSiteSettings)
        Dim areaElevScosSiteSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With areaElevScosSiteSettings
            .Left = positionLeftSecondColumn
            .Top = row3TopPosition + BA_ChartHeight + 10
            .Message = "Area-Elevation and Snow Course Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAreaElevScosPdf, areaElevScosSiteSettings)
        Dim slopeSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With slopeSettings
            .Left = BA_ChartSpacing
            .Top = row3TopPosition + BA_ChartHeight + 10
            .Message = "Slope Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartSlopePdf, slopeSettings)
        Dim aspectSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With aspectSettings
            .Left = BA_ChartSpacing
            .Top = row3TopPosition + BA_ChartHeight + 80 + BA_ChartHeight + 10
            .Message = "Aspect Distribution chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartAspectPdf, aspectSettings)
        Dim correlationSettings As ChartTextBoxSettings = New ChartTextBoxSettings()
        With correlationSettings
            .Width = BA_LargeChartWidth
            .Left = BA_ChartSpacing
            .Top = BA_LargeChartHeight + 10
            .Message = "Elevation Precipitation Correlation chart"
        End With
        BA_DictChartTextBoxSettings.Add(BA_ExportChartElevPrecipCorrelPdf, correlationSettings)

    End Sub

    Public Function BA_ExportActiveViewAsPdf(ByVal sOutputDir As String, ByVal sFileName As String, ByVal iOutputResolution As Long,
                                             ByVal lResampleRatio As Long, ByVal bClipToGraphicsExtent As Boolean) As BA_ReturnCode
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

            'cleanup for the exporter
            docExport.Cleanup()
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ExportActiveViewAsPdf Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_DeleteMapPackageElements() As BA_ReturnCode
        Dim arr_package_all As String() = {BA_ExportChartAreaElevPdf, BA_ExportChartAreaElevPrecipPdf, BA_ExportChartAreaElevPrecipSitePdf,
            BA_ExportChartAreaElevSnotelPdf, BA_ExportChartAreaElevScosPdf, BA_ExportChartSlopePdf, BA_ExportChartAspectPdf,
            BA_ExportChartElevPrecipCorrelPdf, BA_ExportChartAreaElevSubrangePdf, BA_ExportChartAreaElevPrecipSubrangePdf, BA_ExportChartAreaElevPrecipSiteSubrangePdf,
            BA_ExportChartAreaElevSnotelSubrangePdf, BA_ExportChartAreaElevScosSubrangePdf, BA_ExportAllMapsChartsPdf, BA_TitlePagePdf}
        Dim success As BA_ReturnCode = BA_ReturnCode.OtherError
        Dim strFilePath As String = ""
        For Each strFile As String In arr_package_all
            strFilePath = AOIFolderBase + BA_ExportMapPackageFolder + "\" + strFile
            If IO.File.Exists(strFilePath) Then
                Try
                    IO.File.Delete(strFilePath)
                Catch ex As IO.IOException
                    MessageBox.Show("Unable to delete " + strFilePath + "! " + vbCrLf +
                                    "The most likely cause is that you have the file open. " +
                                    "Please check and try again.", "BAGIS")
                    Return success
                End Try
            End If
        Next
        success = BA_ReturnCode.Success
        Return success
    End Function

End Module
