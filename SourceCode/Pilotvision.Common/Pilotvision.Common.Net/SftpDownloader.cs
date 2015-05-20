using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Pilotvision.Common.Net
{
    public class SftpDownloader
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public SftpDownloader()
        {
            Port = 22;
        }

        public SftpDownloader(string hostName) : this()
        {
            HostName = hostName;
        }

        public SftpDownloader(string hostName, int port) : this(hostName)
        {
            Port = port;
        }

        public SftpDownloader(string userName, string password) : this()
        {
            UserName = userName;
            Password = password;
        }

        public SftpDownloader(string hostName, string userName, string password) : this(userName, password)
        {
            HostName = hostName;
        }

        public SftpDownloader(string hostName, int port, string userName, string password) : this(hostName, port)
        {
            UserName = userName;
            Password = password;
        }

        public bool ExistsFile(string path)
        {
            try
            {
                using (var sftp = new SftpClient(HostName, Port, UserName, Password))
                {
                    sftp.Connect();
                    // sftp.ChangeDirectory("/");
                    sftp.GetAttributes(path);
                    sftp.Disconnect();
                }
            }
            catch (SftpPathNotFoundException)
            {
                return false;
            }
            return true;
        }

        public MemoryStream Download(string path)
        {
            var result = new MemoryStream();
            using (var sftp = new SftpClient(HostName, Port, UserName, Password))
            {
                sftp.Connect();

                sftp.DownloadFile(path, result);
                sftp.Disconnect();
            }
            result.Position = 0;
            return result;
        }
    }
}
