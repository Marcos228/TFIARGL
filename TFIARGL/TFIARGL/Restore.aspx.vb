Imports System.IO
Imports System.Web.HttpContext
Public Class Restore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarBackups()
        End If
    End Sub
    Private Sub CargarBackups()
        Dim Directory As New DirectoryInfo(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString())
        Dim fileinf As FileInfo() = Directory.GetFiles()
        Me.Backups.Items.Clear()

        For Each fri As FileInfo In fileinf
            Dim item As New ListItem
            item.Text = fri.Name
            item.Value = fri.FullName
            Me.Backups.Items.Add(item)
        Next fri
    End Sub

    Protected Sub btnserver_Click(sender As Object, e As EventArgs) Handles btnserver.Click
        Dim gestorBK As New Negocio.BackupRestoreBLL
        Dim bkre = New Entidades.BackupRestoreEntidad("")
        bkre.Nombre = Me.Backups.SelectedValue
        File.Decrypt(Me.Backups.SelectedValue)
        If gestorBK.RealizarRestore(bkre) Then
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se realizó una restauracion de la base de datos.", Entidades.Tipo_Bitacora.Restore, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
            Me.success.Visible = True
            Me.alertvalid.Visible = False
        End If
        File.Encrypt(Me.Backups.SelectedValue)
    End Sub

    Protected Sub btnlocal_Click(sender As Object, e As EventArgs) Handles btnlocal.Click
        File.
        FileUpload1.SaveAs(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload")
        File.Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload")
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
        System.IO.File.Delete((System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload"))
    End Sub
End Class