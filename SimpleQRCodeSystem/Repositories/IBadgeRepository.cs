using SimpleQRCodeSystem.IModels;

namespace SimpleQRCodeSystem.Repositories
{
    public interface IBadgeRepository
    {
        IBadge Find(string code);
        void SetUsedAt(string code);
        void Insert(string code);
    }
}
