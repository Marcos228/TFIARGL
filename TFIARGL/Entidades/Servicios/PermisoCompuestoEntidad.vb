﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()>
Public Class PermisoCompuestoEntidad
    Inherits PermisoBaseEntidad
    Implements ICloneable


    Private _hijos As New List(Of PermisoBaseEntidad)
    Public ReadOnly Property Hijos() As List(Of PermisoBaseEntidad)
        Get
            Return _hijos
        End Get
    End Property

    Public Overrides Function ValidarURL(paramURL As String) As Boolean

        Return _hijos.Any(Function(Permiso) Permiso.ValidarURL(paramURL))
    End Function


    Public Overrides Function agregarHijo(Perm As PermisoBaseEntidad) As Boolean
        If Not _hijos.Contains(Perm) Then
            Me._hijos.Add(Perm)
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function tieneHijos() As Boolean
        Return True
    End Function

    Public Overrides Function esValido(nombrePermiso As String) As Boolean
        Dim tieneUnValido As Boolean = False
        If nombrePermiso = Me.Nombre Then
            Return True
        End If
        For Each p In Me._hijos
            If p.Nombre = nombrePermiso Then
                Return True
            Else
                tieneUnValido = p.esValido(nombrePermiso)
            End If
            If tieneUnValido = True Then
                Exit For
            Else

            End If

        Next
        Return tieneUnValido
    End Function
    Public Function Clone() As Object Implements ICloneable.Clone
        Dim m As New MemoryStream()
        Dim f As New BinaryFormatter()
        f.Serialize(m, Me)
        m.Seek(0, SeekOrigin.Begin)
        Return f.Deserialize(m)
    End Function
End Class
