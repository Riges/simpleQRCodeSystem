using System.Collections.Generic;
using SimpleQRCodeSystem.Models;

namespace SimpleQRCodeSystem.Repositories
{
    public interface IBadgeRepository
    {
        Badge Find(string code);
        void SetUsedAt(string code);
        void Insert(List<string> codes);
    }
}
