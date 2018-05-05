Imports Entidades
Imports DAL
Imports System.Web.HttpContext
Imports System.Xml.Serialization
Imports System.IO

Public Class UsuarioBLL

    Private UsuarioDAL As New UsuarioDAL
    Private UsuarioEntidad As New UsuarioEntidad


    Public Function ValidarNombre(usu As UsuarioEntidad) As Boolean
        Try
            'If DigitoVerificadorBLL.Integridad Then
            If IsNothing((New UsuarioDAL).ExisteUsuario(usu)) Then
                    Return True
                Else
                    Return False
                End If
            'Else
            '    Throw New ExceptionIntegridadUsuario
            'End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function CreateToken(ByRef usu As Entidades.UsuarioEntidad) As String
        Try
            Return UsuarioDAL.CrearToken(usu, False)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub LimpiarTokens(ByVal Token As String)
        Try
            UsuarioDAL.LimpiarTokens(Token)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function ACtivarUsuario(ByVal token As String) As Boolean
        Try
            Dim usuario As New Entidades.UsuarioEntidad
            usuario.ID_Usuario = UsuarioDAL.GetTokenUser(token)
            If Not IsNothing(usuario.ID_Usuario) And usuario.ID_Usuario > 0 Then
                Return UsuarioDAL.Bloquear(UsuarioDAL.BuscarUsuarioID(usuario))
            Else
                Return True
            End If

        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo bloquear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function ExisteUsuario(ByVal Usuario As UsuarioEntidad) As UsuarioEntidad
        Try
            Dim UsuarioRetorno As UsuarioEntidad = New UsuarioEntidad
            Dim Pass As String = Usuario.Password
            UsuarioRetorno = UsuarioDAL.ExisteUsuario(Usuario)
            If Not IsNothing(UsuarioRetorno) Then
                If EncriptarBLL.EncriptarPassword(Pass, UsuarioRetorno.Salt).Item(0) = UsuarioRetorno.Password Then
                If UsuarioRetorno.Bloqueo = False Then
                    Usuario.Intento = 0
                    Me.ResetearIntentos(UsuarioRetorno)
                    Return UsuarioDAL.TraerUsuario(UsuarioRetorno)
                Else
                    Throw New ExceptionUsuarioBloqueado
                End If
            Else
                Usuario.Intento += 1
                    UsuarioDAL.SumarIntentos(Usuario)
                    If Usuario.Intento > 2 Then
                        UsuarioDAL.Bloquear(Usuario)
                        Throw New ExceptionPasswordIncorrecta
                    End If
                    Throw New ExceptionPasswordIncorrecta
                End If

            Else
                Throw New ExceptionUsuarioNoExiste
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
            Throw ex
        End Try

    End Function

    Public Sub RefrescarUsuario(ByRef USuario As Entidades.UsuarioEntidad)
        Try
            UsuarioDAL.RefrescarUsuario(USuario)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Sub

    Public Function TraerUsuariosIdioma(ByVal ID_Idioma As Integer) As List(Of UsuarioEntidad)
        Try

            Return UsuarioDAL.TraerUsuariosIdioma(ID_Idioma)

        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function

    Public Function Alta(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If Me.ValidarNombre(Usuario) Then
                If UsuarioDAL.Alta(Usuario) Then
                    'BitacoraBLL.CrearBitacora("Se creó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
                    Return True
                Else
                    Return False
                End If
            Else
                Throw New ExceptionNombreEnUso
            End If

        Catch NombreUso As ExceptionNombreEnUso
            Throw NombreUso
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo crear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function GEtToken(ByVal ID_Usuario As Integer) As String
        Try
            Return UsuarioDAL.GetToken(ID_Usuario)
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo crear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function Modificar(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Modificar(Usuario) Then
                'BitacoraBLL.CrearBitacora("Se modificó el Usuario: " & NombreUsuario & " en el sistema.", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo modificar el Usuario: " & NombreUsuario & " en el sistema. Error de Conexion", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function Eliminar(ByVal Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Eliminar(Usuario) Then
                'BitacoraBLL.CrearBitacora("Se eliminó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo eliminar el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function TraerUsuarios() As List(Of UsuarioEntidad)
        Try
            'If DigitoVerificadorBLL.Integridad Then
            Return UsuarioDAL.TraerUsuarios()
            'Else
            '    Throw New ExceptionIntegridadUsuario
            'End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function
    Public Function TraerUsuariosParaBloqueo(ByRef Usuario As Entidades.UsuarioEntidad) As List(Of UsuarioEntidad)
        Try
            'If DigitoVerificadorBLL.Integridad Then
            Return UsuarioDAL.TraerUsuariosParaBloqueo(Usuario)
            'Else
            '    Throw New ExceptionIntegridadUsuario
            'End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function

    Public Function TraerUsuariosPerfil(ByRef ID_Perfil As Integer) As List(Of UsuarioEntidad)
        Try

            Return UsuarioDAL.TraerUsuariosPerfil(ID_Perfil)

        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function VerificarPassword(ByVal Usuario As UsuarioEntidad) As Boolean
        Try

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Sub ResetearIntentos(ByVal paramusuarioentidad As UsuarioEntidad)
        Try
            UsuarioDAL.ResetearIntentos(paramusuarioentidad)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Sub
    Public Sub SumarIntentos(ByVal Usuario As UsuarioEntidad)
        Try
            Usuario.Intento += 1
            UsuarioDAL.SumarIntentos(Usuario)
            If Usuario.Intento > 2 Then
                UsuarioDAL.Bloquear(Usuario)
                'BitacoraBLL.CrearBitacora("El Usuario: " & SessionBLL.SesionActual.ObtenerUsuarioActual.Nombre & " ha sido bloqueado por esceso de intentos.", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            End If

        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No pudo bloquearse o sumar intentos en el Usuario: " & SessionBLL.SesionActual.ObtenerUsuarioActual.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Sub
    Public Function CambiarPassword(ByRef Usuario As UsuarioEntidad, Optional ByVal token As String = Nothing) As Boolean
        Try
            If Not IsNothing(token) Then
                Dim usu As New Entidades.UsuarioEntidad
                usu.ID_Usuario = UsuarioDAL.GetTokenUser(token)
                UsuarioDAL.BuscarUsuarioID(usu)
                usu.Salt = Usuario.Salt
                usu.Password = Usuario.Password
                If usu.ID_Usuario > 0 Then
                    If UsuarioDAL.CambiarPassword(usu) Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Else
                If UsuarioDAL.CambiarPassword(Usuario) Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function Bloquear(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            If UsuarioDAL.Bloquear(Usuario) Then
                'BitacoraBLL.CrearBitacora("Se bloqueó el Usuario: " & Usuario.Nombre & " en el sistema.", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo bloquear el Usuario: " & Usuario.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Login, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Shared Function ProbarConectividad() As Boolean
        Return (New DAL.UsuarioDAL).ProbarConectividad
    End Function
End Class
