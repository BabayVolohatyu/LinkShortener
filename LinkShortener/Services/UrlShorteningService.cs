using LinkShortener.Data;

namespace LinkShortener.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        public const int _numberOfChars = 8;
        private const string _alphabet = "ABCDEFGHIJKLMNOPQRSTWXYZabcdefgeijklmnopqrstwxyz0123456789";
        
        private readonly Random _random = new Random();
        private readonly IUrlRepository _urlRepository;

        public UrlShorteningService(IUrlRepository urlRepository) 
        {
            _urlRepository = urlRepository;
        }

        public async Task<string> GenerateUniqueCode()
        {
            var codeChars = new char[_numberOfChars];

            while (true)
            {
                for (var i = 0; i < _numberOfChars; i++)
                {
                    var randomIndex = _random.Next(_alphabet.Length - 1);

                    codeChars[i] = _alphabet[randomIndex];
                }
                var code = new string(codeChars);
                if (await _urlRepository.GetByCodeAsync(code) == null)
                {
                    return code;
                }
            }
        }
    }
}
