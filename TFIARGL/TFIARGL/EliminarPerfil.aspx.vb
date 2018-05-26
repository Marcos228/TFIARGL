Imports System.Web.HttpContext
Public Class EliminarPerfil
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
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

    Private Sub TreeView1_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles TreeView1.TreeNodeCheckChanged
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
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
            ControladorPermisos.CargarPermisos(Me.TreeView1, Roles(lstperfil.SelectedIndex))
            Me.TreeView1.ExpandAll()

            Dim Gestor As New Negocio.UsuarioBLL
            Dim Lista As List(Of Entidades.UsuarioEntidad) = Gestor.TraerUsuariosPerfil(Roles(lstperfil.SelectedIndex).ID_Permiso)
            If Lista.Count = 0 Then
                Lista.Add(New Entidades.UsuarioEntidad With {.NombreUsu = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "UsuariosPerfil404").Traduccion})
                Me.gv_Perfiles.DataSource = Lista
                Me.gv_Perfiles.DataBind()
                Dim idiomabitacora As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    idiomabitacora = Application("Español")
                Else
                    idiomabitacora = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
                End If
                gv_Perfiles.HeaderRow.Cells(0).Text = idiomabitacora.Palabras.Find(Function(p) p.Codigo = "HeaderUsuariosSeleccionados").Traduccion
            Else
                Me.gv_Perfiles.DataSource = Lista
                Me.gv_Perfiles.DataBind()
                Dim idiomabitacora As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    idiomabitacora = Application("Español")
                Else
                    idiomabitacora = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
                End If
                gv_Perfiles.HeaderRow.Cells(0).Text = idiomabitacora.Palabras.Find(Function(p) p.Codigo = "HeaderUsuariosSeleccionados").Traduccion
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btneliminarPerfil_Click(sender As Object, e As EventArgs) Handles btneliminarPerfil.Click
        Try
            Dim gestorpermisos As New Negocio.GestorPermisosBLL
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Roles As List(Of Entidades.PermisoBaseEntidad) = TryCast(Session("Roles"), List(Of Entidades.PermisoBaseEntidad))
            If Not gestorpermisos.Baja(Roles(lstperfil.SelectedIndex)) Then
                alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "DeletePerfilError1").Traduccion
                alertvalid.Visible = True
                success.Visible = False
            Else
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraDeletePefilSuccess1").Traduccion & Roles(lstperfil.SelectedIndex).Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)

                alertvalid.Visible = False
                success.Visible = True
                CargarPerfiles()
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class