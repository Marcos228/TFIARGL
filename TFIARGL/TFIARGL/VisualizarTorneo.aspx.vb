Imports System.Globalization
Imports System.IO
Imports System.Web.HttpContext
Public Class VisualizarTorneo
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Session("Torneo")) Then
                CargarDatos()
                If Not IsPostBack Then
                    CargarPartidas()
                End If
            Else
                Response.Redirect("/InscribirTorneo.aspx", False)
            End If
        Else
            CargarDatos()
            btnins.Visible = False
        End If
    End Sub

    Private Sub CargarDatos()
        If Not IsNothing(Session("Torneo")) And Not IsDBNull(Session("Torneo")) Then
            Dim Torneo As Entidades.Torneo = Session("Torneo")
            Dim titulo As String = ""
            For Each Sponson In Torneo.Sponsors
                titulo += Sponson.Nombre & " - "
            Next
            Me.titulotorneo.InnerText = titulo & Torneo.Nombre
            Me.fechadesde.InnerText = Torneo.Fecha_Inicio
            Me.fechainicio.InnerText = Torneo.Fecha_Inicio_Inscripcion
            Me.fechafin.InnerText = Torneo.Fecha_Fin_Inscripcion
            Me.fechahasta.InnerText = Torneo.Fecha_Fin
            Me.precio.InnerText = "AR$ " & Torneo.Precio_Inscripcion
            Me.juego.InnerText = Torneo.Game.Nombre
            Me.id_game.Value = Torneo.Game.ID_Game
            Me.yourube.Src = Torneo.Youtube
            Me.twitch.Src = Torneo.Twitch
            Me.id_torneo.Value = Torneo.ID_Torneo

            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            If Torneo.Premios.Count > 0 Then
                Me.H3premios.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "lblpremios").Traduccion
            End If

            Me.gv_premios.DataSource = Torneo.Premios
            Me.gv_premios.DataBind()
        End If
    End Sub
    Private Sub CargarPartidas()
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim lista As List(Of Entidades.Partida)
            Dim Gestor As New Negocio.PartidaBLL
            lista = Gestor.TraerPartidasTorneo(Me.id_torneo.Value)
            If lista.Count > 0 Then
                Me.H3partidas.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "lbltorneo").Traduccion
            End If
            Me.gv_partidas.DataSource = lista
            Me.gv_partidas.DataBind()
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


            With gv_premios.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderPosicion").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderDescripcion").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderValor").Traduccion
            End With

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_partidas_DataBound(sender As Object, e As EventArgs) Handles gv_partidas.DataBound
        Try
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
            Next

            With gv_partidas.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFase").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFecha").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipoLocal").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEquipoVisitante").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderGanador").Traduccion
            End With

            gv_partidas.BottomPagerRow.Visible = True
            gv_partidas.BottomPagerRow.CssClass = "table-bottom-dark"
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
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_partidas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_partidas.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
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

    Protected Sub btnInscribir_Click(sender As Object, e As EventArgs) Handles btnInscribir.Click
        Try
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then

                Dim Gestorequi As New Negocio.EquipoBLL

                Dim usuarioPagador As Entidades.UsuarioEntidad = TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad)
                Dim Torneo As Entidades.Torneo = Session("Torneo")
                Dim IdiomaActual As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    IdiomaActual = Application("Español")
                Else
                    IdiomaActual = Application(usuarioPagador.Idioma.Nombre)
                End If

                Dim UsuariosEquipo As List(Of Entidades.UsuarioEntidad) = Gestorequi.TraerUsuariosEquipo(usuarioPagador.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = Me.id_game.Value).ID_Jugador)

                Dim listadetalle As New List(Of Entidades.Detalle_Factura)


                For Each Jugad As Entidades.UsuarioEntidad In UsuariosEquipo
                    Dim Detalle As New Entidades.Detalle_Factura(Jugad, (Torneo.Precio_Inscripcion / UsuariosEquipo.Count))
                    listadetalle.Add(Detalle)
                Next
                Dim Factura As New Entidades.Factura(Torneo, usuarioPagador, Now, listadetalle)
                Factura.Tipo_Pago = New Entidades.Tipo_Pago With {.Tipo_Pago = 1, .Descripcion = "Todas"}
                Factura.Equipo = Gestorequi.TraerEquipoJugador(usuarioPagador.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = Me.id_game.Value).ID_Jugador)

                Dim gestorfactura As New Negocio.FacturaBLL
                If gestorfactura.GenerarFactura(Factura) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraVisTorneoSuccess1").Traduccion & " " & Factura.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraVisTorneoSuccess2").Traduccion & " " & Factura.Torneo.Nombre & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraVisTorneoSuccess3").Traduccion & ".", Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openWin();", True)
                Else
                    Me.success.Visible = False
                    Me.alertvalid.Visible = True
                    Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "VisTorneoError1").Traduccion
                End If
            End If

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class