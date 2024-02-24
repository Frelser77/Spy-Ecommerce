<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Premium.aspx.cs" Inherits="EcommerceBW4.Premium" %>

<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Premium</title>
    <!-- CSS qui -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.4.0/font/bootstrap-icons.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <link rel="stylesheet" href="Content/Site.css" />
    <link rel="stylesheet" href="Content/Assets/css/Default.css" />
    <link rel="stylesheet" href="Content/Assets/css/Premium.css">
    <link href="/Content/Assets/images/logo-footer-page.png" rel="shortcut icon" type="image/png" height="32px" widtht="32px" />

</head>
<body>
    <form runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark" style="background-image: linear-gradient(-220deg, #5cdb95 0%, #000000 60%);">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/">
                    <img src="Content/Assets/images/LogoEE.png" style="height: 5vmin" alt="EELogo" />
                </a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportContent" aria-controls="navbarSupportContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportContent">
                    <ul class="navbar-nav me-auto mb-2 mb-md-0">
                        <li class="nav-item"><a class="nav-link" runat="server" href="~/">Home</a></li>
                        <li class="nav-item"><a class="nav-link" runat="server" href="~/Carrello">Carrello</a></li>
                    </ul>
                </div>
                <div class="d-flex align-items-baseline align-items-lg-center gapp">
                    <input id="searchInput" runat="server" class="form-control m-sm-2 px-2 py-1" type="search" placeholder="Search" aria-label="Search">

                    <asp:LinkButton ID="searchButton" runat="server" OnClick="Search_Click" CssClass="btn btn-custom btn-light px-2 py-1 srch-bg">
                   <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-search" viewBox="0 0 16 16">
                       <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0" />
                   </svg>
                    </asp:LinkButton>

                    <div class="dropdown">
                        <button class="btn btn-custom btn-light bg-drop dropdown-toggle px-2 py-1 " type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="bi bi-person-circle"></i>
                            <!-- Icona dell'utente -->
                        </button>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                            <a class="dropdown-item" href="Profilo.aspx">Profilo</a>
                            <a class="dropdown-item" href="ForgotPasswordPage.aspx">Reimposta password</a>
                            <a class="dropdown-item" runat="server" id="adminLink" href="~/AdminPage">Amministrazione</a>
                            <a class="dropdown-item p-0 m-0">
                                <asp:LinkButton ID="logoutLinkButton" runat="server" OnClick="Logout_Click" CssClass="dropdown-item">
                               Logout
                                </asp:LinkButton></a>
                        </div>
                    </div>
                </div>
            </div>
        </nav>

        <div class="d-flex flex-column flex-lg-row">
            <!-- card -->
            <main class="main flow">
                <h1 class="main__heading text--stroke">Pricing</h1>
                <div class="main__cards cards">
                    <div class="cards__inner">
                        <div class="card">
                            <h2 class="card__heading ps-3 py-3">Basic</h2>
                            <p class="card__price ps-3">&euro;99.99</p>
                            <ul role="list" class="card__bullets flow">
                                <li>Piano base per spie basilari</li>
                                <li>Supporto Email</li>
                            </ul>
                         <asp:LinkButton ID="SubscribeBasic" runat="server" CommandName="AddPremiumToCart" CommandArgument="29" CssClass="card__cta cta">Abbonati</asp:LinkButton>
                        </div>

                        <div class="cards__card card ">
                            <h2 class="card__heading ps-3 py-3">Pro</h2>
                            <p class="card__price ps-3">&euro;399.99</p>
                            <ul role="list" class="card__bullets flow">
                                <li>Piano avanzato per veri 007.</li>
                                <li>Prioritá nel supporto email</li>
                                <li>Accesso esclusivo alle sessioni live di Q&A </li>
                            </ul>
                        <asp:LinkButton ID="SubscribePro" runat="server" CommandName="AddPremiumToCart" CommandArgument="30" CssClass="card__cta cta">Passa a Pro</asp:LinkButton>
                        </div>

                        <div class="cards__card card">
                            <h2 class="card__heading ps-3 py-3">Ultimate</h2>
                            <p class="card__price ps-3">&euro;999.99</p>
                            <ul role="list" class="card__bullets flow">
                                <li>Solo per veri Jhon Wick</li>
                                <li>24/7 Supporto Prioritario</li>
                                <li>1-on-1 virtual coaching sessione mensile</li>
                                <li>Contenuti esclusivi e accesso anticipato sulle nuove uscite</li>
                            </ul>
                       <asp:LinkButton ID="SubscribeUltimate" runat="server" CommandName="AddPremiumToCart" CommandArgument="31" CssClass="card__cta cta">Go Ultimate</asp:LinkButton>
                        </div>
                    </div>

                    <div class="overlay cards__inner"></div>
                </div>
            </main>
        </div>
    </form>

    <!-- Footer -->
    <footer>
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-md-8">
                    <img src="Content/Assets/images/logo-footer.png" style="height: 4vmin" alt="EELogo footer" />

                </div>
                <div class="col-lg-8 col-md-4">
                    <p>&copy; <%: DateTime.Now.Year %> - Applicazione ASP.NET <span class="welcome-message">by Team Ⅵ</span></p>
                    <ul class="list-unstyled">
                        <li><a href="https://github.com/Frelser77" target="_blank">Salvatore Alessandro D'Amico</a></li>
                        <li><a href="https://github.com/Aleguid97" target="_blank">Alessio Guida</a></li>
                        <li><a href="https://github.com/Noirspider" target="_blank">Gianluca Grillo</a></li>
                        <li><a href="https://github.com/Aucons94" target="_blank">Alessandro Aucone</a></li>
                        <li><a href="https://github.com/acelilli" target="_blank">Alice Alessandrini</a></li>         
                    </ul>
                </div>
            </div>
        </div>
    </footer>


    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="Scripts/Premium.js"></script>
</body>
</html>
