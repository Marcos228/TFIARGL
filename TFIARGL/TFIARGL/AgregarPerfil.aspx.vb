Imports System.Web.HttpContext
Public Class AgregarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            TreeView1.Attributes.Add("onclick", "postBackByObject()")
            ControladorPermisos.CargarPermisos(Me.TreeView1)
        End If
    End Sub



    Private Sub TreeView1_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView1.TreeNodeCheckChanged
        ControladorPermisos.CheckChildNodes(e.Node)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim Perfil As New Entidades.PermisoCompuestoEntidad
        Perfil.Nombre = txtnombre.Text
        Perfil = ControladorPermisos.RecorrerArbol(Nothing, Perfil, TreeView1)
        If Perfil.Hijos.Count <> 0 Then
            Dim GestorPermisos As New Negocio.GestorPermisosBLL
            If GestorPermisos.Alta(Perfil) Then
                'MessageBox.Show("Se Creó el Perfil de manera satisfactoria.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '  MessageBox.Show(Traductor.TraducirMensaje("Mensaje_37"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
                '     ControladorPermisos.CargarPermisos(Tree)
                txtnombre.Text = ""
                alertvalid.Visible = False
                success.Visible = True
            Else
                alertvalid.InnerText = "El nombre del perfil ya se encuentra en uso, por favor ingrese uno distinto."
                alertvalid.Visible = True
                success.Visible = False
            End If

        Else
            'MessageBox.Show("Debe seleccionar al menos un permiso para continuar.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MessageBox.Show(Traductor.TraducirMensaje("Mensaje_38"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


End Class