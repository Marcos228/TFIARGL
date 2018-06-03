Imports Entidades

Public Class FacturaBLL

    Public Function GenerarFactura(ByRef fact As Entidades.Factura) As Boolean
        Try
            Dim DALFACTura As New DAL.FacturaDAL
            If DALFACTura.GenerarFactura(fact) Then
                For Each dettale In fact.Detalles
                    DALFACTura.GenerarDetalle(fact, dettale)
                Next
                Return true
            End If
            Return False
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarFacturas(Optional ByVal estado As Integer = Nothing, Optional ByVal Desde As Date = Nothing, Optional ByVal Hasta As Date = Nothing, Optional ByRef Usu As String = Nothing, Optional ByRef Torneo As String = Nothing) As List(Of Entidades.Factura)
        Return (New DAL.FacturaDAL).ListarFacturas(estado, Desde, Hasta, Usu, Torneo)
    End Function

    Public Sub AprobarFacturas(listaConfirmar As List(Of Factura))
        Dim FAcuturaDAL As New DAL.FacturaDAL
        For Each Fac In listaConfirmar
            FAcuturaDAL.AprobarFactura(Fac)
        Next
    End Sub
End Class
