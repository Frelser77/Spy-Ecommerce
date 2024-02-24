<%@ Page Title="Il tuo carrello" Language="C#" AutoEventWireup="true" CodeBehind="Carrello.aspx.cs" Inherits="EcommerceBW4.Carrello" MasterPageFile="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="container mt-3">
            <asp:Literal ID="CarrelloVuotoLiteral" runat="server" />
            <asp:Repeater ID="carrelloRepeater" runat="server" OnItemCommand="CarrelloRepeater_ItemCommand">
                <HeaderTemplate>
                    <div class="row">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="col-12 col-md-6 col-lg-4 col-xl-3 xol-xxl-2 mb-4 d-flex mb-4">
                        <a href='Dettagli.aspx?id=<%# Eval("ProdottoID") %>' class="custom-link">
                            <div class="card h-100 w-100 bg-transparent customCardCarrello">
                                <img src='<%# Eval("ImmagineURL") %>' alt="Product Image" class="card-img-top img-size" />
                                <div class="card-body">
                                    <h5 class="card-title text-truncate"><%# Eval("Nome") %></h5>
                                    <p class="card-text">Quantità: <%# Eval("Quantita") %></p>
                                    <p class="card-text">Prezzo unitario: <%# Eval("Prezzo") %>€</p>
                                    <p class="card-text">Prezzo totale: <%# Eval("Totale") %>€</p>
                                    <div class="d-flex align-items-center justify-content-around gap-2 mx-auto mt-2 mb-1">
                                        <asp:LinkButton ID="BtnRemoveOne" runat="server" CommandArgument='<%# Eval("ProdottoID") %>' CommandName="RemoveOne" CssClass="btn btn-sm btn-customRemoveOne btn-CardsCarrello">
                                            <i class="bi bi-dash"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="BtnAddOne" runat="server" CommandArgument='<%# Eval("ProdottoID") %>' CommandName="AddOne" CssClass="btn btn-sm btn-customAddOne btn-CardsCarrello">
                                            <i class="bi bi-plus"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            <div class="d-flex justify-content-between">
                <div class="d-flex align-items-center">
                    <asp:Button ID="BtnCompleteOrder" runat="server" OnClick="BtnCompleteOrder_Click" Text="Completa ordine" CssClass="btn btn-checkOut" />
                    <p class="mb-0 ms-3">Totale ordine: <asp:Label ID="TotaleCarrello" runat="server" Text="" CssClass=""></asp:Label></p>
                </div>
                <asp:Button ID="BtnClearCart" runat="server" OnClick="BtnClearCart_Click" Text="Svuota carrello" CssClass="btn svuotaCarrello" />
            </div>
        </div>
    </main>
</asp:Content>

