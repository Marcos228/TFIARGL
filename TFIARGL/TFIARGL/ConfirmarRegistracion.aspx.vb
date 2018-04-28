Imports System.Globalization
Imports System.Web.HttpContext
Public Class ConfirmarRegistracion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim GestorCliente As New Negocio.UsuarioBLL
            If Not GestorCliente.ACtivarUsuario(Request.QueryString("tok")) Then
                Me.success.InnerText = "Se ha confirmado la creacion de su usuario. Dirijase al login para ingresar."
                Me.success.Visible = True
                Me.alertvalid.Visible = False
            Else
                Me.alertvalid.Visible = True
                Me.alertvalid.InnerText = "El periodo de activacion de cuenta expiró. Vuelva a la registracion de usuario."
                Me.success.Visible = False
            End If
            GestorCliente.LimpiarTokens(Request.QueryString("tok"))
        Catch ex As Exception

        End Try
    End Sub
End Class