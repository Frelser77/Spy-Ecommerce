<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EcommerceBW4.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="./Content/Assets/css/Login.css" />
    <link rel="stylesheet" href="./Content/Assets/css/Premium.css" />
    <link href="~/Content/Assets/images/footer-logo-page.png" rel="shortcut icon" type="image/png" />

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">


    <title>Log-in</title>
    <script src="https://kit.fontawesome.com/2b9cdc1c9a.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.4.0/font/bootstrap-icons.min.css">
    <!-- Bootstrap CSS -->

</head>
<body>

    <div class="cards__inner">
        <div class="overlay">
            <form id="form1" runat="server">
                <div class="card">
                    <div class="con flip cards__card">

                        <button id="btn-1" type="button" class="btn-flip btn-pstn px-2 py-1 rounded-pill" onclick="flipCard()">
                            <i class="bi bi-arrow-left"></i>
                        </button>
                        <div class="card__front">
                            <header class="head-form">
                                <h2>Log-In</h2>
                                <p>Log-in here using your username and password</p>
                            </header>
                            <br />
                            <div class="field-set d-flex flex-column align-items-center justify-content-center">

                                <div class="d-flex align-items-center justify-content-center">
                                    <span class="input-item">
                                        <i class="fa fa-user-circle"></i>
                                    </span>
                                    <input class="form-input" type="text" placeholder="@UserName" id="txtUsername" required runat="server" />
                                </div>
                                <div class="d-flex align-items-center justify-content-center">
                                    <br />
                                    <span class="input-item">
                                        <i class="fa fa-key"></i>
                                    </span>
                                    <div class="input-group">
                                        <input class="form-input" type="password" placeholder="Password" id="txtPassword" required autocomplete="on" runat="server" />
                                        <i class="fa fa-eye mx-0" aria-hidden="true" id="eye"></i>


                                    </div>

                                </div>

                                <asp:Button ID="LoginButton" class="log-in btn-doblue my-2" runat="server" Text="Log In" OnClick="Login_Click" OnClientClick="document.getElementById('loader').style.display='block';" />


                                <button class="btn submits frgt-pass btn-doblue my-2 text-light" runat="server" onserverclick="ForgotPassword_Click">Forgot Password</button>

                                <asp:Label Text="text" ID="lblError" runat="server" Visible="False" CssClass="error" />

                            </div>
                        </div>
                        <div class="card__back" style="display: none;">
                            <header class="head-form">
                                <h2>Sign Up</h2>
                                <p>Insert username and password and click Sign Up</p>
                                <p class="fs-4 fw-semibold">Age is required!</p>
                            </header>
                            <br />

                            <button id="btn-2" type="button" class="btn-flip btn-pstn px-2 py-1 rounded-pill" onclick="flipCard()">
                                <i class="bi bi-arrow-right"></i>
                            </button>
                            <div class="d-flex flex-column align-items-center justify-content-center mb-2">

                                <div class="d-flex align-items-center justify-content-center">
                                    <span class="input-item sing-in">
                                        <i class="fa fa-user-circle"></i>
                                    </span>
                                    <asp:TextBox ID="txtUsernameSignUp" CssClass="form-input" TextMode="SingleLine" placeholder="Username" runat="server" />
                                </div>

                                <div class="d-flex align-items-center justify-content-center">

                                    <span class="input-item">
                                        <i class="fa fa-key"></i>
                                    </span>
                                    <asp:TextBox ID="txtPasswordSignUp" CssClass="form-input" TextMode="Password" placeholder="Password" runat="server" />
                                </div>

                                <div class="d-flex align-items-center justify-content-center">
                                    <span class="input-item">
                                        <i class="fa fa-birthday-cake"></i>
                                    </span>
                                    <asp:TextBox ID="txtEta" CssClass="form-input" TextMode="SingleLine" placeholder="Età" runat="server" />

                                </div>

                                <button class="btn submits sign-up btn-doblue text-white" runat="server" onserverclick="SignUp_Click">
                                    Sign Up
                        <i class="fa fa-user-plus" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col">

                <!-- ###################### LOADING ###################### -->
                <div id="loader">
                    <div class="hand">
                        <span class="finger thumb"></span>
                        <span class="finger"></span>
                        <span class="finger"></span>
                        <span class="finger"></span>
                        <span class="finger"></span>
                        <span class="palm"></span>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <script src="Scripts\CardRotation.js"></script>
    <script src="Scripts\Login.js"></script>
</body>
</html>
