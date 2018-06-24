<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="VisualizarTorneo.aspx.vb" Inherits="Vista.VisualizarTorneo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script src="http://player.twitch.tv/js/embed/v1.js"></script>
    <div id="<player div ID>"></div>
    <script type="text/javascript">
        var options = {
            width: <width>,
    height: <height>,
    channel: "<channel ID>",
    video: "<video ID>",
    collection: "<collection ID>",
  };
  var player = new Twitch.Player("<player div ID>", options);
  player.setVolume(0.5);
    </script>
    <script type="text/javascript">
                                function openWin() {
                                        window.open("https://www.mercadopago.com/mla/checkout/start?pref_id=244721774-f40bff85-3c32-4c24-bd51-a1315fd0e884");
}
                              
    </script>
    <script type="text/javascript">
(function(){function $MPC_load() { window.$MPC_loaded !== true && (function () { var s = document.createElement("script"); s.type = "text/javascript"; s.async = true; s.src = document.location.protocol + "//secure.mlstatic.com/mptools/render.js"; var x = document.getElementsByTagName('script')[0]; x.parentNode.insertBefore(s, x); window.$MPC_loaded = true; })(); }window.$MPC_loaded !== true ? (window.attachEvent ?window.attachEvent('onload', $MPC_load) : window.addEventListener('load', $MPC_load, false)) : null;})();
    </script>

    <div class="container-fluid">
        <br />

        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="lblSuccessVisTorneo" class="text-success">Su Inscripcion esta pendiente de aprobacion, Gracias por su Compra.</label>
        </div>
        <div class="col-md-12">
            <div id="Datos" class="jumbotron" runat="server">
                <h1 id="titulotorneo" class="text-center" runat="server"></h1>
                <hr class="my-4" />
                <div class="col-md-2" style="border-right: ridge; border-right-width: 2px; border-right-color: #d5d5d5;">
                    <h5 id="lblfechadesde">Fecha Desde:</h5>
                    <h4 id="fechadesde" runat="server" class="text-left"></h4>
                </div>
                <div class="col-md-2 " style="border-right: ridge; border-right-width: 2px; border-right-color: #d5d5d5;">
                    <h5 id="lblfechainicioins">Fecha Inicio Inscripcion:</h5>
                    <h4 id="fechainicio" runat="server" class="text-left"></h4>
                </div>
                <div class="col-md-2" style="border-right: ridge; border-right-width: 2px; border-right-color: #d5d5d5;">
                    <h5 id="lblfechafinins">Fecha Fin Inscripcion:</h5>
                    <h4 id="fechafin" runat="server" class="text-left "></h4>
                </div>
                <div class="col-md-2" style="border-right: ridge; border-right-width: 2px; border-right-color: #d5d5d5;">
                    <h5 id="lblfechahasta">Fecha Hasta:</h5>
                    <h4 id="fechahasta" runat="server" class="text-left "></h4>
                </div>
                <div class="col-md-2" style="border-right: ridge; border-right-width: 2px; border-right-color: #d5d5d5;">
                    <h5 id="lblprecio">Precio:</h5>
                    <h4 id="precio" runat="server" class="text-left text"></h4>
                </div>
                <div class="col-md-2">
                    <h5 id="lbljuego">Juego:</h5>
                    <h4 id="juego" runat="server" class="text-left text"></h4>
                </div>
                <br />
                <br />
                <br />
                <br />
                <div>
                         <h3 id="H3partidas" class="text-center" runat="server"></h3>
                    <asp:GridView CssClass="table table-bordered table-responsive" ID="gv_partidas" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_partidas_PageIndexChanging" RowStyle-Height="40px">
                        <HeaderStyle CssClass="thead-dark" />
                        <PagerTemplate>
                            <div class="col-md-4 text-left">
                                <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina"></asp:Label>
                                <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged" />
                                <asp:Label ID="lblde" runat="server" Text="de"></asp:Label>
                                <asp:Label ID="lbltotalpages" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-6 col-md-offset-2">
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
                            <asp:BoundField DataField="Fase" HeaderText="Fase" />
                            <asp:BoundField DataField="FechaHora" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy HH:mm:ss }" />
                            <asp:BoundField DataField="Equipos(0).Nombre" HeaderText="Equipo Local" />
                            <asp:BoundField DataField="Equipos(1).Nombre" HeaderText="Equipo Visitante" />
                            <asp:BoundField DataField="Ganador.Nombre" HeaderText="Ganador" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                        <h3 id="H3premios" class="text-center" runat="server"></h3>
                    <asp:GridView CssClass="table table-bordered table-responsive table-Secondary " ID="gv_premios" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" RowStyle-Height="40px">
                        <HeaderStyle CssClass="thead-dark" />
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Premio" />
                            <asp:BoundField DataField="Posicion" HeaderText="Posicion" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="valor" HeaderText="Valor" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />

                <div class="row ">
                    <div class="">
                        <div class="col-md-6">
                            <div class="embed-responsive embed-responsive-16by9 ">
                                <iframe id="yourube" runat="server" class="embed-responsive-item" src="" frameborder="0" allowfullscreen="true" scrolling="no" height="300"></iframe>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="embed-responsive embed-responsive-16by9">
                                <iframe id="twitch" runat="server" class="embed-responsive-item" src="" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen height="300"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
                    <label runat="server" id="textovalid" class="text-danger"></label>
                </div>
                <br />
                <div id="btnins" runat="server" class="row">
                    <div class="col-md-4 col-md-offset-4">
                        <asp:Button ClientIDMode="Static" ID="btnInscribir" name="btnInscribir" runat="server" Text="Inscribir" CssClass="btn btn-block btn-success" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="id_game" runat="server" />
    <asp:HiddenField ID="id_torneo" runat="server" />
</asp:Content>
