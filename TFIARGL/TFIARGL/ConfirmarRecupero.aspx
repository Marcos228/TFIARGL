<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConfirmarRecupero.aspx.vb" Inherits="Vista.ConfirmarRecupero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label  id="lblsuccessRecupero" class="text-success">Su contraseña ha sido cambiada con exito.</label>
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
                                <asp:Label ID="lblpass" runat="server" Text="Contraseña:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input type="password" id="txtpass" runat="server" class="form-control" />
                                        <span class="input-group-addon" id="basic-addon9"><span class="glyphicon glyphicon-lock" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtpass" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblpassconf" runat="server" Text="Confirmar Contraseña:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input type="password" id="txtpass2" runat="server" class="form-control" />
                                        <span class="input-group-addon" id="basic-addon10"><span class="glyphicon glyphicon-lock" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpass2" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
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
