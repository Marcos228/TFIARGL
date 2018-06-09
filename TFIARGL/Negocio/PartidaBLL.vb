Imports Entidades

Public Class PartidaBLL

    Public Function AltaPartida(ByRef part As Entidades.Partida, ByVal ID_Torneo As Integer, ByVal ID_Game As Integer) As Boolean
        Try
            Dim DALPartida As New DAL.PartidaDAL

            Dim EstadisticaBLL As New EstadisticaBLL
            If DALPartida.AltaPartida(part, ID_Torneo) Then
                If part.Equipos.Count = 2 Then
                    EstadisticaBLL.GenerarEstadisticas(part, ID_Game)
                End If
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerEstadisticas(game As Game, anio As Integer) As List(Of Partida)
        Try
            Dim DALPartida As New DAL.PartidaDAL
            Dim EstadisticaBLL As New EstadisticaBLL

            Dim listaretorno As List(Of Entidades.Partida) = DALPartida.TraerPartidasAnio(game, anio)

            For Each Partida In listaretorno
                EstadisticaBLL.TraerEstadisticasPartida(Partida)
            Next

            Return listaretorno
        Catch ex As Exception
            Throw ex
        End Try


    End Function

    Public Function FinalizarPartida(ByRef part As Entidades.Partida) As Boolean
        Try
            Dim DALPartida As New DAL.PartidaDAL
            DALPartida.FinalizarPartida(part)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerPartidasTorneo(id_torneo As Integer) As List(Of Partida)
        Try
            Dim DALPartida As New DAL.PartidaDAL

            Dim listaretorno = DALPartida.TraerPartidasTorneo(id_torneo)
            For Each partida As Entidades.Partida In listaretorno
                If partida.Equipos.Count = 0 Then
                    Dim EquipoLibre As New Entidades.Equipo With {.Nombre = "TBD", .ID_Equipo = 0}
                    Dim EquipoLibre2 As New Entidades.Equipo With {.Nombre = "TBD", .ID_Equipo = 0}
                    partida.Equipos.Add(EquipoLibre)
                    partida.Equipos.Add(EquipoLibre2)
                ElseIf partida.Equipos.Count = 1 Then
                    Dim EquipoLibre As New Entidades.Equipo With {.Nombre = "TBD", .ID_Equipo = 0}
                    partida.Equipos.Add(EquipoLibre)
                End If
            Next
            Return listaretorno
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub AgregarHorarioPartida(partida As Partida)
        Try
            Dim DALPartida As New DAL.PartidaDAL
            DALPartida.AgregarHorarioPartida(partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
