<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Default.aspx.vb" Inherits="Vista.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row" id="carusel" runat="server">
            <div id="myCarousel" class="carousel slide">
                <!-- Indicators -->
                <ol class="carousel-indicators">
                    <li class="item1 active"></li>
                    <li class="item2"></li>
                    <li class="item3"></li>
                </ol>
                <!-- Wrapper for slides -->
                <div class="carousel-inner" role="listbox">
                    <div class="item active">
                        <img src="Imagenes/estadio.jpg" />
                    </div>

                    <div class="item">
                        <img src="Imagenes/recital.jpeg" />
                    </div>

                    <div class="item">
                        <img src="Imagenes/rural.jpg" />
                    </div>
                </div>

                <!-- Left and right controls -->
                <a class="left carousel-control" href="#myCarousel" role="button">
                    <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="right carousel-control" href="#myCarousel" role="button">
                    <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>
        </div>
        <div id="datos" runat="server" visible="false">
            <div class="panel panel-warning">
                <div class="panel-heading text-center">
                    <asp:Label ID="lblTorneosParticipativos" runat="server" Text="Torneos Participativos" CssClass="TituloPanel"></asp:Label>
                </div>
                <div id="Panel" runat="server" class="panel-body FondoPanel">

                    <div class="form-group">
                        <div>
                            <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_torneos" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_torneos_PageIndexChanging" RowStyle-Height="40px">
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
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Game.Nombre" HeaderText="Juego" />
                                    <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha Creacion" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="Fecha_Fin" HeaderText="Fecha Fin" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="Fecha_Inicio_Inscripcion" HeaderText="Fecha Inicio Inscripcion" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="Fecha_Fin_Inscripcion" HeaderText="Fecha Fin Inscripcion" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="Precio_Inscripcion" HeaderText="Precio" />
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
                </div>
            </div>
        </div>

        <script>
            $(document).ready(function () {
                // Activate Carousel
                $("#myCarousel").carousel();

                // Enable Carousel Indicators
                $(".item1").click(function () {
                    $("#myCarousel").carousel(0);
                });
                $(".item2").click(function () {
                    $("#myCarousel").carousel(1);
                });
                $(".item3").click(function () {
                    $("#myCarousel").carousel(2);
                });

                // Enable Carousel Controls
                $(".left").click(function () {
                    $("#myCarousel").carousel("prev");
                });
                $(".right").click(function () {
                    $("#myCarousel").carousel("next");
                });
            });
        </script>
    </div>
</asp:Content>
