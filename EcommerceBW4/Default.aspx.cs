using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EcommerceBW4
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLogged = Session["UserId"] != null;
            if (isLogged)
            {
                if (!IsPostBack)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM Prodotti", conn);
                    SqlDataReader reader = command.ExecuteReader();

                    prodottiRepeater.DataSource = reader;
                    prodottiRepeater.DataBind();
                    if (Request.QueryString["search"] != null)
                    {
                        string searchText = Request.QueryString["search"];
                        ShowSearchedProducts(searchText); // Visualizza i prodotti corrispondenti alla ricerca
                    }


                    // Ottiene il nome utente da sessione + popola dinamicamente
                    string username = Session["Username"].ToString();
                    helloUser.InnerText = username;

                    reader.Close();
                    conn.Close();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        /*
          * Summary: Gestisce il clic sul pulsante "Dettagli"
          * Parameters: oggetto sender, argomenti del comando
          * Return: null
        */
        protected void ToDetail_Command(object sender, CommandEventArgs e)
        {
            string productId = e.CommandArgument.ToString();
            Response.Redirect($"Dettagli.aspx?id={productId}");
        }

        /*
          * Summary: Gestisce il clic sul pulsante "Aggiungi al carrello"
          * Parameters: oggetto sender, argomenti del comando
          * Return: null
        */
        protected void AddToCart_Command(object sender, CommandEventArgs e)
        {
            string productId = e.CommandArgument.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Prodotti WHERE Id = @id", conn);
            command.Parameters.AddWithValue("@id", productId);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string nome = reader["Nome"].ToString();
                string prezzo = reader["Prezzo"].ToString();
                string quantita = "1";
                string username = reader["Username"].ToString();
                string userId = Session["UserId"].ToString();

                reader.Close();

                command = new SqlCommand("INSERT INTO Carrello (Nome, Prezzo, Quantita, Username) VALUES (@nome, @prezzo, @quantita, @username)", conn);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@prezzo", prezzo);
                command.Parameters.AddWithValue("@quantita", quantita);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }

            conn.Close();
        }


        /*
          * Summary: Gestisce il clic sul pulsante "Aggiungi al carrello" per aggiungere il prodotto selezionato al carrello 
          * Parameters: oggetto sender, argomenti dell'evento
          * Return: null
        */
        protected void AddCart_OnClickButton(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int prodottoId = Convert.ToInt32(btn.CommandArgument);
            int utenteId = Convert.ToInt32(Session["UserId"]);

            // Ottengo l'ID del carrello esistente o ne creo uno nuovo.
            int carrelloId = GetOrCreateTimeCarrello(utenteId);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString))
            {
                conn.Open();

                // Controllo se il prodotto è già nel carrello.
                if (ProdottoAlreadyInCarrello(conn, carrelloId, prodottoId))
                {
                    // Aggiorno la quantità.
                    UpdateQuantitaProdottoInCarrello(conn, carrelloId, prodottoId);
                }
                else
                {
                    // Prima di inserire, controllo se c'è già un abbonamento nel carrello.
                    if (IsAnotherSubscriptionInCart(conn, carrelloId))
                    {
                        // Se c'è già un abbonamento, mostriamo un messaggio popup.
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Puoi avere solo un abbonamento alla volta nel tuo carrello. Per cambiare abbonamento, rimuovi prima quello esistente dal tuo carrello.');", true);
                    }
                    else
                    {
                        // Inserisco un nuovo dettaglio carrello.
                        InsertNewProdottoInCarrello(conn, carrelloId, prodottoId);
                        // Reindirizza l'utente alla pagina del carrello o mostra un messaggio di conferma
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Prodotto aggiunto al carrello con successo.'); window.location='Carrello.aspx';", true);
                    }
                }
            }
        }


        /*
        * Summary: Ottiene l'ID del carrello esistente o crea uno nuovo.
        * Parameters: id dell'utente
        * Return: ID del carrello
        */
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


        /*
          * Summary: Controlla se un prodotto è già nel carrello.
          * Parameters: connessione al database, ID del carrello, ID del prodotto
          * Return: true se il prodotto è già nel carrello, altrimenti false
        */
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

        /*
          * Summary: Aggiorna la quantità di un prodotto nel carrello.
          * Parameters: connessione al database, ID del carrello, ID del prodotto
          * Return: null
        */
        private void UpdateQuantitaProdottoInCarrello(SqlConnection conn, int carrelloId, int prodottoId)
        {
            string query = "UPDATE CarrelloDettaglio SET Quantita = Quantita + 1 WHERE CarrelloID = @carrelloId AND ProdottoID = @prodottoId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@carrelloId", carrelloId);
                cmd.Parameters.AddWithValue("@prodottoId", prodottoId);
                cmd.ExecuteNonQuery();
            }
        }

        /*
        * Summary: Metodo per inserire un nuovo prodotto nel carrello.
        * Parameters: connessione al database, ID del carrello, ID del prodotto
        * Return: void
        */
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

        /*
        * Summary: Metodo per ricercare i prodotti in base a una stringa di ricerca e visualizzarli nel repeater.
        * Parameters: stringa di ricerca
        * Return: void
        */
        protected void ShowSearchedProducts(string searchText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Prodotti WHERE Nome LIKE @searchText", conn);
                command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                SqlDataReader reader = command.ExecuteReader();
                prodottiRepeater.DataSource = reader;
                prodottiRepeater.DataBind();
                reader.Close();
            }
        }

        /*
        *Summary:
        *Parameters:
        *Return:
         */
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
