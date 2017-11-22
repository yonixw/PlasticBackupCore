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
            // Prepare the commands:
            SQL_FOLDERTREE_insert.myConnection = conn;
            SQL_FOLDERTREE_selectById.myConnection = conn;
            SQL_FOLDERTREE_selectByParentAndName.myConnection = conn;
            SQL_FOLDERTREE_lastSequence.myConnection = conn;
        }

        public class FolderTreeRow
        {
            public long id = -1;
            public string folderName;
            public long parentid;
            public bool error = true; // This class has invalid information.
        }

        public FolderTreeRow createOrFindChildFolder(FolderTreeRow parentFolder, string newName) { return null; }

        public FolderTreeRow createOrFindFolder(List<string> pathList)
        {
            // For each part, find o.w. insert and continue.
            FolderTreeRow lastFolder = new FolderTreeRow();
           
            foreach (string folder in pathList)
            {
                lastFolder = findFolderByParentAndName(lastFolder.parentid, folder);

                if (!lastFolder.error)
                {
                    // folder exist! next one!
                }
                else
                {
                    // Create and get it!
                    lastFolder = newFolder(lastFolder.id, folder);
                }
            }

            return lastFolder;
        }

        public List<FolderTreeRow> getSubFolders(FolderTreeRow folder) { return null; }


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
            int resultCode = SQL_FOLDERTREE_insert.ExecuteNonScalar(
                    new List<object>() { parentid, partname }
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
                    (reader) => {
                        return new FolderTreeRow()
                        {
                            id = (reader["id"] as long?) ?? -1,
                            folderName = reader["name"] as string,
                            parentid = reader["parentid"] as long? ?? -1,
                            error = false
                        };
                    }
                );

            // One id is one folder!!
            if (rows.Count == 1)
                result = rows[0];
            else if (rows.Count > 1)
                throw new Exception("Multiple folder with same rowid?");

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
                    (reader) => {
                        return new FolderTreeRow()
                        {
                            id = (reader["id"] as long?) ?? -1,
                            folderName = reader["name"] as string,
                            parentid = reader["parentid"] as long? ?? -1,
                            error = false
                        };
                    }
                );

            // Like real folders only one unique name under each parent!
            if (rows.Count == 1)
                result = rows[0];
            else if (rows.Count > 1)
                throw new Exception("Multiple folder with same name under same parent?");

            return result;
        }

        public SQLUtils.SQLCommand SQL_FOLDERTREE_lastSequence =
           new SQLUtils.SQLCommand(
               @"SELECT seq FROM sqlite_sequence WHERE name = 'FolderTree'",
               null);

        long getLastSequence()
        {
            List<long> seq =
                SQL_FOLDERTREE_lastSequence.ExecuteReadAll(
                    null,
                    (reader) => { return (reader["seq"] as long?) ?? -1; }
                    );

            if (seq.Count != 1) throw new Exception("Can't read sequence counter. try after insert/delete");
            return seq[0];
        }



    }
}
