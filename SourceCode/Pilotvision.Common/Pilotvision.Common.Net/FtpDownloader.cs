using System.Net;

namespace Pilotvision.Common.Net
{
    public class FtpDownloader : WebDownloader
    {
        private NetworkCredential ftpCredential;
        public string UserName { get; set; }
        public string Password { get; set; }

        public FtpDownloader()
        {
        }

        public FtpDownloader(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        protected override WebRequest CreateRequest(string uri)
        {
            FtpWebRequest result = (FtpWebRequest)WebRequest.Create(uri);

            ftpCredential = new NetworkCredential(UserName, Password);
            result.Credentials = ftpCredential;
            result.Method = WebRequestMethods.Ftp.DownloadFile;
            result.UsePassive = true;
            result.KeepAlive = false;

            return result;
        }
    }
}