Imports Entidades

Public Class PuntajeOverwatch
    Inherits Puntaje
    Public Overrides Function CalcularPuntajeEquipo(ByRef Estadisticas As List(Of Estadistica), ByRef Tipo_Estadisticas As List(Of Tipo_Estadistica), ByRef Equipo As Entidades.Equipo, ByRef Ganador As Boolean) As Integer
        Dim Puntaje As Integer = 0

        Dim Ve As New Dictionary(Of Integer, Double)
        Dim VP As New Dictionary(Of Integer, Double)

        Dim NM As Integer = 2
        Dim W As Integer = 0
        If Ganador Then
            W = 300
        Else
            W = 200
        End If

        For Each estadistica In Estadisticas
            If estadistica.Equipo.ID_Equipo = Equipo.ID_Equipo Then
                If Ve.ContainsKey(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) Then
                    Ve(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) += estadistica.Valor_Estadistica
                Else
                    Ve.Add(estadistica.tipo_Estadistica.ID_Tipo_Estadistica, estadistica.Valor_Estadistica)
                End If
            End If
            If VP.ContainsKey(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) Then
                VP(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) += estadistica.Valor_Estadistica
            Else
                VP.Add(estadistica.tipo_Estadistica.ID_Tipo_Estadistica, estadistica.Valor_Estadistica)
            End If
        Next

        Dim R As Integer = 0

        For Each Testad In Tipo_Estadisticas
            R = (((Ve(Testad.ID_Tipo_Estadistica) / VP(Testad.ID_Tipo_Estadistica)) * NM) * Testad.Valor_Base) + W
            Puntaje += R
        Next

        Return Puntaje

    End Function

    Public Overrides Function CalcularPuntajeJugador(ByRef Estadisticas As List(Of Estadistica), ByRef Tipo_Estadisticas As List(Of Tipo_Estadistica), ByRef Jugador As Jugador) As Integer
        Dim Puntaje As Integer = 0

        Dim VJ As New Dictionary(Of Integer, Double)
        Dim VP As New Dictionary(Of Integer, Double)
        Dim VM As New Dictionary(Of Integer, Double)

        Dim NM As Integer = 8



        For Each estadistica In Estadisticas
            If estadistica.Jugador.ID_Jugador = Jugador.ID_Jugador Then
                VJ.Add(estadistica.tipo_Estadistica.ID_Tipo_Estadistica, estadistica.Valor_Estadistica)
            End If
            If Jugador.Rol_Jugador.Tipo_rol = estadistica.tipo_Estadistica.Tipo_rol Then

                If VP.ContainsKey(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) Then
                    VP(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) += estadistica.Valor_Estadistica
                Else
                    VP.Add(estadistica.tipo_Estadistica.ID_Tipo_Estadistica, estadistica.Valor_Estadistica)
                End If
                If VM.ContainsKey(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) Then
                    If VM(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) < estadistica.Valor_Estadistica Then
                        VM(estadistica.tipo_Estadistica.ID_Tipo_Estadistica) = estadistica.Valor_Estadistica
                    End If
                Else
                    VM.Add(estadistica.tipo_Estadistica.ID_Tipo_Estadistica, estadistica.Valor_Estadistica)
                End If

            End If
        Next

        Dim R As Integer = 0

        For Each Testad In Tipo_Estadisticas
            If Testad.Tipo_rol = Jugador.Rol_Jugador.Tipo_rol Then
                If Testad.Destacado Then
                    Dim PrimerModulo As Single = ((VJ(Testad.ID_Tipo_Estadistica) / VP(Testad.ID_Tipo_Estadistica)) * NM)
                    If Not Double.IsNaN(PrimerModulo) Then
                        If PrimerModulo < 0.5 Then
                            PrimerModulo = 0.5
                        ElseIf PrimerModulo > 3 Then
                            PrimerModulo = 3
                        End If
                        R = (PrimerModulo * Testad.Valor_Base)
                    End If
                Else
                    Dim PrimerModulo As Single = VM(Testad.ID_Tipo_Estadistica) * VJ(Testad.ID_Tipo_Estadistica)
                    If Not Double.IsNaN(PrimerModulo) And PrimerModulo <> 0 Then
                        R = (Testad.Valor_Base / PrimerModulo)
                    End If

                    Puntaje += R
                End If
            End If
        Next



        Return Puntaje

    End Function
End Class
