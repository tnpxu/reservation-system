using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reservation_System
{
    public partial class Clock : Form
    {
        public Clock()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;

            /*
            notifyIcon1.BalloonTipText = "Application is running";
            notifyIcon1.BalloonTipTitle = "Reservation System";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Icon = SystemIcons.Exclamation;
            notifyIcon1.ShowBalloonTip(10);
             */
            
            timer1.Interval = 1000;
            timer1.Start();
            file = new CheckFile();
            
        }

        private void menuItem1_Click(object sender, EventArgs e) { 
        }

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80; 
                return cp;
            }
        }

        /*
        private void notify_Onclick(object sender, EventArgs e)
        {
            Tray a = new Tray();
            a.Location = Cursor.Position;
            a.Show();
        }
        */
        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (!Program.session)
            {
                this.Close();
            }

            if (file.checkValidTime())
            {
                
                const int h = 0;
                const int m = 1;
                const int s = 2;
                DateTime now = DateTime.Now.AddHours(-7);
                string[] time = file.getEndDate().Split(':');
                int endHour = Convert.ToInt16(time[h]);
                int endMinute = Convert.ToInt16(time[m]);
                int endSecond = Convert.ToInt16(time[s]);
                int tempEndHour = Convert.ToInt16(time[h]);
                int tempEndMinute = Convert.ToInt16(time[m]);
                int tempEndSecond = Convert.ToInt16(time[s]);  
                int currHour = Convert.ToInt16(now.Hour);
                int currMinute = Convert.ToInt16(now.Minute);
                int currSecond = Convert.ToInt16(now.Second);
                long END = ((endHour * 3600) + (endMinute * 60) + endSecond) - ((currHour * 3600) + (currMinute * 60) + currSecond);
                string temp = END.ToString();
                
                int hourRemain = endHour - currHour ;
                int minuteRemain ;
                int secondRemain ;
                if (endMinute < currMinute)
                {
                    tempEndMinute += 60;
                    hourRemain--;
                    minuteRemain = tempEndMinute - currMinute;
                }
                else {
                    minuteRemain = tempEndMinute - currMinute;
                }
                if (endSecond < currSecond)
                {
                    tempEndSecond += 60;
                    minuteRemain--;
                    secondRemain = tempEndSecond - currSecond;
                }
                else {
                    secondRemain = tempEndSecond - currSecond;
                }
                remainHour.Text = hourRemain+"";
                remainMinute.Text = minuteRemain+"";
                remainSecond.Text = secondRemain+"";
                if (hourRemain == 0 && minuteRemain == 10 && secondRemain == 0)
                {
                    MessageBox.Show(new Form() { TopMost = true }, "10 minutes remaining!!");
                }
                if (hourRemain == 0 && minuteRemain == 0 && secondRemain == 0)
                {
                    //System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "data.txt");
                    this.Close();
                }
            }
            else
            {
                remainHour.Text = "not valid";
                timer1.Stop();
                this.Close();
            } 
            
        }

        private void Clock_Load(object sender, EventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = 0;
           
        }

        private void Clock_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exitForm = new Logout();
            exitForm.Show();
        }

        private void about_Click(object sender, EventArgs e)
        {
            aboutForm = new About();
            aboutForm.Show();
        }



    }
}
