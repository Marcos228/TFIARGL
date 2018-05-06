Imports System.IO
Imports System.Web.HttpContext
Public Class AgregarPerfilJugador
    Inherits System.Web.UI.Page

    Private Sub AgregarPerfilJugador_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarJuegos()
    End Sub

    Private Sub CargarJuegos()
        Dim GestorGame As New Negocio.GameBLL
        Dim Juegos As List(Of Entidades.Game) = GestorGame.TraerJuegos(New Entidades.UsuarioEntidad With {.ID_Usuario = 1})
        For Each Game In Juegos
            Dim base64string As String = Convert.ToBase64String(Game.Imagen, 0, Game.Imagen.Length)
            Dim ImgBut As New ImageButton()

            ImgBut.ImageUrl = Convert.ToString("data:image/jpg;base64,") & base64string
            ImgBut.ID = "Logo" & Game.Nombre
            ImgBut.Height = 150
            ImgBut.CssClass = "img-responsive"
            ImgBut.ImageAlign = ImageAlign.Middle
            ImgBut.PostBackUrl = "/AgregarPerfilJugador2.aspx"
            Dim div As HtmlGenericControl = New HtmlGenericControl("div")
            If Juegos.IndexOf(Game) Mod 2 = 0 Then
                div.Attributes.Add("class", "col-md-5 col-md-offset-1 media")
            Else
                div.Attributes.Add("class", "col-md-5 media")
            End If
            div.Controls.Add(ImgBut)
            Panel.Controls.Add(div)
        Next
    End Sub

End Class