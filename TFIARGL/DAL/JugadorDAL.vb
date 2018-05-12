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

    Private Sub FormatearJugador(ByVal Jugador As Entidades.Jugador, ByVal row As DataRow)
        Try

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
