Imports System.IO
Imports System.Web.HttpContext
Public Class CrearEquipo
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Datos.Visible = False
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

    Private Sub CargarCombos(ByVal ID_Juego As Integer)
        Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        lblPanelAddEquipo.Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddEquipoCrearEquipo").Traduccion
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
            lblPanelAddEquipo.Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddEquipoSeleccionarJuego").Traduccion
            Dim GestorJuegos As New Negocio.GameBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Juegos As List(Of Entidades.Game) = GestorJuegos.TraerJuegosAltaEquipo(clienteLogeado)

            If Juegos.Count = 1 Then
                Response.Redirect("/CrearEquipo.aspx" & "?game=" & Juegos(0).ID_Game, False)
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

                ImgBut.PostBackUrl = "/CrearEquipo.aspx" & "?game=" & Game.ID_Game
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

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            Dim clienteLogeado As Entidades.UsuarioEntidad
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            If txtnombre.Text <> "" And txthistoria.Text <> "" Then
                clienteLogeado = Current.Session("cliente")
                Dim Equi As New Entidades.Equipo(New Entidades.Game With {.ID_Game = lstgame.SelectedValue}, txtnombre.Text, txthistoria.Text, Now, clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = lstgame.SelectedValue))
                Dim Gestorequi As New Negocio.EquipoBLL
                If Gestorequi.AltaEquipo(Equi) Then
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddEquipoSuccess1").Traduccion & " " & Equi.Nombre & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddEquipoError2").Traduccion
                    Me.success.Visible = False
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
End Class