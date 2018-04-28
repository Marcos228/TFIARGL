Imports System.Security.Cryptography
Imports System.Web.HttpContext
Public Class ConfirmarRecupero
    Inherits System.Web.UI.Page


    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnpass.Click
        Try
            If Page.IsValid = True Then
                If txtpass.Value = txtpass2.Value Then
                    Dim GestorCliente As New Negocio.UsuarioBLL
                    Dim usu As New Entidades.UsuarioEntidad
                    Dim PassSalt As List(Of String) = Negocio.EncriptarBLL.EncriptarPassword(txtpass.Value)
                    usu.Salt = PassSalt.Item(0)
                    usu.Password = PassSalt.Item(1)

                    If GestorCliente.CambiarPassword(usu, Request.QueryString("tok")) Then
                        Me.success.Visible = True
                        Me.alertvalid.Visible = False
                        GestorCliente.LimpiarTokens(Request.QueryString("tok"))
                    Else
                        Me.alertvalid.Visible = True
                        Me.textovalid.InnerText = "El tiempo permitido para cambiar la contraseña ha expirado. Por favor vuelva a realizar todo el procedimiento."
                        Me.success.Visible = False
                    End If
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = "Las contraseñas ingresadas no son iguales."
                    Me.success.Visible = False
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

End Class