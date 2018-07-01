Imports System.IO
Imports System.Web.HttpContext
Public Class CargarPartidas
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.datosPar.Visible = False
            Me.datosEst.Visible = False
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
                CargarTorneos()
            End If
        Else
            If Me.datosEst.Visible = True Then
                For Each estad2 In Session("Estadisticas")(0).Estadisticas()
                    CargarTextboxs(estad2)
                Next
            End If
        End If
    End Sub
    Private Sub CargarTorneos()
        Dim IdiomaActual As Entidades.IdiomaEntidad
        If IsNothing(Current.Session("Cliente")) Then
            IdiomaActual = Application("Español")
        Else
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        End If
        Dim lista As List(Of Entidades.Torneo)
        Dim Gestor As New Negocio.TorneoBLL
        Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
        lista = Gestor.TraerTorneosCargaPartidas()
        Me.gv_torneos.DataSource = lista
        Me.gv_torneos.DataBind()
        If lista.Count = 0 Then
            Me.alertvalid.Visible = True
            Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "CarPartidaError1").Traduccion
        Else
            Session("Torneos") = lista
        End If

    End Sub

    Private Sub CargarPartidas()
        Try
            Dim lista As List(Of Entidades.Partida)
            Dim Gestor As New Negocio.PartidaBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            lista = Gestor.TraerPartidasTorneo(Me.id_torneo.Value)
            Me.gv_partidas.DataSource = lista
            Me.gv_partidas.DataBind()
            Session("Partidas") = lista
            Me.datosPar.Visible = True

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub


    Private Sub CargarEstadisticas()
        Try
            Dim Gestor As New Negocio.EstadisticaBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Partida As Entidades.Partida = Session("Partida")
            Gestor.TraerEstadisticasPartida(Partida)
            Dim listavistaestadisticas As New List(Of Vista.EstadisticaVista)

            For Each Estadistica As Entidades.Estadistica In Partida.Estadisticas
                If listavistaestadisticas.Count = 0 Then
                    Dim estadivista As New Vista.EstadisticaVista
                    estadivista.Jugador = Estadistica.Jugador
                    estadivista.Equipo = Estadistica.Equipo
                    estadivista.Estadisticas.Add(Estadistica)
                    listavistaestadisticas.Add(estadivista)
                Else
                    If listavistaestadisticas.Any(Function(p) p.Jugador.ID_Jugador = Estadistica.Jugador.ID_Jugador) Then
                        listavistaestadisticas.Find(Function(p) p.Jugador.ID_Jugador = Estadistica.Jugador.ID_Jugador).Estadisticas.Add(Estadistica)
                    Else
                        Dim estadivista As New Vista.EstadisticaVista
                        estadivista.Jugador = Estadistica.Jugador
                        estadivista.Equipo = Estadistica.Equipo
                        estadivista.Estadisticas.Add(Estadistica)
                        listavistaestadisticas.Add(estadivista)
                    End If
                End If

            Next


            Dim x As Integer = 0
            If Me.gv_estadisticas.Columns.Count = 3 Then
                For Each estad2 In listavistaestadisticas(0).Estadisticas()
                    Dim bfield As New BoundField()
                    bfield.HeaderText = estad2.tipo_Estadistica.Nombre
                    bfield.DataField = String.Concat("Estadisticas(", x, ")", ".Valor_Estadistica")
                    Me.gv_estadisticas.Columns.Add(bfield)

                    CargarTextboxs(estad2)
                    x += 1
                Next
            Else
                If Me.datosEst.Visible = False Then
                    For Each estad2 In listavistaestadisticas(0).Estadisticas()
                        CargarTextboxs(estad2)
                    Next
                End If

            End If

            Me.gv_estadisticas.DataSource = listavistaestadisticas
            Me.gv_estadisticas.DataBind()
            Session("Estadisticas") = listavistaestadisticas
            Me.datosEst.Visible = True
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub CargarTextboxs(ByRef estad2 As Entidades.Estadistica)

        Dim divmaster As HtmlGenericControl = New HtmlGenericControl("div")
        divmaster.Attributes.Add("class", "form-group")
        divmaster.ID = "1-" & estad2.tipo_Estadistica.Nombre

        Dim Labl As New Label
        Labl.ID = "lbl" & estad2.tipo_Estadistica.Nombre
        Labl.Text = estad2.tipo_Estadistica.Nombre
        Labl.CssClass = "col-sm-4 control-label labelform"
        Dim div As HtmlGenericControl = New HtmlGenericControl("div")
        div.Attributes.Add("class", "col-md-6")
        div.ID = "2-" & estad2.tipo_Estadistica.Nombre
        Dim div2 As HtmlGenericControl = New HtmlGenericControl("div")
        div2.Attributes.Add("class", "input-group")
        div2.ID = "3-" & estad2.tipo_Estadistica.Nombre
        div.Controls.Add(div2)
        Dim Texbox As New TextBox
        Texbox.ID = "txt" & estad2.tipo_Estadistica.Nombre
        Texbox.CssClass = "form-control"
        Dim span As HtmlGenericControl = New HtmlGenericControl("span")
        span.Attributes.Add("class", "input-group-addon")

        Dim span2 As HtmlGenericControl = New HtmlGenericControl("span")
        span2.Attributes.Add("class", "glyphicon glyphicon-user")
        span2.Attributes.Add("aria-hidden", "true")
        span.Controls.Add(span2)

        div2.Controls.Add(Texbox)
        div2.Controls.Add(span)

        divmaster.Controls.Add(Labl)

        divmaster.Controls.Add(div)
        Me.EstadisticasTextbox.Controls.Add(divmaster)


    End Sub
    Private Sub gv_torneos_DataBound(sender As Object, e As EventArgs) Handles gv_torneos.DataBound
        Try
            Try
                Dim ddl2 As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Catch ex As Exception
                Return
            End Try
            Dim ddl As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_torneos.PageSize).Selected = True

            txttotal.Text = gv_torneos.PageCount

            For cnt As Integer = 0 To gv_torneos.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_torneos.PageIndex Then
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
            For Each row As GridViewRow In gv_torneos.Rows
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_torneos.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJuego").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaInicio").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaFin").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaInicioInscripcion").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFechaFinInscripcion").Traduccion
                .Cells(6).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPrecio").Traduccion
                .Cells(7).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_torneos.BottomPagerRow.Visible = True
            gv_torneos.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_torneos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_torneos.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Select Case e.CommandName.ToString
                Case "S"
                    Dim Torneo As Entidades.Torneo = TryCast(Session("Torneos"), List(Of Entidades.Torneo))(e.CommandArgument + (gv_torneos.PageIndex * gv_torneos.PageSize))
                    For Each r As GridViewRow In gv_torneos.Rows
                        r.BackColor = Drawing.Color.FromName("#c3e6cb")
                    Next
                    gv_torneos.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                    Me.id_torneo.Value = Torneo.ID_Torneo
                    Me.id_game.Value = Torneo.Game.ID_Game
                    CargarPartidas()
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_torneos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarTorneos()
            gv_torneos.PageIndex = e.NewPageIndex
            gv_torneos.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_torneos.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_torneos.PageSize = ddl.SelectedValue
            CargarTorneos()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Private Sub gv_partidas_DataBound(sender As Object, e As EventArgs) Handles gv_partidas.DataBound
        Try
            Try
                Dim ddl2 As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Catch ex As Exception
                Return
            End Try
            Dim ddl As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_partidas.PageSize).Selected = True

            txttotal.Text = gv_partidas.PageCount

            For cnt As Integer = 0 To gv_partidas.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_partidas.PageIndex Then
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
            Dim noasignados As Boolean = False
            For Each row As GridViewRow In gv_partidas.Rows
                If row.Cells(2).Text = "01-01-0001 00:00:00 " Then
                    noasignados = True
                    row.Cells(2).Text = "Sin Asignar"
                End If
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_partidas.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFase").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFecha").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipoLocal").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipoVisitante").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderGanador").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_partidas.BottomPagerRow.Visible = True
            gv_partidas.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_partidas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_partidas.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Select Case e.CommandName.ToString
                Case "S"
                    Dim Partida As Entidades.Partida = TryCast(Session("Partidas"), List(Of Entidades.Partida))(e.CommandArgument + (gv_partidas.PageIndex * gv_partidas.PageSize))
                    For Each r As GridViewRow In gv_partidas.Rows
                        r.BackColor = Drawing.Color.FromName("#c3e6cb")
                    Next

                    Me.lstequipo.Items.Clear()
                    Me.lstequipo.Items.Add(New ListItem(Partida.Equipos(0).Nombre, Partida.Equipos(0).ID_Equipo))
                    Me.lstequipo.Items.Add(New ListItem(Partida.Equipos(1).Nombre, Partida.Equipos(1).ID_Equipo))

                    If Not IsNothing(Partida.Ganador) Then
                        Me.lstequipo.Items.FindByValue(Partida.Ganador.ID_Equipo).Selected = True
                    End If

                    Me.txtresultadoloc.Text = Partida.ResultadoLocal
                    Me.txtresultadovis.Text = Partida.ResultadoVisitante

                    gv_partidas.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                    Session("Partida") = Partida
                    CargarEstadisticas()
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_partidas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarPartidas()
            gv_partidas.PageIndex = e.NewPageIndex
            gv_partidas.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_partidas.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged2(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_partidas.PageSize = ddl.SelectedValue
            CargarPartidas()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub gv_estadisticas_DataBound(sender As Object, e As EventArgs) Handles gv_estadisticas.DataBound
        Try

            Try
                Dim ddl2 As DropDownList = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Catch ex As Exception
                Return
            End Try

            Dim ddl As DropDownList = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_estadisticas.PageSize).Selected = True

            txttotal.Text = gv_estadisticas.PageCount

            For cnt As Integer = 0 To gv_estadisticas.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_estadisticas.PageIndex Then
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
            For Each row As GridViewRow In gv_estadisticas.Rows
                If row.Cells(3).Text = "-1" Then
                    row.Cells(3).Text = "Sin Asignar"
                End If
                If row.Cells(4).Text = "-1" Then
                    row.Cells(4).Text = "Sin Asignar"
                End If
                If row.Cells(5).Text = "-1" Then
                    row.Cells(5).Text = "Sin Asignar"
                End If
                If row.Cells(6).Text = "-1" Then
                    row.Cells(6).Text = "Sin Asignar"
                End If
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_estadisticas.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJugador").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipo").Traduccion
            End With

            gv_estadisticas.BottomPagerRow.Visible = True
            gv_estadisticas.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_estadisticas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_estadisticas.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Select Case e.CommandName.ToString
                Case "S"
                    Dim Estadastica As EstadisticaVista = TryCast(Session("Estadisticas"), List(Of EstadisticaVista))(e.CommandArgument + (gv_estadisticas.PageIndex * gv_estadisticas.PageSize))
                    For Each r As GridViewRow In gv_estadisticas.Rows
                        r.BackColor = Drawing.Color.FromName("#c3e6cb")
                    Next
                    For Each estad In Estadastica.Estadisticas

                        Dim textbox As TextBox = Me.EstadisticasTextbox.FindControl("1-" & estad.tipo_Estadistica.Nombre).FindControl("2-" & estad.tipo_Estadistica.Nombre).FindControl("3-" & estad.tipo_Estadistica.Nombre).FindControl("txt" & estad.tipo_Estadistica.Nombre)
                        If estad.Valor_Estadistica >= 0 Then
                            textbox.Text = estad.Valor_Estadistica
                        Else
                            textbox.Text = ""
                        End If

                    Next
                    gv_estadisticas.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                    Session("Estadistica") = Estadastica
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_estadisticas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarEstadisticas()
            gv_estadisticas.PageIndex = e.NewPageIndex
            gv_estadisticas.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged3(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_estadisticas.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged3(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_estadisticas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_estadisticas.PageSize = ddl.SelectedValue
            CargarEstadisticas()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Protected Sub btnAsignar_Click(sender As Object, e As EventArgs) Handles btnAsignar.Click

        Try

            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            If IsNumeric(txtresultadoloc.Text) And IsNumeric(txtresultadovis.Text) Then
                Dim Partida As Entidades.Partida = TryCast(Session("Partida"), Entidades.Partida)
                Partida.ResultadoLocal = CInt(txtresultadoloc.Text)
                Partida.ResultadoVisitante = CInt(txtresultadovis.Text)
                Partida.Ganador = Partida.Equipos.Find(Function(p) p.ID_Equipo = lstequipo.SelectedValue)
                Dim gestorPartida As New Negocio.PartidaBLL
                gestorPartida.FinalizarPartida(Partida)
                CargarPartidas()
                Me.datosEst.Visible = False

                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraCarPartidaSuccess1").Traduccion & " " & Partida.ID_Partida & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraCarPartidaSuccess2").Traduccion & " " & Partida.ResultadoLocal & "-" & Partida.ResultadoVisitante & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraCarPartidaSuccess3").Traduccion & " " & Partida.Ganador.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
                Me.alertvalid.Visible = False
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Protected Sub btncargar_Click(sender As Object, e As EventArgs) Handles btncargar.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim validador As Boolean = True
            For Each estad In Session("Estadistica").Estadisticas
                Dim textbox As TextBox = Me.EstadisticasTextbox.FindControl("1-" & estad.tipo_Estadistica.Nombre).FindControl("2-" & estad.tipo_Estadistica.Nombre).FindControl("3-" & estad.tipo_Estadistica.Nombre).FindControl("txt" & estad.tipo_Estadistica.Nombre)
                If Not IsNumeric(textbox.Text) Then
                    validador = False
                    Exit For
                End If
            Next

            If validador Then
                For Each estad In Session("Estadistica").Estadisticas
                    Dim textbox As TextBox = Me.EstadisticasTextbox.FindControl("1-" & estad.tipo_Estadistica.Nombre).FindControl("2-" & estad.tipo_Estadistica.Nombre).FindControl("3-" & estad.tipo_Estadistica.Nombre).FindControl("txt" & estad.tipo_Estadistica.Nombre)
                    Dim estadistica As Entidades.Estadistica = estad
                    estadistica.Valor_Estadistica = CDbl(textbox.Text)
                    Dim gestoresta As New Negocio.EstadisticaBLL
                    gestoresta.CargarEstadistica(estadistica, TryCast(Session("Partida"), Entidades.Partida).ID_Partida)
                    CargarEstadisticas()
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraCarPartidaSuccess4").Traduccion & estadistica.Valor_Estadistica & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraCarPartidaSuccess5").Traduccion & " " & TryCast(Session("Partida"), Entidades.Partida).ID_Partida & " " & estadistica.Jugador.NickName & "-" & estadistica.tipo_Estadistica.Nombre & "-" & estadistica.Equipo.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.alertvalid.Visible = False
                Next
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
            End If


        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            Dim Gestor As New Negocio.ManejadorPuntos
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim validador As Boolean = True
            Dim Partida As Entidades.Partida = Session("Partida")
            For Each estad In Partida.Estadisticas
                If estad.Valor_Estadistica = -1 Then
                    validador = False
                    Exit For
                End If
            Next
            Dim Manejador As New Negocio.ManejadorPuntos
            If validador = True Then
                For Each Equipo In Partida.Equipos
                    Equipo.Puntos = Manejador.CalcularPuntosEquipo(Equipo, Partida)
                    For Each Jugador In Equipo.Jugadores
                        Jugador.Puntos = Manejador.CalcularPuntosJugador(Jugador, Partida)
                        Gestor.GuardarPuntajeJugador(Jugador, Partida)
                    Next
                    Gestor.GuardarPuntajeEquipo(Equipo, Partida)
                Next
            Else
                'Mensaje Error
            End If

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class