using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace EcommerceBW4
{
    public partial class SiteMaster : MasterPage
    {
        public bool IsAdmin { get; private set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VerificaAdmin();
                adminLink.Visible = IsAdmin;
            }

            string url = Request.Url.ToString();
            if (url.Contains("/Default") || url.Contains("localhost:44321") &&
                !url.Contains("/Carrello") &&
                !url.Contains("/Dettagli") &&
                !url.Contains("Premium") &&
                !url.Contains("OrderConfermation") &&
                !url.Contains("Checkout") &&
                !url.Contains("AdminPage"))
            {
                bannerWelcome.Visible = true;
            }
            else
            {
                bannerWelcome.Visible = false;
            }
        }

        private void VerificaAdmin()
        {
            if (Session["UserId"] != null)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT IsAdmin FROM Utenti WHERE UtenteID = @UtenteID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UtenteID", userId);
                        conn.Open();
                        IsAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
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

    }
}