﻿Imports System.Data.SqlClient
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