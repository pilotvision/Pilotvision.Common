using System.IO;
using System.Net;

namespace Pilotvision.Common.Net
{
    public class WebDownloader
    {
        protected virtual WebRequest CreateRequest(string uri)
        {
            return WebRequest.Create(uri);
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

                    return result;
                }
            }
        }
    }
}
