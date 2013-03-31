using System;
using System.IO;
using Ionic.Zip;

namespace Pilotvision.Common
{
    public static class ZipArchiver
    {
        public static MemoryStream Compress(MemoryStream target, string fileName, string password = "")
        {
            var result = new MemoryStream();
            using (ZipFile zip = new ZipFile())
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                byte[] buffer = target.ToArray();
                ZipEntry e = zip.AddEntry(fileName, buffer);
                if (!string.IsNullOrEmpty(password))
                {
                    e.Password = password;
                    e.Encryption = EncryptionAlgorithm.WinZipAes256;
                }
                zip.Save(result);
            }
            return result;
            
            /*
            // var result = new MemoryStream();

            using (ZipOutputStream zipStream = new ZipOutputStream(result)
            {
                IsStreamOwner = false
            })
            {
                // 圧縮レベルを設定する
                zipStream.SetLevel(9);

                if (!string.IsNullOrEmpty(password))
                {
                    // パスワードを設定する
                    zipStream.Password = password;
                }

                // ディレクトリを保持する時は次のようにする
                // string f = file.Remove(
                //     0, System.IO.Path.GetPathRoot(file).Length);
                // f = f.Replace("\\","/");

                byte[] buffer = target.ToArray();

                // CRCを設定する
                Crc32 crc = new Crc32();
                crc.Reset();
                crc.Update(buffer);

                // ZIPに追加するときのファイル名を決定する
                ZipEntry entry = new ZipEntry(fileName)
                {
                    Crc = crc.Value, 
                    // サイズを設定する
                    Size = buffer.Length, 
                    // 時間を設定する
                    DateTime = DateTime.Now
                };

                // 新しいエントリの追加を開始
                zipStream.PutNextEntry(entry);
                // 書き込む
                zipStream.Write(buffer, 0, buffer.Length);
                zipStream.Finish();
                zipStream.Close();

                return result;             
            }
            */
        }
    }
}