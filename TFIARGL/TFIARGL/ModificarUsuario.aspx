<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ModificarUsuario.aspx.vb" Inherits="Vista.ModificarUsuario" MaintainScrollPositionOnPostback="true" %>

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
            <label id="lblsuccessmodUser" class="text-success">El Usuario se modificó correctamente.</label>
        </div>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-success">
                    <div class="panel-heading text-center">
                        <asp:Label ID="lblPanelModUser" runat="server" Text="Administración de Usuarios" CssClass="TituloPanel"></asp:Label>
                    </div>
                    <div class="panel-body FondoPanel">
                        <br />
                        <div class="form-horizontal has-success col-md-12">
                            <div class="form-group">
                                <div>
                                    <div>
                                        <asp:GridView CssClass="table table-hover table-bordered table-responsive table-success " ID="gv_Usuarios" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" AllowPaging="true" PageSize="5" OnPageIndexChanging="gv_Usuarios_PageIndexChanging" RowStyle-Height="40px">
                                            <HeaderStyle CssClass="thead-dark" />                                            
                                            <PagerTemplate>
                                                <div class="col-md-4 text-left">
                                                    <asp:Label ID="lblmostrarpag" runat="server" Text="Mostrar Pagina" ></asp:Label>
                                                    <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPaging_SelectedIndexChanged" />
                                                     <asp:Label ID="lblde" runat="server" Text="de" ></asp:Label>
                                                    <asp:Label ID="lbltotalpages" runat="server" Text="" ></asp:Label>
                                                </div>
                                                <div class="col-md-4 col-md-offset-4">
                                                    <asp:Label ID="lblMostrar" runat="server" Text="Mostrar" ></asp:Label>
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="margenPaginacion" OnSelectedIndexChanged="ddlPageSize_SelectedPageSizeChanged">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10" ></asp:ListItem>
                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                    </asp:DropDownList>
                                                     <asp:Label ID="lblRegistrosPag" runat="server" Text="Registros por Pagina" ></asp:Label>
                                                </div>
                                            </PagerTemplate>
                                            <Columns>
                                                <asp:BoundField DataField="ID_Usuario" HeaderText="ID" />
                                                <asp:BoundField DataField="NombreUsu" HeaderText="Nombre Usuario" />
                                                <asp:BoundField DataField="Idioma.Nombre" HeaderText="Idioma" />
                                                <asp:BoundField DataField="Perfil.Nombre" HeaderText="Permiso" />
                                                <asp:BoundField DataField="Bloqueo" HeaderText="Estado" />
                                                <asp:BoundField DataField="Empleado" HeaderText="Empleado" />
                                                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha de Alta" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" />
                                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <div>
                                                            <asp:ImageButton ID="btn_Bloquear" runat="server" CommandName="B" ImageUrl="~/Imagenes/padlock-close.png" Height="18px" />
                                                            <asp:ImageButton ID="btn_desbloqueo" runat="server" CommandName="U" ImageUrl="~/Imagenes/padlock-open.png" Height="18px" />
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
                            <div id="usuariot" class="form-group" runat="server">
                                <asp:Label ID="lblusuarioname" runat="server" Text="Nombre de Usuario:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtusuario" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-addon" id="basic-addon8"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtusuario" ErrorMessage="*" EnableClientScript="false" Display="Dynamic" CssClass="textoValidacion"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div id="perfilt" runat="server" class="form-group">
                                <asp:Label ID="lblperfil" runat="server" Text="Perfil:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstperfil" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Permiso" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon10"><span class="	glyphicon glyphicon-list-alt" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                            <div id="idiomat" class="form-group" runat="server">
                                <asp:Label ID="lblidioma" runat="server" Text="Idioma:" CssClass="col-sm-4 control-label labelform"></asp:Label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <asp:DropDownList ID="lstidioma" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="ID_Idioma" DataTextField="Nombre"></asp:DropDownList>
                                        <span class="input-group-addon" id="basic-addon11"><span class="glyphicon glyphicon-flag" aria-hidden="true"></span></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div id="botont" runat="server" class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <asp:Button ClientIDMode="Static" ID="btnModificar" name="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-block btn-warning" />
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
