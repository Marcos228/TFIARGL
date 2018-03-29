Imports System.Web.HttpContext
Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsNothing(Current.Session("cliente")) Or IsDBNull(Current.Session("Cliente")) Then
        '    ' CargarSinPerfil()
        '    'Idioma Predeterminado
        'Else
        '    Dim Usuario As Entidades.UsuarioEntidad = TryCast(Current.Session("cliente"), Entidades.UsuarioEntidad)
        '    'CargarPerfil(Usuario)
        '    'TraducirPagina(Usuario)
        'End If
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
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/RecuperarPassword.aspx"})
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/BaseCorrupta.aspx"})
        UsuarioInvitado.Perfil = PermisosInvitado
        If UsuarioInvitado.Perfil.ValidarURL(Me.Page.Request.FilePath) = False Then
            Response.Redirect("AccesoRestringido.aspx")
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
                Response.Redirect("AccesoRestringido.aspx")
            End If

        End If
    End Sub

    Private Sub ArmarMenuCompleto()
        Me.Menu.Items.Add(New MenuItem("Home", "Home", Nothing, "/Default.aspx"))
        Me.Menu.Items.Add(New MenuItem("Administración del Sistema", "AdminSist"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Crear Usuario", "AgregarUsuario", Nothing, "/AgregarUsuario.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Copia de Seguridad", "Backup", Nothing, "/backup.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Restauración de Datos", "Restore", Nothing, "/restore.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Crear Perfil", "Permisos", Nothing, "/AgregarPerfil.aspx"))
        Me.Menu.Items.Add(New MenuItem("Empresa", "Institucional", Nothing, "/Institucional.aspx"))
        Me.Menu.Items.Add(New MenuItem("Area de Cliente", "Cliente"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Carrito", "Carrito", Nothing, "/Orders.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Mis Compras", "Compras", Nothing, "/MyOrders.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Lista de Productos", "Productos", Nothing, "/ProductList.aspx"))
    End Sub

    Private Sub Menu_MenuItemClick(sender As Object, e As MenuEventArgs) Handles Menu.MenuItemClick
        Dim deslogear As Menu = TryCast(sender, Menu)

        If deslogear.SelectedItem.Text = "Logout" Then
            Current.Session("cliente") = DBNull.Value
            Response.Redirect("/Default.aspx")
        End If
    End Sub

    Protected Sub TraducirPagina(ByRef Usuario As Entidades.UsuarioEntidad)


        Try
            Dim MiIdioma As New Entidades.IdiomaEntidad

            MiIdioma = Usuario.Idioma

            Dim MiPagina As String = Right(Request.Path, Len(Request.Path) - 1)

            Session("Idioma") = MiIdioma

            Me.traducirMenu

            'Traduzco el  Copyright

            'Me.Copyright.Text = BLL.IdiomaBLL.traducirMensaje(DirectCast(Session("Usuario"), Entidades.Usuario).Idioma, 147) '147



            'Falta traducir el menu especial del usuario logeado (Fotito Arriba)

            Dim mpContentPlaceHolder As New ContentPlaceHolder
            mpContentPlaceHolder = Me.FindControl("contenidoPagina")


            'traducirControl(mpContentPlaceHolder.Controls)


        Catch ex As System.Data.SqlClient.SqlException
            'Sino referencia cíclica....
            'Session("SQLERROR") = ex.Message
            'Response.Redirect("Mensajes.aspx", False)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub traducirMenu()
        Try
            Dim MiMenuP As Menu
            MiMenuP = Me.FindControl("Menu")
            If MiMenuP.Items.Count > 0 Then
                traducirMenuRecursivo(MiMenuP.Items)
            End If

            'Me.traducir(DirectCast(Me.FindControl("lkPassword"), LinkButton))
            'Me.traducir(DirectCast(Me.FindControl("lkIdioma"), LinkButton))
            'Me.traducir(DirectCast(Me.FindControl("lkPerfil"), LinkButton))
            'Me.traducir(DirectCast(Me.FindControl("lkLogOut"), LinkButton))
        Catch ex As System.Data.SqlClient.SqlException
            'Session("SQLERROR") = ex.Message
            'Response.Redirect("Mensajes.aspx", False)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub traducir(ByVal _menuitem As MenuItem)
        Try
            For Each MiPalabra As Entidades.Palabras In CType(Session("Idioma"), Entidades.IdiomaEntidad).Palabras
                If UCase(MiPalabra.Codigo) = UCase(_menuitem.Value) Then
                    _menuitem.Text = MiPalabra.Traduccion
                    Exit For
                End If
            Next
        Catch ex As System.Data.SqlClient.SqlException
            'Session("SQLERROR") = ex.Message
            'Response.Redirect("Mensajes.aspx", False)
        Catch ex As Exception

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
        Catch ex As System.Data.SqlClient.SqlException
            Session("SQLERROR") = ex.Message
            Response.Redirect("Mensajes.aspx", False)
        Catch ex As Exception

        End Try

    End Sub

End Class