Imports Entidades
Imports DAL
Public Class BackupRestoreBLL
    Private _backuprestoredal As BackupRestoreDAL
    Private _backuprestoreentidad As BackupRestoreEntidad
    Private Bitacora As New BitacoraBLL

    Public Function CrearBackup(ByRef directorio As String, ByRef nombre As String, ByRef usuario As UsuarioEntidad) As Boolean
        Try
            Me._backuprestoreentidad = New BackupRestoreEntidad(directorio, usuario, nombre)
            Me._backuprestoredal = New BackupRestoreDAL
            If Me._backuprestoredal.RealizarBackup(Me._backuprestoreentidad) Then
                Bitacora.makeSimpleLog("Se ha creado un backup del sistema.")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Bitacora.makeSimpleLog("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message)
            Throw ex
        End Try

    End Function

    Public Function RealizarRestore(ByRef BackupEntidad As BackupRestoreEntidad) As Boolean
        Try
            _backuprestoredal = New BackupRestoreDAL
            If _backuprestoredal.RealizarRestore(BackupEntidad) Then
                Bitacora.makeSimpleLog("Se ha realizado un restore del sistema.")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Bitacora.makeSimpleLog("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message)
            Throw ex
        End Try
    End Function
End Class
