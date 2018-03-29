Public Class BackupRestoreEntidad
    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Private _directorio As String
    Public Property Directorio() As String
        Get
            Return _directorio
        End Get
        Set(ByVal value As String)
            _directorio = value
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

    Sub New(ByVal Direccion As String)
        Me.Directorio = Direccion
    End Sub

    Sub New(ByVal Nombre As String, ByRef Usuario As UsuarioEntidad, ByVal Direccion As String)
        Me.Directorio = Direccion
        Me.Nombre = Nombre
        Me.Usuario = Usuario
    End Sub
End Class
