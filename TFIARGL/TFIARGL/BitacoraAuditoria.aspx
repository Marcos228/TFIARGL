<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="BitacoraAuditoria.aspx.vb" Inherits="Vista.ConsultarBitacoraAuditoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelBackup" runat="server" Text="Bitácora" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-inline has-success">
                            <div class="col-md-6">
                                <asp:Label ID="lblfecha" runat="server" Text="Fecha Desde:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker1" name="datepicker1" runat="server" />

                                    <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        <asp:TextBox ID="Fecha_Desde" runat="server" Visible="False"></asp:TextBox>
                                    </span>

                                </div>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblfecha2" runat="server" Text="Fecha Hasta:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" onclick="Static" class="form-control" type="text" id="datepicker2" name="datepicker2" />
                                    <span class="input-group-addon" id="basic-addon2"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </span>

                                </div>
                            </div>
                        </div>


                        <br />
                        <br />

                        <br />
                        <div class="col-md-4 col-md-offset-4">
                            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-block btn-warning" />
                        </div>
                        <br />

                        <br />
                        <br />
                        <div class="form-horizontal">

                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Bitacora" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_Usuarios_PageIndexChanging" RowStyle-Height="40px">
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
                                                <asp:Label ID="lblRegistros" runat="server" Text="Registros por Pagina"></asp:Label>
                                            </div>
                                        </PagerTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="ID_Bitacora" HeaderText="ID" />
                                            <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Idioma" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" />
                                            <asp:BoundField DataField="Usuario.NombreUsu" HeaderText="Usuario" />
                                            <asp:BoundField DataField="IP_Usuario" HeaderText="IP Usuario" />
                                            <asp:BoundField DataField="Browser" HeaderText="Navegador" />
                                            <asp:BoundField DataField="Tipo_Bitacora" HeaderText="Tipo Bitacora" />
                                            <asp:BoundField DataField="Valor_Anterior" HeaderText="Valor Anterior" />
                                            <asp:BoundField DataField="Valor_Posterior" HeaderText="Valor Posterior" />

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
