using System.Data.SQLite;
using SimpleQRCodeSystem.IModels;
using SimpleQRCodeSystem.Models;
using SimpleQRCodeSystem.Repositories;

namespace SimpleQRCodeSystem.Services
{
    class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;

        public BadgeService(IBadgeRepository badgeRepository)
        {
            this._badgeRepository = badgeRepository;
        }

        public IBadge Find(string code)
        {
            var badge = new Badge();
            var sql = "SELECT * FROM badge WHERE code = @code LIMIT 1";
            var cmd = new SQLiteCommand(sql, _badgeRepository.SqLiteConnection);
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
                _badgeRepository.SqLiteConnection
            );
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
        }

        public void Insert(string code)
        {
            SQLiteCommand command = new SQLiteCommand(
                "INSERT OR IGNORE INTO badge (id, code, usedAt) VALUES (null, @code, null);",
                _badgeRepository.SqLiteConnection
            );
            command.Parameters.AddWithValue("@code", code);
            command.ExecuteNonQuery();
        }
    }
}
