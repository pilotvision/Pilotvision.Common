namespace Pilotvision.Common.DataAccess
{
    /// <summary>
    /// 接続先のデータソースの種類を表します。
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        SqlServer,
        /// <summary>
        /// Microsoft SQL Server Compact
        /// </summary>
        SqlServerCompact,
        /// <summary>
        /// MySql
        /// </summary>
        MySql,
    }
}