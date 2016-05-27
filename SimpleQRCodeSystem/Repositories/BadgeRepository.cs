using System.IO;
using System.Data.SQLite;
using SimpleQRCodeSystem.IModels;
using SimpleQRCodeSystem.Models;

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

        public IBadge Find(string code)
        {
            var badge = new Badge();
            var sql = "SELECT * FROM badge WHERE code = @code LIMIT 1";
            var cmd = new SQLiteCommand(sql, _sqLiteConnection);
            cmd.Parameters.AddWithValue("@code", code);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (!reader.IsClosed && reader.Read())
                {
                    badge.Id = int.Parse(reader["id"].ToString());
                    badge.Code = reader["code"].ToString();
                    badge.Used = reader["usedAt"].ToString() != "";
                    cmd.Cancel();
                    reader.Close();
                }
            }
            return badge;
        }

        public void SetUsedAt(string code)
        {
            SQLiteCommand command = new SQLiteCommand(
                "UPDATE badge set usedAt = datetime() WHERE code = @code;",
                _sqLiteConnection
            );
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
        }

        public void Insert(string code)
        {
            SQLiteCommand command = new SQLiteCommand(
                "INSERT OR IGNORE INTO badge (id, code, usedAt) VALUES (null, @code, null);",
                _sqLiteConnection
            );
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
        }
    }
}
