using System.IO;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IPdfService
    {
        byte[] ReplaceText(byte[] bytes, string oldValue, string newValue);
    }
}
