Imports System.Web.HttpContext
Public Class ModificarUsuario
    Inherits System.Web.UI.Page

    Private Sub CargarUsuarios()
        Dim lista As List(Of Entidades.UsuarioEntidad)
        Dim Gestor As New Negocio.UsuarioBLL
        lista = Gestor.TraerUsuariosParaBloqueo(Current.Session("cliente"))
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

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Usuarios.PageSize).Selected = True

            txttotal.Text = gv_Usuarios.PageCount

            For cnt As Integer = 0 To gv_Usuarios.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Usuarios.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            For Each row As GridViewRow In gv_Usuarios.Rows
                Dim imagen1 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_Bloquear"), System.Web.UI.WebControls.ImageButton)
                Dim imagen2 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_desbloqueo"), System.Web.UI.WebControls.ImageButton)
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_editar"), System.Web.UI.WebControls.ImageButton)

                imagen1.CommandArgument = row.RowIndex
                imagen2.CommandArgument = row.RowIndex
                imagen3.CommandArgument = row.RowIndex


                If row.Cells(4).Text = "False" Then
                    row.Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MsjNoBloqueado").Traduccion
                Else
                    row.Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MsjBloqueado").Traduccion
                End If
                If row.Cells(5).Text = "False" Then
                    row.Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MsjNo").Traduccion
                    imagen3.Visible = False
                Else
                    row.Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MsjSi").Traduccion
                End If
            Next

            With gv_Usuarios.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderID").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombreUsuario").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderIdioma").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPermiso").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderBloqueo").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEmpleado").Traduccion
                .Cells(6).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaAlta").Traduccion
                .Cells(7).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

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
        Me.botont2.Visible = bi
    End Sub

    Private Sub gv_Usuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Usuarios.RowCommand
        Try
            Ocultamiento(False)
            Dim gestor As New Negocio.UsuarioBLL
            Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(e.CommandArgument + (gv_Usuarios.PageIndex * gv_Usuarios.PageSize))
            Me.id_usuario.Value = e.CommandArgument
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            Select Case e.CommandName.ToString
                Case "B"
                    If Usuario.Bloqueo = True Then
                        Me.alertvalid.Visible = True
                        Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ModUserError1").Traduccion
                        Me.success.Visible = False
                    Else
                        gestor.Bloquear(Usuario)
                        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                        Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraModUserSuccess1").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                        Negocio.BitacoraBLL.CrearBitacora(Bitac)

                        CargarUsuarios()
                        Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ModUserSuccess1").Traduccion
                        Me.success.Visible = True
                        Me.alertvalid.Visible = False
                    End If
                Case "U"
                    If Usuario.Bloqueo = False Then
                        Me.alertvalid.Visible = True
                        Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ModUserError2").Traduccion
                        Me.success.Visible = False
                    Else
                        gestor.Bloquear(Usuario)
                        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                        Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraModUserSuccess2").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                        Negocio.BitacoraBLL.CrearBitacora(Bitac)
                        CargarUsuarios()
                        Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ModUserSuccess2").Traduccion
                        Me.success.Visible = True
                        Me.alertvalid.Visible = False
                    End If

                Case "E"
                    txtusuario.Text = Usuario.NombreUsu
                    lstidioma.ClearSelection()
                    lstperfil.ClearSelection()

                    lstidioma.Items.FindByValue(Usuario.Idioma.ID_Idioma).Selected = True

                    lstperfil.Items.FindByValue(Usuario.Perfil.ID_Permiso).Selected = True

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
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            If Page.IsValid = True Then
                Usuario.NombreUsu = txtusuario.Text
                Usuario.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = lstidioma.SelectedValue}
                Usuario.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = lstperfil.SelectedValue}
                If GestorCliente.Modificar(Usuario) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraModUserSuccess3").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac, UsuarioAnterior, Usuario)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    CargarUsuarios()
                    Ocultamiento(False)
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
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

    Protected Sub btneliminar_Click(sender As Object, e As EventArgs) Handles btneliminar.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try
            Dim Usuario As Entidades.UsuarioEntidad = TryCast(Session("Usuarios"), List(Of Entidades.UsuarioEntidad))(Me.id_usuario.Value)
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            If GestorCliente.Eliminar(Usuario) Then
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraDelUserSuccess").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
                Me.success.Visible = True
                Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "DelUserSuccess").Traduccion
                Me.alertvalid.Visible = False
                CargarUsuarios()
                Ocultamiento(False)
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class