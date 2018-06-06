Imports System.Data.SqlClient
Imports Entidades

Public Class EstadisticaDAL
    Public Function AltaEstadistica(ByRef estad As Entidades.Estadistica, ByVal ID_Partida As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Estadistica (ID_Jugador,ID_Partida,ID_Equipo,ID_Tipo_Estadistica,Valor_Estadistica) values (@ID_Jugador,@ID_Partida,@ID_Equipo,@ID_Tipo_Estadistica,0)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Jugador", estad.Jugador.ID_Jugador))
                .Add(New SqlParameter("@ID_Partida", ID_Partida))
                .Add(New SqlParameter("@ID_Equipo", estad.Equipo.ID_Equipo))
                .Add(New SqlParameter("@ID_Tipo_Estadistica", estad.tipo_Estadistica.ID_Tipo_Estadistica))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTipoEstadisticas(id_game As Integer) As List(Of Tipo_Estadistica)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Tipo_Estadistica where ID_Game=@ID_Game")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Game", id_game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim Listaestad As New List(Of Entidades.Tipo_Estadistica)
            For Each _dr As DataRow In dt.Rows
                Dim estad As New Entidades.Tipo_Estadistica
                FormatearTipoEstadistica(estad, _dr)
                Listaestad.Add(estad)
            Next
            Return Listaestad
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Sub TraerEstadisticasPartida(partida As Partida)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Estadistica where ID_Partida=@ID_Partida")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", partida.ID_Partida))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            For Each _dr As DataRow In dt.Rows
                Dim estad As New Entidades.Estadistica
                FormatearEstadistica(estad, _dr)
                partida.Estadisticas.Add(estad)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function TRaerTipoEStadisticaID(ID_Tipo As Integer) As Tipo_Estadistica
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Tipo_Estadistica where ID_Tipo_Estadistica=@ID_Tipo_Estadistica")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Tipo_Estadistica", ID_Tipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Dim estad As New Entidades.Tipo_Estadistica
                FormatearTipoEstadistica(estad, dt.Rows(0))
                Return estad
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub CargarEstadistica(estad As Estadistica, id_partida As Integer)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Estadistica set valor_estadistica=@valor where id_partida=@ID_Partida and ID_Jugador=@ID_Jugador and ID_Equipo=@ID_Equipo and ID_Tipo_Estadistica=@ID_Tipo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Partida", id_partida))
                .Add(New SqlParameter("@ID_Jugador", estad.Jugador.ID_Jugador))
                .Add(New SqlParameter("@ID_Equipo", estad.Equipo.ID_Equipo))
                .Add(New SqlParameter("@ID_Tipo", estad.tipo_Estadistica.ID_Tipo_Estadistica))
                .Add(New SqlParameter("@Valor", estad.Valor_Estadistica))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FormatearTipoEstadistica(tipo_estadistica As Tipo_Estadistica, row As DataRow)
        Try
            tipo_estadistica.ID_Tipo_Estadistica = row("ID_tipo_Estadistica")
            tipo_estadistica.Tipo_rol = row("id_tipo_Rol_Jugador")
            tipo_estadistica.Nombre = row("descripcion")
            tipo_estadistica.Destacado = row("Destacado")
            tipo_estadistica.Valor_Base = row("Valor_Base")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FormatearEstadistica(estadist As Estadistica, row As DataRow)
        Try
            Dim gestorjug As New JugadorDAL
            Dim gestorEqui As New EquipoDAL
            estadist.Jugador = gestorjug.TraerJugadorID(row("ID_Jugador"))
            estadist.Equipo = gestorEqui.TraerEquipoID(row("ID_Equipo"))
                estadist.Valor_Estadistica = row("Valor_Estadistica")
                estadist.tipo_Estadistica = TRaerTipoEStadisticaID(row("ID_Tipo_Estadistica"))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
