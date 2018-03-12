Imports System.IO
Imports Newtonsoft.Json
Imports Entidades
Public Class SerializadorJSON(Of T)
    Public Sub Serializar(que As T)
        Dim fs As FileStream
        Dim bitacoras As New List(Of BitacoraEntidad)
        If que.GetType.Equals(bitacoras.GetType) Then
            fs = New FileStream("Bitacoras.JSON", FileMode.Append)
        Else
            fs = New FileStream("Objetos.JSON", FileMode.Append)
        End If
        Dim writer As TextWriter
        writer = New StreamWriter(fs)
        Dim serializer As New JsonSerializer
        Using writer
            serializer.Serialize(writer, que)
        End Using
        writer.Close()
        fs.Close()
    End Sub

    Public Function Deserializar(str As Stream, obj As T) As Object
        Dim serializer As New JsonSerializer()
        Dim tr As TextReader = New StreamReader(str)
        Dim o As T = serializer.Deserialize(tr, GetType(T))
        tr.Close()
        Return o
    End Function

End Class
