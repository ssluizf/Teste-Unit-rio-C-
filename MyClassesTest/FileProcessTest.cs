using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;

namespace MyClassesTest {
    [TestClass]
    public class FileProcessTest {
        private const string BAD_FILE_NAME = @"C:\BadFileName.bat";
        private string _GoodFileName;

        public TestContext TestContext { get; set; }

        #region Test Initialize e Cleanup

        [TestInitialize]
        public void TestInitialize() {
            if(TestContext.TestName == "FileNameDoesExists") {
                SetGoodFileName();
                if (!string.IsNullOrEmpty(_GoodFileName)) {
                    TestContext.WriteLine($"Creating File: {_GoodFileName}");
                    File.AppendAllText(_GoodFileName, "Some Text");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup() {
            if (TestContext.TestName == "FileNameDoesExists") {
                if (!string.IsNullOrEmpty(_GoodFileName)) {
                    TestContext.WriteLine($"Deleting File: {_GoodFileName}");
                    File.Delete(_GoodFileName);
                }
            }
        }

        #endregion

        [TestMethod]
        [Description("Check to see if a file does exist.")]
        [Owner("LuizF")]
        public void FileNameDoesExists() {
            FileProcess fp = new FileProcess();
            bool fromCall;
            
            TestContext.WriteLine($"Testing File: {_GoodFileName}");
            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        public void SetGoodFileName() {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if(_GoodFileName.Contains("[AppPath]")) {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

        [TestMethod]
        [Timeout(3100)]
        public void SimulateTimeout() {
            System.Threading.Thread.Sleep(3000);
        }

        [TestMethod]
        [Description("Check to see if a file does NOT exist.")]
        [Owner("LuizF")]
        public void FileNameDoesNotExists() {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("LuizF")]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException() {
            // TODO;
            FileProcess fp = new FileProcess();

            fp.FileExists("");
        }

        [TestMethod]
        [Owner("LuizF")]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UsingTryCatch() {
            // TODO;
            FileProcess fp = new FileProcess();

            try {
                fp.FileExists("");
            }
            catch (ArgumentException) {
                // The test was a sucess;
                return;
            }

            Assert.Fail("Fail expected");
        }
    }
}
