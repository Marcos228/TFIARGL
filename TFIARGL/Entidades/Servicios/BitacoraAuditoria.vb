Public Class BitacoraAuditoria
    Inherits Bitacora
    Private _valor_anterior As String
    Public Property Valor_Anterior() As String
        Get
            Return _valor_anterior
        End Get
        Set(ByVal value As String)
            _valor_anterior = value
        End Set
    End Property

    Private _valor_posterior As String
    Public Property Valor_Posterior() As String
        Get
            Return _valor_posterior
        End Get
        Set(ByVal value As String)
            _valor_posterior = value
        End Set
    End Property

    Sub New(ByRef usu As UsuarioEntidad, ByVal detalle As String, ByVal Tipo_bit As Tipo_Bitacora, ByVal fec As DateTime, ByVal brws As String, ByVal IP As String, ByVal ValorAnterior As String, ByVal ValorPosterior As String)
        Me.Usuario = usu
        Me.Detalle = detalle
        Me.Tipo_Bitacora = Tipo_bit
        Me.Fecha = fec
        Me.Browser = brws
        Me.IP_Usuario = IP
        Me.Valor_Anterior = ValorAnterior
        Me.Valor_Posterior = ValorPosterior
    End Sub

    Sub New()

    End Sub

End Class
