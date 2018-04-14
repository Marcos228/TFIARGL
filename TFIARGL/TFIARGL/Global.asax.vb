Imports System.Web.SessionState
Imports System.Web.HttpContext
Public Class Global_asax
    Inherits System.Web.HttpApplication
    Dim Corrupted As New List(Of Entidades.FilaCorrupta)
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Corrupted = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
            Application_BeginRequest(Nothing, Nothing)

        Catch ex As Exception
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la sesión
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        If (Corrupted.Count > 0) Then
            Current.Session("FilasCorruptas") = Corrupted
            Response.Redirect("BaseCorrupta.aspx")
        End If
        ' Se desencadena al comienzo de cada solicitud
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

End Class