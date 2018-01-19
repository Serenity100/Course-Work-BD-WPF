using System;
using System.Data.SqlClient;

namespace Course_Work_BD_WPF
{
    public class BadPassword : Exception {}
    public class SqlConnector
    {
        private String login;
        private String pass;
        public SqlConnection SqlConnection { get; private set; }

        public SqlConnector(String login, String pass)
        {
            this.login = login;
            this.pass = pass;
        }

        public void Connect()
        {
            SqlConnection = new SqlConnection("Server=MEMMACHINE;Database=BookShop;User Id=" + login + ";Password = " + pass + ";");

            try
            {
                SqlConnection.Open();
            }

            catch (Exception)
            {
                throw new BadPassword();
            }
        }

        public void Disconnect()
        {
            SqlConnection.Close();
        }
    }
}
