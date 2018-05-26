Public Class Solicitudes
    Private _id As Integer
    Public Property ID_Solicitud() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _jugador As Entidades.Jugador
    Public Property Jugador() As Entidades.Jugador
        Get
            Return _jugador
        End Get
        Set(ByVal value As Entidades.Jugador)
            _jugador = value
        End Set
    End Property

    Private _equipo As Entidades.Equipo
    Public Property Equipo() As Entidades.Equipo
        Get
            Return _equipo
        End Get
        Set(ByVal value As Entidades.Equipo)
            _equipo = value
        End Set
    End Property

    Private _Mensaje As String
    Public Property Mensaje() As String
        Get
            Return _Mensaje
        End Get
        Set(ByVal value As String)
            _Mensaje = value
        End Set
    End Property
    Private _fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property

End Class
