<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AgregarPerfil.aspx.vb" Inherits="Vista.AgregarPerfil" MaintainScrollPositionOnPostback="true" %>

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
            <label id="lblsuccessAddPerfil" class="text-success">Se Creó el Perfil correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelAddPerfil" runat="server" Text="Crear Perfiles" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                                                <div class="form-horizontal ">
                            <div class="form-group">
                                <asp:Label ID="lblnombreAddPerfil" runat="server" Text="Nombre de Perfil:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon8"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtnombre" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>


                        <div class="row">
                            <div class="col-md-12">
                                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowCheckBoxes="All"  >
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                    <ParentNodeStyle Font-Bold="False" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                </asp:TreeView>
                          </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="btnAddPerfil" runat="server" Text="Crear Perfil" CssClass="btn btn-block btn-warning" />
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
