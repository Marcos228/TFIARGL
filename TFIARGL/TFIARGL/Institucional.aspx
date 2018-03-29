<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Institucional.aspx.vb" Inherits="Vista.Institucional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <br />
        <div class="row">
            <div class="panel panel-info">
                <div class="panel-heading text-center">
                    <asp:Label ID="lblPanelLogin" runat="server" Text="Información Sobre Nosotros" CssClass="TituloPanel"></asp:Label>
                </div>
                <div class="panel-body FondoPanel">
                    <div class="col-md-2 media" style="padding-bottom: 10px">
                        <asp:Image ID="LogoMenu" runat="server" ImageUrl="Imagenes/edificio.jpg" CssClass="img-responsive" Height="160px" />
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="lblnosotros1" runat="server" Text="

Somos una empresa formada por los mejores especialistas y profesionales del sector, estamos conducidos por Natalia Diez Ledesma, quien cuenta con más de 10 años de experiencia en la industria.
Nuestros servicios:
    Soluciones Integrales: planificación, coordinación y supervisión de cada detalle antes, durante y al finalizar el evento, realizando la búsqueda, selección y asesoramiento sobre los proveedores más eficientes para cada tipo de evento.
    Organización a su medida: Nos adaptamos a su organización y planificación inicial, enriqueciéndola y ultimando los detalles, desde la búsqueda de algún proveedor de último momento hasta la confirmación de asistencia de los invitados y la permanencia durante el desarrollo del evento.
    Supervisión el día del evento: Coordinamos cada paso durante el día del evento siendo los referentes para usted.
Misión y Principios
Nuestro objetivo principal es lograr que cada evento sea único y transmita realmente lo que se quiere comunicar.
Nos identificamos y trabajamos en base a los siguientes principios:
    Lealtad
    Respeto
    Compromiso
    Excelencia
"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblnosotros2" runat="server" Text="  Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et
                             magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat 
                            massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, 
                            justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend
                             tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. 
                            Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper 
                            ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing 
                            sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae 
                            sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo.
                             Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
