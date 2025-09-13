namespace LinkShortener.Services
{
    public interface IUrlShorteningService
    {
        Task<string> GenerateUniqueCode();
    }
}
