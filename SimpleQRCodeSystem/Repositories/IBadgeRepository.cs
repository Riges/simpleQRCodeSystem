using System.Data.SQLite;

namespace SimpleQRCodeSystem.Repositories
{
    public interface IBadgeRepository
    {
        SQLiteConnection SqLiteConnection { get; }
    }
}
