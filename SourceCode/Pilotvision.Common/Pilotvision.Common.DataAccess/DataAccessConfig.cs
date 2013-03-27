namespace Pilotvision.Common.DataAccess
{
    /// <summary>
    /// データソースへの接続情報を設定するデータアクセスコントローラクラスです。
    /// </summary>
    /// <remarks>
    /// 全てのデータアクセスクラスは、このクラスのインスタンスに設定された接続情報を参照して、
    /// 適切なデータソースからデータを取得します。
    /// </remarks>
    public class DataAccessConfig
    {
        private DatabaseType databaseType = DatabaseType.SqlServer;
        private string serverName = "localhost";
        private string userName = string.Empty;
        private string password = string.Empty;
        private string databaseName = string.Empty;
        private int connectionTimeout = 30;
        private int commandTimeout = 300;
        private int transactionTimeout = 60;

        /// <summary>
        /// 接続するデータソースの種類を設定します。
        /// </summary>
        public DatabaseType DatabaseType
        {
            get { return databaseType; }
            set { databaseType = value; }
        }

        /// <summary>
        /// 接続先のサーバ名を設定します。
        /// <para>
        /// 初期状態では「localhost」が設定されます。
        /// </para>
        /// </summary>
        public string ServerName
        {
            get { return serverName; }
            set
            {
                if ((value == string.Empty) || (value == null))
                {
                    serverName = "localhost";
                }
                else
                {
                    serverName = value;
                }
            }
        }

        /// <summary>
        /// データソースへ接続する際のユーザ名を設定します。
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// データソースへ接続するユーザのパスワードを設定します。
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// データベース名を表します。
        /// </summary>
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        /// <summary>
        /// DB接続のタイムアウト時間(秒)を表します。
        /// </summary>
        public int ConnectionTimeout
        {
            get { return connectionTimeout; }
            set { connectionTimeout = value; }
        }

        /// <summary>
        /// クエリ実行のタイムアウト時間(秒)を表します。
        /// </summary>
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <summary>
        /// トランザクションのタイムアウト時間(秒)を表します。
        /// </summary>
        public int TransactionTimeout
        {
            get { return transactionTimeout; }
            set { transactionTimeout = value; }
        }

        /// <summary>
        /// <see cref="DataAccessConfig"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DataAccessConfig()
        {
        }

        /// <summary>
        /// データソースの種類（<see cref="DatabaseType"/>）を指定して、<see cref="DataAccessConfig"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dbType">
        /// データソースの種類をあらわす <see cref="DatabaseType"/> 列挙体。
        /// </param>
        public DataAccessConfig(DatabaseType dbType)
            : this()
        {
            databaseType = dbType;
        }

        /// <summary>
        /// <see cref="DataAccessConfig"/>クラスのクローンを取得します。
        /// </summary>
        /// <returns>
        /// 現在の<see cref="DataAccessConfig"/>と同様の設定内容を持つ新しい<see cref="DataAccessConfig"/>
        /// </returns>
        public DataAccessConfig Clone()
        {
            return (DataAccessConfig)MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return databaseType.GetHashCode() ^ serverName.GetHashCode() ^ userName.GetHashCode() ^ password.GetHashCode() ^ databaseName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataAccessConfig))
            {
                return false;
            }

            return (this == (obj as DataAccessConfig));
        }

        public static bool operator ==(DataAccessConfig lhs, DataAccessConfig rhs)
        {
            if ((object.ReferenceEquals(lhs, null)) && (object.ReferenceEquals(rhs, null)))
            {
                return true;
            }

            if (object.ReferenceEquals(lhs, null))
            {
                return false;
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return false;
            }

            if (object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if ((lhs.databaseType == rhs.databaseType) &&
                (lhs.serverName == rhs.serverName) &&
                (lhs.userName == rhs.userName) &&
                (lhs.password == rhs.password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(DataAccessConfig lhs, DataAccessConfig rhs)
        {
            return !(lhs == rhs);
        }
    }
}
