Imports System.Globalization
Imports System.Web.HttpContext
Public Class ConfirmarRegistracion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim GestorCliente As New Negocio.UsuarioBLL
            If Not GestorCliente.ACtivarUsuario(Request.QueryString("tok")) Then
                Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ConfirmarRegSuccess").Traduccion
                Me.success.Visible = True
                Me.alertvalid.Visible = False
            Else
                Me.alertvalid.Visible = True
                Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ConfirmarRegError").Traduccion
                Me.success.Visible = False
            End If
            GestorCliente.LimpiarTokens(Request.QueryString("tok"))
        Catch ex As Exception

        End Try
    End Sub
End Class