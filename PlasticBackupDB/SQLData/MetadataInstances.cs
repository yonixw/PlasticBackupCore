using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasticBackupDB.SQLData
{
    public class MetadataInstances
    {
        public abstract class MetaInstanceRow
        {
            public long id = -1;
            private bool isFolder; // Only for inner uses
            public long sourceid;
            public bool error = true; // This class has invalid information.
        }

        public class FileMetaInstanceRow : MetaInstanceRow { }

        public class FolderMetaInstanceRow : MetaInstanceRow { }

        public List<FileMetaInstanceRow> getFileMetaInstances(Files.FileRow file) { return null; }

        public List<FolderMetaInstanceRow> getFolderMetaInstances(FolderTree.FolderTreeRow fodler) { return null; }

       
    }
}
