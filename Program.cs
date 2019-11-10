using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace ATM_Sim
{

    // this is the top level control class
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BankSys());
        }
    }


    // this class controls the bank system
    public class BankSystem
    {
        private BankAccount[] accounts;
        private int ATMCount;

        // constructor
        public BankSystem()
        {
            accounts = new BankAccount[3];
            initialiseAccounts();
            ATMCount = 0;
        }

        // initalises the 3 accounts provided by the breif
        public void initialiseAccounts()
        {
            accounts[0] = new BankAccount(300, 1111, 111111);
            accounts[1] = new BankAccount(750, 2222, 222222);
            accounts[2] = new BankAccount(3000, 3333, 333333);
        }

        public int findAccount(int desiredAccount)
        {
            for (int i = 0; i<accounts.Length; i++)
            {
                if (accounts[i].getAccountNum() == desiredAccount)
                {
                    return i;
                }
            }

            return -999;
        }

        public void toggle()
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].toggleBool();
            }
        }

        public bool withdraw(int desiredAccount, int amount)
        {
            int index = findAccount(desiredAccount);

            if (index == -999)
            {
                return false;
            }

           return accounts[index].decrementBalence(amount);
        }

        public BankAccount[] getAccounts()
        {
            return accounts;
        }


    }


    public class BankAccount
    {
        private static Semaphore semaphore;

        private int accountNum;
        private float balence;
        private bool toggle;
        private int pin { get; set; }
        
        public override string ToString()
        {
            return accountNum + "\t\t" + balence + Environment.NewLine;
        }

        public BankAccount(float startBalance, int newPin, int accountNumber)
        {
            this.accountNum = accountNumber;
            this.balence = startBalance;
            this.pin = newPin;
            this.toggle = false;
            semaphore = new Semaphore(1, 1);
        }

        public bool decrementBalence(float value)
        {
            if (toggle)
            {
                semaphore.WaitOne();
            }
            if (balence > value)
            {
                Debug.Write("Hello");
                float temp = balence;
                Thread.Sleep(10000);
                balence = temp - value;
                if (toggle)
                {
                    semaphore.Release();
                }
                return true;
            }
            else
            {
                Debug.Write("Not enough money.");
                if (toggle)
                {
                    semaphore.Release();
                }
                return false;
            }
        }

        public bool checkPin(int enterredPin)
        {
            if (pin == enterredPin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getAccountNum()
        {
            return this.accountNum;
        }

        public float getBalance()
        {
            return this.balence;
        }

        public bool isToggle()
        {
            return this.toggle;
        }

        public void toggleBool()
        {
            this.toggle = !this.toggle;
        }


    }
}
