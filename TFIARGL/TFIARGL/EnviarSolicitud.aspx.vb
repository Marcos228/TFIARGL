Imports System.IO
Imports System.Web.HttpContext
Public Class EnviarSolicitud
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.Datos.Visible = False
            Me.datos2.Visible = False
        End If
        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Request.QueryString("game")) Then
                If IsNumeric(Request.QueryString("game")) Then
                    CargarCombos(CInt(Request.QueryString("game")))
                Else
                    CargarJuegos()
                End If
            Else
                CargarJuegos()
            End If
        End If
    End Sub
    Private Sub CargarSolicitudes()
        Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)

        Dim gestoj As New Negocio.JugadorBLL
        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
        Dim solicitudes As List(Of Entidades.Solicitudes) = gestoj.TraeSolicitudesJugador(clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = lstgame.SelectedValue))
        If solicitudes.Count > 0 Then
            Session("Solicitudes") = solicitudes
            Me.gv_Solicitudes.DataSource = solicitudes
            Me.gv_Solicitudes.DataBind()
        Else
            Me.warningalert.Visible = True
            Me.textowarning.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvSolicitudError3").Traduccion
        End If

        Me.datos2.Visible = True
    End Sub
    Private Sub CargarCombos(ByVal ID_Juego As Integer)
        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
        Dim Games As New List(Of Entidades.Game)
        Games.Add(clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = ID_Juego).Game)
        Me.lstgame.DataSource = Games
        Me.lstgame.DataBind()
        Me.lstgame.SelectedIndex = 0
        Me.Datos.Visible = True
    End Sub

    Private Sub CargarJuegos()
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            Dim GestorJuegos As New Negocio.GameBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Juegos As List(Of Entidades.Game) = GestorJuegos.TraerJuegosAltaEquipo(clienteLogeado)

            If Juegos.Count = 1 Then
                Response.Redirect("/EnviarSolicitud.aspx" & "?game=" & Juegos(0).ID_Game, False)
            ElseIf Juegos.Count = 0 Then
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvSolicitudError1").Traduccion
                Me.success.Visible = False
            End If

            For Each Game In Juegos
                Dim base64string As String = Convert.ToBase64String(Game.Imagen, 0, Game.Imagen.Length)
                Dim ImgBut As New ImageButton()

                ImgBut.ImageUrl = Convert.ToString("data:image/jpg;base64,") & base64string
                ImgBut.ID = "Logo" & Game.Nombre
                ImgBut.Height = 150
                ImgBut.CssClass = "img-responsive"
                ImgBut.ImageAlign = ImageAlign.Middle

                ImgBut.PostBackUrl = "/EnviarSolicitud.aspx" & "?game=" & Game.ID_Game
                Dim div As HtmlGenericControl = New HtmlGenericControl("div")
                If Juegos.IndexOf(Game) Mod 2 = 0 Then
                    div.Attributes.Add("class", "col-md-5 col-md-offset-1 media")
                Else
                    div.Attributes.Add("class", "col-md-5 media")
                End If
                div.Controls.Add(ImgBut)
                Panel.Controls.Add(div)
            Next
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_Equipos_DataBound(sender As Object, e As EventArgs) Handles gv_Equipos.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_Equipos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Equipos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Equipos.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Equipos.PageSize).Selected = True

            txttotal.Text = gv_Equipos.PageCount

            For cnt As Integer = 0 To gv_Equipos.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Equipos.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            For Each row As GridViewRow In gv_Equipos.Rows
                Dim imagen1 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_envio"), System.Web.UI.WebControls.ImageButton)
                imagen1.CommandArgument = row.RowIndex
            Next

            With gv_Equipos.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaAlta").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderHistoria").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_Equipos.BottomPagerRow.Visible = True
            gv_Equipos.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_solicitudes_DataBound(sender As Object, e As EventArgs) Handles gv_Solicitudes.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_Solicitudes.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Solicitudes.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Solicitudes.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Solicitudes.PageSize).Selected = True

            txttotal.Text = gv_Solicitudes.PageCount

            For cnt As Integer = 0 To gv_Solicitudes.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Solicitudes.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            For Each row As GridViewRow In gv_Solicitudes.Rows
                Dim imagen1 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_aceptar_solicitud"), System.Web.UI.WebControls.ImageButton)
                Dim imagen2 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_rechazar_solicitud"), System.Web.UI.WebControls.ImageButton)
                imagen1.CommandArgument = row.RowIndex
                imagen2.CommandArgument = row.RowIndex
            Next

            With gv_Solicitudes.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderHistoria").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderMensaje").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFecha").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_Solicitudes.BottomPagerRow.Visible = True
            gv_Solicitudes.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub


    Private Sub gv_Equipos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Equipos.RowCommand
        Try
            Select Case e.CommandName.ToString
                Case "E"
                    Session("Equipo") = TryCast(Session("Equipos"), List(Of Entidades.Equipo))(e.CommandArgument + (gv_Equipos.PageIndex * gv_Equipos.PageSize))
                    Session("Equipos") = Nothing
                    Response.Redirect("/EscribirSolicitud.aspx", False)
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub gv_Solicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Solicitudes.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Solicitud As Entidades.Solicitudes = TryCast(Session("Solicitudes"), List(Of Entidades.Solicitudes))(e.CommandArgument + (gv_Solicitudes.PageIndex * gv_Solicitudes.PageSize))
            Dim Gestorequi As New Negocio.EquipoBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Select Case e.CommandName.ToString
                Case "A"
                    If Gestorequi.AgregarJugador(Solicitud) Then
                        Dim Bitac As Entidades.BitacoraAuditoria
                        Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Solicitud.Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEnvSolicitudSuccess1").Traduccion & " " & Solicitud.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                        Negocio.BitacoraBLL.CrearBitacora(Bitac)
                        Me.success.Visible = True
                        Me.alertvalid.Visible = False
                        CargarSolicitudes()
                    Else
                        Me.alertvalid.Visible = True
                        Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvSolicitudError4").Traduccion
                        Me.success.Visible = False
                        CargarSolicitudes()
                    End If
                Case "R"
                    Gestorequi.RechazarSolicitud(Solicitud)
                    Dim Bitac As Entidades.BitacoraAuditoria
                    Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Solicitud.Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEnvSolicitudSuccess2").Traduccion & " " & Solicitud.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvSolicitudError5").Traduccion & " " & Solicitud.Equipo.Nombre
                    Me.alertvalid.Visible = False
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub gv_Solucitudes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarSolicitudes()
            gv_Solicitudes.PageIndex = e.NewPageIndex
            gv_Solicitudes.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Solicitudes.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Solicitudes.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Solicitudes.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Solicitudes.PageSize = ddl.SelectedValue
            CargarSolicitudes()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_Equipos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarEquipos()
            gv_Equipos.PageIndex = e.NewPageIndex
            gv_Equipos.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Equipos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Equipos.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Equipos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Equipos.PageSize = ddl.SelectedValue
            CargarEquipos()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub CargarEquipos()
        Dim IdiomaActual As Entidades.IdiomaEntidad
        If IsNothing(Current.Session("Cliente")) Then
            IdiomaActual = Application("Español")
        Else
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        End If

        Dim lista As List(Of Entidades.Equipo)
        Dim Gestor As New Negocio.EquipoBLL
        If txtnombre.Text <> "" Then
            lista = Gestor.TraerEquiposSolicitud(RTrim(LTrim(txtnombre.Text)), New Entidades.Game With {.ID_Game = lstgame.SelectedValue})
            If lista.Count > 0 Then
                Session("Equipos") = lista
                Me.gv_Equipos.DataSource = lista
                Me.gv_Equipos.DataBind()
            Else
                Me.alertvalid.Visible = True
                Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvSolicitudError2").Traduccion
                Me.success.Visible = False
            End If
        Else
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
            Me.success.Visible = False
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            CargarEquipos()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

End Class