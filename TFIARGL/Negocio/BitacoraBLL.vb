Imports Entidades
Imports DAL
Imports System.Data.SqlClient
Imports System.Threading
Imports System.IO

Public Class BitacoraBLL
    Private _BitacoraDAL As BitacoraDAL
    Public Function ConsultarBitacora(Optional ByRef Usuario As UsuarioEntidad = Nothing, Optional ByRef Tipo As TipoBitacora = Nothing, Optional ByVal FechaInicio As DateTime = Nothing, Optional ByVal FechaFin As DateTime = Nothing) As List(Of BitacoraEntidad)
        Try
            If DigitoVerificadorBLL.Integridad Then
                _BitacoraDAL = New BitacoraDAL
                Dim BitacoraResultado As List(Of BitacoraEntidad)
                If IsNothing(Usuario) And Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora()
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If

                ElseIf Not IsNothing(Usuario) And Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(Usuario)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf IsNothing(Usuario) And Not Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(, Tipo)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf Not IsNothing(Usuario) And Not Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(Usuario, Tipo)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf IsNothing(Usuario) And Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(, , FechaInicio, FechaFin)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf Not IsNothing(Usuario) And Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(Usuario, , FechaInicio, FechaFin)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf IsNothing(Usuario) And Not Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(, Tipo, FechaInicio, FechaFin)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                ElseIf Not IsNothing(Usuario) And Not Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                    BitacoraResultado = _BitacoraDAL.ConsultarBitacora(Usuario, Tipo, FechaInicio, FechaFin)
                    If BitacoraResultado.Count > 0 Then
                        Return BitacoraResultado
                    Else
                        Throw New ExceptionNoHayBitacoras
                    End If
                Else
                    Throw New ExceptionNoHayBitacoras
                End If
            Else
                Throw New ExceptionIntegridadUsuario
            End If
        Catch ExcepcionUsuario As ExceptionIntegridadUsuario
            Throw ExcepcionUsuario
        Catch ExcepcionBitacora As ExceptionIntegridadBitacora
            Throw ExcepcionBitacora
        Catch ExcepcionEvento As ExceptionIntegridadEvento
            Throw ExcepcionEvento
        Catch NoHayBitacoras As ExceptionNoHayBitacoras
            Throw NoHayBitacoras
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
        Catch ex As Exception
            BitacoraBLL.CrearBitacora("El Metodo " & ex.TargetSite.ToString & " generó un error. Su mensaje es: " & ex.Message, TipoBitacora.Errores, (New UsuarioEntidad With {.ID_Usuario = 0, .Nombre = "Sistema"}))
            Throw ex
        End Try
    End Function

    Public Shared Sub CrearBitacora(DetalleError As String, Tipo As TipoBitacora, Usuario As UsuarioEntidad)
        Try
            Thread.CurrentThread.CurrentCulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Thread.CurrentThread.CurrentUICulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Dim BitacoraDal As BitacoraDAL = New BitacoraDAL
            Dim BitacoraEntidad As BitacoraEntidad = New BitacoraEntidad
            BitacoraEntidad = New BitacoraEntidad With {.Codigo = Tipo, .Usuario = Usuario, .Fecha = Now, .Detalle = DetalleError}
            BitacoraDal.GuardarBitacora(BitacoraEntidad)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
            ArchivarBitacora(New BitacoraEntidad With {.Codigo = Tipo, .Usuario = Usuario, .Fecha = Now, .Detalle = DetalleError})
        Catch ex As Exception
            ArchivarBitacora(New BitacoraEntidad With {.Codigo = Tipo, .Usuario = Usuario, .Fecha = Now, .Detalle = DetalleError})
            Throw ex
        End Try
    End Sub

    Public Shared Sub CrearBitacora(ByRef Bit As BitacoraEntidad)
        Try
            Thread.CurrentThread.CurrentCulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Thread.CurrentThread.CurrentUICulture = SessionBLL.SesionActual.IdiomaPredeterminado.Cultura
            Dim BitacoraDal As BitacoraDAL = New BitacoraDAL
            Dim BitacoraEntidad As BitacoraEntidad = New BitacoraEntidad
            BitacoraEntidad = New BitacoraEntidad With {.Codigo = Bit.Codigo, .Usuario = Bit.Usuario, .Fecha = Bit.Fecha, .Detalle = Bit.Detalle}
            BitacoraDal.GuardarBitacora(BitacoraEntidad)
        Catch FalloConexion As InvalidOperationException
            Throw FalloConexion
            ArchivarBitacora(Bit)
        Catch ex As Exception
            ArchivarBitacora(Bit)
            Throw ex
        End Try
    End Sub

    Public Shared Sub ArchivarBitacora(ByRef Bitacora As BitacoraEntidad)
        Dim Jsonarray As SerializadorJSON(Of List(Of BitacoraEntidad)) = New SerializadorJSON(Of List(Of BitacoraEntidad))
        If File.Exists("Bitacoras.json") Then
            Dim mistreamreader = File.Open("Bitacoras.json", FileMode.Open, FileAccess.Read)
            Dim p As List(Of BitacoraEntidad) = Jsonarray.Deserializar(mistreamreader, New List(Of BitacoraEntidad))
            mistreamreader.Close()
            File.Delete("Bitacoras.json")
            p.Add(Bitacora)
            Jsonarray.Serializar(p)
        Else
            Dim mistream = File.Open("Bitacoras.json", FileMode.Create)
            Dim p As New List(Of BitacoraEntidad)
            p.Add(Bitacora)
            mistream.Close()
            Jsonarray.Serializar(p)
        End If
    End Sub

End Class
