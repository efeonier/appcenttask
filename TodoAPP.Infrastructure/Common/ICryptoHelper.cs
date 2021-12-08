namespace TodoAPP.Infrastructure.Common
{
    public interface ICryptoHelper
    {
        string Hash(string input);
        bool Verify(string input, string hashedInput);
    }
}