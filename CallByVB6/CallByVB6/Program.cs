using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallByVB6
{
    //for test
    class Program
    {
        public static void Main()
        {
            Utility util = new Utility();
            Console.WriteLine(util.StrToPinyin("你好,做饭.回家还是不回家？"));
            Console.ReadKey();
        }
    }
}
