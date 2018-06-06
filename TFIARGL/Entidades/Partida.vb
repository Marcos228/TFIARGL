Public Class Partida
    Private _id_partida As Integer
    Public Property ID_Partida() As Integer
        Get
            Return _id_partida
        End Get
        Set(ByVal value As Integer)
            _id_partida = value
        End Set
    End Property
    Private _fecha_hora As DateTime
    Public Property FechaHora() As DateTime
        Get
            Return _fecha_hora
        End Get
        Set(ByVal value As DateTime)
            _fecha_hora = value
        End Set
    End Property

    Private _equipos As New List(Of Equipo)
    Public Property Equipos() As List(Of Equipo)
        Get
            Return _equipos
        End Get
        Set(ByVal value As List(Of Equipo))
            _equipos = value
        End Set
    End Property

    Private _estadisticas As New List(Of Estadistica)
    Public Property Estadisticas() As List(Of Estadistica)
        Get
            Return _estadisticas
        End Get
        Set(ByVal value As List(Of Estadistica))
            _estadisticas = value
        End Set
    End Property

    Private _resultado As String
    Public Property Resultado() As String
        Get
            Return _resultado
        End Get
        Set(ByVal value As String)
            _resultado = value
        End Set
    End Property

    Private _ganador As Equipo
    Public Property Ganador() As Equipo
        Get
            Return _ganador
        End Get
        Set(ByVal value As Equipo)
            _ganador = value
        End Set
    End Property

    Private _fase As Fases
    Public Property Fase() As Fases
        Get
            Return _fase
        End Get
        Set(ByVal value As Fases)
            _fase = value
        End Set
    End Property

End Class
