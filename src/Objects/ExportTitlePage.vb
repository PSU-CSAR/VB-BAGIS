Imports System.Xml.Serialization
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class ExportTitlePage
    Inherits SerializableData

    Public aoi_name As String
    Public publisher As String
    Public comments As String
    Public local_path As String
    Public date_created As DateTime

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Public Property DateCreatedText() As String
        Get
            Dim zone As System.TimeZoneInfo = System.TimeZoneInfo.Local
            Dim strDate As String = date_created.ToString("MMMM d, yyyy a\t h:mm tt ")
            Return strDate & zone.DisplayName
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

End Class
