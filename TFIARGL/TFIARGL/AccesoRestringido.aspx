<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AccesoRestringido.aspx.vb" Inherits="Vista.AccesoRestringido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container-fluid">
                <br />
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-danger">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblErrorAcceso" runat="server" Text="Acceso Denegado" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                <h3 class="text-danger text-center"><strong> <asp:Label ID="lblAccesoDenegado" runat="server" Text="Usted no tiene permisos para acceder a esta pagina."></asp:Label></strong></h3>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
