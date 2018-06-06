Public Class ManejadorPuntos
    Dim _estrategiaPuntaje As Puntaje

    Public Function CalcularPuntajeEquipo(ByRef estadisticas As List(Of Entidades.Estadistica)) As Integer

    End Function

    Public Function CalcularPuntajeJugador(ByRef estadisticas As List(Of Entidades.Estadistica)) As Integer

    End Function

    Public Sub SetearEstrategiaPuntaje(ByRef puntaje As Puntaje)
        Me._estrategiaPuntaje = puntaje
    End Sub
End Class
