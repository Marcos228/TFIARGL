Imports System.Web.HttpContext
Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Current.Session("cliente")) Or IsDBNull(Current.Session("Cliente")) Then
            CargarSinPerfil()
            'Idioma Predeterminado
        Else
            Try
                Dim Usuario As Entidades.UsuarioEntidad = TryCast(Current.Session("cliente"), Entidades.UsuarioEntidad)
                CargarPerfil(Usuario)
                TraducirPagina(Usuario)
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub

    Private Sub CargarSinPerfil()
        If Me.Menu.Items.Count = 0 Then
            Me.Menu.Items.Add(New MenuItem("Home", "Home", Nothing, "/Default.aspx"))
            Me.Menu.Items.Add(New MenuItem("Empresa", "Institucional", Nothing, "/Institucional.aspx"))
            Me.Menu.Items.Add(New MenuItem("Login", "Login", Nothing, "/Login.aspx"))
        End If
        Dim UsuarioInvitado As New Entidades.UsuarioEntidad
        Dim PermisosInvitado As New Entidades.PermisoCompuestoEntidad
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/Default.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/Institucional.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/Login.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/AccesoRestringido.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/Registracion.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/RecuperarPassword.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/BaseCorrupta.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/ConfirmarRegistracion.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/ConfirmarRecupero.aspx"})

        UsuarioInvitado.Perfil = PermisosInvitado
        If UsuarioInvitado.Perfil.ValidarURL(Me.Page.Request.FilePath) = False Then
            Response.Redirect("AccesoRestringido.aspx", False)
        End If
    End Sub




    Private Sub CargarPerfil(ByRef Usuario As Entidades.UsuarioEntidad)
        Me.Menu.Items.Clear()
        ArmarMenuCompleto()
        Dim listaAremover As New List(Of MenuItem)
        Dim flag As Integer
        For Each pagina As MenuItem In Menu.Items
            If pagina.ChildItems.Count > 0 Then
                flag = 0
                For Each paginadentro As MenuItem In pagina.ChildItems
                    If Not Usuario.Perfil.ValidarURL(paginadentro.NavigateUrl) Then
                        listaAremover.Add(paginadentro)
                        flag += 1
                    End If
                Next
                If flag = pagina.ChildItems.Count Then
                    listaAremover.Add(pagina)
                End If
            Else
                If pagina.Text = "Home" Or pagina.Text = "Empresa" Or pagina.Text = "Login" Then
                Else
                    If Usuario.Perfil.ValidarURL(pagina.NavigateUrl) = False Then
                        listaAremover.Add(pagina)
                    End If

                End If
            End If
        Next
        For Each item As MenuItem In listaAremover
            Menu.Items.Remove(item)
            For Each subnivel As MenuItem In Menu.Items
                subnivel.ChildItems.Remove(item)
            Next
        Next

        Me.Menu.Items.Add(New MenuItem("Logout", "Logout"))
        If Me.Page.Request.FilePath = "/default.aspx" Or Me.Page.Request.FilePath = "/Institucional.aspx" Or Me.Page.Request.FilePath = "/Default.aspx" Or Me.Page.Request.FilePath = "/AccesoRestringido.aspx" Or Me.Page.Request.FilePath = "/RecuperarPassword.aspx" Or Me.Page.Request.FilePath = "/BaseCorrupta.aspx" Then

        Else

            If Usuario.Perfil.ValidarURL(Me.Page.Request.FilePath) = False Then
                Response.Redirect("AccesoRestringido.aspx", False)
            End If

        End If
    End Sub

    Private Sub ArmarMenuCompleto()
        Me.Menu.Items.Add(New MenuItem("Home", "Home", Nothing, "/Default.aspx"))
        Me.Menu.Items.Add(New MenuItem("Administración del Sistema", "AdminSist"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Copia de Seguridad", "Backup", Nothing, "/backup.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Restauración de Datos", "Restore", Nothing, "/restore.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Crear Perfil", "CrearPerfil", Nothing, "/AgregarPerfil.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Modificar Perfil", "ModificarPerfil", Nothing, "/ModificarPerfil.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Eliminar Perfil", "EliminarPerfil", Nothing, "/EliminarPerfil.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Agregar Usuario", "AgregarUsuario", Nothing, "/AgregarUsuario.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Modificar Usuario", "ModificarUsuario", Nothing, "/ModificarUsuario.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Eliminar Usuario", "EliminarUsuario", Nothing, "/EliminarUsuario.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Visualizar Bitacora Auditoria", "BitacoraAuditoria", Nothing, "/BitacoraAuditoria.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Visualizar Bitacora Errores", "BitacoraErrores", Nothing, "/BitacoraErrores.aspx"))
        Me.Menu.Items.Add(New MenuItem("Empresa", "Institucional", Nothing, "/Institucional.aspx"))
        Me.Menu.Items.Add(New MenuItem("Area de Cliente", "Cliente"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Carrito", "Carrito", Nothing, "/Orders.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Mis Compras", "Compras", Nothing, "/MyOrders.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Lista de Productos", "Productos", Nothing, "/ProductList.aspx"))

    End Sub

    Private Sub Menu_MenuItemClick(sender As Object, e As MenuEventArgs) Handles Menu.MenuItemClick
        Dim deslogear As Menu = TryCast(sender, Menu)

        If deslogear.SelectedItem.Value = "Logout" Then
            Current.Session("cliente") = DBNull.Value
            Response.Redirect("/Default.aspx", False)
        End If
    End Sub

    Protected Sub TraducirPagina(ByRef Usuario As Entidades.UsuarioEntidad)
        Try
            Dim MiIdioma As New Entidades.IdiomaEntidad
            MiIdioma = Usuario.Idioma
            Dim MiPagina As String = Right(Request.Path, Len(Request.Path) - 1)
            Session("Idioma") = MiIdioma
            Me.traducirMenu()
            Me.lblcopyright.Text = MiIdioma.Palabras.Find(Function(p) p.Codigo = "lblcopyright").Traduccion

            Dim mpContentPlaceHolder As New ContentPlaceHolder
            mpContentPlaceHolder = Me.FindControl("ContentPlaceHolder1")

            traducirControl(mpContentPlaceHolder.Controls)

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducirMenu()
        Try
            Dim MasterMenu As Menu
            MasterMenu = Me.FindControl("Menu")
            If MasterMenu.Items.Count > 0 Then
                traducirMenuRecursivo(MasterMenu.Items)
            End If

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub traducir(ByVal _menuitem As MenuItem)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Session("Idioma"), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _menuitem.Value)
            _menuitem.Text = PalabraAEncontrar.Traduccion
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducir(ByVal _button As Button)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Session("Idioma"), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _button.ID)
            _button.Text = PalabraAEncontrar.Traduccion

        Catch ex As System.Data.SqlClient.SqlException
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducir(ByVal _label As Label)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Session("Idioma"), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _label.ID)
            _label.Text = PalabraAEncontrar.Traduccion

        Catch ex As System.Data.SqlClient.SqlException
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducirMenuRecursivo(ByVal _items As MenuItemCollection)
        Try
            For Each MiMenuItem As MenuItem In _items
                Me.traducir(MiMenuItem)
                If MiMenuItem.ChildItems.Count > 0 Then
                    traducirMenuRecursivo(MiMenuItem.ChildItems)
                End If
            Next
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now, Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub traducirControl(ByVal paramListaControl As ControlCollection)
        Try
            For Each miControl As Control In paramListaControl
                If TypeOf miControl Is Button Then
                    traducir(DirectCast(miControl, Button))
                ElseIf TypeOf miControl Is Label Then
                    traducir(DirectCast(miControl, Label))
                ElseIf TypeOf miControl Is GridView Then
                    Dim ControlGrview As GridView = DirectCast(miControl, GridView)
                    For Each GrvLabel In ControlGrview.BottomPagerRow.Cells(0).Controls
                        If TypeOf GrvLabel Is Label Then
                            traducir(DirectCast(GrvLabel, Label))
                        End If
                    Next
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

End Class