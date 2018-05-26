Imports System.IO
Imports System.Web.HttpContext
Public Class AgregarPerfilJugador
    Inherits System.Web.UI.Page

    Private Sub AgregarPerfilJugador_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Datos.Visible = False
        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Request.QueryString("game")) Then
                If IsNumeric(Request.QueryString("game")) Then
                    CargarRoles(CInt(Request.QueryString("game")))
                Else
                    CargarJuegos()
                End If
            Else
                If Not IsNothing(Request.QueryString("rol")) Then
                    If IsNumeric(Request.QueryString("rol")) Then
                        Me.Datos.Visible = True
                        CargarCombos(Request.QueryString("rol"))
                    Else
                        CargarJuegos()

                    End If
                Else
                    CargarJuegos()
                End If
            End If
        End If
    End Sub

    Private Sub CargarCombos(ByVal ID_Rol As Integer)
        Dim GestorRoles As New Negocio.Rol_JugadorBLL
        Dim Rol As Entidades.Rol_Jugador = GestorRoles.TraerRol(ID_Rol)
        Dim Roles As New List(Of Entidades.Rol_Jugador)
        Dim Games As New List(Of Entidades.Game)
        Roles.Add(Rol)
        Games.Add(Rol.Game)
        Me.lstroljugador.DataSource = Roles
        Me.lstroljugador.DataBind()
        Me.lstroljugador.SelectedIndex = 0
        Me.lstgame.DataSource = Games
        Me.lstgame.DataBind()
        Me.lstgame.SelectedIndex = 0
    End Sub

    Private Sub CargarRoles(ByVal id_game As Integer)
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            lblPanelAddPerfJug.Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfJugSeleccionarRol").Traduccion
            Dim GestorRoles As New Negocio.Rol_JugadorBLL
            Dim Roles As List(Of Entidades.Rol_Jugador) = GestorRoles.TraerRolesJuego(New Entidades.Game With {.ID_Game = id_game})
            For Each RolJug In Roles
                Dim base64string As String = Convert.ToBase64String(RolJug.imagen, 0, RolJug.imagen.Length)
                Dim ImgBut As New ImageButton()

                ImgBut.ImageUrl = Convert.ToString("data:image/jpg;base64,") & base64string
                ImgBut.Height = 100
                ImgBut.ID = "Logo" & RolJug.Nombre
                ImgBut.CssClass = "img-responsive"
                ImgBut.ImageAlign = ImageAlign.Middle

                ImgBut.PostBackUrl = "/AgregarPerfilJugador.aspx" & "?rol=" & RolJug.ID_Rol
                Dim div As HtmlGenericControl = New HtmlGenericControl("div")
                If Roles.IndexOf(RolJug) = 0 Then
                    Select Case Roles.Count
                        Case 1
                            div.Attributes.Add("class", "col-md-2 col-md-offset-5 media")
                        Case 4
                            div.Attributes.Add("class", "col-md-2 col-md-offset-2 media")
                        Case 5
                            div.Attributes.Add("class", "col-md-2 col-md-offset-1 media")
                    End Select
                Else
                    div.Attributes.Add("class", "col-md-2 media")
                End If
                div.Controls.Add(ImgBut)
                Panel.Controls.Add(div)
            Next
            For Each RolJug In Roles
                Dim Label1 As New Label
                Label1.Text = RolJug.Nombre
                Label1.CssClass = "TituloPanel"
                Dim div As HtmlGenericControl = New HtmlGenericControl("div")
                If Roles.IndexOf(RolJug) = 0 Then
                    Select Case Roles.Count
                        Case 1
                            div.Attributes.Add("class", "col-md-2 col-md-offset-5 media")
                        Case 4
                            div.Attributes.Add("class", "col-md-2 col-md-offset-2 media")
                        Case 5
                            div.Attributes.Add("class", "col-md-2 col-md-offset-1 media")
                    End Select

                Else
                    div.Attributes.Add("class", "col-md-2 media")
                End If
                div.Controls.Add(Label1)
                Panel.Controls.Add(div)
            Next
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Private Sub CargarJuegos()
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            lblPanelAddPerfJug.Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfJugSeleccionarJuego").Traduccion
            Dim GestorGame As New Negocio.GameBLL
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Juegos As List(Of Entidades.Game) = GestorGame.TraerJuegos(clienteLogeado)
            If Juegos.Count = 1 Then
                Response.Redirect("/AgregarPerfilJugador.aspx" & "?game=" & Juegos(0).ID_Game, False)
            End If
            For Each Game In Juegos
                Dim base64string As String = Convert.ToBase64String(Game.Imagen, 0, Game.Imagen.Length)
                Dim ImgBut As New ImageButton()

                ImgBut.ImageUrl = Convert.ToString("data:image/jpg;base64,") & base64string
                ImgBut.ID = "Logo" & Game.Nombre
                ImgBut.Height = 150
                ImgBut.CssClass = "img-responsive"
                ImgBut.ImageAlign = ImageAlign.Middle

                ImgBut.PostBackUrl = "/AgregarPerfilJugador.aspx" & "?game=" & Game.ID_Game
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
            If txtnickname.Text <> "" Then
                Dim JugadorPerfil As New Entidades.Jugador(New Entidades.Game With {.ID_Game = lstgame.SelectedValue}, txtnickname.Text, txtgametag.Text, New Entidades.Rol_Jugador With {.ID_Rol = lstroljugador.SelectedValue})
                Dim GestorJugador As New Negocio.JugadorBLL
                clienteLogeado = Current.Session("cliente")
                If GestorJugador.AltaJugador(JugadorPerfil, clienteLogeado) Then
                    clienteLogeado.Perfiles_Jugador = GestorJugador.TraerPerfiles(clienteLogeado)
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddPerfJugSuccess1").Traduccion & " " & JugadorPerfil.Game.Nombre, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
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