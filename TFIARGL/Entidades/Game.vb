Public Class Game
    Private _id_game As Integer
    Public Property ID_Game() As Integer
        Get
            Return _id_game
        End Get
        Set(ByVal value As Integer)
            _id_game = value
        End Set
    End Property
    Private _cant_jugadores As Integer
    Public Property CantJugadores() As Integer
        Get
            Return _cant_jugadores
        End Get
        Set(ByVal value As Integer)
            _cant_jugadores = value
        End Set
    End Property

    Private _reglas As String
    Public Property Reglas() As String
        Get
            Return _reglas
        End Get
        Set(ByVal value As String)
            _reglas = value
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
    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Private _tipo_juego As Tipo_Juego
    Public Property Tipo_Juego() As Tipo_Juego
        Get
            Return _tipo_juego
        End Get
        Set(ByVal value As Tipo_Juego)
            _tipo_juego = value
        End Set
    End Property

    Private _Imagen As Byte()
    Public Property Imagen() As Byte()
        Get
            Return _Imagen
        End Get
        Set(ByVal value As Byte())
            _Imagen = value
        End Set
    End Property

End Class
