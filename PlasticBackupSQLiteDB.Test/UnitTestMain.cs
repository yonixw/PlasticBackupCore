using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlasticBackupDB.SQLUtils;
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
            conn.Open();

            SQLFunctions func = new SQLFunctions();
            func.myConnection = conn;

            List<string> testPath = new List<string>();
            testPath.AddRange(new[] { "C:", "Folder1", "Folder Space" , "utfשלום" });

            SQLFunctions.FolderTreeRow folder = func.createOrFindFolder(testPath);

            Assert.IsNotNull(folder);

            // Check for errors.
            Assert.IsTrue(folder.error == false);

            // Check if id is legit.
            Assert.IsTrue(folder.id > 0);

            SQLFunctions.FolderTreeRow folder2 = func.createOrFindFolder(testPath);

            // Check if both result ids are the same ==> same folder in db.
            Assert.Equals(folder.id, folder2.id);

            conn.Close();
        }
    }
}
