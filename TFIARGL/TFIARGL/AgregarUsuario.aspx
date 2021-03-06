﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AgregarUsuario.aspx.vb" Inherits="Vista.AgregarUsuario" %>

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
            <label  id="lblSuccessAddUser" class="text-success">El Usuario se creó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelAddUser" runat="server" Text="Usuarios" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal has-success">
                                   <div class="form-group">
                                <asp:Label ID="lblapellido" runat="server" Text="Apellido:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtapellido" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon1"><span class="	glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtapellido" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>        
                            <div class="form-group">
                                <asp:Label ID="lblnombre" runat="server" Text="Nombre:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon2"><span class="	glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnombre" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblusuarioname" runat="server" Text="Nombre de Usuario:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtusuario" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon8"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtusuario" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
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
                                <asp:Label ID="lblperfil" runat="server" Text="Perfil:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstperfil" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Permiso" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon10"><span class="	glyphicon glyphicon-list-alt" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
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
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnAceptar" name="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-block btn-success" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
