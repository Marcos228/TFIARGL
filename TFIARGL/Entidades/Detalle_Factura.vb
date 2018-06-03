Public Class Detalle_Factura
    Private _id_detalle_factura As Integer
    Public Property ID_Detalle_Factura() As Integer
        Get
            Return _id_detalle_factura
        End Get
        Set(ByVal value As Integer)
            _id_detalle_factura = value
        End Set
    End Property
    Private _usuario As UsuarioEntidad
    Public Property Usuario() As UsuarioEntidad
        Get
            Return _usuario
        End Get
        Set(ByVal value As UsuarioEntidad)
            _usuario = value
        End Set
    End Property

    Private _monto As Double
    Public Property Monto() As Double
        Get
            Return _monto
        End Get
        Set(ByVal value As Double)
            _monto = value
        End Set
    End Property

    Sub New(ByRef usuent As UsuarioEntidad, mont As Double)
        Me.Usuario = usuent
        Me.Monto = mont
    End Sub
End Class
