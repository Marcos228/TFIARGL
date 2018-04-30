<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ModificarPerfil.aspx.vb" Inherits="Vista.ModificarPerfil" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                if (o.hasChildNodes) {
                    __doPostBack("", "");
                }
            }
        }
    </script>
    <div class="container-fluid">


        <br />
        <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>

        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label id="lblsuccessModPerfil" class="text-success">Se modificó el Perfil correctamente.</label>
        </div>
        <div class="row">
            <div class="col-md-10 col-md-offset-1">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelModPerfil" runat="server" Text="Modificar Perfiles" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <div class="form-horizontal ">
                            <div class="form-group">
                                <asp:Label ID="lblperfil" runat="server" Text="Perfil:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstperfil" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Permiso" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon8"><span class="	glyphicon glyphicon-list-alt" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-5 col-md-offset-1">
                                    <div class="panel panel-warning">
                                        <div class="panel-heading text-center">
                                            <asp:Label ID="lblpermisosactuales" runat="server" Text="Permisos Actuales" CssClass="TituloPanel"></asp:Label>
                                        </div>
                                        <div class="panel-body FondoPanel">
                                                                                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" enabled="false">
                                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                    </asp:TreeView>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5 ">
                                    <div class="panel panel-success">
                                        <div class="panel-heading text-center">
                                            <asp:Label ID="lblnuevosPermisos" runat="server" Text="Nuevos Permisos" CssClass="TituloPanel"></asp:Label>
                                        </div>
                                        <div class="panel-body FondoPanel">
                                                                                <asp:TreeView ID="TreeView2" runat="server" ImageSet="Arrows" ShowCheckBoxes="All">
                                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                    </asp:TreeView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6 col-md-offset-3">
                                        <asp:GridView ID="gv_Perfiles" runat="server" CssClass="Grid-verde" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="10" RowStyle-Height="40px">
                                            <Columns>
                                                <asp:BoundField DataField="NombreUsu" HeaderText="Usuarios con el Perfil Seleccionado" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:Button ID="btnmodificarperfil" runat="server" Text="Modificar Perfil" CssClass="btn btn-block btn-warning" />
                                </div>
                                <br />
                            </div>
    </div>
    </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
