Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Text
Imports Entidades
Public Class UsuarioDAL

    Public Function Alta(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Usuario (NombreUsuario,Password,Nombre,Apellido,Fecha_Alta,Salt,Bloqueo,Intentos,ID_Idioma,ID_Perfil,Empleado,BL) OUTPUT INSERTED.ID_Usuario values (@NombreUsuario, @Password,@Nombre,@Apellido,@Fecha ,@Salt, @Bloqueo, @Intento, @Idioma, @Perfil,@Empleado,@BL)")
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
                .Add(New SqlParameter("@Password", Usuario.Password))
                .Add(New SqlParameter("@Nombre", Usuario.Nombre))
                .Add(New SqlParameter("@Apellido", Usuario.Apellido))
                .Add(New SqlParameter("@Fecha", Usuario.FechaAlta))
                .Add(New SqlParameter("@Salt", Usuario.Salt))
                .Add(New SqlParameter("@Bloqueo", Usuario.Bloqueo))
                .Add(New SqlParameter("@Intento", Usuario.Intento))
                .Add(New SqlParameter("@Idioma", Usuario.Idioma.ID_Idioma))
                .Add(New SqlParameter("@Perfil", Usuario.Perfil.ID_Permiso))
                .Add(New SqlParameter("@Empleado", Usuario.Empleado))
                .Add(New SqlParameter("@BL", False))
            End With

            Usuario.ID_Usuario = Acceso.Scalar(Command)
            Command.Dispose()
            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
            Dim Command2 As SqlCommand = Acceso.MiComando("Update Usuario set DVH=@DVH where ID_Usuario=@ID_Usuario")
            With Command2.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command2)
            Command2.Dispose()

            ActualizarDVH()

            If Usuario.Bloqueo = True Then
                CrearToken(Usuario, True)
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CrearToken(usu As UsuarioEntidad, ByVal registro As Boolean) As String

        If usu.ID_Usuario = 0 Then
            Dim Command As SqlCommand = Acceso.MiComando("Select top 1 ID_Usuario from Usuario where NombreUsuario=@NombreUsuario")
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", usu.NombreUsu))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                usu.ID_Usuario = dt.Rows(0)("ID_Usuario")
            End If
            Command.Dispose()
        End If

        Dim Command3 As SqlCommand = Acceso.MiComando("insert into Token_Usuario (ID_Token,ID_Usuario,Fecha_Expiro,Registro) values (@Token,@Usuario,@Fecha,@Registro)")
        Dim r As New Random
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim sb As New StringBuilder
        Dim cnt As Integer = r.Next(20, 35)
        For i As Integer = 1 To cnt
            Dim idx As Integer = r.Next(0, s.Length)
            sb.Append(s.Substring(idx, 1))
        Next
        With Command3.Parameters
            .Add(New SqlParameter("@Token", sb.ToString()))
            .Add(New SqlParameter("@Fecha", DateAdd(DateInterval.Day, 1, Now())))
            .Add(New SqlParameter("@Usuario", usu.ID_Usuario))
            .Add(New SqlParameter("@Registro", registro))
        End With
        Acceso.Escritura(Command3)
        Command3.Dispose()
        Return sb.ToString()
    End Function

    Public Sub LimpiarTokens(ByVal token As String)
        Dim Command As SqlCommand = Acceso.MiComando("Select u.ID_Usuario,TK.ID_Token from Usuario as U inner join Token_Usuario as TK on U.ID_usuario=Tk.ID_Usuario where TK.Fecha_Expiro<getdate() or TK.ID_Token=@Token")
        With Command.Parameters
            .Add(New SqlParameter("@Token", token))
        End With
        Dim dt As DataTable = Acceso.Lectura(Command)
        Command.Dispose()
        For Each rw As DataRow In dt.Rows
            Dim CommandEliminacion As SqlCommand = Acceso.MiComando("Delete Token_Usuario where ID_token=@token and ID_usuario=@Usuario")
            With CommandEliminacion.Parameters
                .Add(New SqlParameter("@Token", rw("ID_Token")))
                .Add(New SqlParameter("@Usuario", rw("ID_Usuario")))
            End With
            Acceso.Escritura(CommandEliminacion)
        Next
    End Sub

    Private Sub ActualizarDVH()
        Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
        Dim DataTabla = Acceso.Lectura(CommandVerificador)
        Dim Digitos As New List(Of String)
        For Each row As DataRow In DataTabla.Rows
            Digitos.Add(row.Item("DVH"))
        Next
        DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
    End Sub

    Public Function RefrescarUsuario(uSuario As UsuarioEntidad) As UsuarioEntidad
        Try
            Dim consulta As String = "Select Bloqueo,ID_Perfil from Usuario where ID_Usuario=@ID_Usuario"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", uSuario.ID_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                uSuario.Bloqueo = dt.Rows(0)("Bloqueo")
                Dim GestorPermisos As New GestorPermisosDAL
                uSuario.Perfil = GestorPermisos.ConsultarporID(dt.Rows(0)("ID_Perfil"))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Modificar(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Usuario set NombreUsuario=@NombreUsuario, ID_Idioma=@Idioma, ID_Perfil=@Perfil, DVH=@DVH where ID_Usuario=@ID_Usuario and BL=@BL")
            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
                .Add(New SqlParameter("@Idioma", Usuario.Idioma.ID_Idioma))
                .Add(New SqlParameter("@Perfil", Usuario.Perfil.ID_Permiso))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            ActualizarDVH()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetToken(ByVal ID_usuario As Integer) As String
        Dim Command As SqlCommand = Acceso.MiComando("Select TOP 1 ID_Token from Token_Usuario inner join Usuario on Usuario.ID_Usuario=Token_Usuario.ID_Usuario where Token_Usuario.ID_Usuario=@Usuario and Fecha_Expiro>Getdate() and Usuario.Bloqueo = 1 order by Fecha_Expiro desc")
        With Command.Parameters
            .Add(New SqlParameter("@Usuario", ID_usuario))
        End With
        Dim dt As DataTable = Acceso.Lectura(Command)
        Return dt.Rows(0)("ID_Token")
        Command.Dispose()
    End Function

    Public Function GetTokenUser(ByVal token As String) As Integer
        Dim Command As SqlCommand = Acceso.MiComando("Select TOP 1 Token_Usuario.ID_Usuario, Usuario.NombreUsuario from Token_Usuario inner join Usuario on Usuario.ID_Usuario=Token_Usuario.ID_Usuario where ID_Token=@Token and Fecha_Expiro>Getdate()  order by Fecha_Expiro desc")
        With Command.Parameters
            .Add(New SqlParameter("@Token", token))
        End With
        Dim dt As DataTable = Acceso.Lectura(Command)
        Command.Dispose()
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0)("ID_Usuario")
        Else
            Return Nothing
        End If


    End Function

    Public Function Eliminar(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Usuario Set BL=@BL, DVH = @DVH where ID_Usuario = @ID_Usuario")
            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(True.ToString) 'Agregado de Baja Logica
            With Command.Parameters
                .Add(New SqlParameter("@BL", True))
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            ActualizarDVH()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerUsuario(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim GestorPermisos As New GestorPermisosDAL
            Usuario.Perfil = GestorPermisos.ConsultarporID(Usuario.Perfil.ID_Permiso)
            Dim GestorIdioma As New IdiomaDAL
            Usuario.Idioma = GestorIdioma.ConsultarPorID(Usuario.Idioma.ID_Idioma)
            Return Usuario
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function ExisteUsuario(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim consulta As String = "Select * from Usuario where NombreUsuario= @NombreUsuario And BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                FormatearUsuario(Usuario, dt.Rows(0))
                Return Usuario
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function BuscarUsuarioID(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim consulta As String = "Select * from Usuario where ID_Usuario=@ID_Usuario"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                FormatearUsuario(Usuario, dt.Rows(0))
                Return Usuario
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function BuscarUsuarioIDBitacora(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim consulta As String = "Select * from Usuario where ID_Usuario= @ID_Usuario"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                FormatearUsuario(Usuario, dt.Rows(0))
                Return Usuario
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function VerificarPassword(ByVal Usuario As Entidades.UsuarioEntidad) As Boolean
        Try
            Dim consulta As String = "Select * from Usuario where NombreUsuario= @NombreUsuario And Password= @Password And BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
                .Add(New SqlParameter("@Password", Usuario.Password))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CambiarPassword(ByVal Usuario As Entidades.UsuarioEntidad) As Boolean
        Try

            Dim consulta As String = "update Usuario set Password = @Password, salt= @Salt, DVH = @DVH where ID_Usuario=@Usuario And BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
            With Command.Parameters
                .Add(New SqlParameter("@Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@Salt", Usuario.Salt))
                .Add(New SqlParameter("@Password", Usuario.Password))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command)
            ActualizarDVH()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub ResetearIntentos(ByVal Usuario As Entidades.UsuarioEntidad)
        Try
            Dim Consulta As String = "update Usuario set Intentos = @Intentos, DVH = @DVH where NombreUsuario=@NombreUsuario and BL=0"
            Dim Command = Acceso.MiComando(Consulta)
            Usuario.Intento = 0
            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
            With Command.Parameters
                .Add(New SqlParameter("@Intentos", Usuario.Intento))
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With

            Acceso.Escritura(Command)

            ActualizarDVH()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function Integridad() As String
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Usuario")
            Dim DataTabla = Acceso.Lectura(Command)
            Dim DigitosHorizontales As String = ""
            For Each row As DataRow In DataTabla.Rows
                Dim ListaColumnas As New List(Of String)
                ListaColumnas.Add(row.Item(0))
                ListaColumnas.Add(row.Item(1))
                ListaColumnas.Add(row.Item(2))
                ListaColumnas.Add(row.Item(3))
                ListaColumnas.Add(row.Item(4))
                ListaColumnas.Add(row.Item(5))
                ListaColumnas.Add(row.Item(6))
                ListaColumnas.Add(row.Item(7))

                If Not DigitoVerificadorDAL.CalcularDVH(ListaColumnas) = row.Item(8) Then
                    Return Nothing
                End If
                DigitosHorizontales = DigitosHorizontales + row.Item(8)
            Next
            Return DigitosHorizontales
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Bloquear(ByVal Usuario As Entidades.UsuarioEntidad) As Boolean
        Try
            If Usuario.Bloqueo = True Then
                Dim Consulta As String = "update Usuario set Bloqueo = 'False', DVH = @DVH where ID_Usuario=@Usuario and BL = 0"
                Dim Command = Acceso.MiComando(Consulta)
                Usuario.Bloqueo = False
                Dim ListaParametros As New List(Of String)
                Acceso.AgregarParametros(Usuario, ListaParametros)
                ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
                With Command.Parameters
                    .Add(New SqlParameter("@Usuario", Usuario.ID_Usuario))
                    .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
                End With
                Acceso.Escritura(Command)
                Usuario.Bloqueo = False
                ResetearIntentos(Usuario)
                ActualizarDVH()

                Return False
            Else
                Dim Consulta As String = "update Usuario set Bloqueo = 'True', DVH = @DVH where ID_Usuario=@Usuario and BL = 0"
                Dim Command = Acceso.MiComando(Consulta)
                Usuario.Bloqueo = True
                Dim ListaParametros As New List(Of String)
                Acceso.AgregarParametros(Usuario, ListaParametros)
                ListaParametros.Add(False.ToString) 'Agregado de Baja Logica
                With Command.Parameters
                    .Add(New SqlParameter("@Usuario", Usuario.ID_Usuario))
                    .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
                End With
                Acceso.Escritura(Command)


                ActualizarDVH()

                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub SumarIntentos(ByVal Usuario As Entidades.UsuarioEntidad)
        Try
            Dim Consulta As String = "update Usuario set Intentos = @Intentos, DVH = @DVH where NombreUsuario=@NombreUsuario and BL = 0"
            Dim Command = Acceso.MiComando(Consulta)

            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(Usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica

            With Command.Parameters
                .Add(New SqlParameter("@Intentos", Usuario.Intento))
                .Add(New SqlParameter("@NombreUsuario", Usuario.NombreUsu))
            End With
            Usuario = TraerUsuario(Usuario)

            Command.Parameters.Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))

            Acceso.Escritura(Command)

            ActualizarDVH()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function TraerUsuariosIdioma(ByVal id_idioma As Integer) As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select * from Usuario where Bloqueo=0 and BL=0 and ID_Usuario <> 0 and ID_Idioma=@ID_Idioma"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Idioma", id_idioma))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim ListaUsuario As List(Of UsuarioEntidad) = New List(Of UsuarioEntidad)
            For Each row As DataRow In dt.Rows
                Dim Usuario As UsuarioEntidad = New UsuarioEntidad
                FormatearUsuario(Usuario, row)
                ListaUsuario.Add(Usuario)
            Next
            Return ListaUsuario
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerUsuariosPerfil(ByRef ID_Perfil As Integer) As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select * from Usuario where Bloqueo=0 and BL=0 and ID_Usuario <> 0 and ID_Perfil=@ID_Perfil"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Perfil", ID_Perfil))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim ListaUsuario As List(Of UsuarioEntidad) = New List(Of UsuarioEntidad)
            For Each row As DataRow In dt.Rows
                Dim Usuario As UsuarioEntidad = New UsuarioEntidad
                FormatearUsuario(Usuario, row)
                Usuario.Password = row.Item(2)
                ListaUsuario.Add(Usuario)
            Next
            Return ListaUsuario
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerUsuarios() As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select * from Usuario where Bloqueo=0 and BL=0 and ID_Usuario <> 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim ListaUsuario As List(Of UsuarioEntidad) = New List(Of UsuarioEntidad)
            For Each row As DataRow In dt.Rows
                Dim Usuario As UsuarioEntidad = New UsuarioEntidad
                FormatearUsuario(Usuario, row)
                ListaUsuario.Add(Usuario)
            Next
            Return ListaUsuario
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerUsuariosParaBloqueo(ByRef Usuari As Entidades.UsuarioEntidad) As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select Usuario.*, Permiso.Nombre as PermN,Idioma.Nombre as IdioN from Usuario inner join Permiso on ID_Rol=ID_Perfil inner join Idioma on Usuario.ID_Idioma=Idioma.ID_Idioma where ID_Usuario <>0 and ID_Usuario <>@ID_Usuario and Usuario.BL=0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuari.ID_Usuario))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim ListaUsuario As List(Of UsuarioEntidad) = New List(Of UsuarioEntidad)
            For Each row As DataRow In dt.Rows
                Dim Usuario As UsuarioEntidad = New UsuarioEntidad
                FormatearUsuario(Usuario, row)
                Usuario.Idioma.Nombre = row("IdioN")
                Usuario.Perfil.Nombre = row("PermN")
                ListaUsuario.Add(Usuario)
            Next
            Return ListaUsuario
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub PerfilEliminadoActualizacion()
        Dim Command As SqlCommand = Acceso.MiComando("Select * from Usuario where Perfil = @PerfilEliminado and BL =@BL")
        With Command.Parameters
            .Add(New SqlParameter("@PerfilEliminado", 0))
            .Add(New SqlParameter("@BL", False))
        End With
        Dim TableUsuarios As DataTable = Acceso.Lectura(Command)
        For Each row As DataRow In TableUsuarios.Rows
            Dim usuario As UsuarioEntidad = New UsuarioEntidad
            Dim UsuarioDAL As UsuarioDAL = New UsuarioDAL
            usuario.NombreUsu = row.Item(1)
            usuario = TraerUsuario(usuario)

            Dim Command2 As SqlCommand = Acceso.MiComando("update Usuario set DVH=@DVH where ID_Usuario = @ID_Usuario and BL = @BL")

            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica

            With Command2.Parameters
                .Add(New SqlParameter("@ID_Usuario", usuario.ID_Usuario))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command2)
            ActualizarDVH()
        Next
        Command.Dispose()
    End Sub

    Public Sub IdiomaEliminadoActualizacion()
        Dim Command As SqlCommand = Acceso.MiComando("Select * from Usuario where Idioma = @Idioma and BL =@BL")
        With Command.Parameters
            .Add(New SqlParameter("@Idioma", 1))
            .Add(New SqlParameter("@BL", False))
        End With
        Dim TableUsuarios As DataTable = Acceso.Lectura(Command)
        For Each row As DataRow In TableUsuarios.Rows
            Dim usuario As UsuarioEntidad = New UsuarioEntidad
            Dim UsuarioDAL As UsuarioDAL = New UsuarioDAL
            usuario.NombreUsu = row.Item(1)
            usuario = TraerUsuario(usuario)
            Dim Command2 As SqlCommand = Acceso.MiComando("update Usuario set DVH=@DVH where ID_Usuario = @ID_Usuario and BL = @BL")

            Dim ListaParametros As New List(Of String)
            Acceso.AgregarParametros(usuario, ListaParametros)
            ListaParametros.Add(False.ToString) 'Agregado de Baja Logica

            With Command2.Parameters
                .Add(New SqlParameter("@ID_Usuario", usuario.ID_Usuario))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
            End With
            Acceso.Escritura(Command2)
            ActualizarDVH()
        Next
        Command.Dispose()
    End Sub

    Public Sub FormatearUsuario(ByVal Usuario As Entidades.UsuarioEntidad, ByVal row As DataRow)
        Try

            Usuario.ID_Usuario = row("ID_Usuario")
            Usuario.NombreUsu = row("NombreUsuario")
            Usuario.Apellido = row("Apellido")
            Usuario.Nombre = row("Nombre")
            Usuario.FechaAlta = row("Fecha_Alta")
            Usuario.Salt = row("Salt")
            Usuario.Password = row("Password")
            Usuario.Intento = row("Intentos")
            Usuario.Bloqueo = row("Bloqueo")
            Usuario.Perfil = New Entidades.PermisoCompuestoEntidad With {.ID_Permiso = row("ID_Perfil")}
            Usuario.Idioma = New Entidades.IdiomaEntidad With {.ID_Idioma = row("ID_Idioma")}
            Usuario.Empleado = row("Empleado")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ProbarConectividad() As Boolean
        Try
            Dim MiConecction As New SqlCommand
            MiConecction.Connection = Acceso.MiConexion
            MiConecction.Connection.Open()
            If MiConecction.Connection.State = ConnectionState.Open Then
                MiConecction.Connection.Close()
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
