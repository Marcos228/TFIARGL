
Imports System.Security.Cryptography
Imports System.Text
Imports Negocio
Imports Entidades
Public Class EncriptarBLL

    Public Shared Function EncriptarPassword(ByVal Pass As String) As String
        Try
            Dim MiMD5 As MD5 = MD5CryptoServiceProvider.Create()
            Dim MiData As Byte() = MiMD5.ComputeHash(Encoding.Default.GetBytes(Pass))
            Dim MiStringBuilder As StringBuilder = New StringBuilder()
            For i As Integer = 0 To MiData.Length - 1
                MiStringBuilder.AppendFormat("{0:x2}", MiData(i))
            Next
            Return MiStringBuilder.ToString.ToUpper
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function
End Class
