namespace LinkShortener.DTO
{
    public class UrlDTO
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
