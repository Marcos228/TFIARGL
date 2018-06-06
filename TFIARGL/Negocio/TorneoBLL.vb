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

    Public Function TraerTorneosSorteo() As List(Of Torneo)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            Return TorneDAL.TraerTorneosSorteo()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTorneosCargaPartidas() As List(Of Torneo)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            Return TorneDAL.TraerTorneosCargaPartidas()
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

    Public Sub TraerEquiposInscriptos(torneosorteo As Entidades.Torneo)
        Try
            Dim TorneDAL As New DAL.TorneoDAL
            TorneDAL.TraerEquiposInscriptos(torneosorteo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub RealizarSorteo(torneoSorteo As Torneo)
        Try
            torneoSorteo.Equipos = New List(Of Equipo)
            torneoSorteo.Partidas = New List(Of Partida)
            TraerEquiposInscriptos(torneoSorteo)
            Dim PrimeraFaseTorneo As Entidades.Fases = DeterminaFase(torneoSorteo)
            Dim contador As Integer = 0
            Dim randy As New Random
            Dim cantidadEquipos As Integer = torneoSorteo.Equipos.Count
            For x = 0 To cantidadEquipos - 1
                Dim Partida As Entidades.Partida
                Dim EquipoInscripto As Equipo = torneoSorteo.Equipos.ElementAt(randy.Next(0, torneoSorteo.Equipos.Count - 1))
                If x Mod 2 = 0 Then
                    Partida = New Entidades.Partida
                    Partida.Fase = PrimeraFaseTorneo
                    Partida.Equipos.Add(EquipoInscripto)
                    torneoSorteo.Equipos.Remove(EquipoInscripto)
                    torneoSorteo.Partidas.Add(Partida)
                Else
                    Partida = torneoSorteo.Partidas.Last
                    Partida.Equipos.Add(EquipoInscripto)
                    torneoSorteo.Equipos.Remove(EquipoInscripto)
                End If
            Next

            LLenarPartidas(PrimeraFaseTorneo, torneoSorteo, torneoSorteo.Partidas.Count)

            Dim PArtidaBLL As New PartidaBLL
            For Each PArtida In torneoSorteo.Partidas
                PArtidaBLL.AltaPartida(PArtida, torneoSorteo.ID_Torneo, torneoSorteo.Game.ID_Game)
            Next

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Private Sub LLenarPartidas(primeraFaseTorneo As Fases, torneoSorteo As Torneo, cantidadpartidas As Integer)
        Try
            Dim partidasvacias As Integer = 0
            Select Case primeraFaseTorneo
                Case Fases.Final
                    partidasvacias = 1 - cantidadpartidas
                Case Fases.SemiFinal
                    partidasvacias = 2 - cantidadpartidas
                Case Fases.CuartosFinal
                    partidasvacias = 4 - cantidadpartidas
                Case Fases.OctavosFinal
                    partidasvacias = 8 - cantidadpartidas
                Case Fases.DieciseisavosFinal
                    partidasvacias = 16 - cantidadpartidas
                Case Fases.TreintaidosavosFinal
                    partidasvacias = 32 - cantidadpartidas
                Case Fases.SesentaicuatroavosFinal
                    partidasvacias = 64 - cantidadpartidas
            End Select

            For x = 1 To partidasvacias
                Dim Partida As New Entidades.Partida
                Partida.Fase = primeraFaseTorneo
                torneoSorteo.Partidas.Add(Partida)
            Next
            Dim FasePosterior As Entidades.Fases = primeraFaseTorneo - 1
            If FasePosterior > 0 Then
                LLenarPartidas(FasePosterior, torneoSorteo, 0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function DeterminaFase(torneoSorteo As Torneo) As Entidades.Fases
        Select Case torneoSorteo.Equipos.Count
            Case 1 To 2
                Return Fases.Final
            Case 3 To 4
                Return Fases.SemiFinal
            Case 5 To 8
                Return Fases.CuartosFinal
            Case 9 To 16
                Return Fases.OctavosFinal
            Case 17 To 32
                Return Fases.DieciseisavosFinal
            Case 33 To 64
                Return Fases.TreintaidosavosFinal
            Case 65 To 128
                Return Fases.SesentaicuatroavosFinal
            Case Else
                Return Fases.Final
        End Select
    End Function
End Class
