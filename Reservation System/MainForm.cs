using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
using DatabaseConnection;
namespace Reservation_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.SlateGray;
            //this.BackgroundImage = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "Advertisemant.jpg");
            //this.BackgroundImageLayout = ImageLayout.Stretch;
            Taskbar.Hide();
            panel1.BackColor = Color.YellowGreen;
            panel1.Left = (Screen.PrimaryScreen.Bounds.Width - panel1.Width) / 2;
            panel1.Top = (Screen.PrimaryScreen.Bounds.Height - panel1.Height) / 2;
            //disableTaskManager();
            TopMost = true;           
        }

        public delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32", EntryPoint = "CallNextHookEx", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        public const int WH_KEYBOARD_LL = 13;

        /*code needed to disable start menu*/
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        public static IntPtr intLLKey;

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            bool blnEat = false;
            try
            {
                switch (wParam.ToInt64())
                {
                    case 256:
                    case 257:
                    case 260:
                    case 261:
                        //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key,
                        blnEat = ((lParam.vkCode == 9) && (lParam.flags == 32))  // alt+tab
                            | ((lParam.vkCode == 27) && (lParam.flags == 32)) // alt+esc
                            | ((lParam.vkCode == 27) && (lParam.flags == 0))  // ctrl+esc
                            | ((lParam.vkCode == 91) && (lParam.flags == 1))  // left winkey
                            | ((lParam.vkCode == 115) && (lParam.flags == 32)) //alt+F4  
                            | ((lParam.vkCode == 92) && (lParam.flags == 1))
                            | ((lParam.vkCode == 92) && (lParam.flags == 1))
                            | ((lParam.vkCode == 73) && (lParam.flags == 0));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (blnEat == true)
            {
                return (IntPtr)1;
            }
            else
            {
                return CallNextHookEx(intLLKey, nCode, wParam, ref lParam);
            }
        }

        public static void disableTaskManager()
        {
            RegistryKey regkey;
            string keyValueInt = "1";
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(subKey);
                regkey.SetValue("DisableTaskMgr", keyValueInt);
                regkey.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void enableTaskManager()
        {
            try
            {
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
            {
                intLLKey = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardProc, GetModuleHandle(curModule.ModuleName), 0);
            }
            if (intLLKey.ToInt64() == 0)
            {
                throw new Win32Exception();
            }
        }
        private void login_Click(object sender, EventArgs e){
            string temp = idInput.Text;
            if (Program.isAdmin)
            {
                CheckFile file = new CheckFile("admin.txt");
                if(file.checkAdmin().Equals(temp))
                {
                    enableTaskManager();
                    UnhookWindowsHookEx(intLLKey);
                    Taskbar.Show();
                    this.Close();
                }else
                {
                    Program.session = false;
                }
            }
            else if (temp.Equals(""))
            {
                MessageBox.Show("USERNAME IS REQUIRED");
            }
            else
            {
                long n;
                if (temp.Length != 10 || !long.TryParse(temp, out n))
                {
                    MessageBox.Show("USERNAME IS NOT VALID");
                }
                else
                {
                    Program.username = temp;
                    string date = getCurrentDate();
                    databaseConnect = new DBConnect();
                    if (databaseConnect.Select(Program.username, date))
                    {
                        enableTaskManager();
                        UnhookWindowsHookEx(intLLKey);
                        Taskbar.Show();
                        this.Close();
                    }
                    else
                    {
                        //MessageBox.Show("Fail to Login");
                        //enableTaskManager();
                        //UnhookWindowsHookEx(intLLKey);
                        //Taskbar.Show();
                        Program.session = false;
                        this.Close();
                    }
                }

            }
            
        }

        public string getCurrentDate() {
            DateTime tempTime = DateTime.Now.AddHours(-7);
            string time = tempTime.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));
            string year = time.Substring(6, 4);
            string month = time.Substring(3, 2);
            string day = time.Substring(0, 2);
            string startHour = time.Substring(11, 2);
            string startMinute = time.Substring(14, 2);
            string startSecond = time.Substring(17, 2);
            string fullDateTime = year + "-" + month + "-" + day + " " + startHour + ":" + startMinute + ":" + startSecond + ".000000";
            return fullDateTime;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            if(Program.isAdmin)
            {
                label1.Text = "STUDENT ID";
                Program.isAdmin = false;
            } else
            {
                label1.Text = "ADMIN";
                Program.isAdmin = true;
                    
            }
            
        }
    }
}
