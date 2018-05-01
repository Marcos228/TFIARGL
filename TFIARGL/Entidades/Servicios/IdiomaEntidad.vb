Imports System.Globalization
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()>
Public Class IdiomaEntidad
    Implements ICloneable

    Private _cultura As CultureInfo
    Public Property Cultura() As CultureInfo
        Get
            Return _cultura
        End Get
        Set(ByVal value As CultureInfo)
            _cultura = value
        End Set
    End Property
    Private _id_idoma As Integer
    Public Property ID_Idioma() As Integer
        Get
            Return _id_idoma
        End Get
        Set(ByVal value As Integer)
            _id_idoma = value
        End Set
    End Property
    Private _editable As Boolean
    Public Property Editable() As Boolean
        Get
            Return _editable
        End Get
        Set(ByVal value As Boolean)
            _editable = value
        End Set
    End Property
    Private _palabras As List(Of Palabras)
    Public Property Palabras() As List(Of Palabras)
        Get
            Return _palabras
        End Get
        Set(ByVal value As List(Of Palabras))
            _palabras = value
        End Set
    End Property
    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim m As New MemoryStream()
        Dim f As New BinaryFormatter()
        f.Serialize(m, Me)
        m.Seek(0, SeekOrigin.Begin)
        Return f.Deserialize(m)
    End Function
End Class
