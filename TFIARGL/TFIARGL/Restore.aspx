<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Restore.aspx.vb" Inherits="Vista.Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
                <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label  id="Label1" class="text-success">Se Realizó la restauracion de la base de datos correctamente.</label>
        </div>
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelError" runat="server" Text="Restauración del Sistema" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">

                        <div class="row">
                            <div class="col-md-12">
                                <h5 class="text-warning text-center"><strong>Solamente se podrán ingresar archivos de extension .bak previamente generados por el sistema, de lo contrario no funcionará.</strong></h5>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6 col-md-offset-3">
                                <asp:FileUpload ID="FileUpload1" CssClass="btn btn-block btn-default" runat="server" />
                            </div>
                        </div>

                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="Button1" CssClass="btn btn-block btn-warning" runat="server" Text="Realizar Restauración" />

                            </div>
                            <br />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />

</asp:Content>
