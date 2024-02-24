using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceBW4
{
    public partial class ForgotPasswordPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Session["UserId"] != null)
                {

                    BackToLogin.Visible = false;
                    BackToShop.Visible = true;
                }
                else
                {

                    BackToLogin.Visible = true;
                    BackToShop.Visible = false;
                }
            }

        }

        /*
        * Summary: Metodo per aggiornare la password dell'utente.
        * Parameters: click del bottone "PassReset".
        * Return: Nessuno.
        */
        protected void PassReset_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcommerceBW4"].ConnectionString;

            string username = Username.Value;
            string newPassword = NewPass.Value;
            string confirmPassword = ConfirmPass.Value;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = connection.CreateCommand();

            try
            {
                connection.Open();

                // Controllo che le password corrispondano
                if (newPassword != confirmPassword)
                {
                    FeedbackMsg.InnerText = "Error: the password doesn't match.";
                    return;
                }

                // Query per controllare se l'utente esiste nel database
                string queryCheckUser = "SELECT COUNT(*) FROM Utenti WHERE NomeUtente = @Username";

                command.CommandText = queryCheckUser;
                command.Parameters.AddWithValue("@Username", username);

                int userCount = 0;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userCount = Convert.ToInt32(reader[0]);
                    }
                }

                // Controllo se l'utente esiste
                if (userCount == 0)
                {
                    FeedbackMsg.InnerText = "Error: User not found.";
                    return;
                }

                // Query per aggiornare la password dell'utente nel database
                string queryUpdatePassword = "UPDATE Utenti SET Password = @NewPassword WHERE NomeUtente = @Username";

                command.CommandText = queryUpdatePassword;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.Parameters.AddWithValue("@Username", username);

                int rowsAffected = command.ExecuteNonQuery();

                // Controllo se la query ha modificato con successo la password
                if (rowsAffected > 0)
                {
                    FeedbackMsg.InnerText = "Your password has been updated";
                }
                else
                {
                    FeedbackMsg.InnerText = "Error: couldn't update your password.";
                }
            }
            catch (Exception ex)
            {
                FeedbackMsg.InnerText = "Error: system error.";
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        /*
          * Summary: Metodo per tornare alla pagina di Login.
          * Parameters: click del bottone "BackToLogin".
          * Return: Nessuno.
        */
        protected void BackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BackToShop_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}