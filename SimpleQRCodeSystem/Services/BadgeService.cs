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

        //Todo remove dbConnection in Service!
        public IBadge Find(string code, SQLiteConnection dbConnection)
        {
            var badge = new Badge();
            var sql = "SELECT * FROM badge WHERE code = @code LIMIT 1";
            var cmd = new SQLiteCommand(sql, dbConnection);
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
    }
}
