namespace PasswordGenerator.Core
{
    public interface IPasswordGenerator
    {
        string Generate(string input, string login = "");
    }
}
