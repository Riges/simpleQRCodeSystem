namespace SimpleQRCodeSystem.IModels
{
    public interface IBadge
    {
        int Id { get; set; }

        string Code { get; set; }
        bool Used { get; set; }
    }
}
