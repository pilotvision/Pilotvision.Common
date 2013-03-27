using Pilotvision.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Pilotvision.Common.Tests
{    
    /// <summary>
    /// StringExtensionsTest のテスト クラスです。すべての
    /// StringExtensionsTest 単体テストをここに含めます
    /// </summary>
    [TestClass()]
    public class StringExtensionsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        /// 現在のテストの実行についての情報および機能を
        /// 提供するテスト コンテキストを取得または設定します。
        /// </summary>
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
        /// ComputeHashString のテスト
        /// </summary>
        [TestMethod()]
        public void ComputeHashStringTest1()
        {
            string originalString = "あいうえお";

            // アルゴリズム、エンコーディングは省略できる。
            string expected = "fdb481ea956fdb654afcc327cff9b626966b2abdabc3f3e6dbcb1667a888ed9a";
            string actual;

            actual = originalString.ComputeHashString();
            Assert.AreEqual(expected, actual);

            // 明示的にアルゴリズムとエンコーディングを指定することが出来る。
            HashAlgorithm crypto = null;
            Encoding encoding = null;

            crypto = new SHA512CryptoServiceProvider();
            encoding = Encoding.GetEncoding("shift_jis");

            expected = "114392120c35be09d8fa5713ab0626d9d6b66d22c6c0d5f589768f172e0f924ef61c8402586ba2a434510f1b655b289fd537a708ca2efa012906caa2a147c5b3";
            actual = originalString.ComputeHashString(1, crypto, encoding);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ComputeHashString のテスト。
        /// ストレッチさせる。
        /// </summary>
        [TestMethod()]
        public void ComputeHashStringTest2()
        {
            string originalString = "あいうえお";

            // アルゴリズム、エンコーディングは省略できる。
            string expected = "9327834a219bd4bf180c9fc7dcc3f05551dcae4b4497944ec5e294d40ea0a2f7";
            string actual;

            actual = originalString.ComputeHashString(2);
            Assert.AreEqual(expected, actual);

            // 明示的にアルゴリズムとエンコーディングを指定することが出来る。
            HashAlgorithm crypto = null;
            Encoding encoding = null;

            crypto = new SHA512CryptoServiceProvider();
            encoding = Encoding.GetEncoding("shift_jis");

            expected = "6ad7287b3b5f3536b5383445eed241a9915efa7cbfa53498f8eb4d28aeb1622ca2d2bd8c5b1e4dd3fd88a3d8a038fcc419260f8dd3b6bebc52b64f04948e9756";
            actual = originalString.ComputeHashString(2, crypto, encoding);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ComputeHashString のテスト。
        /// 同一性、一意性の確認。
        /// </summary>
        [TestMethod]
        public void ComputeHashStringTest3()
        {
            string original = "supplier01";
            Guid guid = Guid.NewGuid();
            string salt = guid.ToString();

            original = salt + original;

            string expected = original.ComputeHashString(256);

            // 同じ文字列からは何度求めても同じ結果になる。
            Assert.AreEqual(expected, original.ComputeHashString(256));

            // ストレッチ回数が異なれば、異なる結果が取得できる。
            Assert.AreNotEqual(expected, original.ComputeHashString(255));

            // 文字列が少しでも異なれば、異なる結果が取得できる。
            original = salt + "supplier02";
            Assert.AreNotEqual(expected, original.ComputeHashString(256));
        }

        /// <summary>
        /// ComputeMD5HashString のテスト
        /// </summary>
        [TestMethod()]
        public void ComputeMD5HashStringTest()
        {
            string originalString = "あいうえお";

            string expected = "86deb27a32903da70a7b2348fcf36bc3";
            string actual;

            // ストレッチとエンコーディングは省略できる。
            // その場合、UTF8 エンコーディングで 1 回算出される。
            actual = originalString.ComputeMD5HashString();
            Assert.AreEqual(expected, actual);

            expected = "4f95d1bc8398a74bf0d0090ac3943d64";
            // ストレッチ回数とエンコーディングを指定することができる。
            actual = originalString.ComputeMD5HashString(2, Encoding.GetEncoding("shift_jis"));

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ComputeSHA256HashString のテスト
        /// </summary>
        [TestMethod()]
        public void ComputeSHA256HashStringTest()
        {
            string originalString = "あいうえお";

            string expected = "fdb481ea956fdb654afcc327cff9b626966b2abdabc3f3e6dbcb1667a888ed9a";
            string actual;

            // ストレッチとエンコーディングは省略できる。
            // その場合、UTF8 エンコーディングで 1 回算出される。
            actual = originalString.ComputeSHA256HashString();
            Assert.AreEqual(expected, actual);

            expected = "aaa7b426a020463062db671ac58104cd1964bd670249b4b4577047803cb1eca6";
            // ストレッチ回数とエンコーディングを指定することができる。
            actual = originalString.ComputeSHA256HashString(2, Encoding.GetEncoding("shift_jis"));

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ToHexString のテスト
        /// </summary>
        [TestMethod()]
        public void ToHexStringTest()
        {
            byte[] source = new byte[] { 1, 2, 4, 8, 15, 16 };
            string expected = "010204080f10";
            string actual;
            actual = source.ToHexString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ストレッチングのテスト。主にパフォーマンスの確認。
        /// </summary>
        [TestMethod]
        public void StretchTest()
        {
            string original = "The quick brown fox jumps over the lazy dog";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            original.ComputeHashString(1000);
            sw.Stop();

            // 1000 回ストレッチして 100ms 以上かかるようなアルゴリズムだと使い者にならないだろ。。
            Assert.IsTrue(sw.ElapsedMilliseconds < 100);
        }
    }
}
