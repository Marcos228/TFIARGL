Imports System.Data.SqlClient
Public Class Acceso
    Public Function Lectura(command As SqlCommand) As DataTable

    End Function

    Public Function Scalar(command As SqlCommand) As DataTable

    End Function
    Public Function TraerID(IDGenerico As String, TablaGenerica As String) As Integer

    End Function

    Public Function Escritura(command As SqlCommand) As Integer

    End Function

    Public Function MiComando(consulta As String) As SqlCommand

    End Function
    Public Function MiconexionMaster() As SqlConnection

    End Function

    Public Function Miconexion() As SqlConnection

    End Function
End Class
