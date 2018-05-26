Imports System.Data.SqlClient
Imports Entidades

Public Class EquipoDAL
    Public Function AltaEquipo(ByRef Equipo As Entidades.Equipo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Equipo (Nombre,Fecha_Creacion,ID_Game,Historia) OUTPUT INSERTED.ID_Equipo values (@Nombre,@Fecha_Creacion,@ID_Game,@Historia)")
            With Command.Parameters
                .Add(New SqlParameter("@Nombre", Equipo.Nombre))
                .Add(New SqlParameter("@Fecha_Creacion", Equipo.Fecha_Inicio))
                .Add(New SqlParameter("@ID_Game", Equipo.Game.ID_Game))
                .Add(New SqlParameter("@Historia", Equipo.Historia))
            End With
            Equipo.ID_Equipo = Acceso.Scalar(Command)
            Command.Dispose()

            Dim CommandJ As SqlCommand = Acceso.MiComando("insert into Jugador_Equipo (ID_Jugador,ID_Equipo,Fecha_Inicio,Administrador_Equipo) OUTPUT INSERTED.ID_Jug_Equipo values (@ID_Jugador,@ID_Equipo,@Fecha_Inicio,@Admin)")
            With CommandJ.Parameters
                .Add(New SqlParameter("@ID_Jugador", Equipo.Jugadores(0).ID_Jugador))
                .Add(New SqlParameter("@ID_Equipo", Equipo.ID_Equipo))
                .Add(New SqlParameter("@Fecha_Inicio", Equipo.Fecha_Inicio))
                .Add(New SqlParameter("@Admin", True))
            End With
            Acceso.Scalar(CommandJ)
            CommandJ.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraeSolicitudesEquipo(jugador As Jugador) As List(Of Solicitudes)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("SELECT SI.* FROM Solicitud_Invitacion as SI inner join Jugador_Equipo as JE on SI.ID_Equipo=Je.ID_Equipo where JE.ID_Jugador=@ID_Jugador and SI.Jug_a_Equipo =1 and si.aprobada is null")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim listasoli As New List(Of Solicitudes)
            For Each _dr As DataRow In dt.Rows
                Dim soli As New Entidades.Solicitudes
                soli.ID_Solicitud = _dr("ID_solicitud")
                soli.Equipo = TraerEquipoID(_dr("ID_Equipo"))
                Dim jugdal As New JugadorDAL
                soli.Jugador = jugdal.TraerJugadorID(_dr("ID_Jugador"))
                soli.Mensaje = _dr("Mensaje")
                soli.Fecha = _dr("Fecha")
                listasoli.Add(soli)
            Next
            Return listasoli
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Sub RechazarSolicitud(solicitud As Solicitudes)
        Try
            Dim CommandJ As SqlCommand = Acceso.MiComando("Update Solicitud_Invitacion set Aprobada = @aprueba where ID_solicitud=@id_solicitud")
            With CommandJ.Parameters
                .Add(New SqlParameter("@aprueba", False))
                .Add(New SqlParameter("@id_solicitud", solicitud.ID_Solicitud))
            End With
            Acceso.Escritura(CommandJ)
            CommandJ.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ValidaSolicitud(solicitud As Solicitudes) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("SELECT ID_Solicitud FROM Solicitud_Invitacion where ID_Solicitud=@ID_Solicitud and Aprobada is null")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Solicitud", solicitud.ID_Solicitud))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub AprobarSolicitud(solicitud As Solicitudes)
        Try
            Dim CommandJ As SqlCommand = Acceso.MiComando("Update Solicitud_Invitacion set Aprobada = @aprueba where ID_solicitud=@id_solicitud")
            With CommandJ.Parameters
                .Add(New SqlParameter("@aprueba", True))
                .Add(New SqlParameter("@id_solicitud", solicitud.ID_Solicitud))
            End With
            Acceso.Escritura(CommandJ)
            CommandJ.Dispose()

            Dim Command As SqlCommand = Acceso.MiComando("Select * from Solicitud_Invitacion where id_jugador=@id_jugador and Aprobada is null")
            With Command.Parameters
                .Add(New SqlParameter("@id_jugador", solicitud.Jugador.ID_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            For Each _dr As DataRow In dt.Rows
                Dim soli As New Entidades.Solicitudes
                soli.ID_Solicitud = _dr("ID_solicitud")
                RechazarSolicitud(soli)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub SolicitudesCantidadMaxima(Equipo As Equipo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("SELECT * FROM [ARGLeague].[dbo].[Solicitud_Invitacion] as SI where SI.ID_Equipo = @ID_Equipo and Exists(  Select * from Equipo as E inner join Game as G on E.ID_game = G.ID_Game where E.ID_Equipo = @ID_Equipo and Cantidad_Max_Jugadores> (Select Count(*) from Jugador_Equipo as JE where JE.ID_Equipo = @ID_Equipo))")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Equipo", Equipo.ID_Equipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            For Each _dr As DataRow In dt.Rows
                Dim soli As New Entidades.Solicitudes
                soli.ID_Solicitud = _dr("ID_solicitud")
                RechazarSolicitud(soli)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function AgregarJugador(solicitud As Solicitudes) As Boolean
        Try
            AprobarSolicitud(solicitud)
            Dim CommandJ As SqlCommand = Acceso.MiComando("insert into Jugador_Equipo (ID_Jugador,ID_Equipo,Fecha_Inicio,Administrador_Equipo) OUTPUT INSERTED.ID_Jug_Equipo values (@ID_Jugador,@ID_Equipo,@Fecha_Inicio,@Admin)")
            With CommandJ.Parameters
                .Add(New SqlParameter("@ID_Jugador", solicitud.Jugador.ID_Jugador))
                .Add(New SqlParameter("@ID_Equipo", solicitud.Equipo.ID_Equipo))
                .Add(New SqlParameter("@Fecha_Inicio", Now))
                .Add(New SqlParameter("@Admin", False))
            End With
            Acceso.Scalar(CommandJ)
            CommandJ.Dispose()
            SolicitudesCantidadMaxima(solicitud.Equipo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerEquipoJugador(iD_Jugador As Integer) As Equipo
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Jugador_Equipo where id_jugador=@id_jugador")
            With Command.Parameters
                .Add(New SqlParameter("@id_jugador", iD_Jugador))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return TraerEquipoID(dt.Rows(0)("ID_Equipo"))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function TraerEquipoID(ID_Equipo As Integer) As Equipo
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Equipo where ID_Equipo=@ID_Equipo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Equipo", ID_Equipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Dim equipo As New Entidades.Equipo
                FormatearEquipo(equipo, dt.Rows(0))
                Return equipo
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EnviarSolicitud(txtmensaje As String, jugador As Jugador, equipo As Equipo, jugador_equipo As Boolean) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Solicitud_Invitacion (ID_Equipo,ID_Jugador,Mensaje,Jug_a_Equipo,Fecha)  values (@ID_Equipo,@ID_Jugador,@Mensaje,@Jug_a_Equipo, @Fecha)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Equipo", equipo.ID_Equipo))
                .Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
                .Add(New SqlParameter("@Mensaje", txtmensaje))
                .Add(New SqlParameter("@Jug_a_Equipo", jugador_equipo))
                .Add(New SqlParameter("@Fecha", Now))
            End With
            equipo.ID_Equipo = Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerEquiposSolicitud(nombre As String, gam As Game) As List(Of Equipo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Equipo where id_game=@id_game and nombre like Concat('%',@nombre,'%')")
            With Command.Parameters
                .Add(New SqlParameter("@nombre", nombre))
                .Add(New SqlParameter("@ID_Game", gam.ID_Game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim Listaequipo As New List(Of Entidades.Equipo)
            For Each _dr As DataRow In dt.Rows
                Dim equipo As New Entidades.Equipo
                FormatearEquipo(equipo, _dr)
                Listaequipo.Add(equipo)
            Next
            Return Listaequipo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(equipo As Equipo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Equipo from  Equipo where  id_game=@id_game and nombre=@nombre")
            With Command.Parameters
                .Add(New SqlParameter("@nombre", equipo.Nombre))
                .Add(New SqlParameter("@ID_Game", equipo.Game.ID_Game))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearEquipo(ByVal Equip As Entidades.Equipo, ByVal row As DataRow)
        Try
            Equip.ID_Equipo = row("ID_Equipo")
            Equip.Nombre = row("Nombre")
            Equip.Historia = row("historia")
            Equip.Fecha_Inicio = row("Fecha_Creacion")
            If Not IsDBNull(row("Fecha_fin")) Then
                Equip.Fecha_Fin = row("Fecha_fin")
            End If
            Dim gestorGame As New GameDAL
            Equip.Game = gestorGame.TraerJuego(row("ID_Game"))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerPerfiles(iD_Usuario As Integer) As List(Of Jugador)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Jugador, nickname,ID_Game,Game_tag,ID_Rol_Jugador from  Jugador where  ID_Usuario=@usuario")
            With Command.Parameters
                .Add(New SqlParameter("@usuario", iD_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
