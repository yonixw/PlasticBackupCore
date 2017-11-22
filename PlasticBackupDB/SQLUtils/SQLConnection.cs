using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace PlasticBackupDB.SQLUtils
{
    public class SQLConnection
    {
        internal SQLiteConnection myConnection { get; set; }

        private SQLConnection() { }

        public SQLConnection(string path)
        {
            myConnection = new SQLiteConnection("data source=\"" + path + "\"");
        }

        public void Open()
        {
            if (myConnection.State == System.Data.ConnectionState.Closed)
                myConnection.Open();
        }

        public void Close()
        {
            if (myConnection.State == System.Data.ConnectionState.Open)
                myConnection.Close();
        }

        public List<string> GetAllTables() {
            // RAW Data reading to check connection validity. Other go through SQLCommand

            List<string> result = new List<string>();

            SQLiteCommand com = new SQLiteCommand(SQLData.SQLQueries.GET_ALL_TABLES_RAW , myConnection);
            SQLiteDataReader reader = com.ExecuteReader();
            while(reader.Read())
            {
                result.Add(reader["name"] as string);
            }

            return result;
        }
    }
}
