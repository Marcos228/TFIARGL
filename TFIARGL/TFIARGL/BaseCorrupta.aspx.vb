Imports System.Web.HttpContext
Public Class BaseCorrupta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            For Each corruptedrow As Entidades.FilaCorrupta In Global_asax.Corrupted
                Me.FilasCorruptas.Text += "</br>" + "ID de la Fila Corrupta: " + corruptedrow.ID + " Tabla de Fila Corrupta: " + corruptedrow.NombreTabla
            Next
            Global_asax.Corrupted.Clear()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Protected Sub btnReparar_Click(sender As Object, e As EventArgs) Handles btnReparar.Click
        Response.Redirect("/Restore.aspx")
    End Sub
End Class