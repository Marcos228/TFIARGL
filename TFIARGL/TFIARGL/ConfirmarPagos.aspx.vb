Imports System.IO
Imports System.Web.HttpContext
Public Class ConfirmarPAgos
    Inherits System.Web.UI.Page

    Private Sub ConfirmarPagos_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("FacturasSeleccionados") = New List(Of Entidades.Factura)
            If Not IsNothing(Current.Session("cliente")) And Not IsDBNull(Current.Session("Cliente")) Then
                CargarFacturas()
                CargarEstados()
            End If
        End If
    End Sub

    Private Sub CargarEstados()
        Dim IdiomaActual As Entidades.IdiomaEntidad
        If IsNothing(Current.Session("Cliente")) Then
            IdiomaActual = Application("Español")
        Else
            IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
        End If
        Dim tipo As New Entidades.Estado
        Dim itemValues As Array = System.Enum.GetValues(tipo.GetType)
        Dim itemNames As Array = System.Enum.GetNames(tipo.GetType)
        Me.lstestado.Items.Add(New ListItem(IdiomaActual.Palabras.Find(Function(p) p.Codigo = "MensajeTodos").Traduccion, -1))
        For i As Integer = 0 To itemNames.Length - 1
            Dim item As New ListItem(itemNames(i), itemValues(i))
            Me.lstestado.Items.Add(item)
        Next

    End Sub
    Private Sub CargarFacturas(Optional ByVal estado As Integer = -1, Optional ByVal Desde As Date = Nothing, Optional ByVal Hasta As Date = Nothing, Optional ByRef Usu As String = Nothing, Optional ByRef torneo As String = Nothing)
        Dim lista As List(Of Entidades.Factura)
        Dim Gestor As New Negocio.FacturaBLL
        lista = Gestor.ListarFacturas(estado, Desde, Hasta, Usu, torneo)
        If IsNothing(lista) Then
            Me.alertvalid.Visible = True
            Me.gv_Facturas.DataSource = lista
            Me.gv_Facturas.DataBind()

        Else
            Me.alertvalid.Visible = False
            Me.gv_Facturas.DataSource = lista
            Me.gv_Facturas.DataBind()
            Session("Facturas") = lista
        End If
    End Sub
    Private Sub FiltrarFActuras()
        Try
            Dim Desde As Date
            Dim Hasta As Date
            Dim tipo As New Entidades.Tipo_Pago
            Dim estado As Entidades.Estado
            Dim usu As String
            Dim torneo As String
            If datepicker1.Value <> "" And datepicker2.Value <> "" Then
                Desde = datepicker1.Value
                Hasta = DateAdd(DateInterval.Day, 1, CDate(datepicker2.Value))
            End If
            If Not lstestado.SelectedValue = -1 Then
                estado = lstestado.SelectedValue
            Else
                estado = Nothing
            End If

            If txtusuarios.Text <> "" Then
                usu = txtusuarios.Text
            Else
                usu = Nothing
            End If
            If txttorneo.Text <> "" Then
                torneo = txttorneo.Text
            Else
                torneo = Nothing
            End If
            CargarFacturas(estado, Desde, Hasta, usu, torneo)

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub gv_Facturas_DataBound(sender As Object, e As EventArgs) Handles gv_Facturas.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_Facturas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_Facturas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_Facturas.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_Facturas.PageSize).Selected = True

            txttotal.Text = gv_Facturas.PageCount

            For cnt As Integer = 0 To gv_Facturas.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_Facturas.PageIndex Then
                    item.Selected = True
                End If

                ddl.Items.Add(item)

            Next cnt
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            For Each row As GridViewRow In gv_Facturas.Rows

                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
                If row.Cells(6).Text = "Aprobado" Then
                    imagen3.Visible = False
                End If

            Next

            With gv_Facturas.HeaderRow

                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderID").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderTorneo").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderUsuario").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderFecha").Traduccion
                .Cells(4).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderTotal").Traduccion
                .Cells(5).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderTipoPago").Traduccion
                .Cells(6).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderEstado").Traduccion
                .Cells(7).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_Facturas.BottomPagerRow.Visible = True
            gv_Facturas.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub gv_Facturas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Facturas.RowCommand
        Try
            Dim Factura As Entidades.Factura = TryCast(Session("Facturas"), List(Of Entidades.Factura))(e.CommandArgument + (gv_Facturas.PageIndex * gv_Facturas.PageSize))
            Select Case e.CommandName.ToString
                Case "S"
                    If TryCast(Session("FacturasSeleccionados"), List(Of Entidades.Factura)).Any(Function(p) p.ID_Factura = Factura.ID_Factura) Then
                        gv_Facturas.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.FromName("#c3e6cb")
                        TryCast(Session("FacturasSeleccionados"), List(Of Entidades.Factura)).Remove(Factura)
                        Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_Facturas.Rows.Item(e.CommandArgument).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                        imagen3.ImageUrl = "~/Imagenes/check.png"
                    Else
                        gv_Facturas.Rows.Item(e.CommandArgument).BackColor = Drawing.Color.Cyan
                        TryCast(Session("FacturasSeleccionados"), List(Of Entidades.Factura)).Add(Factura)
                        Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(gv_Facturas.Rows.Item(e.CommandArgument).FindControl("btn_seleccionar"), System.Web.UI.WebControls.ImageButton)
                        imagen3.ImageUrl = "~/Imagenes/clear.png"
                    End If

            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_Facturas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarFacturas()
            gv_Facturas.PageIndex = e.NewPageIndex
            gv_Facturas.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Facturas.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_Facturas.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_Facturas.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_Facturas.PageSize = ddl.SelectedValue
            CargarFacturas()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Try
            Dim GestorFact As New Negocio.FacturaBLL
            Dim GestorTorneo As New Negocio.TorneoBLL

            Dim IdiomaActual As Entidades.IdiomaEntidad

            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim ListaConfirmar As List(Of Entidades.Factura) = TryCast(Session("FacturasSeleccionados"), List(Of Entidades.Factura))
            If ListaConfirmar.Count > 0 Then
                GestorFact.AprobarFacturas(ListaConfirmar)
                For Each Fact In ListaConfirmar
                    GestorTorneo.InscribirEquipo(Fact)
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraConPagosSuccess1").Traduccion & " " & Fact.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraConPagosSuccess2").Traduccion & " " & Fact.Torneo.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Dim Bitac2 As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraVisTorneoSuccess1").Traduccion & " " & Fact.Equipo.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraConPagosSuccess3").Traduccion & " " & Fact.Torneo.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac2)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                Next
            Else
                Me.alertvalid.Visible = True
                Me.alertvalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "ConPagosError1").Traduccion
                Me.success.Visible = False
            End If
             FiltrarFacturas()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        FiltrarFacturas()
    End Sub
End Class