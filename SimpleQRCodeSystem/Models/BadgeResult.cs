namespace SimpleQRCodeSystem.Models
{
    public class BadgeResult
    {
        public Badge Badge { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }

        public BadgeResult()
        {
            Label = "Error";
            Color = "Red";
        }
    }
}
