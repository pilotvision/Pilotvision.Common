using Pilotvision.Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.IO;

namespace Pilotvision.Common.Net.Tests
{
    
    
    /// <summary>
    ///FtpDownloaderTest のテスト クラスです。すべての
    ///FtpDownloaderTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class FtpDownloaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        /// Download のテスト
        /// </summary>
        [TestMethod]
        public void DownloadTest()
        {
            string uri = @"ftp://www.pilotvision.co.jp/googlehostedservice.html";
            string userName = "username";
            string password = "password";

            FtpDownloader target = new FtpDownloader(userName, password);

            MemoryStream actual = null;
            actual = target.Download(uri);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        /// CreateRequest のテスト
        /// </summary>
        [TestMethod()]
        [DeploymentItem("Pilotvision.Common.Net.dll")]
        public void CreateRequestTest()
        {
            var target = new FtpDownloader_Accessor();
            string uri = "ftp://www.pilotvision.co.jp/index.php";
            var actual = target.CreateRequest(uri);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual is FtpWebRequest);
        }

        /// <summary>
        ///ExistsFile のテスト
        ///</summary>
        [TestMethod()]
        public void ExistsFileTest()
        {
            string uri = @"ftp://www.pilotvision.co.jp/googlehostedservice.html";
            string userName = "username";
            string password = "password";

            FtpDownloader target = new FtpDownloader(userName, password);

            var actual = target.ExistsFile(uri);
            Assert.AreEqual(true, actual);

            uri = @"ftp://www.pilotvision.co.jp/foo.bar";
            actual = target.ExistsFile(uri);
            Assert.AreEqual(false, actual);
        }
    }
}
