Imports System.Globalization
Imports System.Web.HttpContext
Public Class Registracion
    Inherits System.Web.UI.Page


    Protected Sub Btnregistrarse_Click(sender As Object, e As EventArgs) Handles Btnregistrarse.Click
        Dim GestorCliente As New Negocio.UsuarioBLL
        Dim usu As New Entidades.UsuarioEntidad
        Try
            If Page.IsValid = True Then

                If IsValidEmail(txtusuario.Text) Then
                    usu.NombreUsu = txtusuario.Text

                Else
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = "Debe ingresar un correo electronico valido."
                    Me.success.Visible = False
                    Return
                End If

                If Me.txtPasswordConf.Value <> Me.txtpass.Value Then
                    Me.alertvalid.Visible = True
                    Me.textovalid.InnerText = "Las contraseñas no coinciden."
                    Me.success.Visible = False
                    Return
                End If

                Dim PassSalt As List(Of String) = Negocio.EncriptarBLL.EncriptarPassword(txtpass.Value)
                usu.Nombre = txtnombre.Text
                usu.Apellido = txtapellido.Text
                usu.Salt = PassSalt.Item(0)
                    usu.Password = PassSalt.Item(1)
                    usu.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = 1}
                usu.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID = 0}
                usu.FechaAlta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                usu.Empleado = False
                If GestorCliente.Alta(usu) Then
                        Me.success.Visible = True
                    Me.alertvalid.Visible = False
                    Me.txtusuario.Text = ""
                End If
                Else
                    Me.alertvalid.Visible = True
                Me.textovalid.InnerText = "Complete los campos requeridos"
                Me.success.Visible = False
            End If
        Catch ex As Exception
        End Try
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