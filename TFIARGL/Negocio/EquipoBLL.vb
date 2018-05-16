Imports Entidades

Public Class EquipoBLL

    Public Function AltaEquipo(ByRef Equipo As Entidades.Equipo) As Boolean
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            If DALEquipo.ValidaNombre(Equipo) Then
                Return DALEquipo.AltaEquipo(Equipo)
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(Equipo As Entidades.Equipo) As Boolean
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            Return DALEquipo.ValidaNombre(Equipo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Function ActualizaImagen(ByVal imagen As Byte(), ByVal ID_Game As Integer) As Boolean
    '    Try

    '        Dim DalGame As New DAL.GameDAL
    '        Return DalGame.ActualizaImagen(imagen, ID_Game)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
End Class
