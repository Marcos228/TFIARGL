<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="EscribirSolicitud.aspx.vb" Inherits="Vista.EscribirSolicitud" %>

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
            <label id="lblSuccessEscrSolicitud" class="text-success">La Solicitud/Invitación se envió correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelEscrSolicitud" runat="server" Text="Envió de Solicitud/Invitación (Mensaje)" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">
                        <br />
                        <div id="Datos" runat="server" class="form-horizontal has-success">
                            <div class="form-group">
                                <asp:Label ID="lblPara" runat="server" Text="Para:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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
                                <asp:Label ID="lblmensaje" runat="server" Text="Mensaje:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtmensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon13"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnEnviar" name="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-block btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="Equipo" runat="server" />
    </div>
</asp:Content>
