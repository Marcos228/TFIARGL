﻿Imports System.IO
Imports System.Web.HttpContext
Imports System.Environment
Imports System.Net

Imports Entidades
Imports Negocio
Public Class Login
    Inherits System.Web.UI.Page
    Private GestorUsu As New UsuarioBLL

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Dim Cliente As New Entidades.UsuarioEntidad

        Try
            If Page.IsValid = True Then
                Cliente.NombreUsu = txtUsuario.Text
                Cliente.Password = txtPassword.Text
                Dim clienteLogeado = GestorUsu.ExisteUsuario(Cliente)
                Dim Bitac As New BitacoraAuditoria(clienteLogeado, "El Usuario: " & clienteLogeado.NombreUsu & " ingresó al sistema de forma correcta.", Tipo_Bitacora.Login, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                BitacoraBLL.CrearBitacora(Bitac)
                Current.Session("cliente") = clienteLogeado
                Me.success.Visible = True
                Me.alertvalid.Visible = False
                Response.Redirect("~/default.aspx", False)
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = "Complete los campos requeridos"
                Me.success.Visible = False
            End If
        Catch UsuarioBloqueado As Negocio.ExceptionUsuarioBloqueado
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = UsuarioBloqueado.Mensaje
            Me.success.Visible = False
        Catch UsuarioNoExiste As Negocio.ExceptionUsuarioNoExiste
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = UsuarioNoExiste.Mensaje
            Me.success.Visible = False
        Catch Password As Negocio.ExceptionPasswordIncorrecta
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = Password.Mensaje
            Me.success.Visible = False
            Dim clienteLogeado As UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New BitacoraAuditoria(clienteLogeado, "El Usuario: " & clienteLogeado.NombreUsu & " quiso ingresar al sistema con una contraseña invalida.", Tipo_Bitacora.Login, Now, Request.UserAgent, Request.UserHostAddress, "", "")
            BitacoraBLL.CrearBitacora(Bitac)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
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
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnpass_Click(sender As Object, e As EventArgs) Handles btnpass.Click
        Response.Redirect("/RecuperarPassword.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Negocio.UsuarioBLL.ProbarConectividad Then
                ConsultarporBitacoras()
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = "Error de Conexion a la Base de Datos Comuniquese con un administrador del sistema."
                Me.success.Visible = False
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnregistro_Click(sender As Object, e As EventArgs) Handles btnregistro.Click
        Response.Redirect("/Registracion.aspx")
    End Sub
End Class