using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Pilotvision.Common.DataAccess
{
    /// <summary>
    /// <see cref="DataAccess"/> の MySQL 版実装です。
    /// DBとの接続・SQLの実行などは、すべてこのクラスを介して行います。
    /// </summary>
    public class MySqlDataAccess : DataAccess,IDataAccess
    {
        public MySqlDataAccess() : base() { }
        public MySqlDataAccess(string serverName, string userName, string password, string databaseName) : base(serverName, userName, password, databaseName) { }
        public MySqlDataAccess(DataAccessConfig config) : base(config) { }

        public override DbConnection GetDbConnection()
        {
            return new MySqlConnection(GetDbConnectionString());
        }

        protected override string GetDbConnectionString()
        {
            string result = string.Empty;

            result = String.Format("Server={0};Uid={1};Pwd={2}", Config.ServerName, Config.UserName, Config.Password);
            if (!string.IsNullOrEmpty(Config.DatabaseName))
            {
                result += ";Database=" + Config.DatabaseName;
            }
            result += string.Format(";Connection Timeout={0};CharSet=utf8", (ConnectionTimeout == 0 ? 60 : ConnectionTimeout));

            return result;
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MySqlCommand();
        }

        protected override DbDataAdapter GetDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        protected override DbParameter CreateParameter(DbCommand cmd, string name, object value)
        {
            var result = (cmd as MySqlCommand).CreateParameter();

            result.ParameterName = name;
            result.Direction = ParameterDirection.Input;

            if (value == null)
            {
                result.Value = DBNull.Value;
            }
            else
            {
                result.MySqlDbType = GetMySqlDbType(value);
                result.Value = value;

                if (result.MySqlDbType == MySqlDbType.VarChar)
                {
                    int size = value.ToString().Length;
                    if (size > 256)
                    {
                        result.Size = value.ToString().Length;
                    }
                    else
                    {
                        result.Size = 256;
                    }
                }
            }

            return result;
        }

        private static MySqlDbType GetMySqlDbType(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Int16:
                    return MySqlDbType.Int16;
                case TypeCode.Int32:
                    return MySqlDbType.Int32;
                case TypeCode.Int64:
                    return MySqlDbType.Int64;
                case TypeCode.UInt16:
                    return MySqlDbType.UInt16;
                case TypeCode.UInt32:
                    return MySqlDbType.UInt32;
                case TypeCode.UInt64:
                    return MySqlDbType.UInt64;
                case TypeCode.Decimal:
                    return MySqlDbType.Decimal;
                case TypeCode.Double:
                case TypeCode.Single:
                    return MySqlDbType.Float;
                case TypeCode.DateTime:
                    return MySqlDbType.DateTime;
                default:
                    return MySqlDbType.VarChar;
            }
        }
    }
}