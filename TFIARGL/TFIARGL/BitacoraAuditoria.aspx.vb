Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Public Class ConsultarBitacoraAuditoria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Current.Session("FilasCorruptas") = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
        If (Current.Session("FilasCorruptas").Count > 0) Then
            Current.Session("cliente") = DBNull.Value
            Response.Redirect("/BaseCorrupta.aspx")
        End If
        CargarBitacoras()
    End Sub

    Private Sub CargarBitacoras()
        Dim lista As List(Of Entidades.Bitacora)
        Dim Gestor As New Negocio.BitacoraBLL
        lista = Gestor.listar
        Me.gv_Bitacora.DataSource = lista
        Me.gv_Bitacora.DataBind()
    End Sub


    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click

    End Sub
End Class