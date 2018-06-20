Public Class Ranking
    Dim Manejador As New ManejadorPuntos
    Public Function CalcularRankingJugador(ByRef Game As Entidades.Game, ByVal anio As Integer, Optional ByRef Rol As Entidades.Rol_Jugador = Nothing, Optional ByVal NombreJugador As String = "") As List(Of Entidades.Jugador)
        Dim GestorPartida As New PartidaBLL
        Dim gestorEstadisticas As New EstadisticaBLL

        Dim PartidasAnalizar As List(Of Entidades.Partida)
        PartidasAnalizar = GestorPartida.TraerEstadisticas(Game, anio)
            Dim listaTipos_Estadisticas As List(Of Entidades.Tipo_Estadistica) = gestorEstadisticas.TraerTipoEstadisticas(Game.ID_Game)
        Return CalculoRankJugador(PartidasAnalizar, listaTipos_Estadisticas, Game.ID_Game, Rol)
    End Function
    Public Function CalcularRankingEquipo(ByRef Game As Entidades.Game, ByVal anio As Integer) As List(Of Entidades.Equipo)
        Dim GestorPartida As New PartidaBLL
        Dim gestorEstadisticas As New EstadisticaBLL
        Dim PartidasAnalizar As List(Of Entidades.Partida) = GestorPartida.TraerEstadisticas(Game, anio)
        Dim listaTipos_Estadisticas As List(Of Entidades.Tipo_Estadistica) = gestorEstadisticas.TraerTipoEstadisticas(Game.ID_Game)
        Return CalculoRankEquipo(PartidasAnalizar, listaTipos_Estadisticas, Game.ID_Game)
    End Function
    Private Function CalculoRankEquipo(ByRef partidas As List(Of Entidades.Partida), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByVal ID_Game As Integer) As List(Of Entidades.Equipo)
        Dim ListaRetorno As New List(Of Entidades.Equipo)

        Dim strategy As Puntaje
        Select Case ID_Game
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



        For Each part In partidas
            For Each Equi As Entidades.Equipo In part.Equipos
                Dim EquipoAgregar As Entidades.Equipo = ListaRetorno.Find(Function(p) p.ID_Equipo = Equi.ID_Equipo)
                If IsNothing(EquipoAgregar) Then
                    Equi.Puntos = manejador.CalcularPuntajeEquipo(part.Estadisticas, Tipo_Estadisticas, Equi, IIf(Equi.ID_Equipo = part.Ganador.ID_Equipo, True, False))
                    ListaRetorno.Add(Equi)
                Else
                    EquipoAgregar.Puntos += manejador.CalcularPuntajeEquipo(part.Estadisticas, Tipo_Estadisticas, EquipoAgregar, IIf(EquipoAgregar.ID_Equipo = part.Ganador.ID_Equipo, True, False))
                End If
            Next
        Next

        Return ListaRetorno
    End Function

    Private Function CalculoRankJugador(ByRef partidas As List(Of Entidades.Partida), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByVal ID_Game As Integer, ByRef rol As Entidades.Rol_Jugador) As List(Of Entidades.Jugador)
        Dim ListaRetorno As New List(Of Entidades.Jugador)

        Dim strategy As Puntaje
        Select Case ID_Game
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

        For Each part In partidas
            For Each Equi As Entidades.Equipo In part.Equipos
                For Each Jugador As Entidades.Jugador In Equi.Jugadores
                    If Not IsNothing(rol) Then
                        If rol.ID_Rol = Jugador.Rol_Jugador.ID_Rol Then
                            Dim JugadorAgregar As Entidades.Jugador = ListaRetorno.Find(Function(p) p.ID_Jugador = Jugador.ID_Jugador)
                            If IsNothing(JugadorAgregar) Then
                                If part.Estadisticas.Count > 0 Then
                                    Jugador.Puntos = manejador.CalcularPuntajeJugador(part.Estadisticas, Tipo_Estadisticas, Jugador)
                                    ListaRetorno.Add(Jugador)
                                End If
                            Else
                                JugadorAgregar.Puntos += manejador.CalcularPuntajeJugador(part.Estadisticas, Tipo_Estadisticas, JugadorAgregar)
                            End If
                        End If
                    Else
                        Dim JugadorAgregar As Entidades.Jugador = ListaRetorno.Find(Function(p) p.ID_Jugador = Jugador.ID_Jugador)
                        If IsNothing(JugadorAgregar) Then
                            If part.Estadisticas.Count > 0 Then
                                Jugador.Puntos = manejador.CalcularPuntajeJugador(part.Estadisticas, Tipo_Estadisticas, Jugador)
                                ListaRetorno.Add(Jugador)
                            End If
                        Else
                            JugadorAgregar.Puntos += manejador.CalcularPuntajeJugador(part.Estadisticas, Tipo_Estadisticas, JugadorAgregar)
                        End If
                    End If
                Next
            Next
        Next

        Return ListaRetorno
    End Function

End Class
