namespace PasswordGenerator.Core
{
    public interface ICryptographer
    {
        string Encrypt(string input);
    }
}
