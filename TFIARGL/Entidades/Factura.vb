Public Class Factura
    Private _id_factura As Integer
    Public Property ID_Factura() As Integer
        Get
            Return _id_factura
        End Get
        Set(ByVal value As Integer)
            _id_factura = value
        End Set
    End Property
    Private _fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property

    Private _monto_total As Double
    Public Property Monto_Total() As Double
        Get
            Return _monto_total
        End Get
        Set(ByVal value As Double)
            _monto_total = value
        End Set
    End Property

    Private _detalles As New List(Of Detalle_Factura)
    Public Property Detalles() As List(Of Detalle_Factura)
        Get
            Return _detalles
        End Get
        Set(ByVal value As List(Of Detalle_Factura))
            _detalles = value
        End Set
    End Property
    Private _tipo_pago As Tipo_Pago
    Public Property Tipo_Pago() As Tipo_Pago
        Get
            Return _tipo_pago
        End Get
        Set(ByVal value As Tipo_Pago)
            _tipo_pago = value
        End Set
    End Property

    Private _torneo As Torneo
    Public Property Torneo() As Torneo
        Get
            Return _torneo
        End Get
        Set(ByVal value As Torneo)
            _torneo = value
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

    Private _estado As Estado
    Public Property Estado() As Estado
        Get
            Return _estado
        End Get
        Set(ByVal value As Estado)
            _estado = value
        End Set
    End Property

    Private _equipo As Equipo
    Public Property Equipo() As Equipo
        Get
            Return _equipo
        End Get
        Set(ByVal value As Equipo)
            _equipo = value
        End Set
    End Property

    Private Function Total() As Double
        Dim retorno As Double
        For Each deta As Detalle_Factura In Me.Detalles
            retorno += deta.Monto
        Next
        Return retorno
    End Function

    Sub New(ByRef tor As torneo, ByRef usu As UsuarioEntidad, fec As DateTime, det As List(Of Detalle_Factura))
        Me.Detalles = det
        Me.Torneo = tor
        Me.Fecha = fec
        Me.Usuario = usu
        Me.Monto_Total = Total()
        Me.Estado = Estado.Pendiente
    End Sub
    Sub New()

    End Sub
End Class
