using DatabaseConnection;
using System;
namespace Reservation_System
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.idInput = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.account_swap = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "STUDENT ID";
            // 
            // idInput
            // 
            this.idInput.Location = new System.Drawing.Point(95, 6);
            this.idInput.MaxLength = 10;
            this.idInput.Name = "idInput";
            this.idInput.Size = new System.Drawing.Size(149, 20);
            this.idInput.TabIndex = 2;
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(95, 32);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(75, 23);
            this.login.TabIndex = 3;
            this.login.Text = "Log in";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.account_swap);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.login);
            this.panel1.Controls.Add(this.idInput);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 83);
            this.panel1.TabIndex = 5;
            // 
            // account_swap
            // 
            this.account_swap.AutoSize = true;
            this.account_swap.Location = new System.Drawing.Point(204, 57);
            this.account_swap.Name = "account_swap";
            this.account_swap.Size = new System.Drawing.Size(77, 13);
            this.account_swap.TabIndex = 4;
            this.account_swap.TabStop = true;
            this.account_swap.Text = "Account Swap";
            this.account_swap.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idInput;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.Panel panel1;
        private DBConnect databaseConnect;
        private System.Windows.Forms.LinkLabel account_swap;
    }
}

