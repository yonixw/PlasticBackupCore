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

        public SQLUtils.SQLCommand SQL_FILES_insert =
          new SQLUtils.SQLCommand(
              @"INSERT INTO Files (folderid, name) VALUES (@folderid, @name)",
              new List<SQLUtils.SQLCommand.SQLParam>()
              {
                    new SQLUtils.SQLCommand.SQLParam("@folderid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
              });

        private FileRow newFile(long folderid, string filename) { 
            // Insert new:
            int rowsAdded = SQL_FILES_insert.ExecuteNonScalar(
                    new List<object>() { folderid, filename }, myConnection
                );

            // Get last index:
            long fileId = getLastSequence();

            // Return folder:
            FileRow result = findFileById(fileId);

            return result;
        }

        public SQLUtils.SQLCommand SQL_FILES_selectByParentAndName =
           new SQLUtils.SQLCommand(
               @"SELECT * FROM Files WHERE folderid=@folderid AND name=@name",
               new List<SQLUtils.SQLCommand.SQLParam>()
               {
                    new SQLUtils.SQLCommand.SQLParam("@folderid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
               });

        public FileRow createOrFindFile(FolderTree.FolderTreeRow folder, string filename) {
            List<FileRow> result =
                SQL_FILES_selectByParentAndName.ExecuteReadAll<FileRow>(
                    new List<object>() { folder.id, filename },
                    (reader) =>
                    {
                        return new FileRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            fileName = reader["name"] as string,
                            myFolderId = Convert.ToInt64(reader["folderid"]),
                            error = false
                        };
                    }, myConnection
                );

            if (result.Count == 0)
                return newFile(folder.id, filename);
            else
                return result[0];
        }


        public SQLUtils.SQLCommand SQL_FILES_selectByID =
            new SQLUtils.SQLCommand(
                @"SELECT * FROM Files WHERE id=@id",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@id", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER)
                });

        public FileRow findFileById(long rowid) {
            FileRow result = new FileRow();

            List<FileRow> rows = new List<FileRow>();
            rows = SQL_FILES_selectByID.ExecuteReadAll<FileRow>(
                    new List<object>() { rowid },
                    (reader) =>
                    {
                        return new FileRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            fileName = reader["name"] as string,
                            myFolderId = Convert.ToInt64(reader["folderid"]),
                            error = false
                        };
                    }, myConnection
                );

            // One id is one folder!!
            if (rows.Count == 1)
                result = rows[0];
            // Else no id found.

            return result;
        }

        public SQLUtils.SQLCommand SQL_FILES_deleteById =
          new SQLUtils.SQLCommand(
              @"DELETE FROM Files WHERE id = @id",
              new List<SQLUtils.SQLCommand.SQLParam>()
              {
                    new SQLUtils.SQLCommand.SQLParam("@id", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER)
              });

        public void deleteFile(FileRow file) {
            int deletedRowsCount = (int) // https://stackoverflow.com/a/24235553/1997873
                SQL_FILES_deleteById.ExecuteNonScalar(new List<object>() { file.id }, myConnection);

            //if (deletedRowsCount != 1) Folder already deleted.

            // Invalidate Object.
            file.error = true;
        }

        public SQLUtils.SQLCommand SQL_FILES_selectByFolder =
           new SQLUtils.SQLCommand(
               @"SELECT * FROM Files WHERE folderid=@folderid",
               new List<SQLUtils.SQLCommand.SQLParam>()
               {
                    new SQLUtils.SQLCommand.SQLParam("@folderid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER)
               });

        public List<FileRow> getFolderFiles(FolderTree.FolderTreeRow folder) {
            List<FileRow> result =
                SQL_FILES_selectByFolder.ExecuteReadAll<FileRow>(
                    new List<object>() { folder.id },
                    (reader) =>
                    {
                        return new FileRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            fileName = reader["name"] as string,
                            myFolderId = Convert.ToInt64(reader["folderid"]),
                            error = false
                        };
                    }, myConnection
                );
            return result;
        }

        public SQLUtils.SQLCommand SQL_FILES_lastSequence =
           new SQLUtils.SQLCommand(
               @"SELECT seq FROM sqlite_sequence WHERE name = 'Files'",
               null);

        public long getLastSequence()
        {
            List<long> seq =
                SQL_FILES_lastSequence.ExecuteReadAll(
                    null,
                    (reader) => { return Convert.ToInt64(reader["seq"]); },
                    myConnection
                    );

            if (seq.Count != 1) throw new Exception("Can't read sequence counter. try after insert/delete");
            return seq[0];
        }
    }
}
