using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class Files: SQLUtils.SQLFunctions
    {
        public Files(SQLUtils.SQLConnection conn) : base(conn)
        {

        }

        public class FileRow
        {
            public long id = -1;
            public string fileName;
            public long myFolderId;
            public bool error = true; // This class has invalid information.
        }


        public SQLUtils.SQLCommand SQL_FILES_selectByParentID =
            new SQLUtils.SQLCommand(
                @"INSERT INTO FolderTree (parentid, name) VALUES (@parentid, @name)",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@parentid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
                });

        public FileRow createOrFindFile(FolderTree.FolderTreeRow folder, string filename) { return null; }

        public FileRow findFileById(long id) { return null; }

        public void deleteFile(FileRow file) {  }

        public List<FileRow> getFolderFiles(FolderTree.FolderTreeRow folder) { return null; }
    }
}
