Imports Entidades
Imports System.Data.SqlClient
Public Class BitacoraDAL
    Public Function ConsultarBitacora(Optional ByRef Usuario As UsuarioEntidad = Nothing, Optional ByRef Tipo As TipoBitacora = Nothing, Optional ByVal FechaInicio As DateTime = Nothing, Optional ByVal FechaFin As DateTime = Nothing) As List(Of Entidades.BitacoraEntidad)
        Try
            Dim Command As SqlCommand
            Dim DataTabla As DataTable
            Dim listabitacora As List(Of BitacoraEntidad) = New List(Of BitacoraEntidad)
            If IsNothing(Usuario) And Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                Command = Acceso.MiComando("select * from Bitacora order by Fecha desc")
            ElseIf Not IsNothing(Usuario) And Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                Command = Acceso.MiComando("select * from Bitacora where ID_Usuario=@ID_Usuario order by Fecha desc")
                Command.Parameters.Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
            ElseIf IsNothing(Usuario) And Not Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                Command = Acceso.MiComando("select * from Bitacora where Codigo=@Codigo order by Fecha desc")
                Command.Parameters.Add(New SqlParameter("@Codigo", Tipo))
            ElseIf Not IsNothing(Usuario) And Not Tipo = 0 And FechaFin = New Date And FechaInicio = New Date Then
                Command = Acceso.MiComando("select * from Bitacora where ID_Usuario=@ID_Usuario and Codigo=@Codigo order by Fecha desc")
                Command.Parameters.Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                Command.Parameters.Add(New SqlParameter("@Codigo", Tipo))
            ElseIf IsNothing(Usuario) And Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                If FechaInicio.DayOfYear = FechaFin.DayOfYear Then
                    Command = Acceso.MiComando("select * from Bitacora where  CAST(Fecha AS DATE)=@Fecha  order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@Fecha", FechaInicio.Date))
                    End With
                Else
                    Command = Acceso.MiComando("select * from Bitacora where Fecha between @FechaIni and @FechaFin order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@FechaIni", FechaInicio.Date))
                        .Add(New SqlParameter("@FechaFin", FechaFin.Date))
                    End With
                End If
            ElseIf Not IsNothing(Usuario) And Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                If FechaInicio.DayOfYear = FechaFin.DayOfYear Then
                    Command = Acceso.MiComando("select * from Bitacora where CAST(Fecha AS DATE)=@Fecha and ID_Usuario = @ID_Usuario order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@Fecha", FechaInicio.Date))
                        .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                    End With
                Else
                    Command = Acceso.MiComando("select * from Bitacora where Fecha between @FechaIni and @FechaFin and ID_Usuario = @ID_Usuario order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@FechaIni", FechaInicio.Date))
                        .Add(New SqlParameter("@FechaFin", FechaFin.Date))
                        .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                    End With
                End If
            ElseIf IsNothing(Usuario) And Not Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                If FechaInicio.DayOfYear = FechaFin.DayOfYear Then
                    Command = Acceso.MiComando("select * from Bitacora where CAST(Fecha AS DATE)=@Fecha and Codigo = @Codigo  order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@Fecha", FechaInicio.Date))
                        .Add(New SqlParameter("@Codigo", Tipo))
                    End With
                Else
                    Command = Acceso.MiComando("select * from Bitacora where Fecha between @FechaIni and @FechaFin and Codigo = @Codigo order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@FechaIni", FechaInicio.Date))
                        .Add(New SqlParameter("@FechaFin", FechaFin.Date))
                        .Add(New SqlParameter("@Codigo", Tipo))
                    End With
                End If
            ElseIf Not IsNothing(Usuario) And Not Tipo = 0 And Not FechaFin = New Date And Not FechaInicio = New Date Then
                If FechaInicio.DayOfYear = FechaFin.DayOfYear Then
                    Command = Acceso.MiComando("select * from Bitacora where CAST(Fecha AS DATE)=@Fecha and ID_Usuario=@ID_Usuario and Codigo = @Codigo  order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@Fecha", FechaInicio.Date))
                        .Add(New SqlParameter("@Codigo", Tipo))
                        .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                    End With
                Else
                    Command = Acceso.MiComando("select * from Bitacora where Fecha between @FechaIni and @FechaFin and ID_Usuario=@ID_Usuario and Codigo = @Codigo order by Fecha desc")
                    With Command.Parameters
                        .Add(New SqlParameter("@FechaIni", FechaInicio.Date))
                        .Add(New SqlParameter("@FechaFin", FechaFin.Date))
                        .Add(New SqlParameter("@Codigo", Tipo))
                        .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                    End With
                End If
            Else
                Throw New Exception
            End If
            DataTabla = Acceso.Lectura(Command)
            For Each row As DataRow In DataTabla.Rows
                Dim Bita As New BitacoraEntidad
                Bita.Codigo = row.Item("Codigo")
                Bita.Fecha = row.Item("Fecha")
                Bita.Detalle = row.Item("Detalle")
                Dim UsuDAl As UsuarioDAL = New UsuarioDAL
                Bita.Usuario = UsuDAl.BuscarUsuarioIDBitacora(New UsuarioEntidad With {.ID_Usuario = row.Item("ID_Usuario")})
                listabitacora.Add(Bita)
            Next
            Return listabitacora
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GuardarBitacora(Bitacora As BitacoraEntidad)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Bitacora values (@ID_Bitacora, @Codigo, @Fecha, @Detalle, @ID_Usuario,@DVH)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Bitacora", Acceso.TraerID("ID_Bitacora", "Bitacora")))
                .Add(New SqlParameter("@Codigo", Bitacora.Codigo))
                .Add(New SqlParameter("@Fecha", Bitacora.Fecha))
                .Add(New SqlParameter("@Detalle", Bitacora.Detalle))
                .Add(New SqlParameter("@ID_Usuario", Bitacora.Usuario.ID_Usuario))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Command.Parameters("@ID_Bitacora").Value & Bitacora.Codigo & Bitacora.Fecha & Bitacora.Detalle & Bitacora.Usuario.ID_Usuario.ToString)))
            End With
            Acceso.Escritura(Command)

            Dim CommandVerificador As SqlCommand = Acceso.MiComando("Select DVH from Bitacora")
            Dim DataTabla = Acceso.Lectura(CommandVerificador)
            Dim Digitos As String = ""
            For Each row As DataRow In DataTabla.Rows
                Digitos = Digitos + row.Item("DVH")
            Next
            DigitoVerificadorDAL.CalcularDVV(Digitos, "Bitacora")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Integridad() As String
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Bitacora")
            Dim DataTabla = Acceso.Lectura(Command)
            Dim DigitosHorizontales As String = ""
            For Each row As DataRow In DataTabla.Rows
                Dim Fila As String = ""
                Fila = Fila & row.Item(0)
                Fila = Fila & row.Item(1)
                Fila = Fila & row.Item(2)
                Fila = Fila & row.Item(3)
                Fila = Fila & row.Item(4)
                If Not DigitoVerificadorDAL.CalcularDVH(Fila) = row.Item(5) Then
                    Return Nothing
                End If
                DigitosHorizontales = DigitosHorizontales & row.Item(5)
            Next
            Return DigitosHorizontales
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
