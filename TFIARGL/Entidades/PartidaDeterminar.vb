Public Class PartidaDeterminar
    Inherits Partida

    Private _Partida1 As Partida
    Public Property Partida1() As Partida
        Get
            Return _Partida1
        End Get
        Set(ByVal value As Partida)
            _Partida1 = value
        End Set
    End Property

    Private _Partida2 As Partida
    Public Property Partida2() As Partida
        Get
            Return _Partida2
        End Get
        Set(ByVal value As Partida)
            _Partida2 = value
        End Set
    End Property

End Class
