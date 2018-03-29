Public Class Restore
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub RealizarRestore(sender As Object, e As EventArgs) Handles Button1.Click
        FileUpload1.SaveAs(Server.MapPath("restoreUpload"))
        Dim gestorBK As New Negocio.BackupRestoreBLL
        Dim bkre = New Entidades.BackupRestoreEntidad("")
        bkre.Nombre = Server.MapPath("restoreUpload")
        If gestorBK.RealizarRestore(bkre) Then
            Me.success.Visible = True
            Me.alertvalid.Visible = False
        End If
    End Sub
End Class