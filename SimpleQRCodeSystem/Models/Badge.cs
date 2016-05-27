using NodaTime;

namespace SimpleQRCodeSystem.Models
{
    public class Badge
    {
        public string Code { get; set; }
        public Instant? UsedAt { get; set; }
    }
}
