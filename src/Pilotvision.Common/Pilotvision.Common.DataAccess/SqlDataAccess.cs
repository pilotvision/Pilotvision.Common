using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Pilotvision.Common.DataAccess
{
    /// <summary>
    /// <see cref="DataAccess"/> の Microsoft SQL Server 版実装です。
    /// DBとの接続・SQLの実行などは、すべてこのクラスを介して行います。
    /// </summary>
    public class SqlDataAccess : DataAccess, IDataAccess
    {
        public SqlDataAccess() : base() { }
        public SqlDataAccess(string serverName, string userName, string password, string databaseName) : base(serverName, userName, password, databaseName) { }
        public SqlDataAccess(DataAccessConfig config) : base(config) { }

        public override DbConnection GetDbConnection()
        {
            return new SqlConnection(GetDbConnectionString());
        }

        protected override string GetDbConnectionString()
        {
            string result = string.Empty;

            result += string.Format("Data Source={0}", Config.ServerName);
            if (!string.IsNullOrEmpty(Config.DatabaseName))
            {
                result += string.Format(";Initial Catalog={0}", Config.DatabaseName);
            }
            result += string.Format(";Connection Timeout={0}", ConnectionTimeout);
            if (Config.UserName == string.Empty)
            {
                result += ";Integrated Security=True";
            }
            else
            {
                result += string.Format(";User ID={0}", Config.UserName);
                result += string.Format(";Password={0}", Config.Password);
                result += ";Persist Security Info=False";
            }

            return result;
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SqlCommand();
        }

        protected override DbDataAdapter GetDataAdapter()
        {
            return new SqlDataAdapter();
        }

        protected override DbParameter CreateParameter(DbCommand cmd, string name, object value)
        {
            var result = (cmd as SqlCommand).CreateParameter();
            result.ParameterName = name;
            result.Direction = ParameterDirection.Input;

            if (value == null)
            {
                result.SqlDbType = SqlDbType.Decimal;
                result.SqlValue = DBNull.Value;
                result.Scale = 2;
                result.Size = 18;
                result.Precision = 18;
            }
            else
            {
                result.SqlDbType = GetSqlDbType(value);
                result.SqlValue = value;

                if (result.SqlDbType == SqlDbType.NVarChar)
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
                else if (result.SqlDbType == SqlDbType.Decimal)
                {
                    result.Scale = 2;
                    result.Size = 18;
                    result.Precision = 18;
                }
            }

            return result;
        }
    
        private static SqlDbType GetSqlDbType(object value)
        {
            if (value == null)
            {
                //throw new ArgumentNullException();
                return SqlDbType.Variant;
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return SqlDbType.Int;
                case TypeCode.Decimal:
                    return SqlDbType.Decimal;
                case TypeCode.Double:
                case TypeCode.Single:
                    return SqlDbType.Float;
                case TypeCode.DateTime:
                    return SqlDbType.DateTime;
                default:
                    return SqlDbType.NVarChar;
            }
        }
    }
}