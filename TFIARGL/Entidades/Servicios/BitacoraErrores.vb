Public Class BitacoraErrores
    Inherits Bitacora
    Private _url As String
    Public Property URL() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

    Private _stacktrace As String
    Public Property StackTrace() As String
        Get
            Return _stacktrace
        End Get
        Set(ByVal value As String)
            _stacktrace = value
        End Set
    End Property
    Private _exception As String
    Public Property Exception() As String
        Get
            Return _exception
        End Get
        Set(ByVal value As String)
            _exception = value
        End Set
    End Property

End Class
