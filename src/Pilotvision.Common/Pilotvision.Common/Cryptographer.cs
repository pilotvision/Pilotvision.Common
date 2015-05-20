using System.IO;
using System.Security.Cryptography;

namespace Pilotvision.Common.Cryptography
{
    /// <summary>
    /// 鍵ファイルと対称アルゴリズムを利用して暗号化・複合を行います。
    /// </summary>
    public class Cryptographer
    {
        private byte[] key;
        private byte[] iv;
        private readonly AesCryptoServiceProvider crypto;

        public Cryptographer()
        {
            crypto = new AesCryptoServiceProvider();
        }

        public Cryptographer(string keyFileName)
            : this()
        {
            SetKey(keyFileName);
        }

        private void SetKey(string keyFileName)
        {
            // 鍵ファイルを開く
            using (FileStream fs = new FileStream(keyFileName, FileMode.Open, FileAccess.Read))
            {
                // 先頭 16 byte には IV を記述してある。
                iv = new byte[16];
                fs.Read(iv, 0, 16);

                // ファイルを読み込むバイト型配列を作成する
                key = new byte[fs.Length - iv.Length];
                // ファイルの残りの内容をすべて読み込む
                fs.Read(key, 0, key.Length);
                // 閉じる
                fs.Close();
            }

            crypto.Key = key;
            crypto.IV = iv;
        }

        /// <summary>
        /// 鍵ファイルを新たに作成します。
        /// </summary>
        /// <param name="keyFileName">
        /// 作成する鍵ファイルの名前。
        /// </param>
        public void GenerateKeyFile(string keyFileName)
        {
            // key と IV を作成する。あえてファイルが無ければ誰にも復号できぬよう、ランダム値。
            crypto.GenerateKey();
            crypto.GenerateIV();

            key = crypto.Key;
            iv = crypto.IV;

            // ファイルを作成して書き込む
            // ファイルが存在しているときは、上書きする
            using (FileStream fs = new FileStream(keyFileName, FileMode.Create, FileAccess.Write))
            {
                // バイト配列の内容をすべて書き込む
                // まずは IV。
                fs.Write(iv, 0, iv.Length);

                // 次に Key
                fs.Write(key, 0, key.Length);

                // 閉じる
                fs.Close();
            }
        }

        public string Encrypt(string originalString, string keyFileName)
        {
            SetKey(keyFileName);
            return Encrypt(originalString);
        }

        public string Decrypt(string encryptedString, string keyFileName)
        {
            SetKey(keyFileName);
            return Decrypt(encryptedString);
        }

        public string Encrypt(string originalString)
        {
            // AES 暗号化オブジェクトの作成
            using (ICryptoTransform encryptor = crypto.CreateEncryptor())
            {

                // 文字列をバイト型配列に変換する
                byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(originalString);

                // 暗号化
                byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

                encryptor.Dispose();

                return System.Convert.ToBase64String(encBytes);
            }
        }

        public string Decrypt(string encryptedString)
        {
            // 文字列をバイト配列に戻す
            byte[] strBytes = System.Convert.FromBase64String(encryptedString);

            // AES 暗号化オブジェクトの作成
            using (ICryptoTransform decryptor = crypto.CreateDecryptor())
            {
                // バイト配列を復号する
                byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                
                // 閉じる
                decryptor.Dispose();

                // バイト配列を文字列に戻して返す
                return System.Text.Encoding.UTF8.GetString(decBytes);
            }
        }
    }
}
