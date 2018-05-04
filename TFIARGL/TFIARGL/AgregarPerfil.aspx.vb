Imports System.Web.HttpContext
Public Class AgregarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                TreeView1.Attributes.Add("onclick", "postBackByObject()")
                ControladorPermisos.CargarPermisos(Me.TreeView1)
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try

        End If
    End Sub

    Private Sub TreeView1_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView1.TreeNodeCheckChanged
        Try
            ControladorPermisos.CheckChildNodes(e.Node)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub btnAddPerfil_Click(sender As Object, e As EventArgs) Handles btnAddPerfil.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            Dim Perfil As New Entidades.PermisoCompuestoEntidad
            Perfil.Nombre = txtnombre.Text
            Perfil = ControladorPermisos.RecorrerArbol(Nothing, Perfil, TreeView1)
            If Perfil.Hijos.Count > 0 Then
                Dim GestorPermisos As New Negocio.GestorPermisosBLL
                If GestorPermisos.Alta(Perfil) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")

                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddPerfilSuccess1").Traduccion & Perfil.Nombre & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    txtnombre.Text = ""
                    alertvalid.Visible = False
                    success.Visible = True
                Else
                    alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfilError1").Traduccion
                    alertvalid.Visible = True
                    success.Visible = False
                End If

            Else
                alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfilError2").Traduccion
                alertvalid.Visible = True
                success.Visible = False
                'MessageBox.Show("Debe seleccionar al menos un permiso para continuar.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'MessageBox.Show(Traductor.TraducirMensaje("Mensaje_38"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class