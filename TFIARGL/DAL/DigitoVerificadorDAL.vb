Imports DAL
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Threading
Imports System.Globalization
Public Class DigitoVerificadorDAL
    Public Shared Function CalcularDVH(ByRef Parametros As List(Of String)) As String
        Try
            Dim fila As String = ""
            For Each Param In Parametros
                fila += Param
            Next
            If fila = "" Then
                Return Nothing
            End If
            Dim UE As New UnicodeEncoding
            Dim bHash As Byte()
            Dim bCadena() As Byte = UE.GetBytes(fila)
            Dim s1Service As New SHA1CryptoServiceProvider
            bHash = s1Service.ComputeHash(bCadena)
            Dim Resumen As String
            Resumen = Convert.ToBase64String(bHash)
            Return Resumen
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Shared Function CalcularDVV(ByRef Parametros As List(Of String), ByRef NombreTabla As String) As Boolean
        Try
            Dim commandVerificador As SqlCommand
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Digito_Verificador_Vertical where Nombre_Tabla = @Nombre_Tabla")
            Command.Parameters.Add(New SqlParameter("@Nombre_Tabla", NombreTabla))
            Dim DataTabla = Acceso.Lectura(Command)
            Dim contador As Integer = 0
            For Each row As DataRow In DataTabla.Rows
                contador = contador + 1
            Next
            If contador = 0 Then
                commandVerificador = Acceso.MiComando("Insert into Digito_Verificador_Vertical values (@Nombre_Tabla, @Digito)")
            Else
                commandVerificador = Acceso.MiComando("Update Digito_Verificador_Vertical set Digito=@Digito where Nombre_Tabla=@Nombre_Tabla")
            End If
            Dim Resumen As String
            Resumen = CalcularDVH(Parametros)
            With commandVerificador.Parameters
                .Add(New SqlParameter("@Nombre_Tabla", NombreTabla))
                .Add(New SqlParameter("@Digito", Resumen))
            End With
            Acceso.Escritura(commandVerificador)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function VerificarIntegridad() As DataTable
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from Digito_Verificador_Vertical")
            Dim DataTabla = Acceso.Lectura(Command)
            Return DataTabla
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetCorruptRows(nombreTabla As String) As DataTable
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from " + nombreTabla)
            Dim DataTabla = Acceso.Lectura(Command)
            Dim DigitosHorizontales As String = ""

            Dim problematicRows As New DataTable
            problematicRows = DataTabla.Clone



            For Each row As DataRow In DataTabla.Rows
                Dim Parametros As New List(Of String)
                Dim c As Integer = 0
                For Each column In row.ItemArray
                    If (Not row.ItemArray.Count - 1 = c) Then Parametros.Add(column)
                    c += 1
                Next
                If Not DigitoVerificadorDAL.CalcularDVH(Parametros) = row.ItemArray.Last Then problematicRows.ImportRow(row)
            Next

            Return problematicRows
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function VerifyHorizontally(nombreTabla As String) As List(Of Entidades.FilaCorrupta)
        Dim FilasCorruptas As New List(Of Entidades.FilaCorrupta)
        Dim Problematicrows As DataTable = GetCorruptRows(nombreTabla)
        For Each fila As DataRow In Problematicrows.Rows
            FilasCorruptas.Add(New Entidades.FilaCorrupta(fila.Item(0), nombreTabla))
        Next
        Return FilasCorruptas
    End Function

    Public Shared Sub RepareIntegrity(nombreTabla As String)
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from " + nombreTabla)
            Dim DataTabla = Acceso.Lectura(Command)

            Dim DigitosHorizontales As New List(Of String)
            For Each row As DataRow In DataTabla.Rows
                Dim Parametros As New List(Of String)
                Dim c As Integer = 0
                For Each column In row.ItemArray
                    If (Not row.ItemArray.Count - 1 = c) Then Parametros.Add(column)
                    c += 1
                Next
                If Not DigitoVerificadorDAL.CalcularDVH(Parametros) = row.ItemArray.Last Then
                    Dim commandCorrector As SqlCommand = Acceso.MiComando("Update " + nombreTabla + " set DVH=@Digito where " + DataTabla.Columns(0).ColumnName + "=@ID_valor")
                    Dim Digito As String = DigitoVerificadorDAL.CalcularDVH(Parametros)
                    With commandCorrector.Parameters
                        .Add(New SqlParameter("@Digito", Digito))
                        .Add(New SqlParameter("@ID_valor", row.Item(0)))
                    End With
                    Acceso.Escritura(commandCorrector)
                    DigitosHorizontales.Add(Digito)
                Else
                    DigitosHorizontales.Add(row.ItemArray.Last)
                End If
            Next
            Dim Command2 As SqlCommand = Acceso.MiComando("Update Digito_Verificador_Vertical set Digito=@digito where Digito_Verificador_Vertical.Nombre_Tabla = '" + nombreTabla + "'")
            With Command2.Parameters
                .Add(New SqlParameter("@Digito", DigitoVerificadorDAL.CalcularDVH(DigitosHorizontales)))
            End With
            Acceso.Escritura(Command2)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function VerifyVertically(nombreTabla As String) As Boolean
        Try
            Dim Command As SqlCommand = Acceso.MiComando("Select * from " + nombreTabla)
            Dim DataTabla = Acceso.Lectura(Command)
            Dim DigitosHorizontales As New List(Of String)

            For Each row As DataRow In DataTabla.Rows
                DigitosHorizontales.Add(row.ItemArray.Last)
            Next

            Dim actualDVV = GetDigitoVerticalFor(nombreTabla)

            Return actualDVV = CalcularDVH(DigitosHorizontales)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetDigitoVerticalFor(nombreTabla As String) As String
        Dim Command As SqlCommand = Acceso.MiComando("Select Digito from Digito_Verificador_Vertical where Digito_Verificador_Vertical.Nombre_Tabla = '" + nombreTabla + "'")
        Dim DataTabla = Acceso.Lectura(Command)
        Dim dvv As String
        For Each row As DataRow In DataTabla.Rows
            dvv = row.Item("Digito")
        Next
        Return dvv
    End Function

    'Public Shared Function GetAllHorizontallyCorruptRecords(tablasARevisar As List(Of String)) As List(Of DataTable)

    '    Dim allCorruptTablas As New List(Of DataTable)
    '    For Each tabla In tablasARevisar
    '        allCorruptTablas.Add(GetCorruptRows(tabla))
    '    Next

    '    Return allCorruptTablas
    'End Function

    'Public Shared Function VerifyAllIntegrity() As Boolean
    '    Dim tablasARevisar As New List(Of String)
    '    tablasARevisar.Add("Cliente")
    '    tablasARevisar.Add("Bitacora")

    '    Dim isHorizontallyIntegral As Boolean = True
    '    Dim isVerticallyIntegral As Boolean = True
    '    For Each t In tablasARevisar
    '        isHorizontallyIntegral = VerifyHorizontally(t) And isHorizontallyIntegral
    '        isVerticallyIntegral = VerifyVertically(t) And isVerticallyIntegral
    '    Next

    '    Return isHorizontallyIntegral And isVerticallyIntegral
    'End Function

End Class
