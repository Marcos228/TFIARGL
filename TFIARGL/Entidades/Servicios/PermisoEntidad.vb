<Serializable()>
Public Class PermisoEntidad
    Inherits PermisoBaseEntidad

    Public Overrides Function ValidarURL(paramURL As String) As Boolean
        If UCASE(Me.URL) = UCASE(paramURL) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function agregarHijo(Perm As PermisoBaseEntidad) As Boolean
        Return False
    End Function

    Public Overrides Function tieneHijos() As Boolean
        Return False
    End Function

    Public Overrides Function esValido(nombrePermiso As String) As Boolean
        Return Me.Nombre.Equals(nombrePermiso)
    End Function
End Class
