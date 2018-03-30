Imports System.Web.HttpContext
Public Class BackUp
    Inherits System.Web.UI.Page
    Private nombreArchivo As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub hacerBackup(sender As Object, e As EventArgs) Handles Button1.Click
        Current.Session("FilasCorruptas") = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
        If (Current.Session("FilasCorruptas").Count > 0) Then
            Current.Session("cliente") = DBNull.Value
            Response.Redirect("/BaseCorrupta.aspx")
        End If

        Dim gestorBK As New Negocio.BackupRestoreBLL
        nombreArchivo = "ArgLeague_" + Now.Ticks.ToString
        gestorBK.CrearBackup("", nombreArchivo, Current.Session("cliente"))
        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
        Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se creó un backup de forma correcta.", Entidades.Tipo_Bitacora.Backup, Now, Request.UserAgent, Request.UserHostAddress, "", "")
        Negocio.BitacoraBLL.CrearBitacora(Bitac)
        ofrecerDownloadAlUsuario()

    End Sub
    Protected Sub ofrecerDownloadAlUsuario()
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() + "/" + nombreArchivo + ".bak")
        Response.AppendHeader("Content-Disposition", "attachment; filename=backup" + Now.Year.ToString + Now.Month.ToString + Now.Day.ToString + ".bak")
        Response.Flush()
    End Sub
End Class