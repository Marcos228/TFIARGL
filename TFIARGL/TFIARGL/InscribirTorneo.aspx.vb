Imports System.IO
Imports System.Web.HttpContext
Public Class InscribirTorneo
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Datos.Visible = False
        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Request.QueryString("game")) Then
                If IsNumeric(Request.QueryString("game")) Then
                    Me.id_game.Value = Request.QueryString("game")
                    CargarTorneos(Me.id_game.Value)
                Else
                    CargarJuegos()
                End If
            Else
                CargarJuegos()
            End If
        End If
    End Sub
    Private Sub CargarTorneos(ByRef id_game As Integer)
        Dim lista As List(Of Entidades.Torneo)
        Dim Gestor As New Negocio.TorneoBLL
        lista = Gestor.TraerTorneosInscripcion(New Entidades.Game With {.ID_Game = id_game})
        Me.gv_torneos.DataSource = lista
        Me.gv_torneos.DataBind()
        Session("Torneos") = lista
        Me.Datos.Visible = True
    End Sub
    Private Sub CargarJuegos()
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            Dim GestorJuegos As New Negocio.GameBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Juegos As List(Of Entidades.Game) = GestorJuegos.TraerJuegosSolicitud(clienteLogeado)

            If Juegos.Count = 1 Then
                Response.Redirect("/InscribirTorneo.aspx" & "?game=" & Juegos(0).ID_Game, False)
            ElseIf Juegos.Count = 0 Then
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddEquipoError1").Traduccion
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

                ImgBut.PostBackUrl = "/InscribirTorneo.aspx" & "?game=" & Game.ID_Game
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
                '.Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                '.Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCUIL").Traduccion
                '.Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCorreo").Traduccion
                '.Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
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
                    Session("Torneo") = TryCast(Session("Torneos"), List(Of Entidades.Torneo))(e.CommandArgument + (gv_torneos.PageIndex * gv_torneos.PageSize))
                    Session("Torneos") = Nothing
                    Response.Redirect("/VisualizarTorneo.aspx", False)
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_torneos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarTorneos(Me.id_game.Value)
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
            CargarTorneos(Me.id_game.Value)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Protected Sub btnInscribir_Click(sender As Object, e As EventArgs) Handles btnInscribir.Click
        Try
            Dim GestorTorneo As New Negocio.TorneoBLL
            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            'If ValidarFechas(IdiomaActual) Then
            '    If txtnombre.Text <> "" And ValidarNumero(txtprecio.Text) Then

            '        Dim TorneoNew As New Entidades.Torneo With {.Nombre = txtnombre.Text,
            '                            .Fecha_Inicio = datepicker1.Value, .Fecha_Fin = datepicker2.Value,
            '                            .Fecha_Inicio_Inscripcion = datepicker3.Value, .Fecha_Fin_Inscripcion = datepicker4.Value,
            '                            .Game = New Entidades.Game With {.ID_Game = lstgame.SelectedValue}, .Precio_Inscripcion = txtprecio.Text}
            '        TorneoNew.Sponsors = TryCast(Session("SponsorsSeleccionados"), List(Of Entidades.Sponsor))
            '        TorneoNew.Premios = TryCast(Session("Premios"), List(Of Entidades.Premio))
            '        If GestorTorneo.AltaTorneo(TorneoNew) Then
            '            'Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            '            'Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraDelUserSuccess").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            '            'Negocio.BitacoraBLL.CrearBitacora(Bitac)
            '            'Me.success.Visible = True
            '            'Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "DelUserSuccess").Traduccion
            '            'Me.alertvalid.Visible = False
            '        End If
            '    Else

            '    End If
            'End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class