Imports System.Data.SqlClient
Imports Entidades

Public Class FacturaDAL
    Public Function GenerarFactura(ByRef fact As Entidades.Factura) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Factura (ID_Torneo,ID_usuario,ID_Tipo_Pago,Monto_Total,Fecha,ID_Equipo,Estado) OUTPUT INSERTED.ID_Factura values (@ID_Torneo,@ID_usuario,@ID_Tipo_Pago,@Monto_Total,@Fecha,@ID_Equipo,@estado)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Torneo", fact.Torneo.ID_Torneo))
                .Add(New SqlParameter("@ID_usuario", fact.Usuario.ID_Usuario))
                .Add(New SqlParameter("@ID_Tipo_Pago", fact.Tipo_Pago.Tipo_Pago))
                .Add(New SqlParameter("@Monto_Total", fact.Monto_Total))
                .Add(New SqlParameter("@Fecha", fact.Fecha))
                .Add(New SqlParameter("@ID_Equipo", fact.Equipo.ID_Equipo))
                .Add(New SqlParameter("@Estado", fact.Estado))
            End With
            fact.ID_Factura = Acceso.Scalar(Command)
            Command.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarFacturas(estado As Integer, desde As Date, hasta As Date, usu As String, torneo As String) As List(Of Factura)
        Try
            Dim consulta As String = ""

            If desde = DateTime.MinValue And hasta = DateTime.MinValue Then
                consulta = "Select Top 50 "
            Else
                consulta = "Select "
            End If
            consulta += " ID_factura, F.ID_Torneo,F.ID_Usuario,ID_Tipo_Pago,Monto_Total,Fecha,Estado,F.ID_Equipo from Factura as F inner join Usuario as U on U.id_usuario=f.ID_Usuario inner join Torneo as T on T.ID_Torneo=F.ID_Torneo inner join Equipo as E on E.ID_Equipo=F.ID_Equipo where Fecha between isnull(@desde,'19000101') and isnull(@hasta,'99990101') and  u.NombreUsuario like Concat('%',isnull(@Usuario,u.NombreUsuario),'%') and  T.Nombre like Concat('%',isnull(@Torneo,T.Nombre),'%') and Estado=isnull(@estado,estado) order by ID_Factura DESC  "

            Dim Command As SqlCommand = Acceso.MiComando(consulta)
            With Command.Parameters
                If Not desde = DateTime.MinValue And Not hasta = DateTime.MinValue Then
                    .Add(New SqlParameter("@Desde", desde))
                    .Add(New SqlParameter("@Hasta", hasta))
                Else
                    .Add(New SqlParameter("@Desde", DBNull.Value))
                    .Add(New SqlParameter("@Hasta", DBNull.Value))
                End If
                If estado >= 0 Then
                    .Add(New SqlParameter("@Estado", estado))
                Else
                    .Add(New SqlParameter("@Estado", DBNull.Value))
                End If
                If Not IsNothing(usu) Then
                    .Add(New SqlParameter("@Usuario", usu))
                Else
                    .Add(New SqlParameter("@Usuario", DBNull.Value))
                End If
                If Not IsNothing(torneo) Then
                    .Add(New SqlParameter("@Torneo", torneo))
                Else
                    .Add(New SqlParameter("@Torneo", DBNull.Value))
                End If
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Dim listaFact As New List(Of Entidades.Factura)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim fact As New Entidades.Factura
                    FormatearFactura(fact, dr)
                    listaFact.Add(fact)
                Next
                Return listaFact
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub AprobarFactura(fac As Factura)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("update Factura set Estado=1 where ID_Factura=@ID_Factura")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Factura", fac.ID_Factura))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GenerarDetalle(fact As Factura, detal As Detalle_Factura)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("insert into Detalle_Factura (ID_Factura,ID_Usuario,Monto) values (@ID_Factura,@ID_Usuario,@Monto)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Factura", fact.ID_Factura))
                .Add(New SqlParameter("@ID_usuario", detal.Usuario.ID_Usuario))
                .Add(New SqlParameter("@Monto", detal.Monto))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub FormatearFactura(fact As Factura, row As DataRow)
        Try
            fact.ID_Factura = row("ID_factura")
            fact.Fecha = row("Fecha")
            fact.Monto_Total = row("Monto_total")
            fact.Tipo_Pago = New Tipo_Pago With {.Tipo_Pago = row("ID_Tipo_Pago")}
            fact.Torneo = (New TorneoDAL).TraerTorneoID(row("ID_Torneo"))
            fact.Usuario = (New UsuarioDAL).BuscarUsuarioID(New UsuarioEntidad With {.ID_Usuario = row("id_usuario")})
            fact.Detalles = TraerDetalles(fact)
            fact.Equipo = (New EquipoDAL).TraerEquipoID(row("ID_Equipo"))
            fact.Estado = row("estado")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function TraerDetalles(fact As Factura) As List(Of Detalle_Factura)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Detalle_FActura,ID_Factura,ID_Usuario,Monto from Detalle_Factura where ID_Factura=@ID_Factura")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Factura", fact.ID_Factura))
            End With
            Dim dt As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            Dim Listdet As New List(Of Entidades.Detalle_Factura)
            For Each _dr As DataRow In dt.Rows
                Dim pre As New Entidades.Detalle_Factura((New UsuarioDAL).BuscarUsuarioID(New UsuarioEntidad With {.ID_Usuario = _dr("ID_Usuario")}), _dr("monto"))
                pre.ID_Detalle_Factura = _dr("ID_detalle_Factura")
                Listdet.Add(pre)
            Next
            Return Listdet
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
