﻿Imports System.Web.HttpContext
Public Class EliminarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
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

    Private Sub TreeView1_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView1.TreeNodeCheckChanged
        ControladorPermisos.CheckChildNodes(e.Node)
    End Sub


    Private Sub lstperfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstperfil.SelectedIndexChanged
        Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
        ControladorPermisos.CargarPermisos(Me.TreeView1, Roles(lstperfil.SelectedIndex))
        Me.TreeView1.ExpandAll()

        Dim Gestor As New Negocio.UsuarioBLL
        Dim Lista As List(Of Entidades.UsuarioEntidad) = Gestor.TraerUsuariosPerfil(Roles(lstperfil.SelectedIndex).ID_Permiso)
        If Lista.Count = 0 Then
            Lista.Add(New Entidades.UsuarioEntidad With {.NombreUsu = "No se encontraron usuarios con el perfil Seleccionado"})
            Me.gv_Perfiles.DataSource = Lista
            Me.gv_Perfiles.DataBind()
        Else
            Me.gv_Perfiles.DataSource = Lista
            Me.gv_Perfiles.DataBind()
        End If

    End Sub

    Protected Sub btneliminar_Click(sender As Object, e As EventArgs) Handles btneliminar.Click
        Dim gestorpermisos As New Negocio.GestorPermisosBLL
        Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
        If Not gestorpermisos.Baja(Roles(lstperfil.SelectedIndex)) Then
            alertvalid.InnerText = "No se puede eliminar el perfil debido a que tiene un usuario asociado"
            alertvalid.Visible = True
            success.Visible = False
        Else
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se eliminó el perfil " & Roles(lstperfil.SelectedIndex).Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            Negocio.BitacoraBLL.CrearBitacora(Bitac)

            alertvalid.Visible = False
            success.Visible = True
            CargarPerfiles()
        End If
    End Sub
End Class