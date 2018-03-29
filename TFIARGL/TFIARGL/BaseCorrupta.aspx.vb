Imports System.Web.HttpContext
Public Class BaseCorrupta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each corruptedrow As Entidades.FilaCorrupta In Current.Session("FilasCorruptas")
            Me.FilasCorruptas.Text += "</br>" + "ID de la Fila Corrupta: " + corruptedrow.ID + " Tabla de Fila Corrupta:" + corruptedrow.NombreTabla
        Next
    End Sub

    Protected Sub btnReparar_Click(sender As Object, e As EventArgs) Handles btnReparar.Click
        Negocio.DigitoVerificadorBLL.RepareIntegrity()
        Response.Redirect("/ClientRegistration.aspx")
    End Sub
End Class