Imports System.Data.SqlClient
Imports Entidades

Public Class SponsorDAL
    Public Function AltaSponsor(ByRef Sponsor As Entidades.Sponsor) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Sponsor (Nombre,Cuil,Correo) values (@Nombre,@Cuil,@Correo)")
            With Command.Parameters
                .Add(New SqlParameter("@Nombre", Sponsor.Nombre))
                .Add(New SqlParameter("@Cuil", Sponsor.CUIL))
                .Add(New SqlParameter("@Correo", Sponsor.Correo))
            End With
            Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ModificarSponsor(spons As Sponsor) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Sponsor set Nombre=@Nombre ,Cuil=@CUIL, Correo=@Correo where ID_Sponsor=@ID_Sponsor")
            With Command.Parameters
                .Add(New SqlParameter("@Nombre", spons.Nombre))
                .Add(New SqlParameter("@Cuil", spons.CUIL))
                .Add(New SqlParameter("@Correo", spons.Correo))
                .Add(New SqlParameter("@ID_Sponsor", spons.ID_Sponsor))
            End With
            Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerSponsors() As List(Of Sponsor)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Sponsor ")

            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListSpon As New List(Of Entidades.Sponsor)
            For Each _dr As DataRow In dt.Rows
                Dim spons As New Entidades.Sponsor
                FormatearSponsor(spons, _dr)
                ListSpon.Add(spons)
            Next
            Return ListSpon
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Friend Function TraerSponsorsTorneo(id_torneo As Integer) As List(Of Sponsor)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select S.* from Sponsor as S inner join Torneo_Sponsor as TS on Ts.ID_Sponsor=S.ID_Sponsor where TS.ID_Torneo=@ID_Torneo ")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", id_torneo))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim ListSpon As New List(Of Entidades.Sponsor)
            For Each _dr As DataRow In dt.Rows
                Dim spons As New Entidades.Sponsor
                FormatearSponsor(spons, _dr)
                ListSpon.Add(spons)
            Next
            Return ListSpon
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(spons As Sponsor) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Sponsor from  Sponsor where  cuil=@cuil and nombre=@nombre")
            With Command.Parameters
                .Add(New SqlParameter("@nombre", spons.Nombre))
                .Add(New SqlParameter("@cuil", spons.CUIL))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidaModificacion(spons As Sponsor) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Sponsor from Torneo_Sponsor where ID_Sponsor=@ID_Sponsor")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Sponsor", spons.ID_Sponsor))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub FormatearSponsor(ByVal Spons As Entidades.Sponsor, ByVal row As DataRow)
        Try
            Spons.ID_Sponsor = row("ID_Sponsor")
            Spons.Nombre = row("Nombre")
            Spons.CUIL = row("Cuil")
            Spons.Correo = row("Correo")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


End Class
