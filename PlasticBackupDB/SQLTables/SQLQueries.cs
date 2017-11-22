using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLTables
{
    static class SQLQueries
    {
        public const string GET_ALL_TABLES = @"SELECT name FROM sqlite_temp_master WHERE type='table';";
    }
}
