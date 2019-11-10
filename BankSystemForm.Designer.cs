namespace ATM_Sim
{
    partial class BankSys
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
            this.addAtmButton = new System.Windows.Forms.Button();
            this.accountsDisplay = new System.Windows.Forms.TextBox();
            this.toggleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addAtmButton
            // 
            this.addAtmButton.Location = new System.Drawing.Point(333, 314);
            this.addAtmButton.Name = "addAtmButton";
            this.addAtmButton.Size = new System.Drawing.Size(123, 57);
            this.addAtmButton.TabIndex = 0;
            this.addAtmButton.Text = "Add ATM";
            this.addAtmButton.UseVisualStyleBackColor = true;
            this.addAtmButton.Click += new System.EventHandler(this.addAtmButton_Click);
            // 
            // accountsDisplay
            // 
            this.accountsDisplay.Location = new System.Drawing.Point(92, 65);
            this.accountsDisplay.Multiline = true;
            this.accountsDisplay.Name = "accountsDisplay";
            this.accountsDisplay.ReadOnly = true;
            this.accountsDisplay.Size = new System.Drawing.Size(434, 212);
            this.accountsDisplay.TabIndex = 1;
            this.accountsDisplay.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // toggleButton
            // 
            this.toggleButton.Location = new System.Drawing.Point(170, 314);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(123, 57);
            this.toggleButton.TabIndex = 2;
            this.toggleButton.Text = "Toggle Semaphore";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);
            // 
            // BankSys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 453);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.accountsDisplay);
            this.Controls.Add(this.addAtmButton);
            this.Name = "BankSys";
            this.Text = "Ba";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addAtmButton;
        private System.Windows.Forms.TextBox accountsDisplay;
        private System.Windows.Forms.Button toggleButton;
    }
}

