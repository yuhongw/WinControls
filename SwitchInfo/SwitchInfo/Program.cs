using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchInfo
{
    class Program
    {
        public static void Main()
        {
            SwitchReader switchReader = new SwitchReader();

            /*
            Console.WriteLine("----Bandwidth----");
            Console.WriteLine( string.Join("\r\n", swithreader.Get1_Bandwith()));
            Console.WriteLine("----Online Users----");
            Console.WriteLine(string.Join("\r\n", swithreader.Get2_onlineUsers()));
            Console.WriteLine("----AccessUser slot 2----");
            Console.WriteLine(string.Join("\r\n", swithreader.Get3_accessuser_sl2()));
            Console.WriteLine("----AccessUser slot 3----");
            Console.WriteLine(string.Join("\r\n", swithreader.Get3_accessuser_sl3()));
            Console.WriteLine("----AccessUser slot 4----");
            Console.WriteLine(string.Join("\r\n", swithreader.Get3_accessuser_sl4()));
            */

            //switchReader.BatchGet();

            Console.WriteLine("----Bandwidth--Input--Value----");
            Console.WriteLine(switchReader.Get1_BandWithInputValue());
            Console.WriteLine("----Bandwidth--Output--Value----");
            Console.WriteLine(switchReader.Get1_BandWithOutputValue());

            Console.WriteLine("----Online Users---Value----");
            Console.WriteLine(switchReader.Get2_onlineUsersValue());

            Console.WriteLine("----Access Users sl2---Value----");
            Console.WriteLine(switchReader.Get3_accessuser_sl2Value());

            Console.WriteLine("----Access Users sl3---Value----");
            Console.WriteLine(switchReader.Get3_accessuser_sl3Value());

            Console.WriteLine("----Access Users sl4---Value----");
            Console.WriteLine(switchReader.Get3_accessuser_sl4Value());
            Console.Read();
        }
    }
}
