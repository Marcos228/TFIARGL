Imports Entidades

Public Class EstadisticaBLL

    Public Function AltaEstadistica(ByRef estad As Entidades.Estadistica, ByVal ID_Partida As Integer) As Boolean
        Try
            Dim DALEstadistica As New DAL.EstadisticaDAL
            Return DALEstadistica.AltaEstadistica(estad, ID_Partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GenerarEstadisticas(ByRef Partida As Entidades.Partida, ByVal ID_Game As Integer) As List(Of Estadistica)
        Try
            Dim DALEstadistica As New DAL.EstadisticaDAL
            Dim ListaRetorno As New List(Of Estadistica)
            Dim ListaTipos As List(Of Tipo_Estadistica) = DALEstadistica.TraerTipoEstadisticas(ID_Game)

            For Each Jugad As Jugador In Partida.Equipos(0).Jugadores
                For Each Tipo_estad As Tipo_Estadistica In ListaTipos
                    If Tipo_estad.Tipo_rol = Jugad.Rol_Jugador.Tipo_rol Then
                        Dim Estadistica As New Estadistica
                        Estadistica.Jugador = Jugad
                        Estadistica.Equipo = Partida.Equipos(0)
                        Estadistica.tipo_Estadistica = Tipo_estad
                        ListaRetorno.Add(Estadistica)
                    End If
                Next
            Next

            For Each Jugad As Jugador In Partida.Equipos(1).Jugadores
                For Each Tipo_estad As Tipo_Estadistica In ListaTipos
                    If Tipo_estad.Tipo_rol = Jugad.Rol_Jugador.Tipo_rol Then
                        Dim Estadistica As New Estadistica
                        Estadistica.Jugador = Jugad
                        Estadistica.Equipo = Partida.Equipos(1)
                        Estadistica.tipo_Estadistica = Tipo_estad
                        ListaRetorno.Add(Estadistica)
                    End If
                Next
            Next

            For Each estad In ListaRetorno
                DALEstadistica.AltaEstadistica(estad, Partida.ID_Partida)
            Next

            Return ListaRetorno
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub TraerEstadisticasPartida(partida As Partida)
        Try
            Dim DALEstadistica As New DAL.EstadisticaDAL
            partida.Estadisticas = New List(Of Estadistica)
            DALEstadistica.TraerEstadisticasPartida(partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarEstadistica(estadistica As Estadistica, id_partida As Integer)
        Try
            Dim DALEstadistica As New DAL.EstadisticaDAL
            DALEstadistica.CargarEstadistica(estadistica, id_partida)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
