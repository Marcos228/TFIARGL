Imports System.Data.SqlClient
Imports Entidades

Public Class Rol_JugadorDAL
    Public Function TraerRolesJuego(ByRef game As Entidades.Game) As List(Of Entidades.Rol_Jugador)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Rol_Jugador, ID_Game, Nombre, ID_tipo_rol_Jugador, imagen from Rol_Jugador where ID_Game=@game")
            With Command.Parameters
                .Add(New SqlParameter("@game", game.ID_Game))
            End With
            Dim _dt As DataTable = Acceso.Lectura(Command)
            Dim ListaRoles As New List(Of Entidades.Rol_Jugador)
            For Each _dr As DataRow In _dt.Rows
                Dim RolJuego As New Entidades.Rol_Jugador
                FormatearRol(RolJuego, _dr)
                ListaRoles.Add(RolJuego)
            Next
            Return ListaRoles
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerRol(iD_Rol As Integer) As Rol_Jugador
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Rol_Jugador, ID_Game, Nombre, ID_tipo_rol_Jugador, imagen from Rol_Jugador where ID_Rol_Jugador=@ID_Rol_Jugador")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Rol_Jugador", iD_Rol))
            End With
            Dim _dt As DataTable = Acceso.Lectura(Command)
            Dim rol As New Entidades.Rol_Jugador
            If _dt.Rows.Count > 0 Then
                FormatearRol(rol, _dt.Rows(0))
                Dim GestorGame As New GameDAL
                rol.Game = GestorGame.TraerJuego(rol.Game.ID_Game)
                Return rol
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizaImagen(imagen() As Byte, id_Rol As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Rol_jugador set Imagen=@Imagen where ID_Rol_Jugador=@ID_Rol_Jugador")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Rol_Jugador", id_Rol))
                .Add(New SqlParameter("@Imagen", imagen))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FormatearRol(ByVal rolj As Entidades.Rol_Jugador, ByVal row As DataRow)
        Try
            rolj.ID_Rol = row("ID_Rol_Jugador")
            rolj.Nombre = row("Nombre")
            rolj.Game = New Entidades.Game With {.ID_Game = row("ID_Game")}
            rolj.Tipo_rol = row("ID_tipo_Rol_Jugador")
            rolj.imagen = row("Imagen")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
