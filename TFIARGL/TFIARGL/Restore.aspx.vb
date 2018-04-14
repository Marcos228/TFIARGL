Imports System.Web.HttpContext
Public Class Restore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub RealizarRestore(sender As Object, e As EventArgs) Handles Button1.Click
        FileUpload1.SaveAs(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload")
        Dim gestorBK As New Negocio.BackupRestoreBLL
        Dim bkre = New Entidades.BackupRestoreEntidad("")
        bkre.Nombre = System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload"
        If gestorBK.RealizarRestore(bkre) Then
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se realizó una restauracion de la base de datos.", Entidades.Tipo_Bitacora.Restore, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
            Me.success.Visible = True
            Me.alertvalid.Visible = False
        End If
    End Sub
End Class