using System.IO;
using System.Data.SQLite;

namespace SimpleQRCodeSystem.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly SQLiteConnection _sqLiteConnection;

        public SQLiteConnection SqLiteConnection => _sqLiteConnection;

        public BadgeRepository()
        {
            var initSql = "";
            if (!File.Exists("database.sqlite"))
            {
                SQLiteConnection.CreateFile("database.sqlite");
                initSql = "CREATE TABLE badge(id INTEGER PRIMARY KEY AUTOINCREMENT, code VARCHAR(50) UNIQUE, usedAt DateTime);";
            }
            _sqLiteConnection = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
            _sqLiteConnection.Open();
            if (initSql != "")
            {
                SQLiteCommand command = new SQLiteCommand(initSql, _sqLiteConnection);
                command.ExecuteNonQuery();
            }
        } 
    }
}
