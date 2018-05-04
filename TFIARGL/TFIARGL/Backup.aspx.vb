Imports System.IO
Imports System.Web.HttpContext
Public Class BackUp
    Inherits System.Web.UI.Page
    Private nombreArchivo As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub hacerBackup(sender As Object, e As EventArgs) Handles BtnBackup.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            Current.Session("FilasCorruptas") = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
            If (Current.Session("FilasCorruptas").Count > 0) Then
                Current.Session("cliente") = DBNull.Value
                Response.Redirect("/BaseCorrupta.aspx", False)
            End If

            Dim gestorBK As New Negocio.BackupRestoreBLL
            nombreArchivo = "BKP_ArgLeague_" & Now.Year & "-" & Now.Month & "-" & Now.Day & " " & Now.Hour & ";" & Now.Minute & ";" & Now.Second
            If gestorBK.CrearBackup("", nombreArchivo, Current.Session("cliente")) Then
                System.IO.File.Encrypt(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\" & nombreArchivo & ".bak")
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraBackupSuccess").Traduccion, Entidades.Tipo_Bitacora.Backup, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End If
            ofrecerDownloadAlUsuario()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ofrecerDownloadAlUsuario()
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" & nombreArchivo + ".bak")
        Response.WriteFile(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() + "/" + nombreArchivo + ".bak")
        Response.Flush()
    End Sub
End Class