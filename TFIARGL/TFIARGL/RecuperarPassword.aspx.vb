Imports System.Web.HttpContext
Public Class RecuperarPassword
    Inherits System.Web.UI.Page


    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnpass.Click
        'Current.Session("FilasCorruptas") = BLL.DigitoVerificadorBLL.VerifyAllIntegrity()
        'If (Current.Session("FilasCorruptas").Count > 0) Then
        '    Current.Session("cliente") = DBNull.Value
        '    Response.Redirect("/BaseCorrupta.aspx")
        'End If

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
End Class