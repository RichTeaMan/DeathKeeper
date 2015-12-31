using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.WikiData
{
    public static class Extensions
    {
        public static DateTime AsDateTime(this string dateTimeStr)
        {
            var dateTime = DateTime.Parse(dateTimeStr.Replace("+", ""));
            return dateTime;
        }
    }
}
