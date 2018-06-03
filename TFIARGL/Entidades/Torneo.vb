Public Class Torneo
    Private _id_torneo As Integer
    Public Property ID_Torneo() As Integer
        Get
            Return _id_torneo
        End Get
        Set(ByVal value As Integer)
            _id_torneo = value
        End Set
    End Property
    Private _fecha_inicio As DateTime
    Public Property Fecha_Inicio() As DateTime
        Get
            Return _fecha_inicio
        End Get
        Set(ByVal value As DateTime)
            _fecha_inicio = value
        End Set
    End Property

    Private _fecha_fin As DateTime
    Public Property Fecha_Fin() As DateTime
        Get
            Return _fecha_fin
        End Get
        Set(ByVal value As DateTime)
            _fecha_fin = value
        End Set
    End Property

    Private _fecha_inicio_inscripcion As DateTime
    Public Property Fecha_Inicio_Inscripcion() As DateTime
        Get
            Return _fecha_inicio_inscripcion
        End Get
        Set(ByVal value As DateTime)
            _fecha_inicio_inscripcion = value
        End Set
    End Property

    Private _fecha_fin_inscripcion As DateTime
    Public Property Fecha_Fin_Inscripcion() As DateTime
        Get
            Return _fecha_fin_inscripcion
        End Get
        Set(ByVal value As DateTime)
            _fecha_fin_inscripcion = value
        End Set
    End Property

    Private _precio_inscripcion As Double
    Public Property Precio_Inscripcion() As Double
        Get
            Return _precio_inscripcion
        End Get
        Set(ByVal value As Double)
            _precio_inscripcion = value
        End Set
    End Property

    Private _game As Game
    Public Property Game() As Game
        Get
            Return _game
        End Get
        Set(ByVal value As Game)
            _game = value
        End Set
    End Property

    Private _equipos As New List(Of Equipo)
    Public Property Equipos() As List(Of Equipo)
        Get
            Return _equipos
        End Get
        Set(ByVal value As List(Of Equipo))
            _equipos = value
        End Set
    End Property


    Private _partidas As New List(Of Partida)
    Public Property Partidas() As List(Of Partida)
        Get
            Return _partidas
        End Get
        Set(ByVal value As List(Of Partida))
            _partidas = value
        End Set
    End Property



    Private _sponsors As New List(Of Sponsor)
    Public Property Sponsors() As List(Of Sponsor)
        Get
            Return _sponsors
        End Get
        Set(ByVal value As List(Of Sponsor))
            _sponsors = value
        End Set
    End Property

    Private _premios As New List(Of Premio)
    Public Property Premios() As List(Of Premio)
        Get
            Return _premios
        End Get
        Set(ByVal value As List(Of Premio))
            _premios = value
        End Set
    End Property

    Private _lugar_final As String
    Public Property Lugar_Final() As String
        Get
            Return _lugar_final
        End Get
        Set(ByVal value As String)
            _lugar_final = value
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

    Private _cantidadparticipantes As Integer
    Public Property CantidadParticipantes() As Integer
        Get
            Return _cantidadparticipantes
        End Get
        Set(ByVal value As Integer)
            _cantidadparticipantes = value
        End Set
    End Property

End Class
