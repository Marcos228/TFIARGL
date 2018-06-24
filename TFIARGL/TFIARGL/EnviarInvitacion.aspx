<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="EnviarInvitacion.aspx.vb" Inherits="Vista.EnviarInvitacion" MaintainScrollPositionOnPostback="true" %>

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
            <label id="lblSuccessEnvInvitacion" class="text-success">La solicitud se aceptó correctamente. Has agregado al equipo a XXXX</label>
        </div>
        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div id="datos2" runat="server" class="panel panel-warning">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblAdminEnvInvitacion" runat="server" Text="Administración de Invitaciones (Aceptar Solicitudes)" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel2" runat="server" class="panel-body FondoPanel">
                        <div id="warningalert" runat="server" name="warningalert" class="alert alert-warning  text-center" visible="false">
                            <label runat="server" id="textowarning" class="text-warning"></label>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Solicitudes" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_Solucitudes_PageIndexChanging" RowStyle-Height="40px">
                                    <HeaderStyle CssClass="thead-dark" />
                                    <PagerTemplate>
                                        <div class="col-md-4 text-left">
                                            <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina"></asp:Label>
                                            <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged2" />
                                            <asp:Label ID="lblde" runat="server" Text="de"></asp:Label>
                                            <asp:Label ID="lbltotalpages" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-md-offset-4">
                                            <asp:Label ID="lblMostrar" runat="server" Text="Mostrar"></asp:Label>
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPageSize_SelectedPageSizeChanged2">
                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblRegistrosPag" runat="server" Text="Registros por Pagina"></asp:Label>
                                        </div>
                                    </PagerTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="Jugador.NickName" HeaderText="NickName" />
                                        <asp:BoundField DataField="Jugador.Rol_Jugador.Nombre" HeaderText="Rol" />
                                        <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:ImageButton ID="btn_aceptar_solicitud" runat="server" CommandName="A" ImageUrl="~/Imagenes/check.png" Height="20px" />
                                                    <asp:ImageButton ID="btn_rechazar_solicitud" runat="server" CommandName="R" ImageUrl="~/Imagenes/clear.png" Height="20px" />
                                                </div>

                                            </ItemTemplate>
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelEnvInvitacion" runat="server" Text="Administración de Invitaciones (Enviar Invitaciones)" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">

                        <br />
                        <div id="Datos" runat="server" class="form-horizontal has-success">
                            <div class="form-group">
                                <asp:Label ID="lblnombre" runat="server" Text="Nombre:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Jugadores" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_Jugadores_PageIndexChanging" RowStyle-Height="40px">
                                        <HeaderStyle CssClass="thead-dark" />
                                        <PagerTemplate>
                                            <div class="col-md-4 text-left">
                                                <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina"></asp:Label>
                                                <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged" />
                                                <asp:Label ID="lblde" runat="server" Text="de"></asp:Label>
                                                <asp:Label ID="lbltotalpages" runat="server" Text=""></asp:Label>
                                            </div>
                                            <div class="col-md-4 col-md-offset-4">
                                                <asp:Label ID="lblMostrar" runat="server" Text="Mostrar"></asp:Label>
                                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPageSize_SelectedPageSizeChanged">
                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblRegistrosPag" runat="server" Text="Registros por Pagina"></asp:Label>
                                            </div>
                                        </PagerTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="NickName" HeaderText="NickName" />
                                            <asp:BoundField DataField="Rol_Jugador.Nombre" HeaderText="Rol" />
                                            <asp:TemplateField HeaderText="Accion" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:ImageButton ID="btn_envio" runat="server" CommandName="E" ImageUrl="~/Imagenes/arrow.png" Height="18px" />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px"></HeaderStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnBuscar" name="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-block btn-success" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnrecomendar" name="btnrecomendar" runat="server" Text="Solicitar Recomendacion" CssClass="btn btn-block btn-warning" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <asp:HiddenField ID="Equipo" runat="server" />
        </div>
    </div>
</asp:Content>
