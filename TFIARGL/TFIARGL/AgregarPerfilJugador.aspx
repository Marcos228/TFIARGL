﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AgregarPerfilJugador.aspx.vb" Inherits="Vista.AgregarPerfilJugador" %>

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
            <label id="lblSuccessAddPerfJug" class="text-success">El Perfil de Jugador se creó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelAddPerfJug" runat="server" Text="" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">
                        <br />
                        <div id="Datos" runat="server" class="form-horizontal has-success">
                            <div class="form-group">
                                <asp:Label ID="lblnickname" runat="server" Text="NickName:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnickname" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon8"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbljuego" runat="server" Text="Juego:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstgame" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Game" DataTextField="Nombre" Enabled="False"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon12"><span class="glyphicon glyphicon-heart" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblroljugador" runat="server" Text="Rol Jugador:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstroljugador" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Rol" DataTextField="Nombre" Enabled="False"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon10"><span class="	glyphicon glyphicon-star" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblgametag" runat="server" Text="GameTag:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtgametag" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnAceptar" name="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-block btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="id_game" runat="server" />
    </div>
</asp:Content>
