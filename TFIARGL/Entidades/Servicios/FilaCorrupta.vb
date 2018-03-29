Public Class FilaCorrupta
    Sub New(id As String, tabla As String)
        Me.ID = id
        Me.NombreTabla = tabla
    End Sub
    Private _id As String
    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _nombretabla As String
    Public Property NombreTabla() As String
        Get
            Return _nombretabla
        End Get
        Set(ByVal value As String)
            _nombretabla = value
        End Set
    End Property
End Class
