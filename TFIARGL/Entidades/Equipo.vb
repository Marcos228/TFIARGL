Public Class Equipo
    Private _id_equipo As Integer
    Public Property ID_Equipo() As Integer
        Get
            Return _id_equipo
        End Get
        Set(ByVal value As Integer)
            _id_equipo = value
        End Set
    End Property
    Private _fecha_inicio As DateTime
    Public Property Fecha_Inicio() As DateTime
        Get
            Return _fecha_inicio
        End Get
        Set(ByVal value As DateTime)
            _fecha_inicio = value
        End Set
    End Property

    Private _fecha_fin As DateTime
    Public Property Fecha_Fin() As DateTime
        Get
            Return _fecha_fin
        End Get
        Set(ByVal value As DateTime)
            _fecha_fin = value
        End Set
    End Property

    Private _puntos As Double
    Public Property Puntos() As Double
        Get
            Return _puntos
        End Get
        Set(ByVal value As Double)
            _puntos = value
        End Set
    End Property

    Private _game As Game
    Public Property Game() As Game
        Get
            Return _game
        End Get
        Set(ByVal value As Game)
            _game = value
        End Set
    End Property

    Private _jugadores As New List(Of Jugador)
    Public Property Jugadores() As List(Of Jugador)
        Get
            Return _jugadores
        End Get
        Set(ByVal value As List(Of Jugador))
            _jugadores = value
        End Set
    End Property


    Private _historia As String
    Public Property Historia() As String
        Get
            Return _historia
        End Get
        Set(ByVal value As String)
            _historia = value
        End Set
    End Property



    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property
    Sub New(ByRef juego As Game, ByRef nom As String, ByRef histy As String, FechaHoy As DateTime, Player As Jugador)
        Me.Game = juego
        Me.Fecha_Inicio = FechaHoy
        Me.Historia = histy
        Me.Nombre = nom
        Me.Jugadores.Add(Player)

    End Sub

    Sub New()

    End Sub

End Class
