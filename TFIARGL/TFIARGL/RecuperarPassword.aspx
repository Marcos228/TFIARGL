<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="RecuperarPassword.aspx.vb" Inherits="Vista.RecuperarPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label  id="lblsuccessRecPass" class="text-success">Revise su casilla de correo para continuar con la operación.</label>
        </div>

        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-warning">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelRecuperoPass" runat="server" Text="Restauracion de Contraseña" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <div class="form-horizontal has-warning">
                            <div class="form-group">
                                <asp:Label ID="lblUsuario" runat="server" Text="Usuario:" CssClass="col-md-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RtxtUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="*" EnableClientScript="false" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="btnpass" runat="server" Text="Cambiar Contraseña" CssClass="btn btn-block btn-default" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
