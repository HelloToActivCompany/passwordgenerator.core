namespace PasswordGeneratorCore
{
    public interface ICryptographer
    {
        string Encrypt(string input);
    }
}
