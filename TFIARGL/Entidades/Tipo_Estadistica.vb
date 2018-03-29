Public Class Tipo_Estadistica
    Private _tipo_rol As Tipo_Rol
    Public Property Tipo_rol() As Tipo_Rol
        Get
            Return _tipo_rol
        End Get
        Set(ByVal value As Tipo_Rol)
            _tipo_rol = value
        End Set
    End Property
    Private _destacado As Boolean
    Public Property Destacado() As Boolean
        Get
            Return _destacado
        End Get
        Set(ByVal value As Boolean)
            _destacado = value
        End Set
    End Property

    Private _valor_base As Integer
    Public Property Valor_Base() As Integer
        Get
            Return _valor_base
        End Get
        Set(ByVal value As Integer)
            _valor_base = value
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
