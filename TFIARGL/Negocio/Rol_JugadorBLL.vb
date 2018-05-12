Imports Entidades

Public Class Rol_JugadorBLL


    Public Function TraerRolesJuego(game As Entidades.Game) As List(Of Entidades.Rol_Jugador)
        Try
            Dim DALRol As New DAL.Rol_JugadorDAL
            Return DALRol.TraerRolesJuego(game)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizaImagen(ByVal imagen As Byte(), ByVal ID_Rol As Integer) As Boolean
        Try
            Dim DALRol As New DAL.Rol_JugadorDAL
            Return DALRol.ActualizaImagen(imagen, ID_Rol)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerRol(iD_Rol As Integer) As Rol_Jugador
        Try
            Dim DALRol As New DAL.Rol_JugadorDAL
            Return DALRol.TraerRol(iD_Rol)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
