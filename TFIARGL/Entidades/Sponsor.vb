Public Class Sponsor
    Private _id_sponsor As Integer
    Public Property ID_Sponsor() As Integer
        Get
            Return _id_sponsor
        End Get
        Set(ByVal value As Integer)
            _id_sponsor = value
        End Set
    End Property
    Private _cuil As String
    Public Property CUIL() As String
        Get
            Return _cuil
        End Get
        Set(ByVal value As String)
            _cuil = value
        End Set
    End Property

    Private _correo As String
    Public Property Valor() As String
        Get
            Return _correo
        End Get
        Set(ByVal value As String)
            _correo = value
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


End Class
