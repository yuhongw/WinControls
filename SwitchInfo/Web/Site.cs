using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Web
{
    public class Site
    {
        public int Id { get; set; }             //每个地点的Id,数据以此作为文件夹
        public string Name { get; set; }        //名称
        public int X { get; set; }              //显示位置
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
                    new Site{  Id=1 , Name="农安", X=100, Y=100 
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

        //从原始数据处更新数据
        public static void UpdateData()
        {
            var list = ReadData();
            {
                foreach (Site site in list)
                {
                    UpdateData(site);
                }
            }

            SaveData(list);
        }

        private static void UpdateData(Site site)
        {
            SwitchInfo.SwitchReader swReader = new SwitchInfo.SwitchReader();
            string path = $"{Properties.Settings.Default.DataPath}\\{site.Id}";
            if (Directory.Exists(path))
            {
                site. swReader.Get1_BandWithInputValue();
            }
        }
    }
}