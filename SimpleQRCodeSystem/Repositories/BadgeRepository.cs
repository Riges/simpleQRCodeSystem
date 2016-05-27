using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using SimpleQRCodeSystem.Models;

namespace SimpleQRCodeSystem.Repositories
{
    internal class BadgeRepository : IBadgeRepository
    {
        private readonly string _databasePath = "Database.json";
        private readonly List<Badge> _badges;

        public BadgeRepository()
        {
            _badges = new List<Badge>();
            if (File.Exists(_databasePath))
            {
                _badges = JsonConvert.DeserializeObject<List<Badge>>(File.ReadAllText(_databasePath));
            }
        }

        public Badge Find(string code)
        {
            var result = _badges.FirstOrDefault(x => x.Code == code) ?? new Badge();

            return result;
        }

        public void Insert(List<string> codes)
        {
            foreach (var code in codes)
            {
                var result = _badges.FirstOrDefault(x => x.Code == code);
                if (result == null)
                {
                    _badges.Add(new Badge { Code = code });
                }
            }
            Save();
        }

        public void SetUsedAt(string code)
        {
            var badge = _badges.FirstOrDefault(x => x.Code == code);
            if (badge != null)
            {
                badge.UsedAt = SystemClock.Instance.Now;
            }
            Save();
        }

        private void Save()
        {
            File.WriteAllText(_databasePath, JsonConvert.SerializeObject(_badges));
        }
    }
}
