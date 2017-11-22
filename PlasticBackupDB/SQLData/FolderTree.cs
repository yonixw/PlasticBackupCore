using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class FolderTree : SQLUtils.SQLFunctions
    {
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
    }
}
