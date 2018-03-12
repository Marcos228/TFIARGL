Imports Entidades
Imports System.Data.SqlClient
Imports System.Globalization
Public Class IdiomaDAL

    Public Function GuardarIdioma(ByRef Idioma As IdiomaEntidad) As Boolean
        Try
            Idioma.ID_Idioma = Acceso.TraerID("ID_Idioma", "Idioma")
            Dim Command As SqlCommand = Acceso.MiComando("insert into Idioma values (@ID_Idioma, @Nombre, @Editable, @Cultura , @BL)")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
                .Add(New SqlParameter("@Nombre", Idioma.Nombre))
                .Add(New SqlParameter("@Editable", Idioma.Editable))
                .Add(New SqlParameter("@Cultura", Idioma.Cultura.Name))
                .Add(New SqlParameter("@BL", False))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Dim Micomando As SqlCommand
            Dim ComandoStr As String = "insert into Traduccion values (@ID_Control, @ID_Idioma, @Palabra)"
            For Each MiPalabra As Palabras In Idioma.Palabras
                Micomando = Acceso.MiComando(ComandoStr)
                With Micomando.Parameters
                    .Add(New SqlParameter("@ID_Control", MiPalabra.ID_Control))
                    .Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
                    .Add(New SqlParameter("@Palabra", MiPalabra.Traduccion))

                End With
                Acceso.Escritura(Micomando)
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ModificarIdioma(ByRef Idioma As IdiomaEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Idioma set Nombre=@Nombre, Cultura=@Cultura where ID_Idioma = @ID_Idioma")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
                .Add(New SqlParameter("@Nombre", Idioma.Nombre))
                .Add(New SqlParameter("@Cultura", Idioma.Cultura.Name))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Dim Micomando As SqlCommand

            Dim CommandoEliminador As SqlCommand = Acceso.MiComando("Delete From Traduccion where ID_Idioma = @ID_Idioma")
            CommandoEliminador.Parameters.Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
            Acceso.Escritura(CommandoEliminador)

            Dim ComandoStr As String = "insert into Traduccion values (@ID_Control, @ID_Idioma, @Palabra)"
            For Each MiPalabra As Palabras In Idioma.Palabras
                Micomando = Acceso.MiComando(ComandoStr)
                With Micomando.Parameters
                    .Add(New SqlParameter("@ID_Control", MiPalabra.ID_Control))
                    .Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
                    .Add(New SqlParameter("@Palabra", MiPalabra.Traduccion))
                End With
                Acceso.Escritura(Micomando)
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EliminarIdioma(ByRef Idioma As IdiomaEntidad) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Idioma set BL=@BL where ID_Idioma = @ID_Idioma;Update Usuario set idioma = 1 where idioma = @ID_Idioma")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Idioma", Idioma.ID_Idioma))
                .Add(New SqlParameter("@BL", True))
            End With
            Acceso.Escritura(Command)
            Command.Dispose()
            Dim gestorusu As UsuarioDAL = New UsuarioDAL
            gestorusu.IdiomaEliminadoActualizacion()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarIdioma(ByRef Usuario As UsuarioEntidad, ByRef ID_Idioma As Integer) As IdiomaEntidad
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Update Usuario SET Idioma=@ID_Idioma, DVH=@DVH where ID_Usuario=@ID_Usuario")
            With Command.Parameters
                .Add(New SqlParameter("@ID_Idioma", ID_Idioma))
                .Add(New SqlParameter("@ID_Usuario", Usuario.ID_Usuario))
                .Add(New SqlParameter("@DVH", DigitoVerificadorDAL.CalcularDVH(Usuario.ID_Usuario & Usuario.Nombre & Usuario.Password & Usuario.Bloqueo & Usuario.Intento & ID_Idioma & Usuario.Perfil.ID & False)))
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
            Return Me.ConsultarPorID(ID_Idioma)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarNombre(ByRef Idiomas As String) As Boolean
        Try
            Dim _listaidiomas As List(Of IdiomaEntidad) = New List(Of IdiomaEntidad)
            Dim Command As SqlCommand
            Command = Acceso.MiComando("Select Nombre from Idioma where bl= 0")
            Dim _dt As DataTable = Acceso.Lectura(Command)
            For Each _dr As DataRow In _dt.Rows
                Dim _idiom As IdiomaEntidad = New IdiomaEntidad
                _idiom.Nombre = _dr("Nombre")
                If Idiomas = _idiom.Nombre Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarIdiomas() As List(Of IdiomaEntidad)
        Try
            Dim _listaidiomas As List(Of IdiomaEntidad) = New List(Of IdiomaEntidad)
            Dim Command As SqlCommand
            Command = Acceso.MiComando("Select * from Idioma where bl= 0")
            Dim _dt As DataTable = Acceso.Lectura(Command)
            For Each _dr As DataRow In _dt.Rows
                Dim _idiom As IdiomaEntidad = New IdiomaEntidad
                _idiom.ID_Idioma = _dr("ID_Idioma")
                _idiom.Nombre = _dr("Nombre")
                _idiom.Editable = _dr("Editable")
                _idiom.Cultura = CultureInfo.GetCultureInfo(_dr("Cultura"))
                _listaidiomas.Add(_idiom)
            Next
            Return _listaidiomas
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ConsultarIdiomasEditables() As List(Of IdiomaEntidad)
        Try
            Dim _listaidiomas As New List(Of IdiomaEntidad)
            Dim Command As SqlCommand
            Command = Acceso.MiComando("Select * from Idioma where Editable = 1 and bl= 0")
            Dim _dt As DataTable = Acceso.Lectura(Command)
            For Each _dr As DataRow In _dt.Rows
                Dim _idiom As IdiomaEntidad = New IdiomaEntidad
                _idiom.ID_Idioma = _dr("ID_Idioma")
                _idiom.Nombre = _dr("Nombre")
                _idiom.Editable = _dr("Editable")
                _idiom.Cultura = CultureInfo.GetCultureInfo(_dr("Cultura"))
                _listaidiomas.Add(_idiom)
            Next
            Return _listaidiomas
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarPorID(ByRef ID_Idioma As Integer) As IdiomaEntidad
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Idioma where ID_Idioma=@IDIdioma and bl= 0")
            Command.Parameters.Add(New SqlParameter("@IDIdioma", ID_Idioma))
            Dim _dt As DataTable = Acceso.Lectura(Command)
            Dim Miidioma As IdiomaEntidad = New IdiomaEntidad
            For Each row As DataRow In _dt.Rows
                Miidioma.ID_Idioma = row("ID_Idioma")
                Miidioma.Nombre = row("Nombre")
                Miidioma.Editable = row("Editable")
                Miidioma.Cultura = CultureInfo.GetCultureInfo(row("Cultura"))
                Miidioma.Palabras = Me.TraerPalabras(Miidioma.ID_Idioma)
            Next
            Return Miidioma
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function TraerPalabras(ByRef ID_Idioma As Integer) As List(Of Palabras)
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Traduccion INNER JOIN Control on Traduccion.ID_Control = Control.ID_Control where ID_Idioma =@IDIdioma")
            Command.Parameters.Add(New SqlParameter("@IDIdioma", ID_Idioma))
            Dim _dt As DataTable = Acceso.Lectura(Command)
            Dim listaPalabras As List(Of Palabras) = New List(Of Palabras)
            For Each row As DataRow In _dt.Rows
                Dim Palabra As Palabras = New Palabras
                Palabra.ID_Control = row("ID_Control")
                Palabra.Codigo = row("Nombre")
                Palabra.Traduccion = row("Palabra")
                listaPalabras.Add(Palabra)
            Next
            Return listaPalabras
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
