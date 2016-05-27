using System.IO;
using SimpleQRCodeSystem.Models;
using SimpleQRCodeSystem.Repositories;

namespace SimpleQRCodeSystem.Services
{
    internal class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;

        public BadgeService(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        public BadgeResult Find(string code)
        {
            var badge = _badgeRepository.Find(code);
            var badgeResult = new BadgeResult();

            if (badge.Id == 0)
            {
                badgeResult.Label = "Billet NON Valide";
                badgeResult.Color = "Red";
            }
            else
            {
                badgeResult.Badge = badge;
                if (badge.Used)
                {
                    badgeResult.Label = "Billet Valide, mais déjà utilisé";
                    badgeResult.Color = "Red";
                }
                else
                {
                    _badgeRepository.SetUsedAt(code);
                    badgeResult.Label = "Billet Valide";
                    badgeResult.Color = "Green";
                }
            }

            return badgeResult;
        }

        public BadgeResult Import(string path)
        {
            var reader = new StreamReader(File.OpenRead(@path));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line?.Split('|');
                if (values?.Length == 21)
                {
                    _badgeRepository.Insert(values[1]);
                }
            }

            var badgeResult = new BadgeResult
            {
                Label = "Donnés importés",
                Color = "LightSkyBlue"
            };

            return badgeResult;
        }
    }
}
