<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="MyOrders.aspx.vb" Inherits="Vista.MyOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="container-fluid">
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
    </div>
</asp:Content>
