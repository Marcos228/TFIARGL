<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="CrearTorneo.aspx.vb" Inherits="Vista.CrearTorneo" MaintainScrollPositionOnPostback="true" %>

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
            <label id="lblSuccessAddTorneo" class="text-success">El torneo se creó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelAddTorneo" runat="server" Text="Crear Torneo" CssClass="TituloPanel"></asp:Label>
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
                                <asp:Label ID="lblfechadesde" runat="server" Text="Fecha Desde:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input runat="server" clientidmode="Static" class="form-control Nodisable" type="text" id="datepicker1" name="datepicker1" readonly="true" />
                                        <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblfechahasta" runat="server" Text="Fecha Hasta:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker2" name="datepicker2" readonly="true" />
                                        <span class="input-group-addon" id="basic-addon2"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>
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
                                <asp:Label ID="lblfechainicioins" runat="server" Text="Fecha Inicio Inscripción:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker3" name="datepicker3" readonly="true" />
                                        <span class="input-group-addon" id="basic-addon10"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblfechafinins" runat="server" Text="Fecha Fin Inscripcion:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker4" name="datepicker4" readonly="true" />
                                        <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblprecio" runat="server" Text="Precio:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtprecio" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon13"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblcantidad" runat="server" Text="Cantidad Participantes:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtcantidad" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon14"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblyoutube" runat="server" Text="Youtube:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtyoutube" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon17"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbltwitch" runat="server" Text="Twitch:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txttwitch" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon18"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                        </span>

                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblSponsors" runat="server" Text="Sponsors:" CssClass=" col-sm-4 control-label labelform"></asp:Label>
                                <br />
                                <br />
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_sponsors" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_sponsors_PageIndexChanging" RowStyle-Height="40px">
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
                                            <asp:BoundField DataField="CUIL" HeaderText="CUIL" />
                                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
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
                            <div class="panel panel-warning">
                                <div class="panel-heading text-center">
                                    <asp:Label ID="lblpremios" runat="server" Text="Premios" CssClass="TituloPanel"></asp:Label>
                                </div>
                                <div id="Panel2" runat="server" class="panel-body FondoPanel">
                                    <div id="Ocultable" runat="server">
                                        <div class="form-inline has-warning">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblNombrePremio" runat="server" Text="Nombre Premio:" CssClass="control-label labelform"></asp:Label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtnombrepremio" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon" id="basic-addon20"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                                    </span>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lbldescripcion" runat="server" Text="Descripcion:" CssClass="control-label labelform"></asp:Label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtdescripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon" id="basic-addon21"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span>
                                                    </span>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblposicion" runat="server" Text="Posicion:" CssClass="control-label labelform"></asp:Label>
                                                <div class="input-group">
                                                    <asp:DropDownList ID="lstposicion" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <span class="input-group-addon" id="basic-addon22"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblvalor" runat="server" Text="Valor:" CssClass="control-label labelform"></asp:Label>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtvalor" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon" id="basic-addon23"><span class="glyphicon glyphicon-asterisk" aria-hidden="true"></span></span>
                                                </div>
                                            </div>
                                        </div>

                                        <br />
                                        <br />
                                        <br />

                                        <br />
                                        <div class="col-md-2 col-md-offset-5">
                                            <asp:Button ID="btnagregar" runat="server" Text="Agregar" CssClass="btn btn-block btn-warning" />
                                        </div>
                                        <br />
                                    </div>
                                    <br />
                                    <div class="form-group">
                                        <div>
                                            <div>
                                                <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_premios" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" RowStyle-Height="40px">
                                                    <HeaderStyle CssClass="thead-dark" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                        <asp:BoundField DataField="Posicion" HeaderText="Posicion" />
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                                                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <asp:ImageButton ID="btn_eliminar" runat="server" CommandName="E" ImageUrl="~/Imagenes/clear.png" Height="18px" />
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
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ClientIDMode="Static" ID="btnCrear" name="btnCrear" runat="server" Text="Crear" CssClass="btn btn-block btn-success" />
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
