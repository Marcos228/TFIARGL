Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Public Class VisualizarRanking
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarJuegos()
            CargarRankingJugadores()
        End If
    End Sub
    Private Sub CargarRankingJugadores(Optional Rol As Entidades.Rol_Jugador = Nothing)
        Dim lista As New List(Of Entidades.Jugador)
        Dim Gestor As New Negocio.Ranking
        If Me.lstrol.SelectedIndex <> lstrol.Items.Count - 1 Then
            Rol = New Entidades.Rol_Jugador With {.ID_Rol = Me.lstrol.SelectedValue}
        End If
        lista = Gestor.CalcularRankingJugador(New Entidades.Game With {.ID_Game = Me.lstgame.SelectedValue}, 2018, Rol)
        lista = lista.OrderByDescending(Function(x) x.Puntos).ToList
        Me.gv_jugadores.DataSource = lista
        Me.gv_jugadores.DataBind()
        Me.datosequipos.Visible = False
        Me.datosjugadores.Visible = True
        Session("Jugadores") = lista
    End Sub

    Private Sub CargarRankingEquipos()
        Dim lista As New List(Of Entidades.Equipo)
        Dim Gestor As New Negocio.Ranking
        lista = Gestor.CalcularRankingEquipo(New Entidades.Game With {.ID_Game = Me.lstgame.SelectedValue}, 2018)
        lista = lista.OrderByDescending(Function(x) x.Puntos).ToList
        Me.gv_equipos.DataSource = lista
        Me.gv_equipos.DataBind()
        Me.datosjugadores.Visible = False
        Me.datosequipos.Visible = True
        Session("Equipos") = lista
    End Sub

    Private Sub CargarJuegos()
        Try
            Dim GestorGame As New Negocio.GameBLL
            Dim Juegos As List(Of Entidades.Game) = GestorGame.TraerJuegos()
            lstgame.DataSource = Juegos
            lstgame.DataBind()
            CargarRoles

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub CargarRoles()
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Gestor As New Negocio.Rol_JugadorBLL
            Dim Roles As List(Of Entidades.Rol_Jugador) = Gestor.TraerRolesJuego(New Entidades.Game With {.ID_Game = Me.lstgame.SelectedValue})

            lstrol.DataSource = Roles
            lstrol.DataBind()
            Me.lstrol.Items.Add(New ListItem(IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MensajeTodos").Traduccion, -1))
            Me.lstrol.SelectedIndex = Me.lstrol.Items.Count - 1
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub gv_Jugadores_DataBound(sender As Object, e As EventArgs) Handles gv_jugadores.DataBound
        Try


            Try
                Dim ddl2 As DropDownList = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Catch ex As Exception
                Return
            End Try

            Dim ddl As DropDownList = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_jugadores.PageSize).Selected = True

            txttotal.Text = gv_jugadores.PageCount

            For cnt As Integer = 0 To gv_jugadores.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_jugadores.PageIndex Then
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

            For Each row As GridViewRow In gv_jugadores.Rows
                row.Cells(0).Text = row.RowIndex + 1
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex

            Next

            With gv_jugadores.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPosicion").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJugador").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJuego").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderRol").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPuntos").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_jugadores.BottomPagerRow.Visible = True
            gv_jugadores.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_Equipos_DataBound(sender As Object, e As EventArgs) Handles gv_equipos.DataBound
        Try

            Try
                Dim ddl2 As DropDownList = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Catch ex As Exception
                Return
            End Try

            Dim ddl As DropDownList = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_equipos.PageSize).Selected = True

            txttotal.Text = gv_equipos.PageCount

            For cnt As Integer = 0 To gv_equipos.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_equipos.PageIndex Then
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
            Dim Equipos As List(Of Entidades.Equipo) = Session("Equipos")
            For Each row As GridViewRow In gv_equipos.Rows
                row.Cells(0).Text = row.RowIndex + 1
                Dim logo As System.Web.UI.WebControls.Image = DirectCast(row.FindControl("logo"), System.Web.UI.WebControls.Image)
                If Not IsNothing(Equipos(row.RowIndex).Logo) Then
                    Dim base64string As String = Convert.ToBase64String(Equipos(row.RowIndex).Logo, 0, Equipos(row.RowIndex).Logo.Length)
                    logo.ImageUrl = Convert.ToString("data:image/jpg;base64,") & base64string
                Else
                    logo.ImageUrl = "~/Imagenes/flag-white-icon.png"
                End If

                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_equipos.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPosicion").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipo").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJuego").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaAlta").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPuntos").Traduccion
                .Cells(6).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_equipos.BottomPagerRow.Visible = True
            gv_equipos.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Protected Sub gv_Jugadores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarRankingJugadores()
            gv_jugadores.PageIndex = e.NewPageIndex
            gv_jugadores.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_equipos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarRankingEquipos()
            gv_equipos.PageIndex = e.NewPageIndex
            gv_equipos.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub ddlPaging_SelectedIndexChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_equipos.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_equipos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_equipos.PageSize = ddl.SelectedValue
            CargarRankingEquipos()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_jugadores.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_jugadores.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_jugadores.PageSize = ddl.SelectedValue
            CargarRankingJugadores()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub gv_equipos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_equipos.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Select Case e.CommandName.ToString
                Case "S"
                    Session("Equipo") = TryCast(Session("Equipos"), List(Of Entidades.Equipo))(e.CommandArgument + (gv_equipos.PageIndex * gv_equipos.PageSize))
                    Session("Equipos") = Nothing
                    Session("Jugador") = Nothing
                    Response.Redirect("/VisualizarEstadisticas.aspx", False)
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub gv_jugadores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_jugadores.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Select Case e.CommandName.ToString
                Case "S"
                    Session("Jugador") = TryCast(Session("Jugadores"), List(Of Entidades.Jugador))(e.CommandArgument + (gv_jugadores.PageIndex * gv_jugadores.PageSize))
                    Session("Jugadores") = Nothing
                    Session("Equipo") = Nothing
                    Response.Redirect("/VisualizarEstadisticas.aspx", False)
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub lstgame_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstgame.SelectedIndexChanged
        CargarRoles()
    End Sub

    Protected Sub btnjugadores_Click(sender As Object, e As EventArgs) Handles btnjugadores.Click
        CargarRankingJugadores()
    End Sub

    Protected Sub btnequipos_Click(sender As Object, e As EventArgs) Handles btnequipos.Click
        CargarRankingEquipos()
    End Sub
End Class