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

        [TestMethod]
        public void AddSubfoldersAndCheckThem()
        {
            FolderTree FolderTreefunc = new FolderTree(conn);

            List<string> testPath = new List<string>();
            testPath.AddRange(new[] { "MY-PC2", "D:", "SubfolderTest"});

            FolderTree.FolderTreeRow folder = FolderTreefunc.createOrFindFolder(testPath);

            Assert.IsNotNull(folder);

            // Check for errors.
            Assert.IsTrue(folder.error == false);

            // Create subfolders:
            FolderTree.FolderTreeRow sub1 = FolderTreefunc.createOrFindChildFolder(folder, "Sub1");
            FolderTree.FolderTreeRow sub2 = FolderTreefunc.createOrFindChildFolder(folder, "Sub2");
            Assert.AreEqual(sub1.parentid, sub2.parentid);

            // Get SubFolders:
            List<FolderTree.FolderTreeRow> subfolders = FolderTreefunc.getSubFolders(folder);
            Assert.IsTrue(subfolders.Count == 2); 

            // See if subfolders are the same :
            Assert.IsTrue(
                /*Option 1 : s1 = s[0] && s2 = s[1]*/
                (sub1.id == subfolders[0].id && sub2.id == subfolders[1].id)
                ||
                /*Option 2 : reversed */
                (sub1.id == subfolders[1].id && sub2.id == subfolders[0].id)
                );

            // Check we get subfolder with other methods:
            List<string> testPath2 = new List<string>();
            testPath2.AddRange(new[] { "MY-PC2", "D:", "SubfolderTest", "Sub2" });
            FolderTree.FolderTreeRow sub2Path = FolderTreefunc.createOrFindFolder(testPath2);

            Assert.AreEqual(sub2Path.id, sub2.id);

        }
    }
}
