Imports System.Globalization
Imports System.Web.HttpContext
Public Class AgregarIdioma
    Inherits System.Web.UI.Page

    Private Sub AgregarIdioma_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarCulturas()
                CargarPalabras()
                Current.Session("Traducciones") = Nothing
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub

    Private Sub CargarPalabras()
        Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")

        If IsNothing(IdiomaActual.Palabras) Then
            Me.alertvalid.Visible = True
            Me.gv_Traducciones.DataSource = IdiomaActual.Palabras
            Me.gv_Traducciones.DataBind()
        Else
            Me.alertvalid.Visible = False
            Me.gv_Traducciones.DataSource = IdiomaActual.Palabras
            Me.gv_Traducciones.DataBind()
        End If
    End Sub

    Private Sub CargarCulturas()
        Dim culturas = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures)
        For i As Integer = 1 To culturas.Length - 1
            Dim item As New ListItem(culturas(i).Name, culturas(i).NativeName)
            Me.lstCultura.Items.Add(item)
        Next
    End Sub

    Private Sub gv_traducciones_DataBound(sender As Object, e As EventArgs) Handles gv_Traducciones.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_Traducciones.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Traducciones.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Traducciones.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Traducciones.PageSize).Selected = True


            txttotal.Text = gv_Traducciones.PageCount
            For cnt As Integer = 0 To gv_Traducciones.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Traducciones.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
            If Not IsNothing(Current.Session("Traducciones")) Then
                Dim traduccionesNuevas As Dictionary(Of Integer, String) = Current.Session("Traducciones")
                For Each rw As GridViewRow In gv_Traducciones.Rows
                    Dim txt As TextBox = CType(rw.Cells(0).FindControl("txtTraduccion"), TextBox)
                    If traduccionesNuevas.ContainsKey(rw.Cells(0).Text) Then
                        txt.Text = traduccionesNuevas(rw.Cells(0).Text)
                    End If
                Next
            End If

            gv_Traducciones.BottomPagerRow.Visible = True
            gv_Traducciones.BottomPagerRow.CssClass = "table-bottom-dark"

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Private Sub CargarIdioma(ByRef Idioma As Entidades.IdiomaEntidad)
        Dim traduccionesNuevas As Dictionary(Of Integer, String)
        If Not IsNothing(Current.Session("Traducciones")) Then
            traduccionesNuevas = Current.Session("Traducciones")
            For Each Palabra In traduccionesNuevas
                Idioma.Palabras.Add(New Entidades.Palabras With {.ID_Control = Palabra.Key, .Traduccion = Palabra.Value})
            Next
            For Each rw As GridViewRow In gv_Traducciones.Rows
                Dim txt As TextBox = CType(rw.Cells(0).FindControl("txtTraduccion"), TextBox)
                If Not traduccionesNuevas.ContainsKey(rw.Cells(0).Text) Then
                    If txt.Text <> "" Then
                        Idioma.Palabras.Add(New Entidades.Palabras With {.ID_Control = rw.Cells(0).Text, .Traduccion = txt.Text})
                    End If
                End If
            Next
        Else
            For Each rw As GridViewRow In gv_Traducciones.Rows
                Dim txt As TextBox = CType(rw.Cells(0).FindControl("txtTraduccion"), TextBox)
                If txt.Text <> "" Then
                    Idioma.Palabras.Add(New Entidades.Palabras With {.ID_Control = rw.Cells(0).Text, .Traduccion = txt.Text})
                End If
            Next
        End If


        Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
        For Each Palabra In IdiomaActual.Palabras
            If Not Idioma.Palabras.Contains(Palabra) Then
                Idioma.Palabras.Add(New Entidades.Palabras With {.ID_Control = Palabra.ID_Control, .Traduccion = Palabra.Traduccion})
            End If
        Next
    End Sub
    Protected Sub btnaddIdioma_Click(sender As Object, e As EventArgs) Handles btnaddIdioma.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try

            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            If Page.IsValid = True Then
                Dim GestorIdioma As New Negocio.IdiomaBLL
                Dim Idioma As New Entidades.IdiomaEntidad
                Idioma.Nombre = txtidioma.Text
                Idioma.Cultura = CultureInfo.GetCultureInfo(lstCultura.SelectedItem.Text)
                Idioma.Editable = True
                Idioma.Palabras = New List(Of Entidades.Palabras)
                CargarIdioma(Idioma)
                If GestorIdioma.AltaIdioma(Idioma) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddIdiomaSuccess").Traduccion & Idioma.Nombre & ".", Entidades.Tipo_Bitacora.Alta, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddIdiomalError1").Traduccion
                    Me.success.Visible = False
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
        Catch NombreUso As Negocio.ExceptionNombreEnUso

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Sub GuardarTraducciones()
        Dim traduccionesNuevas As Dictionary(Of Integer, String)
        If Not IsNothing(Current.Session("Traducciones")) Then
            traduccionesNuevas = Current.Session("Traducciones")
        Else
            traduccionesNuevas = New Dictionary(Of Integer, String)
        End If
        For Each rw As GridViewRow In gv_Traducciones.Rows
            Dim txt As TextBox = CType(rw.Cells(0).FindControl("txtTraduccion"), TextBox)
            If traduccionesNuevas.ContainsKey(rw.Cells(0).Text) Then
                traduccionesNuevas(rw.Cells(0).Text) = txt.Text
            Else
                If txt.Text <> "" Then
                    traduccionesNuevas.Add(rw.Cells(0).Text, txt.Text)
                End If
            End If
        Next
        Current.Session("Traducciones") = traduccionesNuevas
    End Sub

    Protected Sub gv_traducciones_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            GuardarTraducciones()
            CargarPalabras()
            gv_Traducciones.PageIndex = e.NewPageIndex
            gv_Traducciones.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Traducciones.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Traducciones.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Traducciones.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Traducciones.PageSize = ddl.SelectedValue
            GuardarTraducciones()
            CargarPalabras()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

End Class