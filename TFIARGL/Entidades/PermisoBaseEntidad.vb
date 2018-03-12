Public MustInherit Class PermisoBaseEntidad
    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
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
    Public MustOverride Function esValido(nombrePermiso As String) As Boolean
    Public MustOverride Sub BorrarHijos()
    Public MustOverride Function agregarHijo(ByVal Perm As PermisoBaseEntidad) As Boolean
    Public MustOverride Function quitarHijo(permiso As PermisoBaseEntidad) As Boolean
    Public MustOverride Function obtenerHijos() As List(Of PermisoBaseEntidad)
    Public MustOverride Function tieneHijos() As Boolean

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not obj Is Nothing Then
            If TypeOf obj Is PermisoBaseEntidad Then
                Return Me.Nombre.Equals(CType(obj, PermisoBaseEntidad).Nombre)
            ElseIf TypeOf obj Is String Then
                Return Me.Nombre.Equals(obj)
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Return Me.Nombre
    End Function

End Class
