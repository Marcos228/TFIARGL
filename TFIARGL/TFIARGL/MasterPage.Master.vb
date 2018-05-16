Imports System.Web.HttpContext
Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Current.Session("cliente")) Or IsDBNull(Current.Session("Cliente")) Then
            Dim UsuarioInvitado As New Entidades.UsuarioEntidad
            CargarSinPerfilIdioma(UsuarioInvitado)
            TraducirPagina(UsuarioInvitado)
        Else
            Try
                Dim Usuario As Entidades.UsuarioEntidad = TryCast(Current.Session("cliente"), Entidades.UsuarioEntidad)
                CargarPerfil(Usuario)
                TraducirPagina(Usuario)
            Catch ex As Exception
                Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
                Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
                Negocio.BitacoraBLL.CrearBitacora(Bitac)
            End Try
        End If
    End Sub

    Private Sub CargarSinPerfilIdioma(ByRef UsuarioInvitado As Entidades.UsuarioEntidad)
        If Me.Menu.Items.Count = 0 Then
            Me.Menu.Items.Add(New MenuItem("Home", "Home", Nothing, "/Default.aspx"))
            Me.Menu.Items.Add(New MenuItem("Empresa", "Institucional", Nothing, "/Institucional.aspx"))
            Me.Menu.Items.Add(New MenuItem("Cambiar Idioma", "SeleccionarIdioma", Nothing, "/SeleccionarIdioma.aspx"))
            Me.Menu.Items.Add(New MenuItem("Login", "Login", Nothing, "/Login.aspx"))
        End If
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
        PermisosInvitado.Hijos.Add(New Entidades.PermisoEntidad With {.URL = "/SeleccionarIdioma.aspx"})

        UsuarioInvitado.Perfil = PermisosInvitado
        Dim GestorIdioma As New Negocio.IdiomaBLL
        If IsNothing(Current.Session("Idioma")) Then
            UsuarioInvitado.Idioma = GestorIdioma.ConsultarPorCultura(Request.UserLanguages(0))
            If IsNothing(Application(UsuarioInvitado.Idioma.Nombre)) Then
                Application(UsuarioInvitado.Idioma.Nombre) = UsuarioInvitado.Idioma
            End If
        Else
            If IsNothing(Application(TryCast(Current.Session("Idioma"), Entidades.IdiomaEntidad).Nombre)) Then
                Application(TryCast(Current.Session("Idioma"), Entidades.IdiomaEntidad).Nombre) = GestorIdioma.ConsultarPorID(TryCast(Current.Session("Idioma"), Entidades.IdiomaEntidad).ID_Idioma)
            End If
            UsuarioInvitado.Idioma = Application(TryCast(Current.Session("Idioma"), Entidades.IdiomaEntidad).Nombre)
        End If

        If UsuarioInvitado.Perfil.ValidarURL(Me.Page.Request.FilePath) = False Then
            Response.Redirect("AccesoRestringido.aspx", False)
        End If
    End Sub

    Private Sub RecursividadMenu(ByRef pagina As MenuItem, ByRef Usuario As Entidades.UsuarioEntidad, ByRef ListaAremover As List(Of MenuItem))
        Dim flag As Integer = 0
        For Each paginadentro As MenuItem In pagina.ChildItems
            If paginadentro.ChildItems.Count > 0 Then
                RecursividadMenu(paginadentro, Usuario, ListaAremover)
            Else
                If Not Usuario.Perfil.ValidarURL(paginadentro.NavigateUrl) Then
                    ListaAremover.Add(paginadentro)
                    flag += 1
                End If
            End If
        Next
        If flag = pagina.ChildItems.Count Then
            ListaAremover.Add(pagina)
        End If
    End Sub


    Private Sub CargarPerfil(ByRef Usuario As Entidades.UsuarioEntidad)

        Dim GestorUsuario As New Negocio.UsuarioBLL

        GestorUsuario.RefrescarUsuario(Usuario)

        If Usuario.Bloqueo = True Then
            Current.Session("cliente") = Nothing
            Response.Redirect("/Default.aspx", False)
        End If


        Me.Menu.Items.Clear()
        ArmarMenuCompleto()
        Dim listaAremover As New List(Of MenuItem)
        For Each pagina As MenuItem In Menu.Items
            If pagina.ChildItems.Count > 0 Then
                RecursividadMenu(pagina, Usuario, listaAremover)
            Else
                If pagina.Text = "Home" Or pagina.Text = "Empresa" Or pagina.Text = "Login" Or pagina.Text = "Cambiar Idioma" Then
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
        If Me.Page.Request.FilePath = "/default.aspx" Or Me.Page.Request.FilePath = "/Institucional.aspx" Or Me.Page.Request.FilePath = "/Default.aspx" Or Me.Page.Request.FilePath = "/AccesoRestringido.aspx" Or Me.Page.Request.FilePath = "/RecuperarPassword.aspx" Or Me.Page.Request.FilePath = "/BaseCorrupta.aspx" Or Me.Page.Request.FilePath = "/SeleccionarIdioma.aspx" Then
        Else
            If Usuario.Perfil.ValidarURL(Me.Page.Request.FilePath) = False Then
                Response.Redirect("AccesoRestringido.aspx", False)
            End If
            If (Application("Corruption").Count > 0) And Me.Page.Request.FilePath <> "/Restore.aspx" Then
                Response.Redirect("BaseCorrupta.aspx", False)
            End If
        End If
    End Sub

    Private Sub ArmarMenuCompleto()
        Me.Menu.Items.Add(New MenuItem("Home", "Home", Nothing, "/Default.aspx"))
        Me.Menu.Items.Add(New MenuItem("Administración del Sistema", "AdminSist"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Copia de Seguridad", "Backup", Nothing, "/backup.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Restauración de Datos", "Restore", Nothing, "/restore.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Visualizar Bitacora Auditoria", "BitacoraAuditoria", Nothing, "/BitacoraAuditoria.aspx"))
        Me.Menu.Items.Item(1).ChildItems.Add(New MenuItem("Visualizar Bitacora Errores", "BitacoraErrores", Nothing, "/BitacoraErrores.aspx"))
        Me.Menu.Items.Add(New MenuItem("Administración Usuarios", "AdminUsu"))
        Me.Menu.Items.Item(2).ChildItems.Add(New MenuItem("Agregar Usuario", "AgregarUsuario", Nothing, "/AgregarUsuario.aspx"))
        Me.Menu.Items.Item(2).ChildItems.Add(New MenuItem("Modificar Usuario", "ModificarUsuario", Nothing, "/ModificarUsuario.aspx"))

        Me.Menu.Items.Add(New MenuItem("Administración Perfiles", "AdminPer"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Crear Perfil", "CrearPerfil", Nothing, "/AgregarPerfil.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Modificar Perfil", "ModificarPerfil", Nothing, "/ModificarPerfil.aspx"))
        Me.Menu.Items.Item(3).ChildItems.Add(New MenuItem("Eliminar Perfil", "EliminarPerfil", Nothing, "/EliminarPerfil.aspx"))

        Me.Menu.Items.Add(New MenuItem("Administración Idiomas", "AdminIdi"))
        Me.Menu.Items.Item(4).ChildItems.Add(New MenuItem("Crear Idioma", "AgregarIdioma", Nothing, "/AgregarIdioma.aspx"))
        Me.Menu.Items.Item(4).ChildItems.Add(New MenuItem("Modificar Idioma", "ModificarIdioma", Nothing, "/ModificarIdioma.aspx"))
        Me.Menu.Items.Item(4).ChildItems.Add(New MenuItem("Eliminar Idioma", "EliminarIdioma", Nothing, "/EliminarIdioma.aspx"))

        Me.Menu.Items.Add(New MenuItem("Empresa", "Institucional", Nothing, "/Institucional.aspx"))
        Me.Menu.Items.Add(New MenuItem("Area de Cliente", "Cliente"))
        Me.Menu.Items.Item(6).ChildItems.Add(New MenuItem("Carrito", "Carrito", Nothing, "/Orders.aspx"))
        Me.Menu.Items.Item(6).ChildItems.Add(New MenuItem("Mis Compras", "Compras", Nothing, "/MyOrders.aspx"))
        Me.Menu.Items.Item(6).ChildItems.Add(New MenuItem("Lista de Productos", "Productos", Nothing, "/ProductList.aspx"))

        Me.Menu.Items.Add(New MenuItem("Crear Perfil Jugador", "PerfilJugador", Nothing, "/AgregarPerfilJugador.aspx"))
        Me.Menu.Items.Add(New MenuItem("Crear Equipo", "Equipo", Nothing, "/CrearEquipo.aspx"))
        Me.Menu.Items.Add(New MenuItem("Cambiar Idioma", "SeleccionarIdioma", Nothing, "/SeleccionarIdioma.aspx"))

    End Sub

    Private Sub Menu_MenuItemClick(sender As Object, e As MenuEventArgs) Handles Menu.MenuItemClick
        Dim deslogear As Menu = TryCast(sender, Menu)

        If deslogear.SelectedItem.Value = "Logout" Then
            Current.Session("cliente") = Nothing
            Response.Redirect("/Default.aspx", False)
        End If
    End Sub

    Protected Sub TraducirPagina(ByRef Usuario As Entidades.UsuarioEntidad)
        Try

            Dim MiPagina As String = Right(Request.Path, Len(Request.Path) - 1)

            Dim GestorIdioma As New Negocio.IdiomaBLL
            If IsNothing(Application(Usuario.Idioma.Nombre)) Then
                Application(Usuario.Idioma.Nombre) = GestorIdioma.ConsultarPorID(Usuario.Idioma.ID_Idioma)
            End If

            Me.traducirMenu(Usuario.Idioma.Nombre)
            Me.lblcopyright.Text = TryCast(Application(Usuario.Idioma.Nombre), Entidades.IdiomaEntidad).Palabras.Find(Function(p) p.Codigo = "lblcopyright").Traduccion

            Dim mpContentPlaceHolder As New ContentPlaceHolder
            mpContentPlaceHolder = Me.FindControl("ContentPlaceHolder1")

            traducirControl(mpContentPlaceHolder.Controls, Usuario.Idioma.Nombre)

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducirMenu(ByVal Idioma As String)
        Try
            Dim MasterMenu As Menu
            MasterMenu = Me.FindControl("Menu")
            If MasterMenu.Items.Count > 0 Then
                traducirMenuRecursivo(MasterMenu.Items, Idioma)
            End If

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub traducir(ByVal _menuitem As MenuItem, ByVal Idioma As String)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Application(Idioma), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _menuitem.Value)
            If Not IsNothing(PalabraAEncontrar) Then
                _menuitem.Text = PalabraAEncontrar.Traduccion
            End If

        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducir(ByVal _button As Button, ByVal Idioma As String)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Application(Idioma), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _button.ID)
            If Not IsNothing(PalabraAEncontrar) Then
                _button.Text = PalabraAEncontrar.Traduccion
            End If


        Catch ex As System.Data.SqlClient.SqlException
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducir(ByVal _label As Label, ByVal Idioma As String)
        Try
            Dim LStPalabras As List(Of Entidades.Palabras) = CType(Application(Idioma), Entidades.IdiomaEntidad).Palabras
            Dim PalabraAEncontrar As Entidades.Palabras = LStPalabras.Find(Function(p) p.Codigo = _label.ID)
            If Not IsNothing(PalabraAEncontrar) Then
                _label.Text = PalabraAEncontrar.Traduccion
            End If
        Catch ex As System.Data.SqlClient.SqlException
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try
    End Sub

    Private Sub traducirMenuRecursivo(ByVal _items As MenuItemCollection, ByVal Idioma As String)
        Try
            For Each MiMenuItem As MenuItem In _items
                Me.traducir(MiMenuItem, Idioma)
                If MiMenuItem.ChildItems.Count > 0 Then
                    traducirMenuRecursivo(MiMenuItem.ChildItems, Idioma)
                End If
            Next
        Catch ex As Exception
            Dim clienteLogeado As Entidades.UsuarioEntidad = Current.Session("cliente")
            Dim Bitac As New Entidades.BitacoraErrores(clienteLogeado, ex.Message, Entidades.Tipo_Bitacora.Errores, Now.AddMilliseconds(-Now.Millisecond), Request.UserAgent, Request.UserHostAddress, ex.StackTrace, ex.GetType().ToString, Request.Url.ToString)
            Negocio.BitacoraBLL.CrearBitacora(Bitac)
        End Try

    End Sub

    Private Sub traducirControl(ByVal paramListaControl As ControlCollection, ByVal Idioma As String)
        Try
            For Each miControl As Control In paramListaControl
                If TypeOf miControl Is Button Then
                    traducir(DirectCast(miControl, Button), Idioma)
                ElseIf TypeOf miControl Is Label Then
                    traducir(DirectCast(miControl, Label), Idioma)
                ElseIf TypeOf miControl Is GridView Then
                    Dim ControlGrview As GridView = DirectCast(miControl, GridView)
                    For Each GrvLabel In ControlGrview.BottomPagerRow.Cells(0).Controls
                        If TypeOf GrvLabel Is Label Then
                            traducir(DirectCast(GrvLabel, Label), Idioma)
                        End If
                    Next
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

End Class