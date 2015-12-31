using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathKeeper.Wdq
{
    public class WdqResult
    {
        public Status status { get; set; }
        public int[] items { get; set; }

        public class Status
        {
            public string error { get; set; }
            public int items { get; set; }
            public string querytime { get; set; }
            public string parsed_query { get; set; }
        }

    }
}
