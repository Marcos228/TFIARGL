Public Class Jugador
    Private _id_jugador As Integer
    Public Property ID_Jugador() As Integer
        Get
            Return _id_jugador
        End Get
        Set(ByVal value As Integer)
            _id_jugador = value
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

    Private _rol_jugador As Rol_Jugador
    Public Property Rol_Jugador() As Rol_Jugador
        Get
            Return _rol_jugador
        End Get
        Set(ByVal value As Rol_Jugador)
            _rol_jugador = value
        End Set
    End Property

    Private _nickname As String
    Public Property NickName() As String
        Get
            Return _nickname
        End Get
        Set(ByVal value As String)
            _nickname = value
        End Set
    End Property

    Private _game_tag As String
    Public Property Game_Tag() As String
        Get
            Return _game_tag
        End Get
        Set(ByVal value As String)
            _game_tag = value
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


    Sub New(ByRef juego As Game, ByRef nick As String, ByRef tag As String, Rol_juego As Rol_jugador)
        Me.Game = juego
        Me.NickName = nick
        Me.Game_Tag = tag
        Me.Rol_Jugador = Rol_juego

    End Sub

End Class
