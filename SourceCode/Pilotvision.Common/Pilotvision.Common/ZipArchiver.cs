using System;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;

namespace Pilotvision.Common
{
    public static class ZipArchiver
    {
        public static MemoryStream Compress(MemoryStream target, string fileName, string password = "")
        {
            var result = new MemoryStream();
            using (var zip = new ZipFile())
            {
                zip.CompressionLevel = CompressionLevel.BestCompression;

                byte[] buffer = target.ToArray();
                var e = zip.AddEntry(fileName, buffer);
                
                if (!string.IsNullOrEmpty(password))
                {
                    e.Password = password;
                    e.Encryption = EncryptionAlgorithm.WinZipAes256;
                }
                zip.Save(result);
            }
            return result;
        }

        public static void Extract(Stream target, string destinationFolder, string password = "")
        {
            // ZipFileを読み込む
            using (var zip = ZipFile.Read(target))
            {
                // パスワードが設定されている場合
                if (string.IsNullOrEmpty(password))
                {
                    zip.Password = password;
                }

                // ZIP書庫内のエントリを取得
                foreach (var entry in zip)
                {
                    //エントリを展開する
                    // 展開先に同名のファイルがあれば上書きする
                    entry.Extract(destinationFolder, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}