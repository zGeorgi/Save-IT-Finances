using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MySqlConnector;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestProject1
{
    public partial class Currency_Convertor : Form
    {
        //---------- global varialbles-----
        private decimal minTransactionValue;
        private decimal maxTransactionValue;
        private int limitId = 1;
        private List<FeeTier> feeTiers = new List<FeeTier>();
        private string transactionType = "";
        private decimal lastConversionRate;
        private decimal lastAmountCurrency1;
        private decimal lastFeePercentage;
        private decimal lastFinalAmount;

        public Currency_Convertor()
        {
            InitializeComponent();
        }
// ------------- execute the methods when the form load----------
        private void Currency_Convertor_Load(object sender, EventArgs e)
        {       
            PopulateCurrencyComboBoxes();
            Utility.PopulateClientsComboBox(cboClientName);
            FetchAndDisplayTransactionLimits();  
            FetchAndDisplayFeeTiers(limitId);
        }

    // ------comboBoxes-------------------
        private void cboClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClientName.SelectedItem is ComboBoxItem<int> selectedItem)
            {
                int customerId = selectedItem.Value;            
               SessionManager.SetCustomerId(customerId); 
            }
        }       
        //validate if the input is digit!!!!
        private void cboCurrency1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCurrency1.SelectedItem is ComboBoxItem<decimal> selectedItem)
            {
                decimal currencyValue = selectedItem.Value;              
            }
        }

        private void cboCurrency2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCurrency2.SelectedItem is ComboBoxItem<decimal> selectedItem)
            {
                decimal currencyValue = selectedItem.Value;            
            }
        }

        
        // ------------other methods -------------------------------------          
        private void PopulateCurrencyComboBoxes()
        {            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT currency_rate_id, currency_name, value FROM currency_rates", connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing items
                            cboCurrency1.Items.Clear();
                            cboCurrency2.Items.Clear();

                            while (reader.Read())
                            {
                                int currency_id = reader.GetInt32("currency_rate_id");
                                string currencyName = reader["currency_name"].ToString();
                                decimal currencyRate = reader.GetDecimal("value");

                                // Create new ComboBoxItem
                                ComboBoxItem<decimal> item = new ComboBoxItem<decimal>
                                {
                                    Id = currency_id,
                                    Text = currencyName,
                                    Value = currencyRate
                                };

                                // Add the item to both ComboBoxes
                                cboCurrency1.Items.Add(item);
                                cboCurrency2.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                object userData = "No user data"; // new{ };
                Utility.LogError(ex, userData, "Currency_Convertor: An error occurred while populating currency ComboBoxes!", stackTraceLog);
                MessageBox.Show("An error occurred while retrieving information. Please try again later.");

            }
        }

        private void FetchAndDisplayTransactionLimits()
        {
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    //get transaction limits
                    string query = "SELECT min_transaction_value, max_transaction_value FROM transaction_limits WHERE limit_Id = "+limitId;
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                minTransactionValue = reader.GetDecimal("min_transaction_value");
                                maxTransactionValue = reader.GetDecimal("max_transaction_value");

                                // Display the limits in the labels
                                lblMinimumTransaction.Text = $"Minimum transaction: {minTransactionValue}";
                                lblMaxTransaction.Text = $"Maximum transaction: {maxTransactionValue}";                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                object userData = "No user data"; // new { };
                Utility.LogError(ex, userData, "Currency_Convertor: An error occurred while fetching transaction limits!", stackTraceLog);
                MessageBox.Show($"Failed to fetch transaction limits: {ex.Message}");
            }
        }
       
        private void FetchAndDisplayFeeTiers(int limitId)
        {
           
            StringBuilder feesDisplay = new StringBuilder();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    string query = "SELECT min_amount, max_amount, fee_percentage FROM fee_tiers WHERE limit_id = @LimitId ORDER BY min_amount ASC";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@LimitId", limitId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               decimal minAmount = reader.GetDecimal("min_amount");
                                decimal maxAmount = reader.GetDecimal("max_amount");
                                decimal feePercentage = reader.GetDecimal("fee_percentage");

                                feeTiers.Add(new FeeTier
                                {
                                    MinAmount = minAmount,
                                    MaxAmount = maxAmount,
                                    FeePercentage = feePercentage
                                });

                                feesDisplay.AppendLine($"Min: {minAmount}, Max: {maxAmount}, Fee: {feePercentage}%");
                            }
                        }
                    }
                }

                lblFee1.Text = feesDisplay.ToString();
            }
            catch (Exception ex)
            {
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                object userData = "No user data"; // new {};
                Utility.LogError(ex, userData, "Currency_Convertor: An error occurred while fetching Fees!", stackTraceLog);
                MessageBox.Show($"Failed to fetch fee tiers: {ex.Message}");
            }
        }

       
        // fee on the initial amount
        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCurrency1.SelectedItem is ComboBoxItem<decimal> selectedCurrency1 &&
                cboCurrency2.SelectedItem is ComboBoxItem<decimal> selectedCurrency2)
                {
                    decimal rateCurrency1 = selectedCurrency1.Value;
                    decimal rateCurrency2 = selectedCurrency2.Value;
                    decimal conversionRate = rateCurrency2 / rateCurrency1;
                    DefineTransactionType();
                    lastConversionRate = conversionRate;
                    txtConversionRate.Text = conversionRate.ToString("N4"); // Display the conversion rate with 4 decimal places

                    if (decimal.TryParse(txtCurrency1Amount.Text, out decimal amountCurrency1))
                    {
                        // Check if the amount is within the limits
                        if (amountCurrency1 >= minTransactionValue && amountCurrency1 <= maxTransactionValue)
                        {
                            // Apply the fee based on the converted amount
                            decimal feePercentage = DetermineFeePercentage(amountCurrency1);              
                            lastFeePercentage = feePercentage;
                            decimal feeAmount = amountCurrency1 * (feePercentage / 100);                           
                            decimal amountBeforeConversion = amountCurrency1- feeAmount;
                           
                            // Calculate the conversion
                            decimal convertedAmount = amountBeforeConversion * conversionRate;

                            lastAmountCurrency1 = amountCurrency1;
                            lastFinalAmount = convertedAmount;
                            txtCurrency2Amount.Text = convertedAmount.ToString("N2"); // Display the converted amount with 2 decimal places
                        }
                        else
                        {
                            MessageBox.Show($"Amount must be between {minTransactionValue} and {maxTransactionValue}.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid input amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select currencies to convert.", "Selection Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                object userData = new {
                    Currency1= cboCurrency1.SelectedItem.ToString(),
                    Currency2= cboCurrency2.SelectedItem.ToString(),
                    ConvertionRate = lastConversionRate, 
                    Fee = lastFeePercentage,
                    InitialCurreencyAmount = lastAmountCurrency1,
                    otherCurrencyAmount = lastFinalAmount };
                Utility.LogError(ex, userData, "Currency_Convertor: An error occurred during conversion!", stackTraceLog);
                MessageBox.Show("An error occurred during conversion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);             
            }
        }


        // chech the fee tier 
        private decimal DetermineFeePercentage(decimal amount)
        {
            foreach (var feeTier in feeTiers)
            {
                if (amount >= feeTier.MinAmount && amount <= feeTier.MaxAmount)
                {
                    return feeTier.FeePercentage; 
                }
            }
            return 0; // Return 0 if no matching fee tier is found
        }


        private void txtConversionRate_TextChanged(object sender, EventArgs e)
        {

        }  

        private void btnSaveQuote_Click(object sender, EventArgs e)
        {
            if (SessionManager.CustomerId <= 0 || SessionManager.CustomerId == null) 
            {            
                MessageBox.Show("Please select a client name before saving the quote.", "Client Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // prevent saving the quote without a selected client
            }
            try
            {
                // Retrieve currency IDs 
                decimal currencyFromId = ((ComboBoxItem<decimal>)cboCurrency1.SelectedItem).Id;
                decimal currencyToId = ((ComboBoxItem<decimal>)cboCurrency2.SelectedItem).Id;

                DateTime quoteTime = DateTime.Now; // Current time as quote time

                // Database connection and SQL command to insert the quote
                
                using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                {
                    connection.Open();
                    string cmdText = @"INSERT INTO convertor_quotes 
                                (customer_id, transaction_type, currency_from_id, currency_to_id, 
                                 currency_amount, exchange_rate, fee, quote_total, quote_time) 
                                VALUES 
                                (@CustomerId, @TransactionType, @CurrencyFromId, @CurrencyToId, 
                                 @CurrencyAmount, @ExchangeRate, @Fee, @QuoteTotal, @QuoteTime)";

                    using (MySqlCommand cmd = new MySqlCommand(cmdText, connection))
                    {
                        // Parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@CustomerId", SessionManager.CustomerId);
                        cmd.Parameters.AddWithValue("@TransactionType", transactionType);
                        cmd.Parameters.AddWithValue("@CurrencyFromId", currencyFromId);
                        cmd.Parameters.AddWithValue("@CurrencyToId", currencyToId);
                        cmd.Parameters.AddWithValue("@CurrencyAmount", lastAmountCurrency1);
                        cmd.Parameters.AddWithValue("@ExchangeRate", lastConversionRate);
                        cmd.Parameters.AddWithValue("@Fee", lastFeePercentage); 
                        cmd.Parameters.AddWithValue("@QuoteTotal", lastFinalAmount);
                        cmd.Parameters.AddWithValue("@QuoteTime", quoteTime);

                        cmd.ExecuteNonQuery(); // Execute the insert command
                        
                        SessionManager.ClearCustomer();
                    }
                }

                MessageBox.Show("Quote saved successfully.");
            }
            catch (Exception ex)
            {
                DateTime quoteTime = DateTime.Now;
                
                object userData = new
                {
                    Customer_id = SessionManager.CustomerId,
                    CurrencyFromId = ((ComboBoxItem<decimal>)cboCurrency1.SelectedItem).Id,
                    Currency2ToId = ((ComboBoxItem<decimal>)cboCurrency2.SelectedItem).Id,
                    ConvertionRate = lastConversionRate,
                    Fee = lastFeePercentage,
                    InitialCurreencyAmount = lastAmountCurrency1,
                    OtherCurrencyAmount = lastFinalAmount,            
                    Time = quoteTime
                };
                bool stackTraceLog = false; // detailed info about the error if it's TRUE
                Utility.LogError(ex, userData, "Currency_Convertor: An error occurred while saving the quote!", stackTraceLog);
                MessageBox.Show($"Failed to save the quote: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // sell or buy transaction
        private void DefineTransactionType()
        {
           
            string currency1 = ((ComboBoxItem<decimal>)cboCurrency1.SelectedItem)?.Text; // Assuming the Text property holds the currency code
            string currency2 = ((ComboBoxItem<decimal>)cboCurrency2.SelectedItem)?.Text;        

            if (currency1 == "GBP" && currency2 != "GBP")
            {
                transactionType = "buy"; // Buying foreign currency with GBP
            }
            else if (currency1 != "GBP" && currency2 == "GBP")
            {
                transactionType = "sell"; // Selling foreign currency for GBP
            }
            else
            {
                transactionType = "cross-currency"; //  Neither currency is GBP, indicating a cross-currency transaction
            }
            
        }
      

        // ------------- Forms navigation methods-----------------------------------
        private void btnAddClient_Click(object sender, EventArgs e)
        {
            SessionManager.ClearCustomer();
            New_Client nextForm = new New_Client();
            this.Hide();
            nextForm.ShowDialog();
            this.Close();
        }
        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            SessionManager.ClearCustomer();
            MainMenu nextForm = new MainMenu();
            this.Hide();
            nextForm.ShowDialog();
            this.Close();
        }


        // -----------------------  labels -------------
        private void lblMinimumTransaction_Click(object sender, EventArgs e)
        {

        }
        private void lblMaxTransaction_Click(object sender, EventArgs e)
        {

        }
        private void lblFee1_Click(object sender, EventArgs e)
        {

        }

        private void btnRetrieveQuote_Click(object sender, EventArgs e)
        {
            if (SessionManager.CustomerId <= 0 || SessionManager.CustomerId == null)
            {
                MessageBox.Show("Please select a client name before proceeding.", "Client Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // prevent retrieving the data without a selected client
            }
            
            Utility.PopulateDataGrid("convertor_quotes", dgvQuotesConrvertor, "Currency_Convertor");
                     
        }
    }
}

