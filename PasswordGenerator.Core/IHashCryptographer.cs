namespace PasswordGenerator.Core
{
    public interface IHashCryptographer
    {
        byte[] Encrypt(byte[] data);
    }
}
