Imports Entidades
Imports System.Text
Imports System.Data.SqlClient
Imports System.IO

Public Class BackupRestoreDAL
    Public Function RealizarBackup(ByRef BackupEntidad As BackupRestoreEntidad) As Boolean
        Dim ruta As String = ""
        If BackupEntidad.Directorio.Length <> 3 Then
            ruta = Acceso.BackUpFolder & "\" & BackupEntidad.Nombre & ".bak"
        Else
            ruta = Acceso.BackUpFolder & "\" & BackupEntidad.Nombre & ".bak"
        End If
        Using MiConectionMaster = Acceso.MiConexionMaster()
            Try
                Dim MiStringBuilder As New StringBuilder
                MiStringBuilder.Append("BACKUP DATABASE [ARGLeague] TO DISK = '" & ruta & "' ")
                MiStringBuilder.Append("WITH DESCRIPTION = 'Backup ARGLeague', NOFORMAT, NOINIT, ")
                MiStringBuilder.Append("NAME = '" & BackupEntidad.Nombre & "', SKIP, NOREWIND, NOUNLOAD, STATS = 10")
                Dim MiComando As New SqlCommand(MiStringBuilder.ToString, MiConectionMaster)
                MiConectionMaster.Open()
                MiComando.ExecuteNonQuery()
                Return True
            Catch ex As Exception
                Throw ex
            Finally
                MiConectionMaster.Dispose()
                MiConectionMaster.Close()
            End Try
        End Using
    End Function

    Public Function RealizarRestore(ByRef BackupEntidad As BackupRestoreEntidad) As Boolean
        Dim MiConectionMaster As New SqlConnection
        Try
            MiConectionMaster = Acceso.MiConexionMaster
            Dim Strcomando As String = " ALTER DATABASE  [ARGLeague] SET SINGLE_USER WITH ROLLBACK IMMEDIATE RESTORE DATABASE [ARGLeague] FROM DISK = '" & BackupEntidad.Nombre & "'  With Replace ALTER DATABASE [ARGLeague] SET MULTI_USER "
            Dim MiComando As New SqlCommand(Strcomando, MiConectionMaster)
            MiConectionMaster.Open()
            MiComando.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        Finally
            MiConectionMaster.Dispose()
            MiConectionMaster.Close()
        End Try
    End Function
End Class
