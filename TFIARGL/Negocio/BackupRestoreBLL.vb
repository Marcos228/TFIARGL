Imports Entidades
Imports DAL
Public Class BackupRestoreBLL
    Private _backuprestoredal As BackupRestoreDAL
    Private _backuprestoreentidad As BackupRestoreEntidad

    Public Function CrearBackup(ByRef directorio As String, ByRef nombre As String) As Boolean
        Try
            Me._backuprestoreentidad = New BackupRestoreEntidad With {.Directorio = directorio, .Nombre = nombre}
            Me._backuprestoredal = New BackupRestoreDAL
            If Me._backuprestoredal.RealizarBackup(Me._backuprestoreentidad) Then
                BitacoraBLL.CrearBitacora("Se ha creado un backup del sistema.", TipoBitacora.Backup, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo crear un backup del sistema. Error de Conexion", TipoBitacora.Backup, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try

    End Function

    Public Function RealizarRestore(ByRef BackupEntidad As BackupRestoreEntidad) As Boolean
        Try
            _backuprestoredal = New BackupRestoreDAL
            If _backuprestoredal.RealizarRestore(BackupEntidad) Then
                BitacoraBLL.CrearBitacora("Se ha realizado un restore del sistema.", TipoBitacora.Restore, SessionBLL.SesionActual.ObtenerUsuarioActual)
                Return True
            Else
                Return False
            End If
        Catch FalloConexion As InvalidOperationException
            Dim Bitacora As New BitacoraEntidad("No se pudo realizar un restore del sistema. Error de Conexion", TipoBitacora.Backup, SessionBLL.SesionActual.ObtenerUsuarioActual)
            BitacoraBLL.ArchivarBitacora(Bitacora)
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function
End Class
