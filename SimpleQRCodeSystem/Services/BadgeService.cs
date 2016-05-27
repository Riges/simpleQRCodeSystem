using System.Collections.Generic;
using System.IO;
using NodaTime;
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
            var badgeResult = new BadgeResult();
            var badge = _badgeRepository.Find(code);

            if (string.IsNullOrEmpty(badge.Code))
            {
                badgeResult.Label = "Billet NON Valide";
                badgeResult.Color = "Red";
            }
            else
            {
                badgeResult.Badge = badge;
                if (badge.UsedAt.HasValue)
                {
                    badgeResult.Label = "Billet Valide, mais déjà utilisé à " + badge.UsedAt.Value.InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault());
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
            var codes = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line?.Split('|');
                if (values?.Length == 21)
                {
                    codes.Add(values[1]);
                }
            }

            _badgeRepository.Insert(codes);

            var badgeResult = new BadgeResult
            {
                Label = "Donnés importés",
                Color = "LightSkyBlue"
            };

            return badgeResult;
        }
    }
}
