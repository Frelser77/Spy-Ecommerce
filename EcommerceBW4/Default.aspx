<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EcommerceBW4._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
 <!-- Da qui le cose dei main-->
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-10">
                <main>
                    <!-- Cards -->
                    <div class="row">
                        <asp:Repeater ID="prodottiRepeater" runat="server">
                            <ItemTemplate>
                                <div class="col-12 col-md-6 col-lg-4 col-xl-3 xol-xxl-2 mb-4 d-flex">
                                    <div class="card card-custom flex-fill d-flex flex-column h-100 bg-transparent hover">
                                        <div class="card-img-container text-center m-0 mt-2">
                                            <img src='<%# Eval("ImmagineURL") %>' class="card-img-top card-img-top-default" alt="Immagini del prodotto">
                                        </div>
                                        <div class="card-body card-body-default d-flex flex-column">
                                            <h5 class="flex-fill title"><%# Eval("Nome") %></h5>
                                            <p class="mb-4 price">Prezzo: <i><%#Eval("Prezzo") %>&euro;</i></p>
                                            <div class="mt-auto d-flex justify-content-between card-footer-default  card-footer  gap-2">
                                                <asp:LinkButton ID="ToDetail" runat="server" CommandArgument='<%# Eval("ProdottoID") %>' OnCommand="ToDetail_Command" CssClass="btn btn-light btn-custom"> 
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-lg" viewBox="0 0 16 16">
                                                        <path d="m9.708 6.075-3.024.379-.108.502.595.108c.387.093.464.232.38.619l-.975 4.577c-.255 1.183.14 1.74 1.067 1.74.72 0 1.554-.332 1.933-.789l.116-.549c-.263.232-.65.325-.905.325-.363 0-.494-.255-.402-.704zm.091-2.755a1.32 1.32 0 1 1-2.64 0 1.32 1.32 0 0 1 2.64 0"/>
                                                    </svg>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="AddCart" CommandArgument='<%# Eval("ProdottoID") %>' runat="server" OnCommand="AddCart_OnClickButton" CssClass="btn btn-light btn-custom btn-buy">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bag-plus-fill" viewBox="0 0 16 16">
                                                        <path fill-rule="evenodd" d="M10.5 3.5a2.5 2.5 0 0 0-5 0V4h5zm1 0V4H15v10a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V4h3.5v-.5a3.5 3.5 0 1 1 7 0M8.5 8a.5.5 0 0 0-1 0v1.5H6a.5.5 0 0 0 0 1h1.5V12a.5.5 0 0 0 1 0v-1.5H10a.5.5 0 0 0 0-1H8.5z"/>
                                                    </svg>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </div>
                    <!-- Fine Card -->
                </main>
            </div>
            <!-- Side per pubblicità -->
            <div class="col-md-2 d-flex justify-content-center sideAds">
                <div class="aligh-content-center" style="text-align: center">
                    <h6>Benvenuto <span id="helloUser" runat="server" class="welcome-message"><%# Session["Username"] %></span>,</h6>
                    <p>Dai un'occhiata ai nostri partner!</p>
                    <div>
                        <div id="AdsCarousel" class="carousel slide carousel-style" data-touch="false" data-interval="3000">
                            <div class="carousel-inner">
                                <div class="carousel-item active">
                                    <img src="Content/Assets/images/biggamesquare.png" class="img-fluid d-block w-100" alt="Banner1">
                                </div>
                                <div class="carousel-item">
                                    <img src="Content/Assets/images/securityaddsquare.png" class="img-fluid d-block w-100" alt="Banner2">
                                </div>
                                <div class="carousel-item">
                                    <img src="Content/Assets/images/murderisteryaddsquare.png" class="img-fluid d-block w-100" alt="Banner3">
                                </div>
                                <div class="carousel-item">
                                    <img src="Content/Assets/images/detectiveaddsquare.png" class="img-fluid d-block w-100" alt="Banner4">
                                </div>
                            </div>
                        </div>
                    </div>
                    <a href="Premium.aspx" class="btn btn-light btn-custom mt-3">Diventa Premium</a>
                </div>
            </div>
            <!-- Fine side -->
        </div>
    </div>

</asp:Content>
