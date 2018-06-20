<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="VisualizarRanking.aspx.vb" Inherits="Vista.VisualizarRanking" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-warning  text-center" visible="false">
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelVisRanking" runat="server" Text="Visualizar Ranking" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                                                <div class="col-md-2 col-md-offset-3">
                            <asp:Button ID="btnjugadores" runat="server" Text="Ranking Jugadores" CssClass="btn btn-block btn-warning" />
                        </div>

                        <div class="col-md-2 col-md-offset-1">
                            <asp:Button ID="btnequipos" runat="server" Text="Ranking Equipos" CssClass="btn btn-block btn-warning" />
                        </div>
                        <br />
                                  <br />
                                  <br />
                        <div class="form-inline has-success">
                            <div class="col-md-3 col-md-offset-3">
                                <asp:Label ID="lbljuego" runat="server" Text="Juego:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:DropDownList ID="lstgame" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Game" DataTextField="Nombre"></asp:DropDownList>
                                    <span class="input-group-addon" id="basic-addon12"><span class="glyphicon glyphicon-heart" aria-hidden="true"></span></span>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblrol" runat="server" Text="Rol:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:DropDownList ID="lstrol" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Rol" DataTextField="Nombre"></asp:DropDownList>
                                    <span class="input-group-addon" id="basic-addon13"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span></span>
                                </div>
                            </div>
                        </div>
                            <br />
                                  <br />
                        <br />
                        <br />
                        <div class="form-horizontal">

                            <div id="datosjugadores" class="form-group" runat="server">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_jugadores" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_jugadores_PageIndexChanging" RowStyle-Height="40px">
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
                                            <asp:TemplateField HeaderText="Posicion"></asp:TemplateField>
                                            <asp:BoundField DataField="Nickname" HeaderText="Jugador" />
                                            <asp:BoundField DataField="Game.Nombre" HeaderText="Juego" />
                                            <asp:BoundField DataField="Rol_Jugador.Nombre" HeaderText="Rol" />
                                            <asp:BoundField DataField="Puntos" HeaderText="Puntos" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div id="datosequipos" class="form-group" runat="server">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_equipos" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_equipos_PageIndexChanging" RowStyle-Height="40px">
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
                                            <asp:TemplateField HeaderText="Posicion" ></asp:TemplateField>
                                            <asp:BoundField DataField="Nombre" HeaderText="Equipo" />
                                            <asp:BoundField DataField="Game.Nombre" HeaderText="Juego" />
                                            <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha Creacion" />
                                            <asp:BoundField DataField="Puntos" HeaderText="Puntos" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
