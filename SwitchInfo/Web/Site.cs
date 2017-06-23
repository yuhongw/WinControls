using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Web
{
    public class KV
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Site
    {
        public int Num { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<KV> Values { get; set; }
    }



    public class SiteSvc
    {
        private string RootPath;
        public SiteSvc(string path)
        {
            RootPath = path;
        }
        public  List<Site> ReadData()
        {
            string fn = $"{RootPath}/sites.xml";
            XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
            return ser.Deserialize(System.IO.File.OpenRead(fn)) as List<Site>;
        }

        public  void SaveData(List<Site> list)
        {
            string fn = $"{RootPath}/sites.xml";
            XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
            if (File.Exists(fn)) File.Delete(fn);
            using (Stream fs = File.OpenWrite(fn))
            {
                ser.Serialize(fs, list);
            }
        }

        public List<Site> GetSample()
        {
            List<Site> site = new List<Site>()
            {
            new Site { Num = 1, Name = "农安", X = 100, Y = 100, Values = new List<KV>{
                new KV{ Key="输入带宽",Value="10%"},
                new KV{ Key="在线用户",Value="12345"},
                new KV{ Key="访问用户SL2",Value="12"},
                new KV{ Key="访问用户SL3",Value="123"},
                new KV{ Key="访问用户SL4",Value="1234"}
            }}};
            return site;
        }
    }
}