Imports System.IO
Imports System.Web.HttpContext
Public Class ModificarTorneo
    Inherits System.Web.UI.Page

    Private Sub ModificarEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("SponsorsSeleccionados") = New List(Of Entidades.Sponsor)
            Session("Premios") = New List(Of Entidades.Premio)
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
                CargarTorneos()
                CargarSponsors()
                CargarPosiciones()
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
        lista = Gestor.TraerTorneosModificables()
        Me.gv_torneos.DataSource = lista
        Me.gv_torneos.DataBind()
        If lista.Count = 0 Then
            Me.alertvalid.Visible = True
            Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "CarPartidaError1").Traduccion
        Else
            Session("Torneos") = lista
        End If

    End Sub

    Private Sub CargarSponsors()
        Dim lista As List(Of Entidades.Sponsor)
        Dim Gestor As New Negocio.SponsorBLL
        lista = Gestor.TraerSponsors()
        Me.gv_sponsors.DataSource = lista
        Me.gv_sponsors.DataBind()
        Session("Sponsors") = lista
    End Sub

    Private Sub CargarPosiciones()
        Me.lstposicion.Items.Clear()
        Dim tipo As New Entidades.Posicion
        Dim itemValues As Array = System.Enum.GetValues(tipo.GetType)
        Dim itemNames As Array = System.Enum.GetNames(tipo.GetType)
        For i As Integer = 0 To itemNames.Length - 1
            If Not TryCast(Session("Premios"), List(Of Entidades.Premio)).Any(Function(p) p.Posicion = itemValues(i)) Then
                Dim item As New ListItem(itemNames(i), itemValues(i))
                Me.lstposicion.Items.Add(item)
            End If
        Next
        If Me.lstposicion.Items.Count = 0 Then
            Ocultable.Visible = False
        Else
            Ocultable.Visible = True
        End If
    End Sub

    Private Sub gv_Sponsors_DataBound(sender As Object, e As EventArgs) Handles gv_sponsors.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_sponsors.PageSize).Selected = True

            txttotal.Text = gv_sponsors.PageCount

            For cnt As Integer = 0 To gv_sponsors.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_sponsors.PageIndex Then
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
            For Each row As GridViewRow In gv_sponsors.Rows
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_sponsors.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCUIL").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCorreo").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_sponsors.BottomPagerRow.Visible = True
            gv_sponsors.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_premios_DataBound(sender As Object, e As EventArgs) Handles gv_premios.DataBound
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            For Each row As GridViewRow In gv_premios.Rows
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_Eliminar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_premios.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPosicion").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderDescripcion").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderValor").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_sponsors_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_sponsors.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Sponsor As Entidades.Sponsor = TryCast(Session("Sponsors"), List(Of Entidades.Sponsor))(e.CommandArgument + (gv_sponsors.PageIndex * gv_sponsors.PageSize))
            Select Case e.CommandName.ToString
                Case "S"
                    If TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor)).Any(Function(p) p.ID_Sponsor = Sponsor.ID_Sponsor) Then
                        gv_sponsors.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.FromName("#c3e6cb")
                        TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor)).Remove(Sponsor)
                        Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_sponsors.Rows.Item(e.CommandArgument).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                        imagen3.ImageUrl = "~/Imagenes/check.png"
                    Else
                        gv_sponsors.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                        TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor)).Add(Sponsor)
                        Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_sponsors.Rows.Item(e.CommandArgument).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                        imagen3.ImageUrl = "~/Imagenes/clear.png"
                    End If


            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub gv_premios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_premios.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Select Case e.CommandName.ToString
                Case "E"
                    TryCast(Session("Premios"), List(Of Entidades.Premio)).RemoveAt(e.CommandArgument)
                    TryCast(Session("Premios"), List(Of Entidades.Premio)).Sort()
                    gv_premios.DataSource = Session("Premios")
                    gv_premios.DataBind()
                    CargarPosiciones()
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Protected Sub gv_sponsors_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarSponsors()
            gv_sponsors.PageIndex = e.NewPageIndex
            gv_sponsors.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_sponsors.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_sponsors.PageSize = ddl.SelectedValue
            CargarSponsors()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub gv_torneos_DataBound(sender As Object, e As EventArgs) Handles gv_torneos.DataBound
        Try
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
                    Me.txtnombre.Text = Torneo.Nombre
                    Me.txtprecio.Text = Torneo.Precio_Inscripcion
                    Me.txtyoutube.Text = Torneo.Youtube
                    Me.txttwitch.Text = Torneo.Twitch
                    Me.txtcantidad.Text = Torneo.CantidadParticipantes
                    Me.datepicker1.Value = Torneo.Fecha_Inicio.Date
                    Me.datepicker2.Value = Torneo.Fecha_Fin.Date
                    Me.datepicker3.Value = Torneo.Fecha_Inicio_Inscripcion.Date
                    Me.datepicker4.Value = Torneo.Fecha_Fin_Inscripcion.Date
                    Dim listajuego As New List(Of Entidades.Game)
                    listajuego.Add(Torneo.Game)
                    Me.lstgame.DataSource = listajuego
                    Me.lstgame.DataBind()

                    TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor)).Clear()

                    Dim ListaSponsor As List(Of Entidades.Sponsor) = Session("Sponsors")

                    For Each row As GridViewRow In gv_sponsors.Rows
                        If Torneo.Sponsors.Any(Function(p) p.ID_Sponsor = ListaSponsor(row.RowIndex).ID_Sponsor) Then
                            gv_sponsors.Rows.Item(row.RowIndex).BackColor = Drawing.Color.Cyan
                            Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_sponsors.Rows.Item(row.RowIndex).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                            imagen3.ImageUrl = "~/Imagenes/clear.png"
                            TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor)).Add(ListaSponsor(row.RowIndex))
                        Else
                            gv_sponsors.Rows.Item(row.RowIndex).BackColor = Drawing.Color.FromName("#c3e6cb")
                            Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_sponsors.Rows.Item(row.RowIndex).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                            imagen3.ImageUrl = "~/Imagenes/check.png"
                        End If
                    Next


                    TryCast(Session("Premios"), List(Of Entidades.Premio)).Clear()
                    For Each Premio In Torneo.Premios
                        TryCast(Session("Premios"), List(Of Entidades.Premio)).Add(Premio)
                    Next
                    gv_premios.DataSource = Session("Premios")
                    gv_premios.DataBind()
                    CargarPosiciones()
                    Session("TorneoSeleccionado") = Torneo
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
    Protected Sub ddlPaging_SelectedIndexChanged3(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_torneos.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_torneos.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged3(sender As Object, e As EventArgs)
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

    Private Function ValidarFechas(idio As Entidades.IdiomaEntidad) As Boolean
        If datepicker1.Value <> "" And datepicker2.Value <> "" And datepicker3.Value <> "" And datepicker4.Value <> "" Then
            Dim Desde As Date = datepicker1.Value
            Dim Hasta As Date = datepicker2.Value
            Dim Inicio As Date = datepicker3.Value
            Dim Fin As Date = datepicker4.Value
            Dim valor = DateDiff(DateInterval.Hour, Inicio, Desde)

            If DateDiff(DateInterval.Hour, Desde, Hasta) > 0 And DateDiff(DateInterval.Hour, Inicio, Hasta) > 0 And DateDiff(DateInterval.Hour, Fin, Hasta) > 0 Then
                If DateDiff(DateInterval.Hour, Inicio, Desde) <= 0 And DateDiff(DateInterval.Hour, Fin, Desde) <= 0 Then
                    If DateDiff(DateInterval.Hour, Inicio, Fin) > 0 Then
                        Return True
                    End If
                End If
            End If
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = idio.Palabras.Find(Function(p) p.Codigo = "AddTorneoError1").Traduccion
            Me.success.Visible = False
        Else
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = idio.Palabras.Find(Function(p) p.Codigo = "AddTorneoError2").Traduccion
            Me.success.Visible = False
        End If
        Return False
    End Function
    Private Function ValidarNumero(ByVal numero As String) As Boolean
        If IsNumeric(numero) Then
            If CInt(numero) >= 1 Then
                Return True
            End If
        End If
        Return False
    End Function
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnagregar.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            If txtnombrepremio.Text <> "" And txtdescripcion.Text <> "" And ValidarNumero(txtvalor.Text) Then
                Dim Premio2 As New Entidades.Premio With {.Nombre = txtnombrepremio.Text, .Posicion = lstposicion.SelectedValue, .Descripcion = txtdescripcion.Text, .Valor = txtvalor.Text}
                TryCast(Session("Premios"), List(Of Entidades.Premio)).Add(Premio2)
                TryCast(Session("Premios"), List(Of Entidades.Premio)).Sort()
                gv_premios.DataSource = Session("Premios")
                gv_premios.DataBind()
                CargarPosiciones()
                txtdescripcion.Text = ""
                txtnombrepremio.Text = ""
                txtvalor.Text = ""
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddTorneoError3").Traduccion
                Me.success.Visible = False
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try


    End Sub

    Protected Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Try
            Dim GestorTorneo As New Negocio.TorneoBLL
            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            If ValidarFechas(IdiomaActual) Then
                If txtnombre.Text <> "" And ValidarNumero(txtprecio.Text) And ValidarNumero(txtcantidad.Text) Then
                    If ValidarURLs(txtyoutube.Text, txttwitch.Text) Then
                        Dim TorneoNew As Entidades.Torneo = Session("TorneoSeleccionado")
                        TorneoNew.CantidadParticipantes = txtcantidad.Text
                        TorneoNew.Fecha_Fin = datepicker2.Value
                        TorneoNew.Fecha_Fin_Inscripcion = datepicker4.Value
                        TorneoNew.Fecha_Inicio = datepicker1.Value
                        TorneoNew.Fecha_Inicio_Inscripcion = datepicker3.Value
                        TorneoNew.Nombre = txtnombre.Text
                        TorneoNew.Precio_Inscripcion = txtprecio.Text
                        TorneoNew.Youtube = txtyoutube.Text
                        TorneoNew.Twitch = txttwitch.Text
                        TorneoNew.Sponsors = TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor))
                        TorneoNew.Premios = TryCast(Session("Premios"), List(Of Entidades.Premio))
                        If GestorTorneo.ModificarTorneo(TorneoNew) Then
                            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddTorneoSuccess1").Traduccion & TorneoNew.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion & ".", Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                            Negocio.BitacoraBLL.CrearBitacora(Bitac)
                            Me.success.Visible = True
                            Me.alertvalid.Visible = False
                        Else
                            Me.alertvalid.Visible = True
                            Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfJugError1").Traduccion
                            Me.success.Visible = False
                        End If
                    Else
                        Me.alertvalid.Visible = True
                        Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddTorneoError4").Traduccion
                        Me.success.Visible = False
                    End If
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                    Me.success.Visible = False
                End If
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Function ValidarURLs(youtube As String, twitch As String) As Boolean
        If youtube.Contains("https://www.youtube.com") And twitch.Contains("https://player.twitch.tv") Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
End Class