Public Class Tipo_Pago
    Private id_tipo_pago As Integer
    Public Property Tipo_Pago() As Integer
        Get
            Return id_tipo_pago
        End Get
        Set(ByVal value As Integer)
            id_tipo_pago = value
        End Set
    End Property

    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property


End Class
