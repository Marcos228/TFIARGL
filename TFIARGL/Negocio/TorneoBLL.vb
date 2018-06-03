Imports Entidades

Public Class TorneoBLL


    Public Function AltaTorneo(torn As Entidades.Torneo) As Boolean
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            Return TorneDAL.AltaTorneo(torn)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTorneosInscripcion(game As Entidades.Game, jugad As Entidades.Jugador) As List(Of Torneo)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            Return TorneDAL.TraerTorneosInscripcion(game, (New DAL.EquipoDAL).TraerEquipoJugador(jugad.ID_Jugador))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub InscribirEquipo(fact As Factura)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            TorneDAL.InscribirEquipo(fact)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
