Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Configuration
Public Class Acceso

    Public Shared Function Lectura(command As SqlCommand) As DataTable
        Try
            Dim Da As New SqlDataAdapter(command)
            Dim Dt As New DataTable
            Da.Fill(Dt)
            Return Dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function Scalar(ByVal command As SqlCommand) As Integer

        Try
            command.Connection.Open()
            Return command.ExecuteScalar
        Catch ex As Exception
            Throw
        Finally
            command.Connection.Close()
            command.Connection.Dispose()
        End Try
    End Function
    Public Shared Function Escritura(command As SqlCommand) As Integer
        Try
            command.Connection.Open()
            Return command.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            command.Connection.Close()
            command.Connection.Dispose()
        End Try
    End Function
    Shared Function MiComando(ByVal consulta As String) As SqlCommand
        Dim objCommando As New SqlCommand(consulta, MiConexion())
        objCommando.CommandType = CommandType.Text
        Try
            objCommando.Connection.Open()
            If objCommando.Connection.State = ConnectionState.Open Then
                objCommando.Connection.Close()
                Return objCommando
            Else
                Dim exc As InvalidOperationException = New InvalidOperationException
                MsgBox("La base de Datos se encuentra sin conectividad. Vuelva a intentarlo nuevamente o consulte al administrador de la Base de Datos.", MsgBoxStyle.Critical, "Error Conexión")
                Throw exc
            End If
        Catch ex As Exception
            Dim exc As InvalidOperationException = New InvalidOperationException
            MsgBox("La base de Datos se encuentra sin conectividad. Vuelva a intentarlo nuevamente o consulte al administrador de la Base de Datos.", MsgBoxStyle.Critical, "Error Conexión")
            Throw exc
        End Try
    End Function
    Public Shared Function MiConexion() As SqlConnection
        Dim MiConecction = New SqlConnection(ConfigurationManager.ConnectionStrings("SQLProvider").ConnectionString)
        Return MiConecction
    End Function

    Public Shared Function MiConexionMaster() As SqlConnection
        Dim MiConecction = New SqlConnection(ConfigurationManager.ConnectionStrings("SQLProviderMaster").ConnectionString)
        Return MiConecction
    End Function

    Shared Function TraerID(ByRef IDGenerico As String, ByRef TablaGenerica As String) As Integer
        Try
            Dim ID As Integer
            Dim Command As SqlCommand = Acceso.MiComando("select Max(" & IDGenerico & ") as IDretorno from " & TablaGenerica)
            Dim DataTabla = Acceso.Lectura(Command)
            For Each row As DataRow In DataTabla.Rows
                If IsDBNull(row.Item(0)) Then
                    ID = 1
                Else
                    ID = row.Item(0) + 1
                End If
            Next
            Return ID
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
