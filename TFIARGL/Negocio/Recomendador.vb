Public Class Recomendador
    Public Function RecomendarEquipo(ByRef Jugador As Entidades.Jugador, ByRef Equipos As List(Of Entidades.Equipo)) As List(Of Entidades.Equipo)
        Dim listaretorno As New List(Of Entidades.Equipo)

        For Each Equipo In Equipos
            If ValidarRol_Jugador(Jugador, Equipo) And CompararPuntajes(Jugador, Equipo) Then
                listaretorno.Add(Equipo)
            End If
        Next
        Return listaretorno
    End Function

    Public Function RecomendarJugadores(ByRef Jugadores As List(Of Entidades.Jugador), ByRef Equipo As Entidades.Equipo) As List(Of Entidades.Jugador)
        Dim listaretorno As New List(Of Entidades.Jugador)

        For Each jugador In Jugadores
            If ValidarRol_Jugador(jugador, Equipo) And CompararPuntajes(jugador, Equipo) Then
                listaretorno.Add(jugador)
            End If
        Next
        Return listaretorno
    End Function

    Private Function ValidarRol_Jugador(Juga As Entidades.Jugador, Equi As Entidades.Equipo)
        For Each Jugado As Entidades.Jugador In Equi.Jugadores
            If Juga.Rol_Jugador.ID_Rol = Jugado.Rol_Jugador.ID_Rol Then
                Return False
                Exit For
            End If
        Next
        Return True
    End Function

    Private Function CompararPuntajes(Juga As Entidades.Jugador, Equi As Entidades.Equipo) As Boolean
        Try

            Dim GestorPartida As New PartidaBLL
            Dim gestorEstadisticas As New EstadisticaBLL
            Dim PartidasAnalizarEquipo As List(Of Entidades.Partida) = GestorPartida.TraerEstadisticas(Equi)
            Dim listaTipos_Estadisticas As List(Of Entidades.Tipo_Estadistica) = gestorEstadisticas.TraerTipoEstadisticas(Juga.Game.ID_Game)

            Dim strategy As Puntaje
            Select Case Juga.Game.ID_Game
                Case 1
                    strategy = New PuntajeCSGO
                Case 2
                    strategy = New PuntajeOverwatch
                Case 3
                    strategy = New PuntajeLOL
                Case 4
                    strategy = New PuntajeDota2
                Case 5
                    strategy = New PuntajeStarcraft
                Case 6
                    strategy = New PuntajeFIFA
            End Select
            Dim manejador As New ManejadorPuntos
            manejador.SetearEstrategiaPuntaje(strategy)

            For Each part In PartidasAnalizarEquipo
                Equi.Puntos += manejador.CalcularPuntajeEquipo(part.Estadisticas, listaTipos_Estadisticas, Equi, IIf(Equi.ID_Equipo = part.Ganador.ID_Equipo, True, False))
            Next

            Dim PartidasAnalizarJugador As List(Of Entidades.Partida) = GestorPartida.TraerEstadisticas(Juga)

            For Each part In PartidasAnalizarJugador
                Juga.Puntos += manejador.CalcularPuntajeJugador(part.Estadisticas, listaTipos_Estadisticas, Juga)
            Next

            Dim Determinador As Integer = ((Juga.Puntos * 1.5) / Equi.Puntos) * 100

            If Determinador > 65 And Determinador < 135 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
