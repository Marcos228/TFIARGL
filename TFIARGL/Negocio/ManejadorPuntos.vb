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
End Class
