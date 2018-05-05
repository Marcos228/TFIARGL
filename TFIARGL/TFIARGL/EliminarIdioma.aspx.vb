Imports System.Globalization
Imports System.Web.HttpContext
Public Class EliminarIdioma
    Inherits System.Web.UI.Page

    Private Sub AgregarIdioma_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarIdiomas()

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

    Protected Sub btndelIdioma_Click(sender As Object, e As EventArgs) Handles btndelIdioma.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Try
            Dim idiomabitacora As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                idiomabitacora = Application("Español")
            Else
                idiomabitacora = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If

            Dim GestorIdioma As New Negocio.IdiomaBLL
            Dim IdiomaActual As New Entidades.IdiomaEntidad With {.ID_Idioma = lstidioma.SelectedValue, .Nombre = lstidioma.SelectedItem.Text}
            If GestorIdioma.EliminarIdioma(IdiomaActual) Then
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")

                Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, idiomabitacora.Palabras.Find(Function(p) p.Codigo = "BitacoraDelIdiomaSuccess").Traduccion & IdiomaActual.Nombre & ".", Entidades.Tipo_Bitacora.Baja, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, "", "")
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
                Me.success.Visible = True
                Me.alertvalid.Visible = False
                CargarIdiomas()
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = idiomabitacora.Palabras.Find(Function(p) p.Codigo = "DeleteIdiomaError1").Traduccion
                Me.success.Visible = False
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub


    Private Sub lstidioma_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstidioma.SelectedIndexChanged
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad
            If IsNothing(Current.Session("Cliente")) Then
                IdiomaActual = Application("Español")
            Else
                IdiomaActual = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
            End If
            Dim GestorIdioma As New Negocio.IdiomaBLL
            Dim Gestor As New Negocio.UsuarioBLL
            Dim Lista As List(Of Entidades.UsuarioEntidad) = Gestor.TraerUsuariosIdioma(lstidioma.SelectedValue)
            If Lista.Count = 0 Then
                Lista.Add(New Entidades.UsuarioEntidad With {.NombreUsu = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "UsuariosIdiomas404").Traduccion})
                Me.gv_idiomas.DataSource = Lista
                Me.gv_idiomas.DataBind()
                Dim idiomabitacora As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    idiomabitacora = Application("Español")
                Else
                    idiomabitacora = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
                End If

                gv_idiomas.HeaderRow.Cells(0).Text = idiomabitacora.Palabras.Find(Function(p) p.Codigo = "HeaderUsuariosSeleccionados2").Traduccion
            Else
                Me.gv_idiomas.DataSource = Lista
                Me.gv_idiomas.DataBind()
                Dim idiomabitacora As Entidades.IdiomaEntidad
                If IsNothing(Current.Session("Cliente")) Then
                    idiomabitacora = Application("Español")
                Else
                    idiomabitacora = Application(TryCast(Current.Session("Cliente"), Entidades.UsuarioEntidad).Idioma.Nombre)
                End If
                gv_idiomas.HeaderRow.Cells(0).Text = idiomabitacora.Palabras.Find(Function(p) p.Codigo = "HeaderUsuariosSeleccionados2").Traduccion
            End If
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub
End Class