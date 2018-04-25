Imports System.Web.HttpContext
Public Class AgregarUsuario
    Inherits System.Web.UI.Page

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click


        Dim GestorCliente As New Negocio.UsuarioBLL
        Dim usu As New Entidades.UsuarioEntidad
        Try
            If Page.IsValid = True Then
                usu.NombreUsu = txtusuario.Text
                usu.Nombre = txtnombre.Text
                usu.Apellido = txtapellido.Text
                Dim PassSalt As List(Of String) = Negocio.EncriptarBLL.EncriptarPassword(txtpass.Value)

                usu.Salt = PassSalt.Item(0)
                usu.Password = PassSalt.Item(1)
                usu.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = lstidioma.SelectedValue}
                usu.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = lstperfil.SelectedValue}
                usu.FechaAlta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                usu.Empleado = True
                If GestorCliente.Alta(usu) Then
                    Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                    Dim Bitac As New Entidades.BitacoraAuditoria(clienteLogeado, "Se creó el usuario " & usu.Nombre & " de forma correcta.", Entidades.Tipo_Bitacora.Alta, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = "Complete los campos requeridos"
                Me.success.Visible = False
            End If
        Catch nombreuso As Negocio.ExceptionNombreEnUso
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = nombreuso.Mensaje
            Me.success.Visible = False
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub AgregarUsuario_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                CargarPerfiles()
                CargarIdiomas()
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub
    Private Sub CargarPerfiles()
        Dim lista As List(Of Entidades.PermisoBaseEntidad)
        Dim Gestor As New Negocio.GestorPermisosBLL
        lista = Gestor.ListarFamilias(True)
        Session("Roles") = lista
        Me.lstperfil.DataSource = lista
        Me.lstperfil.DataBind()
    End Sub

    Private Sub CargarIdiomas()
        Dim lista As List(Of Entidades.IdiomaEntidad)
        Dim Gestor As New Negocio.IdiomaBLL
        lista = Gestor.ConsultarIdiomas()
        Me.lstidioma.DataSource = lista
        Me.lstidioma.DataBind()
    End Sub

End Class