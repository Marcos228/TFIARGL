<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Backup.aspx.vb" Inherits="Vista.BackUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-info">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelBackup" runat="server" Text="Resguardo de Seguridad" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <div class="row">
                            <div class="col-md-12">
                                <h5 class="text-warning text-center"><strong>Apretando el siguiente botón se generará un archivo de extensión .bak en el servidor y este podra ser descargado para su posterior utilizacion en la restauración de la base de datos.</strong></h5>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ID="Button1" runat="server" Text="Realizar Backup" CssClass="btn btn-block btn-warning" />
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
