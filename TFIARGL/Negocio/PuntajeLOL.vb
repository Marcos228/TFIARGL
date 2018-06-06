Imports Entidades

Public Class PuntajeLOL
    Inherits Puntaje

    Public Overrides Function CalcularPuntajeEquipo(ByRef Estadisticas As List(Of Estadistica)) As Object
        Throw New NotImplementedException()
    End Function

    Public Overrides Function CalcularPuntajeJugador(ByRef Estadisticas As List(Of Estadistica)) As Object
        Throw New NotImplementedException()
    End Function
End Class
