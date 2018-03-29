Imports System.Data.SqlClient
Imports Entidades
Imports DAL
Public Class GestorPermisosDAL

    Public Sub Alta(ByVal perm As PermisoCompuestoEntidad)
        Try
            If perm.tieneHijos = True Then
                'Es un Perfil
                Dim Command As SqlCommand = Acceso.MiComando("insert into Permiso (Nombre,esAccion) OUTPUT INSERTED.ID_Rol values(@Nombre,@esAccion)")
                With Command.Parameters
                    .Add(New SqlParameter("@Nombre", perm.Nombre))
                    .Add(New SqlParameter("@esAccion", 0))
                End With
                Dim ID As Integer = Acceso.Scalar(Command)

                For Each MiPermiso As PermisoBaseEntidad In perm.Hijos
                    Command = Acceso.MiComando("insert into Permiso_Permiso values (@ID_Padre, @ID_Hijo)")
                    With Command.Parameters
                        .Add(New SqlParameter("@ID_Padre", ID))
                        .Add(New SqlParameter("@ID_Hijo", MiPermiso.ID))
                    End With
                    Acceso.Escritura(Command)
                Next
            Else
                'Es un Permiso
                'Dim Command As SqlCommand = Acceso.MiComando("insert into Permiso values(@ID_Permiso, @Nombre, @esAccion)")
                'With Command.Parameters
                '    .Add(New SqlParameter("@ID_Permiso", MiID))
                '    .Add(New SqlParameter("@Nombre", perm.Nombre))
                '    .Add(New SqlParameter("@esAccion", 1))
                'End With
                'Acceso.Escritura(Command)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Baja(ByVal ID As Integer) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select ID_Usuario from Usuario where ID_Perfil=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Padre", ID))
            End With
            Dim dt_usu As DataTable = Acceso.Lectura(Command)
            Command.Dispose()
            If dt_usu.Rows.Count = 0 Then
                Command = Acceso.MiComando("delete from Permiso_Permiso where ID_Rol=@ID_Padre")
                With Command.Parameters
                    .Add(New SqlParameter("@ID_Padre", ID))
                End With
                Acceso.Escritura(Command)
                Command.Dispose()
                Command = Acceso.MiComando("delete from Permiso_Permiso where ID_Permiso=@ID_Padre")
                With Command.Parameters
                    .Add(New SqlParameter("@ID_Padre", ID))
                End With
                Acceso.Escritura(Command)
                Command.Dispose()

                Command = Acceso.MiComando("delete from Permiso where ID_ROL=@IDPermiso")
                With Command.Parameters
                    .Add(New SqlParameter("@IDPermiso", ID))
                End With
                Acceso.Escritura(Command)
                Return True
            Else
                Return False
            End If




            'Dim Command As SqlCommand = Acceso.MiComando("Update Usuario set Perfil=@PerfilEliminado where Perfil=@ID_Padre")
            'With Command.Parameters
            '    .Add(New SqlParameter("@PerfilEliminado", 0))
            '    .Add(New SqlParameter("@ID_Padre", ID))
            'End With
            'Acceso.Escritura(Command)
            'Command.Dispose()

            'Dim VerificadorUsuario As UsuarioDAL = New UsuarioDAL
            'VerificadorUsuario.PerfilEliminadoActualizacion()


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub Modificar(ByVal perm As PermisoCompuestoEntidad)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("delete from permiso_permiso where ID_Rol=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Padre", perm.ID))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            For Each MiPermiso As Entidades.PermisoBaseEntidad In perm.Hijos
                Command = Acceso.MiComando("insert into permiso_permiso values (@ID_Padre, @ID_Hijo)")
                With Command.Parameters
                    .Add(New SqlParameter("@ID_Padre", perm.ID))
                    .Add(New SqlParameter("@ID_Hijo", MiPermiso.ID))
                End With
                If Not perm.ID = MiPermiso.ID Then
                    Acceso.Escritura(Command)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ListarFamilias(ByVal filtro As Boolean) As List(Of PermisoBaseEntidad)
        Try
            Dim _listaFamilias As New List(Of PermisoBaseEntidad)
            Dim Command As SqlCommand
            Command = Acceso.MiComando("Select * from Permiso where esAccion= @accion and ID_ROL > @PerfilEliminado order by esAccion asc, ID_ROL asc")

            If filtro = True Then
                Command.Parameters.Add(New SqlParameter("@accion", 0))
                Command.Parameters.Add(New SqlParameter("@PerfilEliminado", 0))
            Else
                Command.Parameters.Add(New SqlParameter("@accion", 1))
                Command.Parameters.Add(New SqlParameter("@PerfilEliminado", 0))
            End If
            Dim _dt As DataTable = Acceso.Lectura(Command)
            For Each _dr As DataRow In _dt.Rows
                Dim _per As PermisoBaseEntidad = ConvertirDataRowEnPermiso(_dr)
                _listaFamilias.Add(_per)
            Next
            Return _listaFamilias
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ConsultarporID(ByVal ID As Integer) As Entidades.PermisoCompuestoEntidad
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Permiso where ID_ROL=@ID_ROL")
            Command.Parameters.Add(New SqlParameter("@ID_ROL", ID))
            Dim _dt As DataTable = Acceso.Lectura(Command)
            If _dt.Rows.Count = 1 Then
                Return ConvertirDataRowEnPermiso(_dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ListarPermisos() As List(Of PermisoBaseEntidad)
        Try
            Dim _listaPermisos As New List(Of PermisoBaseEntidad)
            Dim Command As SqlCommand
            Command = Acceso.MiComando("Select * from Permiso where ID_Rol <> 0 order by esAccion asc, ID_Rol asc")
            Dim _dt As DataTable = Acceso.Lectura(Command)
            For Each _dr As DataRow In _dt.Rows
                Dim _permiso As PermisoBaseEntidad = ConvertirDataRowEnPermiso(_dr)
                _listaPermisos.Add(_permiso)
            Next
            Return _listaPermisos
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarNombre(ByVal Nombre As String) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("select Nombre from Permiso where Nombre=@Nombre")
            Command.Parameters.Add(New SqlParameter("@Nombre", Nombre))
            Dim DataTabla = Acceso.Lectura(Command)
            If DataTabla.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ConvertirDataRowEnPermiso(_dr As DataRow) As PermisoBaseEntidad
        Try
            Dim _permiso As PermisoBaseEntidad
            If Not _dr.Item("esAccion") Is DBNull.Value AndAlso Convert.ToBoolean(_dr.Item("esAccion")) Then
                _permiso = New PermisoEntidad
            Else
                _permiso = New PermisoCompuestoEntidad
            End If
            _permiso.ID = CInt(_dr.Item("ID_Rol"))
            _permiso.Nombre = Convert.ToString(_dr.Item("Nombre"))
            _permiso.URL = Convert.ToString(_dr.Item("URL"))
            If _permiso.tieneHijos Then
                Dim ListaHijos As List(Of PermisoBaseEntidad) = Me.ListarHijos(_permiso.ID)
                For Each hijo As PermisoBaseEntidad In ListaHijos
                    _permiso.agregarHijo(hijo)
                Next
            End If
            Return _permiso
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function ListarHijos(ByVal _id As Integer) As List(Of PermisoBaseEntidad)
        Try
            Dim lista As List(Of PermisoBaseEntidad) = New List(Of PermisoBaseEntidad)
            Dim Command As SqlClient.SqlCommand = Acceso.MiComando("SELECT P.ID_ROL,Nombre,esAccion,URL FROM Permiso as P LEFT JOIN Permiso_Permiso as PP ON (PP.ID_Permiso=P.ID_ROL) WHERE PP.ID_ROL = @IDPadre  ORDER BY P.ID_Rol ASC")
            Command.Parameters.Add(New SqlParameter("@IDPadre", _id))
            Dim dt As DataTable = Acceso.Lectura(Command)
            For Each row As DataRow In dt.Rows
                Dim MiPermiso As PermisoBaseEntidad = Me.ConvertirDataRowEnPermiso(row)
                lista.Add(MiPermiso)
            Next
            Return lista
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
