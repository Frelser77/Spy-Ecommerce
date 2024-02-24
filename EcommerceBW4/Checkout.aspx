<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="EcommerceBW4.Checkout" MasterPageFile="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container mt-5">
        <h1 class="text-center mb-4">Checkout</h1>

        <div class="row justify-content-center">
            <div class="col-md-6 align-items-center">

                <asp:Image ID="imgAnteprimaCarrello" CssClass="mb-3 rounded" runat="server" ImageUrl="~/path/to/your/image.gif" />
                <asp:Label ID="lblTotaleCarrello" runat="server" CssClass="h3 d-block mb-4" Text="Totale Carrello: " />

                <div class="formCheckout" CssClass="needs-validation">
                    <div class="form-group">
                        <label for="nomeDestinatario" class="text-light">Nome:</label>
                        <asp:TextBox ID="nomeDestinatario" CssClass="form-control" runat="server" ClientIDMode="Static" required="true" />
                    </div>
                    
                    <div class="form-group">
                        <label for="indirizzoDestinatario" class="text-light">Indirizzo:</label>
                        <asp:TextBox ID="indirizzoDestinatario" CssClass="form-control" runat="server" ClientIDMode="Static" required="true" />
                    </div>

                    <div class="form-group">
                        <label for="cittaDestinatario" class="text-light">Città:</label>
                        <asp:TextBox ID="cittaDestinatario" CssClass="form-control" runat="server" ClientIDMode="Static" required="true" />
                    </div>

                    <div class="form-group">
                        <label for="capDestinatario" class="text-light">CAP:</label>
                        <asp:TextBox ID="capDestinatario" CssClass="form-control" runat="server" ClientIDMode="Static" required="true" />
                    </div>

                    <div class="form-group">
                        <label for="paeseDestinatario" class="text-light">Nazione:</label>
                        <asp:TextBox ID="paeseDestinatario" CssClass="form-control" runat="server" ClientIDMode="Static" required="true" />
                    </div>

                    <asp:Button ID="btnCompletaOrdine" runat="server" CssClass="btn btn-primary btn-block" OnClick="CompletaOrdine_ServerClick" Text="Completa Ordine" />
                </div>

            </div>
        </div>
    </main>
    <div id="myModal" class="modal" runat="server" visible="false">
        <div class="modal-content">
            <asp:Label ID="ModalContent" runat="server" CssClass="text-white" />
            <asp:Button ID="CloseButton" runat="server" Text="Chiudi" OnClick="CloseButton_Click" CssClass="bottoneModale" />
        </div>
    </div>
</asp:Content>




