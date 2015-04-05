using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation_System
{
    class CheckFile
    {
        public static String[] lines;
        public CheckFile()
        {
            lines = new String[5];
            lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "data.txt");
        }

        public CheckFile(string file)
        {
            lines = new String[1];
            lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + file);
        }

        public string getSID()
        {
            return lines[5];
        }

        public bool checkValidTime()
        {
            string temp = DateTime.Now.AddHours(-7) + "";
            if (temp.CompareTo(lines[1]) >= 0 && temp.CompareTo(lines[2]) <= 0)
            {
                return true;
            }
            return false;


        }

        public string getEndDate()
        {
            string date = "";
            try
            {
                date = lines[2].Substring(10, 8);
            }
            catch (Exception e)
            {
                date = lines[2].Substring(10, 7);
            }
            return date;
        }

        public string checkAdmin()
        {
            return lines[0];
        }
    }
}
