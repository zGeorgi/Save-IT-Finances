using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;


namespace TestProject1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // DO VALIDATION
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            // change to the real BD seve_it_finances and set up password
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT user_id, password_hash FROM users WHERE username = @username";
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password_hash"].ToString(); // This should be the hashed password
                                int userId = Convert.ToInt32(reader["user_id"]);

                                //  VerifyPassword checks the password is correct
                                if (VerifyPassword(password, storedPassword)) // Bcrypt encryption on the password id the DB to be implemented 
                                {
                                    // set user session
                                    SessionManager.SetUserId(userId);

                                    // Load the main menu form                           
                                    MainMenu mainMenu = new MainMenu();
                                    this.Hide();
                                    mainMenu.ShowDialog();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid username or password.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                object userData = new { };
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                Utility.LogError(ex, userData, "Loggin Form error!", stackTraceLog);
                MessageBox.Show("A system error occurred!");
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Bcrypt encryption on the password id the DB to be implemented
            return inputPassword == storedHash; 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye");
            SessionManager.ClearUser();
            SessionManager.ClearCustomer();
            Application.Exit();
        }

        
    }
}
