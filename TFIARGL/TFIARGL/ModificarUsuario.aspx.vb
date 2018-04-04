Imports System.Web.HttpContext
Public Class ModificarUsuario
    Inherits System.Web.UI.Page



    Private Sub CargarUsuarios()
        Dim lista As List(Of Entidades.UsuarioEntidad)
        Dim Gestor As New Negocio.UsuarioBLL
        lista = Gestor.TraerUsuariosParaBloqueo
        Session("Usuarios") = lista
        Me.gv_Usuarios.DataSource = lista
        Me.gv_Usuarios.DataBind()

    End Sub

    Private Sub AgregarUsuario_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Ocultamiento(False)
            CargarUsuarios()
            CargarPerfiles()
            CargarIdiomas()
        End If
    End Sub
    Private Sub CargarPerfiles()
        Dim lista As List(Of Entidades.PermisoBaseEntidad)
        Dim Gestor As New Negocio.GestorPermisosBLL
        lista = Gestor.ListarFamilias(True)
        Session("Roles") = lista
        Me.lstperfil.DataSource = lista
        Me.lstperfil.DataBind()
    End Sub

    Private Sub CargarIdiomas()
        Dim lista As List(Of Entidades.IdiomaEntidad)
        Dim Gestor As New Negocio.IdiomaBLL
        lista = Gestor.ConsultarIdiomas()
        Session("Idiomas") = lista
        Me.lstidioma.DataSource = lista
        Me.lstidioma.DataBind()
    End Sub

    Private Sub gv_Usuarios_DataBound(sender As Object, e As EventArgs) Handles gv_Usuarios.DataBound
        For Each row As GridViewRow In gv_Usuarios.Rows
            Dim imagen1 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_Bloquear"), System.Web.UI.WebControls.ImageButton)
            Dim imagen2 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_desbloqueo"), System.Web.UI.WebControls.ImageButton)
            Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_editar"), System.Web.UI.WebControls.ImageButton)

            imagen1.CommandArgument = row.RowIndex
            imagen2.CommandArgument = row.RowIndex
            imagen3.CommandArgument = row.RowIndex

            If row.Cells(4).Text = "False" Then
                row.Cells(4).Text = "No Bloqueado"
            Else
                row.Cells(4).Text = "Bloqueado"
            End If
            If row.Cells(5).Text = "False" Then
                row.Cells(5).Text = "NO"
                imagen3.Visible = False
            Else
                row.Cells(5).Text = "SI"
            End If

        Next
    End Sub

    Private Sub ModificarUsuario_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            gv_Usuarios.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Ocultamiento(ByVal bi As Boolean)
        Me.usuariot.Visible = bi
        Me.perfilt.Visible = bi
        Me.idiomat.Visible = bi
        Me.botont.Visible = bi
    End Sub

    Private Sub gv_Usuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Usuarios.RowCommand
        Ocultamiento(False)
        Dim gestor As New Negocio.UsuarioBLL
        Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(e.CommandArgument)
        Me.id_usuario.Value = e.CommandArgument
        Select Case e.CommandName.ToString
            Case "B"
                If Usuario.Bloqueo = True Then
                    Me.alertvalid.Visible = True
                    Me.alertvalid.InnerText = "El usuario ya se encuentra bloqueado."
                    Me.success.Visible = False
                Else
                    gestor.Bloquear(Usuario)
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se bloqueó el usuario " & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)

                    CargarUsuarios()
                        Me.success.InnerText = "El Usuario se bloqueo correctamente."
                        Me.success.Visible = True
                    Me.alertvalid.Visible = False
                End If
            Case "U"
                If Usuario.Bloqueo = False Then
                    Me.alertvalid.Visible = True
                    Me.alertvalid.InnerText = "El usuario no se encuentra bloqueado."
                    Me.success.Visible = False
                Else
                    gestor.Bloquear(Usuario)
                    'Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    'Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se desbloqueó el usuario " & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    'Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    CargarUsuarios()
                    Me.success.InnerText = "El Usuario se desbloqueo correctamente."
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                End If

            Case "E"
                txtusuario.Text = Usuario.NombreUsu
                lstidioma.ClearSelection()
                lstperfil.ClearSelection()
                For Each item As ListItem In lstidioma.Items
                    If item.Value = Usuario.Idioma.ID_Idioma Then
                        item.Selected = True
                        Exit For
                    End If
                Next

                For Each item As ListItem In lstperfil.Items
                    If item.Value = Usuario.Perfil.ID_Permiso Then
                        item.Selected = True
                        Exit For
                    End If
                Next
                Ocultamiento(True)
        End Select

    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(Me.id_usuario.Value)
        Dim UsuarioAnterior As Entidades.UsuarioEntidad = Usuario.Clone
        Try
            If Page.IsValid = True Then
                Usuario.NombreUsu = txtusuario.Text
                Usuario.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = lstidioma.SelectedValue}
                Usuario.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = lstperfil.SelectedValue}
                If GestorCliente.Modificar(Usuario) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se modificó el usuario " & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac, UsuarioAnterior, Usuario)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    CargarUsuarios()
                    Ocultamiento(False)
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = "Complete los campos requeridos"
                Me.success.Visible = False
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class