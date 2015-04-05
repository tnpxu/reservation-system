using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
namespace DatabaseConnection
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "192.168.1.22";
            database = "phpbooked";
            uid = "root";
            password = "sonitaloli";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void update(string ID)
        {
            string query = "UPDATE reservation_series SET status_id = 2,description=\"logout\" WHERE series_id =" + ID;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        private void updateLogin(string ID)
        {
            string query = "UPDATE reservation_series SET description=\"login\" WHERE series_id =" + ID;
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public string getComName(string ID)
        {
            string nameC = string.Empty;
            string query = "SELECT name FROM resources WHERE resource_id = \"" + ID + "\";";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader nameReader = cmd.ExecuteReader();
            while (nameReader.Read())
            {
                nameC = nameReader.GetString(0);
            }
            nameReader.Close();
            return nameC;

        }

        public string getStatus(string ID) {
            string status = string.Empty;
            string query = "SELECT status_id FROM reservation_series WHERE series_id = \"" + ID + "\";";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader nameReader = cmd.ExecuteReader();
            while (nameReader.Read())
            {
                status = nameReader.GetString(0);
            }
            nameReader.Close();
            return status;
        }

        //Select statement
        public bool Select(string nameID, string date)
        {   
            bool check = false;
            string query = "SELECT username,start_date,end_date,resource_id,series_id FROM (((users NATURAL JOIN reservation_users) NATURAL JOIN reservation_instances) NATURAL JOIN reservation_resources) WHERE username = \"" + nameID + "\"" + "AND end_date >=" + "\"" + date + "\"" + "AND start_date <=" + "\"" + date + "\" ORDER BY reservation_instance_id ASC;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!(dataReader.HasRows))
                {
                    MessageBox.Show("NOT FOUND RESERVATION OR USER \nPlease Check you're already to make a reservation.");
                    check = false;
                }
                //Read the data and store them
                else
                {
                    string uName = string.Empty;
                    string sDate = string.Empty;
                    string eDate = string.Empty;
                    string Rid = string.Empty;
                    string Sid = string.Empty;
                    
                    while (dataReader.Read())
                    {
                        uName = dataReader.GetString(0);
                        sDate = dataReader.GetString(1);
                        eDate = dataReader.GetString(2);
                        Rid = dataReader.GetString(3);
                        Sid = dataReader.GetString(4);                        
                    }
                    dataReader.Close();
                    string comName = getComName(Rid);
                    string status = getStatus(Sid);
                    if (status == "1")
                    {
                        if(comName == Environment.MachineName){
                            ////////////
                            updateLogin(Sid);
                            string userN = uName;
                            string startD = sDate;
                            string endD = eDate;
                            string comID = comName;
                            string statusID = status;
                            string[] lines = { userN, startD, endD, comID, statusID, Sid };
                            System.IO.File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "data.txt", lines);
                            ////////////  
                            check = true;
                        }
                        else
                        {
                            MessageBox.Show("WORNG COMPUTER\nYour computername is "+comName+".");
                        }
                    }
                    else
                    {
                        MessageBox.Show("RESERVATION IS TIMEOUT OR DELETED.");
                        check = false;
                    }
                }               
                dataReader.Close(); //close Data Reader                
                this.CloseConnection();//close Connection                
                return check;//return list to be displayed
            }
            else
            {
                return check;
            }
        }

        public void AdminSelect()
        {
            string query = "SELECT * FROM admin_account ;";
            if (this.OpenConnection() == true)
            { 
                MySqlCommand cmd = new MySqlCommand(query, connection);
           
                MySqlDataReader dataReader = cmd.ExecuteReader();
                string adminPass = "";
                while (dataReader.Read()) {
                    adminPass = dataReader.GetString(0);
                }
                    
                dataReader.Close();
                string[] lines = { adminPass };
                System.IO.File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "admin.txt", lines);
                this.CloseConnection();
            }
            

            

        }

    }
}
