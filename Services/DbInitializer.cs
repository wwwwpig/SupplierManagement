using SqlSugar;
using System;

namespace WebFirst.Services
{
    /// <summary>
    /// 全局数据库初始化器，非泛型，供所有 Repository<T> 共享同一个 SqlSugarScope
    /// </summary>
    public static class DbInitializer
    {
        private static string _connectionString;
        private static SqlSugarScope _db;

        public static void Configure(string connectionString, DbType dbType = DbType.SqlServer)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString 不能为空。", nameof(connectionString));

            if (_db != null)
                return;

            _connectionString = connectionString;

            _db = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = _connectionString,
                IsAutoCloseConnection = true
            },
            db =>
            {
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    // 可选日志处理
                };
            });
        }

        public static SqlSugarScope Db
        {
            get
            {
                if (_db == null)
                    throw new InvalidOperationException("Db 未初始化。请在应用启动时调用 DbInitializer.Configure(connectionString)。");
                return _db;
            }
        }
    }
}