Imports System.Data.SqlClient
Imports Entidades

Public Class JugadorDAL
    Public Function AltaJugador(ByRef Jugado As Entidades.Jugador, ByRef Usu As Entidades.UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Jugador (ID_Usuario,Nickname,ID_Game,Game_Tag,ID_Rol_Jugador) OUTPUT INSERTED.ID_Jugador values (@ID_Usuario,@Nickname,@ID_Game,@Game_Tag,@ID_Rol_Jugador)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usu.ID_Usuario))
                .Add(New SqlParameter("@Nickname", Jugado.NickName))
                .Add(New SqlParameter("@ID_Game", Jugado.Game.ID_Game))
                .Add(New SqlParameter("@Game_Tag", Jugado.Game_Tag))
                .Add(New SqlParameter("@ID_Rol_Jugador", Jugado.Rol_Jugador.ID_Rol))
            End With
            Jugado.ID_Jugador = Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(jugador As Jugador) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Jugador from  Jugador where  id_game=@id_game and nickname=@nickname")
            With Command.Parameters
                .Add(New SqlParameter("@nickname", jugador.NickName))
                .Add(New SqlParameter("@ID_Game", jugador.Game.ID_Game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerJugadoresSolicitud(nombre As String, game As Game) As List(Of Jugador)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Jugador where id_game=@id_game and nickname like Concat('%',@nombre,'%') and not exists(select ID_Jugador from Jugador_Equipo where Jugador_Equipo.ID_Jugador=Jugador.ID_Jugador and Jugador_Equipo.Fecha_fin is null)")
            With Command.Parameters
                .Add(New SqlParameter("@nombre", nombre))
                .Add(New SqlParameter("@ID_Game", game.ID_Game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim Listajugador As New List(Of Entidades.Jugador)
            For Each _dr As DataRow In dt.Rows
                Dim juga As New Entidades.Jugador
                FormatearJugador(juga, _dr)
                Listajugador.Add(juga)
            Next
            Return Listajugador
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TraeSolicitudesJugador(jugador As Jugador) As List(Of Solicitudes)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("SELECT SI.* FROM Solicitud_Invitacion as SI  where SI.ID_Jugador=@ID_Jugador and SI.Jug_a_Equipo =0 and aprobado is null")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim listasoli As New List(Of Solicitudes)
            For Each _dr As DataRow In dt.Rows
                Dim soli As New Entidades.Solicitudes
                soli.ID_Solicitud = _dr("ID_solicitud")
                Dim equigest As New EquipoDAL
                soli.Equipo = equigest.TraerEquipoID(_dr("ID_Equipo"))
                soli.Jugador = TraerJugadorID(_dr("ID_Jugador"))
                soli.Mensaje = _dr("Mensaje")
                soli.Fecha = _dr("Fecha")
                listasoli.Add(soli)
            Next
            Return listasoli
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Friend Function TraerJugadorID(id_jugador As Integer) As Jugador
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Jugador where id_jugador=@id_jugador")
            With Command.Parameters
                .Add(New SqlParameter("@id_jugador", id_jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Dim juga As New Entidades.Jugador
                FormatearJugador(juga, dt.Rows(0))
                Return juga
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearJugador(ByVal Jugador As Entidades.Jugador, ByVal row As DataRow)
        Try
            Jugador.ID_Jugador = row("ID_Jugador")
            Jugador.NickName = row("Nickname")
            Jugador.Game_Tag = row("Game_Tag")
            Dim gestorRoles As New Rol_JugadorDAL
            Jugador.Rol_Jugador = gestorRoles.TraerRol(row("ID_Rol_Jugador"))
            Dim gestroGame As New GameDAL
            Jugador.Game = gestroGame.TraerJuego(row("ID_Game"))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerPerfiles(iD_Usuario As Integer) As List(Of Jugador)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Jugador, nickname,ID_Game,Game_tag,ID_Rol_Jugador from  Jugador where  ID_Usuario=@usuario")
            With Command.Parameters
                .Add(New SqlParameter("@usuario", iD_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaGames As New List(Of Entidades.Jugador)
            For Each _dr As DataRow In dt.Rows
                Dim Juga As New Entidades.Jugador
                FormatearJugador(Juga, _dr)
                ListaGames.Add(Juga)
            Next
            Return ListaGames
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
