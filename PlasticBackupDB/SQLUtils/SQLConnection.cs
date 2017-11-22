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
        private SQLiteConnection myConnection { get; set; }

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
            List<string> result = new List<string>();

            SQLiteCommand com = new SQLiteCommand(SQLQueries.GET_ALL_TABLES , myConnection);
            SQLiteDataReader reader = com.ExecuteReader();
            while(reader.Read())
            {
                result.Add(reader["name"] as string);
            }

            return result;
        }

        public int RunNonScalar(string SQL)
        {
            SQLiteCommand com = new SQLiteCommand(SQL, myConnection);
            return com.ExecuteNonQuery();
        }

        public List<T> ReadAll<T>(string SQL, Func<DbDataReader,T> readFunc)
        {
            List<T> result = new List<T>();

            SQLiteCommand com = new SQLiteCommand(SQL, myConnection);
            SQLiteDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                result.Add(readFunc(reader));
            }

            return result;
        }


    }
}
