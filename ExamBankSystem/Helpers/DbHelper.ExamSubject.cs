using System.Collections.Generic;
using System.Threading.Tasks;
using ExamBankSystem.Models;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        /// <summary>
        /// 创建考试科目表
        /// </summary>
        private static void CreateExamSubjectsTable()
        {
            ExecuteReader(command =>
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS `ExamSubjects` (" +
                                          "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                                          "`subject` NVARCHAR(30) , " +
                                          "`createTime` TIMESTAMP NOT NULL, " +
                                          "`updateTime` TIMESTAMP NULL " +
                                          ")";
                    return command;
                }
            );
        }

        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static async Task<List<ExamSubject>> GetExamSubjectsAsync(long page = 1, int limit = 15)
        {
            return await GetAsync<ExamSubject>(page, limit);
        }

        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static async Task<ExamSubject> GetExamSubjectAsync(string name)
        {
            return await ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `ExamSubjects` WHERE `subject` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", name);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return new ExamSubject(query);
                }

                return null;
            });
        }

        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static async Task<ExamSubject> GetExamSubjectAsync(int id)
        {
            return await GetByIdAsync<ExamSubject>(id);
        }

        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static ExamSubject GetExamSubject(int id)
        {
            return GetById<ExamSubject>(id);
        }

        /// <summary>
        /// 插入考试科目到数据库中
        /// </summary>
        public static void InsertExamSubject(string name)
        {
            ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText =
                    $"INSERT INTO `ExamSubjects` VALUES (NULL, @Name, @CreateTime, @UpdateTime);";
                selectCommand.Parameters.AddWithValue("@Name", name);
                var t = DateTimeHelper.GetTimeStamp();
                selectCommand.Parameters.AddWithValue("@CreateTime", t);
                selectCommand.Parameters.AddWithValue("@UpdateTime", t);
                return selectCommand;
            });
        }

        /// <summary>
        /// 从数据库中删除考试科目
        /// </summary>
        public static void DeleteExamSubject(int id)
        {
            DeleteById<ExamSubject>(id);
        }

        /// <summary>
        /// 更新科目名称
        /// </summary>
        public static void UpdateExamSubjectName(int id, string newName)
        {
            ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText =
                    $"UPDATE `ExamSubjects` SET `subject` = @Name, `updateTime` = @UpdateTime WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@Name", newName);
                selectCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.GetTimeStamp());
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }

        /// <summary>
        /// 考试科目下是否有问题
        /// </summary>
        public static async Task<bool> ExamSubjectHasAnyQuestionAsync(int id)
        {
            return ((long)await ExecuteScalarAsync(selectCommand =>
            {
                selectCommand.CommandText = "SELECT COUNT(*) FROM Questions WHERE subjectId = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            })) > 0;
        }

    }
}