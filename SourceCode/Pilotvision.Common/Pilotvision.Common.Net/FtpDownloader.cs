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

        public override bool ExistsFile(string uri)
        {
            var req = CreateRequest(uri);
            (req as FtpWebRequest).Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                using (var res = req.GetResponse()) { }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var r = (FtpWebResponse)e.Response;
                    if (r.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
                else
                {
                    throw;
                }
            }
            return true;
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