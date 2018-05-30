Imports Entidades

Public Class SponsorBLL

    Public Function AltaSponsor(ByRef Spons As Entidades.Sponsor) As Boolean
        Try
            Dim DALSponsor As New DAL.SponsorDAL
            If DALSponsor.ValidaNombre(Spons) Then
                Return DALSponsor.AltaSponsor(Spons)
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ModificarSponsor(ByRef Spons As Entidades.Sponsor) As Boolean
        Try
            Dim DALSponsor As New DAL.SponsorDAL
            If DALSponsor.ValidaModificacion(Spons) Then
                Return DALSponsor.ModificarSponsor(Spons)
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidaNombre(Spons As Entidades.Sponsor) As Boolean
        Try
            Dim DALSponsor As New DAL.SponsorDAL
            Return DALSponsor.ValidaNombre(Spons)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerSponsors() As List(Of Sponsor)
        Try
            Dim DALSponsor As New DAL.SponsorDAL
            Return DALSponsor.TraerSponsors()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
