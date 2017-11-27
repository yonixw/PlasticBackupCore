using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlasticBackupDB.SQLUtils;
using PlasticBackupDB.SQLData;
using PlasticBackupSQLiteDB;

namespace PlasticBackupSQLiteDB.Test
{
    [TestClass]
    public class FilesTests
    {
        Files FolderTreefunc = new Files(new SQLConnection(
               "PlasticBackupSQLiteDB_Debug.sqlite3"
               ));
    }
}
