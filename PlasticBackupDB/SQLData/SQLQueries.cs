using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    static class SQLQueries
    {
        public const string GET_ALL_TABLES_RAW = @"SELECT name FROM sqlite_master WHERE type='table';";

        // FolderTree
        public static SQLUtils.SQLCommand FOLDERTREE_insert =
            new SQLUtils.SQLCommand(
                @"INSERT INTO FolderTree (parentid, name) VALUES (@parentid,@name)",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@parentid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
                });

        public static SQLUtils.SQLCommand FOLDERTREE_selectById =
            new SQLUtils.SQLCommand(
                @"SELECT * FROM FolderTree WHERE id = @id",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@id", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                });

    }
}
