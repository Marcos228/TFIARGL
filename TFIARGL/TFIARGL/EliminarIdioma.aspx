<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="EliminarIdioma.aspx.vb" Inherits="Vista.EliminarIdioma" MaintainScrollPositionOnPostback="true" %>

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
            <label id="lblsuccessdelIdioma" class="text-success">El Idioma se eliminó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelDelIdioma" runat="server" Text="Eliminar Idioma" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal has-success col-md-12">
                        <div class="form-group">
                                <asp:Label ID="lblidioma" runat="server" Text="Idioma:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstidioma" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Idioma" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-flag" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>      
                                                        <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6 col-md-offset-3">
                                  <asp:GridView ID="gv_idiomas" runat="server" CssClass="Grid-verde" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="10" RowStyle-Height="40px">
                                    <Columns>
                                        <asp:BoundField DataField="NombreUsu" HeaderText="Usuarios con el Perfil Seleccionado" />
                                    </Columns>
                                </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div id="botont" runat="server" class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btndelIdioma" name="btndelIdioma" runat="server" Text="Eliminar Idioma" CssClass="btn btn-block btn-warning" />
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="id_usuario" runat="server" />
</asp:Content>
