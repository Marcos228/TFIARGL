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

End Class
