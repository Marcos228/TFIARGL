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

    Public Function TraerTorneosInscripcion(game As Entidades.Game) As List(Of Torneo)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            Return TorneDAL.TraerTorneosInscripcion(game)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
