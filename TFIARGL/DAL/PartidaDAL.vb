﻿Imports System.Data.SqlClient
Imports Entidades

Public Class PartidaDAL
    Public Function AltaPartida(ByRef part As Entidades.Partida, ByVal ID_Torneo As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Partida (ID_Torneo,ID_Equipo_Local,ID_Equipo_Visitante,ID_FAse) OUTPUT INSERTED.ID_PArtida values (@ID_Torneo,@ID_Equipo_Local,@ID_Equipo_Visitante,@ID_FAse)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", ID_Torneo))
                If part.Equipos.Count = 0 Then
                    .Add(New SqlParameter("@ID_Equipo_Local", DBNull.Value))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", DBNull.Value))
                ElseIf part.Equipos.Count = 1 Then
                    .Add(New SqlParameter("@ID_Equipo_Local", part.Equipos(0).ID_Equipo))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", DBNull.Value))
                Else
                    .Add(New SqlParameter("@ID_Equipo_Local", part.Equipos(0).ID_Equipo))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", part.Equipos(1).ID_Equipo))
                End If
                .Add(New SqlParameter("@ID_FAse", part.Fase))
            End With
            part.ID_Partida = Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Sub RelacionPartidaDeterminar(part As PartidaDeterminar)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Partida_Partida (ID_Partida_Determinar,ID_Partida_Jugar,ID_Partida_Jugar2) values (@ID_Partida_D,@ID_Partida_J1,@ID_Partida_J2)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida_D", part.ID_Partida))
                .Add(New SqlParameter("@ID_Partida_J1", part.Partida1.ID_Partida))
                If Not IsNothing(part.Partida2) Then
                    .Add(New SqlParameter("@ID_Partida_J2", part.Partida2.ID_Partida))
                Else
                    .Add(New SqlParameter("@ID_Partida_J2", DBNull.Value))
                End If
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerPartidasEquipo(equi As Equipo) As List(Of Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Partida as P where (p.ID_Equipo_Local=@ID_Equipo or p.ID_Equipo_Visitante=@ID_equipo) and Ganador_Local is not Null")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Equipo", equi.ID_Equipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaPArtida As New List(Of Entidades.Partida)
            For Each _dr As DataRow In dt.Rows
                Dim partida As New Entidades.Partida
                FormatearPartida(partida, _dr)
                ListaPArtida.Add(partida)
            Next
            Return ListaPArtida
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerPartidasJugador(jugador As Jugador) As List(Of Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select P.* from Partida as P inner join Jugador_Equipo as JE on P.ID_Equipo_Local=JE.ID_Equipo where (JE.ID_Jugador=@ID_Jugador) and Ganador_Local is not Null union select P.* from Partida as P inner join Jugador_Equipo as JE on P.ID_Equipo_Visitante=JE.ID_Equipo where (JE.ID_Jugador=@ID_Jugador) and Ganador_Local is not Null")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaPArtida As New List(Of Entidades.Partida)
            For Each _dr As DataRow In dt.Rows
                Dim partida As New Entidades.Partida
                FormatearPartida(partida, _dr)
                ListaPArtida.Add(partida)
            Next
            Return ListaPArtida
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerPartidasAnio(game As Game, anio As Integer) As List(Of Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Partida as P inner join Torneo as T on T.ID_Torneo=P.ID_Torneo where T.ID_Game=@Game and Year(P.FechaHora)=@Anio and Ganador_Local is not Null")
            With Command.Parameters
                .Add(New SqlParameter("@Anio", anio))
                .Add(New SqlParameter("@Game", game.ID_Game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaPArtida As New List(Of Entidades.Partida)
            For Each _dr As DataRow In dt.Rows
                Dim partida As New Entidades.Partida
                FormatearPartida(partida, _dr)
                ListaPArtida.Add(partida)
            Next
            Return ListaPArtida
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub FinalizarPartida(partida As Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Partida set Resultado_Local=@ResultadoLocal,Resultado_Visitante=@ResultadoVisitante, Ganador_Local=@Ganador where id_partida=@ID_Partida")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
                .Add(New SqlParameter("@ResultadoLocal", partida.ResultadoLocal))
                .Add(New SqlParameter("@ResultadoVisitante", partida.ResultadoVisitante))
                If partida.Ganador.ID_Equipo = partida.Equipos(0).ID_Equipo Then
                    .Add(New SqlParameter("@Ganador", True))
                Else
                    .Add(New SqlParameter("@Ganador", False))
                End If
            End With
            Acceso.Escritura(Command)
            Command.Dispose()

            Dim CommandVer As SqlCommand = Acceso.MiComando(" select ID_Partida_Determinar, 1 as Localia from Partida_Partida where ID_Partida_Jugar=@ID_Partida union   select ID_Partida_Determinar, 0 as Localia from Partida_Partida where ID_Partida_Jugar2=@ID_Partida")
            With CommandVer.Parameters
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
            End With

            Dim dt As DataTable = Acceso.Lectura(CommandVer)

            If dt.Rows.Count > 0 Then
                Dim Command2 As SqlCommand
                If dt.Rows(0)("Localia") = 1 Then
                    Command2 = Acceso.MiComando("Update Partida set ID_Equipo_Local=@ID_Equipo where id_partida=@ID_Partida")
                Else
                    Command2 = Acceso.MiComando("Update Partida set ID_Equipo_Visitante=@ID_Equipo where id_partida=@ID_Partida")
                End If

                With Command2.Parameters
                    .Add(New SqlParameter("@ID_Partida", dt.Rows(0)("ID_Partida_Determinar")))
                    .Add(New SqlParameter("@ID_Equipo", partida.Ganador.ID_Equipo))
                End With
                Acceso.Escritura(Command2)
                Command2.Dispose()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub AgregarHorarioPartida(partida As Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Partida set FechaHora=@Fecha where id_partida=@ID_Partida")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
                .Add(New SqlParameter("@Fecha", partida.FechaHora))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerPartidasTorneo(id_torneo As Integer) As List(Of Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Partida where ID_Torneo=@ID_torneo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", id_torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaPArtida As New List(Of Entidades.Partida)
            For Each _dr As DataRow In dt.Rows
                Dim partida As New Entidades.Partida
                FormatearPartida(partida, _dr)
                ListaPArtida.Add(partida)
            Next
            Return ListaPArtida
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerPartidasTorneoVisualizacion(id_torneo As Integer) As List(Of Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select P.ID_Partida,P.ID_Torneo,P.ID_Equipo_Local,P.ID_Equipo_Visitante,P.ID_Fase,P.Resultado_Local,P.Resultado_Visitante,P.Ganador_Local,IIF(T.Fechas_Publicas=0,null,P.FechaHora) as FechaHora from Partida as P inner join Torneo as T on T.ID_Torneo=P.ID_Torneo  where P.ID_Torneo=@ID_Torneo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", id_torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListaPArtida As New List(Of Entidades.Partida)
            For Each _dr As DataRow In dt.Rows
                Dim partida As New Entidades.Partida
                FormatearPartida(partida, _dr)
                ListaPArtida.Add(partida)
            Next
            Return ListaPArtida
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearPartida(partida As Partida, row As DataRow)
        Try
            partida.ID_Partida = Row("ID_Partida")
            partida.Fase = row("ID_Fase")
            If Not IsDBNull(row("FechaHora")) Then
                partida.FechaHora = row("FechaHora")
            End If

            If Not IsDBNull(row("Resultado_Local")) Then
                partida.ResultadoLocal = row("Resultado_Local")
            End If
            If Not IsDBNull(row("Resultado_Visitante")) Then
                partida.ResultadoVisitante = row("Resultado_Visitante")
            End If
            Dim gestorEquipo As New EquipoDAL
            If Not IsDBNull(row("ID_Equipo_Local")) Then
                partida.Equipos.Add(gestorEquipo.TraerEquipoID(row("ID_Equipo_Local")))
            End If
            If Not IsDBNull(row("ID_Equipo_Visitante")) Then
                partida.Equipos.Add(gestorEquipo.TraerEquipoID(row("ID_Equipo_Visitante")))
            End If
            If Not IsDBNull(row("Ganador_Local")) Then
                If row("Ganador_Local") = True Then
                    partida.Ganador = partida.Equipos(0)
                Else
                    partida.Ganador = partida.Equipos(1)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
