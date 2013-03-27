using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Pilotvision.Common.DataAccess
{
    /// <summary>
    /// 設定された DBMS の内容に従い、適切なデータ接続を提供します。
    /// DBとの接続・SQLの実行などは、すべてこのクラスを介して行います。
    /// </summary>
    public abstract class DataAccess : IDataAccess
    {
        public static DataAccess Create(DataAccessConfig config)
        {
            switch (config.DatabaseType)
            {
                case DatabaseType.MySql:
                    return new MySqlDataAccess(config);
                case DatabaseType.SqlServerCompact:
                    return new SqlCeDataAccess(config);
                case DatabaseType.SqlServer:
                default:
                    return new SqlDataAccess(config);
            }
        }

        #region Constructors
        /// <summary>
        /// <see cref="DataAccess"/> のインスタンスを初期化します。
        /// </summary>
        public DataAccess()
        {
            Config = new DataAccessConfig();
        }

        /// <summary>
        /// 指定した情報を利用して <see cref="DataAccess"/> のインスタンスを初期化します。
        /// </summary>
        /// <param name="serverName">
        /// 接続先サーバ名（またはIPアドレス）
        /// </param>
        /// <param name="userName">
        /// 接続DBユーザ名
        /// </param>
        /// <param name="password">
        /// 接続DBユーザのパスワード
        /// </param>
        /// <param name="databaseName">
        /// データベース名
        /// </param>
        public DataAccess(string serverName, string userName, string password, string databaseName)
            : this()
        {
            Config.ServerName = serverName;
            Config.UserName = userName;
            Config.Password = password;
            Config.DatabaseName = databaseName;
        }

        /// <summary>
        /// 指定した <see cref="DataAccessConfig"/> を利用して <see cref="DataAccess"/> のインスタンスを初期化します。
        /// </summary>
        /// <param name="config">
        /// データベース接続情報
        /// </param>
        public DataAccess(DataAccessConfig config)
            : this()
        {
            Config = config;
        }
        #endregion

        #region Properties
        /// <summary>
        /// データベースへアクセスするための接続情報を表します。
        /// </summary>
        public DataAccessConfig Config { get; set; }

        /// <summary>
        /// データベースへの接続タイムアウト時間を設定します。
        /// </summary>
        /// <value>
        /// 接続が開かれるまでの待機時間 (秒)。既定値は、30 秒です。0 は、無制限です。
        /// </value>
        /// <remarks>
        /// 代入された ConnectionTimeout プロパティ値が 0 未満の場合は、<see cref="ArgumentException"/> が生成されます。 
        /// </remarks>
        public int ConnectionTimeout
        {
            get { return Config.ConnectionTimeout; }
            set { Config.ConnectionTimeout = value; }
        }

        /// <summary>
        /// データベースへのコマンド実行のタイムアウト時間を設定します。 
        /// </summary>
        /// <value>
        /// コマンドが実行されるまでの待機時間 (秒)。 既定値は、60 秒です。0 は、無制限です。
        /// </value>
        /// <remarks>
        /// 代入された CommandTimeout プロパティ値が 0 未満の場合は、<see cref="ArgumentException"/> が生成されます。 
        /// </remarks>
        public int CommandTimeout
        {
            get { return Config.CommandTimeout; }
            set { Config.CommandTimeout = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 現在の <see cref="DataAccess"/> のクローンを取得します。
        /// </summary>
        /// <returns>
        /// 現在の<see cref="DataAccess"/>と同様の設定内容を持つ新しい<see cref="DataAccess"/>
        /// </returns>
        public DataAccess Clone()
        {
            DataAccess result = MemberwiseClone() as DataAccess;
            result.Config = Config.Clone();

            return result;
        }

        /// <summary>
        /// 接続文字列を取得します。
        /// </summary>
        /// <returns>接続文字列</returns>
        protected abstract string GetDbConnectionString();

        /// <summary>
        /// 接続する DBMS ごとに適切なコネクションオブジェクトを、DbConnection を介して取得します。
        /// </summary>
        /// <returns>
        /// コネクションオブジェクト（<see cref="DbConnection"/>）
        /// </returns>
        public abstract DbConnection GetDbConnection();


        private void AddCommandParameters(DbCommand cmd, Dictionary<string, object> parameters)
        {
            foreach (var p in parameters)
            {
                cmd.Parameters.Add(CreateParameter(cmd, p.Key, p.Value));
            }
        }

        protected abstract DbParameter CreateParameter(DbCommand cmd, string name, object value);

        /// <summary>
        /// 適切な <see cref="DbCommand"/> オブジェクトを生成します。
        /// </summary>
        /// <returns>
        /// データソースに適切な <see cref="DbCommand"/> オブジェクト。
        /// </returns>
        protected abstract DbCommand CreateDbCommand();

        /// <summary>
        /// 接続する DBMS ごとに適切な DbCommand オブジェクトを取得します。
        /// その際、<see cref="Config"/> の情報を元に、DbCommand.Connection オブジェクトの内容も設定されます。
        /// </summary>
        /// <returns>
        /// DBコマンドオブジェクト
        /// </returns>
        public DbCommand GetDbCommand()
        {
            DbCommand result = CreateDbCommand();
            // コマンドタイムアウト時間を設定する。
            result.CommandTimeout = CommandTimeout;
            // コネクションオブジェクトを設定してあげる
            result.Connection = GetDbConnection();
            return result;
        }

        public DbCommand GetDbCommand(string sql, Dictionary<string, object> parameters)
        {
            var result = GetDbCommand();
            result.CommandText = sql;

            if (parameters != null)
            {
                AddCommandParameters(result, parameters);
            }

            return result;
        }

        /// <summary>
        /// 接続する DBMS ごとに適切な DataAdapter クラスを取得します。
        /// </summary>
        /// <returns>DataAdapter クラス</returns>
        protected abstract DbDataAdapter GetDataAdapter();

        /// <summary>
        /// SQLを実行し、データソースから単一値を取得します。
        /// TtlDAL アセンブリ内部からのみ利用可能です。
        /// </summary>
        /// <param name="sql">実行する SQL コマンド</param>
        /// <returns>
        /// 取得した単一値。オブジェクト型で返すため受け取り側で適切な型キャストが必要です。
        /// </returns>
        public object GetScalarData(string sql, Dictionary<string, object> parameters = null)
        {
            using (DbCommand cmd = GetDbCommand(sql, parameters))
            {
                // 接続オープン
                cmd.Connection.Open();
                cmd.Prepare();

                object result = cmd.ExecuteScalar();

                // 接続クローズ
                cmd.Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// SQL を実行し、データソースから結果セットを取得します。
        /// </summary>
        /// <param name="sql">
        /// 実行する SQL コマンド
        /// </param>
        /// <returns>
        /// 取得した結果セット。<see cref="DataTable"/> 型で取得します。
        /// </returns>
        public DataTable GetDataTable(string sql, Dictionary<string, object> parameters = null)
        {
            using (var da = GetDataAdapter())
            using (var cmd = GetDbCommand(sql, parameters))
            {
                da.SelectCommand = cmd;
                // 接続のオープンクローズは DbDataAdapter.Fill メソッドが自動で行ってくれる
                var result = new DataTable("result");
                da.Fill(result);
                return result;
            }
        }

        /// <summary>
        /// SQL を実行し、データソースから 適切なDBDataReader クラス（結果取得前方向カーソル）を取得します。
        /// このメソッドはパフォーマンス的に利用せざるを得ない場面に備えて準備されています。
        /// パフォーマンス問題は解決するために、まず結果セットの受け取り方（ロジック）の工夫を検討してください。
        /// このメソッドの利用時は必ず前もってレビューを行ってください。
        /// </summary>
        /// <remarks>
        /// 取得した DbDataReader, および取得に利用した DbCommand.Connection オブジェクトは、
        /// クローズされていません。それぞれ明示的に Close() メソッドを呼び出してください。
        /// </remarks>
        /// <param name="sql">
        /// 実行する SQL コマンド
        /// </param>
        /// <param name="command">
        /// 実行に利用する DbCommand オブジェクト
        /// </param>
        /// <returns>
        /// 取得した前方向カーソル。
        /// </returns>
        public DbDataReader GetDataReader(string sql, Dictionary<string, object> parameters = null)
        {
            using (var cmd = GetDbCommand(sql, parameters))
            {
                cmd.Connection.Open();
                cmd.Prepare();
                DbDataReader result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return result;
            }
        }

        /// <summary>
        /// 更新系クエリを実行します。
        /// </summary>
        /// <param name="sql">
        /// 実行する SQL コマンド。
        /// </param>
        /// <returns>
        /// 影響を受けた件数。
        /// </returns>
        public int Execute(string sql, Dictionary<string, object> parameters = null)
        {
            using (DbCommand cmd = GetDbCommand(sql, parameters))
            {
                cmd.Connection.Open();
                cmd.Prepare();

                int result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 実行 SQL をファイルから読み取り、その内容を実行します。
        /// </summary>
        /// <param name="sqlFileName">
        /// SQL ファイル名。
        /// </param>
        /// <returns>
        /// 影響を受けた件数。
        /// </returns>
        public int ExecuteFromFile(string sqlFileName)
        {
            var sql = string.Empty;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(sqlFileName))
            {
                sql = sr.ReadToEnd();
                sr.Close();
            }
            return Execute(sql);
        }
        public void ActionForDataReaderRead(string sql, Action<DbDataReader> action)
        {
            ActionForDataReaderRead(sql, null, action);
        }

        public void ActionForDataReaderRead(string sql, Dictionary<string, object> parameters, Action<DbDataReader> action)
        {
            using (var reader = GetDataReader(sql, parameters))
            {
                while (reader.Read())
                {
                    action(reader);
                }
            }
        }

        public IEnumerable<T> EnumerateForDataReaderRead<T>(string sql, Func<DbDataReader, T> func)
        {
            return EnumerateForDataReaderRead(sql, func);                
        }
        public IEnumerable<T> EnumerateForDataReaderRead<T>(string sql, Dictionary<string, object> parameters, Func<DbDataReader, T> func)
        {
            using (var reader = GetDataReader(sql, parameters))
            {                
                while (reader.Read())
                {
                    yield return func(reader);
                }
            }
        }
        #endregion
    }
}