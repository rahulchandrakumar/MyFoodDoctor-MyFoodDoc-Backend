namespace MyFoodDoc.CMS.Application.Common
{
    public interface IHashingManager
    {
        byte[] Hash(string clearText, int iterations = 10000);
        string HashToString(string clearText, int iterations = 10000);
        bool Verify(string clearText, byte[] data);
        bool Verify(string clearText, string data);
    }
}