﻿Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Public Class BitacoraErrores
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Current.Session("FilasCorruptas") = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
                If (Current.Session("FilasCorruptas").Count > 0) Then
                    Current.Session("cliente") = DBNull.Value
                    Response.Redirect("/BaseCorrupta.aspx", False)
                End If
                CargarBitacoras()
                CargarUsuarios()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try

        End If
    End Sub
    Private Sub CargarUsuarios()
        Dim lista As New List(Of Entidades.UsuarioEntidad)
        Dim Gestor As New Negocio.UsuarioBLL
        lista.Add(New Entidades.UsuarioEntidad With {.ID_Usuario = -1, .NombreUsu = "Todos"})
        lista.AddRange(Gestor.TraerUsuariosParaBloqueo)
        Me.lstusuarios.DataSource = lista
        Me.lstusuarios.DataBind()
    End Sub
    Private Sub CargarBitacoras(Optional ByVal tipoBitacora As Entidades.Tipo_Bitacora = Nothing, Optional ByVal Desde As Date = Nothing, Optional ByVal Hasta As Date = Nothing, Optional ByRef Usu As Entidades.UsuarioEntidad = Nothing)
        Dim lista As List(Of Entidades.BitacoraErrores)
        Dim Gestor As New Negocio.BitacoraBLL
        lista = Gestor.ListarBitacorasErrores(tipoBitacora, Desde, Hasta, Usu)
        If IsNothing(lista) Then
            Me.alertvalid.Visible = True
            Me.gv_Bitacora.DataSource = lista
            Me.gv_Bitacora.DataBind()
        Else
            Me.alertvalid.Visible = False
            Me.gv_Bitacora.DataSource = lista
            Me.gv_Bitacora.DataBind()
        End If


    End Sub

    Private Sub gv_Bitacora_DataBound(sender As Object, e As EventArgs) Handles gv_Bitacora.DataBound
        Try
            If Not IsNothing(gv_Bitacora.DataSource) Then


                Dim ddl As DropDownList = CType(gv_Bitacora.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
                Dim ddlpage As DropDownList = CType(gv_Bitacora.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
                Dim txttotal As Label = CType(gv_Bitacora.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

                For Each item As ListItem In ddlpage.Items
                    If item.Value = gv_Bitacora.PageSize Then
                        item.Selected = True
                    End If
                Next

                txttotal.Text = gv_Bitacora.PageCount
                For cnt As Integer = 0 To gv_Bitacora.PageCount - 1
                    Dim curr As Integer = cnt + 1
                    Dim item As New ListItem(curr.ToString())
                    If cnt = gv_Bitacora.PageIndex Then
                        item.Selected = True
                    End If

                    ddl.Items.Add(item)

                Next cnt

                gv_Bitacora.BottomPagerRow.Visible = True
                gv_Bitacora.BottomPagerRow.CssClass = "table-bottom-dark"
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_Bitacora_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            FiltrarBitacoras()
            gv_Bitacora.PageIndex = e.NewPageIndex
            gv_Bitacora.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Bitacora.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Bitacora.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Bitacora.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Bitacora.PageSize = ddl.SelectedValue
            FiltrarBitacoras()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub FiltrarBitacoras()
        Try
            Dim Desde As Date
            Dim Hasta As Date
            Dim tipo = Entidades.Tipo_Bitacora.Errores
            Dim usu As New Entidades.UsuarioEntidad
            If datepicker1.Value <> "" And datepicker2.Value <> "" Then
                Desde = datepicker1.Value
                Hasta = DateAdd(DateInterval.Day, 1, CDate(datepicker2.Value))
            End If
            If Not lstusuarios.SelectedValue = -1 Then
                usu.ID_Usuario = lstusuarios.SelectedValue
            End If
            CargarBitacoras(tipo, Desde, Hasta, usu)

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        FiltrarBitacoras()
    End Sub
End Class