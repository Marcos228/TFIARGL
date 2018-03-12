Imports Entidades
Imports System.Text
Imports System.Data.SqlClient

Public Class BackupRestoreDAL
    Public Function RealizarBackup(ByRef BackupEntidad As BackupRestoreEntidad) As Boolean
            Dim ruta As String = ""
            If BackupEntidad.Directorio.Length <> 3 Then
                ruta = BackupEntidad.Directorio & "\" & BackupEntidad.Nombre & ".bak"
            Else
                ruta = BackupEntidad.Directorio & BackupEntidad.Nombre & ".bak"
            End If
            Using MiConectionMaster = Acceso.MiConexionMaster()
                Try
                    Dim MiStringBuilder As New StringBuilder
                    MiStringBuilder.Append("BACKUP DATABASE [OrganieV] TO DISK = '" & ruta & "' ")
                    MiStringBuilder.Append("WITH DESCRIPTION = 'Backup OrganieV', NOFORMAT, NOINIT, ")
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
            Dim Strcomando As String = " ALTER DATABASE  [OrganieV] SET SINGLE_USER WITH ROLLBACK IMMEDIATE RESTORE DATABASE [OrganieV] FROM DISK = '" & BackupEntidad.Directorio & "'  With Replace ALTER DATABASE [OrganieV] SET MULTI_USER "
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
