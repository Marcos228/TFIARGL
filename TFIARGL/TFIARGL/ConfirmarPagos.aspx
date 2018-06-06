<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConfirmarPagos.aspx.vb" Inherits="Vista.ConfirmarPagos" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(function () {
            $("#datepicker1").datepicker();
            $("#datepicker2").datepicker();
        });

    </script>
    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="lblErrorConPagos" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="lblSuccessConPagos" class="text-success"></label>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelConPagos" runat="server" Text="Confirmar Pagos" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div id="Panel" runat="server" class="panel-body FondoPanel">
                        <br />

                        <div class="form-inline has-success">
                            <div class="col-md-2 col-md-offset-1">
                                <asp:Label ID="lblfechadesde" runat="server" Text="Fecha Desde:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker1" readonly="true" name="datepicker1" />
                                    <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        <asp:TextBox ID="Fecha_Desde" runat="server" Visible="False"></asp:TextBox>
                                    </span>

                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblfechahasta" runat="server" Text="Fecha Hasta:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker2" name="datepicker2" readonly="true" />
                                    <span class="input-group-addon" id="basic-addon2"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </span>

                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblusuario" runat="server" Text="Usuario:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtusuarios" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon" id="basic-addon10"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lbltorneo" runat="server" Text="Torneo:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:TextBox ID="txttorneo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span></span>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblestado" runat="server" Text="Estado:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:DropDownList ID="lstestado" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="EStado" DataTextField="Estado"></asp:DropDownList>

                                    <span class="input-group-addon" id="basic-addon12"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span></span>
                                </div>
                            </div>
                        </div>


                        <br />
                        <br />
                        <br />

                        <br />
                        <div class="col-md-2 col-md-offset-5">
                            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-block btn-warning" />
                        </div>
                        <br />

                        <br />
                        <br />

                        <div id="Datos" runat="server" class="form-horizontal has-success">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Facturas" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_facturas_PageIndexChanging" RowStyle-Height="40px">
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
                                            <asp:BoundField DataField="ID_Factura" HeaderText="ID" />
                                            <asp:BoundField DataField="Torneo.Nombre" HeaderText="Torneo" />
                                            <asp:BoundField DataField="Usuario.NombreUsu" HeaderText="Usuario" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy}" />
                                            <asp:BoundField DataField="Monto_Total" HeaderText="Total" />
                                            <asp:BoundField DataField="Tipo_Pago.Descripcion" HeaderText="Tipo Pago" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:ImageButton ID="btn_Seleccionar" runat="server" CommandName="S" ImageUrl="~/Imagenes/check.png" Height="18px" />
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px"></HeaderStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnConfirmar" name="btnConfirmar" runat="server" Text="Confirmar" CssClass="btn btn-block btn-success" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
