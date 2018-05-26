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

    Public Function TraerEquiposSolicitud(nombre As String, gam As Entidades.Game) As List(Of Equipo)
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            Return DALEquipo.TraerEquiposSolicitud(nombre, gam)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraeSolicitudesEquipo(jugador As Jugador) As List(Of Solicitudes)
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            Return DALEquipo.TraeSolicitudesEquipo(jugador)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerEquipoJugador(iD_Jugador As Integer) As Equipo
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            Return DALEquipo.TraerEquipoJugador(iD_Jugador)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EnviarSolicitud(txtmensaje As String, jugador As Jugador, equipo As Equipo, jugador_a_equipo As Boolean) As Boolean
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            Return DALEquipo.EnviarSolicitud(txtmensaje, jugador, equipo, jugador_a_equipo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AgregarJugador(solicitud As Solicitudes) As Boolean
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            If DALEquipo.ValidaSolicitud(solicitud) Then
                Return DALEquipo.AgregarJugador(solicitud)
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub RechazarSolicitud(solicitud As Solicitudes)
        Try
            Dim DALEquipo As New DAL.EquipoDAL
            DALEquipo.RechazarSolicitud(solicitud)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Public Function ActualizaImagen(ByVal imagen As Byte(), ByVal ID_Game As Integer) As Boolean
    '    Try

    '        Dim DalGame As New DAL.GameDAL
    '        Return DalGame.ActualizaImagen(imagen, ID_Game)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
End Class
