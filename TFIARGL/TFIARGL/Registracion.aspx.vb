Imports System.Globalization
Imports System.Web.HttpContext
Public Class Registracion
    Inherits System.Web.UI.Page


    Protected Sub Btnregistrarse_Click(sender As Object, e As EventArgs) Handles Btnregistrarse.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Dim usu As New Entidades.UsuarioEntidad
        Try
            Dim IdiomaActual As Entidades.IdiomaEntidad = Current.Session("Idioma")
            If Page.IsValid = True Then

                If IsValidEmail(txtusuario.Text) Then
                    usu.NombreUsu = txtusuario.Text

                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "RegistrError1").Traduccion
                    Me.success.Visible = False
                    Return
                End If

                If Me.txtPasswordConf.Value <> Me.txtpass.Value Then
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "RecuperoPassError2").Traduccion
                    Me.success.Visible = False
                    Return
                End If

                Dim PassSalt As List(Of String) = Negocio.EncriptarBLL.EncriptarPassword(txtpass.Value)
                usu.Nombre = txtnombre.Text
                usu.Apellido = txtapellido.Text
                usu.Salt = PassSalt.Item(0)
                usu.Password = PassSalt.Item(1)
                usu.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = 1}
                usu.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = 0}
                usu.FechaAlta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                usu.Empleado = False
                usu.Bloqueo = True
                If GestorCliente.Alta(usu) Then
                    Dim Bitac As New Entidades.BitacoraAuditoria(usu, IdiomaActual.Palabras.Find(Function(p) p.Codigo = "BitacoraRegistrSuccess1").Traduccion & usu.Nombre & ".", Entidades.Tipo_Bitacora.Modificacion, Now, Request.UserAgent, Request.UserHostAddress, "", "")
                    Negocio.BitacoraBLL.CrearBitacora(Bitac)
                    Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    Me.txtusuario.Text = ""
                    EnviarMailRegistro(GestorCliente.GEtToken(usu.ID_Usuario))
                End If
            Else
                Me.alertvalid.Visible = True
                Me.textovalid.InnerText = IdiomaActual.Palabras.Find(Function(p) p.Codigo = "FieldValidator1").Traduccion
                Me.success.Visible = False
            End If
        Catch nombreuso As Negocio.ExceptionNombreEnUso
            Me.alertvalid.Visible = True
            Me.textovalid.InnerText = nombreuso.Mensaje(Current.Session("Idioma"))
            Me.success.Visible = False
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub EnviarMailRegistro(ByVal token As String)
        Dim body As String = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("EmailTemplates/registracion.html"))
        Dim ruta As String = HttpContext.Current.Server.MapPath("Imagenes")
        Negocio.MailingBLL.enviarMailRegistroUsuario(token, body, ruta)
    End Sub

    Dim invalid As Boolean = False

    Public Function IsValidEmail(strIn As String) As Boolean
        invalid = False
        If String.IsNullOrEmpty(strIn) Then Return False

        ' Use IdnMapping class to convert Unicode domain names.
        Try
            strIn = Regex.Replace(strIn, "(@)(.+)$", AddressOf Me.DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200))
        Catch e As RegexMatchTimeoutException
            Return False
        End Try

        If invalid Then Return False

        ' Return true if strIn is in valid e-mail format.
        Try
            Return Regex.IsMatch(strIn,
                   "^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                   RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))
        Catch e As RegexMatchTimeoutException
            Return False
        End Try


    End Function
    Private Function DomainMapper(match As Match) As String
        Try
            ' IdnMapping class with default property values.
            Dim idn As New IdnMapping()

            Dim domainName As String = match.Groups(2).Value
            Try
                domainName = idn.GetAscii(domainName)
            Catch e As ArgumentException
                invalid = True
            End Try
            Return match.Groups(1).Value + domainName
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class