namespace TestProject1
{
    partial class MainMenu
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
            this.btnConvertor = new System.Windows.Forms.Button();
            this.btnInvestments = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConvertor
            // 
            this.btnConvertor.Location = new System.Drawing.Point(196, 89);
            this.btnConvertor.Name = "btnConvertor";
            this.btnConvertor.Size = new System.Drawing.Size(157, 46);
            this.btnConvertor.TabIndex = 0;
            this.btnConvertor.Text = "Currency Convertor";
            this.btnConvertor.UseVisualStyleBackColor = true;
            this.btnConvertor.Click += new System.EventHandler(this.btnConvertor_Click);
            // 
            // btnInvestments
            // 
            this.btnInvestments.Location = new System.Drawing.Point(196, 152);
            this.btnInvestments.Name = "btnInvestments";
            this.btnInvestments.Size = new System.Drawing.Size(157, 50);
            this.btnInvestments.TabIndex = 1;
            this.btnInvestments.Text = "Investments";
            this.btnInvestments.UseVisualStyleBackColor = true;
            this.btnInvestments.Click += new System.EventHandler(this.btnInvestments_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(196, 217);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(157, 50);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 358);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInvestments);
            this.Controls.Add(this.btnConvertor);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConvertor;
        private System.Windows.Forms.Button btnInvestments;
        private System.Windows.Forms.Button btnExit;
    }
}
