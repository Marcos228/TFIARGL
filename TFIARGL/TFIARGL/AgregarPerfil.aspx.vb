﻿Imports System.Web.HttpContext
Public Class AgregarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                TreeView1.Attributes.Add("onclick", "postBackByObject()")
                ControladorPermisos.CargarPermisos(Me.TreeView1)
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try

        End If
    End Sub

    Private Sub TreeView1_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView1.TreeNodeCheckChanged
        Try
            ControladorPermisos.CheckChildNodes(e.Node)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            Dim Perfil As New Entidades.PermisoCompuestoEntidad
            Perfil.Nombre = txtnombre.Text
            Perfil = ControladorPermisos.RecorrerArbol(Nothing, Perfil, TreeView1)
            If Perfil.Hijos.Count > 0 Then
                Dim GestorPermisos As New Negocio.GestorPermisosBLL
                If GestorPermisos.Alta(Perfil) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se creó el perfil " & Perfil.Nombre & " de forma correcta.", Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    txtnombre.Text = ""
                    alertvalid.Visible = False
                    success.Visible = True
                Else
                    alertvalid.InnerText = "El nombre del perfil ya se encuentra en uso, por favor ingrese uno distinto."
                    alertvalid.Visible = True
                    success.Visible = False
                End If

            Else
                alertvalid.InnerText = "Debe seleccionar al menos un permiso para continuar."
                alertvalid.Visible = True
                success.Visible = False
                'MessageBox.Show("Debe seleccionar al menos un permiso para continuar.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'MessageBox.Show(Traductor.TraducirMensaje("Mensaje_38"), Traductor.TraducirMensaje("Titulo_03"), MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class