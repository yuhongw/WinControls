﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Web
{
    public class Site
    {
        public int Num { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<KeyValuePair<string,string>> Values { get; set; }
    }

    public class SiteSvc
    {
        public static List<Site> ReadData()
        {
            string fn = HttpContext.Current.Server.MapPath("~/Data/sites.xml");
            XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
            return ser.Deserialize(System.IO.File.OpenRead(fn)) as List<Site>;
        }

        public static void SaveData(List<Site> list)
        {
            string fn = HttpContext.Current.Server.MapPath("~/Data/sites.xml");
            XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
            if (File.Exists(fn)) File.Delete(fn);
            using (Stream fs = File.OpenWrite(fn))
            {
                ser.Serialize(fs, list);
            }
        }

        public static List<Site> GetSample()
        {
            return new List<Site> {
                    new Site{  Num=1 , Name="农安", X=100, Y=100 
                    , Values= new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("输入带宽","10%"),
                        new KeyValuePair<string, string>("输出带宽","20%"),
                        new KeyValuePair<string, string>("在线用户","12345"),
                        new KeyValuePair<string, string>("在线用户","12345"),
                        new KeyValuePair<string, string>("在线用户","12345"),
                        new KeyValuePair<string, string>("在线用户","12345")
                    }
                    }
            };
        }
    }
}