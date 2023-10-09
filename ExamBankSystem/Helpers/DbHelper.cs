using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;
using ExamBankSystem.Models;
using ExamBankSystem.Enums;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;

namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public partial class DbHelper
    {
        private static string _dbpath;

        /// <summary>
        /// 获取表名
        /// </summary>
        private static string GetTable<T>()
        {
            return typeof(T).Name + "s";
        }

        private static T ExecuteReader<T>(Func<SqliteCommand, SqliteCommand> command, Func<SqliteDataReader, T> res)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            return res(dbCommand.ExecuteReader());
        }

        private static object ExecuteScalar(Func<SqliteCommand, SqliteCommand> command)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            return dbCommand.ExecuteScalar();
        }

        private static async Task<object> ExecuteScalarAsync(Func<SqliteCommand, SqliteCommand> command)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            return await dbCommand.ExecuteScalarAsync();
        }

        private static async Task<T> ExecuteReaderAsync<T>(Func<SqliteCommand, SqliteCommand> command,
            Func<SqliteDataReader, T> res)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            return res(await dbCommand.ExecuteReaderAsync());
        }

        private static async void ExecuteReader(Func<SqliteCommand, SqliteCommand> command)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            await dbCommand.ExecuteReaderAsync();
        }

        #region Initialize

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static async void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("bank.sqlite",
                CreationCollisionOption.OpenIfExists);
            _dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "bank.sqlite");
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                CreateUsersTable();
                CreateKnowledgePointsTable();
                CreateExamSubjectsTable();
                CreateQuestionsTable();
                CreateTestPapersTable();
                CreateQuestionPapersTable();
            }
        }
        public static long GetCount<T>() where T : OrderModel
        {
            return  (long) ExecuteScalar(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"SELECT COUNT(*) FROM {table};";
                return selectCommand;
            } );
        }
        /// <summary>
        /// 从数据库中获取分页数据
        /// </summary>
        public static async Task<List<T>> GetAsync<T>(long page = 1, int limit = 15) where T : OrderModel
        {
            page--;
            return await ExecuteReaderAsync(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"SELECT * FROM {table} LIMIT @Limit OFFSET @Offset;";
                selectCommand.Parameters.AddWithValue("@Limit", limit);
                selectCommand.Parameters.AddWithValue("@Offset", page * limit);
                return selectCommand;
            }, query =>
            {
                var order = page * limit;
                var res = new List<T>();
                while (query.Read())
                {
                    var item = (T)Activator.CreateInstance(typeof(T), query);
                    item.Order = ++order;
                    res.Add(item);
                }

                return res;
            });
        }

        /// <summary>
        /// 获取实体类
        /// </summary>
        public static async Task<T> GetByIdAsync<T>(int id) where T : OrderModel
        {
            return await ExecuteReaderAsync(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"SELECT * FROM {table} WHERE `id` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", id);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return (T)Activator.CreateInstance(typeof(T), query);
                }

                return null;
            });
        }

        /// <summary>
        /// 获取实体类
        /// </summary>
        public static T GetById<T>(int id) where T : OrderModel
        {
            return ExecuteReader(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"SELECT * FROM {table} WHERE `id` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", id);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return (T)Activator.CreateInstance(typeof(T), query);
                }

                return null;
            });
        }

        /// <summary>
        /// 删除实体类
        /// </summary>
        public static void DeleteById<T>(int id) where T : OrderModel
        {
            ExecuteReader(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"DELETE FROM {table} WHERE `id` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", id);
                return selectCommand;
            });
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        public static async Task<long> CountAsync<T>() where T : OrderModel
        {
            return (long)await ExecuteScalarAsync(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText = $"SELECT COUNT(*) FROM {table};";
                return selectCommand;
            });
        }

        /// <summary>
        /// 搜索
        /// </summary>
        public static async Task<List<T>> SearchAsync<T>(string col, string keyword, long page,
            int limit = 15) where T : OrderModel
        {
            page--;
            if (keyword.Contains("%") && !keyword.Contains("_")) keyword = "%" + keyword + "%";
            return await ExecuteReaderAsync(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText =
                    $"SELECT * FROM {table} WHERE {col} LIKE @Keyword LIMIT @Limit OFFSET @Offset;";
                selectCommand.Parameters.AddWithValue("@Keyword", keyword);
                selectCommand.Parameters.AddWithValue("@Limit", limit);
                selectCommand.Parameters.AddWithValue("@Offset", page * limit);
                return selectCommand;
            }, query =>
            {
                var order = page * limit;
                var res = new List<T>();
                while (query.Read())
                {
                    var item = (T)Activator.CreateInstance(typeof(T), query);
                    item.Order = ++order;
                    res.Add(item);
                }

                return res;
            });
        }
        /// <summary>
        /// 搜索
        /// </summary>
        public static async Task<List<T>> SearchAsync<T>(string col, string keyword) where T : OrderModel
        {
            if (keyword.Contains("%") && !keyword.Contains("_")) keyword = "%" + keyword + "%";
            return await ExecuteReaderAsync(selectCommand =>
            {
                var table = GetTable<T>();
                selectCommand.CommandText =
                    $"SELECT * FROM {table} WHERE {col} LIKE @Keyword;";
                selectCommand.Parameters.AddWithValue("@Keyword", keyword);
                return selectCommand;
            }, query =>
            {
                var order = 0;
                var res = new List<T>();
                while (query.Read())
                {
                    var item = (T)Activator.CreateInstance(typeof(T), query);
                    item.Order = ++order;
                    res.Add(item);
                }

                return res;
            });
        }
        #endregion

    }
}