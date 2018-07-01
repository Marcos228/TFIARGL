Imports System.IO
Imports System.Web.HttpContext
Public Class VisualizarEstadisticas
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not IsNothing(Current.Session("Jugador")) And Not IsDBNull(Current.Session("Jugador")) Then
                Dim Jugador As Entidades.Jugador = Current.Session("Jugador")
                CargarPartidas(Jugador)
                If Not IsNothing(Current.Session("Equipo")) And Not IsDBNull(Current.Session("Equipo")) Then
                    Current.Session("Equipo") = Nothing
                End If
            End If
            If Not IsNothing(Current.Session("Equipo")) And Not IsDBNull(Current.Session("Equipo")) Then
                Dim Equipo As Entidades.Equipo = Current.Session("Equipo")
                CargarPartidas(Equipo)
                If Not IsNothing(Current.Session("Jugador")) And Not IsDBNull(Current.Session("Jugador")) Then
                    Current.Session("Jugador") = Nothing
                End If
            End If
        End If
    End Sub
    Private Sub CargarPartidas(ByRef Jugador As Entidades.Jugador)
        Try
            Me.Nombre.InnerText = Jugador.NickName
            Dim lista As List(Of Entidades.Partida)
            Dim Gestor As New Negocio.PartidaBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            lista = Gestor.TraerPartidasJugador(Jugador)
            Me.gv_partidas.DataSource = lista
            Me.gv_partidas.DataBind()
            Session("Partidas") = lista
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Private Sub CargarPartidas(ByRef Equipo As Entidades.Equipo)
        Try
            Me.Nombre.InnerText = Equipo.Nombre
            Dim lista As List(Of Entidades.Partida)
            Dim Gestor As New Negocio.PartidaBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            lista = Gestor.TraerPartidasEquipo(Equipo)
            Me.gv_partidas.DataSource = lista
            Me.gv_partidas.DataBind()
            Session("Partidas") = lista
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
            Dim Jugador As New Entidades.Jugador With {.ID_Jugador = -1}
            Dim Equipo As New Entidades.Equipo With {.ID_Equipo = -1}
            If Not IsNothing(Current.Session("Jugador")) And Not IsDBNull(Current.Session("Jugador")) Then
                Jugador = Current.Session("Jugador")
            ElseIf Not IsNothing(Current.Session("Equipo")) And Not IsDBNull(Current.Session("Equipo")) Then
                Equipo = Current.Session("Equipo")
            End If

            For Each Estadistica As Entidades.Estadistica In Partida.Estadisticas
                If (Equipo.ID_Equipo = -1 And Jugador.ID_Jugador = Estadistica.Jugador.ID_Jugador) Or (Jugador.ID_Jugador = -1 And Equipo.ID_Equipo = Estadistica.Equipo.ID_Equipo) Then

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

                End If
            Next


            Dim x As Integer = 0
            If Me.gv_estadisticas.Columns.Count = 2 Then
                For Each estad2 In listavistaestadisticas(0).Estadisticas()
                    Dim bfield As New BoundField()
                    bfield.HeaderText = estad2.tipo_Estadistica.Nombre
                    bfield.DataField = String.Concat("Estadisticas(", x, ")", ".Valor_Estadistica")
                    Me.gv_estadisticas.Columns.Add(bfield)
                    x += 1
                Next
            Else

            End If

            Me.gv_estadisticas.DataSource = listavistaestadisticas
            Me.gv_estadisticas.DataBind()
            Session("Estadisticas") = listavistaestadisticas
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
            If Not IsNothing(Current.Session("Jugador")) And Not IsDBNull(Current.Session("Jugador")) Then
                CargarPartidas(Current.Session("Jugador"))
            End If
            If Not IsNothing(Current.Session("Equipo")) And Not IsDBNull(Current.Session("Equipo")) Then
                CargarPartidas(Current.Session("Equipo"))
            End If
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
            If Not IsNothing(Current.Session("Jugador")) And Not IsDBNull(Current.Session("Jugador")) Then
                CargarPartidas(Current.Session("Jugador"))
            End If
            If Not IsNothing(Current.Session("Equipo")) And Not IsDBNull(Current.Session("Equipo")) Then
                CargarPartidas(Current.Session("Equipo"))
            End If
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
                If row.Cells(2).Text = "-1" Then
                    row.Cells(2).Text = "Sin Asignar"
                End If
                If row.Cells(3).Text = "-1" Then
                    row.Cells(3).Text = "Sin Asignar"
                End If
                If row.Cells(4).Text = "-1" Then
                    row.Cells(4).Text = "Sin Asignar"
                End If
                If row.Cells(5).Text = "-1" Then
                    row.Cells(5).Text = "Sin Asignar"
                End If
            Next

            With gv_estadisticas.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderJugador").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipo").Traduccion
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


End Class