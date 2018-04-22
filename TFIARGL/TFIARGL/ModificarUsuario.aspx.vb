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
            Try
                Ocultamiento(False)
                CargarUsuarios()
                CargarPerfiles()
                CargarIdiomas()
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
        Try
            Dim ddl As DropDownList = CType(gv_Usuarios.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Usuarios.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Usuarios.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            For Each item As ListItem In ddlpage.Items
                If item.Value = gv_Usuarios.PageSize Then
                    item.Selected = True
                End If
            Next

            txttotal.Text = gv_Usuarios.PageCount
            For cnt As Integer = 0 To gv_Usuarios.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Usuarios.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
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
            gv_Usuarios.BottomPagerRow.Visible = True
            gv_Usuarios.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub ModificarUsuario_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            gv_Usuarios.HeaderRow.TableSection = TableRowSection.TableHeader
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub Ocultamiento(ByVal bi As Boolean)
        Me.usuariot.Visible = bi
        Me.perfilt.Visible = bi
        Me.idiomat.Visible = bi
        Me.botont.Visible = bi
    End Sub

    Private Sub gv_Usuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Usuarios.RowCommand
        Try
            Ocultamiento(False)
            Dim gestor As New Negocio.UsuarioBLL
            Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(e.CommandArgument + (gv_Usuarios.PageIndex * gv_Usuarios.PageSize))
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
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try
            Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(Me.id_usuario.Value)
            Dim UsuarioAnterior As Entidades.UsuarioEntidad = Usuario.Clone
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
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub gv_Usuarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarUsuarios()
            gv_Usuarios.PageIndex = e.NewPageIndex
            gv_Usuarios.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Usuarios.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Usuarios.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Usuarios.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Usuarios.PageSize = ddl.SelectedValue
            CargarUsuarios()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

End Class