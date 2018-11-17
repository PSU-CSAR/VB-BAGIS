'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System
Imports System.Collections.Generic

Namespace My
    
    '''<summary>
    '''A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    '''</summary>
    Friend Module ThisAddIn
        
        Friend ReadOnly Property Name() As String
            Get
                Return "BAGIS"
            End Get
        End Property
        
        Friend ReadOnly Property AddInID() As String
            Get
                Return "{6efa6a44-8e41-41ea-b59f-6dee2a5453ba}"
            End Get
        End Property
        
        Friend ReadOnly Property Company() As String
            Get
                Return "Portland State University"
            End Get
        End Property
        
        Friend ReadOnly Property Version() As String
            Get
                Return "3.2"
            End Get
        End Property
        
        Friend ReadOnly Property Description() As String
            Get
                Return "Basin Analysis GIS"
            End Get
        End Property
        
        Friend ReadOnly Property Author() As String
            Get
                Return "Lesley Bross, Masoud Momeni, and Geoffrey Duh"
            End Get
        End Property
        
        Friend ReadOnly Property [Date]() As String
            Get
                Return "11/16/2018"
            End Get
        End Property
        
        <System.Runtime.CompilerServices.ExtensionAttribute()>  _
        Friend Function ToUID(ByVal id As String) As ESRI.ArcGIS.esriSystem.UID
            Dim uid As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()
            uid.Value = id
            Return uid
        End Function
        
        '''<summary>
        '''A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        '''</summary>
        Friend Class IDs
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAddRefLayers', the id declared for Add-in Button class 'BtnAddRefLayers'
            '''</summary>
            Friend Shared ReadOnly Property BtnAddRefLayers() As String
                Get
                    Return "Microsoft_BAGIS_BtnAddRefLayers"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnSaveAOIMXD', the id declared for Add-in Button class 'BtnSaveAOIMXD'
            '''</summary>
            Friend Shared ReadOnly Property BtnSaveAOIMXD() As String
                Get
                    Return "Microsoft_BAGIS_BtnSaveAOIMXD"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnBasinInfo', the id declared for Add-in Button class 'BtnBasinInfo'
            '''</summary>
            Friend Shared ReadOnly Property BtnBasinInfo() As String
                Get
                    Return "Microsoft_BAGIS_BtnBasinInfo"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAOIUtilities', the id declared for Add-in Button class 'BtnAOIUtilities'
            '''</summary>
            Friend Shared ReadOnly Property BtnAOIUtilities() As String
                Get
                    Return "Microsoft_BAGIS_BtnAOIUtilities"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnOptions', the id declared for Add-in Button class 'BtnOptions'
            '''</summary>
            Friend Shared ReadOnly Property BtnOptions() As String
                Get
                    Return "Microsoft_BAGIS_BtnOptions"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAbout', the id declared for Add-in Button class 'BtnAbout'
            '''</summary>
            Friend Shared ReadOnly Property BtnAbout() As String
                Get
                    Return "Microsoft_BAGIS_BtnAbout"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnBasin_Tool', the id declared for Add-in Button class 'BtnBasin_Tool'
            '''</summary>
            Friend Shared ReadOnly Property BtnBasin_Tool() As String
                Get
                    Return "Microsoft_BAGIS_BtnBasin_Tool"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnClipDEM', the id declared for Add-in Button class 'BtnClipDEM'
            '''</summary>
            Friend Shared ReadOnly Property BtnClipDEM() As String
                Get
                    Return "Microsoft_BAGIS_BtnClipDEM"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_cboTargetedBasin', the id declared for Add-in ComboBox class 'cboTargetedBasin'
            '''</summary>
            Friend Shared ReadOnly Property cboTargetedBasin() As String
                Get
                    Return "Microsoft_BAGIS_cboTargetedBasin"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAOI_Tool', the id declared for Add-in Button class 'BtnAOI_Tool'
            '''</summary>
            Friend Shared ReadOnly Property BtnAOI_Tool() As String
                Get
                    Return "Microsoft_BAGIS_BtnAOI_Tool"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnCreateAOI', the id declared for Add-in Button class 'BtnCreateAOI'
            '''</summary>
            Friend Shared ReadOnly Property BtnCreateAOI() As String
                Get
                    Return "Microsoft_BAGIS_BtnCreateAOI"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_cboTargetedAOI', the id declared for Add-in ComboBox class 'cboTargetedAOI'
            '''</summary>
            Friend Shared ReadOnly Property cboTargetedAOI() As String
                Get
                    Return "Microsoft_BAGIS_cboTargetedAOI"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnCreateAOIStream', the id declared for Add-in Button class 'BtnCreateAOIStream'
            '''</summary>
            Friend Shared ReadOnly Property BtnCreateAOIStream() As String
                Get
                    Return "Microsoft_BAGIS_BtnCreateAOIStream"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnGenerateMaps', the id declared for Add-in Button class 'BtnGenerateMaps'
            '''</summary>
            Friend Shared ReadOnly Property BtnGenerateMaps() As String
                Get
                    Return "Microsoft_BAGIS_BtnGenerateMaps"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnElevationDist', the id declared for Add-in Button class 'BtnElevationDist'
            '''</summary>
            Friend Shared ReadOnly Property BtnElevationDist() As String
                Get
                    Return "Microsoft_BAGIS_BtnElevationDist"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnElevationSNOTEL', the id declared for Add-in Button class 'BtnElevationSNOTEL'
            '''</summary>
            Friend Shared ReadOnly Property BtnElevationSNOTEL() As String
                Get
                    Return "Microsoft_BAGIS_BtnElevationSNOTEL"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnPrecipitationDist', the id declared for Add-in Button class 'BtnPrecipitationDist'
            '''</summary>
            Friend Shared ReadOnly Property BtnPrecipitationDist() As String
                Get
                    Return "Microsoft_BAGIS_BtnPrecipitationDist"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAspectDist', the id declared for Add-in Button class 'BtnAspectDist'
            '''</summary>
            Friend Shared ReadOnly Property BtnAspectDist() As String
                Get
                    Return "Microsoft_BAGIS_BtnAspectDist"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnSlopeDist', the id declared for Add-in Button class 'BtnSlopeDist'
            '''</summary>
            Friend Shared ReadOnly Property BtnSlopeDist() As String
                Get
                    Return "Microsoft_BAGIS_BtnSlopeDist"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnSiteScenario', the id declared for Add-in Button class 'BtnSiteScenario'
            '''</summary>
            Friend Shared ReadOnly Property BtnSiteScenario() As String
                Get
                    Return "Microsoft_BAGIS_BtnSiteScenario"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnScenario1', the id declared for Add-in Button class 'BtnScenario1'
            '''</summary>
            Friend Shared ReadOnly Property BtnScenario1() As String
                Get
                    Return "Microsoft_BAGIS_BtnScenario1"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnScenario2', the id declared for Add-in Button class 'BtnScenario2'
            '''</summary>
            Friend Shared ReadOnly Property BtnScenario2() As String
                Get
                    Return "Microsoft_BAGIS_BtnScenario2"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnDifferenceCondition', the id declared for Add-in Button class 'BtnDifferenceCondition'
            '''</summary>
            Friend Shared ReadOnly Property BtnDifferenceCondition() As String
                Get
                    Return "Microsoft_BAGIS_BtnDifferenceCondition"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnSiteRepresentation', the id declared for Add-in Button class 'BtnSiteRepresentation'
            '''</summary>
            Friend Shared ReadOnly Property BtnSiteRepresentation() As String
                Get
                    Return "Microsoft_BAGIS_BtnSiteRepresentation"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnElevSnowCourse', the id declared for Add-in Button class 'BtnElevSnowCourse'
            '''</summary>
            Friend Shared ReadOnly Property BtnElevSnowCourse() As String
                Get
                    Return "Microsoft_BAGIS_BtnElevSnowCourse"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_setDEMExtenttool', the id declared for Add-in Tool class 'setDEMExtenttool'
            '''</summary>
            Friend Shared ReadOnly Property setDEMExtenttool() As String
                Get
                    Return "Microsoft_BAGIS_setDEMExtenttool"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_setPourPointtool', the id declared for Add-in Tool class 'setPourPointtool'
            '''</summary>
            Friend Shared ReadOnly Property setPourPointtool() As String
                Get
                    Return "Microsoft_BAGIS_setPourPointtool"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_BtnAOIfromShapefile', the id declared for Add-in Button class 'BtnAOIfromShapefile'
            '''</summary>
            Friend Shared ReadOnly Property BtnAOIfromShapefile() As String
                Get
                    Return "Microsoft_BAGIS_BtnAOIfromShapefile"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Microsoft_BAGIS_SelectSiteTool', the id declared for Add-in Tool class 'SelectSiteTool'
            '''</summary>
            Friend Shared ReadOnly Property SelectSiteTool() As String
                Get
                    Return "Microsoft_BAGIS_SelectSiteTool"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Portland_State_University_BAGIS_BtnWebServices', the id declared for Add-in Button class 'BtnWebServices'
            '''</summary>
            Friend Shared ReadOnly Property BtnWebServices() As String
                Get
                    Return "Portland_State_University_BAGIS_BtnWebServices"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Portland_State_University_BAGIS_frmSiteScenario', the id declared for Add-in DockableWindow class 'frmSiteScenario+AddinImpl'
            '''</summary>
            Friend Shared ReadOnly Property frmSiteScenario() As String
                Get
                    Return "Portland_State_University_BAGIS_frmSiteScenario"
                End Get
            End Property
            
            '''<summary>
            '''Returns 'Portland_State_University_BAGIS_frmSiteRepresentations', the id declared for Add-in DockableWindow class 'frmSiteRepresentations+AddinImpl'
            '''</summary>
            Friend Shared ReadOnly Property frmSiteRepresentations() As String
                Get
                    Return "Portland_State_University_BAGIS_frmSiteRepresentations"
                End Get
            End Property
        End Class
    End Module
    
Friend Module ArcMap
  Private s_app As ESRI.ArcGIS.Framework.IApplication
  Private s_docEvent As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event

  Public ReadOnly Property Application() As ESRI.ArcGIS.Framework.IApplication
    Get
      If s_app Is Nothing Then
        s_app = TryCast(Internal.AddInStartupObject.GetHook(Of ESRI.ArcGIS.ArcMapUI.IMxApplication)(), ESRI.ArcGIS.Framework.IApplication)
                If s_app Is Nothing Then
                    Dim editorHost As ESRI.ArcGIS.Editor.IEditor = Internal.AddInStartupObject.GetHook(Of ESRI.ArcGIS.Editor.IEditor)()
                    If editorHost IsNot Nothing Then s_app = editorHost.Parent
                End If
            End If

            Return s_app
        End Get
    End Property

    Public ReadOnly Property Document() As ESRI.ArcGIS.ArcMapUI.IMxDocument
        Get
            If Application IsNot Nothing Then
                Return TryCast(Application.Document, ESRI.ArcGIS.ArcMapUI.IMxDocument)
            End If

            Return Nothing
        End Get
    End Property
    Public ReadOnly Property ThisApplication() As ESRI.ArcGIS.ArcMapUI.IMxApplication
        Get
            Return TryCast(Application, ESRI.ArcGIS.ArcMapUI.IMxApplication)
        End Get
    End Property
    Public ReadOnly Property DockableWindowManager() As ESRI.ArcGIS.Framework.IDockableWindowManager
        Get
            Return TryCast(Application, ESRI.ArcGIS.Framework.IDockableWindowManager)
        End Get
    End Property

    Public ReadOnly Property Events() As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event
        Get
            s_docEvent = TryCast(Document, ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event)
            Return s_docEvent
        End Get
    End Property

    Public ReadOnly Property Editor() As ESRI.ArcGIS.Editor.IEditor
        Get
            Dim editorUID As New ESRI.ArcGIS.esriSystem.UID
            editorUID.Value = "esriEditor.Editor"
            Return TryCast(Application.FindExtensionByCLSID(editorUID), ESRI.ArcGIS.Editor.IEditor)
        End Get
    End Property
End Module

Namespace Internal
  <ESRI.ArcGIS.Desktop.AddIns.StartupObjectAttribute(), _
   Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
   Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()> _
  Partial Public Class AddInStartupObject
    Inherits ESRI.ArcGIS.Desktop.AddIns.AddInEntryPoint

    Private m_addinHooks As List(Of Object)
    Private Shared _sAddInHostManager As AddInStartupObject

    Public Sub New()

    End Sub

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Protected Overrides Function Initialize(ByVal hook As Object) As Boolean
      Dim createSingleton As Boolean = _sAddInHostManager Is Nothing
      If createSingleton Then
        _sAddInHostManager = Me
        m_addinHooks = New List(Of Object)
        m_addinHooks.Add(hook)
      ElseIf Not _sAddInHostManager.m_addinHooks.Contains(hook) Then
        _sAddInHostManager.m_addinHooks.Add(hook)
      End If

      Return createSingleton
    End Function

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Protected Overrides Sub Shutdown()
      _sAddInHostManager = Nothing
      m_addinHooks = Nothing
    End Sub

    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Friend Shared Function GetHook(Of T As Class)() As T
      If _sAddInHostManager IsNot Nothing Then
        For Each o As Object In _sAddInHostManager.m_addinHooks
          If TypeOf o Is T Then
            Return DirectCast(o, T)
          End If
        Next
      End If

      Return Nothing
    End Function

    ''' <summary>
    ''' Expose this instance of Add-in class externally
    ''' </summary>
    Public Shared Function GetThis() As AddInStartupObject
      Return _sAddInHostManager
    End Function

  End Class
End Namespace

End Namespace
