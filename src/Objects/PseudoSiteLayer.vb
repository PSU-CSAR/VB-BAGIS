Imports System.Xml.Serialization
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class PseudoSiteLayer
    Inherits SerializableData

    Dim m_layerName As String
    Dim m_layerPath As String
    Dim m_valueField As String
    Dim m_bufferDistance As Double
    Dim m_selectedValues As List(Of String)
    Dim m_bufferUnits As MeasurementUnit

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal layerName As String, ByVal layerPath As String, ByVal valueField As String, _
            ByVal selectedValues As IList(Of String))
        m_layerName = layerName
        m_layerPath = layerPath
        m_valueField = valueField
        m_selectedValues = selectedValues
    End Sub

    Public Property LayerName() As String
        Get
            Return m_layerName
        End Get
        Set(value As String)
            m_layerName = value
        End Set
    End Property

    Public Property LayerPath() As String
        Get
            Return m_layerPath
        End Get
        Set(value As String)
            m_layerPath = value
        End Set
    End Property

    Public Property ValueField() As String
        Get
            Return m_valueField
        End Get
        Set(value As String)
            m_valueField = value
        End Set
    End Property

    'Public Property SelectedValues() As IList(Of String)
    '    Get
    '        Return m_selectedValues
    '    End Get
    '    Set(value As IList(Of String))
    '        m_selectedValues = New List(Of String)
    '        m_selectedValues.AddRange(value)
    '    End Set
    'End Property

    'Public Property BufferDistance() As Double
    '    Get
    '        Return m_bufferDistance
    '    End Get
    '    Set(value As Double)
    '        m_bufferDistance = value
    '    End Set
    'End Property

    '<XmlIgnore()> Public Property BufferUnits() As esriUnits
    '    Get
    '        Return m_bufferUnits
    '    End Get
    '    Set(value As esriUnits)
    '        m_bufferUnits = value
    '    End Set
    'End Property

    'Public Property BufferUnitsText() As String
    '    Get
    '        Dim unitsText As String = m_bufferUnits.ToString
    '        If Left(unitsText, 4).ToLower = "esri" Then
    '            unitsText = unitsText.Remove(0, Len("esri"))
    '        End If
    '        Return unitsText
    '    End Get
    '    Set(ByVal value As String)
    '        m_bufferUnits = BA_GetEsriUnits(value)
    '    End Set
    'End Property

End Class
