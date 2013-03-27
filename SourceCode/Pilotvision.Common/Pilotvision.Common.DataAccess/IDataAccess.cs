using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Pilotvision.Common.DataAccess
{
    public interface IDataAccess
    {
        int ConnectionTimeout { get; set; }
        int CommandTimeout { get; set; }
        object GetScalarData(string sql, Dictionary<string, object> parameters = null);
        DataTable GetDataTable(string sql, Dictionary<string, object> parameters = null);
        DbDataReader GetDataReader(string sql, Dictionary<string, object> parameters = null);
        int Execute(string sql, Dictionary<string, object> parameters = null);
        void ActionForDataReaderRead(string sql, Action<DbDataReader> action);
        void ActionForDataReaderRead(string sql, Dictionary<string, object> parameters, Action<DbDataReader> action);
        IEnumerable<T> EnumerateForDataReaderRead<T>(string sql, Func<DbDataReader, T> func);
        IEnumerable<T> EnumerateForDataReaderRead<T>(string sql, Dictionary<string, object> parameters, Func<DbDataReader, T> func);
    }
}
