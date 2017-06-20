Imports System.Xml.Serialization
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class PseudoSite
    Inherits SerializableData

    Dim m_objectId As Integer
    Dim m_dateCreated As DateTime
    Dim m_siteName As String
    Dim m_useElev As Boolean
    Dim m_lowerElev As Double
    Dim m_upperElev As Double
    Dim m_elevUnits As esriUnits
    Dim m_useProximity As Boolean
    Dim m_bufferDistance As Double
    Dim m_bufferUnits As esriUnits
    Dim m_proximityLayer As String
    Dim m_usePrism As Boolean
    Dim m_precipTypeIdx As Short
    Dim m_precipBeginIdx As Short
    Dim m_precipEndIdx As Short
    Dim m_upperPrecip As Double
    Dim m_lowerPrecip As Double
    Dim m_useLocation As Boolean
    Dim m_locationLayers As List(Of PseudoSiteLayer)

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal objectId As Integer, ByVal siteName As String, ByVal useElevation As Boolean, ByVal usePrism As Boolean, _
            ByVal useProximity As Boolean, ByVal useDistance As Boolean)
        m_objectId = objectId
        m_siteName = siteName
        m_useElev = useElevation
        m_usePrism = usePrism
        m_useProximity = useProximity
        m_useLocation = useDistance
        m_dateCreated = DateAndTime.Now
    End Sub

    Public Property ObjectId() As Integer
        Get
            Return m_objectId
        End Get
        Set(ByVal value As Integer)
            m_objectId = value
        End Set
    End Property

    Public Property DateCreated() As DateTime
        Get
            Return m_dateCreated
        End Get
        Set(ByVal value As DateTime)
            m_dateCreated = value
        End Set
    End Property

    Public Property DateCreatedText() As String
        Get
            Dim zone As System.TimeZoneInfo = System.TimeZoneInfo.Local
            Dim strDate As String = m_dateCreated.ToString("d-MMM-yyyy h:m tt ")
            Return strDate & zone.DisplayName
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property SiteName() As String
        Get
            Return m_siteName
        End Get
        Set(value As String)
            m_siteName = value
        End Set
    End Property

    Public Property UseElevation() As Boolean
        Get
            Return m_useElev
        End Get
        Set(value As Boolean)
            m_useElev = value
        End Set
    End Property

    Public Property UpperElev() As Double
        Get
            Return m_upperElev
        End Get
        Set(value As Double)
            m_upperElev = value
        End Set
    End Property

    Public Property LowerElev() As Double
        Get
            Return m_lowerElev
        End Get
        Set(value As Double)
            m_lowerElev = value
        End Set
    End Property

    Public Property UseProximity() As Boolean
        Get
            Return m_useProximity
        End Get
        Set(value As Boolean)
            m_useProximity = value
        End Set
    End Property

    Public Property BufferDistance() As Double
        Get
            Return m_bufferDistance
        End Get
        Set(value As Double)
            m_bufferDistance = value
        End Set
    End Property

    <XmlIgnore()> Public Property BufferUnits() As esriUnits
        Get
            Return m_bufferUnits
        End Get
        Set(value As esriUnits)
            m_bufferUnits = value
        End Set
    End Property

    Public Property BufferUnitsText() As String
        Get
            Dim unitsText As String = m_bufferUnits.ToString
            If Left(unitsText, 4).ToLower = "esri" Then
                unitsText = unitsText.Remove(0, Len("esri"))
            End If
            Return unitsText
        End Get
        Set(ByVal value As String)
            m_bufferUnits = BA_GetEsriUnits(value)
        End Set
    End Property

    Public Property ProximityLayer() As String
        Get
            Return m_proximityLayer
        End Get
        Set(value As String)
            m_proximityLayer = value
        End Set
    End Property

    Public Property UsePrism() As Boolean
        Get
            Return m_usePrism
        End Get
        Set(value As Boolean)
            m_usePrism = value
        End Set
    End Property

    Public Property PrecipTypeIdx() As Short
        Get
            Return m_precipTypeIdx
        End Get
        Set(value As Short)
            m_precipTypeIdx = value
        End Set
    End Property

    Public Property PrecipBeginIdx() As Short
        Get
            Return m_precipBeginIdx
        End Get
        Set(value As Short)
            m_precipBeginIdx = value
        End Set
    End Property

    Public Property PrecipEndIdx() As Short
        Get
            Return m_precipEndIdx
        End Get
        Set(value As Short)
            m_precipEndIdx = value
        End Set
    End Property

    Public Property UpperPrecip As Double
        Get
            Return m_upperPrecip
        End Get
        Set(value As Double)
            m_upperPrecip = value
        End Set
    End Property

    Public Property LowerPrecip As Double
        Get
            Return m_lowerPrecip
        End Get
        Set(value As Double)
            m_lowerPrecip = value
        End Set
    End Property

    <XmlIgnore()> Public Property ElevUnits() As esriUnits
        Get
            Return m_elevUnits
        End Get
        Set(value As esriUnits)
            m_elevUnits = value
        End Set
    End Property

    Public Property ElevUnitsText() As String
        Get
            Dim unitsText As String = m_elevUnits.ToString
            If Left(unitsText, 4).ToLower = "esri" Then
                unitsText = unitsText.Remove(0, Len("esri"))
            End If
            Return unitsText
        End Get
        Set(ByVal value As String)
            m_elevUnits = BA_GetEsriUnits(value)
        End Set
    End Property

    Public Sub ElevationProperties(ByVal usingElevUnits As esriUnits, ByVal lowerElev As Double, ByVal upperElev As Double)
        m_elevUnits = usingElevUnits
        m_lowerElev = lowerElev
        m_upperElev = upperElev
    End Sub

    Public Sub PrismProperties(ByVal precipTypeIdx As Short, ByVal precipBeginIdx As Short, ByVal precipEndIdx As Short, _
                               ByVal precipLower As Double, ByVal precipUpper As Double)
        m_precipTypeIdx = precipTypeIdx
        m_precipBeginIdx = precipBeginIdx
        m_precipEndIdx = precipEndIdx
        m_lowerPrecip = precipLower
        m_upperPrecip = precipUpper
    End Sub

    Public Sub ProximityProperties(ByVal usingXYUnits As esriUnits, ByVal proximityLayer As String, ByVal proximityDistance As Double)
        m_bufferUnits = usingXYUnits
        m_proximityLayer = proximityLayer
        m_bufferDistance = proximityDistance
    End Sub

    Public Sub AddLocationProperties(ByVal layerName As String, ByVal layerPath As String, ByVal valueField As String, _
                                     ByVal lstSelValues As List(Of String), ByVal lstAllValues As List(Of String))
        If m_locationLayers Is Nothing Then
            m_locationLayers = New List(Of PseudoSiteLayer)
        End If
        Dim psiteLayer As PseudoSiteLayer = New PseudoSiteLayer(layerName, layerPath, valueField, lstSelValues, lstAllValues)
        m_locationLayers.Add(psiteLayer)
    End Sub

    Public Property UseLocation() As Boolean
        Get
            Return m_useLocation
        End Get
        Set(value As Boolean)
            m_useLocation = value
        End Set
    End Property

    Public Property LocationLayers() As List(Of PseudoSiteLayer)
        Get
            Return m_locationLayers
        End Get
        Set(value As List(Of PseudoSiteLayer))
            m_locationLayers = New List(Of PseudoSiteLayer)
            m_locationLayers.AddRange(value)
        End Set
    End Property

End Class
