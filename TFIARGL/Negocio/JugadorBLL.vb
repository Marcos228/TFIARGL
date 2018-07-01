Imports Entidades

Public Class JugadorBLL


    Public Function AltaJugador(Jugador As Entidades.Jugador, ByRef usu As Entidades.UsuarioEntidad) As Boolean
        Try
            Dim DALJugador As New DAL.JugadorDAL
            If DALJugador.ValidaNombre(Jugador) Then
                Return DALJugador.AltaJugador(Jugador, usu)
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function ValidaNombre(Jugador As Entidades.Jugador) As Boolean
        Try
            Dim DALJugador As New DAL.JugadorDAL
            Return DALJugador.ValidaNombre(Jugador)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerPerfiles(usuario As Entidades.UsuarioEntidad) As List(Of Entidades.Jugador)
        Try
            Dim DALJugador As New DAL.JugadorDAL
            Return DALJugador.TraerPerfiles(usuario.ID_Usuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerJugadoresSolicitud(nombre As String, game As Game) As List(Of Jugador)
        Try
            Dim DALJugador As New DAL.JugadorDAL
            Return DALJugador.TraerJugadoresSolicitud(nombre, game)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TraerJugadoresSolicitud(game As Game) As List(Of Jugador)
        Try
            Dim DALJugador As New DAL.JugadorDAL
            Return DALJugador.TraerJugadoresSolicitud(game)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraeSolicitudesJugador(jugador As Jugador) As List(Of Solicitudes)
        Try
            Dim DAljugadores As New DAL.JugadorDAL
            Return DAljugadores.TraeSolicitudesJugador(jugador)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
