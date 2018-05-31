Imports Entidades

Public Class Premio
    Implements IComparable(Of Premio)
    Private _id_premio As Integer
    Public Property ID_Premio() As Integer
        Get
            Return _id_premio
        End Get
        Set(ByVal value As Integer)
            _id_premio = value
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

    Private _valor As Double
    Public Property Valor() As Double
        Get
            Return _valor
        End Get
        Set(ByVal value As Double)
            _valor = value
        End Set
    End Property

    Private _posicion As Posicion
    Public Property Posicion() As Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Posicion)
            _posicion = value
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

    Public Function CompareTo(other As Premio) As Integer Implements IComparable(Of Premio).CompareTo

        Return Me.Posicion.CompareTo(other.Posicion)
    End Function
End Class
