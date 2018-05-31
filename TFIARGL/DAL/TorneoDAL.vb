Imports System.Data.SqlClient
Imports Entidades

Public Class TorneoDAL
    Public Function AltaTorneo(ByRef torn As Entidades.Torneo) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Torneo (Fecha_Inicio,Fecha_Fin,Nombre,ID_Game,Precio_Inscripcion,Fecha_Inicio_Inscripcion,Fecha_Fin_Inscripcion) OUTPUT INSERTED.ID_Torneo values (@Fecha_Inicio,@Fecha_Fin,@Nombre,@ID_Game,@Precio_Inscripcion,@Fecha_Inicio_Inscripcion,@Fecha_Fin_Inscripcion)")
            With Command.Parameters
                .Add(New SqlParameter("@Fecha_Inicio", torn.Fecha_Inicio))
                .Add(New SqlParameter("@Fecha_Fin", torn.Fecha_Fin))
                .Add(New SqlParameter("@ID_Game", torn.Game.ID_Game))
                .Add(New SqlParameter("@Nombre", torn.Nombre))
                .Add(New SqlParameter("@Precio_Inscripcion", torn.Precio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Inicio_Inscripcion", torn.Fecha_Inicio_Inscripcion))
                .Add(New SqlParameter("@Fecha_Fin_Inscripcion", torn.Fecha_Fin_Inscripcion))
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
                Dim CommandP As SqlCommand = Acceso.MiComando("insert into Premios (ID_Torneo,Nombre,ID_Posicion,Descripcion,Valor) values (@ID_torneo,@Nombre,@ID_Posicion,@Descripcion,@Valor)")
                With CommandP.Parameters
                    .Add(New SqlParameter("@ID_torneo", torn.ID_Torneo))
                    .Add(New SqlParameter("@Nombre", premio.Nombre))
                    .Add(New SqlParameter("@ID_Posicion", premio.Posicion))
                    .Add(New SqlParameter("@Descripcion", premio.Descripcion))
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

    Public Function TraerTorneosInscripcion(game As Game) As List(Of Torneo)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Torneo,Fecha_Inicio,Fecha_Fin,Nombre,Lugar_Final, ID_Game,PRecio_Inscripcion,Fecha_Fin_Inscripcion,Fecha_Inicio_Inscripcion from Torneo where ID_game=@ID_Game")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Game", game.ID_Game))
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
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Premio,Nombre,ID_Posicion,Descripcion,Valor from Premios where ID_Torneo=@ID_Torneo ")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", Torneo.ID_Torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListPre As New List(Of Entidades.Premio)
            For Each _dr As DataRow In dt.Rows
                Dim pre As New Entidades.Premio
                pre.ID_Premio = _dr("ID_Premio")
                pre.Descripcion = _dr("Descripcion")
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

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
