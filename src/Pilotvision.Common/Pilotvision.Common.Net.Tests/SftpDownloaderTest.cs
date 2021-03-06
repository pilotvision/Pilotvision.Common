﻿using Pilotvision.Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Pilotvision.Common.Net.Tests
{


    /// <summary>
    ///SftpDownloaderTest のテスト クラスです。すべての
    ///SftpDownloaderTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SftpDownloaderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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
        ///ExistsFile のテスト
        ///</summary>
        [TestMethod()]
        public void ExistsFileTest()
        {
            var target = new SftpDownloader("ftp.foobar.com", "username", "password");
            string path = "aaa.csv";
            var actual = target.ExistsFile(path);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///Download のテスト
        ///</summary>
        [TestMethod()]
        public void DownloadTest()
        {
            var target = new SftpDownloader("ftp.foobar.com", 22, "username", "password");
            string path = "aaa.csv";
            MemoryStream actual = null;
            actual = target.Download(path);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length > 0);
        }
    }
}