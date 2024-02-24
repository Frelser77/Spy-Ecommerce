<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="EcommerceBW4.OrderConfirmation" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="container mt-5">
            <div class="card bg-dark">
                <div class="card-header">Ordine Confermato</div>
                <div class="card-body">
                    <h5 class="card-title">Grazie per il tuo acquisto!</h5>
                    <p class="card-text">Agente, la tua missione di acquisto è stata un successo. Ecco i dettagli dell'operazione:</p>
                    <ul>
                        <li>Numero dell'ordine: <%# OrdineID %></li>
                        <li>Data dell'ordine: <%# DataOrdine.ToString("dd MMMM yyyy HH:mm:ss") %></li>
                        <li>Totale dell'ordine: <%# TotaleOrdine.ToString("C") %></li>
                    </ul>
                    <p class="card-text">Un messaggio con tutti i dettagli è stato inviato alla tua casella di posta segreta. Ti aspettiamo per la tua prossima missione di acquisto.</p>
                    <a href="Default.aspx" class="btn btn-primary">Continua a fare acquisti</a>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
