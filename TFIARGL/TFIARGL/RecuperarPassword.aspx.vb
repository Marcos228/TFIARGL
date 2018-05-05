Imports System.Security.Cryptography
Imports System.Web.HttpContext
Public Class RecuperarPassword
    Inherits System.Web.UI.Page


    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnpass.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            If Page.IsValid = True Then
                Dim GestorCliente As New Negocio.UsuarioBLL
                Dim usu As New Entidades.UsuarioEntidad With {.NombreUsu = txtUsuario.Text}
                EnviarMail(GestorCliente.CreateToken(usu))
                Me.success.Visible = True
                Me.alertvalid.Visible = False
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub EnviarMail(ByRef token As String)
        Dim body As String = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("EmailTemplates/Recupero.html"))
        Dim ruta As String = HttpContext.Current.Server.MapPath("Imagenes")
        Dim ur As Uri = Request.Url
        Negocio.MailingBLL.enviarMailRecupero(token, body, ruta, Replace(ur.AbsoluteUri, ur.AbsolutePath, ""))
    End Sub

End Class