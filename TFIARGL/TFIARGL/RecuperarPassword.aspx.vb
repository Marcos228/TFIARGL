Imports System.Security.Cryptography
Imports System.Web.HttpContext
Public Class RecuperarPassword
    Inherits System.Web.UI.Page


    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnpass.Click

        Dim GestorCliente As New Negocio.UsuarioBLL
        Dim usu As New Entidades.UsuarioEntidad
        EnviarMail(usu, "hola")
        'Try
        '    If Page.IsValid = True Then
        '        Dim newPass As String = GenerarPassword()
        '        Dim PassSalt As List(Of String) = Negocio.EncriptarBLL.EncriptarPassword(newPass)
        '        usu.Salt = PassSalt.Item(0)
        '        usu.Password = PassSalt.Item(1)
        '        If GestorCliente.CambiarPassword(usu) Then
        '            Me.success.Visible = True
        '            Me.alertvalid.Visible = False
        '        Else
        '            Me.alertvalid.Visible = True
        '            Me.textovalid.InnerText = "El Usuario no existe. Recuerde que debe ingresar el mail con el que se registró"
        '            Me.success.Visible = False
        '        End If
        '        EnviarMail(usu, newPass)
        '    Else
        '        Me.alertvalid.Visible = True
        '        Me.textovalid.InnerText = "Complete los campos requeridos"
        '        Me.success.Visible = False
        '    End If

        'Catch ex As Exception

        'End Try


        'Dim GestorCliente As New BLL.ClienteBLL
        'Dim Cliente As New Entidades.UsuarioEntidad
        'Try
        '    If Page.IsValid = True Then
        '        Cliente.Contrasenia = txtPassword.Text
        '        Cliente.Cuil = txtcuil.Text
        '        Cliente.NombreUsuario = txtUsuario.Text
        '        If GestorCliente.CambiarPass(Cliente) Then
        '            Me.success.Visible = True
        '            Me.alertvalid.Visible = False
        '        Else
        '            Me.alertvalid.Visible = True
        '            Me.textovalid.InnerText = "El Usuario no existe o el Cuil ingresado es incorrecto"
        '            Me.success.Visible = False
        '        End If
        '    Else
        '        Me.alertvalid.Visible = True
        '        Me.textovalid.InnerText = "Complete los campos requeridos"
        '        Me.success.Visible = False
        '    End If

        'Catch ex As Exception
        'End Try
    End Sub
    Private Sub EnviarMail(ByRef usu As Entidades.UsuarioEntidad, ByVal newpass As String)
        Dim body As String = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("EmailTemplates/index.html"))
        Negocio.MailingBLL.enviarMailRegistroUsuario(usu, newpass, body)
    End Sub

    Private Function GenerarPassword() As Integer
        Dim byte_count As Byte() = New Byte(6) {}
        Dim random_number As New RNGCryptoServiceProvider()
        random_number.GetBytes(byte_count)
        Return Math.Abs(BitConverter.ToInt32(byte_count, 0)).ToString
    End Function
End Class