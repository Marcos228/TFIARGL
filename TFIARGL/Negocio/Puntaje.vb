Public MustInherit Class Puntaje
    Public MustOverride Function CalcularPuntajeEquipo(ByRef Estadisticas As List(Of Entidades.Estadistica))

    Public MustOverride Function CalcularPuntajeJugador(ByRef Estadisticas As List(Of Entidades.Estadistica))

End Class
