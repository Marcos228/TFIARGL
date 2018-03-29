<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Default.aspx.vb" Inherits="Vista.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div id="myCarousel" class="carousel slide">
                <!-- Indicators -->
                <ol class="carousel-indicators">
                    <li class="item1 active"></li>
                    <li class="item2"></li>
                    <li class="item3"></li>
                </ol>

                <!-- Wrapper for slides -->
                <div class="carousel-inner" role="listbox">
                    <div class="item active">
                        <img src="Imagenes/estadio.jpg" alt="Primera Slide" />
                    </div>

                    <div class="item">
                        <img src="Imagenes/recital.jpg" alt="Segunda Slide"/>
                    </div>

                    <div class="item">
                        <img src="Imagenes/rural.jpg" alt="Tercera Slide" />
                    </div>
                </div>

                <!-- Left and right controls -->
                <a class="left carousel-control" href="#myCarousel" role="button">
                    <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="right carousel-control" href="#myCarousel" role="button">
                    <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>
        </div>

        <script>
            $(document).ready(function () {
                // Activate Carousel
                $("#myCarousel").carousel();

                // Enable Carousel Indicators
                $(".item1").click(function () {
                    $("#myCarousel").carousel(0);
                });
                $(".item2").click(function () {
                    $("#myCarousel").carousel(1);
                });
                $(".item3").click(function () {
                    $("#myCarousel").carousel(2);
                });

                // Enable Carousel Controls
                $(".left").click(function () {
                    $("#myCarousel").carousel("prev");
                });
                $(".right").click(function () {
                    $("#myCarousel").carousel("next");
                });
            });
        </script>
    </div>
</asp:Content>
