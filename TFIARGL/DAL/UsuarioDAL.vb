Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Entidades
Public Class UsuarioDAL

    Public Function Alta(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Usuario.ID_Usuario = Acceso.TraerID("ID_Usuario", "Usuario")
            Dim Command As SqlCommand = Acceso.MiComando("insert into Usuario values (@ID_Usuario, @NombreUsuario, @Password, @Bloqueo, @Intento, @Idioma, @Perfil,@BL, @DVH)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
                .Add(New SqlParameter("@Password", Usuario.Password))
                .Add(New SqlParameter("@Bloqueo", False))
                .Add(New SqlParameter("@Intento", Usuario.Intento))
                .Add(New SqlParameter("@Idioma", Usuario.IdiomaEntidad.ID_Idioma))
                .Add(New SqlParameter("@Perfil", Usuario.Perfil.ID))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Command.Parameters("@ID_Usuario").Value & Command.Parameters("@NombreUsuario").Value & Command.Parameters("@Password").Value & Command.Parameters("@Bloqueo").Value & Command.Parameters("@Intento").Value & Command.Parameters("@Idioma").Value & Command.Parameters("@Perfil").Value & Command.Parameters("@BL").Value)))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Modificar(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Usuario set NombreUsuario=@NombreUsuario, Idioma=@Idioma, Perfil=@Perfil, DVH=@DVH where ID_Usuario=@ID_Usuario and BL=@BL")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
                .Add(New SqlParameter("@Idioma", Usuario.IdiomaEntidad.ID_Idioma))
                .Add(New SqlParameter("@Perfil", Usuario.Perfil.ID))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Command.Parameters("@ID_Usuario").Value & Command.Parameters("@NombreUsuario").Value & Usuario.Password & Usuario.Bloqueo & Usuario.Intento & Command.Parameters("@Idioma").Value & Command.Parameters("@Perfil").Value & Command.Parameters("@BL").Value)))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Eliminar(ByRef Usuario As UsuarioEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Usuario set BL=@BL, DVH = @DVH where ID_Usuario = @ID_Usuario")
            With Command.Parameters
                .Add(New SqlParameter("@BL", True))
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & Usuario.Bloqueo & Usuario.Intento & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & Command.Parameters("@BL").Value)))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()

            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TraerUsuario(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim consulta As String = "Select * from Usuario where NombreUsuario= @NombreUsuario and BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            If dt.Rows.Count > 0 Then
                FormatearUsuario(Usuario, dt.Rows(0))
                Usuario.Password = dt.Rows(0).Item(2)
                Return Usuario
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function ExisteUsuario(ByVal Usuario As Entidades.UsuarioEntidad) As Entidades.UsuarioEntidad
        Try
            Dim consulta As String = "Select * from Usuario where NombreUsuario= @NombreUsuario and BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
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
            Dim consulta As String = "Select * from Usuario where ID_Usuario= @ID_Usuario and BL = 0"
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
            Dim consulta As String = "Select * from Usuario where NombreUsuario= @NombreUsuario and Password= @Password and BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
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
            Dim consulta As String = "update Usuario set Password= @Password, DVH = @DVH where ID_Usuario=@ID_Usuario and BL = 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@Password", Usuario.Password))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & Usuario.Bloqueo & Usuario.Intento & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & False)))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub ResetearIntentos(ByVal Usuario As Entidades.UsuarioEntidad)
        Try
            Dim Consulta As String = "update Usuario set Intentos = @Intentos, DVH = @DVH where NombreUsuario=@NombreUsuario and BL=0"
            Dim Command = Acceso.MiComando(Consulta)
            With Command.Parameters
                .Add(New SqlParameter("@Intentos", 0))
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & Usuario.Bloqueo & Usuario.Intento & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & False)))
            End With
            Acceso.Escritura(Command)

            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
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
                Dim Fila As String = ""
                Fila = Fila & row.Item(0)
                Fila = Fila & row.Item(1)
                Fila = Fila & row.Item(2)
                Fila = Fila & row.Item(3)
                Fila = Fila & row.Item(4)
                Fila = Fila & row.Item(5)
                Fila = Fila & row.Item(6)
                Fila = Fila & row.Item(7)
                If Not DigitoVerificadorDAL.CalcularDVH(Fila) = row.Item(8) Then
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
                Dim Consulta As String = "update Usuario set Bloqueo = 'False', DVH = @DVH where NombreUsuario=@NombreUsuario and BL = 0"
                Dim Command = Acceso.MiComando(Consulta)
                With Command.Parameters
                    .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
                    .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & False & Usuario.Intento & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & False)))
                End With
                Acceso.Escritura(Command)
                Usuario.Bloqueo = False
                ResetearIntentos(Usuario)
                Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
                Dim DataTabla = Acceso.Lectura(CommandVerificador)
                Dim Digitos As String = ""
                For Each row As DataRow In DataTabla.Rows
                    Digitos = Digitos + row.Item("DVH")
                Next
                DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")

                Return False
            Else
                Dim Consulta As String = "update Usuario set Bloqueo = 'True', DVH = @DVH where NombreUsuario=@NombreUsuario and BL = 0"
                Dim Command = Acceso.MiComando(Consulta)
                With Command.Parameters
                    .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
                    .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & True & Usuario.Intento & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & False)))
                End With
                Acceso.Escritura(Command)


                Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
                Dim DataTabla = Acceso.Lectura(CommandVerificador)
                Dim Digitos As String = ""
                For Each row As DataRow In DataTabla.Rows
                    Digitos = Digitos + row.Item("DVH")
                Next
                DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
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
            With Command.Parameters
                .Add(New SqlParameter("@Intentos", Usuario.Intento))
                .Add(New SqlParameter("@NombreUsuario", Usuario.Nombre))
            End With
            Usuario = TraerUsuario(Usuario)
            Command.Parameters.Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & Usuario.Bloqueo & Command.Parameters("@Intentos").Value & Usuario.IdiomaEntidad.ID_Idioma & Usuario.Perfil.ID & False)))

            Acceso.Escritura(Command)

            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function TraerUsuarios() As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select * from Usuario where Bloqueo=0 and BL=0 and ID_Usuario <> 0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
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

    Public Function TraerUsuariosParaBloqueo() As List(Of UsuarioEntidad)
        Try
            Dim consulta As String = "Select * from Usuario where BL=0 and ID_Usuario <>0"
            Dim Command As SqlCommand = Acceso.MiComando(consulta)
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
            usuario.Nombre = row.Item(1)
            usuario = TraerUsuario(usuario)
            Dim Command2 As SqlCommand = Acceso.MiComando("update Usuario set DVH=@DVH where ID_Usuario = @ID_Usuario and BL = @BL")
            With Command2.Parameters
                .Add(New SqlParameter("@ID_Usuario", usuario.ID_Usuario))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(usuario.ID_Usuario & usuario.Nombre & usuario.Password & usuario.Bloqueo & usuario.Intento & usuario.IdiomaEntidad.ID_Idioma & usuario.Perfil.ID & Command.Parameters("@BL").Value)))
            End With
            Acceso.Escritura(Command2)
            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row2 As DataRow In DataTabla.Rows
                Digitos = Digitos + row2.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
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
            usuario.Nombre = row.Item(1)
            usuario = TraerUsuario(usuario)
            Dim Command2 As SqlCommand = Acceso.MiComando("update Usuario set DVH=@DVH where ID_Usuario = @ID_Usuario and BL = @BL")
            With Command2.Parameters
                .Add(New SqlParameter("@ID_Usuario", usuario.ID_Usuario))
                .Add(New SqlParameter("@BL", False))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(usuario.ID_Usuario & usuario.Nombre & usuario.Password & usuario.Bloqueo & usuario.Intento & usuario.IdiomaEntidad.ID_Idioma & usuario.Perfil.ID & Command.Parameters("@BL").Value)))
            End With
            Acceso.Escritura(Command2)
            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Usuario")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row2 As DataRow In DataTabla.Rows
                Digitos = Digitos + row2.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Usuario")
        Next
        Command.Dispose()
    End Sub

    Public Sub FormatearUsuario(ByVal Usuario As Entidades.UsuarioEntidad, ByVal row As DataRow)
        Try
            Usuario.ID_Usuario = row(0)
            Usuario.Nombre = row(1)
            Usuario.Bloqueo = row(3)
            Usuario.Intento = row(4)
            Usuario.IdiomaEntidad = New Entidades.IdiomaEntidad With {.ID_Idioma = row(5)}
            Usuario.Perfil = New Entidades.GrupoPermisoEntidad With {.ID = row(6)}
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ProbarConectividad() As Boolean

    End Function

End Class
