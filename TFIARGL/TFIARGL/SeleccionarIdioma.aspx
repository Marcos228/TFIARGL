<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="SeleccionarIdioma.aspx.vb" Inherits="Vista.SeleccionarIdioma" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="lblsuccessSelIdioma" class="text-success">El Idioma se cambió correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelSelIdioma" runat="server" Text="Seleccionar Idioma" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal has-success col-md-12">
                        <div class="form-group">
                                <asp:Label ID="lblidioma" runat="server" Text="Idioma:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstidioma" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Idioma" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-flag" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>      
                        </div>
                        <br />
                        <br />            
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnAceptar" name="btnAceptar" runat="server" Text="" CssClass="btn btn-block btn-info" />
                            </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="id_usuario" runat="server" />
</asp:Content>
