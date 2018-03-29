<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Login.aspx.vb" Inherits="Vista.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />

        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="Label1" class="text-success">El Cliente se registró correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-warning">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelLogin" runat="server" Text="Autenticación" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <br />
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
                            <div class="form-group">

                                <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon2"><span class="glyphicon glyphicon-lock" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RtxtPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                                                <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="btnregistro" runat="server" Text="Registrarse" CssClass="btn btn-block btn-warning" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="btnpass" runat="server" Text="¿Olvido su Contraseña?" CssClass="btn btn-block btn-default" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="btnAceptar" runat="server" Text="Ingresar" CssClass="btn btn-block btn-success" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
