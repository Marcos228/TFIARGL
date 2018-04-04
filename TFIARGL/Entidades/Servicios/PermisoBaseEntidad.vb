<Serializable()>
Public MustInherit Class PermisoBaseEntidad
    Private _id_Permiso As Integer
    Public Property ID_Permiso() As Integer
        Get
            Return _id_Permiso
        End Get
        Set(ByVal value As Integer)
            _id_Permiso = value
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
    Private _url As String
    Public Property URL() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property
    Public MustOverride Function ValidarURL(ByVal paramURL As String) As Boolean
    Public MustOverride Function agregarHijo(ByVal Perm As PermisoBaseEntidad) As Boolean
    Public MustOverride Function tieneHijos() As Boolean
    Public MustOverride Function esValido(nombrePermiso As String) As Boolean
    Public Overrides Function ToString() As String
        Return Me.Nombre
    End Function

End Class
