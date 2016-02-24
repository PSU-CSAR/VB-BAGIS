Public Class Settings

    ' The terrain path is currently stored stored in the PublicPath enum. The .lyr file name
    ' may be updated and the Add-In would need to be recompiled. This keeps the .lyr file name
    ' in sync with the AddIn. If we can make the .lyr file available via webservice, the path
    ' should be put back into the JSON.
    'Public terrain As String
    Public drainage As String
    Public watershed As String
    Public dem10 As String
    Public dem30 As String
    Public preferredDem As String
    Public demElevUnit As String
    Public gaugeStation As String
    Public gaugeStationName As String
    Public gaugeStationArea As String
    Public gaugeStationUnits As String
    Public snotel As String
    Public snotelElev As String
    Public snotelName As String
    Public snowCourse As String
    Public snowCourseElev As String
    Public snowCourseName As String
    Public prism As String

End Class
