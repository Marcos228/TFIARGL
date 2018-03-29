<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ProductList.aspx.vb" Inherits="Vista.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--            <div class="container-fluid">
                <br />
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-warning">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelError" runat="server" Text="Error 404 - Page not Found" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                <h3 class="text-warning text-center"><strong>Pagina en Mantenimiento.</strong></h3>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="Label1" class="text-success">El Producto se creó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelLogin" runat="server" Text="Ranking" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal ">
 
                        </div>
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnAceptar" name="btnAceptar" runat="server" Text="Ingresar" CssClass="btn btn-block btn-success" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
