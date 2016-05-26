using SimpleQRCodeSystem.IModels;

namespace SimpleQRCodeSystem.Services
{
    public interface IBadgeService
    {
        IBadge Find(string code);
        void SetUsedAt(string code);
        void Insert(string code);
    }
}
