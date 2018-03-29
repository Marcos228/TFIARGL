Imports Entidades
Imports DAL
Public Class GestorPermisosBLL

    Private PermisosDAL As GestorPermisosDAL

    Public Function Alta(ByVal perm As PermisoBaseEntidad) As Boolean
        Try
            If ValidarNombre(perm.Nombre) Then
                PermisosDAL = New GestorPermisosDAL
                PermisosDAL.Alta(perm)
                'BitacoraBLL.CrearBitacoraAuditoria("Se creó el Perfil: " & perm.Nombre & " en el sistema.", TipoBitacora.Alta, Now, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo crear el Perfil: " & perm.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Alta, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            'Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            'Throw ex
        End Try
    End Function

    Public Function Baja(ByVal Perfil As PermisoCompuestoEntidad)
        Try
            'If DigitoVerificadorBLL.VerifyAllIntegrity Then
            PermisosDAL = New GestorPermisosDAL
            Return PermisosDAL.Baja(Perfil.ID)
            '    BitacoraBLL.CrearBitacora("Se eliminó el Perfil: " & Perfil.Nombre & " en el sistema.", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
            ''Else
            '    Throw New ExceptionIntegridadUsuario
            'End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo eliminar el Perfil: " & Perfil.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Baja, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Sub Modificar(ByVal perm As PermisoBaseEntidad)
        Try
            PermisosDAL = New GestorPermisosDAL
            PermisosDAL.Modificar(perm)
            '    BitacoraBLL.CrearBitacora("Se modificó el Perfil: " & perm.Nombre & " en el sistema.", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
        Catch FalloConexion As InvalidOperationException
            'Dim Bitacora As New BitacoraEntidad("No se pudo modificar el Perfil: " & perm.Nombre & " en el sistema. Error de Conexion", TipoBitacora.Modificación, SessionBLL.SesionActual.ObtenerUsuarioActual)
            'BitacoraBLL.ArchivarBitacora(Bitacora)
            'Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Sub

    Public Function ListarFamilias(ByVal filtro As Boolean) As List(Of PermisoBaseEntidad)
        Try
            Dim Permisos As List(Of PermisoBaseEntidad) = New List(Of PermisoBaseEntidad)
            Permisos = (New GestorPermisosDAL).ListarFamilias(filtro)
            If Permisos.Count > 0 Then
                Return Permisos
            Else
                Throw New ExceptionNoHayPerfiles
            End If
        Catch NoHayPerfiles As ExceptionNoHayPerfiles
            Throw NoHayPerfiles
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function ConsultarporID(ByVal ID As Integer) As PermisoCompuestoEntidad
        Try
            Dim Permisos As PermisoCompuestoEntidad = New PermisoCompuestoEntidad
            Permisos = (New GestorPermisosDAL).ConsultarporID(ID)
            If Not IsNothing(Permisos) Then
                If Permisos.ID = 0 Then
                    Throw New ExceptionPermisoNoExiste
                End If
                Return Permisos
            Else
                Throw New ExceptionPermisoNoExiste
            End If
        Catch PermisoNoExiste As ExceptionPermisoNoExiste
            Throw PermisoNoExiste
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function ListarPermisos() As List(Of PermisoBaseEntidad)
        Try
            PermisosDAL = New GestorPermisosDAL
            Return PermisosDAL.ListarPermisos()
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function ValidarNombre(ByVal nombrepermiso As String) As Boolean
        Try
            PermisosDAL = New GestorPermisosDAL
            Return PermisosDAL.ValidarNombre(nombrepermiso)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            'BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function
End Class
