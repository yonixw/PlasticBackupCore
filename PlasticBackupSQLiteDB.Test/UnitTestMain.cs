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
    public class UnitTestMain
    {
        public static string GetRelativePath(string relativePath)
        {
            //https://stackoverflow.com/q/10204091/1997873
            return
                System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                relativePath);
        }

        SQLConnection conn = new SQLConnection(
               "PlasticBackupSQLiteDB_Debug.sqlite3"
               );

        [TestMethod]
        public void TestConnectionListTables()
        {
            conn.Open();

            List<string> tables = conn.GetAllTables();
            Trace.WriteLine("Has " + tables.Count + " Tables.");
            Assert.IsTrue(tables.Count > 0);

            conn.Close();
        }

        [TestMethod]
        public void AddFolderAndSearchIt()
        {

            FolderTree FolderTreefunc = new FolderTree(conn);

            List<string> testPath = new List<string>();
            testPath.AddRange(new[] { "MY-PC", "C:", "Folder1", "Folder Space", "utfשלום" });

            FolderTree.FolderTreeRow folder = FolderTreefunc.createOrFindFolder(testPath);

            Assert.IsNotNull(folder);

            // Check for errors.
            Assert.IsTrue(folder.error == false);

            // Check if id is legit.
            Assert.IsTrue(folder.id > 0);

            // Check if parent id is legit because we are not root but a leaf in FolderTree.
            Assert.IsTrue(folder.parentid > 0);

            FolderTree.FolderTreeRow folder2 = FolderTreefunc.createOrFindFolder(testPath);

            // Check if both result ids are the same ==> same folder in db.
            Assert.AreEqual(folder.id, folder2.id);

        }
    }
}
