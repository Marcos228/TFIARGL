<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ABMSponsor.aspx.vb" Inherits="Vista.CrearSponsor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- <script type="text/javascript" src="JS/ClienteValid.js"></script>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label  id="lblSuccessAddSponsor" class="text-success">El Sponsor correctamente.</label>
        </div>
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelAddSponsor" runat="server" Text="Administracion de Sponsor" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal has-success">     
                                                    <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btn_nuevo" name="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-block btn-info" />
                            </div>
                        </div>
                            <br />

                                                        <div class="form-group">
                                <div>
                                    <div>
                                        <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_sponsors" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_sponsors_PageIndexChanging" RowStyle-Height="40px">
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
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="CUIL" HeaderText="CUIL" />
                                                <asp:BoundField DataField="Correo" HeaderText="Correo" />
                                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <div>
                                                           <asp:ImageButton ID="btn_editar" runat="server" CommandName="E" ImageUrl="~/Imagenes/edit.png" Height="18px" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100px"></HeaderStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblnombre" runat="server" Text="Nombre:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon2"><span class="	glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnombre" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblcuil" runat="server" Text="CUIL:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtcuil" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon8"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtusuario" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                                                        <div class="form-group">
                                <asp:Label ID="lblcorreo" runat="server" Text="Correo:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtcorreo" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon9"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtusuario" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnAceptar" name="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-block btn-success" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="id_sponsor" runat="server" />

    </div>
</asp:Content>
