Imports System.Globalization
Imports System.IO
Imports System.Web.HttpContext
Public Class VisualizarTorneo
    Inherits System.Web.UI.Page

    Private Sub CrearEquipo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
            If Not IsNothing(Session("Torneo")) Then
                CargarDatos()
            Else
                Response.Redirect("/InscribirTorneo.aspx", False)
            End If
        Else
            CargarDatos()
            btnins.Visible = False
        End If
    End Sub

    Private Sub CargarDatos()
        Dim IdiomaActual As Entidades.IdiomaEntidad = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        Dim Torneo As Entidades.Torneo = Session("Torneo")
        Me.titulotorneo.InnerText = Torneo.Nombre
        Me.fechadesde.InnerText = Torneo.Fecha_Inicio
        Me.fechainicio.InnerText = Torneo.Fecha_Inicio_Inscripcion
        Me.fechafin.InnerText = Torneo.Fecha_Fin_Inscripcion
        Me.fechahasta.InnerText = Torneo.Fecha_Fin
        Me.precio.InnerText = Torneo.Precio_Inscripcion.ToString("c", IdiomaActual.Cultura)
        Me.juego.InnerText = Torneo.Game.Nombre
        Me.id_game.Value = Torneo.Game.ID_Game
    End Sub

    Protected Sub btnInscribir_Click(sender As Object, e As EventArgs) Handles btnInscribir.Click
        Try
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then

                Dim Gestorequi As New Negocio.EquipoBLL
                Dim IdiomaActual As Entidades.IdiomaEntidad
                Dim usuarioPagador As Entidades.UsuarioEntidad = TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad)
                Dim Torneo As Entidades.Torneo = Session("Torneo")

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
                    'Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    'Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraDelUserSuccess").Traduccion & Usuario.Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    'Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    'Me.success.Visible = True
                    'Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "DelUserSuccess").Traduccion
                    'Me.alertvalid.Visible = False
                End If
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openWin();", True)
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
End Class