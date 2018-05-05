Imports System.Web.HttpContext
Public Class BaseCorrupta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            For Each corruptedrow As Entidades.FilaCorrupta In Application("Corruption")
                Me.FilasCorruptas.Text += "</br>" + "ID: " + corruptedrow.ID + IdiomaActual.Palabras.Find(Function(p) p.Codigo = "TablaCorrupta1").Traduccion + corruptedrow.NombreTabla
            Next
            Global_asax.Corrupted.Clear()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Protected Sub btnReparar_Click(sender As Object, e As EventArgs) Handles btnReparar.Click
        Response.Redirect("/Restore.aspx", False)
    End Sub
End Class