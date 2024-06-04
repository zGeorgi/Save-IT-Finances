using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using TestProject1;
using System.IO;
using Mysqlx.Expr;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;


public static class Utility
{
    
    public static string MyConnectionString { get; } = "server=127.0.0.1;uid=root;pwd=your_password;database=save_it_finances";

    public static void CacheUserData(object data, string filename = "ErrorLog.txt")
    {
        string filePath = Path.Combine(@"C:\Users\extre\source\repos\TestProject1\", filename);
        string jsonData = JsonConvert.SerializeObject(data); // Using Newtonsoft.Json to serialize data

        try
        {
            // Appending the serialized data to the file
            File.AppendAllText(filePath, jsonData + Environment.NewLine); // Add a newline for readability if needed
            File.AppendAllText(filePath, "--- next log---\n");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to cache user data locally: " + ex.Message);
        }
    }


    public static void LogError(Exception ex, object userData, string contextMessage = "", bool logStackTrace= false)
    {
        int? userId = SessionManager.UserId;
        int? customerId = SessionManager.UserId;
        try
        {
            using (var connection = new MySqlConnection(MyConnectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO error_logs (customer_id, error_type, error_message, diagnostic_data, timestamp, user_id) " +
                        "VALUES (@customer_id, @error_type, @error_message, @diagnostic_data, @timestamp, @user_id)";

                    cmd.Parameters.AddWithValue("@customer_id", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@error_type", "Exception"); 
                    cmd.Parameters.AddWithValue("@error_message", ex.Message);
                    cmd.Parameters.AddWithValue("@diagnostic_data", ex.StackTrace + (string.IsNullOrEmpty(contextMessage) ? "" : " - Context: " + contextMessage));
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@user_id", userId.HasValue ? (object)userId.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception logEx)
        {
            if (logEx is MySqlException || logEx.Message.Contains("Unable to connect"))
            {
                
                // Log to file if it's a connection issue
                LogErrorToFile(ex, contextMessage, logStackTrace);
                CacheUserData(userData);
            }
            else
            {
                MessageBox.Show("Failed to log error: " + logEx.Message);
            }
            
        }        
    }

    public static void LogErrorToFile(Exception ex, string contextMessage = "", bool logStackTrace = true)
    {
        string logPath = @"C:\Users\extre\source\repos\TestProject1\ErrorLog.txt";
        string errorMessage = $"Time: {DateTime.Now}\nError: {ex.Message}\n";
        if (logStackTrace)
        {
            errorMessage += $"Stack Trace: {ex.StackTrace}\n";
        }
        errorMessage += $"Context: {contextMessage}\n\n";

        try
        {
            File.AppendAllText(logPath, errorMessage);
        }
        catch (Exception logEx)
        {
            MessageBox.Show("Failed to log error to file: " + logEx.Message);
        }
    }
 
    
    public static void PopulateClientsComboBox(ComboBox comboBox)
    {
        // Change to the real DB save_it_finances and set up password later
        
        try
        {
            using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT full_name, customer_id FROM customers";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear the ComboBox to ensure it doesn't hold old data
                        comboBox.Items.Clear();

                        // Add the names to the ComboBox
                        while (reader.Read())
                        {
                            // Assuming you want to show the full_name in the ComboBox, 
                            // and use customer_id as the hidden value
                            string itemName = reader["full_name"].ToString();
                            int itemValue = Convert.ToInt32(reader["customer_id"]);

                            // Create a new ComboBoxItem with the name as text and id as value
                            comboBox.Items.Add(new ComboBoxItem<int> { Text = itemName, Value = itemValue });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            bool stackTraceLog = false;
            object userData = new {User = SessionManager.UserId };
            LogError(ex, userData, "Retrieving customers from the DataBase Failed!", stackTraceLog);
            MessageBox.Show("An error occurred while retrieving information. Please try again later.");

        }
    }

    public static void PopulateDataGrid(string dbTableName, DataGridView dataGrid, string formName)
    {
        try
        {
            // Validate table name against known good values to prevent SQL Injection
            List<string> validTableNames = new List<string> { "convertor_quotes", "investment_quotes" };
            if (!validTableNames.Contains(dbTableName))
            {
                MessageBox.Show("Invalid table name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    // Customize SQL command text based on the table name
                    if (dbTableName == "convertor_quotes")
                    {
                        cmd.CommandText = @"SELECT customer_id AS 'Customer ID', 
                                        transaction_type AS 'Transaction Type', 
                                        currency_from_id AS 'Currency 1', 
                                        currency_to_id AS 'Currency 2', 
                                        currency_amount AS 'Amount', 
                                        exchange_rate AS 'Exchange Rate', 
                                        fee AS 'Fee', 
                                        quote_total AS 'Total', 
                                        quote_time AS 'Quote Date' 
                                        FROM convertor_quotes WHERE customer_id = @CustomerId";
                    }
                    else if (dbTableName == "investment_quotes")
                    {
                        cmd.CommandText = @"SELECT customer_id AS 'Customer ID', 
                                        product_id AS 'Product ID', 
                                        initial_lump_sum AS 'Initial Lump Sum', 
                                        monthly_investment AS 'Monthly Investment', 
                                        returns_1_year_low AS '1Y Returns (Low)', 
                                        returns_1_year_high AS '1Y Returns (High)', 
                                        returns_5_years_low AS '5Y Returns (Low)', 
                                        returns_5_years_high AS '5Y Returns (High)', 
                                        returns_10_years_low AS '10Y Returns (Low)', 
                                        returns_10_years_high AS '10Y Returns (High)', 
                                        profit_1_year_low AS '1Y Profit (Low)', 
                                        profit_1_year_high AS '1Y Profit (High)', 
                                        profit_5_years_low AS '5Y Profit (Low)', 
                                        profit_5_years_high AS '5Y Profit (High)', 
                                        profit_10_years_low AS '10Y Profit (Low)', 
                                        profit_10_years_high AS '10Y Profit (High)', 
                                        fees_1_year_low AS '1Y Fees (Low)', 
                                        fees_1_year_high AS '1Y Fees (High)', 
                                        fees_5_years_low AS '5Y Fees (Low)', 
                                        fees_5_years_high AS '5Y Fees (High)', 
                                        fees_10_years_low AS '10Y Fees (Low)', 
                                        fees_10_years_high AS '10Y Fees (High)', 
                                        taxes_1_year_low AS '1Y Taxes (Low)', 
                                        taxes_1_year_high AS '1Y Taxes (High)', 
                                        taxes_5_years_low AS '5Y Taxes (Low)', 
                                        taxes_5_years_high AS '5Y Taxes (High)', 
                                        taxes_10_years_low AS '10Y Taxes (Low)', 
                                        taxes_10_years_high AS '10Y Taxes (High)', 
                                        total_invested_1 AS 'Total Invested (1 Year)', 
                                        total_invested_5 AS 'Total Invested (5 Years)', 
                                        total_invested_10 AS 'Total Invested (10 Years)', 
                                        timestamp AS 'Date' 
                                        FROM investment_quotes WHERE customer_id = @CustomerId";
                    }
                    

                    cmd.Parameters.AddWithValue("@CustomerId", SessionManager.CustomerId);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); // Fill the DataTable with data returned from database

                        if (dataTable.Rows.Count > 0) // Check if the data table contains any rows
                        {
                            dataGrid.DataSource = dataTable; // Set the DataGridView to display the DataTable
                        }
                        else
                        {
                            dataGrid.DataSource = null; // Clear the DataGridView if no data is returned
                            MessageBox.Show("No data found for the selected client.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            bool stackTraceLog = false; // Consider making this dynamic or configurable
            object userData = new { User = SessionManager.UserId, Custormer = SessionManager.CustomerId };
            LogError(ex, userData, $"{formName}: Error occurred while fetching data!", stackTraceLog);
            MessageBox.Show("An error occurred while retrieving information. Please try again later.");
        }
    }


}

