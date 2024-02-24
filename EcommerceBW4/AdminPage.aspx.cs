using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace EcommerceBW4
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Procedere solo se l'utente è loggato.
            bool isAdmin = false;
            int userId = Convert.ToInt32(Session["UserId"]);
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IsAdmin FROM Utenti WHERE UtenteID = @UtenteID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UtenteID", userId);
                    conn.Open();
                    isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            // Se l'utente è amministratore e non è un postback, effettuare il binding dei prodotti.
            if (isAdmin && !IsPostBack)
            {
                BindProdottiDropDown();
                PopolaDropDownList();
            }
            else if (!isAdmin)
            {
                Response.Redirect("Login.aspx");
            }
        }

        private void BindProdottiDropDown()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProdottoID, Nome FROM Prodotti";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DropDownProdotto.DataSource = reader;
                            DropDownProdotto.DataTextField = "Nome";
                            DropDownProdotto.DataValueField = "ProdottoID";
                            DropDownProdotto.DataBind();
                            DropDownProdotto.Items.Insert(0, new ListItem("Scegli il tuo SpyGadget:", ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModalContent.Text = $"Si è verificato un errore: {ex.Message}";
                myModal.Visible = true;
            }
        }


        protected void DropDownProdotto_SelectedIndexChanged(object sender, EventArgs e)

        {
            string selectedValue = DropDownProdotto.SelectedValue;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"SELECT p.Nome, p.Descrizione, p.Prezzo, p.ImmagineURL, d.DescrizioneEstesa, d.QuantitaDisponibile
                                                FROM Prodotti p
                                                INNER JOIN DettagliProdotto d ON p.ProdottoID = d.ProdottoID
                                                WHERE p.ProdottoID = @ProdottoID";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    ImgCarrello.ImageUrl = reader["ImmagineURL"].ToString();
                                    LblNome.Text = reader["Nome"].ToString();
                                    LblDescrizione.Text = reader["Descrizione"].ToString();
                                    LblDescrizioneEstesa.Text = reader["DescrizioneEstesa"].ToString();
                                    LblQuantitaDisponibile.Text = reader["QuantitaDisponibile"].ToString();
                                    LblPrezzo.Text = string.Format("Prezzo: {0:C}", reader["Prezzo"]);
                                    Card.Visible = true;
                                }
                                else
                                {
                                    ModalContent.Text = $"Si è verificato un errore";
                                    myModal.Visible = true;
                                    Card.Visible = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModalContent.Text = $"Si è verificato un errore: {ex.Message}";
                    myModal.Visible = true;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Errore3");
                Card.Visible = false;
            }
        }
        protected void InsertItem(object sender, EventArgs e)
        {
            string Nome = TextBoxNome.Text;
            string Prezzo = TextBoxPrezzo.Text;
            string ImmagineURL = string.Empty;
            string Descrizione = TextBoxDescrizione.Text;
            string DescrizioneEstesa = TextBoxDescrizioneEstesa.Text;
            int QuantitaDisponibile = Convert.ToInt32(TextBoxQuantita.Text);

            // Controlla se il FileUpload ha un file e che sia un'immagine
            if (FileUploadImmagine.HasFile)
            {
                // Elenco delle estensioni di file immagine accettate
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                string fileExtension = Path.GetExtension(FileUploadImmagine.FileName).ToLower();

                if (allowedExtensions.Contains(fileExtension))
                {
                    try
                    {
                        // Costruisci il percorso dove l'immagine sarà salvata
                        string filename = Path.GetFileName(FileUploadImmagine.FileName);
                        string savePath = Server.MapPath("/Content/Assets/images/prodottiUp/") + filename;

                        // Salva l'immagine nel percorso specificato
                        FileUploadImmagine.SaveAs(savePath);

                        // Imposta l'URL dell'immagine da salvare nel database
                        ImmagineURL = "/Content/Assets/images/prodottiUp/" + filename;
                    }
                    catch (Exception ex)
                    {
                        ModalContent.Text = $"Si è verificato un errore: {ex.Message}";
                        myModal.Visible = true;
                        return;
                    }
                }
                else
                {
                    // Mostra un messaggio di errore se il file non è un'immagine
                    ModalContent.Text = $"Il file selezionato non è un'immagine valida.";
                    myModal.Visible = true;
                    return;
                }
            }

            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Inserisci i dati nella tabella Prodotti e ottieni l'ID
                    string queryProdotti = "INSERT INTO Prodotti (Nome, Descrizione, Prezzo, ImmagineURL) VALUES (@Nome, @Descrizione, @Prezzo, @ImmagineURL); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdProdotti = new SqlCommand(queryProdotti, conn, transaction);
                    cmdProdotti.Parameters.AddWithValue("@Nome", Nome);
                    cmdProdotti.Parameters.AddWithValue("@Descrizione", Descrizione);
                    cmdProdotti.Parameters.AddWithValue("@Prezzo", Prezzo);
                    cmdProdotti.Parameters.AddWithValue("@ImmagineURL", ImmagineURL);
                    int prodottoId = Convert.ToInt32(cmdProdotti.ExecuteScalar());

                    // Inserisci i dettagli nella tabella DettagliProdotto
                    string queryDettagliProdotto = "INSERT INTO DettagliProdotto (ProdottoID, DescrizioneEstesa, QuantitaDisponibile) VALUES (@ProdottoID, @DescrizioneEstesa, @QuantitaDisponibile);";
                    SqlCommand cmdDettagliProdotto = new SqlCommand(queryDettagliProdotto, conn, transaction);
                    cmdDettagliProdotto.Parameters.AddWithValue("@ProdottoID", prodottoId);
                    cmdDettagliProdotto.Parameters.AddWithValue("@DescrizioneEstesa", DescrizioneEstesa);
                    cmdDettagliProdotto.Parameters.AddWithValue("@QuantitaDisponibile", QuantitaDisponibile);
                    cmdDettagliProdotto.ExecuteNonQuery();

                    transaction.Commit(); // Esegui il commit della transazione se tutto va a buon fine
                    ModalContent.Text = $"Prodotto Inserito con Successo";
                    myModal.Visible = true;
                    // Aggiorna il DropDownList per mostrare il nuovo prodotto
                    BindProdottiDropDown();
                }
                catch (Exception ex)
                {
                    // Se si verifica un errore, annulla la transazione
                    transaction.Rollback();
                    ModalContent.Text = $"Si è verificato un errore durante l'aggiunta del prodotto: {ex.Message}";
                    myModal.Visible = true;
                }
            }
        }

        protected void DeleteItem(object sender, EventArgs e)
        {
            string selectedValue = DropDownProdotto.SelectedValue;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {

                        string deleteDetailsSql = "DELETE FROM DettagliProdotto WHERE ProdottoID = @ProdottoID";
                        SqlCommand deleteDetailsCommand = new SqlCommand(deleteDetailsSql, connection, transaction);
                        deleteDetailsCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                        deleteDetailsCommand.ExecuteNonQuery();


                        string deleteProductSql = "DELETE FROM Prodotti WHERE ProdottoID = @ProdottoID";
                        SqlCommand deleteProductCommand = new SqlCommand(deleteProductSql, connection, transaction);
                        deleteProductCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                        int rowsAffected = deleteProductCommand.ExecuteNonQuery();

                        // Completa la transazione
                        transaction.Commit();

                        ModalContent.Text = $"Prodotto eliminato";
                        myModal.Visible = true;
                        BindProdottiDropDown();

                        if (rowsAffected > 0)
                        {
                            Card.Visible = false;
                            DropDownProdotto.SelectedValue = "";
                        }
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();

                        Console.WriteLine($"Error: {ex.Message}");
                        string script = "alert('Errore durante l'eliminazione del prodotto.');";
                        ClientScript.RegisterStartupScript(GetType(), "alert", script, true);
                    }
                }
            }
        }


        protected void ModificaItem(object sender, EventArgs e)
        {
            string selectedValue = DropDownProdotto.SelectedValue;
            bool modificato = false;

            if (!string.IsNullOrEmpty(selectedValue))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        if (!string.IsNullOrEmpty(TextBoxNome.Text))
                        {
                            string updateNomeSql = "UPDATE Prodotti SET Nome = @Nome WHERE ProdottoID = @ProdottoID";
                            SqlCommand updateNomeCommand = new SqlCommand(updateNomeSql, connection);
                            updateNomeCommand.Parameters.AddWithValue("@Nome", TextBoxNome.Text);
                            updateNomeCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updateNomeCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        if (!string.IsNullOrEmpty(TextBoxDescrizione.Text))
                        {
                            string updateDescrizioneSql = "UPDATE Prodotti SET Descrizione = @Descrizione WHERE ProdottoID = @ProdottoID";
                            SqlCommand updateDescrizioneCommand = new SqlCommand(updateDescrizioneSql, connection);
                            updateDescrizioneCommand.Parameters.AddWithValue("@Descrizione", TextBoxDescrizione.Text);
                            updateDescrizioneCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updateDescrizioneCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        if (!string.IsNullOrEmpty(TextBoxDescrizioneEstesa.Text))
                        {
                            string updatePrezzoSql = "UPDATE DettagliProdotto SET DescrizioneEstesa = @DescrizioneEstesa WHERE ProdottoID = @ProdottoID";
                            SqlCommand updatePrezzoCommand = new SqlCommand(updatePrezzoSql, connection);
                            updatePrezzoCommand.Parameters.AddWithValue("@DescrizioneEstesa", TextBoxDescrizioneEstesa.Text);
                            updatePrezzoCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updatePrezzoCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        if (!string.IsNullOrEmpty(TextBoxQuantita.Text))
                        {
                            string updatePrezzoSql = "UPDATE DettagliProdotto SET QuantitaDisponibile = @Quantita WHERE ProdottoID = @ProdottoID";
                            SqlCommand updatePrezzoCommand = new SqlCommand(updatePrezzoSql, connection);
                            updatePrezzoCommand.Parameters.AddWithValue("@Quantita", TextBoxQuantita.Text);
                            updatePrezzoCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updatePrezzoCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        if (!string.IsNullOrEmpty(TextBoxPrezzo.Text))
                        {
                            string updatePrezzoSql = "UPDATE Prodotti SET Prezzo = @Prezzo WHERE ProdottoID = @ProdottoID";
                            SqlCommand updatePrezzoCommand = new SqlCommand(updatePrezzoSql, connection);
                            updatePrezzoCommand.Parameters.AddWithValue("@Prezzo", TextBoxPrezzo.Text);
                            updatePrezzoCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updatePrezzoCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        if (FileUploadImmagine.HasFile)
                        {
                            string filename = Path.GetFileName(FileUploadImmagine.FileName);
                            string savePath = Server.MapPath("/Content/Assets/images/prodottiUp/") + filename;
                            FileUploadImmagine.SaveAs(savePath);
                            string newImageURL = "/Content/Assets/images/prodottiUp/" + filename;
                            string updateImageSql = "UPDATE Prodotti SET ImmagineURL = @ImmagineURL WHERE ProdottoID = @ProdottoID";
                            SqlCommand updateImageCommand = new SqlCommand(updateImageSql, connection);
                            updateImageCommand.Parameters.AddWithValue("@ImmagineURL", newImageURL);
                            updateImageCommand.Parameters.AddWithValue("@ProdottoID", selectedValue);
                            updateImageCommand.ExecuteNonQuery();
                            modificato = true;
                        }

                        DropDownProdotto.SelectedValue = selectedValue;
                        AggiornaCard(selectedValue);
                        BindProdottiDropDown();
                        TextBoxNome.Text = "";
                        TextBoxDescrizione.Text = "";
                        TextBoxDescrizioneEstesa.Text = "";
                        TextBoxQuantita.Text = "";
                        TextBoxPrezzo.Text = "";


                        if (modificato)
                        {
                            ModalContent.Text = $"Prodotto modificato con successo";
                            myModal.Visible = true;
                        }
                        else
                        {
                            ModalContent.Text = $"Nessuna modifica effettuata";
                            myModal.Visible = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        ModalContent.Text = $"Non hai modificato nulla: {ex.Message}";
                        myModal.Visible = true;
                    }
                }
            }
        }

        private void AggiornaCard(string selectedValue)
        {
            System.Diagnostics.Debug.WriteLine("partita");
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT p.Nome, p.Descrizione, p.Prezzo, p.ImmagineURL, d.DescrizioneEstesa, d.QuantitaDisponibile
                                           FROM Prodotti p
                                           INNER JOIN DettagliProdotto d ON p.ProdottoID = d.ProdottoID
                                           WHERE p.ProdottoID = @ProdottoID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProdottoID", selectedValue);
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            LblNome.Text = reader["Nome"].ToString();
                            LblDescrizione.Text = reader["Descrizione"].ToString();
                            LblDescrizioneEstesa.Text = reader["DescrizioneEstesa"].ToString();
                            LblQuantitaDisponibile.Text = reader["QuantitaDisponibile"].ToString();
                            LblPrezzo.Text = string.Format("Prezzo: {0:C}", reader["Prezzo"]);
                            ImgCarrello.ImageUrl = reader["ImmagineURL"].ToString();
                            Card.Visible = true;
                        }
                        else
                        {
                            Card.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Eccezione brutta");
                Console.WriteLine($"Si è verificato un errore durante l'aggiornamento della card: {ex.Message}");
            }
        }

        // Gestione delle statistiche
        protected void DropDownStats_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = DropDownStats.SelectedValue;
            switch (selectedValue)
            {
                case "TotalOrders":
                    GetTotalOrders();
                    break;
                case "TotalProductsSold":
                    GetTotalProductsSold();
                    break;
                case "TotalRevenue":
                    GetTotalRevenue();
                    break;
                case "OrdersPerUser":
                    GetOrdersPerUser();
                    break;
                case "UsersPerAge":
                    GetUsersPerAge();
                    break;
                case "OrdersPerCountry":
                    GetOrdersPerCountry();
                    break;
                case "AverageOrderValue":
                    GetAverageOrderValue();
                    break;
                case "AllProduct":
                    GetProduct();
                    break;
                case "SalesByProduct":
                    GetSalesByProduct();
                    break;
                default:
                    // Gestisci il caso in cui non sia stata selezionata una statistica valida
                    break;
            }
        }
        // Metodi per ottenere le statistiche totale ordini
        protected void GetTotalOrders()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryTotalOrders = "SELECT 'Totale ordini effettuati' AS Statistica, COUNT(*) AS Valore FROM Ordini";
                SqlDataAdapter adapter = new SqlDataAdapter(queryTotalOrders, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }

        // Metodi per ottenere le statistiche totale prodotti venduti
        protected void GetTotalProductsSold()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryTotalProductsSold = "SELECT 'Totale prodotti venduti' AS Statistica, SUM(QuantitaDisponibile) AS Valore FROM DettagliProdotto";
                SqlDataAdapter adapter = new SqlDataAdapter(queryTotalProductsSold, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche totale incasso
        protected void GetTotalRevenue()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryTotalRevenue = "SELECT 'Incasso totale' AS Statistica, SUM(TotaleOrdine) AS Valore FROM Ordini";
                SqlDataAdapter adapter = new SqlDataAdapter(queryTotalRevenue, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche ordini per utente
        protected void GetOrdersPerUser()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryOrdersPerUser = "SELECT Utenti.UtenteID, Utenti.NomeUtente AS Nome,  COUNT(*) AS NumeroOrdini FROM Ordini INNER JOIN Utenti ON Ordini.UtenteID = Utenti.UtenteID GROUP BY Utenti.UtenteID, Utenti.NomeUtente";
                SqlCommand cmd = new SqlCommand(queryOrdersPerUser, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche utenti per età
        protected void CloseButton_Click(object sender, EventArgs e)
        {
            myModal.Visible = false;
        }
        protected void GetUsersPerAge()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryUsersPerAge = "SELECT eta, COUNT(*) AS NumeroUtenti FROM Utenti GROUP BY eta";
                SqlCommand cmd = new SqlCommand(queryUsersPerAge, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche ordini per paese
        protected void GetOrdersPerCountry()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryOrdersPerUser = "SELECT PaeseDestinatario, COUNT(*) AS NumeroOrdini FROM Spedizioni GROUP BY PaeseDestinatario";
                SqlCommand cmd = new SqlCommand(queryOrdersPerUser, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche valore medio ordini
        protected void GetAverageOrderValue()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryAverageOrderValue = "SELECT 'Valore medio ordini' AS Statistica, ROUND(AVG(TotaleOrdine), 2) AS Valore FROM Ordini";
                SqlDataAdapter adapter = new SqlDataAdapter(queryAverageOrderValue, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();

                    // Format the value in the GridView to display only two decimal places
                    foreach (GridViewRow row in GridViewResults.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            Label lblValore = (Label)row.FindControl("lblValore");
                            if (lblValore != null)
                            {
                                decimal valore = Convert.ToDecimal(lblValore.Text);
                                lblValore.Text = valore.ToString("0.00");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche prodotti
        protected void GetProduct()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryGetProduct = @"
                    SELECT p.Nome AS Prodotto, 
                           dp.QuantitaDisponibile AS QuantitaDisponibile
                    FROM DettagliProdotto dp
                    INNER JOIN Prodotti p ON dp.ProdottoID = p.ProdottoID
                    ORDER BY p.Nome ASC";

                SqlDataAdapter adapter = new SqlDataAdapter(queryGetProduct, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }
        // Metodi per ottenere le statistiche vendite per prodotto
        protected void GetSalesByProduct()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string querySalesByProduct = @"
            SELECT p.Nome AS Prodotto, 
                   SUM(dp.QuantitaVenduta), 
                   SUM(dp.QuantitaVenduta * p.Prezzo) AS RicavoTotale
            FROM DettagliProdotto dp
            INNER JOIN Prodotti p ON dp.ProdottoID = p.ProdottoID
            GROUP BY p.Nome
            ORDER BY RicavoTotale DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(querySalesByProduct, conn);
                DataTable dataTable = new DataTable();
                try
                {
                    adapter.Fill(dataTable);
                    GridViewResults.DataSource = dataTable;
                    GridViewResults.DataBind();
                }
                catch (Exception ex)
                {
                    DataTable errorTable = new DataTable();
                    errorTable.Columns.Add("Errore");
                    errorTable.Rows.Add(ex.Message);
                    GridViewResults.DataSource = errorTable;
                    GridViewResults.DataBind();
                }
            }
        }




        private void PopolaDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT UtenteID, NomeUtente, IsAdmin FROM Utenti", con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string nomeUtente = reader["NomeUtente"].ToString();
                        string utenteId = reader["UtenteID"].ToString();
                        bool isAdmin = Convert.ToBoolean(reader["IsAdmin"]);

                        ListItem item = new ListItem
                        {
                            Text = isAdmin ? $"{nomeUtente} (Admin)" : nomeUtente,
                            Value = utenteId
                        };

                        UsersDropDownList.Items.Add(item);
                    }
                }
            }
            UsersDropDownList.Items.Insert(0, new ListItem("-- Seleziona Utente --", "0"));
        }


        protected void AssignAdminButton_Click(object sender, EventArgs e)
        {
            int userId;
            if (int.TryParse(UsersDropDownList.SelectedValue, out userId) && userId > 0)
            {
                bool result = AssegnaRuoloAmministratore(userId);
                if (result)
                {
                    lblMessage.Text = "Ruolo di amministratore assegnato con successo.";
                }
                else
                {
                    lblMessage.Text = "Errore nell'assegnazione del ruolo di amministratore.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                lblMessage.Visible = true;
            }
        }

        private bool AssegnaRuoloAmministratore(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Utenti SET IsAdmin = 1 WHERE UtenteID = @UtenteID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UtenteID", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        protected void DeleteAdminButton_Click(object sender, EventArgs e)
        {
            int userId;
            int currentUserId = Convert.ToInt32(Session["UserId"]); // Assumiamo che l'ID utente corrente sia memorizzato in sessione

            if (int.TryParse(UsersDropDownList.SelectedValue, out userId) && userId > 0)
            {
                if (userId == currentUserId)
                {
                    lblMessage.Text = "Non è possibile rimuovere i privilegi di amministratore dal proprio account.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    RimuoviRuoloAmministratore(userId);
                }
                lblMessage.Visible = true;
            }
        }

        private void RimuoviRuoloAmministratore(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Utenti SET IsAdmin = 0 WHERE UtenteID = @UtenteID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UtenteID", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Privilegi di amministratore rimossi con successo.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.Text = "Errore nella rimozione dei privilegi di amministratore.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void ChangeView(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int viewIndex = Convert.ToInt32(clickedButton.CommandArgument);
            MainMultiView.ActiveViewIndex = viewIndex;
        }
    }



}

