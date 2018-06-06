Imports System.Web.HttpContext
Public Class CrearSponsor
    Inherits System.Web.UI.Page

    Private Sub CrearSponsor_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarSponsors()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub

    Private Sub CargarSponsors()
        Dim lista As List(Of Entidades.Sponsor)
        Dim Gestor As New Negocio.SponsorBLL
        lista = Gestor.TraerSponsors()
        Me.gv_sponsors.DataSource = lista
        Me.gv_sponsors.DataBind()
        Session("Sponsors") = lista
    End Sub

    Private Sub gv_Sponsors_DataBound(sender As Object, e As EventArgs) Handles gv_sponsors.DataBound
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            Dim ddlpage As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            Dim txttotal As Label = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("lbltotalpages"), Label)

            ddlpage.ClearSelection()
            ddlpage.Items.FindByValue(gv_sponsors.PageSize).Selected = True

            txttotal.Text = gv_sponsors.PageCount

            For cnt As Integer = 0 To gv_sponsors.PageCount - 1
                Dim curr As Integer = cnt + 1
                Dim item As New ListItem(curr.ToString())
                If cnt = gv_sponsors.PageIndex Then
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
            For Each row As GridViewRow In gv_sponsors.Rows
                Dim imagen3 As System.Web.UI.WebControls.ImageButton = DirectCast(row.FindControl("btn_editar"), System.Web.UI.WebControls.ImageButton)
                imagen3.CommandArgument = row.RowIndex
            Next

            With gv_sponsors.HeaderRow
                .Cells(0).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderNombre").Traduccion
                .Cells(1).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCUIL").Traduccion
                .Cells(2).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderCorreo").Traduccion
                .Cells(3).Text = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "HeaderAcciones").Traduccion
            End With

            gv_sponsors.BottomPagerRow.Visible = True
            gv_sponsors.BottomPagerRow.CssClass = "table-bottom-dark"
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Private Sub gv_sponsors_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_sponsors.RowCommand
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim Sponsor As Entidades.Sponsor = TryCast(Session("Sponsors"), List(Of Entidades.Sponsor))(e.CommandArgument + (gv_sponsors.PageIndex * gv_sponsors.PageSize))
            Select Case e.CommandName.ToString
                Case "E"
                    Me.id_sponsor.Value = Sponsor.ID_Sponsor
                    Me.txtcorreo.Text = Sponsor.Correo
                    Me.txtcuil.Text = Sponsor.CUIL
                    Me.txtnombre.Text = Sponsor.Nombre
            End Select
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub gv_sponsors_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            CargarSponsors()
            gv_sponsors.PageIndex = e.NewPageIndex
            gv_sponsors.DataBind()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
    Protected Sub ddlPaging_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPaging"), DropDownList)
            gv_sponsors.SetPageIndex(ddl.SelectedIndex)
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Protected Sub ddlPageSize_SelectedPageSizeChanged(sender As Object, e As EventArgs)
        Try
            Dim ddl As DropDownList = CType(gv_sponsors.BottomPagerRow.Cells(0).FindControl("ddlPageSize"), DropDownList)
            gv_sponsors.PageSize = ddl.SelectedValue
            CargarSponsors()
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click

        Dim GestorCliente As New Negocio.SponsorBLL
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            If Page.IsValid = True Then
                If IsNumeric(txtcuil.Text) And txtcuil.Text.Length = 11 Then
                    Dim SponsoRnew As New Entidades.Sponsor

                    SponsoRnew.Nombre = txtnombre.Text
                    SponsoRnew.Correo = txtcorreo.Text
                    SponsoRnew.CUIL = txtcuil.Text
                    SponsoRnew.ID_Sponsor = Me.id_sponsor.Value

                    If SponsoRnew.ID_Sponsor = 0 Then
                        If GestorCliente.AltaSponsor(SponsoRnew) Then
                            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddSponsorSuccess1").Traduccion & SponsoRnew.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                            Negocio.BitacoraBLL.CrearBitacora(Bitac)
                            Me.success.Visible = True
                            Me.success.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddSponsorSuccess1").Traduccion
                            Me.alertvalid.Visible = False
                        Else
                            Me.alertvalid.Visible = True
                            Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfJugError1").Traduccion
                            Me.success.Visible = False
                        End If
                    Else
                        If GestorCliente.ModificarSponsor(SponsoRnew) Then
                            Dim SponsorOLD As Entidades.Sponsor = TryCast(Session("Sponsors"), List(Of Entidades.Sponsor))(Me.id_sponsor.Value)
                            Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraAddSponsorSuccess2").Traduccion & SponsoRnew.Nombre & " " & IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraSuccesfully").Traduccion, Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                            Negocio.BitacoraBLL.CrearBitacora(Bitac, SponsorOLD, SponsoRnew)
                            Me.success.Visible = True
                            Me.success.InnerText =
                        Me.alertvalid.Visible = False
                        Else
                            Me.alertvalid.Visible = True
                            Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddPerfJugError1").Traduccion
                            Me.success.Visible = False
                        End If
                    End If
                    CargarSponsors()
                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "AddSponsorError1").Traduccion
                    Me.success.Visible = False
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub
    Private Function ValidarNumero(ByVal numero As String) As Boolean
        If IsNumeric(numero) Then
            If CInt(numero) >= 1 Then
                Return True
            End If
        End If
        Return False
    End Function

    Protected Sub btn_nuevo_Click(sender As Object, e As EventArgs) Handles btn_nuevo.Click
        Me.txtcorreo.Text = ""
        Me.txtcuil.Text = ""
        Me.txtnombre.Text = ""
        Me.id_sponsor.Value = 0

    End Sub
End Class