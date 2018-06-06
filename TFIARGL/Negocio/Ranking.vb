Public Class Ranking
    Dim Manejador As New ManejadorPuntos
    Public Function CalcularRanking(ByRef Game As Entidades.Game, ByVal anio As Integer) As List(Of Entidades.Equipo)

        'CalculoRankEquipo()
    End Function

    Public Function CalculoRankEquipo(ByRef partidas As List(Of Entidades.Partida), ByVal ID_Game As Integer) As List(Of Entidades.Equipo)
        Dim strategy As Puntaje
        Dim manejador As New ManejadorPuntos

    End Function

    Public Function CalculoRankJugador(ByRef partidas As List(Of Entidades.Partida), ByVal ID_Game As Integer) As List(Of Entidades.Jugador)
        Dim strategy As Puntaje
        Select Case ID_Game
            Case 1
            Case 2
            Case 3
            Case 4
        End Select



        Dim manejador As New ManejadorPuntos

            manejador.SetearEstrategiaPuntaje(strategy)

    End Function

End Class
