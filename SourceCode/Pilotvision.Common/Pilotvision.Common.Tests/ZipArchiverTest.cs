using Pilotvision.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Pilotvision.Common.Tests
{
    
    
    /// <summary>
    ///ZipArchiverTest のテスト クラスです。すべての
    ///ZipArchiverTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ZipArchiverTest
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
        ///Extract のテスト
        ///</summary>
        [TestMethod()]
        public void ExtractTest()
        {
            Stream target = new FileStream(@"D:\Users\Eisuke\Desktop\分散処理しないdll.zip", FileMode.Open);

            string destinationFolder = @"D:\ZipTest";

            string password = string.Empty; 
            ZipArchiver.Extract(target, destinationFolder, password);

            Assert.IsTrue(System.IO.File.Exists(@"D:\ZipTest\分散処理しない版\TtlSVC.Core.dll"));
        }
    }
}
