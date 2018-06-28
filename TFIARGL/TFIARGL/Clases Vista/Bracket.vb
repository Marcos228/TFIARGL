Imports Entidades

Public Class Bracket
    Sub New(torneo As Entidades.Torneo)

        teams = New List(Of String())()
        results = New List(Of List(Of List(Of Integer())))()
        results.Add(New List(Of List(Of Integer()))())
        Dim miBracket = results(0)

        Dim grupos = torneo.Partidas.GroupBy(Function(p) p.Fase).OrderByDescending(Function(p) p.Key).ToArray()

        For Each partido In grupos.First()
            teams.Add(New String() {partido.Equipos(0).Nombre, partido.Equipos(1).Nombre})
        Next

        For Each fase In grupos
            Dim listaFase = New List(Of Integer())()
            For Each partida In fase
                Dim a, b As Integer

                If (partida.Ganador Is Nothing) Then
                    listaFase.Add(New Integer() {})
                Else
                    If (partida.Ganador.ID_Equipo = partida.Equipos(0).ID_Equipo) Then
                        a = 1
                        b = 0
                    Else
                        b = 1
                        a = 0
                    End If

                    listaFase.Add(New Integer() {
                        a, b
                    })

                End If

            Next
            miBracket.Add(listaFase)
        Next

    End Sub
    Private _teams As List(Of String())
    Public Property teams() As List(Of String())
        Get
            Return _teams
        End Get
        Set(ByVal value As List(Of String()))
            _teams = value
        End Set
    End Property
    Private _results As List(Of List(Of List(Of Integer())))
    Public Property results() As List(Of List(Of List(Of Integer())))
        Get
            Return _results
        End Get
        Set(ByVal value As List(Of List(Of List(Of Integer()))))
            _results = value
        End Set
    End Property
End Class
