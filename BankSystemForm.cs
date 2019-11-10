using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ATM_Sim
{
    //This class is a bank form
    public partial class BankSys : Form
    {
        BankSystem bankSystem;

        //constructor
        public BankSys()
        {
            InitializeComponent();
            bankSystem = new BankSystem();
            updateAccountsDisplay();
        }

        // adds button to make new atms
        private void addAtmButton_Click(object sender, EventArgs e)
        {
            ATMForm atmForm = new ATMForm(ref bankSystem);
            atmForm.Show();
        }

        //updates the accounts in on the form
        private void updateAccountsDisplay()
        {
            String updated = "Account Number\tBalance" + Environment.NewLine;
            BankAccount[] accounts = bankSystem.getAccounts();

            for (int i = 0; i < accounts.Length;i++)
            {
                updated += accounts[i].ToString();
            }
            accountsDisplay.Text = updated;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toggleButton_Click(object sender, EventArgs e)
        {
            bankSystem.toggle();
            Debug.WriteLine("toggling semaphore to: " + bankSystem.getAccounts()[0].isToggle());
        }
    }
}
