Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Public Class ConsultarBitacoraAuditoria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Current.Session("FilasCorruptas") = Negocio.DigitoVerificadorBLL.VerifyAllIntegrity()
        If (Current.Session("FilasCorruptas").Count > 0) Then
            Current.Session("cliente") = DBNull.Value
            Response.Redirect("/BaseCorrupta.aspx")
        End If
        Me.SqlDataSource1.SelectCommand = "SELECT B.*,C.Nombre_Usuario FROM [Bitacora] As B INNER JOIN Cliente as C ON B.id_usuario = C.id_usuario"
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Me.SqlDataSource1.SelectCommand = "SELECT B.*,C.Nombre_Usuario FROM [Bitacora] As B INNER JOIN Cliente as C ON B.id_usuario = C.id_usuario where B.fecha between '" & Me.datepicker1.Value & "' and '" & Me.datepicker2.Value & "'"

    End Sub

    Protected Sub btnxml_Click(sender As Object, e As EventArgs) Handles btnxml.Click
        Dim miescritor As New XmlTextWriter(Server.MapPath("Bitacora.xml"), Nothing)
        miescritor.WriteStartDocument()
        miescritor.WriteStartElement("root")

        For Each bitacora As GridViewRow In Me.GridView1.Rows
            miescritor.WriteStartElement("Bitacora")
            miescritor.WriteStartElement("ID")
            miescritor.WriteString(bitacora.Cells(0).Text)
            miescritor.WriteEndElement()
            miescritor.WriteStartElement("Descripcion")
            miescritor.WriteString(bitacora.Cells(1).Text)
            miescritor.WriteEndElement()
            miescritor.WriteStartElement("Fecha")
            miescritor.WriteString(bitacora.Cells(2).Text)
            miescritor.WriteEndElement()
            miescritor.WriteStartElement("Usuario")
            miescritor.WriteString(bitacora.Cells(3).Text)
            miescritor.WriteEndElement()
            miescritor.WriteEndElement()
        Next
        miescritor.WriteEndElement()
        miescritor.WriteEndDocument()
        miescritor.Flush()
        miescritor.Close()

    End Sub
End Class