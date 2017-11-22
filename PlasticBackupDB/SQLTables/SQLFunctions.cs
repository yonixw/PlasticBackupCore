using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PlasticBackupDB.SQLTables
{
    class SQLFunctions 
    {

        public SQLiteConnection myConnection { get; set; }

        private SQLFunctions() { }

        public SQLFunctions(SQLiteConnection conn)
        {
            myConnection = conn;
        }

        // ------   FolderTree Table  ----------

        public class FolderTreeRow
        {
            public int id = -1;
            public string folderName;
            public int parentid;
            public bool error = true; // This class has invalid information.
        }

        public FolderTreeRow createOrFindChildFolder(FolderTreeRow parentFolder, string newName) { return null; }

        public FolderTreeRow createOrFindFolder(List<string> pathList) { return null; }

        public List<FolderTreeRow> getSubFolders(FolderTreeRow folder) { return null; }

        // ------   Files Table  ----------

        public class FileRow
        {
            public int id = -1;
            public string fileName;
            public int myFolderId;
            public bool error = true; // This class has invalid information.
        }

        public List<FileRow> getFolderFiles(FolderTreeRow folder) { return null; }

        public FileRow createOrFindFile(FolderTreeRow folder, string filename) { return null; }

        // ------   MetadataInstances Table  ----------

        public abstract class MetaInstanceRow
        {
            public int id = -1;
            private bool isFolder; // Only for inner uses
            public int sourceid;
            public bool error = true; // This class has invalid information.
        }

        public class FileMetaInstanceRow : MetaInstanceRow { }

        public class FolderMetaInstanceRow : MetaInstanceRow { }

        public List<FileMetaInstanceRow> getFileMetaInstances (FileRow file) { return null; }

        public List<FolderMetaInstanceRow> getFolderMetaInstances(FolderTreeRow fodler) { return null; }

        // ------   MetadataValues Table  ----------

        public class MetadataItemRow
        {
            public int id = -1;
            public int instanceid = -1;
            string metakey;
            string metavalue;
            public bool error = true; // This class has invalid information.
        }

        public List<MetadataItemRow> getMetaItems(MetaInstanceRow instance) { return null; }

        public List<MetadataItemRow> findMetaItemsByKeyValue(string key, string value) { return null; }
    }
}
