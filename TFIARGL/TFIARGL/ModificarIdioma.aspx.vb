Imports System.Globalization
Imports System.Web.HttpContext
Public Class ModificarIdioma
    Inherits System.Web.UI.Page

    Private Sub AgregarIdioma_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarCulturas()
                CargarIdiomas()
                Current.Session("Traducciones") = Nothing
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub
    Private Sub CargarIdiomas()
        Dim lista As List(Of Entidades.IdiomaEntidad)
        Dim Gestor As New Negocio.IdiomaBLL
        lista = Gestor.ConsultarIdiomasEditables()
        Me.lstidioma.DataSource = lista
        Me.lstidioma.DataBind()
        lstidioma_SelectedIndexChanged(Nothing, Nothing)
    End Sub
    Private Sub CargarPalabras(ByRef Idioma As Entidades.IdiomaEntidad)

        If IsNothing(Idioma.Palabras) Then
            Me.alertvalid.Visible = True
            Me.gv_Traducciones.DataSource = Idioma.Palabras
            Me.gv_Traducciones.DataBind()
        Else
            Me.alertvalid.Visible = False
            Me.gv_Traducciones.DataSource = Idioma.Palabras
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



        Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("IdiomaEditar")
        For Each Palabra In IdiomaActual.Palabras
            If Not Idioma.Palabras.Contains(Palabra) Then
                Idioma.Palabras.Add(New Entidades.Palabras With {.ID_Control = Palabra.ID_Control, .Traduccion = Palabra.Traduccion})
            End If
        Next
    End Sub
    Protected Sub btnmodIdioma_Click(sender As Object, e As EventArgs) Handles btnmodIdioma.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try

            Dim IdiomaActual As Entidades.IdiomaEntidad = TryCast(Current.Session("IdiomaEditar"), Entidades.IdiomaEntidad).Clone
            Dim IdiomaAnterior As Entidades.IdiomaEntidad = IdiomaActual.Clone
            If Page.IsValid = True Then
                Dim GestorIdioma As New Negocio.IdiomaBLL

                IdiomaActual.Cultura = CultureInfo.GetCultureInfo(lstCultura.SelectedItem.Text)
                IdiomaActual.Palabras = New List(Of Entidades.Palabras)
                CargarIdioma(IdiomaActual)
                If GestorIdioma.ModificarIdioma(IdiomaActual) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    If clienteLogeado.Idioma.ID_Idioma = IdiomaActual.ID_Idioma Then
                        clienteLogeado.Idioma = GestorIdioma.ConsultarPorID(IdiomaActual.ID_Idioma)
                    End If
                    Dim idiomabitacora As Entidades.IdiomaEntidad = Current.Session("Idioma")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, idiomabitacora.Palabras.Find(Function(p) p.Codigo = "BitacoraModIdiomaSuccess").Traduccion & IdiomaActual.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac, IdiomaAnterior, IdiomaActual)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    Response.Redirect("/ModificarIdioma.aspx", False)
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
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
            CargarPalabras(Current.Session("IdiomaEditar"))
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
            CargarPalabras(Current.Session("IdiomaEditar"))
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub lstidioma_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstidioma.SelectedIndexChanged
        Dim GestorIdioma As New Negocio.IdiomaBLL
        Dim Idiomatemp As Entidades.IdiomaEntidad = GestorIdioma.ConsultarPorID(lstidioma.SelectedValue)
        Current.Session("IdiomaEditar") = Idiomatemp
        lstCultura.ClearSelection()
        lstCultura.Items.FindByValue(Idiomatemp.Cultura.NativeName).Selected = True

        CargarPalabras(Current.Session("IdiomaEditar"))
        Current.Session("Traducciones") = Nothing
    End Sub
End Class