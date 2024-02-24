<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPasswordPage.aspx.cs" Inherits="EcommerceBW4.ForgotPasswordPage" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" href="./Content/Assets/css/Premium.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
    <title>Password Reset</title>
    <script src="https://kit.fontawesome.com/2b9cdc1c9a.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.4.0/font/bootstrap-icons.min.css">   
    <link rel="stylesheet" href="./Content/Assets/css/Login.css" />
</head> 

<body> 
    <div class="cards__inner">
        <div class="overlay">
            <form id="form2" runat="server">
                <div class="card"> 
                    <div class="con cards__card"> 
                        <div class="card__front">
                            <header class="head-form">
                                <h2>Password Reset</h2> 
                                <p>You can reset your password here.</p> 
                            </header>
                            <br />
                            <div class="field-set d-flex flex-column align-items-center justify-content-center">
                                <!-- Campo per inserire il nome utente -->
                                <div class="d-flex align-items-center justify-content-center">
                                    <br />
                                    <div class="input-group">
                                        <span class="input-items"> 
                                            <i class="fa fa-user"></i> 
                                        </span>
                                        <input class="form-input" type="text" placeholder="Username" id="Username" runat="server" autocomplete="on" />
                                    </div>
                                </div>

                                <!-- Campo per inserire la nuova password -->
                                <div class="d-flex align-items-center justify-content-center">
                                    <br />
                                    <div class="input-group">
                                        <span class="input-items">
                                            <i class="fa fa-key"></i> 
                                        </span>
                                        <input class="form-input" type="password" placeholder="New password" id="NewPass" runat="server" />
                                    </div> 
                                    </div>
                                    <!-- Campo per confermare la nuova password -->
                                    <div class="d-flex align-items-center justify-content-center">
                                        <div class="input-group">
                                            <span class="input-items">
                                                <i class="fa fa-key"></i>
                                             </span>
                                            <input class="form-input" type="password" placeholder="Confirm new password" id="ConfirmPass" runat="server" />
                                        </div>
                                    </div>
                                <p class="password-feedback" id="FeedbackMsg" runat="server"></p>
                                <!-- Bottone per reset password -->
                                <!-- Riintegrare l'attributo onserverclick ResetPassword_Click e runat -->
                                <button class="log-in btn-doblue my-2" runat="server" onserverclick="PassReset_Click">Reset Password</button>

                                <!-- Bottone per tornare al login -->
                                <!-- Reintegrare l'attributo onserverclick BacktoLogin_Click e runat -->
                                <button class="btn submits frgt-pass btn-doblue my-2 text-light" runat="server" onserverclick="BackToLogin_Click" id="BackToLogin" visible="false">Back to Login</button>
                                <button class="btn submits frgt-pass btn-doblue my-2 text-light" runat="server" onserverclick="BackToShop_Click" id="BackToShop" visible="false">Back to Shop</button>

                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <!-- file JS -->
    <script src="Scripts/Login.js"></script>
</body>
</html>
