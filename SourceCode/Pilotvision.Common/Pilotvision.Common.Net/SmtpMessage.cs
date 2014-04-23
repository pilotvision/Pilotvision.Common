using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Pilotvision.Common.Net
{
    public class SmtpMessage : IDisposable
    {
        private MailMessage message = new MailMessage();
        private Encoding encoding;

        public string ServerName { get; set; }
        public int Port { get; set; }
        public bool UseCredentials { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Subject { get; set; }
        public string Message { get; set; }

        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public List<string> To { get; private set; }
        public List<string> Cc { get; private set; }
        public List<string> Bcc { get; private set; }
        public string EncodingName { get; set; }

        public SmtpMessage()
        {
            ServerName = "127.0.0.1";
            Port = 587;

            UseCredentials = false;
            UserName = string.Empty;
            Password = string.Empty;

            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            EncodingName = "iso-2022-jp";
        }

        public void Dispose()
        {
            if (message != null)
            {
                message.Dispose();
                message = null;
            }
        }

        private static string Encode(string str, Encoding enc)
        {
            return string.Format("=?{0}?B?{1}?=", enc.BodyName, Convert.ToBase64String(enc.GetBytes(str)));
        }

        public void SetAttachments(string fileName, string mediaType)
        {
            message.Attachments.Add(new Attachment(fileName, mediaType));
        }

        public void SetAttachments(System.IO.Stream stream, string mediaType)
        {
            message.Attachments.Add(new Attachment(stream, mediaType));
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtp = new SmtpClient();

            // SMTP サーバの指定
            smtp.Host = ServerName;
            smtp.Port = 587;

            // SMTP 認証の場合、ユーザ名とパスワードを送信
            if (UseCredentials)
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(UserName, Password);
            }

            return smtp;
        }

        public void Send()
        {
            encoding = Encoding.GetEncoding(EncodingName);

            // 送信元
            message.From = new MailAddress(FromAddress, Encode(FromName, encoding));

            // 送信先
            foreach (string address in To)
            {
                message.To.Add(new MailAddress(address));
            }

            // Cc
            foreach (string address in Cc)
            {
                message.CC.Add(new MailAddress(address));
            }

            // Bcc
            foreach (string address in Bcc)
            {
                message.Bcc.Add(new MailAddress(address));
            }

            // 件名
            message.Subject = Encode(Subject, encoding);

            // 本文
            AlternateView altView = AlternateView.CreateAlternateViewFromString(Message, encoding, MediaTypeNames.Text.Plain);
            altView.TransferEncoding = TransferEncoding.SevenBit;

            message.AlternateViews.Add(altView);

            var smtp = CreateSmtpClient();

            // メッセージを送信
            try
            {
                smtp.Send(message);
            }
            catch
            {
                // メッセージが送信できなかったら、
                // 5回リトライしてみる
                for (int i = 0; i < 4; i++)
                {
                    System.Threading.Thread.Sleep(5000);
                    try
                    {
                        smtp.Send(message);
                        // 送信できたら処理終了
                        return;
                    }
                    catch
                    {
                    }
                }
                System.Threading.Thread.Sleep(5000);
                smtp.Send(message);
            }
        }
    }
}