Imports Entidades

Public MustInherit Class GestorErroresBLL
    Inherits Exception
    Public MustOverride Function Mensaje(ByRef Idioma As Entidades.IdiomaEntidad) As String
End Class

Public Class ExceptionEquipoIncompleto
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return idioma.Palabras.Find(Function(p) p.Codigo = "ExceptionEquipoIncompleto").Traduccion
    End Function

End Class

Public Class ExceptionFalloConectividad
    Inherits GestorErroresBLL

    Public Overrides Function Mensaje(ByRef Idioma As IdiomaEntidad) As String
        Return Idioma.Palabras.Find(Function(p) p.Codigo = "ErrorConexionBase").Traduccion
    End Function
End Class

#Region "Integridad"
Public Class ExceptionIntegridadUsuario
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class

Public Class ExceptionIntegridadBitacora
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class
Public Class ExceptionIntegridadEvento
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class
#End Region

#Region "Usuario"
Public Class ExceptionUsuarioNoExiste
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return idioma.Palabras.Find(Function(p) p.Codigo = "ExceptionLoginIncorrecto").Traduccion
    End Function

End Class

Public Class ExceptionUsuarioBloqueado
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return idioma.Palabras.Find(Function(p) p.Codigo = "ExceptionUsuarioBloqueado").Traduccion
    End Function
End Class

Public Class ExceptionPasswordIncorrecta
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return idioma.Palabras.Find(Function(p) p.Codigo = "ExceptionLoginIncorrecto").Traduccion
    End Function

End Class


Public Class ExceptionNombreEnUso
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return idioma.Palabras.Find(Function(p) p.Codigo = "ExceptionNombreEnUso").Traduccion
    End Function

End Class

#End Region

#Region "Permiso"
Public Class ExceptionPermisoNoExiste
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class

Public Class ExceptionNoHayPerfiles
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class

#End Region

#Region "Idioma"
Public Class ExceptionNoHayIdiomasEditables
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class

#End Region

#Region "Bitacora"
Public Class ExceptionNoHayBitacoras
    Inherits GestorErroresBLL
    Public Overrides Function Mensaje(ByRef idioma As Entidades.IdiomaEntidad) As String
        Return ""
    End Function

End Class

#End Region