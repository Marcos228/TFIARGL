﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="BitacoraErrores.aspx.vb" Inherits="Vista.BitacoraErrores" MaintainScrollPositionOnPostback="true" %>

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
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-warning  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger">No se encontraron Bitacoras para los filtros seleccionados</label>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelBackup" runat="server" Text="Bitácora" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-inline has-success">
                            <div class="col-md-3 col-md-offset-2">
                                <asp:Label ID="lblfecha" runat="server" Text="Fecha Desde:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker1" readonly="true" name="datepicker1" />
                                    <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        <asp:TextBox ID="Fecha_Desde" runat="server" Visible="False"></asp:TextBox>
                                    </span>

                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblfecha2" runat="server" Text="Fecha Hasta:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <input runat="server" clientidmode="Static" class="form-control" type="text" id="datepicker2" name="datepicker2" readonly="true" />
                                    <span class="input-group-addon" id="basic-addon2"><span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    </span>

                                </div>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblusuarios" runat="server" Text="Usuario:" CssClass="control-label labelform"></asp:Label>
                                <div class="input-group">
                                    <asp:DropDownList ID="lstusuarios" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Usuario" DataTextField="NombreUsu"></asp:DropDownList>
                                    <span class="input-group-addon" id="basic-addon10"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
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
                        <div class="form-horizontal">

                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Bitacora" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_Bitacora_PageIndexChanging">
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
                                            <asp:BoundField DataField="ID_Bitacora" HeaderText="ID" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="Detalle" HeaderText="Detalle" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="Usuario.NombreUsu" HeaderText="Usuario" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="IP_Usuario" HeaderText="IP" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="Tipo_Bitacora" HeaderText="Tipo" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="URL" HeaderText="Ubicación" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="Exception" HeaderText="Excepción" ItemStyle-Font-Size="12px" />
                                            <asp:BoundField DataField="StackTrace" HeaderText="StackTrace" ItemStyle-Font-Size="13px" />
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