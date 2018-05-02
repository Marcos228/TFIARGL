
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()>
Public Class Palabras
    Implements ICloneable
    Private _traduccion As String
    Public Property Traduccion() As String
        Get
            Return _traduccion
        End Get
        Set(ByVal value As String)
            _traduccion = value
        End Set
    End Property
    Private _id_control As Integer
    Public Property ID_Control() As Integer
        Get
            Return _id_control
        End Get
        Set(ByVal value As Integer)
            _id_control = value
        End Set
    End Property
    Private _codigo As String
    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Shared Function FindValue(ByVal bk As Entidades.Palabras) As Integer
        If bk.ID_Control = FindValue Then
            Return bk.ID_Control
        Else
            Return 0
        End If
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If TryCast(obj, Palabras).ID_Control = Me.ID_Control Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim m As New MemoryStream()
        Dim f As New BinaryFormatter()
        f.Serialize(m, Me)
        m.Seek(0, SeekOrigin.Begin)
        Return f.Deserialize(m)
    End Function
End Class
