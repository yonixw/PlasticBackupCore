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
        Files _Filesfunc = new Files(new SQLConnection(
               "PlasticBackupSQLiteDB_Debug.sqlite3"
               ));

        FolderTree FolderTreefunc = new FolderTree(new SQLConnection(
               "PlasticBackupSQLiteDB_Debug.sqlite3"
               ));

        public FolderTree.FolderTreeRow getTestFolder(string foldername)
        {
            List<string> testPath = new List<string>();
            testPath.AddRange(new[] { "MY-PC", "E:", "FilesTests", foldername });

            return FolderTreefunc.createOrFindFolder(testPath);
        }

        [TestMethod]
        public void TestCreatingAndFindingFile()
        {
            FolderTree.FolderTreeRow folder = getTestFolder("_CreateFile");

            string filename = "file1.ext.עברית.exe";
            Files.FileRow file = _Filesfunc.createOrFindFile(folder, filename);

            Assert.IsFalse(file.error);

            Assert.IsTrue(file.fileName == filename);

            Assert.IsTrue(file.myFolderId == folder.id);

            // Find if already created.
            Files.FileRow fileFind = _Filesfunc.createOrFindFile(folder, filename);

            Assert.IsTrue(file.id == fileFind.id);

        }

        [TestMethod]
        public void TestFindingFileByID()
        {
            FolderTree.FolderTreeRow folder = getTestFolder("_FindingFile");

            string filename = "file2.ext.עברית.exe";
            Files.FileRow file = _Filesfunc.createOrFindFile(folder, filename);

            Files.FileRow fileFind = _Filesfunc.findFileById(file.id);

            Assert.IsTrue(file.id == fileFind.id);

            long lastId = _Filesfunc.getLastSequence();

            // Check file not found 

            Files.FileRow fileOutOfRange = _Filesfunc.findFileById(lastId + 1);

            Assert.IsTrue(fileOutOfRange.error);
        }

        [TestMethod]
        public void TestDeletingFile()
        {
            FolderTree.FolderTreeRow folder = getTestFolder("_DeletingFile");

            string filename = "file3.ext.עברית.exe";
            Files.FileRow file = _Filesfunc.createOrFindFile(folder, filename);

            long lastFileId = file.id;

            Assert.IsFalse(file.error);

            _Filesfunc.deleteFile(file);

            Assert.IsTrue(file.error);

            // Check no id is being reused:

            file = _Filesfunc.createOrFindFile(folder, filename);

            Assert.IsTrue(file.id > lastFileId);
        }

        [TestMethod]
        public void TestIDIncrementInsertOrDelete()
        {
            // Create 2, delete last, add new and check middle id dont ger reused
        }

        [TestMethod]
        public void TestGetFolderFiles()
        {
            // Create files and try to get them, maybe implement fast list compare?
        }
    }
}
