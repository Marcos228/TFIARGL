<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConsultarBitacoraAuditoria.aspx.vb" Inherits="Vista.ConsultarBitacoraAuditoria" %>

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
                                    <asp:GridView ID="GridView1" CssClass="Grid-verde" runat="server" AutoGenerateColumns="False" DataKeyNames="id_bitacora" DataSourceID="SqlDataSource1" HorizontalAlign="Center">

                                        <Columns>
                                            <asp:BoundField DataField="id_bitacora" HeaderText="Numeración" ReadOnly="True" SortExpression="id_bitacora" />
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
                                            <asp:BoundField DataField="Nombre_Usuario" HeaderText="Nombre_Usuario" SortExpression="Nombre_Usuario" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SaitamaConnectionString2 %>"></asp:SqlDataSource>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
                                </div>
                            </div>
                        </div>
                        <asp:Button ID="btnxml" runat="server" Text="Exportar XML" CssClass="btn btn-block btn-default" />

                    </div>

                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
