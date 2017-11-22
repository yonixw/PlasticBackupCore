using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLUtils
{
    static class SQLQueries
    {
        public const string GET_ALL_TABLES = @"SELECT name FROM sqlite_master WHERE type='table';";

        // FolderTree
        public const string FOLDERTREE_INSERT = @"INSERT INTO FolderTree (parentid, name) VALUES (@parentid,@name)";
        public const string FOLDERTREE_selectByID = @"SELECT * FROM FolderTree WHERE id = @id";

    }
}
