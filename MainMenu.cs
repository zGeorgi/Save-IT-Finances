using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProject1
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnConvertor_Click(object sender, EventArgs e)
        {
            Currency_Convertor nextForm = new Currency_Convertor();
            this.Hide();
            nextForm.ShowDialog();
            //You may also want to close the first form
            this.Close();
        }

        private void btnInvestments_Click(object sender, EventArgs e)
        {
            Investments nextForm = new Investments();
            this.Hide();
            nextForm.ShowDialog();
            //You may also want to close the first form
            this.Close();
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
