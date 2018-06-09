Public MustInherit Class Puntaje
    Public MustOverride Function CalcularPuntajeEquipo(ByRef Estadisticas As List(Of Entidades.Estadistica), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByRef Equipo As Entidades.Equipo, ByRef Ganador As Boolean) As Integer

    Public MustOverride Function CalcularPuntajeJugador(ByRef Estadisticas As List(Of Entidades.Estadistica), ByRef Tipo_Estadisticas As List(Of Entidades.Tipo_Estadistica), ByRef Jugador As Entidades.Jugador) As Integer

End Class
