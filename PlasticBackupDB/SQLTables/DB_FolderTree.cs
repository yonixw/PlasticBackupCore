using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PlasticBackupDB.SQLTables
{
    class DB_FolderTree : BaseSQLiteAdapter
    {
        public  DB_FolderTree(SQLiteConnection conn): base(conn)
        {

        }

        public int createFolder(int parentId, string name) { return -1; }
    }
}
