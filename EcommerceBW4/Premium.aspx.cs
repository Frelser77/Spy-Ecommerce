using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceBW4
{
    public partial class Premium : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLogged = Session["UserId"] != null;
            if (!isLogged)
            {
                Response.Redirect("Login.aspx");
            }

            //  le variabili che contengono gli ID dei prodotti
            int ID_Prodotto_Basic = 29;
            int ID_Prodotto_Pro = 30;
            int ID_Prodotto_Ultimate = 31;

            if (!IsPostBack)
            {
                // Assegno ID ai CommandArgument dei LinkButton
                SubscribeBasic.CommandArgument = ID_Prodotto_Basic.ToString();
                SubscribePro.CommandArgument = ID_Prodotto_Pro.ToString();
                SubscribeUltimate.CommandArgument = ID_Prodotto_Ultimate.ToString();
            }
        }


        // Metodo per ricercare il prodotto tramite la barra di ricerca
        protected void Search_Click(object sender, EventArgs e)
        {
            string searchText = searchInput.Value.Trim(); // Ottieni il testo di ricerca dall'input
            Response.Redirect($"Default.aspx?search={searchText}");
        }

        // Metodo per il logout dell'utente
        protected void Logout_Click(object sender, EventArgs e)
        {

            Session.Remove("Username");
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        protected void AddPremiumToCart_Command(object sender, CommandEventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int utenteId = Convert.ToInt32(Session["UserId"]);
            int prodottoId = Convert.ToInt32(e.CommandArgument); // ID dell'abbonamento

            int carrelloId = GetOrCreateTimeCarrello(utenteId);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString))
            {
                conn.Open();

                if (ProdottoAlreadyInCarrello(conn, carrelloId, prodottoId))
                {
                    // Se l'abbonamento è già nel carrello, mostriamo un messaggio popup.
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Hai già selezionato un abbonamento. Per cambiare abbonamento, rimuovi prima quello esistente dal tuo carrello.');", true);
                }
                else
                {
                    // Controlliamo se c'è già un altro abbonamento nel carrello.
                    if (IsAnotherSubscriptionInCart(conn, carrelloId))
                    {
                        // Se c'è già un altro abbonamento, mostriamo un messaggio popup.
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Puoi avere solo un abbonamento alla volta nel tuo carrello. Per cambiare abbonamento, rimuovi prima quello esistente dal tuo carrello.');", true);
                    }
                    else
                    {
                        // Inserisci un nuovo dettaglio carrello per l'abbonamento
                        InsertNewProdottoInCarrello(conn, carrelloId, prodottoId);
                        // Reindirizza l'utente alla pagina del carrello o mostra un messaggio di conferma
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Abbonamento aggiunto al carrello con successo.'); window.location='Carrello.aspx';", true);
                    }
                }
            }
        }

        // Metodo per ottenere l'ID del carrello esistente o crearne uno nuovo.
        private int GetOrCreateTimeCarrello(int utenteId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT CarrelloID FROM Carrello WHERE UtenteID = @utenteId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@utenteId", utenteId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Carrello (UtenteID, DataOra) OUTPUT INSERTED.CarrelloID VALUES (@utenteId, GETDATE())";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@utenteId", utenteId);
                            return (int)insertCmd.ExecuteScalar();
                        }
                    }
                }
            }
        }

        // Metodo per controllare se un prodotto è già nel carrello.
        private bool ProdottoAlreadyInCarrello(SqlConnection conn, int carrelloId, int prodottoId)
        {
            string query = "SELECT COUNT(*) FROM CarrelloDettaglio WHERE CarrelloID = @carrelloId AND ProdottoID = @prodottoId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@carrelloId", carrelloId);
                cmd.Parameters.AddWithValue("@prodottoId", prodottoId);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Metodo per inserire un nuovo prodotto nel carrello.
        private void InsertNewProdottoInCarrello(SqlConnection conn, int carrelloId, int prodottoId)
        {
            // Recupera il prezzo del prodotto da aggiungere al carrello
            string priceQuery = "SELECT Prezzo FROM Prodotti WHERE ProdottoID = @prodottoId";
            SqlCommand priceCmd = new SqlCommand(priceQuery, conn);
            priceCmd.Parameters.AddWithValue("@prodottoId", prodottoId);
            var prezzo = (decimal)priceCmd.ExecuteScalar();

            // Inserisci il nuovo prodotto nel carrello
            string query = "INSERT INTO CarrelloDettaglio (CarrelloID, ProdottoID, Quantita, Prezzo) VALUES (@carrelloId, @prodottoId, 1, @prezzo)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@carrelloId", carrelloId);
                cmd.Parameters.AddWithValue("@prodottoId", prodottoId);
                cmd.Parameters.AddWithValue("@prezzo", prezzo);
                cmd.ExecuteNonQuery();
            }
        }

        private bool IsAnotherSubscriptionInCart(SqlConnection conn, int carrelloId)
        {

            // Esempio di implementazione:
            string checkSubscriptionQuery = @"
            SELECT COUNT(*)
            FROM CarrelloDettaglio cd
            INNER JOIN Prodotti p ON cd.ProdottoID = p.ProdottoID
            WHERE cd.CarrelloID = @CarrelloID AND p.Categoria = 'Abbonamento'";

            using (SqlCommand checkCmd = new SqlCommand(checkSubscriptionQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@CarrelloID", carrelloId);
                int subscriptionCount = (int)checkCmd.ExecuteScalar();
                return subscriptionCount > 0;
            }
        }

    }
}