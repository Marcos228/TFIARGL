<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="VisualizarEstadisticas.aspx.vb" Inherits="Vista.VisualizarEstadisticas" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelVisEstadisticas" runat="server" Text="Visualizar Estadisticas" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">
                           <h1 id="Nombre" class="text-center" runat="server"></h1>
                        <br />
                        <div id="Datos" runat="server" class="form-horizontal has-success">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_partidas" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_partidas_PageIndexChanging" RowStyle-Height="40px">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <PagerTemplate>
                                                <div class="col-md-4 text-left">
                                                    <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina"></asp:Label>
                                                    <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged2" />
                                                    <asp:Label ID="lblde" runat="server" Text="de"></asp:Label>
                                                    <asp:Label ID="lbltotalpages" runat="server" Text=""></asp:Label>
                                                </div>
                                                <div class="col-md-6 col-md-offset-2">
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
                                                <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                                <asp:BoundField DataField="FechaHora" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy HH:mm:ss }" />
                                                <asp:BoundField DataField="Equipos(0).Nombre" HeaderText="Equipo Local" />
                                                <asp:BoundField DataField="Equipos(1).Nombre" HeaderText="Equipo Visitante" />
                                                <asp:BoundField DataField="Ganador.Nombre" HeaderText="Ganador" />
                                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <div>
                                                            <asp:ImageButton ID="btn_Seleccionar" runat="server" CommandName="S" ImageUrl="~/Imagenes/arrow.png" Height="18px" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px"></HeaderStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                                            <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_estadisticas" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_estadisticas_PageIndexChanging" RowStyle-Height="40px">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <PagerTemplate>
                                                <div class="col-md-4 text-left">
                                                    <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina"></asp:Label>
                                                    <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged3" />
                                                    <asp:Label ID="lblde" runat="server" Text="de"></asp:Label>
                                                    <asp:Label ID="lbltotalpages" runat="server" Text=""></asp:Label>
                                                </div>
                                                <div class="col-md-6 col-md-offset-2">
                                                    <asp:Label ID="lblMostrar" runat="server" Text="Mostrar"></asp:Label>
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPageSize_SelectedPageSizeChanged3">
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
                                                <asp:BoundField DataField="Jugador.Nickname" HeaderText="Jugador" />
                                                <asp:BoundField DataField="Equipo.Nombre" HeaderText="Equipo" />

                                            </Columns>
                                        </asp:GridView>
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
