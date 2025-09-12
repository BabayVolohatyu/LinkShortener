namespace LinkShortener.Services
{
    public interface IJwtService
    {
        string Generate(int? id = null, string? role = null);
    }
}
