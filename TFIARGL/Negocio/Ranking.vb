Public Class Ranking
    Dim Manejador As New ManejadorPuntos
    Public Function CalcularRankingJugador(ByRef Game As Entidades.Game, ByVal anio As Integer, Optional ByRef Rol As Entidades.Rol_Jugador = Nothing, Optional ByVal NombreJugador As String = "") As List(Of Entidades.Jugador)
        Dim GestorPartida As New PartidaBLL
        Dim gestorEstadisticas As New EstadisticaBLL

        Dim PartidasAnalizar As List(Of Entidades.Partida)
        PartidasAnalizar = GestorPartida.TraerEstadisticas(Game, anio)

        Return CalculoRankJugador(PartidasAnalizar, Game.ID_Game, Rol)

    End Function
    Public Function CalcularRankingEquipo(ByRef Game As Entidades.Game, ByVal anio As Integer) As List(Of Entidades.Equipo)
        Dim GestorPartida As New PartidaBLL
        Dim gestorEstadisticas As New EstadisticaBLL
        Dim PartidasAnalizar As List(Of Entidades.Partida) = GestorPartida.TraerEstadisticas(Game, anio)
        Return CalculoRankEquipo(PartidasAnalizar, Game.ID_Game)
    End Function
    Private Function CalculoRankEquipo(ByRef partidas As List(Of Entidades.Partida), ByVal ID_Game As Integer) As List(Of Entidades.Equipo)
        Dim ListaRetorno As New List(Of Entidades.Equipo)


        For Each part In partidas
            For Each Equi As Entidades.Equipo In part.Equipos
                Dim EquipoAgregar As Entidades.Equipo = ListaRetorno.Find(Function(p) p.ID_Equipo = Equi.ID_Equipo)
                If IsNothing(EquipoAgregar) Then
                    Equi.Puntos = Manejador.ObtenerPuntajeEquipo(part, Equi)
                    ListaRetorno.Add(Equi)
                Else
                    EquipoAgregar.Puntos += Manejador.ObtenerPuntajeEquipo(part, EquipoAgregar)
                End If
            Next
        Next

        Return ListaRetorno
    End Function

    Private Function CalculoRankJugador(ByRef partidas As List(Of Entidades.Partida), ByVal ID_Game As Integer, ByRef rol As Entidades.Rol_Jugador) As List(Of Entidades.Jugador)
        Dim ListaRetorno As New List(Of Entidades.Jugador)


        For Each part In partidas
            For Each Equi As Entidades.Equipo In part.Equipos
                For Each Jugador As Entidades.Jugador In Equi.Jugadores
                    If Not IsNothing(rol) Then
                        If rol.ID_Rol = Jugador.Rol_Jugador.ID_Rol Then
                            Dim JugadorAgregar As Entidades.Jugador = ListaRetorno.Find(Function(p) p.ID_Jugador = Jugador.ID_Jugador)
                            If IsNothing(JugadorAgregar) Then
                                Jugador.Puntos = Manejador.ObtenerPuntajeJugador(part, Jugador)
                                ListaRetorno.Add(Jugador)
                        Else
                                JugadorAgregar.Puntos += Manejador.ObtenerPuntajeJugador(part, JugadorAgregar)
                            End If
                        End If
                    Else
                        Dim JugadorAgregar As Entidades.Jugador = ListaRetorno.Find(Function(p) p.ID_Jugador = Jugador.ID_Jugador)
                        If IsNothing(JugadorAgregar) Then
                            Jugador.Puntos = Manejador.ObtenerPuntajeJugador(part, Jugador)
                            ListaRetorno.Add(Jugador)
                        Else
                            JugadorAgregar.Puntos += Manejador.ObtenerPuntajeJugador(part, JugadorAgregar)
                        End If
                    End If
                Next
            Next
        Next

        Return ListaRetorno
    End Function

End Class
