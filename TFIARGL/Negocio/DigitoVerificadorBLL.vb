Public Class DigitoVerificadorBLL

    Public Shared Function VerifyAllIntegrity() As List(Of Entidades.FilaCorrupta)
        Dim tablasARevisar As New List(Of String)
        tablasARevisar.Add("Usuario")
        tablasARevisar.Add("BitacoraAuditoria")
        tablasARevisar.Add("BitacoraErrores")

        Dim isHorizontallyIntegral As Boolean = True
        Dim isVerticallyIntegral As Boolean = True
        Dim filascorruptas As New List(Of Entidades.FilaCorrupta)

        For Each t In tablasARevisar
            Dim nocorrupto As Boolean = True
            filascorruptas.AddRange(DAL.DigitoVerificadorDAL.VerifyHorizontally(t))
            If filascorruptas.Count = 0 Then
                nocorrupto = True
            Else
                nocorrupto = False
            End If
            isHorizontallyIntegral = nocorrupto And isHorizontallyIntegral
            isVerticallyIntegral = DAL.DigitoVerificadorDAL.VerifyVertically(t) And isVerticallyIntegral

            If isVerticallyIntegral = False Then
                filascorruptas.Add(New Entidades.FilaCorrupta(t, "Digito_Verificador_Vertical"))
            End If

        Next

        Return filascorruptas
    End Function

    Public Shared Sub RepareIntegrity()
        Dim tablasARevisar As New List(Of String)
        tablasARevisar.Add("Cliente")
        tablasARevisar.Add("Bitacora")

        For Each t In tablasARevisar
            DAL.DigitoVerificadorDAL.RepareIntegrity(t)
        Next
    End Sub

End Class
