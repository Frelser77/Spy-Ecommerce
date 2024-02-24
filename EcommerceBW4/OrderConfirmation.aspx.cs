using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace EcommerceBW4
{
    public partial class OrderConfirmation : Page
    {
        protected int OrdineID { get; set; }
        protected DateTime DataOrdine { get; set; }
        protected decimal TotaleOrdine { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Request.QueryString["OrdineID"] != null)
                {
                    OrdineID = Convert.ToInt32(Request.QueryString["OrdineID"]);
                    RecuperaDettagliOrdine(OrdineID);
                    DataBind();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void RecuperaDettagliOrdine(int ordineId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Assumendo che tu abbia una colonna per il totale dell'ordine nella tabella Ordini
                string query = @"
                    SELECT DataOrdine, TotaleOrdine
                    FROM Ordini
                    WHERE OrdineID = @OrdineID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrdineID", ordineId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DataOrdine = reader.GetDateTime(reader.GetOrdinal("DataOrdine"));
                            TotaleOrdine = reader.GetDecimal(reader.GetOrdinal("TotaleOrdine"));
                        }
                    }
                }
            }
        }
    }
}
