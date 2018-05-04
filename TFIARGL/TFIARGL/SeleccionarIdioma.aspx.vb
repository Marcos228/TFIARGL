Imports System.Globalization
Imports System.Web.HttpContext
Public Class SeleccionarIdioma
    Inherits System.Web.UI.Page

    Private Sub AgregarIdioma_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarIdiomas()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)

            End Try
        End If
    End Sub
    Private Sub CargarIdiomas()
        Dim lista As List(Of Entidades.IdiomaEntidad)
        Dim Gestor As New Negocio.IdiomaBLL
        lista = Gestor.ConsultarIdiomas()
        Me.lstidioma.DataSource = lista
        Me.lstidioma.DataBind()
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim GestorIdioma As New Negocio.IdiomaBLL
            If Not IsNothing(clienteLogeado) Then
                clienteLogeado.Idioma = GestorIdioma.SeleccionarIdioma(clienteLogeado, CInt(lstidioma.SelectedValue))
            Else
                Current.Session("Idioma") = GestorIdioma.ConsultarPorID(CInt(lstidioma.SelectedValue))
            End If
            Me.success.Visible = True
                Me.alertvalid.Visible = False
                Response.Redirect("/SeleccionarIdioma.aspx", False)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class