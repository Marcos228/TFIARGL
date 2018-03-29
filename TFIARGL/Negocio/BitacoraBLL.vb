Imports DAL.BitacoraDAL
Imports System.Web.HttpContext
Imports Entidades
Imports System.Threading
Imports System.Globalization
Imports System.IO

Public Class BitacoraBLL
    Private BitacoraDal As New DAL.BitacoraDAL
    Public Function listar() As List(Of Entidades.Bitacora)
        'Dim dt As DataTable = BitacoraDal.Listar
        'Dim bitacoras As New List(Of Entidades.Bitacora)
        'For Each row In dt.Rows
        '    Dim log As New Entidades.BitacoraAuditoria
        '    log.Detalle = row.Item("Descripcion")
        '    log.Fecha = row.Item("Fecha")
        '    bitacoras.Add(log)
        'Next
        'Return bitacoras
    End Function

    Public Shared Sub CrearBitacora(ByRef Bita As Bitacora)
        Dim bitdal As New DAL.BitacoraDAL
        bitdal.GuardarBitacora(Bita)
    End Sub

    Public Shared Sub ArchivarBitacora(ByRef Bitacora As Bitacora)
        Dim bitaudit As New BitacoraAuditoria
        Dim biterr As New BitacoraErrores

        If Bitacora.GetType() = bitaudit.GetType Then
            Dim Jsonarray As SerializadorJSON(Of List(Of BitacoraAuditoria)) = New SerializadorJSON(Of List(Of BitacoraAuditoria))
            If File.Exists("BitacorasAuditoria.json") Then
                Dim mistreamreader = File.Open("BitacorasAuditoria.json", FileMode.Open, FileAccess.Read)
                Dim p As List(Of BitacoraAuditoria) = Jsonarray.Deserializar(mistreamreader, New List(Of BitacoraAuditoria))
                mistreamreader.Close()
                File.Delete("BitacorasAuditoria.json")
                p.Add(Bitacora)
                Jsonarray.Serializar(p)
            Else
                Dim mistream = File.Open("BitacorasAuditoria.json", FileMode.Create)
                Dim p As New List(Of BitacoraAuditoria)
                p.Add(Bitacora)
                mistream.Close()
                Jsonarray.Serializar(p)
            End If
        Else
            Dim Jsonarray As SerializadorJSON(Of List(Of BitacoraErrores)) = New SerializadorJSON(Of List(Of BitacoraErrores))
            If File.Exists("BitacorasErrores.json") Then
                Dim mistreamreader = File.Open("BitacorasErrores.json", FileMode.Open, FileAccess.Read)
                Dim p As List(Of BitacoraErrores) = Jsonarray.Deserializar(mistreamreader, New List(Of BitacoraErrores))
                mistreamreader.Close()
                File.Delete("BitacorasErrores.json")
                p.Add(Bitacora)
                Jsonarray.Serializar(p)
            Else
                Dim mistream = File.Open("BitacorasErrores.json", FileMode.Create)
                Dim p As New List(Of BitacoraErrores)
                p.Add(Bitacora)
                mistream.Close()
                Jsonarray.Serializar(p)
            End If
        End If




    End Sub

    Public Function makeLog(log As Entidades.Bitacora) As Boolean
        Return BitacoraDal.GuardarBitacora(log)
    End Function
    Public Function makeSimpleLog(logMsg As String, Optional ByVal cliente As Entidades.UsuarioEntidad = Nothing) As Boolean
        'Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
        'Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        'Dim log As New Entidades.BitacoraAuditoria
        'log.Id_Bitacora = DAL.Acceso.TraerID("ID_bitacora", "Bitacora")
        'log.Fecha = Now
        'log.Detalle = logMsg
        'If IsNothing(cliente) Then
        '    log.Usuario = Current.Session("cliente")
        'Else
        '    log.Usuario = cliente
        'End If


        'Return Me.makeLog(log)
    End Function
End Class
