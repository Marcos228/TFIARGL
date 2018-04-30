<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Restore.aspx.vb" Inherits="Vista.Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
                <div id="alertvalid" runat="server" name="alertvalid" class="alert alert-danger  text-center" visible="false">
            <label runat="server" id="textovalid" class="text-danger"></label>
        </div>
        <div id="success" runat="server" name="success" class="alert alert-success  text-center" visible="false">
            <label  id="lblsuccessRestore" class="text-success">Se Realizó la restauracion de la base de datos correctamente.</label>
        </div>
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelRestore" runat="server" Text="Restauración del Sistema" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                               <div class="form-horizontal has-warning">

                         <div class="form-group">
                            <div class="col-md-12">
                                <h5 class="text-warning text-center"><strong>  <asp:Label ID="lblinforestore" runat="server" Text="Solamente se podrán ingresar archivos previamente generados por el sistema, de lo contrario no funcionará."></asp:Label></strong></h5>
                            </div>
                        </div>
                                                    <div class="form-group">
                                <asp:Label ID="lblbackupserv" runat="server" Text="Backup en Servidor:" CssClass="col-sm-3 col-sm-offset-1 control-label labelform"></asp:Label>
                                <div class="col-md-7">
                                    <div class="input-group">
                                        <asp:DropDownList ID="Backups" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon10"><span class="glyphicon glyphicon-hdd" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                        <br />
                       <div class="form-group">
                              <asp:Label ID="lblbackup" runat="server" Text="Ingresar Backup:" CssClass="col-sm-3 col-sm-offset-1 control-label labelform"></asp:Label>
                            <div class="col-md-7">
                                <asp:FileUpload ID="FileUpload1" CssClass="btn btn-block btn-default" runat="server" />
                            </div>
                        </div>
    </div>
                        <br />
                            <div class="form-group">
                            <div class="col-md-5 col-md-offset-1">
                                <asp:Button ID="btnserver" CssClass="btn btn-block btn-danger" runat="server" Text="Restaurar Backup Servidor" />
                            </div>
                                          <div class="col-md-5">
                            <asp:Button ID="btnlocal" CssClass="btn btn-block btn-warning" runat="server" Text="Restaurar Backup Importado" />
                            </div>
                                </div>  
                            <br />
                        </div>
                </div>
            </div>
        </div>
    </div>
    <br />

</asp:Content>
