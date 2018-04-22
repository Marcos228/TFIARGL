Imports Entidades

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

    Sub New()

    End Sub

    Sub New(ByRef usu As UsuarioEntidad, ByVal detalle As String, ByVal Tipo_bit As Tipo_Bitacora, ByVal fec As DateTime, ByVal brws As String, ByVal IP As String, ByVal stack As String, ByVal exception As String, ByVal url As String)
        Me.Usuario = usu
        Me.Detalle = detalle
        Me.Tipo_Bitacora = Tipo_bit
        Me.Fecha = fec
        Me.Browser = brws
        Me.IP_Usuario = IP
        Me.StackTrace = stack
        Me.Exception = exception
        Me.URL = url
    End Sub
End Class
