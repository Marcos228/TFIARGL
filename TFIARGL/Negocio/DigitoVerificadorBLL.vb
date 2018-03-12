Imports DAL
Imports System.Security.Cryptography
Imports Entidades
Imports System.Text
Imports Negocio
Imports System.Threading
Public Class DigitoVerificadorBLL
    Public Shared Function Integridad() As Boolean
        Try
            Thread.CurrentThread.CurrentCulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Thread.CurrentThread.CurrentUICulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Dim gestorbitacora As BitacoraDAL = New BitacoraDAL
            Dim gestorusuario As UsuarioDAL = New UsuarioDAL
            Dim gestorevento As EventoDAL = New EventoDAL
            Dim DHBitacora As String = ""
            Dim DHUsuario As String = ""
            Dim DHEvento As String = ""
            DHBitacora = gestorbitacora.Integridad
            DHUsuario = gestorusuario.Integridad
            DHEvento = gestorevento.integridad
            If Not IsNothing(DHBitacora) Then
                If Not IsNothing(DHUsuario) Then
                    If Not IsNothing(DhEvento) Then
                        Dim DataTabla = (New DigitoVerificadorDAL).VerificarIntegridad
                        For Each row As DataRow In DataTabla.Rows
                            If row.Item(0) = "Bitacora" And DigitoVerificadorDAL.CalcularDVH(DHBitacora) = row.Item(1) Then

                            ElseIf row.Item(0) = "Usuario" And DigitoVerificadorDAL.CalcularDVH(DHUsuario) = row.Item(1) Then

                            ElseIf row.Item(0) = "Evento" And DigitoVerificadorDAL.CalcularDVH(DHEvento) = row.Item(1) Then

                            Else
                                Return False
                            End If
                        Next
                        Return True
                    Else
                        Throw New ExceptionIntegridadEvento
                    End If
                Else
                    Throw New ExceptionIntegridadUsuario
                End If
            Else
                Throw New ExceptionIntegridadBitacora
            End If
            Return False
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Function ProbarConectividad() As Boolean
        Return (New DigitoVerificadorDAL).ProbarConectividad
    End Function
End Class
