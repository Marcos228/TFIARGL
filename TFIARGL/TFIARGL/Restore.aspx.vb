Imports System.IO
Imports System.Web.HttpContext
Public Class Restore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarBackups()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try

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
        Restore(bkre, True)
        AssumingDirectControl()
    End Sub
    Private Sub Restore(ByRef Bkre As Entidades.BackupRestoreEntidad, ByVal Servidor As Boolean)
        Try
            Dim gestorBK As New Negocio.BackupRestoreBLL
            Dim nombreArchivo As String = "BKP_from_Restore_ArgLeague_" & Now.Year & "-" & Now.Month & "-" & Now.Day & " " & Now.Hour & ";" & Now.Minute & ";" & Now.Second
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            If gestorBK.CrearBackup("", nombreArchivo, Current.Session("cliente")) Then
                System.IO.File.Encrypt(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\" & nombreArchivo & ".bak")
            End If
            If gestorBK.RealizarRestore(Bkre) Then
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se realizó una restauracion de la base de datos.", Entidades.Tipo_Bitacora.Restore, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
                Me.success.Visible = True
                Me.alertvalid.Visible = False
            End If
            If Servidor Then
                File.Encrypt(Me.Backups.SelectedValue)
            Else
                System.IO.File.Delete((System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload"))
            End If
            AssumingDirectControl()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub AssumingDirectControl()
        Current.Session.RemoveAll()
        Current.Session.Abandon()
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnlocal_Click(sender As Object, e As EventArgs) Handles btnlocal.Click
        FileUpload1.SaveAs(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload")
        File.Decrypt(System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload")
        Dim bkre = New Entidades.BackupRestoreEntidad("")
        bkre.Nombre = System.Web.Configuration.WebConfigurationManager.AppSettings("RutaBackup").ToString() & "\restoreUpload"
        Restore(bkre, False)
    End Sub
End Class