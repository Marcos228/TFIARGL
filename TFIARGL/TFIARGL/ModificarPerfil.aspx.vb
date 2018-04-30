Imports System.Web.HttpContext
Public Class ModificarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                TreeView2.Attributes.Add("onclick", "postBackByObject()")
                CargarPerfiles()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try

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
        Try
            ControladorPermisos.CheckChildNodes(e.Node)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub



    Private Sub lstperfil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstperfil.SelectedIndexChanged
        Try
            Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
            ControladorPermisos.CargarPermisos(Me.TreeView1, Roles(lstperfil.SelectedIndex))
            ControladorPermisos.CargarPermisos(Me.TreeView2)
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
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try


    End Sub

    Protected Sub btnmodificarperfil_Click(sender As Object, e As EventArgs) Handles btnmodificarperfil.Click
        Try
            Dim Perfil As Entidades.PermisoCompuestoEntidad = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))(lstperfil.SelectedIndex)
            Dim PerfilAnterior As Entidades.PermisoCompuestoEntidad = Perfil.Clone
            Perfil.Hijos.Clear()
            Perfil = ControladorPermisos.RecorrerArbol(Nothing, Perfil, TreeView2)
            If Perfil.Hijos.Count <> 0 Then
                Dim GestorPermisos As New Negocio.GestorPermisosBLL
                GestorPermisos.Modificar(Perfil)
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se modificó el perfil " & Perfil.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac, PerfilAnterior, Perfil)


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

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

End Class