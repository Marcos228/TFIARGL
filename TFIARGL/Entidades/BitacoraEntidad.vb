Public Class BitacoraEntidad
    Private _id_bitacora As Integer
    Public Property ID_Bitacora() As Integer
        Get
            Return _id_bitacora
        End Get
        Set(ByVal value As Integer)
            _id_bitacora = value
        End Set
    End Property
    Private _codigo As TipoBitacora
    Public Property Codigo() As TipoBitacora
        Get
            Return _codigo
        End Get
        Set(ByVal value As TipoBitacora)
            _codigo = value
        End Set
    End Property

    Private _usuario As UsuarioEntidad
    Public Property Usuario() As UsuarioEntidad
        Get
            Return _usuario
        End Get
        Set(ByVal value As UsuarioEntidad)
            _usuario = value
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

    Private _detalle As String
    Public Property Detalle() As String
        Get
            Return _detalle
        End Get
        Set(ByVal value As String)
            _detalle = value
        End Set
    End Property

    Sub New()

    End Sub

    Sub New(ByVal midetalle As String, ByVal micodigo As TipoBitacora, ByVal miusuario As UsuarioEntidad)
        _detalle = midetalle
        _codigo = micodigo
        _usuario = miusuario
        _fecha = Now
    End Sub

End Class
