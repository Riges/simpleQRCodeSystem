using SimpleQRCodeSystem.IModels;

namespace SimpleQRCodeSystem.Models
{
    public class Badge : IBadge
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public bool Used { get; set; }

        public Badge()
        {
            Used = false;
            Code = "";
        }
    }
}
