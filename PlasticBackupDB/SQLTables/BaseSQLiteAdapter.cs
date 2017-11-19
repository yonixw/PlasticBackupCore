using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace PlasticBackupDB.SQLTables
{
    public class BaseSQLiteAdapter
    {

        public SQLiteConnection myConnection { get; set; }

        private BaseSQLiteAdapter() { }

        public BaseSQLiteAdapter(SQLiteConnection conn)
        {
            myConnection = conn;
        }

    }
}
