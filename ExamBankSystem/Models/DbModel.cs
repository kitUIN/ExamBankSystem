using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;

namespace ExamBankSystem.Models
{
    public interface IDbModel
    {
        T FromDb<T>(SqliteDataReader query);
    }
}