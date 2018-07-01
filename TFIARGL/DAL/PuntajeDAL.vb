Imports System.Data.SqlClient
Imports Entidades
Public Class PuntajeDAL
    Public Function GuardarPuntajeJugador(jugador As Jugador, partida As Partida) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Puntajes_Jugador (ID_Jugador,ID_Partida,Puntaje) values (@ID_Jugador,@ID_Partida,@Puntaje)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
                .Add(New SqlParameter("@Puntaje", jugador.Puntos))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()


            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GuardarPuntajeEquipo(Equipo As Equipo, partida As Partida) As Boolean
        Try
            Dim Command2 As SqlCommand = Acceso.MiComando("insert into Puntajes_Equipo (ID_Equipo,ID_Partida,Puntaje) values (@ID_Equipo,@ID_Partida,@Puntaje)")
            With Command2.Parameters
                .Add(New SqlParameter("@ID_Equipo", Equipo.ID_Equipo))
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
                .Add(New SqlParameter("@Puntaje", Equipo.Puntos))
            End With
            Acceso.Escritura(Command2)
            Command2.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerPuntajeJugador(part As Partida, jugador As Jugador) As Double
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select Puntaje from Puntajes_Jugador where ID_Partida=@ID_Partida and ID_Jugador=@ID_Jugador")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", part.ID_Partida))
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("Puntaje")
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerPuntajeEquipo(part As Partida, equipo As Equipo) As Double
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select Puntaje from Puntajes_Equipo where ID_Partida=@ID_Partida and ID_Equipo=@ID_Equipo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", part.ID_Partida))
                .Add(New SqlParameter("@ID_Equipo", equipo.ID_Equipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("Puntaje")
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
