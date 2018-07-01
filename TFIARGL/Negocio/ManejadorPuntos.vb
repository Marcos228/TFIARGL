Imports Entidades

Public Class ManejadorPuntos
    Dim _estrategiaPuntaje As Puntaje

    Public Function CalcularPuntajeEquipo(ByRef Estadisticas As List(Of Entidades.Estadistica), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByRef Equipo As Entidades.Equipo, ByRef Ganador As Boolean) As Integer

        Return _estrategiaPuntaje.CalcularPuntajeEquipo(Estadisticas, Tipo_Estadisticas, Equipo, Ganador)
    End Function

    Public Function CalcularPuntajeJugador(ByRef Estadisticas As List(Of Entidades.Estadistica), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByRef Jugador As Entidades.Jugador) As Integer
        Return _estrategiaPuntaje.CalcularPuntajeJugador(Estadisticas, Tipo_Estadisticas, Jugador)
    End Function

    Public Sub SetearEstrategiaPuntaje(ByRef puntaje As Puntaje)
        Me._estrategiaPuntaje = puntaje
    End Sub

    Public Function CalcularPuntosJugador(ByRef Juga As Entidades.Jugador, part As Entidades.Partida) As Double
        Dim gestorEstadisticas As New EstadisticaBLL
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
        Dim listaTipos_Estadisticas As List(Of Entidades.Tipo_Estadistica) = gestorEstadisticas.TraerTipoEstadisticas(Juga.Game.ID_Game)
        Return manejador.CalcularPuntajeJugador(part.Estadisticas, listaTipos_Estadisticas, Juga)
    End Function

    Public Function CalcularPuntosEquipo(ByRef Equipo As Entidades.Equipo, part As Entidades.Partida) As Double
        Dim gestorEstadisticas As New EstadisticaBLL
        Dim strategy As Puntaje
        Select Case Equipo.Game.ID_Game
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
        Dim listaTipos_Estadisticas As List(Of Entidades.Tipo_Estadistica) = gestorEstadisticas.TraerTipoEstadisticas(Equipo.Game.ID_Game)
        Return manejador.CalcularPuntajeEquipo(part.Estadisticas, listaTipos_Estadisticas, Equipo, IIf(Equipo.ID_Equipo = part.Ganador.ID_Equipo, True, False))
    End Function

    Public Function GuardarPuntajeJugador(jugador As Jugador, partida As Partida) As Boolean
        Try
            Dim DALPuntaje As New DAL.PuntajeDAL
            Return DALPuntaje.GuardarPuntajeJugador(jugador, partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GuardarPuntajeEquipo(equipo As Equipo, partida As Partida) As Boolean
        Try
            Dim DALPuntaje As New DAL.PuntajeDAL
            Return DALPuntaje.GuardarPuntajeEquipo(equipo, partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerPuntajeJugador(part As Partida, jugador As Jugador) As Double
        Try
            Dim DALPuntaje As New DAL.PuntajeDAL
            Return DALPuntaje.ObtenerPuntajeJugador(part, jugador)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerPuntajeEquipo(part As Partida, equipo As Equipo) As Double
        Try
            Dim DALPuntaje As New DAL.PuntajeDAL
            Return DALPuntaje.ObtenerPuntajeEquipo(part, equipo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


End Class
