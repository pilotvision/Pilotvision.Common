using System.IO;
using System.Net;

namespace Pilotvision.Common.Net
{
    public class WebDownloader
    {
        protected virtual WebRequest CreateRequest(string uri)
        {
            return (HttpWebRequest)WebRequest.Create(uri);
        }

        public virtual bool ExistsFile(string uri)
        {
            var req = CreateRequest(uri);

            try
            {
                using (var res = req.GetResponse())
                {
                    res.Close();
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var r = (HttpWebResponse)e.Response;
                    if (r.StatusCode == HttpStatusCode.NotFound)
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

        public MemoryStream Download(string uri)
        {
            var req = CreateRequest(uri);

            using (var res = req.GetResponse())
            {
                using (Stream resStream = res.GetResponseStream())
                {
                    var result = new MemoryStream();
                    byte[] buffer = new byte[10240000];
                    while (true)
                    {
                        int readSize = resStream.Read(buffer, 0, buffer.Length);
                        if (readSize == 0)
                        {
                            break;
                        }
                        result.Write(buffer, 0, readSize);
                    }

                    res.Close();
                    return result;
                }
            }
        }
    }
}
