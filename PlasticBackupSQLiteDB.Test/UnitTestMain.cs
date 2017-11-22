using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlasticBackupDB.SQLTables;
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

        [TestMethod]
        public void TestConnection()
        {
            SQLConnection conn = new SQLConnection(
                "PlasticBackupSQLiteDB_Debug.sqlite3"
                );
            conn.Open();

            List<string> tables = conn.GetAllTables();
            Trace.WriteLine("Has " + tables.Count + " Tables.");
            Assert.IsFalse(tables.Count == 0);

            conn.Close();
        }
    }
}
