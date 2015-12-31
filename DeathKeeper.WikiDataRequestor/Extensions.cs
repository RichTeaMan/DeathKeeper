using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData
{
    public static class Extensions
    {
        public static NodaTime.Instant? AsDateTime(this string dateTimeStr)
        {
            if (dateTimeStr == null)
                return null;
            var result = InstantPattern.ExtendedIsoPattern.Parse(dateTimeStr.Replace("-00", "-01").Replace("+", ""));
            return result.Value;
        }
    }
}
