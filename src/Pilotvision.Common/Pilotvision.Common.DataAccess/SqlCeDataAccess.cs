using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;

namespace Pilotvision.Common.DataAccess
{
    public class SqlCeDataAccess : DataAccess, IDataAccess
    {
        public SqlCeDataAccess() : base() { }
        public SqlCeDataAccess(string serverName, string userName, string password, string databaseName) : base(serverName, userName, password, databaseName) { }
        public SqlCeDataAccess(DataAccessConfig config) : base(config) { }

        public override DbConnection GetDbConnection()
        {
            return new SqlCeConnection(GetDbConnectionString());
        }

        protected override string GetDbConnectionString()
        {
            string result = string.Empty;

            result = string.Format("Data Source={0};Enlist=False;Persist Security Info=False;Max Buffer Size=64000;Max Database Size=4091;Temp File Max Size=400;Default Lock Timeout=5000;Temp File Max Size=400", Config.DatabaseName);

            return result;
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SqlCeCommand();
        }

        protected override DbDataAdapter GetDataAdapter()
        {
            return new SqlCeDataAdapter();
        }

        protected override DbParameter CreateParameter(DbCommand cmd, string name, object value)
        {
            var result = (cmd as SqlCeCommand).CreateParameter();

            result.ParameterName = name;
            result.Direction = ParameterDirection.Input;

            if (value == null)
            {
                result.Value = DBNull.Value;
            }
            else
            {
                result.SqlDbType = GetSqlDbType(value);
                result.Value = value;

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
            }

            return result;
        }

        private static SqlDbType GetSqlDbType(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
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