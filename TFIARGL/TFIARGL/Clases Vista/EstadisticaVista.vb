Public Class EstadisticaVista
    Private _estadisticas As New List(Of Entidades.Estadistica)
    Public Property Estadisticas() As List(Of Entidades.Estadistica)
        Get
            Return _estadisticas
        End Get
        Set(ByVal value As List(Of Entidades.Estadistica))
            _estadisticas = value
        End Set
    End Property

    Private _Jugador As Entidades.Jugador
    Public Property Jugador() As Entidades.Jugador
        Get
            Return _Jugador
        End Get
        Set(ByVal value As Entidades.Jugador)
            _Jugador = value
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
End Class
