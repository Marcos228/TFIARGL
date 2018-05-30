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



    Private Sub FormatearGame(ByVal Game As Entidades.Game, ByVal row As DataRow)
        Try
            'Game.ID_Game = row("ID_Game")
            'Game.Nombre = row("Nombre")
            'Game.Reglas = row("Reglas")
            'Game.Descripcion = row("descripcion")
            'Game.CantJugadores = row("Cantidad_Max_Jugadores")
            'Game.Tipo_Juego = row("ID_Tipo_Game")
            'Game.Imagen = row("Imagen")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
