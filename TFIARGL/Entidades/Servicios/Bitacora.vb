Public MustInherit Class Bitacora
#Region "Propiedades"
    Private _id_bitacora As Integer
    Public Property Id_Bitacora() As Integer
        Get
            Return _id_bitacora
        End Get
        Set(ByVal value As Integer)
            _id_bitacora = value
        End Set
    End Property

    Private _detalle As String
    Public Property Detalle() As String
        Get
            Return _detalle
        End Get
        Set(ByVal value As String)
            _detalle = value
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
    Private _usuario As Entidades.UsuarioEntidad
    Public Property Usuario() As Entidades.UsuarioEntidad
        Get
            Return _usuario
        End Get
        Set(ByVal value As Entidades.UsuarioEntidad)
            _usuario = value
        End Set
    End Property

    Private _ip_usuario As String
    Public Property IP_Usuario() As String
        Get
            Return _ip_usuario
        End Get
        Set(ByVal value As String)
            _ip_usuario = value
        End Set
    End Property


    Private _browser As String
    Public Property Browser() As String
        Get
            Return _browser
        End Get
        Set(ByVal value As String)
            _browser = value
        End Set
    End Property

    Private _tipo_bitacora As Tipo_Bitacora
    Public Property Tipo_Bitacora() As Tipo_Bitacora
        Get
            Return _tipo_bitacora
        End Get
        Set(ByVal value As Tipo_Bitacora)
            _tipo_bitacora = value
        End Set
    End Property


#End Region

    Sub New(ByVal Midetalle As String, ByVal micodigo As Tipo_Bitacora, ByRef Usuario As UsuarioEntidad)
        Me.Detalle = Midetalle
        Me.Tipo_Bitacora = micodigo
        Me.Usuario = Usuario
    End Sub
    Sub New()

    End Sub
End Class
