Public Class Rol_Jugador
    Private _id_rol As Integer
    Public Property ID_Rol() As Integer
        Get
            Return _id_rol
        End Get
        Set(ByVal value As Integer)
            _id_rol = value
        End Set
    End Property
    Private _tipo_rol As Tipo_rol
    Public Property Tipo_rol() As Tipo_rol
        Get
            Return _tipo_rol
        End Get
        Set(ByVal value As Tipo_rol)
            _tipo_rol = value
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



    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Private _imagen As Byte()
    Public Property imagen() As Byte()
        Get
            Return _imagen
        End Get
        Set(ByVal value As Byte())
            _imagen = value
        End Set
    End Property

End Class
