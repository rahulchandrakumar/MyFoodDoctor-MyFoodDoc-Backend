using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.FileProcessors
{
    public class CouponFileProcessor
    {
        public static async Task<IEnumerable<string>> ReadCouponFile(byte[] file)
        {
            var couponCodes = new List<string>();
            using (var ms = new MemoryStream(file))
            using (var sr = new StreamReader(ms))
            {
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        couponCodes.Add(line.Trim());
                    }
                }
            }
            return couponCodes;
        }
    }
}
