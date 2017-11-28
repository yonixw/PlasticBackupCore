using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    static class SQLQueriesRaw
    {
        public const string GET_ALL_TABLES_RAW = @"SELECT name FROM sqlite_master WHERE type='table';";
        public const string CLEAR_SEQUENCES = @"DELETE FROM sqlite_sequence;";
    }
}
