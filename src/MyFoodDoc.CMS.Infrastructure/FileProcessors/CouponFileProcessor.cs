using MyFoodDoc.Application.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.FileProcessors
{
    public class CouponFileProcessor
    {
        public static async Task<IEnumerable<string>> ReadCouponFile(byte[] file, CancellationToken cancellationToken = default)
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

        public static async Task<byte[]> MakeCouponFile(IList<Coupon> coupons, CancellationToken cancellationToken = default)
        {
            byte[] file = null;

            if (coupons?.Count > 0)
                using (var ms = new MemoryStream())
                {
                    using (var sr = new StreamWriter(ms))
                    {
                        foreach (var coupon in coupons.OrderBy(x => x.Redeemed))
                        {
                            await sr.WriteLineAsync($"{coupon.Code} {coupon.Redeemed != null} { (coupon.Redeemed == null ? "" : coupon.Redeemed.Value.ToShortDateString()) }");
                        }
                    }
                    file = ms.ToArray();
                }
            return file;
        }
    }
}
