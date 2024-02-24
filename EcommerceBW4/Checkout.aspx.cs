using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace EcommerceBW4
{
    public partial class Checkout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verifica se l'utente è loggato
                if (Session["UserId"] != null)
                {
                    // Recupera l'ID del carrello dalla query string
                    if (Request.QueryString["carrelloId"] != null)
                    {
                        int carrelloId = Convert.ToInt32(Request.QueryString["carrelloId"]);
                        BindDettagliCarrello(carrelloId);
                    }
                    else
                    {
                        // Gestire il caso in cui l'ID del carrello non sia presente
                        Response.Redirect("Carrello.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        // metodo per recuperare i dettagli del carrello
        private void BindDettagliCarrello(int carrelloId)
        {
            decimal totaleCarrello = 0m;

            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT SUM(cd.Quantita * cd.Prezzo) AS TotaleCarrello
                    FROM CarrelloDettaglio cd
                    WHERE cd.CarrelloID = @CarrelloID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarrelloID", carrelloId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Calcola il totale del carrello
                            totaleCarrello = reader.IsDBNull(reader.GetOrdinal("TotaleCarrello")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotaleCarrello"));
                        }
                    }
                }
            }


            lblTotaleCarrello.Text = "Totale Carrello: " + totaleCarrello.ToString("C");


            imgAnteprimaCarrello.ImageUrl = "https://www.gifanimate.com/data/media/353/spia-immagine-animata-0019.gif";
        }

        // Metodo per completare l'ordine e inserire i dati nella tabella Ordini
        protected void CompletaOrdine_ServerClick(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int utenteId = Convert.ToInt32(Session["UserId"]);
            int carrelloId = GetCarrelloId(utenteId);

            if (CarrelloVuoto(carrelloId))
            {
                // Usa lo script per mostrare un messaggio di alert
                ModalContent.Text = "Il tuo carrello è vuoto! Aggiungi dei prodotti prima di procedere al checkout.";
                myModal.Visible = true;
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("CompletaOrdine", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Imposta i parametri per la Stored Procedure
                    cmd.Parameters.AddWithValue("@UtenteID", utenteId);
                    cmd.Parameters.AddWithValue("@CarrelloID", carrelloId);
                    cmd.Parameters.AddWithValue("@NomeDestinatario", nomeDestinatario.Text);
                    cmd.Parameters.AddWithValue("@IndirizzoDestinatario", indirizzoDestinatario.Text);
                    cmd.Parameters.AddWithValue("@CittaDestinatario", cittaDestinatario.Text);
                    cmd.Parameters.AddWithValue("@CAPDestinatario", capDestinatario.Text);
                    cmd.Parameters.AddWithValue("@PaeseDestinatario", paeseDestinatario.Text);

                    // Parametro di output per il totale dell'ordine
                    SqlParameter totaleOrdineParam = new SqlParameter("@TotaleOrdine", SqlDbType.Decimal)
                    {
                        Precision = 10,
                        Scale = 2,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(totaleOrdineParam);

                    // Aggiunta del parametro di output per l'ID dell'ordine
                    SqlParameter ordineIdParam = new SqlParameter("@OrdineID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(ordineIdParam);

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    // Recupero dell'ID dell'ordine e del totale dell'ordine dai parametri di output
                    decimal totaleOrdine = (decimal)totaleOrdineParam.Value;
                    int ordineId = (int)ordineIdParam.Value; // Recupera l'ID dell'ordine dal parametro di output

                    // Salvataggio del totale dell'ordine in una sessione o passarlo alla pagina di conferma
                    Session["TotaleOrdine"] = totaleOrdine;

                    // Reindirizzamento alla pagina di conferma con l'ID dell'ordine e il totale dell'ordine
                    Response.Redirect($"OrderConfirmation.aspx?OrdineID={ordineId}&TotaleOrdine={totaleOrdine}");
                }
                catch (Exception ex)
                {
                    // Mostra un messaggio di errore all'utente
                    ClientScript.RegisterStartupScript(GetType(), "alert", $"alert('Errore durante il completamento dell'ordine: {ex.Message}');", true);
                }
            }
        }




        private bool CarrelloVuoto(int carrelloId)
        {
            bool vuoto = true;
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CarrelloDettaglio WHERE CarrelloID = @CarrelloID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarrelloID", carrelloId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    vuoto = count == 0;
                }
            }
            return vuoto;
        }

        // Metodo per svuotare il carrello dopo aver completato l'ordine
        /*
        private void SvuotaCarrello(int carrelloId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SvuotaCarrello", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UtenteID", Convert.ToInt32(Session["UserId"]));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        */

        // Metodo recupera l'ID del carrello dell'utente
        private int GetCarrelloId(int utenteId)
        {
            // Implementazione del metodo per recuperare l'ID del carrello dell'utente
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            int carrelloId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CarrelloID FROM Carrello WHERE UtenteID = @UtenteID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UtenteID", utenteId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            carrelloId = Convert.ToInt32(reader["CarrelloID"]);
                        }
                    }
                }
            }

            return carrelloId;
        }

        // Metodo per calcolare il totale del carrello in base all'ID del carrello

        private decimal CalcolaTotaleCarrello(int carrelloId)
        {
            decimal totaleCarrello = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT SUM(Quantita * Prezzo) AS TotaleCarrello
                    FROM CarrelloDettaglio
                    WHERE CarrelloID = @CarrelloID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarrelloID", carrelloId);
                    conn.Open();
                    totaleCarrello = (decimal)cmd.ExecuteScalar();
                }
            }
            return totaleCarrello;
        }
        protected void CloseButton_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }
    }
}