Imports System.Web.HttpContext
Public Class ModificarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            TreeView2.Attributes.Add("onclick", "postBackByObject()")
            CargarPerfiles()
        End If
    End Sub

    Private Sub CargarPerfiles()
        Dim lista As List(Of Entidades.PermisoBaseEntidad)
        Dim Gestor As New Negocio.GestorPermisosBLL
        lista = Gestor.ListarFamilias(True)
        Session("Roles") = lista
        Me.lstperfil.DataSource = lista
        Me.lstperfil.DataBind()
        Me.lstperfil.SelectedIndex = 0
        Me.lstperfil_SelectedIndexChanged(Nothing, Nothing)
    End Sub


    Private Sub TreeView2_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView2.TreeNodeCheckChanged
        ControladorPermisos.CheckChildNodes(e.Node)
    End Sub



    Private Sub lstperfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstperfil.SelectedIndexChanged
        Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
        ControladorPermisos.CargarPermisos(Me.TreeView1, Roles(lstperfil.SelectedIndex))
        ControladorPermisos.CargarPermisos(Me.TreeView2)
        Me.TreeView1.ExpandAll()
        Dim Gestor As New Negocio.UsuarioBLL
        Dim Lista As List(Of Entidades.UsuarioEntidad) = Gestor.TraerUsuariosPerfil(Roles(lstperfil.SelectedIndex).ID)
        If Lista.Count = 0 Then
            Lista.Add(New Entidades.UsuarioEntidad With {.NombreUsu = "No se encontraron usuarios con el perfil Seleccionado"})
            Me.gv_Perfiles.DataSource = Lista
            Me.gv_Perfiles.DataBind()
        Else
            Me.gv_Perfiles.DataSource = Lista
            Me.gv_Perfiles.DataBind()
        End If

    End Sub

    Protected Sub btnmodificar_Click(sender As Object, e As EventArgs) Handles btnmodificar.Click
        Dim Perfil As Entidades.PermisoCompuestoEntidad = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))(lstperfil.SelectedIndex)
        Perfil.Hijos.Clear()
        Perfil = ControladorPermisos.RecorrerArbol(Nothing, Perfil, TreeView2)
        If Perfil.Hijos.Count <> 0 Then
            Dim GestorPermisos As New Negocio.GestorPermisosBLL
            GestorPermisos.Modificar(Perfil)
            'MessageBox.Show("Se Creó el Perfil de manera satisfactoria.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '  MessageBox.Show(Traductor.TraducirMensaje("Mensaje_37"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
            '     ControladorPermisos.CargarPermisos(Tree)
            alertvalid.Visible = False
            success.Visible = True

        Else
            alertvalid.InnerText = "Debe seleccionar al menos un permiso, que no sea el mismo a modificar, para continuar."
            alertvalid.Visible = True
            success.Visible = False
            'MessageBox.Show("Debe seleccionar al menos un permiso para continuar.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MessageBox.Show(Traductor.TraducirMensaje("Mensaje_38"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        CargarPerfiles()

    End Sub

End Class