using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Pilotvision.Common
{
    /// <summary>
    /// 文字列処理に関する拡張メソッド群を含む静的クラスです。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指定した文字列の SHA256 ハッシュ値を取得し、16 進数表記文字列として返します。
        /// </summary>
        /// <param name="originalString">
        /// ハッシュ値を取得する文字列。
        /// </param>
        /// <param name="stretch">
        /// ストレッチ回数。
        /// </param>
        /// <param name="encoding">
        /// ハッシュ値を取得する際の文字コード
        /// </param>
        /// <returns>
        /// ハッシュ値を 16 進数表記文字列化した文字列。
        /// </returns>
        public static string ComputeSHA256HashString(this string originalString, int stretch = 1, Encoding encoding = null)
        {
            using (var crypto = new SHA256CryptoServiceProvider())
            {
                return originalString.ComputeHashString(stretch, crypto, encoding);
            }
        }

        /// <summary>
        /// 指定した文字列の MD5 ハッシュ値を取得し、16 進数表記文字列として返します。
        /// </summary>
        /// <param name="originalString">
        /// ハッシュ値を取得する文字列。
        /// </param>
        /// <param name="stretch">
        /// ストレッチ回数。
        /// </param>
        /// <param name="encoding">
        /// ハッシュ値を取得する際の文字コード
        /// </param>
        /// <returns>
        /// ハッシュ値を 16 進数表記文字列化した文字列。
        /// </returns>
        public static string ComputeMD5HashString(this string originalString, int stretch = 1, Encoding encoding = null)
        {
            using (var crypto = new MD5CryptoServiceProvider())
            {
                return originalString.ComputeHashString(stretch, crypto, encoding);
            }
        }

        /// <summary>
        /// 任意のアルゴリズムでハッシュ値を取得し、16 進数表記文字列として返します。
        /// </summary>
        /// <param name="originalString">
        /// ハッシュ値を取得する文字列。
        /// </param>
        /// <param name="stretch">
        /// ストレッチ回数。
        /// </param>
        /// <param name="crypto">
        /// ハッシュ値を求める任意の <see cref="HashAlgorithm"/>。<c>null</c> の場合、<see cref="SHA256CryptoServiceProvider"/> を利用します。
        /// </param>
        /// <param name="encoding">
        /// ハッシュ値を取得する際の文字コード。<c>null</c> の場合、<see cref="Encoding.UTF8"/> でエンコーディングします。
        /// </param>
        /// <returns>
        /// ハッシュ値を 16 進数表記文字列化した文字列。
        /// </returns>
        public static string ComputeHashString(this string originalString, int stretch = 1, HashAlgorithm crypto = null, Encoding encoding = null)
        {
            // ここはコード重複しちゃうけど、先にやっておいたほうが無難だろう。
            if (string.IsNullOrEmpty(originalString))
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (crypto == null)
            {
                crypto = new SHA256CryptoServiceProvider();
            }

            string result = originalString;

            for (int i = 0; i < stretch; i++)
            {
                result = result.ComputeHashString(crypto, encoding);
            }

            return result;
        }

        /// <summary>
        /// 任意のアルゴリズムでハッシュ値を取得し、16 進数表記文字列として返します。
        /// </summary>
        /// <param name="originalString">
        /// ハッシュ値を取得する文字列。
        /// </param>
        /// <param name="crypto">
        /// ハッシュ値を求める任意の <see cref="HashAlgorithm"/>。<c>null</c> の場合、<see cref="SHA256CryptoServiceProvider"/> を利用します。
        /// </param>
        /// <param name="encoding">
        /// ハッシュ値を取得する際の文字コード。<c>null</c> の場合、<see cref="Encoding.UTF8"/> でエンコーディングします。
        /// </param>
        /// <returns>
        /// ハッシュ値を 16 進数表記文字列化した文字列。
        /// </returns>
        private static string ComputeHashString(this string originalString, HashAlgorithm crypto, Encoding encoding)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (crypto == null)
            {
                crypto = new SHA256CryptoServiceProvider();
            }

            byte[] source = encoding.GetBytes(originalString);

            return crypto.ComputeHash(source).ToHexString();
        }

        /// <summary>
        /// 指定したバイト配列を 16 進数表記文字列に変換します。
        /// </summary>
        /// <param name="source">
        /// 16 進数表記文字列を取得するバイト配列。
        /// </param>
        /// <returns>
        /// 取得した 16 進数文字列
        /// </returns>
        public static string ToHexString(this byte[] source)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                stringBuilder.Append(source[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 指定した文字列をSHIFT-JISでメモリーストリームに書き込みます。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <returns>メモリーストリーム</returns>
        public static MemoryStream ToMemoryStream(this string value)
        {
            return ToMemoryStream(value, Encoding.GetEncoding("SHIFT-JIS"));
        }

        /// <summary>
        /// 指定した文字列を指定したエンコードでメモリーストリームに書き込みます。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="encoding">エンコード</param>
        /// <returns>メモリーストリーム</returns>
        public static MemoryStream ToMemoryStream(this string value, Encoding encoding)
        {
            byte[] binaryData = encoding.GetBytes(value);
            MemoryStream result = new MemoryStream();
            result.Write(binaryData, 0, binaryData.Length);
            result.Flush();
            result.Position = 0;
            return result;
        }

        /// <summary>
        /// 対象文字列から、指定した文字数分右から取得します。
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="length">文字数</param>
        /// <returns></returns>
        public static string Right(this string value, int length)
        {
            if (value.Length <= length)
            {
                return value;
            }
            else
            {
                return value.Substring(value.Length - length, length);
            }
        }
    }
}