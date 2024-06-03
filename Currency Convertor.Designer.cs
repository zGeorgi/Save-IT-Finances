namespace TestProject1
{
    partial class Currency_Convertor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblClientName = new System.Windows.Forms.Label();
            this.lblCurrency1 = new System.Windows.Forms.Label();
            this.lblCurrency2 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.cboClientName = new System.Windows.Forms.ComboBox();
            this.cboCurrency1 = new System.Windows.Forms.ComboBox();
            this.cboCurrency2 = new System.Windows.Forms.ComboBox();
            this.txtConversionRate = new System.Windows.Forms.TextBox();
            this.lblConversationRate = new System.Windows.Forms.Label();
            this.btnAddClient = new System.Windows.Forms.Button();
            this.btnSaveQuote = new System.Windows.Forms.Button();
            this.txtCurrency1Amount = new System.Windows.Forms.TextBox();
            this.txtCurrency2Amount = new System.Windows.Forms.TextBox();
            this.lblAmounts = new System.Windows.Forms.Label();
            this.btnMainMenu = new System.Windows.Forms.Button();
            this.lblMinimumTransaction = new System.Windows.Forms.Label();
            this.lblMaxTransaction = new System.Windows.Forms.Label();
            this.lblFees = new System.Windows.Forms.Label();
            this.lblFee1 = new System.Windows.Forms.Label();
            this.txtFinalAmount = new System.Windows.Forms.Label();
            this.dgvQuotesConrvertor = new System.Windows.Forms.DataGridView();
            this.btnRetrieveQuote = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuotesConrvertor)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(12, 63);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(64, 13);
            this.lblClientName.TabIndex = 0;
            this.lblClientName.Text = "Client Name";
            // 
            // lblCurrency1
            // 
            this.lblCurrency1.AutoSize = true;
            this.lblCurrency1.Location = new System.Drawing.Point(12, 103);
            this.lblCurrency1.Name = "lblCurrency1";
            this.lblCurrency1.Size = new System.Drawing.Size(58, 13);
            this.lblCurrency1.TabIndex = 1;
            this.lblCurrency1.Text = "Currency 1";
            // 
            // lblCurrency2
            // 
            this.lblCurrency2.AutoSize = true;
            this.lblCurrency2.Location = new System.Drawing.Point(12, 143);
            this.lblCurrency2.Name = "lblCurrency2";
            this.lblCurrency2.Size = new System.Drawing.Size(58, 13);
            this.lblCurrency2.TabIndex = 2;
            this.lblCurrency2.Text = "Currency 2";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(316, 169);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(136, 23);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // cboClientName
            // 
            this.cboClientName.FormattingEnabled = true;
            this.cboClientName.Location = new System.Drawing.Point(100, 63);
            this.cboClientName.Name = "cboClientName";
            this.cboClientName.Size = new System.Drawing.Size(175, 21);
            this.cboClientName.TabIndex = 7;
            this.cboClientName.SelectedIndexChanged += new System.EventHandler(this.cboClientName_SelectedIndexChanged);
            // 
            // cboCurrency1
            // 
            this.cboCurrency1.FormattingEnabled = true;
            this.cboCurrency1.Location = new System.Drawing.Point(100, 103);
            this.cboCurrency1.Name = "cboCurrency1";
            this.cboCurrency1.Size = new System.Drawing.Size(58, 21);
            this.cboCurrency1.TabIndex = 8;
            this.cboCurrency1.SelectedIndexChanged += new System.EventHandler(this.cboCurrency1_SelectedIndexChanged);
            // 
            // cboCurrency2
            // 
            this.cboCurrency2.FormattingEnabled = true;
            this.cboCurrency2.Location = new System.Drawing.Point(100, 140);
            this.cboCurrency2.Name = "cboCurrency2";
            this.cboCurrency2.Size = new System.Drawing.Size(58, 21);
            this.cboCurrency2.TabIndex = 9;
            this.cboCurrency2.SelectedIndexChanged += new System.EventHandler(this.cboCurrency2_SelectedIndexChanged);
            // 
            // txtConversionRate
            // 
            this.txtConversionRate.Location = new System.Drawing.Point(175, 141);
            this.txtConversionRate.Name = "txtConversionRate";
            this.txtConversionRate.Size = new System.Drawing.Size(100, 20);
            this.txtConversionRate.TabIndex = 11;
            this.txtConversionRate.TextChanged += new System.EventHandler(this.txtConversionRate_TextChanged);
            // 
            // lblConversationRate
            // 
            this.lblConversationRate.AutoSize = true;
            this.lblConversationRate.Location = new System.Drawing.Point(173, 115);
            this.lblConversationRate.Name = "lblConversationRate";
            this.lblConversationRate.Size = new System.Drawing.Size(86, 13);
            this.lblConversationRate.TabIndex = 12;
            this.lblConversationRate.Text = "Conversion Rate";
            // 
            // btnAddClient
            // 
            this.btnAddClient.Location = new System.Drawing.Point(15, 17);
            this.btnAddClient.Name = "btnAddClient";
            this.btnAddClient.Size = new System.Drawing.Size(134, 23);
            this.btnAddClient.TabIndex = 13;
            this.btnAddClient.Text = "Add Client";
            this.btnAddClient.UseVisualStyleBackColor = true;
            this.btnAddClient.Click += new System.EventHandler(this.btnAddClient_Click);
            // 
            // btnSaveQuote
            // 
            this.btnSaveQuote.Location = new System.Drawing.Point(461, 169);
            this.btnSaveQuote.Name = "btnSaveQuote";
            this.btnSaveQuote.Size = new System.Drawing.Size(134, 23);
            this.btnSaveQuote.TabIndex = 14;
            this.btnSaveQuote.Text = "Save the Quote";
            this.btnSaveQuote.UseVisualStyleBackColor = true;
            this.btnSaveQuote.Click += new System.EventHandler(this.btnSaveQuote_Click);
            // 
            // txtCurrency1Amount
            // 
            this.txtCurrency1Amount.Location = new System.Drawing.Point(342, 108);
            this.txtCurrency1Amount.Name = "txtCurrency1Amount";
            this.txtCurrency1Amount.Size = new System.Drawing.Size(136, 20);
            this.txtCurrency1Amount.TabIndex = 15;
            // 
            // txtCurrency2Amount
            // 
            this.txtCurrency2Amount.Location = new System.Drawing.Point(342, 140);
            this.txtCurrency2Amount.Name = "txtCurrency2Amount";
            this.txtCurrency2Amount.Size = new System.Drawing.Size(136, 20);
            this.txtCurrency2Amount.TabIndex = 16;
            // 
            // lblAmounts
            // 
            this.lblAmounts.AutoSize = true;
            this.lblAmounts.Location = new System.Drawing.Point(288, 124);
            this.lblAmounts.Name = "lblAmounts";
            this.lblAmounts.Size = new System.Drawing.Size(48, 13);
            this.lblAmounts.TabIndex = 17;
            this.lblAmounts.Text = "Amounts";
            // 
            // btnMainMenu
            // 
            this.btnMainMenu.Location = new System.Drawing.Point(164, 17);
            this.btnMainMenu.Name = "btnMainMenu";
            this.btnMainMenu.Size = new System.Drawing.Size(137, 23);
            this.btnMainMenu.TabIndex = 18;
            this.btnMainMenu.Text = "Main Menu";
            this.btnMainMenu.UseVisualStyleBackColor = true;
            this.btnMainMenu.Click += new System.EventHandler(this.btnMainMenu_Click);
            // 
            // lblMinimumTransaction
            // 
            this.lblMinimumTransaction.AutoSize = true;
            this.lblMinimumTransaction.Location = new System.Drawing.Point(339, 22);
            this.lblMinimumTransaction.Name = "lblMinimumTransaction";
            this.lblMinimumTransaction.Size = new System.Drawing.Size(125, 13);
            this.lblMinimumTransaction.TabIndex = 19;
            this.lblMinimumTransaction.Text = "Minimun transaction: 200";
            this.lblMinimumTransaction.Click += new System.EventHandler(this.lblMinimumTransaction_Click);
            // 
            // lblMaxTransaction
            // 
            this.lblMaxTransaction.AutoSize = true;
            this.lblMaxTransaction.Location = new System.Drawing.Point(338, 63);
            this.lblMaxTransaction.Name = "lblMaxTransaction";
            this.lblMaxTransaction.Size = new System.Drawing.Size(140, 13);
            this.lblMaxTransaction.TabIndex = 20;
            this.lblMaxTransaction.Text = "Maximum Transaction: 5000";
            this.lblMaxTransaction.Click += new System.EventHandler(this.lblMaxTransaction_Click);
            // 
            // lblFees
            // 
            this.lblFees.AutoSize = true;
            this.lblFees.Location = new System.Drawing.Point(510, 22);
            this.lblFees.Name = "lblFees";
            this.lblFees.Size = new System.Drawing.Size(33, 13);
            this.lblFees.TabIndex = 21;
            this.lblFees.Text = "Fees:";
            // 
            // lblFee1
            // 
            this.lblFee1.AutoSize = true;
            this.lblFee1.Location = new System.Drawing.Point(510, 51);
            this.lblFee1.Name = "lblFee1";
            this.lblFee1.Size = new System.Drawing.Size(57, 13);
            this.lblFee1.TabIndex = 22;
            this.lblFee1.Text = "Fees data ";
            this.lblFee1.Click += new System.EventHandler(this.lblFee1_Click);
            // 
            // txtFinalAmount
            // 
            this.txtFinalAmount.AutoSize = true;
            this.txtFinalAmount.Location = new System.Drawing.Point(500, 144);
            this.txtFinalAmount.Name = "txtFinalAmount";
            this.txtFinalAmount.Size = new System.Drawing.Size(95, 13);
            this.txtFinalAmount.TabIndex = 23;
            this.txtFinalAmount.Text = "Amount after Fee\'s";
            // 
            // dgvQuotesConrvertor
            // 
            this.dgvQuotesConrvertor.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dgvQuotesConrvertor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuotesConrvertor.Location = new System.Drawing.Point(12, 229);
            this.dgvQuotesConrvertor.Name = "dgvQuotesConrvertor";
            this.dgvQuotesConrvertor.Size = new System.Drawing.Size(903, 285);
            this.dgvQuotesConrvertor.TabIndex = 24;
            // 
            // btnRetrieveQuote
            // 
            this.btnRetrieveQuote.Location = new System.Drawing.Point(15, 200);
            this.btnRetrieveQuote.Name = "btnRetrieveQuote";
            this.btnRetrieveQuote.Size = new System.Drawing.Size(134, 23);
            this.btnRetrieveQuote.TabIndex = 25;
            this.btnRetrieveQuote.Text = "Retrieve Quotes";
            this.btnRetrieveQuote.UseVisualStyleBackColor = true;
            this.btnRetrieveQuote.Click += new System.EventHandler(this.btnRetrieveQuote_Click);
            // 
            // Currency_Convertor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 526);
            this.Controls.Add(this.btnRetrieveQuote);
            this.Controls.Add(this.dgvQuotesConrvertor);
            this.Controls.Add(this.txtFinalAmount);
            this.Controls.Add(this.lblFee1);
            this.Controls.Add(this.lblFees);
            this.Controls.Add(this.lblMaxTransaction);
            this.Controls.Add(this.lblMinimumTransaction);
            this.Controls.Add(this.btnMainMenu);
            this.Controls.Add(this.lblAmounts);
            this.Controls.Add(this.txtCurrency2Amount);
            this.Controls.Add(this.txtCurrency1Amount);
            this.Controls.Add(this.btnSaveQuote);
            this.Controls.Add(this.btnAddClient);
            this.Controls.Add(this.lblConversationRate);
            this.Controls.Add(this.txtConversionRate);
            this.Controls.Add(this.cboCurrency2);
            this.Controls.Add(this.cboCurrency1);
            this.Controls.Add(this.cboClientName);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.lblCurrency2);
            this.Controls.Add(this.lblCurrency1);
            this.Controls.Add(this.lblClientName);
            this.Name = "Currency_Convertor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Currency Convertor";
            this.Load += new System.EventHandler(this.Currency_Convertor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuotesConrvertor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblCurrency1;
        private System.Windows.Forms.Label lblCurrency2;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.ComboBox cboClientName;
        private System.Windows.Forms.ComboBox cboCurrency1;
        private System.Windows.Forms.ComboBox cboCurrency2;
        private System.Windows.Forms.TextBox txtConversionRate;
        private System.Windows.Forms.Label lblConversationRate;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.Button btnSaveQuote;
        private System.Windows.Forms.TextBox txtCurrency1Amount;
        private System.Windows.Forms.TextBox txtCurrency2Amount;
        private System.Windows.Forms.Label lblAmounts;
        private System.Windows.Forms.Button btnMainMenu;
        private System.Windows.Forms.Label lblMinimumTransaction;
        private System.Windows.Forms.Label lblMaxTransaction;
        private System.Windows.Forms.Label lblFees;
        private System.Windows.Forms.Label lblFee1;
        private System.Windows.Forms.Label txtFinalAmount;
        private System.Windows.Forms.DataGridView dgvQuotesConrvertor;
        private System.Windows.Forms.Button btnRetrieveQuote;
    }
}