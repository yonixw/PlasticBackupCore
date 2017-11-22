using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class Files
    {
        public class FileRow
        {
            public long id = -1;
            public string fileName;
            public long myFolderId;
            public bool error = true; // This class has invalid information.
        }

        public List<FileRow> getFolderFiles(FolderTree.FolderTreeRow folder) { return null; }

        public FileRow createOrFindFile(FolderTree.FolderTreeRow folder, string filename) { return null; }
    }
}
