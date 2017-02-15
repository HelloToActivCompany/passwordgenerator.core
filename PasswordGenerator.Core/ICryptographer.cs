namespace PasswordGenerator.Core
{
    public interface ICryptographer
    {
        byte[] Encrypt(string input);
    }
}
