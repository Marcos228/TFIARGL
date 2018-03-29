
Imports System.Security.Cryptography
Imports System.Text
Imports Entidades
Public Class EncriptarBLL

    Public Shared Function EncriptarPassword(ByVal pass As String, Optional ByVal salt As String = Nothing) As List(Of String)
        Try
            Dim Listaretorno As New List(Of String)
            If IsNothing(salt) Then
                Dim byte_count As Byte() = New Byte(6) {}
                Dim random_number As New RNGCryptoServiceProvider()
                random_number.GetBytes(byte_count)
                salt = Math.Abs(BitConverter.ToInt32(byte_count, 0)).ToString
                Listaretorno.Add(salt)
            End If


            Dim UE As New UnicodeEncoding
            Dim bHash As Byte()
            Dim bCadena() As Byte = UE.GetBytes(Left(salt, salt.Length - 4) & pass & Right(salt, salt.Length - (salt.Length - 4)))
            Dim s1Service As New SHA1CryptoServiceProvider
            bHash = s1Service.ComputeHash(bCadena)
            Listaretorno.Add(Convert.ToBase64String(bHash))

            Return Listaretorno

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
