
Public Class Palabras
    Private _id_control As Integer
    Public Property ID_Control() As Integer
        Get
            Return _id_control
        End Get
        Set(ByVal value As Integer)
            _id_control = value
        End Set
    End Property
    Private _traduccion As String
    Public Property Traduccion() As String
        Get
            Return _traduccion
        End Get
        Set(ByVal value As String)
            _traduccion = value
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

End Class
