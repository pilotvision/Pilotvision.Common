using Pilotvision.Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.IO;

namespace Pilotvision.Common.Net.Tests
{
    
    
    /// <summary>
    ///WebDownloaderTest のテスト クラスです。すべての
    ///WebDownloaderTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class WebDownloaderTest
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
        [TestMethod()]
        public void DownloadTest()
        {
            string url = @"https://foobar.com/";
            string downloadFileNameFormat = "aaa{0:yyyyMMdd}.zip";

            var target = new WebDownloader();
            DateTime transactionDate = new DateTime(2013, 3, 5);
            string fileName = string.Format(downloadFileNameFormat, transactionDate);

            MemoryStream actual = null;
            actual = target.Download(string.Format("{0}/{1}", url, fileName));
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
            var target = new WebDownloader_Accessor();
            string uri = "http://www.pilotvision.co.jp/index.php";
            var actual = target.CreateRequest(uri);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual is WebRequest);
        }
    }
}
