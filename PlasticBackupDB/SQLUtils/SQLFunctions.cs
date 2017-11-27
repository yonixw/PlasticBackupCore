using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PlasticBackupDB.SQLUtils
{
    public class SQLFunctions 
    {
        private SQLFunctions() { }
        public SQLFunctions(SQLConnection conn)
        {
            myConnection = conn;
        }

        public SQLConnection myConnection { get; set; }
    }
}
