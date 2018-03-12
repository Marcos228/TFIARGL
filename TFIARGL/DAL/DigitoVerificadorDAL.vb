Imports DAL
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.SqlClient
Imports System.Configuration
Public Class DigitoVerificadorDAL
    Public Shared Function CalcularDVH(ByRef Fila As String) As String
        Try
            Dim UE As New UnicodeEncoding
            Dim bHash As Byte()
            Dim bCadena() As Byte = UE.GetBytes(Fila)
            Dim s1Service As New SHA1CryptoServiceProvider
            bHash = s1Service.ComputeHash(bCadena)
            Dim Resumen As String
            Resumen = Convert.ToBase64String(bHash)
            Return Resumen
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Shared Function CalcularDVV(ByRef Fila As String, ByRef NombreTabla As String) As Boolean
        Try
            Dim commandVerificador As SqlCommand
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Digito_Verificador_Vertical where Nombre_Tabla = @Nombre_Tabla")
            Command.Parameters.Add(New SqlParameter("@Nombre_Tabla", NombreTabla))
            Dim DataTabla = Acceso.Lectura(Command)
            Dim contador As Integer = 0
            For Each row As DataRow In DataTabla.Rows
                contador = contador + 1
            Next
            If contador = 0 Then
                commandVerificador = Acceso.MiComando("Insert into Digito_Verificador_Vertical values (@Nombre_Tabla, @Digito)")
            Else
                commandVerificador = Acceso.MiComando("Update Digito_Verificador_Vertical set Digito=@Digito where Nombre_Tabla=@Nombre_Tabla")
            End If
            Dim Resumen As String
            Resumen = CalcularDVH(Fila)
            With commandVerificador.Parameters
                .Add(New SqlParameter("@Nombre_Tabla", NombreTabla))
                .Add(New SqlParameter("@Digito", Resumen))
            End With
            Acceso.Escritura(commandVerificador)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function VerificarIntegridad() As DataTable
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Digito_Verificador_Vertical")
            Dim DataTabla = Acceso.Lectura(Command)
            Return DataTabla
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ProbarConectividad() As Boolean
        Try
            Dim MiConecction As New SqlCommand
            MiConecction.Connection = Acceso.MiConexion
            MiConecction.Connection.Open()
            If MiConecction.Connection.State = ConnectionState.Open Then
                MiConecction.Connection.Close()
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
