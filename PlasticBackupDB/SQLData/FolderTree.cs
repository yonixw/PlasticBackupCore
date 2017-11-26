using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class FolderTree : SQLUtils.SQLFunctions
    {
        public FolderTree(SQLUtils.SQLConnection conn)
        {
            myConnection = conn;
        }

        public class FolderTreeRow
        {
            public long id = -1;
            public string folderName;
            public long parentid = -1;
            public bool error = true; // This class has invalid information.
        }

        public FolderTreeRow createOrFindChildFolder(FolderTreeRow parentFolder, string newName) {
            FolderTreeRow result = findFolderByParentAndName(parentFolder.id, newName);
            if (result.error)
                result = newFolder(parentFolder.id, newName);
            return result;
        }

        public FolderTreeRow createOrFindFolder(List<string> pathList)
        {
            // For each part, find o.w. insert and continue.
            FolderTreeRow lastFolder = new FolderTreeRow();

            foreach (string folder in pathList)
            {
                FolderTreeRow nextFolder = findFolderByParentAndName(lastFolder.id, folder);

                if (!nextFolder.error)
                {
                    // folder exist! next one!
                }
                else
                {
                    // Create and get it!
                    nextFolder = newFolder(lastFolder.id, folder);
                }
                lastFolder = nextFolder;
            }

            return lastFolder;
        }

        public List<FolderTreeRow> getSubFolders(FolderTreeRow folder) {
            return findFoldersByParentID(folder.id);
        }


        // ************************
        // FolderTree SQL
        // ************************

        public SQLUtils.SQLCommand SQL_FOLDERTREE_insert =
            new SQLUtils.SQLCommand(
                @"INSERT INTO FolderTree (parentid, name) VALUES (@parentid, @name)",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@parentid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
                });

        FolderTreeRow newFolder(long parentid, string partname)
        {
            FolderTreeRow result = new FolderTreeRow();

            // Insert new:
            int rowsAdded = SQL_FOLDERTREE_insert.ExecuteNonScalar(
                    new List<object>() { parentid, partname } , myConnection
                );

            // Get last index:
            long folderId = getLastSequence();

            // Return folder:
            result = findFolderByID(folderId);

            return result;
        }

        public SQLUtils.SQLCommand SQL_FOLDERTREE_selectById =
            new SQLUtils.SQLCommand(
                @"SELECT * FROM FolderTree WHERE id = @id",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@id", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                });

        FolderTreeRow findFolderByID(long rowid)
        {
            FolderTreeRow result = new FolderTreeRow();

            List<FolderTreeRow> rows = new List<FolderTreeRow>();
            rows = SQL_FOLDERTREE_selectById.ExecuteReadAll<FolderTreeRow>(
                    new List<object>() { rowid },
                    (reader) =>
                    {
                        return new FolderTreeRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            folderName = reader["name"] as string,
                            parentid = Convert.ToInt64(reader["parentid"]),
                            error = false
                        };
                    } , myConnection
                );

            // One id is one folder!!
            if (rows.Count == 1)
                result = rows[0];
            // Else no id found.

            return result;
        }

        public SQLUtils.SQLCommand SQL_FOLDERTREE_selectByParentAndName =
           new SQLUtils.SQLCommand(
               @"SELECT * FROM FolderTree WHERE parentid = @parentid AND name = @name",
               new List<SQLUtils.SQLCommand.SQLParam>()
               {
                    new SQLUtils.SQLCommand.SQLParam("@parentid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                    new SQLUtils.SQLCommand.SQLParam("@name", SQLUtils.SQLCommand.SQLParam.sqliteType.TEXT)
               });

        FolderTreeRow findFolderByParentAndName(long parentid, string partname)
        {
            FolderTreeRow result = new FolderTreeRow();

            List<FolderTreeRow> rows = new List<FolderTreeRow>();
            rows = SQL_FOLDERTREE_selectByParentAndName.ExecuteReadAll<FolderTreeRow>(
                    new List<object>() { parentid, partname },
                    (reader) =>
                    {
                        return new FolderTreeRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            folderName = reader["name"] as string,
                            parentid = Convert.ToInt64(reader["parentid"]),
                            error = false
                        };
                    }, myConnection
                );

            // Like real folders only one unique name under each parent!
            if (rows.Count == 1)
                result = rows[0];
            else if (rows.Count > 1)
                throw new Exception("Multiple folder with same name under same parent?");

            return result;
        }

        public SQLUtils.SQLCommand SQL_FOLDERTREE_selectByParentID =
          new SQLUtils.SQLCommand(
              @"SELECT * FROM FolderTree WHERE parentid = @parentid",
              new List<SQLUtils.SQLCommand.SQLParam>()
              {
                    new SQLUtils.SQLCommand.SQLParam("@parentid", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
              });

        List<FolderTreeRow> findFoldersByParentID(long parentid)
        {
            List<FolderTreeRow> result = new List<FolderTreeRow>();
            result = SQL_FOLDERTREE_selectByParentID.ExecuteReadAll<FolderTreeRow>(
                    new List<object>() { parentid },
                    (reader) =>
                    {
                        return new FolderTreeRow()
                        {
                            id = Convert.ToInt64(reader["id"]),
                            folderName = reader["name"] as string,
                            parentid = Convert.ToInt64(reader["parentid"]),
                            error = false
                        };
                    }, myConnection
                );

            return result;
        }

        public SQLUtils.SQLCommand SQL_FOLDERTREE_lastSequence =
           new SQLUtils.SQLCommand(
               @"SELECT seq FROM sqlite_sequence WHERE name = 'FolderTree'",
               null);

        public long getLastSequence()
        {
            List<long> seq =
                SQL_FOLDERTREE_lastSequence.ExecuteReadAll(
                    null,
                    (reader) => { return Convert.ToInt64(reader["seq"]); },
                    myConnection
                    );

            if (seq.Count != 1) throw new Exception("Can't read sequence counter. try after insert/delete");
            return seq[0];
        }


        public SQLUtils.SQLCommand SQL_FOLDERTREE_deleteById =
            new SQLUtils.SQLCommand(
                @"DELETE FROM FolderTree WHERE id = @id",
                new List<SQLUtils.SQLCommand.SQLParam>()
                {
                    new SQLUtils.SQLCommand.SQLParam("@id", SQLUtils.SQLCommand.SQLParam.sqliteType.INTEGER),
                });

        public void deleteFolder(FolderTreeRow folder)
        {
            int deletedRowsCount = (int) // https://stackoverflow.com/a/24235553/1997873
                SQL_FOLDERTREE_deleteById.ExecuteNonScalar(new List<object>() { folder.id }, myConnection);

            //if (deletedRowsCount != 1) Folder already deleted.

            // Invalidate Object.
            folder.error = true;
        }

    }
}
