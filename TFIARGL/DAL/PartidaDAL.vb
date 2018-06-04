Imports System.Data.SqlClient
Imports Entidades

Public Class PartidaDAL
    Public Function AltaPartida(ByRef part As Entidades.Partida, ByVal ID_Torneo As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Partida (ID_Torneo,ID_Equipo_Local,ID_Equipo_Visitante,ID_FAse) OUTPUT INSERTED.ID_PArtida values (@ID_Torneo,@ID_Equipo_Local,@ID_Equipo_Visitante,@ID_FAse)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", ID_Torneo))
                If part.Equipos.Count = 0 Then
                    .Add(New SqlParameter("@ID_Equipo_Local", DBNull.Value))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", DBNull.Value))
                ElseIf part.Equipos.Count = 1 Then
                    .Add(New SqlParameter("@ID_Equipo_Local", part.Equipos(0).ID_Equipo))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", DBNull.Value))
                Else
                    .Add(New SqlParameter("@ID_Equipo_Local", part.Equipos(0).ID_Equipo))
                    .Add(New SqlParameter("@ID_Equipo_Visitante", part.Equipos(1).ID_Equipo))
                End If

                .Add(New SqlParameter("@ID_FAse", part.Fase))
            End With
            part.ID_Partida = Acceso.Scalar(Command)
            Command.Dispose()


            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function



End Class
