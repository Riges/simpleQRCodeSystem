using SimpleQRCodeSystem.Models;

namespace SimpleQRCodeSystem.Services
{
    public interface IBadgeService
    {
        BadgeResult Find(string code);
        BadgeResult Import(string path);
    }
}
