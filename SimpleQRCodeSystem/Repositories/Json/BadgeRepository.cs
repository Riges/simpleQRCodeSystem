using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SimpleQRCodeSystem.IModels;
using SimpleQRCodeSystem.Models;

namespace SimpleQRCodeSystem.Repositories.Json
{
    internal class BadgeRepository : IBadgeRepository
    {
        private readonly string _databasePath = "Database.json";
        private readonly List<Badge> _badges;
        private readonly Random _rnd;


        public BadgeRepository()
        {
            _badges = new List<Badge>();
            _rnd = new Random();
            if (File.Exists(_databasePath))
            {
                var badges = JsonConvert.DeserializeObject<List<Badge>>(File.ReadAllText(_databasePath));

                _badges = badges;
            }
        }

        IBadge IBadgeRepository.Find(string code)
        {
            var result = _badges.FirstOrDefault(x => x.Code == code) ?? new Badge();

            return result;
        }

        void IBadgeRepository.Insert(string code)
        {
            _badges.Add(new Badge {Code = code, Id = _rnd.Next()});
            Save();
        }

        void IBadgeRepository.SetUsedAt(string code)
        {
            var badge = _badges.FirstOrDefault(x => x.Code == code);
            if (badge != null)
            {
                badge.Used = true;
            }
            Save();
        }

        private void Save()
        {
            File.WriteAllText(_databasePath, JsonConvert.SerializeObject(_badges));
        }
    }
}
