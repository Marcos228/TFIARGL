Imports System.IO
Imports System.Web.HttpContext
Public Class EnviarInvitacion
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
                    If Me.datos2.Visible = False Then
                        CargarSolicitudes()
                    End If
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
        Dim gestorequi As New Negocio.EquipoBLL
        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
        Dim solicitudes As List(Of Entidades.Solicitudes) = gestorequi.TraeSolicitudesEquipo(clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = lstgame.SelectedValue))
        If solicitudes.Count > 0 Then
            Session("Solicitudes") = solicitudes
            Me.gv_Solicitudes.DataSource = solicitudes
            Me.gv_Solicitudes.DataBind()
        Else
            Me.warningalert.Visible = True
            Me.textowarning.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvInvitacionError3").Traduccion
        End If

        Me.datos2.Visible = True
    End Sub

    Private Sub CargarCombos(ByVal ID_Juego As Integer)
        Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
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
            Dim Juegos As List(Of Entidades.Game) = GestorJuegos.TraerJuegosSolicitud(clienteLogeado)

            If Juegos.Count = 1 Then
                Response.Redirect("/EnviarInvitacion.aspx" & "?game=" & Juegos(0).ID_Game, False)
            ElseIf Juegos.Count = 0 Then
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvInvitacionError1").Traduccion
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

                ImgBut.PostBackUrl = "/EnviarInvitacion.aspx" & "?game=" & Game.ID_Game
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

    Private Sub gv_Jugadores_DataBound(sender As Object, e As EventArgs) Handles gv_Jugadores.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_Jugadores.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Jugadores.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Jugadores.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Jugadores.PageSize).Selected = True

            txttotal.Text = gv_Jugadores.PageCount

            For cnt As Integer = 0 To gv_Jugadores.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Jugadores.PageIndex Then
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
            For Each row As GridViewRow In gv_Jugadores.Rows
                Dim imagen1 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_envio"), System.Web.UI.WebControls.ImageButton)
                imagen1.CommandArgument = row.RowIndex
            Next

            With gv_Jugadores.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNickname").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderRol").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_Jugadores.BottomPagerRow.Visible = True
            gv_Jugadores.BottomPagerRow.CssClass = "table-bottom-dark"
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
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNickname").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderRol").Traduccion
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

    Private Sub gv_Jugadores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Jugadores.RowCommand
        Try
            Select Case e.CommandName.ToString
                Case "E"
                    Session("Jugador") = TryCast(Session("Jugadores"), List(Of Entidades.Jugador))(e.CommandArgument + (gv_Jugadores.PageIndex * gv_Jugadores.PageSize))
                    Session("Jugadores") = Nothing
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
                        Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Solicitud.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEnvInvitacionSuccess1").Traduccion & " " & Solicitud.Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                        Negocio.BitacoraBLL.CrearBitacora(Bitac)
                        Me.success.Visible = True
                        Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "lblSuccessEnvInvitacion").Traduccion & " " & Solicitud.Jugador.NickName
                        Me.alertvalid.Visible = False
                        CargarSolicitudes()
                    Else
                        Me.alertvalid.Visible = True
                        Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvInvitacionError4").Traduccion
                        Me.success.Visible = False
                        CargarSolicitudes()
                    End If
                Case "R"
                    Gestorequi.RechazarSolicitud(Solicitud)
                    Dim Bitac As Entidades.BitacoraAuditoria
                    Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Solicitud.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEnvInvitacionSuccess2").Traduccion & " " & Solicitud.Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvInvitacionError5").Traduccion & " " & Solicitud.Jugador.NickName
                    Me.alertvalid.Visible = False
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub gv_Jugadores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarJugador()
            gv_Jugadores.PageIndex = e.NewPageIndex
            gv_Jugadores.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub gv_Solucitudes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarJugador()
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

    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Jugadores.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Jugadores.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Jugadores.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Jugadores.PageSize = ddl.SelectedValue
            CargarJugador()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub CargarJugador()
        Dim IdiomaActual As Entidades.IdiomaEntidad
        If IsNothing(Current.Session("Cliente")) Then
            IdiomaActual = Application("Español")
        Else
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        End If

        Dim lista As List(Of Entidades.Jugador)
        Dim Gestor As New Negocio.JugadorBLL
        If txtnombre.Text <> "" Then
            lista = Gestor.TraerJugadoresSolicitud(RTrim(LTrim(txtnombre.Text)), New Entidades.Game With {.ID_Game = lstgame.SelectedValue})
            If lista.Count > 0 Then
                Session("Jugadores") = lista
                Me.gv_Jugadores.DataSource = lista
                Me.gv_Jugadores.DataBind()
                Me.alertvalid.Visible = False
            Else
                Me.alertvalid.Visible = True
                Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EnvInvitacionError2").Traduccion
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
            CargarJugador()
            CargarSolicitudes()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

End Class