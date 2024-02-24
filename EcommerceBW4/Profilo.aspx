<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profilo.aspx.cs" Inherits="EcommerceBW4.Profilo" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <h1 class="text--stroke-profile d-inline-block">Profilo</h1>
            <asp:Repeater ID="RepeaterOrders" runat="server" OnItemCommand="RepeaterOrders_ItemCommand">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-lg-4">

                    <div class="card bg-transparent border mb-3 profile-card">
                        <div class="card-header d-flex align-items-center justify-content-between border-0 bg-transparent">
                           <span class="text-black fw-semibold fs-5"> Ordine #<%# Eval("OrdineID") %>  </span> 
                            <asp:LinkButton ID="LinkButtonOrderDetails" runat="server" CommandName="ShowDetails" CssClass="nav-link d-inline" CommandArgument='<%# Eval("OrdineID") %>'>Dettagli</asp:LinkButton>
                        </div>
                        <div class="card-body bg-transparent">
                            <p class="text-black fs-6 fw-semibold">Data Ordine: <%# Eval("DataOrdine", "{0:d}") %></p>
                            <p class="text-black fs-6 fw-semibold">Totale Ordine: €<%# Eval("TotaleOrdine") %></p>
                            <p class="text-black fs-6 fw-semibold">Stato: <%# Eval("StatoOrdine") %></p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <!-- MODALE -->
  <div id="orderDetailsModal" class="modal" runat="server" visible="false" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                   <h5 class="modal-title fs-3" id="modalTitle" runat="server">Dettagli Ordine</h5>
                </div>
                <div class="modal-body" runat="server" id="modalBody">
                    <asp:Repeater ID="RepeaterOrderDetails" runat="server">
                        <ItemTemplate>
                            <p><%# Eval("NomeProdotto") %> - x: <%# Eval("Quantita") %> - Prezzo: €<%# Eval("Prezzo") %></p>
                        </ItemTemplate>
                    </asp:Repeater>
                   <asp:Button ID="BtnCloseModal" runat="server" CssClass="btn btn-secondary" OnClick="BtnCloseModal_Click" Text="&times;" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
