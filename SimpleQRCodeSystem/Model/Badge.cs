namespace SimpleQRCodeSystem.Model
{
    public class Badge
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public bool Used { get; set; }

        public Badge()
        {
            this.Used = false;
            this.Code = "";
        }
    }
}
