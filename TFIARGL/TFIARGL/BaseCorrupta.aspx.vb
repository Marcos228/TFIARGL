Imports System.Web.HttpContext
Public Class BaseCorrupta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each corruptedrow As Entidades.FilaCorrupta In Global_asax.Corrupted
            Me.FilasCorruptas.Text += "</br>" + "ID de la Fila Corrupta: " + corruptedrow.ID + " Tabla de Fila Corrupta: " + corruptedrow.NombreTabla
        Next
        Global_asax.Corrupted.Clear()
    End Sub

    Protected Sub btnReparar_Click(sender As Object, e As EventArgs) Handles btnReparar.Click
        Response.Redirect("/Restore.aspx")
    End Sub
End Class