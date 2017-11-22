﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace PlasticBackupDB.SQLTables
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
    }
}