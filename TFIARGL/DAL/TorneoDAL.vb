Imports System.Data.SqlClient
Imports Entidades

Public Class TorneoDAL
    Public Function AltaTorneo(ByRef torn As Entidades.Torneo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Torneo (Fecha_Inicio,Fecha_Fin,Nombre,ID_Game,Precio_Inscripcion,Fecha_Inicio_Inscripcion,Fecha_Fin_Inscripcion,Cantidad_Inscripcion,Link_Youtube,Link_Twitch,Fechas_Publicas) OUTPUT INSERTED.ID_Torneo values (@Fecha_Inicio,@Fecha_Fin,@Nombre,@ID_Game,@Precio_Inscripcion,@Fecha_Inicio_Inscripcion,@Fecha_Fin_Inscripcion,@cantidad,@Youtube,@Twitch,@Publicas)")
            With Command.Parameters
                .Add(New SqlParameter("@Fecha_Inicio", torn.Fecha_Inicio))
                .Add(New SqlParameter("@Fecha_Fin", torn.Fecha_Fin))
                .Add(New SqlParameter("@ID_Game", torn.Game.ID_Game))
                .Add(New SqlParameter("@Nombre", torn.Nombre))
                .Add(New SqlParameter("@Precio_Inscripcion", torn.Precio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Inicio_Inscripcion", torn.Fecha_Inicio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Fin_Inscripcion", torn.Fecha_Fin_Inscripcion))
                .Add(New SqlParameter("@cantidad", torn.CantidadParticipantes))
                .Add(New SqlParameter("@Youtube", torn.Youtube))
                .Add(New SqlParameter("@Twitch", torn.Twitch))
                .Add(New SqlParameter("@Publicas", False))
            End With
            torn.ID_Torneo = Acceso.Scalar(Command)
            Command.Dispose()

            For Each spons As Entidades.Sponsor In torn.Sponsors
                Dim CommandJ As SqlCommand = Acceso.MiComando("insert into Torneo_Sponsor (ID_torneo,ID_Sponsor) values (@ID_torneo,@ID_Sponsor)")
                With CommandJ.Parameters
                    .Add(New SqlParameter("@ID_torneo", torn.ID_Torneo))
                    .Add(New SqlParameter("@ID_Sponsor", spons.ID_Sponsor))
                End With
                Acceso.Escritura(CommandJ)
                CommandJ.Dispose()
            Next
            For Each premio As Entidades.Premio In torn.Premios
                Dim CommandP As SqlCommand = Acceso.MiComando("insert into Premios (ID_Torneo,Nombre,ID_Posicion,ID_Tipo_Premio,Valor) values (@ID_torneo,@Nombre,@ID_Posicion,@ID_Tipo_Premio,@Valor)")
                With CommandP.Parameters
                    .Add(New SqlParameter("@ID_torneo", torn.ID_Torneo))
                    .Add(New SqlParameter("@Nombre", premio.Nombre))
                    .Add(New SqlParameter("@ID_Posicion", premio.Posicion))
                    .Add(New SqlParameter("@ID_Tipo_Premio", premio.Tipo_Premio))
                    .Add(New SqlParameter("@Valor", premio.Valor))
                End With
                Acceso.Escritura(CommandP)
                CommandP.Dispose()
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Sub ConfirmarFechasTorneo(torneoNew As Entidades.Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Torneo set Fechas_Publicas=@Publicas where ID_Torneo=@ID_Torneo")
            With Command.Parameters
                .Add(New SqlParameter("@Publicas", True))
                .Add(New SqlParameter("@ID_Torneo", torneoNew.ID_Torneo))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ModificarTorneo(torneoNew As Torneo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Torneo set Fecha_Inicio=@Fecha_Inicio, Fecha_Fin=@Fecha_Fin,Nombre=@Nombre,Precio_Inscripcion=@Precio_Inscripcion,Fecha_Inicio_Inscripcion=@Fecha_Inicio_Inscripcion,Fecha_Fin_Inscripcion=@Fecha_Fin_Inscripcion,Cantidad_Inscripcion=@cantidad,Link_Youtube=@Youtube,Link_Twitch=@Twitch where ID_Torneo=@ID_Torneo")
            With Command.Parameters
                .Add(New SqlParameter("@Fecha_Inicio", torneoNew.Fecha_Inicio))
                .Add(New SqlParameter("@Fecha_Fin", torneoNew.Fecha_Fin))
                .Add(New SqlParameter("@ID_Game", torneoNew.Game.ID_Game))
                .Add(New SqlParameter("@Nombre", torneoNew.Nombre))
                .Add(New SqlParameter("@Precio_Inscripcion", torneoNew.Precio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Inicio_Inscripcion", torneoNew.Fecha_Inicio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Fin_Inscripcion", torneoNew.Fecha_Fin_Inscripcion))
                .Add(New SqlParameter("@cantidad", torneoNew.CantidadParticipantes))
                .Add(New SqlParameter("@Youtube", torneoNew.Youtube))
                .Add(New SqlParameter("@Twitch", torneoNew.Twitch))
                .Add(New SqlParameter("@ID_Torneo", torneoNew.ID_Torneo))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()

            Dim CommandE1 As SqlCommand = Acceso.MiComando("Delete Torneo_Sponsor where ID_Torneo=@ID_torneo")
            With CommandE1.Parameters
                .Add(New SqlParameter("@ID_torneo", torneoNew.ID_Torneo))
            End With
            Acceso.Escritura(CommandE1)
            CommandE1.Dispose()

            For Each spons As Entidades.Sponsor In torneoNew.Sponsors
                Dim CommandJ As SqlCommand = Acceso.MiComando("insert into Torneo_Sponsor (ID_torneo,ID_Sponsor) values (@ID_torneo,@ID_Sponsor)")
                With CommandJ.Parameters
                    .Add(New SqlParameter("@ID_torneo", torneoNew.ID_Torneo))
                    .Add(New SqlParameter("@ID_Sponsor", spons.ID_Sponsor))
                End With
                Acceso.Escritura(CommandJ)
                CommandJ.Dispose()
            Next

            Dim CommandE2 As SqlCommand = Acceso.MiComando("Delete Premios where ID_Torneo=@ID_torneo")
            With CommandE2.Parameters
                .Add(New SqlParameter("@ID_torneo", torneoNew.ID_Torneo))
            End With
            Acceso.Escritura(CommandE2)
            CommandE2.Dispose()


            For Each premio As Entidades.Premio In torneoNew.Premios
                Dim CommandP As SqlCommand = Acceso.MiComando("insert into Premios (ID_Torneo,Nombre,ID_Posicion,ID_Tipo_Premio,Valor) values (@ID_torneo,@Nombre,@ID_Posicion,@ID_Tipo_Premio,@Valor)")
                With CommandP.Parameters
                    .Add(New SqlParameter("@ID_torneo", torneoNew.ID_Torneo))
                    .Add(New SqlParameter("@Nombre", premio.Nombre))
                    .Add(New SqlParameter("@ID_Posicion", premio.Posicion))
                    .Add(New SqlParameter("@ID_Tipo_Premio", premio.Tipo_Premio))
                    .Add(New SqlParameter("@Valor", premio.Valor))
                End With
                Acceso.Escritura(CommandP)
                CommandP.Dispose()
            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarNombreTorneo(torn As Torneo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Torneo from Torneo where Nombre=@Nombre")
            With Command.Parameters
                .Add(New SqlParameter("@Nombre", torn.Nombre))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("ID_Torneo") = torn.ID_Torneo Then
                    Return True
                End If
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTodosTorneos() As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo")
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTorneosCargaPartidas() As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo as T where Fecha_Fin_Inscripcion<GETDATE() and exists(Select Top 1 ID_Partida from Partida as P where P.ID_Torneo =T.ID_Torneo)")
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Sub TraerEquiposInscriptos(torneosorteo As Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo_Equipo where id_torneo=@id_torneo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_torneo", torneosorteo.ID_Torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim gestorequipo As New EquipoDAL
            For Each _dr As DataRow In dt.Rows
                Dim equipotorneo As New Entidades.Equipo
                equipotorneo.ID_Equipo = _dr("ID_Equipo")
                equipotorneo = gestorequipo.TraerEquipoID(equipotorneo.ID_Equipo)
                torneosorteo.Equipos.Add(equipotorneo)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerTorneosModificables() As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo as T where not exists(Select ID_Partida from Partida as P where P.ID_Torneo =T.ID_Torneo and P.Ganador_Local is not null)")
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TraerTorneosVisualizar(jugador As Jugador) As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo as T inner join Torneo_Equipo as Te on T.ID_Torneo=Te.ID_Torneo inner join Jugador_Equipo as JE on Te.ID_Equipo=JE.ID_Equipo where JE.ID_Jugador=@ID_Jugador")
            Command.Parameters.Add(New SqlParameter("@ID_Jugador", jugador.ID_Jugador))
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerTorneosSorteo() As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select * from Torneo as T where Fecha_Fin_Inscripcion<GETDATE() and not exists(Select ID_Partida from Partida as P where P.ID_Torneo =T.ID_Torneo) and (Select Count(*) from Torneo_Equipo as Te where Te.ID_Torneo=T.ID_Torneo)>2")
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub InscribirEquipo(fact As Factura)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Torneo_Equipo (ID_Torneo,ID_Equipo) values (@ID_Torneo,@ID_Equipo)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", fact.Torneo.ID_Torneo))
                .Add(New SqlParameter("@ID_Equipo", fact.Equipo.ID_Equipo))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function TraerTorneosInscripcion(game As Game, equipo As Equipo) As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select T.ID_Torneo,Fecha_Inicio,Fecha_Fin,Nombre,Lugar_Final, ID_Game,PRecio_Inscripcion,Fecha_Fin_Inscripcion,Fecha_Inicio_Inscripcion,cantidad_inscripcion,T.Link_Twitch,T.Link_Youtube  from Torneo as T left join Torneo_Equipo as TE on Te.ID_Torneo=T.ID_Torneo and Te.ID_Equipo=@ID_Equipo Left Join (Select Count(*) as CantidadActual,ID_Torneo from Torneo group by ID_Torneo) as CT on Ct.ID_Torneo=T.ID_Torneo where ID_game=@ID_game and Te.ID_Torneo is null and Cantidad_Inscripcion>isnull(CantidadActual,0) and T.Fecha_Fin_Inscripcion>=GETDATE()")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Game", game.ID_Game))
                .Add(New SqlParameter("@ID_Equipo", equipo.ID_Equipo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListTorneo As New List(Of Entidades.Torneo)
            For Each _dr As DataRow In dt.Rows
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, _dr)
                ListTorneo.Add(torn)
            Next
            Return ListTorneo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function TraerPremios(ByRef Torneo As Entidades.Torneo) As List(Of Premio)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Premio,Nombre,ID_Posicion,ID_Tipo_Premio,Valor from Premios where ID_Torneo=@ID_Torneo ")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", Torneo.ID_Torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListPre As New List(Of Entidades.Premio)
            For Each _dr As DataRow In dt.Rows
                Dim pre As New Entidades.Premio
                pre.ID_Premio = _dr("ID_Premio")
                pre.Tipo_Premio = _dr("ID_Tipo_Premio")
                pre.Nombre = _dr("Nombre")
                pre.Posicion = _dr("ID_Posicion")
                pre.Valor = _dr("Valor")
                ListPre.Add(pre)
            Next
            Return ListPre
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Friend Function TraerTorneoID(ByVal id_torneo As Integer) As Torneo
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Torneo,Fecha_Inicio,Fecha_Fin,Nombre,Lugar_Final, ID_Game,PRecio_Inscripcion,Fecha_Fin_Inscripcion,Fecha_Inicio_Inscripcion,cantidad_inscripcion,Link_twitch,Link_Youtube from Torneo where ID_Torneo=@ID_torneo")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", id_torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Dim torn As New Entidades.Torneo
                FormatearTorneo(torn, dt.Rows(0))
                Return torn
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearTorneo(ByVal torneo As Entidades.Torneo, ByVal row As DataRow)
        Try
            torneo.ID_Torneo = row("ID_Torneo")
            torneo.Nombre = row("Nombre")
            torneo.Fecha_Inicio = row("Fecha_Inicio")
            torneo.Fecha_Fin = row("Fecha_Fin")
            If Not IsDBNull(row("Lugar_Final")) Then
                torneo.Lugar_Final = row("Lugar_Final")
            End If
            torneo.Precio_Inscripcion = row("PRecio_Inscripcion")
            torneo.Fecha_Inicio_Inscripcion = row("Fecha_Inicio_Inscripcion")
            torneo.Fecha_Fin_Inscripcion = row("Fecha_Fin_Inscripcion")
            torneo.Game = (New GameDAL).TraerJuego(row("ID_Game"))
            torneo.Premios = TraerPremios(torneo)
            torneo.CantidadParticipantes = row("cantidad_inscripcion")
            torneo.Twitch = row("Link_Twitch")
            torneo.Youtube = row("Link_Youtube")
            torneo.Sponsors = (New SponsorDAL).TraerSponsorsTorneo(row("ID_Torneo"))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
