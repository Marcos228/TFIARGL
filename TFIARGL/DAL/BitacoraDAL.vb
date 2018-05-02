Imports System.Data.Sql
Imports System.Data.SqlClient
Public Class BitacoraDAL

    Public Function GuardarBitacora(ByRef Bitacora As Entidades.Bitacora) As Boolean
        Try
            Dim Command As SqlCommand
            If Bitacora.GetType = New Entidades.BitacoraAuditoria().GetType Then
                Dim Bitacora2 As Entidades.BitacoraAuditoria = Bitacora
                Command = Acceso.MiComando("insert into BitacoraAuditoria (Tipo_Bitacora, Fecha, Detalle, ID_Usuario,IP_Usuario, WebBrowser, Valor_Anterior, Valor_Posterior) Output Inserted.ID_Bitacora_Auditoria values (@Tipo_Bitacora,@Fecha,@Descripcion,@id_usuario,@iP_usuario, @Browser,@Valor_Anterior,@Valor_Posterior)")
                With Command.Parameters
                    .Add(New SqlParameter("@Valor_Anterior", Bitacora2.Valor_Anterior))
                    .Add(New SqlParameter("@Valor_Posterior", Bitacora2.Valor_Posterior))
                End With
            Else
                Dim Bitacora2 As Entidades.BitacoraErrores = Bitacora
                Command = Acceso.MiComando("insert into BitacoraErrores (Tipo_Bitacora, Fecha, Detalle, ID_Usuario, IP_Usuario, WebBrowser, URL, StackTrace,Exception) Output Inserted.ID_Bitacora_Errores values (@Tipo_Bitacora,@Fecha,@Descripcion,@id_usuario,@iP_usuario, @Browser,@URL,@Stack_Trace,@Exception)")
                With Command.Parameters
                    .Add(New SqlParameter("@Exception", Bitacora2.Exception))
                    .Add(New SqlParameter("@Stack_Trace", Bitacora2.StackTrace))
                    .Add(New SqlParameter("@URL", Bitacora2.URL))
                End With
            End If

            With Command.Parameters
                .Add(New SqlParameter("@Descripcion", Bitacora.Detalle))
                .Add(New SqlParameter("@Fecha", Bitacora.Fecha))
                .Add(New SqlParameter("@id_usuario", Bitacora.Usuario.ID_Usuario))
                .Add(New SqlParameter("@Tipo_Bitacora", Bitacora.Tipo_Bitacora))
                .Add(New SqlParameter("@Browser", Bitacora.Browser))
                .Add(New SqlParameter("@IP_Usuario", Bitacora.IP_Usuario))
            End With
            Dim ListaParametros As New List(Of String)
            Bitacora.Id_Bitacora = Acceso.Scalar(Command)
            Command.Dispose()

            Acceso.AgregarParametros(Bitacora, ListaParametros)
            Dim Command2 As SqlCommand
            If Bitacora.GetType = New Entidades.BitacoraAuditoria().GetType Then
                Command2 = Acceso.MiComando("Update BitacoraAuditoria set DVH=@DVH where ID_Bitacora_Auditoria=@ID_Bitacora")
            Else
                Command2 = Acceso.MiComando("Update BitacoraErrores set DVH=@DVH where ID_Bitacora_Errores=@ID_Bitacora")
            End If
            With Command2.Parameters
                    .Add(New SqlParameter("@ID_Bitacora", Bitacora.Id_Bitacora))
                    .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(ListaParametros)))
                End With
                Acceso.Escritura(Command2)
            Command2.Dispose()


            ActualizarDVH(Bitacora)

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Private Sub ActualizarDVH(ByRef Bitacora As Entidades.Bitacora)
        Dim CommandVerificador As SqlCommand
        If Bitacora.GetType = New Entidades.BitacoraAuditoria().GetType Then
            CommandVerificador = Acceso.MiComando("Select DVH from BitacoraAuditoria")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As New List(Of String)
            For Each row As DataRow In DataTabla.Rows
                Digitos.Add(row.Item("DVH"))
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "BitacoraAuditoria")
        Else
            CommandVerificador = Acceso.MiComando("Select DVH from BitacoraErrores")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As New List(Of String)
            For Each row As DataRow In DataTabla.Rows
                Digitos.Add(row.Item("DVH"))
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "BitacoraErrores")
        End If

    End Sub

    Public Function CompararDigitos(ByVal Digito1 As String) As Boolean
        Return True

    End Function

    Public Function ConsultarBitacoraAuditoria(Optional ByVal tipoBitacora As Entidades.Tipo_Bitacora = Nothing, Optional ByVal Desde As Date = Nothing, Optional ByVal Hasta As Date = Nothing, Optional ByRef Usu As Entidades.UsuarioEntidad = Nothing) As List(Of Entidades.BitacoraAuditoria)
        Try
            Dim consulta As String = ""



            If Desde = DateTime.MinValue And Hasta = DateTime.MinValue Then
                consulta = "Select Top 50 "
            Else
                consulta = "Select "
            End If
            consulta += " Valor_Anterior, Valor_Posterior, Detalle, Fecha, IP_Usuario, WebBrowser,Tipo_Bitacora, ID_Bitacora_Auditoria, ID_Usuario from BitacoraAuditoria where Fecha between isnull(@desde,'19000101') and isnull(@hasta,'99990101') and ID_Usuario=isnull(@Usuario,ID_Usuario) and Tipo_Bitacora=isnull(@TipoBitacora,Tipo_Bitacora) order by ID_Bitacora_Auditoria DESC "

            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                If Not Desde = DateTime.MinValue And Not Hasta = DateTime.MinValue Then
                    .Add(New SqlParameter("@Desde", Desde))
                    .Add(New SqlParameter("@Hasta", Hasta))
                Else
                    .Add(New SqlParameter("@Desde", DBNull.Value))
                    .Add(New SqlParameter("@Hasta", DBNull.Value))
                End If
                If tipoBitacora > 0 Then
                    .Add(New SqlParameter("@TipoBitacora", tipoBitacora))
                Else
                    .Add(New SqlParameter("@TipoBitacora", DBNull.Value))
                End If
                If Not IsNothing(Usu)  Then
                    If Usu.ID_Usuario > 0 Then
                        .Add(New SqlParameter("@Usuario", Usu.ID_Usuario))
                    Else
                        .Add(New SqlParameter("@Usuario", DBNull.Value))
                    End If
                Else
                    .Add(New SqlParameter("@Usuario", DBNull.Value))
                End If
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim listaBita As New List(Of Entidades.BitacoraAuditoria)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim Bita As New Entidades.BitacoraAuditoria
                    FormatearBitacoraAuditoria(Bita, dr)
                    listaBita.Add(Bita)
                Next
                Return listaBita
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ConsultarBitacoraErrores(ByRef Usuario As Entidades.UsuarioEntidad, ByVal Tipo As Entidades.Tipo_Bitacora, ByVal FechaInicio As DateTime, ByVal FechaFin As DateTime) As List(Of Entidades.BitacoraErrores)
        Try
            Dim consulta As String = ""

            If FechaInicio = DateTime.MinValue And FechaFin = DateTime.MinValue Then
                consulta = "Select Top 50 "
            Else
                consulta = "Select "
            End If
            consulta += " URL, Stacktrace, Exception, Detalle, Fecha, IP_Usuario, WebBrowser,Tipo_Bitacora, ID_Bitacora_Errores, ID_Usuario from BitacoraErrores where Fecha between isnull(@desde,'19000101') and isnull(@hasta,'99990101') and ID_Usuario=isnull(@Usuario,ID_Usuario) and Tipo_Bitacora=isnull(@TipoBitacora,Tipo_Bitacora) order by ID_Bitacora_Errores desc "

            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                If Not FechaInicio = DateTime.MinValue And Not FechaFin = DateTime.MinValue Then
                    .Add(New SqlParameter("@Desde", FechaInicio))
                    .Add(New SqlParameter("@Hasta", FechaFin))
                Else
                    .Add(New SqlParameter("@Desde", DBNull.Value))
                    .Add(New SqlParameter("@Hasta", DBNull.Value))
                End If
                If Tipo > 0 Then
                    .Add(New SqlParameter("@TipoBitacora", Tipo))
                Else
                    .Add(New SqlParameter("@TipoBitacora", DBNull.Value))
                End If
                If Not IsNothing(Usuario) Then
                    If Usuario.ID_Usuario > 0 Then
                        .Add(New SqlParameter("@Usuario", Usuario.ID_Usuario))
                    Else
                        .Add(New SqlParameter("@Usuario", DBNull.Value))
                    End If
                Else
                    .Add(New SqlParameter("@Usuario", DBNull.Value))
                End If
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim listaBita As New List(Of Entidades.BitacoraErrores)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim Bita As New Entidades.BitacoraErrores
                    FormatearBitacoraErrores(Bita, dr)
                    listaBita.Add(Bita)
                Next
                Return listaBita
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function Integridad() As String
        Return ""

    End Function

    Public Sub FormatearBitacoraErrores(ByVal Bita As Entidades.BitacoraErrores, ByVal row As DataRow)
        Try
            Bita.Browser = row("WebBrowser")
            Bita.Detalle = row("Detalle")
            Bita.Fecha = row("Fecha")
            Bita.Id_Bitacora = row("ID_Bitacora_Errores")
            Bita.IP_Usuario = row("IP_Usuario")
            Bita.Tipo_Bitacora = row("Tipo_Bitacora")
            Bita.URL = row("URL")
            Bita.StackTrace = row("Stacktrace")
            Bita.Exception = row("Exception")
            Dim usu As New Entidades.UsuarioEntidad
            usu.ID_Usuario = row("ID_Usuario")
            Bita.Usuario = New UsuarioDAL().BuscarUsuarioIDBitacora(usu)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub FormatearBitacoraAuditoria(ByVal Bita As Entidades.BitacoraAuditoria, ByVal row As DataRow)
        Try
            Bita.Browser = row("WebBrowser")
            Bita.Detalle = row("Detalle")
            Bita.Fecha = row("Fecha")
            Bita.Id_Bitacora = row("ID_Bitacora_Auditoria")
            Bita.IP_Usuario = row("IP_Usuario")
            Bita.Tipo_Bitacora = row("Tipo_Bitacora")
            Bita.Valor_Anterior = row("Valor_Anterior")
            Bita.Valor_Posterior = row("Valor_Posterior")
            Dim usu As New Entidades.UsuarioEntidad
            usu.ID_Usuario = row("ID_Usuario")
            Bita.Usuario = New UsuarioDAL().BuscarUsuarioIDBitacora(usu)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
