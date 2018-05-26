Imports System.IO
Imports System.Web.HttpContext
Public Class EscribirSolicitud
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Session("Equipo")) Then
                Me.txtnombre.Text = TryCast(Session("Equipo"), Entidades.Equipo).Nombre
                CargarCombos(TryCast(Session("Equipo"), Entidades.Equipo).Game)
            Else
                If Not IsNothing(Session("Jugador")) Then
                    Me.txtnombre.Text = TryCast(Session("Jugador"), Entidades.Jugador).NickName
                    CargarCombos(TryCast(Session("Jugador"), Entidades.Jugador).Game)
                End If
            End If
        End If
    End Sub

    Private Sub CargarCombos(ByVal Juego As Entidades.Game)

        Dim Games As New List(Of Entidades.Game)
        Games.Add(Juego)
        Me.lstgame.DataSource = Games
        Me.lstgame.DataBind()
        Me.lstgame.SelectedIndex = 0
    End Sub


    Protected Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            Dim clienteLogeado As Entidades.UsuarioEntidad
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            If txtnombre.Text <> "" And txtmensaje.Text <> "" Then
                clienteLogeado = Current.Session("cliente")
                Dim Jugador As Entidades.Jugador
                Dim Equipo As Entidades.Equipo
                Dim Jugador_a_equipo As Boolean = False
                If Not IsNothing(Session("Jugador")) Then
                    Jugador = TryCast(Session("Jugador"), Entidades.Jugador)
                    Jugador_a_equipo = False
                Else
                    Jugador = clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = lstgame.SelectedValue)
                End If
                Dim Gestorequi As New Negocio.EquipoBLL
                If Not IsNothing(Session("Equipo")) Then
                    Equipo = TryCast(Session("Equipo"), Entidades.Equipo)
                    Jugador_a_equipo = True
                Else
                    Equipo = Gestorequi.TraerEquipoJugador(clienteLogeado.Perfiles_Jugador.Find(Function(p) p.Game.ID_Game = lstgame.SelectedValue).ID_Jugador)
                End If

                If Gestorequi.EnviarSolicitud(txtmensaje.Text, Jugador, Equipo, Jugador_a_equipo) Then
                    Dim Bitac As Entidades.BitacoraAuditoria
                    If Jugador_a_equipo Then
                        Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEscrSolicitudSuccess1").Traduccion & " " & Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Else
                        Bitac = New Entidades.BitacoraAuditoria(clienteLogeado, Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraEscrSolicitudSuccess2").Traduccion & " " & Jugador.NickName & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    End If
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "EscrSolicitudError1").Traduccion
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