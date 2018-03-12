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


End Class
