﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlasticBackupDB.SQLUtils;

namespace PlasticBackupSQLiteDB.Test
{
    [TestClass]
    public class SQLiteGeneral
    {
        SQLConnection conn = new SQLConnection(
              "PlasticBackupSQLiteDB_Debug.sqlite3"
              );

        [TestMethod]
        public void TestConnectionListTables()
        {
            List<string> tables = conn.GetAllTables();
            Trace.WriteLine("Has " + tables.Count + " Tables.");
            Assert.IsTrue(tables.Count > 0);
        }


    }
}
