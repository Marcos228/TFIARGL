Public Class UsuarioEntidad
    Private _id_usuario As Integer
    Public Property ID_Usuario() As Integer
        Get
            Return _id_usuario
        End Get
        Set(ByVal value As Integer)
            _id_usuario = value
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

    Private _password As String
    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Private _perfil As GrupoPermisoEntidad
    Public Property Perfil() As GrupoPermisoEntidad
        Get
            Return _perfil
        End Get
        Set(ByVal value As GrupoPermisoEntidad)
            _perfil = value
        End Set
    End Property

    Private _intento As Integer
    Public Property Intento() As Integer
        Get
            Return _intento
        End Get
        Set(ByVal value As Integer)
            _intento = value
        End Set
    End Property

    Private _idiomaentidad As IdiomaEntidad
    Public Property IdiomaEntidad() As IdiomaEntidad
        Get
            Return _idiomaentidad
        End Get
        Set(ByVal value As IdiomaEntidad)
            _idiomaentidad = value
        End Set
    End Property

    Private _bloqueo As Boolean
    Public Property Bloqueo() As Boolean
        Get
            Return _bloqueo
        End Get
        Set(ByVal value As Boolean)
            _bloqueo = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Me.Nombre
    End Function

End Class
