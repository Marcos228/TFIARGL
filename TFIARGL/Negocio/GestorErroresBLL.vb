Public MustInherit Class GestorErroresBLL
    Inherits Exception
    Public MustOverride Function Mensaje() As String
End Class

Public Class ExceptionFalloConectividad
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return "Error de Conexion a la Base de Datos Comuniquese con un administrador del sistema."
    End Function

End Class

#Region "Integridad"
Public Class ExceptionIntegridadUsuario
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class

Public Class ExceptionIntegridadBitacora
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class
Public Class ExceptionIntegridadEvento
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class
#End Region

#Region "Usuario"
Public Class ExceptionUsuarioNoExiste
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return "El Usuario no existe"
    End Function

End Class

Public Class ExceptionUsuarioBloqueado
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return "El usuario se encuentra bloqueado"
    End Function

End Class

Public Class ExceptionPasswordIncorrecta
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return "La Contraseña ingresada es incorrecta."
    End Function

End Class


Public Class ExceptionNombreEnUso
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return "El Nombre de Usuario o Correo Electronico ya se encuentra en uso"
    End Function

End Class

#End Region

#Region "Permiso"
Public Class ExceptionPermisoNoExiste
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class

Public Class ExceptionNoHayPerfiles
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class

#End Region

#Region "Idioma"
Public Class ExceptionNoHayIdiomasEditables
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class

#End Region

#Region "Bitacora"
Public Class ExceptionNoHayBitacoras
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje() As String
        Return ""
    End Function

End Class

#End Region