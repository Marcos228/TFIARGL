Imports System.Data.SqlClient

Public Class GameDAL
    Public Function TraerJuegos(ByRef Usuario As Entidades.UsuarioEntidad) As List(Of Entidades.Game)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select game.ID_Game,game.Nombre,game.ID_Tipo_Game,game.Descripcion,game.Reglas,game.Cantidad_Max_Jugadores, game.imagen from Game left join Jugador on Jugador.ID_Game=Game.ID_Game and ID_Usuario=@usuario ")
            With Command.Parameters
                .Add(New SqlParameter("@usuario", Usuario.ID_Usuario))
            End With
            Dim _dt As DataTable = Acceso.Lectura(Command)
            Dim ListaGames As New List(Of Entidades.Game)
            For Each _dr As DataRow In _dt.Rows
                Dim Game As New Entidades.Game
                FormatearGame(Game, _dr)
                ListaGames.Add(Game)
            Next
            Return ListaGames
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizaImagen(imagen() As Byte, iD_Game As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Game set Imagen=@Imagen where ID_Game=@ID_Game")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Game", iD_Game))
                .Add(New SqlParameter("@Imagen", imagen))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearGame(ByVal Game As Entidades.Game, ByVal row As DataRow)
        Try
            Game.ID_Game = row("ID_Game")
            Game.Nombre = row("Nombre")
            Game.Reglas = row("Reglas")
            Game.Descripcion = row("descripcion")
            Game.CantJugadores = row("Cantidad_Max_Jugadores")
            Game.Tipo_Juego = row("ID_Tipo_Game")
            Game.Imagen = row("Imagen")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
