<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="InscribirTorneo.aspx.vb" Inherits="Vista.InscribirTorneo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(function () {
            $("#datepicker1").datepicker();
            $("#datepicker2").datepicker();
            $("#datepicker3").datepicker();
            $("#datepicker4").datepicker();
        });

    </script>
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="lblSuccessInsTorneo" class="text-success"></label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelInsTorneo" runat="server" Text="Inscribirse en Torneo" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">
                        <br />
                        <div id="Datos" runat="server" class="form-horizontal has-success">
                            <div class="form-group">
                                <div class="col-md-12">
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
                                            <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha Creacion" DataFormatString="{0:dd-MM-yyyy}"/>
                                            <asp:BoundField DataField="Fecha_Fin" HeaderText="Fecha Fin" DataFormatString="{0:dd-MM-yyyy}"/>
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
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnInscribir" name="btnInscribir" runat="server" Text="Inscribir" CssClass="btn btn-block btn-success" />
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
