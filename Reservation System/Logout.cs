using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseConnection;
namespace Reservation_System
{
    public partial class Logout : Form
    {
        public Logout()
        {
            InitializeComponent();
            connect = new DBConnect();
            file = new CheckFile();
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            Program.session = false;
            System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "data.txt");
            connect.update(file.getSID());
            this.Close();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
