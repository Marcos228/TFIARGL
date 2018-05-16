Imports System.Data.SqlClient
Imports Entidades

Public Class EquipoDAL
    Public Function AltaEquipo(ByRef Equipo As Entidades.Equipo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Equipo (Nombre,Fecha_Creacion,ID_Game,Historia) OUTPUT INSERTED.ID_Equipo values (@Nombre,@Fecha_Creacion,@ID_Game,@Historia)")
            With Command.Parameters
                .Add(New SqlParameter("@Nombre", Equipo.Nombre))
                .Add(New SqlParameter("@Fecha_Creacion", Equipo.Fecha_Inicio))
                .Add(New SqlParameter("@ID_Game", Equipo.Game.ID_Game))
                .Add(New SqlParameter("@Historia", Equipo.Historia))
            End With
            Equipo.ID_Equipo = Acceso.Scalar(Command)
            Command.Dispose()

            Dim CommandJ As SqlCommand = Acceso.MiComando("insert into Jugador_Equipo (ID_Jugador,ID_Equipo,Fecha_Inicio) OUTPUT INSERTED.ID_Jug_Equipo values (@ID_Jugador,@ID_Equipo,@Fecha_Inicio)")
            With CommandJ.Parameters
                .Add(New SqlParameter("@ID_Jugador", Equipo.Jugadores(0).ID_Jugador))
                .Add(New SqlParameter("@ID_Equipo", Equipo.ID_Equipo))
                .Add(New SqlParameter("@Fecha_Inicio", Equipo.Fecha_Inicio))
            End With
            Acceso.Scalar(CommandJ)
            CommandJ.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(equipo As Equipo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Equipo from  Equipo where  id_game=@id_game and nombre=@nombre")
            With Command.Parameters
                .Add(New SqlParameter("@nombre", equipo.Nombre))
                .Add(New SqlParameter("@ID_Game", equipo.Game.ID_Game))
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
