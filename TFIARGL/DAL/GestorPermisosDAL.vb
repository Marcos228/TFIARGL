Imports System.Data.SqlClient
Imports Entidades
Imports DAL
Public Class GestorPermisosDAL

    Public Sub Alta(ByVal perm As PermisoBaseEntidad)
        Try
            Dim MiID As Integer = Acceso.TraerID("ID_Permiso", "Permiso")

            If perm.tieneHijos = True Then
                'Es un Perfil
                Dim Command As SqlCommand = Acceso.MiComando("insert into Permiso values(@ID_Permiso, @Nombre, @esAccion)")
                With Command.Parameters
                    .Add(New SqlParameter("@ID_Permiso", MiID))
                    .Add(New SqlParameter("@Nombre", perm.Nombre))
                    .Add(New SqlParameter("@esAccion", 0))
                End With
                Acceso.Escritura(Command)

                For Each MiPermiso As PermisoBaseEntidad In perm.obtenerHijos
                    Command = Acceso.MiComando("insert into Permiso_Permiso values (@ID_Padre, @ID_Hijo)")
                    With Command.Parameters
                        .Add(New SqlParameter("@ID_Padre", MiID))
                        .Add(New SqlParameter("@ID_Hijo", MiPermiso.ID))
                    End With
                    Acceso.Escritura(Command)
                Next
            Else
                'Es un Permiso
                Dim Command As SqlCommand = Acceso.MiComando("insert into Permiso values(@ID_Permiso, @Nombre, @esAccion)")
                With Command.Parameters
                    .Add(New SqlParameter("@ID_Permiso", MiID))
                    .Add(New SqlParameter("@Nombre", perm.Nombre))
                    .Add(New SqlParameter("@esAccion", 1))
                End With
                Acceso.Escritura(Command)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Baja(ByVal ID As Integer)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Usuario set Perfil=@PerfilEliminado where Perfil=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@PerfilEliminado", 0))
                .Add(New SqlParameter("@ID_Padre", ID))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()

            Dim VerificadorUsuario As UsuarioDAL = New UsuarioDAL
            VerificadorUsuario.PerfilEliminadoActualizacion()

            Command = Acceso.MiComando("delete from Permiso_Permiso where ID_Padre=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Padre", ID))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Command = Acceso.MiComando("delete from Permiso_Permiso where ID_Hijo=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Padre", ID))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()

            Command = Acceso.MiComando("delete from Permiso where ID_Permiso=@IDPermiso")
            With Command.Parameters
                .Add(New SqlParameter("@IDPermiso", ID))
            End With
            Acceso.Escritura(Command)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Modificar(ByVal perm As Entidades.PermisoBaseEntidad)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("delete from permiso_permiso where ID_Padre=@ID_Padre")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Padre", perm.ID))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            For Each MiPermiso As Entidades.PermisoBaseEntidad In perm.obtenerHijos
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
            Command = Acceso.MiComando("Select * from Permiso where esAccion= @accion and ID_Permiso <> @PerfilEliminado order by esAccion asc, ID_Permiso asc")

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

    Public Function ConsultarporID(ByVal ID As Integer) As GrupoPermisoEntidad
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Permiso where ID_Permiso=@IDPermiso")
            Command.Parameters.Add(New SqlParameter("@IDPermiso", ID))
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
            Command = Acceso.MiComando("Select * from Permiso where ID_Permiso <> 0 order by esAccion asc, ID_Permiso asc")
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
                _permiso = New GrupoPermisoEntidad
            End If
            _permiso.ID = CInt(_dr.Item("ID_Permiso"))
            _permiso.Nombre = Convert.ToString(_dr.Item("Nombre"))
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
            Dim Command As SqlClient.SqlCommand = Acceso.MiComando("SELECT P.ID_Permiso,Nombre,esAccion FROM Permiso as P LEFT JOIN Permiso_Permiso as PP ON (PP.ID_Hijo=P.ID_Permiso) WHERE PP.ID_Padre = @IDPadre  ORDER BY P.ID_Permiso ASC")
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
