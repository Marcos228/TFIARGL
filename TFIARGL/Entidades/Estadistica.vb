﻿Public Class Estadistica
    Private _jugador As Jugador
    Public Property Jugador() As Jugador
        Get
            Return _jugador
        End Get
        Set(ByVal value As Jugador)
            _jugador = value
        End Set
    End Property
    Private _tipo_estadistica As Tipo_Estadistica
    Public Property tipo_Estadistica() As Tipo_Estadistica
        Get
            Return _tipo_estadistica
        End Get
        Set(ByVal value As Tipo_Estadistica)
            _tipo_estadistica = value
        End Set
    End Property

    Private _valor_estadistica As Integer
    Public Property Valor_Estadistica() As Integer
        Get
            Return _valor_estadistica
        End Get
        Set(ByVal value As Integer)
            _valor_estadistica = value
        End Set
    End Property

    Sub New(ByRef juga As Jugador, ByRef val As Double)
        Me.Jugador = juga
        Me.Valor_Estadistica = val


    End Sub

End Class