Public Class PermisoEntidad
    Inherits PermisoBaseEntidad

    Public Overrides Function agregarHijo(Perm As PermisoBaseEntidad) As Boolean
        Return False
    End Function

    Public Overrides Function obtenerHijos() As List(Of PermisoBaseEntidad)
        Return Nothing
    End Function

    Public Overrides Function tieneHijos() As Boolean
        Return False
    End Function

    Public Overrides Function esValido(nombrePermiso As String) As Boolean
        Return Me.Nombre.Equals(nombrePermiso)
    End Function

    Public Overrides Function quitarHijo(permiso As PermisoBaseEntidad) As Boolean
        Return False
    End Function

    Public Overrides Sub BorrarHijos()

    End Sub
End Class
