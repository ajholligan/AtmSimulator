using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ATM_Sim
{

    //this class Controls the gui for the each atm
    public partial class ATMForm : Form
    {
        bool loggedIn;
        bool enteredPin;
        int accountIndex;
        bool menu;
        bool withdraw;
        bool enteringPin;
        bool waitingToContinue;
        string userInput;
        BankSystem bankSystem;
        Thread atmThread;
        static Label staticLabel;
        static int timerTicks;
        //static System.Windows.Forms.Timer timer;

        // constructor with reference to a bank system
        public ATMForm(ref BankSystem bankSys)
        {
            InitializeComponent();
            initializeButtons();
            displayPrompt();
            bankSystem = bankSys;
            staticLabel = new Label();
            Controls.Add(staticLabel);
            staticLabel.Hide();
            //staticLabel.ForeColor = Window;
            staticLabel.Location = new System.Drawing.Point(131, 220);
            staticLabel.Size = new System.Drawing.Size(89, 28);
            staticLabel.Text = "Continue";
            staticLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f);
            staticLabel.BringToFront();
            timerTicks = 0;
        }

        // initalises the buttons in the num pad to have a shared event handler
        void initializeButtons()
        {
            button1.Click += myEventHandler;
            button2.Click += myEventHandler;
            button3.Click += myEventHandler;
            button4.Click += myEventHandler;
            button5.Click += myEventHandler;
            button6.Click += myEventHandler;
            button7.Click += myEventHandler;
            button8.Click += myEventHandler;
            button9.Click += myEventHandler;
            button13.Click += myEventHandler;
        }

        // event handler foe the number buttons
        void myEventHandler(object sender, EventArgs e)
        {
            
            userInput += ((Button)sender).Text;

            string value = "";
            value = userInput;
            // stars characters if the user is entering their pin
            if (enteringPin)
            {
                lblUserInput.Text = "";
                foreach (char c in value)
                    lblUserInput.Text += "*";
            }
            else
            {
                // non pin gui update
                lblUserInput.Text = userInput;
            }
                
        }

        //dispalys appropriate menu depending on progress through program
        void displayPrompt()
        {
            lblUserInput.Text = "";
            if (!loggedIn)
            {
                displayAccountNumberPrompt();
            }
            else if (!enteredPin)
            {
                displayPinPrompt();
            }
            else if (enteredPin)
            {
                displayAtmMenu();
            }
        }

        //the menu to ask for account number
        void displayAccountNumberPrompt()
        {
            atmPromptLabel.Text = "Please enter the account number" + Environment.NewLine;
            clearLabels();
        }

        // the menu to ask for pin
        void displayPinPrompt()
        {
            enteringPin = true;
            atmPromptLabel.Text = "Please enter your pin number" + Environment.NewLine;
        }


        // decides what to do when confirm is selected
        void handleConfirm(string userInput)
        {
            //checks if user is logged in
            if (!loggedIn)
            {
                // checks for account
                int tempAccountNumber = bankSystem.findAccount(Convert.ToInt32(userInput));

                if (tempAccountNumber != -999)
                {
                    //displaays next menu
                  
                    loggedIn = true;
                    accountIndex = tempAccountNumber;
                    Debug.WriteLine("AccountNumber = " + userInput);
                     this.userInput = "";
                    displayPrompt();
                    
                }
                else
                {
                    errorLabel.Text = "Invalid account number please try again";
                    Debug.WriteLine("Invalid account number");
                    //add error state to methods
                }
                //check existing account if yes store number, display next step
            }
            // checks if pin has been entered and verified
            else if (!enteredPin)
            {
                // checks the pin
                if (bankSystem.getAccounts()[accountIndex].checkPin(Convert.ToInt32(userInput)))
                {
                    this.userInput = "";
                    enteredPin = true;
                    Debug.WriteLine("succesfully logged in.");
                    displayPrompt();
                }
                else
                {
                    errorLabel.Text = "Invalid pin please try again";
                    Debug.WriteLine("Invalid pin");
                }

            }
            else
            {

            }

        }


        //confirm button
        private void button10_Click(object sender, EventArgs e)
        {
            handleConfirm(userInput);
        }

        // displays inital menu when first logged in
        void displayAtmMenu()
        {
            clearLabels();
            atmPromptLabel.Text = "Please Select an Option From Below";
            selLbl1.Text = "Withdraw";
            selLbl2.Text = "View Balance";
            selLbl3.Text = "Exit";
        }

        // withdraw amount menu
        void atmWithdrawMen()
        {
            selLbl1.Text = "£5";
            selLbl2.Text = "£10";
            selLbl3.Text = "£20";
            selLbl4.Text = "£50";
            selLbl5.Text = "£100";
            selLbl6.Text = "£200";
        }

        // dispalys account balance 
        void displayBalance()
        {
            atmPromptLabel.Text = "Your Current Balence is:";
            selLbl2.Text = Convert.ToString(bankSystem.getAccounts()[accountIndex].getBalance());
            selLbl3.Text = "Continue";
        }

       
        // handles select continue button being pressed
        void selectBtnCont(int num)
        {
            // checks if waiting
            if (waitingToContinue)
            {
                // checks contine button was pressed
                if (num == 20)
                {
                    displayAtmMenu();
                    waitingToContinue = false;
                }
            }
            // checks if in withdraw menu
            else if (withdraw)
            {

                //update screen to say you have done whatever
                displayCashWithdrawn(num);
                // starts new thread for this atm to withdraw
                WithdrawThreadWithState wdThreadState = new WithdrawThreadWithState(num,ref bankSystem, accountIndex);
                ThreadStart t = new ThreadStart(wdThreadState.withdrawCash);
                atmThread = new Thread(t);
                atmThread.Start();

               
                waitingToContinue = true;
                //update bankSystemForm
                //on key press or something return to atm menu
                withdraw = false;

            }
            // checks if pin has been enterd 
            else if (enteredPin)
            {
                // if top left button is pressed
                if (num == 5)
                {
                    atmWithdrawMen();
                    withdraw = true;

                }
                // if middle left button is pressed
                else if (num == 10)
                {
                    clearLabels();
                    waitingToContinue = true;
                    displayBalance();
                }
               //  if middle left button is pressed
                else if (num == 20)
                {
                    //close window
                    displayGoodbye();

                    this.Close();
                    System.Threading.Thread.Sleep(1000);
                }
            }

        }

        // displays goodbye message
        void displayGoodbye()
        {
            clearLabels();
            atmPromptLabel.Text = "Thank you for using our services. ";
        }


        //displays the value of cash withdrawn 
        void displayCashWithdrawn(int num)
        {
            clearLabels();

            if (bankSystem.getAccounts()[accountIndex].getBalance() < num)
            {
                errorLabel.Text = "Insufficient Funds";
                Debug.WriteLine("Insufficient Funds.");
                staticLabel.Show();
            }
            else
            {

                // Create a timer and set a two second interval.
                timerTicks = 0;
                timer.Start();

                //atmPromptLabel.Text = "Please wait while we process your request";
                //   progressBar.Hide();
                progressBar.Value = 0;

                //displayWithdrawn();

                atmPromptLabel.Text = "You have withdrawn £" + num;
                Debug.WriteLine("You have withdrawn £" + num);
                //selLbl3.Text = "Continue";
                withdrawCashGif.Show();
            }
        }

        //clears all the lables on screen
        void clearLabels()
        {
            selLbl1.Text = "";
            selLbl2.Text = "";
            selLbl3.Text = "";
            selLbl4.Text = "";
            selLbl5.Text = "";
            selLbl6.Text = "";
            userInput = "";
            errorLabel.Text = "";
            withdrawCashGif.Hide();
        }

        private void select1_Click(object sender, EventArgs e)
        {
            selectBtnCont(5);
        }

        private void select2_Click(object sender, EventArgs e)
        {
            selectBtnCont(10);
        }

        private void select3_Click(object sender, EventArgs e)
        {
            selectBtnCont(20);
            staticLabel.Hide();
            progressBar.Hide();
        }

        private void select4_Click(object sender, EventArgs e)
        {
            selectBtnCont(50);
        }

        private void select5_Click(object sender, EventArgs e)
        {
            selectBtnCont(100);
        }

        private void select6_Click(object sender, EventArgs e)
        {
            selectBtnCont(200);
        }

        private void ATMForm_Load(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            lblUserInput.Text = "";
            userInput = "";
        }

        private void process1_Exited(object sender, EventArgs e)
        {

        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
            atmPromptLabel.Text = "Thank you for using our services";
            this.Close();

        }

        private void progressBar_Click_1(object sender, EventArgs e)
        {

        }

        private void timer_Tick_1(object sender, EventArgs e)
        {
            progressBar.Increment(1);
            timerTicks++;

            if (timerTicks > 100)
            {
                timer.Stop();
                staticLabel.Show();
                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

    public class WithdrawThreadWithState
    {
        int num;
        BankSystem bankSystem;
        int accountIndex;

        public WithdrawThreadWithState(int number, ref BankSystem banksys, int index)
        {
            this.num = number;
            this.bankSystem = banksys;
            accountIndex = index;
        }

        public void withdrawCash()
        {
            bankSystem.getAccounts()[accountIndex].decrementBalence(num);
        }

    }
}
