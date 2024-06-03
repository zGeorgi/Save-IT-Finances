using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using MySql;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Mysqlx.Expr;

namespace TestProject1
{
    public partial class New_Client : Form
    {
        public New_Client()
        {
            InitializeComponent();
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtFulName.Text;
                if (!full_Name_Validation(fullName))
                {
                    MessageBox.Show("Invalid full name format.");
                    return;
                }

                string email = txtEmail.Text;
                if (!email_Validation(email))
                {
                    MessageBox.Show("Invalid email address.");
                    return;
                }

                string address = txtAddress.Text;
                if (!address_Validation(address))
                {
                    MessageBox.Show("Invalid address format.");
                    return;
                }

                string phone = txtPhone.Text;
                if (!phone_Validation(phone))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                string postCode = txtPostCode.Text;
                if (!postCode_Validation(postCode))
                {
                    MessageBox.Show("Invalid UK postcode.");
                    return;
                }
               

                DialogResult dialogResult = MessageBox.Show($"Double check the details:\nFull Name: {fullName}\nAddress: {address}\nPhone: {phone}\nEmail: " +
                    $"{email}\nPostCode: {postCode}\n\nDo you want to save these details?", "Confirm Details", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    // Save these details to the database and check if it was successful
                    bool saveSuccessful = AddClientToDB(fullName, address, phone, email, postCode);

                    if (saveSuccessful)
                    {
                        // Clear all the fields after successful saving
                        ClearFields();
                        MessageBox.Show("Client details saved successfully.");
                    }
                    else
                    {
                        // Handle the failure
                        MessageBox.Show("Failed to save client details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // If the user clicks 'No', don't save and maybe notify the user
                    MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
   
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }



        private bool full_Name_Validation(string fullName)
        {
            // is not null or whitespace & consist only uppercase, lowercase, space, apostrophes or dash -
            return !string.IsNullOrWhiteSpace(fullName) && Regex.IsMatch(fullName, @"^[A-Za-z\s\'-]+$");

        }
        private bool email_Validation(string email)
        {
            try
            {
                // if the format is correct return the email else throw an error
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool phone_Validation(string phone)
        {
            // the phone number consistof only digits and is between 10-11
            return Regex.IsMatch(phone,  @"^(\d{10,11})$");
        }

       
        private bool address_Validation(string address)
        {
            // Correctly check if the address is not empty and matches the pattern
            return !string.IsNullOrWhiteSpace(address) && Regex.IsMatch(address, @"^[0-9A-Za-z]+[0-9A-Za-z\s,-]*$");
        }


        private bool postCode_Validation(string postCode)
        {
            // postcode validation 1 or 2 uppercase , 1 or 2 digits, 0 or 1 uppercase, 0 or 1 space, 1 digit,  2 uppercase on the end
            return Regex.IsMatch(postCode, @"^([A-Z]{1,2}\d{1,2}[A-Z]?\s?\d[A-Z]{2})$");
        }

        private void ClearFields()
        {
            txtFulName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";               
            txtPostCode.Text = "";
        }

        // move to mainMenu
        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            MainMenu nextForm = new MainMenu();
            this.Hide();
            nextForm.ShowDialog();
            this.Close();
        }

       

        private bool AddClientToDB(string fullName, string address, string phone, string email, string postCode)
        {
            // change to the real BD seve_it_finances and set up password
            
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO customers (`full_name`, `address`, `phone`, `email`, `postcode`)" +
                            " VALUES (@fullName, @address, @phone, @email, @postCode)";

                        // Adding parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@postCode", postCode);

                        cmd.ExecuteNonQuery(); // Execute the query

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                object userData = new { };
                userData = new
                {
                    FullName = fullName,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    PostCode = postCode
                };
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                Utility.LogError( ex, userData, "New_Client: Adding NewClient to the DataBase Fail!", stackTraceLog);
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }

        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
