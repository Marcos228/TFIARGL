Public Class GameBLL


    Public Function TraerJuegos(usu As Entidades.UsuarioEntidad) As List(Of Entidades.Game)
        Try
            Dim DalGame As New DAL.GameDAL
            Return DalGame.TraerJuegos(usu)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizaImagen(ByVal imagen As Byte(), ByVal ID_Game As Integer) As Boolean
        Try

            Dim DalGame As New DAL.GameDAL
            Return DalGame.ActualizaImagen(imagen, ID_Game)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
