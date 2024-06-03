using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;
using MySqlConnector;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject1
{
    public partial class Investments : Form
    {
        //------ Global variables fetched from the DB----------
        private decimal? InvestmentPerYearGV = null;
        private decimal MinMonthlyInvestmentGV;
        private decimal? MinInitialLumpSumGV = 0;
        private decimal PredictedReturnLowGV;
        private decimal PredictedReturnHighGV;
        private decimal MonthlyRBSXFeePercentageGV;
        private int SelectedProductIdGV = 0;
        private List<TaxThreshold> TaxThresholds = new List<TaxThreshold>();

        //------ Global variables to store investment results ----------
        private InvestmentPlanResults investmentResults = new InvestmentPlanResults();


        public Investments()
        {
            InitializeComponent();
        }

        private void Investments_Load(object sender, EventArgs e)
        {
            Utility.PopulateClientsComboBox(cboClientName);
        }


        //-------- Forms navigation methods------------
        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            SessionManager.ClearCustomer();
            MainMenu nextForm = new MainMenu();
            this.Hide();
            nextForm.ShowDialog();          
            this.Close();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            SessionManager.ClearCustomer();
            New_Client nextForm = new New_Client();
            this.Hide();
            nextForm.ShowDialog();       
            this.Close();
        }

        private void cboClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClientName.SelectedItem is ComboBoxItem<int> selectedItem)
            {
                int customerId = selectedItem.Value;
                SessionManager.SetCustomerId(customerId);
            }
        }
        // -------------- RadioButtons-----------
        private void rbBasicSavingPlan_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBasicSavingPlan.Checked) FetchAndSetInvestmentPlanDetails();
        }

        private void rbSavingPlanPlus_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSavingPlanPlus.Checked) FetchAndSetInvestmentPlanDetails();
        }

        private void rbManagedStockInvestments_CheckedChanged(object sender, EventArgs e)
        {
            if (rbManagedStockInvestments.Checked) FetchAndSetInvestmentPlanDetails();
        }


        //----------DataBase methods-------
        private void FetchAndSetInvestmentPlanDetails()
        {
            string planName = GetSelectedPlanName();        

            if (!string.IsNullOrEmpty(planName))
            {
                try
                {
                    using (var connection = new MySqlConnection(Utility.MyConnectionString))
                    {
                        connection.Open();

                        // Fetch investment plan details
                        using (var cmd = new MySqlCommand($"SELECT * FROM investment_products WHERE product_name = @planName", connection))
                        {
                            cmd.Parameters.AddWithValue("@planName", planName);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    SelectedProductIdGV = reader.GetInt32("product_id"); // Retrieve and store product_id

                                    InvestmentPerYearGV = reader.IsDBNull(reader.GetOrdinal("max_annual_investment")) ? (decimal?)null : reader.GetDecimal("max_annual_investment");

                                    MinMonthlyInvestmentGV = reader.GetDecimal("min_monthly_investment");
                                    MinInitialLumpSumGV = reader.IsDBNull(reader.GetOrdinal("min_initial_lumpsum")) ? null : (decimal?)reader.GetDecimal("min_initial_lumpsum");
                                    PredictedReturnLowGV = reader.GetDecimal("predicted_return_low");
                                    PredictedReturnHighGV = reader.GetDecimal("predicted_return_high");
                                    MonthlyRBSXFeePercentageGV = reader.GetDecimal("monthly_fees_percentage");
                                }
                            }
                        }

                        if (SelectedProductIdGV > 0)
                        {
                            using (var cmd = new MySqlCommand($"SELECT * FROM estimated_tax WHERE product_id = @productId ORDER BY threshold_amount ASC", connection))
                            {
                                cmd.Parameters.AddWithValue("@productId", SelectedProductIdGV);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    // Initialize an empty string to hold the tax details for all thresholds
                                    string taxDetails = "";
                                    TaxThresholds.Clear();
                                    // Loop through all records returned
                                    while (reader.Read())
                                    {
                                        var currentThresholdAmount = reader.IsDBNull(reader.GetOrdinal("threshold_amount")) ? null : (decimal?)reader.GetDecimal("threshold_amount");
                                        var currentTaxRate = reader.GetDecimal("tax_rate");
                                       // MessageBox.Show($"currentThresholdAmount: {currentThresholdAmount}, currentTaxRate: {currentTaxRate} ");
                                        // Append the current record's details to the taxDetails and taxThresholds list
                                        TaxThresholds.Add(new TaxThreshold { ThresholdAmount = currentThresholdAmount, TaxRate = currentTaxRate });
                                        taxDetails += $"Profits above: {currentThresholdAmount?.ToString() ?? "N/A"}, Tax: {currentTaxRate}%\n";
                                    }

                                    // Check if taxDetails is still empty, implying no records were found
                                    if (string.IsNullOrWhiteSpace(taxDetails))
                                    {
                                        taxDetails = "No tax details available.";
                                    }
                                    // Display the accumulated tax details in a MessageBox or assign to a Label                               
                                    lblTaxValue.Text = taxDetails;
                                }
                            }
                        }

                    }
                    // update the plan info
                    lblYearLimit.Text = InvestmentPerYearGV.HasValue ? InvestmentPerYearGV.Value.ToString() : "N/A";
                    lblValueMonthly.Text = MinMonthlyInvestmentGV.ToString();
                    lblLumpSum.Text = MinInitialLumpSumGV.HasValue ? MinInitialLumpSumGV.Value.ToString() : "N/A";
                    lblReturnsValue.Text = $"Low:{PredictedReturnLowGV}% High: {PredictedReturnHighGV}%";
                    lblGroupFeeValue.Text = MonthlyRBSXFeePercentageGV.ToString("N2") + "%";
                }
                catch(Exception ex)
                {
                    object userData = "No user data";

                    bool stackTraceLog = false; // detailed info about the error if it's TRUE
                    Utility.LogError(ex, userData, "Investments: Failed to fetch investment plan details!", stackTraceLog);
                    MessageBox.Show("An error occurred while fetching investment plan details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an investment plan.", "Plan Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetSelectedPlanName()
        {
            if (rbBasicSavingPlan.Checked) return "Basic Saving Plan";
            if (rbSavingPlanPlus.Checked) return "Saving Plan Plus";
            if (rbManagedStockInvestments.Checked) return "Managed Stock Investments";
            return null;
        }


        private void btnGetQuote_Click(object sender, EventArgs e)
        {
           
            try
            {
               // -------------  input validations------------
                if (SelectedProductIdGV <= 0)
                {
                    MessageBox.Show("Please select an investment plan.");
                    return;
                }
                
                // Initialize initialLumpSum as nullable and set to null by default
                decimal? initialLumpSum = null;

                // Only parse initialLumpSum if txtInitialLump.Text is not empty            
                if (!string.IsNullOrEmpty(txtInitialLump.Text))
                {
                    if (decimal.TryParse(txtInitialLump.Text, out decimal lumpSumResult))
                    {
                        initialLumpSum = lumpSumResult;
                    }
                    else
                    {
                        MessageBox.Show("Invalid initial lump sum entered. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Exit if the parsing fails
                    }
                }

                decimal monthlyInvestment;
                if (!decimal.TryParse(txtMontlyInvestment.Text, out monthlyInvestment))
                {
                    MessageBox.Show("Invalid monthly investment entered. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit if the parsing fails
                }

                // Check if initialLumpSum is required and meets the minimum (if applicable)
                if (MinInitialLumpSumGV.HasValue && (!initialLumpSum.HasValue || initialLumpSum.Value < MinInitialLumpSumGV.Value))
                {
                    MessageBox.Show("The initial lump sum does not meet the minimum requirement.");
                    return;
                }
            
                // Check if the total annual investment exceeds the maximum (if applicable)
                decimal totalAnnualInvestment = (initialLumpSum ?? 0) + (monthlyInvestment * 12);

                if (InvestmentPerYearGV.HasValue && totalAnnualInvestment > InvestmentPerYearGV.Value)
                {
                    MessageBox.Show("The combined annual investment amounts (initial lump sum plus monthly investments) exceed the selected plan's annual limit.");
                    return;
                }

                // Check if the monthly investment meets the minimum requirement
                if (monthlyInvestment < MinMonthlyInvestmentGV)
                {
                    MessageBox.Show("The monthly investment does not meet the minimum requirement.");
                    return;
                }


                // ------------------- return current investment after mothly returns and applied monthly fees 

                int[] yearsToCalculate = new int[] { 1, 5, 10 };
                foreach (int years in yearsToCalculate)
                {
                    CalculateAndStoreResults(years, initialLumpSum ?? 0, monthlyInvestment);
                }

                DisplayQuote();
                
            }
            catch (FormatException)
            {
                // Handle cases where the input text was not a valid decimal number
                MessageBox.Show("Please enter valid numbers for the initial lump sum and monthly investment.");
            }
            catch (Exception ex)
            {
                DateTime quoteTime = DateTime.Now;

                // Safe parsing for InitialLumpSum
                if (!decimal.TryParse(txtInitialLump.Text, out decimal initialLumpSumParsed))
                {
                    initialLumpSumParsed = 0; // Default or indicative value, or you can keep it nullable
                }

                // Safe parsing for MonthlyInvestment
                decimal monthlyInvestmentParsed = 0; // Default to 0
                decimal.TryParse(txtMontlyInvestment.Text, out monthlyInvestmentParsed);

                // Prepare user data for logging
                object userData = new
                {
                    Customer_id = SessionManager.CustomerId,
                    SelectedProductId = SelectedProductIdGV,
                    InitialLumpSum = initialLumpSumParsed, // Use the safely parsed value
                    MonthlyInvestment = monthlyInvestmentParsed, // Use the safely parsed value
                    Time = quoteTime
                };
                // Handle any other unexpected errors
                bool stackTraceLog = false; // detailed info about the error if it's TRUE              
                Utility.LogError(ex, userData, "Investments: An error occurred while fetching the Quote!", stackTraceLog);
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }

            void CalculateAndStoreResults(int years, decimal initialLumpSum, decimal monthlyInvestment)
            {                

                decimal totalInvested = CalculateTotalInvested(initialLumpSum, monthlyInvestment, years);
                (decimal balanceBeforeProfitTaxLowReturn, decimal totalFeesLowReturn) = TotalAfterMonthlyFeeAndReturn(initialLumpSum, monthlyInvestment, years, PredictedReturnLowGV);              
                decimal profitBeforeProfitTaxLowReturn = CalculateProfitBeforeTax(balanceBeforeProfitTaxLowReturn, totalInvested);    
                decimal profitTaxLowReturn = profitBeforeProfitTaxLowReturn > 0 ? CalculateProfitTaxes(profitBeforeProfitTaxLowReturn, TaxThresholds) : 0;        
                decimal returnAfterAllTaxesFeeAndReturnsLowReturn = balanceBeforeProfitTaxLowReturn - profitTaxLowReturn;
                
                (decimal balanceBeforeProfitTaxHighReturn, decimal totalFeesHighReturn) = TotalAfterMonthlyFeeAndReturn(initialLumpSum, monthlyInvestment, years, PredictedReturnHighGV);
               // MessageBox.Show("balanceBeforeProfitTaxHighReturn= "+ balanceBeforeProfitTaxHighReturn);
                decimal profitBeforeProfitTaxHighReturn = CalculateProfitBeforeTax(balanceBeforeProfitTaxHighReturn, totalInvested);
                decimal profitTaxHighReturn = profitBeforeProfitTaxHighReturn > 0 ? CalculateProfitTaxes(profitBeforeProfitTaxHighReturn, TaxThresholds) : 0;
                decimal returnAfterAllTaxesFeeAndReturnsHigh = balanceBeforeProfitTaxHighReturn - profitTaxHighReturn;

                // Store results based on the year
                InvestmentResult result = new InvestmentResult
                {
                    MaxReturn = returnAfterAllTaxesFeeAndReturnsHigh,
                    MinReturn = returnAfterAllTaxesFeeAndReturnsLowReturn,
                    TotalProfitMaxReturn = profitBeforeProfitTaxHighReturn - profitTaxHighReturn,
                    TotalProfitMinReturn = profitBeforeProfitTaxLowReturn - profitTaxLowReturn,
                    TotalFeesMaxReturn = totalFeesHighReturn,
                    TotalFeesMinReturn = totalFeesLowReturn,
                    TotalTaxesMaxReturn = profitTaxHighReturn,
                    TotalTaxesMinReturn = profitTaxLowReturn,
                    TotalInvested = totalInvested
                };

                if (investmentResults == null)
                {
                    investmentResults = new InvestmentPlanResults();
                }
                
                switch (years)
                {
                    case 1:
                        investmentResults.OneYear = result;
                        break;
                    case 5:
                        investmentResults.FiveYears = result;
                        break;
                    case 10:
                        investmentResults.TenYears = result;
                        break;
                }
            }

            // other version on calculation fee monthly
            decimal CalculateMonthlyReturnRate(decimal annualFeeRate)
            {
                // Simple division to convert annual fee rate to monthly fee rate 
           // decimal annualFeeRate = 0.012m; // Example annual fee rate of 1.2%
                return annualFeeRate / 12;
            }
            

            // --------return curent ballance with applied monthly return and fees  (before profit tax), total fees and total invested
            (decimal currentBalance, decimal totalFees) TotalAfterMonthlyFeeAndReturn(decimal lumpSum, decimal monthlyInvestment, int years, decimal annualReturnRate)
            {
                decimal currentBalancee = lumpSum;
                decimal totalFees = 0;

                decimal initialFees = currentBalancee * MonthlyRBSXFeePercentageGV / 100;
                //take the fee on the initial sum
                 currentBalancee = currentBalancee - initialFees;
                int months = 12 * years;            
              
                decimal monthlyFeeAmount = monthlyInvestment * MonthlyRBSXFeePercentageGV / 100;           
                // Convert the annual return rate to a monthly rate
                decimal monthlyReturnRate = CalculateMonthlyReturnRate(annualReturnRate / 100);
                totalFees = initialFees;

                for (int month = 1; month <= months; month++)
                {
                    // take the feee on the monthly invesment               
                    currentBalancee = currentBalancee + (monthlyInvestment - monthlyFeeAmount);

                    // sum the total aplly the return    
                    currentBalancee = currentBalancee +(currentBalancee * monthlyReturnRate); // Apply the monthly return                                 
                    totalFees += monthlyFeeAmount; // Accumulate the total fees                
                }

                return (currentBalancee, totalFees); // Return the final balance and total fees totalInvested
            }



            //----------calculation methods-----------
            decimal CalculateTotalInvested(decimal LumpSum, decimal monthlyInvestments, int years)
            {
                return LumpSum + monthlyInvestments * 12 * years;
            }


            // --- returns profit before profit tax
            decimal CalculateProfitBeforeTax(decimal totalReturnBeforeProfitTax, decimal totalInvested)
            {
                return totalReturnBeforeProfitTax - totalInvested;
            }

            //---- check the profit Tax and return the tax amount
             decimal CalculateProfitTaxes(decimal profitBeforeTax, List<TaxThreshold> thresholds)
             {
                // MessageBox.Show($"profitBeforeTax (total Return Before ProfitTax - total Invested)= {profitBeforeTax :C} " );
                 decimal profitTaxAmount = 0;
                 foreach (var threshold in thresholds.OrderBy(t => t.ThresholdAmount))
                 {

                     if (threshold.ThresholdAmount.Value == null)
                     {
                        break;
                     }

                     if (profitBeforeTax> 45000 && GetSelectedPlanName() == "Managed Stock Investments")
                    {
                        decimal firstTreshahold = 45000 - threshold.ThresholdAmount.Value;
                        decimal taxfirstTreshahold = firstTreshahold * (10m / 100);

                        decimal secondTreshahold = profitBeforeTax - 45000;
                        decimal taxSecondTrashhold = secondTreshahold * (20m / 100);

                        profitTaxAmount = taxfirstTreshahold + taxSecondTrashhold;

                        MessageBox.Show($"taxable over 1500 10% = {taxfirstTreshahold:C} \n" +
                            $"taxable over 45000 on 20% = {taxSecondTrashhold:C}\n" +
                            $"all taxes {profitTaxAmount:C}");
                        break;
                    }
                     if (profitBeforeTax > threshold.ThresholdAmount.Value)
                     {                     
                         decimal taxableAmount = profitBeforeTax - threshold.ThresholdAmount.Value;                      
                         profitTaxAmount += taxableAmount * threshold.TaxRate / 100;
                        break;
                     }
                 }
                 return profitTaxAmount;
             }
            

        }
  


        private void btnSaveTheQuote_Click(object sender, EventArgs e)
        {
           
            try
            {
                // Ensure a customer is selected
                if (!SessionManager.CustomerId.HasValue || SessionManager.CustomerId <= 0)
                {
                    MessageBox.Show("Please select a client name before saving the quote.", "Client Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the TotalInvested value for OneYear has been set (i.e., is greater than zero)
                if (investmentResults == null || investmentResults.OneYear == null ||  investmentResults.OneYear.TotalInvested <= 0)
                {
                    MessageBox.Show("Please retrieve a quote before attempting to save.", "Quote Not Retrieved", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                decimal? initialLumpSum = null; 
                decimal monthlyInvestment;

                if (!decimal.TryParse(txtMontlyInvestment.Text, out monthlyInvestment))
                {
                    MessageBox.Show("Invalid monthly investment entered. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                // Parse the initial lump sum if provided
                if (!string.IsNullOrWhiteSpace(txtInitialLump.Text))
                {
                    if (decimal.TryParse(txtInitialLump.Text, out decimal lumpSumResult))
                    {
                        initialLumpSum = lumpSumResult;
                    }
                    else
                    {
                        MessageBox.Show("Invalid initial lump sum entered. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Exit the method or disable further processing
                    }
                }

                // Database connection and SQL command to insert the quote
               
                    using (MySqlConnection connection = new MySqlConnection(Utility.MyConnectionString))
                    {
                        connection.Open();
                        string cmdText = @"INSERT INTO investment_quotes 
                    (customer_id, product_id, initial_lump_sum, monthly_investment,
                     returns_1_year_low, returns_1_year_high, returns_5_years_low, returns_5_years_high,
                     returns_10_years_low, returns_10_years_high, profit_1_year_low, profit_1_year_high,
                     profit_5_years_low, profit_5_years_high, profit_10_years_low, profit_10_years_high,
                     fees_1_year_low, fees_1_year_high, fees_5_years_low, fees_5_years_high,
                     fees_10_years_low, fees_10_years_high, taxes_1_year_low, taxes_1_year_high,
                     taxes_5_years_low, taxes_5_years_high, taxes_10_years_low, taxes_10_years_high,
                    total_invested_1, total_invested_5, total_invested_10, timestamp) 
                     VALUES 
                     (@CustomerId, @ProductId, @InitialLumpSum, @MonthlyInvestment,
                      @Returns1YearLow, @Returns1YearHigh, @Returns5YearsLow, @Returns5YearsHigh,
                      @Returns10YearsLow, @Returns10YearsHigh, @Profit1YearLow, @Profit1YearHigh,
                      @Profit5YearsLow, @Profit5YearsHigh, @Profit10YearsLow, @Profit10YearsHigh,
                      @Fees1YearLow, @Fees1YearHigh, @Fees5YearsLow, @Fees5YearsHigh,
                      @Fees10YearsLow, @Fees10YearsHigh, @Taxes1YearLow, @Taxes1YearHigh,
                      @Taxes5YearsLow, @Taxes5YearsHigh, @Taxes10YearsLow, @Taxes10YearsHigh,
                      @TotalInvested1Year, @TotalInvested5Year, @TotalInvested10Year, @QuoteTime);";

                        using (MySqlCommand cmd = new MySqlCommand(cmdText, connection))
                        {
                            // Parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@CustomerId", SessionManager.CustomerId);
                            cmd.Parameters.AddWithValue("@ProductId", SelectedProductIdGV);
                            cmd.Parameters.AddWithValue("@InitialLumpSum", initialLumpSum.HasValue ? (object)initialLumpSum.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@MonthlyInvestment", monthlyInvestment);

                            cmd.Parameters.AddWithValue("@Returns1YearLow", investmentResults.OneYear.MinReturn);
                            cmd.Parameters.AddWithValue("@Returns1YearHigh", investmentResults.OneYear.MaxReturn);

                            cmd.Parameters.AddWithValue("@Returns5YearsLow", investmentResults.FiveYears.MinReturn);
                            cmd.Parameters.AddWithValue("@Returns5YearsHigh", investmentResults.FiveYears.MaxReturn);

                            cmd.Parameters.AddWithValue("@Returns10YearsLow", investmentResults.TenYears.MinReturn);
                            cmd.Parameters.AddWithValue("@Returns10YearsHigh", investmentResults.TenYears.MaxReturn);

                            cmd.Parameters.AddWithValue("@Profit1YearLow", investmentResults.OneYear.TotalProfitMinReturn);
                            cmd.Parameters.AddWithValue("@Profit1YearHigh", investmentResults.OneYear.TotalProfitMaxReturn);

                            cmd.Parameters.AddWithValue("@Profit5YearsLow", investmentResults.FiveYears.TotalProfitMinReturn);
                            cmd.Parameters.AddWithValue("@Profit5YearsHigh", investmentResults.FiveYears.TotalProfitMaxReturn);

                            cmd.Parameters.AddWithValue("@Profit10YearsLow", investmentResults.TenYears.TotalProfitMinReturn);
                            cmd.Parameters.AddWithValue("@Profit10YearsHigh", investmentResults.TenYears.TotalProfitMaxReturn);

                            cmd.Parameters.AddWithValue("@Fees1YearLow", investmentResults.OneYear.TotalFeesMinReturn);
                            cmd.Parameters.AddWithValue("@Fees1YearHigh", investmentResults.OneYear.TotalFeesMaxReturn);

                            cmd.Parameters.AddWithValue("@Fees5YearsLow", investmentResults.FiveYears.TotalFeesMinReturn);
                            cmd.Parameters.AddWithValue("@Fees5YearsHigh", investmentResults.FiveYears.TotalFeesMaxReturn);

                            cmd.Parameters.AddWithValue("@Fees10YearsLow", investmentResults.TenYears.TotalFeesMinReturn);
                            cmd.Parameters.AddWithValue("@Fees10YearsHigh", investmentResults.TenYears.TotalFeesMaxReturn);

                            cmd.Parameters.AddWithValue("@Taxes1YearLow", investmentResults.OneYear.TotalTaxesMinReturn);
                            cmd.Parameters.AddWithValue("@Taxes1YearHigh", investmentResults.OneYear.TotalTaxesMaxReturn);

                            cmd.Parameters.AddWithValue("@Taxes5YearsLow", investmentResults.FiveYears.TotalTaxesMinReturn);
                            cmd.Parameters.AddWithValue("@Taxes5YearsHigh", investmentResults.FiveYears.TotalTaxesMaxReturn);

                            cmd.Parameters.AddWithValue("@Taxes10YearsLow", investmentResults.TenYears.TotalTaxesMinReturn);
                            cmd.Parameters.AddWithValue("@Taxes10YearsHigh", investmentResults.TenYears.TotalTaxesMaxReturn);

                            cmd.Parameters.AddWithValue("@TotalInvested1Year", investmentResults.OneYear.TotalInvested);
                            cmd.Parameters.AddWithValue("@TotalInvested5Year", investmentResults.FiveYears.TotalInvested);
                            cmd.Parameters.AddWithValue("@TotalInvested10Year", investmentResults.TenYears.TotalInvested);
                        
                            cmd.Parameters.AddWithValue("@QuoteTime", DateTime.Now);

                            cmd.ExecuteNonQuery(); // Execute the insert command
                        SessionManager.ClearCustomer();

                    }
                    }
                ClearInvestmentData();
                MessageBox.Show("Investment quote saved successfully.");
                }
                catch (Exception ex)
                {
                DateTime quoteTime = DateTime.Now;
                // to saving data in a file if no connection to the DB               

                // Safe parsing for InitialLumpSum
                if (!decimal.TryParse(txtInitialLump.Text, out decimal initialLumpSumParsed))
                {
                    initialLumpSumParsed = 0; // Default or indicative value, or you can keep it nullable
                }

                // Safe parsing for MonthlyInvestment
                decimal monthlyInvestmentParsed = 0; // Default to 0
                decimal.TryParse(txtMontlyInvestment.Text, out monthlyInvestmentParsed);

                // Prepare user data for logging
                object userData = new
                {
                    Customer_id = SessionManager.CustomerId,
                    SelectedProductId = SelectedProductIdGV,
                    InitialLumpSum = initialLumpSumParsed, // Use the safely parsed value
                    MonthlyInvestment = monthlyInvestmentParsed, // Use the safely parsed value
                    Time = quoteTime
                };
                bool stackTraceLog = false; // detailed info about the error if it's TRUE                            
                Utility.LogError(ex, userData, "Investments: An error occurred while saving the quote!", stackTraceLog);
                MessageBox.Show($"Failed to save the investment quote: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
   

        private void DisplayQuote()
        {
            //------One Year----------------------
            lblTotalInvested1.Text =  investmentResults.OneYear.TotalInvested.ToString("C");
            lblMaxReturn1.Text = "Max Return\n " + investmentResults.OneYear.MaxReturn.ToString("C");
            lblMinReturn1.Text = "Min Return \n" + investmentResults.OneYear.MinReturn.ToString("C");

            lblFeesMaxReturn1.Text = "Fees \n " + investmentResults.OneYear.TotalFeesMaxReturn.ToString("C");
            lblFeesMinReturn1.Text = "Fees \n" +  investmentResults.OneYear.TotalFeesMinReturn.ToString("C");

            lblProfitMinReturn1.Text = "Profit \n " + investmentResults.OneYear.TotalProfitMinReturn.ToString("C");
            lblProfitMaxReturn1.Text = "Profit \n " + investmentResults.OneYear.TotalProfitMaxReturn.ToString("C");

            lblTaxesMaxReturn1.Text = "Taxes \n" + investmentResults.OneYear.TotalTaxesMaxReturn.ToString("C");
            lblTaxesMinReturn1.Text = "Taxes \n" + investmentResults.OneYear.TotalTaxesMinReturn.ToString("C");


            //------Five Year-----------------------------
            lblTotalInvested5.Text =   investmentResults.FiveYears.TotalInvested.ToString("C");
            lblMaxReturn5.Text = "Max Return \n" + investmentResults.FiveYears.MaxReturn.ToString("C");
            lblMinReturn5.Text = "Min Return \n" + investmentResults.FiveYears.MinReturn.ToString("C");

            lblFeesMaxReturn5.Text = "Fees \n" + investmentResults.FiveYears.TotalFeesMaxReturn.ToString("C");
            lblFeesMinReturn5.Text = "Fees \n" + investmentResults.FiveYears.TotalFeesMinReturn.ToString("C");

            lblProfitMinReturn5.Text = "Profit \n " + investmentResults.FiveYears.TotalProfitMinReturn.ToString("C");
            lblProfitMaxReturn5.Text = "Profit \n " + investmentResults.FiveYears.TotalProfitMaxReturn.ToString("C");

            lblTaxesMaxReturn5.Text = "Taxes \n" + investmentResults.FiveYears.TotalTaxesMaxReturn.ToString("C");
            lblTaxesMinReturn5.Text = "Taxes \n" + investmentResults.FiveYears.TotalTaxesMinReturn.ToString("C");

            //------Ten Year-----------------------------------
            lblTotalInvested10.Text = investmentResults.TenYears.TotalInvested.ToString("C");
            lblMaxReturn10.Text = "Max Return \n " + investmentResults.TenYears.MaxReturn.ToString("C");
            lblMinReturn10.Text = "Min Return \n" + investmentResults.TenYears.MinReturn.ToString("C");

            lblFeesMaxReturn10.Text = "Fees\n " + investmentResults.TenYears.TotalFeesMaxReturn.ToString("C");
            lblFeesMinReturn10.Text = "Fees \n" + investmentResults.TenYears.TotalFeesMinReturn.ToString("C");

            lblProfitMinReturn10.Text = "Profit \n " + investmentResults.TenYears.TotalProfitMinReturn.ToString("C");
            lblProfitMaxReturn10.Text = "Profit \n " + investmentResults.TenYears.TotalProfitMaxReturn.ToString("C");

            lblTaxesMaxReturn10.Text = "Taxes \n" + investmentResults.TenYears.TotalTaxesMaxReturn.ToString("C");
            lblTaxesMinReturn10.Text = "Taxes \n" + investmentResults.TenYears.TotalTaxesMinReturn.ToString("C");
        }

        private void ClearInvestmentData()
        {
            // Clear the input fields
            txtInitialLump.Text = "";
            txtMontlyInvestment.Text = "";

            // Reset the investmentResults to null
            investmentResults = null;

            // Clear labels for One Year results
            lblTotalInvested1.Text = "";
            lblMaxReturn1.Text = "Max Return\n N/A";
            lblMinReturn1.Text = "Min Return \n N/A";
            lblFeesMaxReturn1.Text = "Fees \n N/A";
            lblFeesMinReturn1.Text = "Fees \n N/A";
            lblProfitMinReturn1.Text = "Profit \n N/A";
            lblProfitMaxReturn1.Text = "Profit \n N/A";
            lblTaxesMaxReturn1.Text = "Taxes \n N/A";
            lblTaxesMinReturn1.Text = "Taxes \n N/A";

            // Clear labels for Five Years results
            lblTotalInvested5.Text = "";
            lblMaxReturn5.Text = "Max Return \n N/A";
            lblMinReturn5.Text = "Min Return \n N/A";
            lblFeesMaxReturn5.Text = "Fees \n N/A";
            lblFeesMinReturn5.Text = "Fees \n N/A";
            lblProfitMinReturn5.Text = "Profit \n N/A";
            lblProfitMaxReturn5.Text = "Profit \n N/A";
            lblTaxesMaxReturn5.Text = "Taxes \n N/A";
            lblTaxesMinReturn5.Text = "Taxes \n N/A";

            // Clear labels for Ten Years results
            lblTotalInvested10.Text = "";
            lblMaxReturn10.Text = "Max Return \n N/A";
            lblMinReturn10.Text = "Min Return \n N/A";
            lblFeesMaxReturn10.Text = "Fees\n N/A";
            lblFeesMinReturn10.Text = "Fees \n N/A";
            lblProfitMinReturn10.Text = "Profit \n N/A";
            lblProfitMaxReturn10.Text = "Profit \n N/A";
            lblTaxesMaxReturn10.Text = "Taxes \n N/A";
            lblTaxesMinReturn10.Text = "Taxes \n N/A";
        }

      
            
        private void btnRetrieveQuotes_Click(object sender, EventArgs e)
        {                    
            if (SessionManager.CustomerId <= 0 || SessionManager.CustomerId == null)
            {
                 MessageBox.Show("Please select a client name before proceeding.", "Client Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return; // prevent retrieving the data without a selected client
             }
            
            Utility.PopulateDataGrid("investment_quotes", dgvQuotesInvestments, "Investments");
        }

        
    }
}
