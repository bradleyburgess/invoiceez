using System.Security.Cryptography;
using System.Text;

namespace Api.Services;

public class Sha256HashingService : IHashingService
{
    public string Hash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}

public interface IHashingService
{
    public string Hash(string input);
}