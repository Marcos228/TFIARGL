<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="BaseCorrupta.aspx.vb" Inherits="Vista.BaseCorrupta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="container-fluid">
                <br />
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-danger">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelError" runat="server" Text="Peligro" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                <h3 class="text-danger text-center"><strong>La base de datos se encuentra comprometida. Por favor repare la integridad para continuar.</strong>
                       <asp:Label ID="FilasCorruptas" runat="server" ></asp:Label>                             
                </h3>
                        <br/>
                         <asp:Button ID="btnReparar" runat="server" Text="Reparar" CssClass="btn btn-block btn-warning" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
