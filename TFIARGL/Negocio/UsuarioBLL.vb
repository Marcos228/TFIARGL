Imports Entidades
Imports DAL
Imports System.Xml.Serialization
Imports System.IO

Public Class UsuarioBLL

    Private UsuarioDAL As New UsuarioDAL
    Private UsuarioEntidad As New UsuarioEntidad
    Public Function ValidarNombre(usu As UsuarioEntidad) As Boolean
        Try
            If DigitoVerificadorBLL.Integridad Then
                If IsNothing((New UsuarioDAL).ExisteUsuario(usu)) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function
    Public Function ExisteUsuario(ByVal Usuario As UsuarioEntidad) As UsuarioEntidad
        Try
            If DigitoVerificadorBLL.Integridad Then
                Dim UsuarioRetorno As UsuarioEntidad = New UsuarioEntidad
                UsuarioRetorno = UsuarioDAL.ExisteUsuario(Usuario)
                If Not IsNothing(UsuarioRetorno) Then
                    If UsuarioRetorno.Bloqueo = False Then
                        Return UsuarioRetorno
                    Else
                        Throw New ExceptionUsuarioBloqueado
                    End If
                Else
                    Throw New ExceptionUsuarioNoExiste
                End If
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch UsuarioBloqueado As ExceptionUsuarioBloqueado
            Throw UsuarioBloqueado
        Catch ExcepcionUsuarioNoExiste As ExceptionUsuarioNoExiste
            Throw ExcepcionUsuarioNoExiste
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function Alta(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Alta(Usuario) Then
                BitacoraBLL.CrearBitacora("Se creó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo crear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function

    Public Function Modificar(ByVal Usuario As UsuarioEntidad, ByVal NombreUsuario As String) As Boolean
        Try
            If UsuarioDAL.Modificar(Usuario) Then
                BitacoraBLL.CrearBitacora("Se modificó el Usuario: " & NombreUsuario & " en el sistema.", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo modificar el Usuario: " & NombreUsuario & " en el sistema. Error de Conexion", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function Eliminar(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Eliminar(Usuario) Then
                BitacoraBLL.CrearBitacora("Se eliminó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo eliminar el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function TraerUsuarios() As List(Of UsuarioEntidad)
        Try
            If DigitoVerificadorBLL.Integridad Then
                Return UsuarioDAL.TraerUsuarios()
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function TraerUsuariosParaBloqueo() As List(Of UsuarioEntidad)
        Try
            If DigitoVerificadorBLL.Integridad Then
                Return UsuarioDAL.TraerUsuariosParaBloqueo()
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function VerificarPassword(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If DigitoVerificadorBLL.Integridad Then
                If UsuarioDAL.VerificarPassword(Usuario) Then
                    Usuario.Intento = 0
                    Me.ResetearIntentos(Usuario)
                    Return True
                Else
                    BitacoraBLL.CrearBitacora("El Usuario: " & Usuario.Nombre & " quiso ingresar al sistema con una contraseña invalida.", TipoBitacora.Login, Usuario)
                    Return False
                End If
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("el Usuario: " & Usuario.Nombre & " no pudo conectarse a el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Sub ResetearIntentos(ByVal paramusuarioentidad As UsuarioEntidad)
        Try
            UsuarioDAL.ResetearIntentos(paramusuarioentidad)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Sub
    Public Sub SumarIntentos(ByVal Usuario As UsuarioEntidad)
        Try
            If DigitoVerificadorBLL.Integridad Then
                Usuario.Intento += 1
                UsuarioDAL.SumarIntentos(Usuario)
                If Usuario.Intento > 2 Then
                    UsuarioDAL.Bloquear(Usuario)
                    BitacoraBLL.CrearBitacora("El Usuario: " & SessionBLL.SesionActual.ObtenerUsuarioActual.Nombre & " ha sido bloqueado por esceso de intentos.", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
                End If
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No pudo bloquearse o sumar intentos en el Usuario: " & SessionBLL.SesionActual.ObtenerUsuarioActual.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Sub
    Public Function CambiarPassword(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.CambiarPassword(Usuario) Then
                BitacoraBLL.CrearBitacora("El Usuario: " & Usuario.Nombre & " actual cambió su contraseña.", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("El Usuario: " & Usuario.Nombre & " no pudo cambiar su contraseña. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function Bloquear(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Bloquear(Usuario) Then
                BitacoraBLL.CrearBitacora("Se bloqueó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo bloquear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function
End Class
