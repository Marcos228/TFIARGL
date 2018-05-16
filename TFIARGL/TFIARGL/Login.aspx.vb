Imports System.IO
Imports System.Web.HttpContext
Imports System.Environment
Imports System.Net

Imports Entidades
Imports Negocio
Public Class Login
    Inherits System.Web.UI.Page
    Private GestorUsu As New UsuarioBLL

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Dim Cliente As New Entidades.UsuarioEntidad
        Dim IdiomaActual As Entidades.IdiomaEntidad
        Dim clienteLogeado As New Entidades.UsuarioEntidad
        If IsNothing(Current.Session("Cliente")) Then
            IdiomaActual = Application("Español")
        Else
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        End If
        Try
            If Page.IsValid = True Then
                Cliente.NombreUsu = txtUsuario.Text
                Cliente.Password = txtPassword.Text
                clienteLogeado = GestorUsu.ExisteUsuario(Cliente)
                Dim Bitac As New BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraLoginSuccess1").Traduccion & clienteLogeado.NombreUsu & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraLoginSuccess2").Traduccion, Tipo_Bitacora.Login, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                BitacoraBLL.CrearBitacora(Bitac)
                Current.Session("cliente") = clienteLogeado
                Me.success.Visible = True
                Me.alertvalid.Visible = False
                Response.Redirect("~/default.aspx", False)
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
        Catch UsuarioBloqueado As Negocio.ExceptionUsuarioBloqueado
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = UsuarioBloqueado.Mensaje(IdiomaActual)
            Me.success.Visible = False
        Catch UsuarioNoExiste As Negocio.ExceptionUsuarioNoExiste
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = UsuarioNoExiste.Mensaje(IdiomaActual)
            Me.success.Visible = False
        Catch Password As Negocio.ExceptionPasswordIncorrecta
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = Password.Mensaje(IdiomaActual)
            Me.success.Visible = False
            Dim Bitac As New BitacoraAuditoria(Cliente, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraLoginSuccess1").Traduccion & Cliente.NombreUsu & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraLoginSuccess3").Traduccion, Tipo_Bitacora.Login, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
            BitacoraBLL.CrearBitacora(Bitac)
        Catch ex As Exception
            Dim Bitac As New Entidades.BitacoraErrores(Cliente, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub ConsultarporBitacoras()
        Try
            'para retirar las bitacoras del archivo
            If File.Exists("BitacorasAuditoria.json") Then
                Dim Jsonarray As SerializadorJSON(Of List(Of BitacoraAuditoria)) = New SerializadorJSON(Of List(Of BitacoraAuditoria))
                Dim mistreamreader = File.Open("BitacorasAuditoria.json", FileMode.Open, FileAccess.Read)
                Dim Lstabitacoras As New List(Of BitacoraAuditoria)
                Lstabitacoras = Jsonarray.Deserializar(mistreamreader, Lstabitacoras)
                For Each bitacora As BitacoraAuditoria In Lstabitacoras
                    BitacoraBLL.CrearBitacora(bitacora)
                Next
                mistreamreader.Close()
                File.Delete("BitacorasAuditoria.json")
            ElseIf File.Exists("BitacorasErrores.json") Then
                Dim Jsonarray As SerializadorJSON(Of List(Of Entidades.BitacoraErrores)) = New SerializadorJSON(Of List(Of Entidades.BitacoraErrores))
                Dim mistreamreader = File.Open("BitacorasErrores.json", FileMode.Open, FileAccess.Read)
                Dim Lstabitacoras As New List(Of Entidades.BitacoraErrores)
                Lstabitacoras = Jsonarray.Deserializar(mistreamreader, Lstabitacoras)
                For Each bitacora As Entidades.BitacoraErrores In Lstabitacoras
                    BitacoraBLL.CrearBitacora(bitacora)
                Next
                mistreamreader.Close()
                File.Delete("BitacorasErrores.json")

            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnolvpass.Click
        Response.Redirect("/RecuperarPassword.aspx", False)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Negocio.UsuarioBLL.ProbarConectividad Then
                ConsultarporBitacoras()
            Else
                Dim IdiomaActual As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    IdiomaActual = Application("Español")
                Else
                    IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
                End If
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ErrorConexionBase").Traduccion
                Me.success.Visible = False
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnregistrarse_Click(sender As Object, e As EventArgs) Handles btnregistrarse.Click
        Response.Redirect("/Registracion.aspx", False)
    End Sub
End Class