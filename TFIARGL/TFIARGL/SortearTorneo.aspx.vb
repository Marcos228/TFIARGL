Imports System.IO
Imports System.Web.HttpContext
Public Class SortearTorneo
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.datosTor.Visible = False
            Me.datosPar.Visible = False
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
                CargarTorneos()
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
        lista = Gestor.TraerTorneosSorteo()
        Me.gv_torneos.DataSource = lista
        Me.gv_torneos.DataBind()
        If lista.Count = 0 Then
            Me.alertvalid.Visible = True
            Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "SorTorneoError1").Traduccion
            Me.success.Visible = False
        Else
            Session("Torneos") = lista
        End If

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
                    Me.titulotorneo.InnerText = Torneo.Nombre
                    Me.fechadesde.InnerText = Torneo.Fecha_Inicio
                    Me.fechainicio.InnerText = Torneo.Fecha_Inicio_Inscripcion
                    Me.fechafin.InnerText = Torneo.Fecha_Fin_Inscripcion
                    Me.fechahasta.InnerText = Torneo.Fecha_Fin
                    Me.precio.InnerText = Torneo.Precio_Inscripcion.ToString("c", IdiomaActual.Cultura)
                    Me.juego.InnerText = Torneo.Game.Nombre
                    Me.id_torneo.Value = Torneo.ID_Torneo
                    Session("Torneo") = Torneo
                    datosTor.Visible = True
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

                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderID").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFase").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFecha").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
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
                    Dim Partida As Entidades.Partida = TryCast(Session("Torneo"), Entidades.Torneo).Partidas(e.CommandArgument + (gv_partidas.PageIndex * gv_partidas.PageSize))
                    For Each r As GridViewRow In gv_partidas.Rows
                        r.BackColor = Drawing.Color.FromName("#c3e6cb")
                    Next
                    If Partida.FechaHora <> Date.MinValue Then
                        Me.datepicker1.Value = Partida.FechaHora.Date
                        Me.timepicker1.Value = Partida.FechaHora.Hour & ":" & Partida.FechaHora.Minute
                    Else
                        Me.timepicker1.Value = ""
                    End If
                    gv_partidas.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                    Session("Partida") = Partida
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_partidas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarPArtidas(Session("Torneo"))
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
            CargarPArtidas(Session("Torneo"))
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Protected Sub btnSortear_Click(sender As Object, e As EventArgs) Handles btnSortear.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Dim GestorTorneo As New Negocio.TorneoBLL
            Dim TorneoSorteo As Entidades.Torneo = Session("Torneo")
            GestorTorneo.RealizarSorteo(TorneoSorteo)
            CargarPArtidas(TorneoSorteo)

            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSorTorneoSuccess").Traduccion & TorneoSorteo.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
            Me.success.Visible = True
            Me.alertvalid.Visible = False
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub CargarPArtidas(ByRef Torneo As Entidades.Torneo)
        Try
            Me.gv_partidas.DataSource = Torneo.Partidas
            Me.gv_partidas.DataBind()
            Me.datosPar.Visible = True
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
            If datepicker1.Value <> "" And timepicker1.Value <> "" Then
                Dim Partida As Entidades.Partida = TryCast(Session("Partida"), Entidades.Partida)
                Partida.FechaHora = datepicker1.Value & " " & timepicker1.Value
                Dim gestorPartida As New Negocio.PartidaBLL
                gestorPartida.AgregarHorarioPartida(Partida)
                CargarPArtidas(Session("Torneo"))

                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSorTorneoSuccess2").Traduccion & " " & Partida.ID_Partida & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
                Me.success.Visible = True
                Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "SorTorneoSuccess").Traduccion
                Me.alertvalid.Visible = False
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
End Class