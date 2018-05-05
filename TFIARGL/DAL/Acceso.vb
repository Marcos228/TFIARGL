Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Globalization
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Threading
Imports System.Reflection

Public Class Acceso
    Public Shared BackUpFolder As String = System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString()
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
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
            command.Connection.Open()
            Return command.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            command.Connection.Close()
            command.Connection.Dispose()
        End Try
    End Function
    Public Shared Function Escritura(command As SqlCommand) As Integer
        Try
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
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
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function MiConexion() As SqlConnection
        Dim MiConecction = New SqlConnection(WebConfigurationManager.ConnectionStrings("Saitama").ConnectionString)
        Return MiConecction
    End Function

    Public Shared Function MiConexionMaster() As SqlConnection
        Dim MiConecction = New SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("SaitamaMaster").ConnectionString)
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

    Public Shared Sub AgregarParametros(ByVal Someobject As Object, ByRef listaparam As List(Of String))
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        Dim _type As Type = Someobject.GetType()
        Dim properties() As PropertyInfo = _type.GetProperties()
        For Each _property As PropertyInfo In properties
            If _property.PropertyType.FullName.Contains("Entidades.") Then
                If Not _property.PropertyType.FullName.Contains("Collections.") Then
                    If _property.PropertyType.GetProperties.Count > 0 Then
                        For Each _property2 As PropertyInfo In _property.PropertyType.GetProperties
                            Dim Objeto As Object = _property.GetValue(Someobject, Nothing)
                            If _property2.Name.Contains("ID") Then
                                If IsNothing(Objeto) Then
                                    listaparam.Add(DBNull.Value.ToString)
                                Else
                                    listaparam.Add(_property2.GetValue(Objeto, Nothing).ToString)
                                End If

                                Exit For
                            End If
                        Next
                    Else
                        listaparam.Add(_property.GetValue(Someobject, Nothing))
                    End If
                End If
            Else
                listaparam.Add(_property.GetValue(Someobject, Nothing))
            End If
        Next
    End Sub

End Class
