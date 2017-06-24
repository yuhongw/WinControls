using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SwitchInfo
{
    public class KV
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Site
    {
        public int Id { get; set; }             //每个地点的Id,数据以此作为文件夹
        public string Name { get; set; }        //名称
        public int X { get; set; }              //显示位置
        public int Y { get; set; }
        public List<KV> Values { get; set; }
    }



    public class SiteSvc
    {
        private List<string> err;
        public List<string> Err { get { return this.err; } }

        

        private string RootPath;    
        public SiteSvc(string path)
        {
            RootPath = path;
        }
       
        public List<Site> ReadData()
        {
            string fn = $"{RootPath}/sites.xml";
            XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
            using (var fs = System.IO.File.OpenRead(fn))
            {
                var list = ser.Deserialize(fs) as List<Site>;
                UpdateData(list);
                return list;
            }
        }

        public void AddNewSite(string name)
        {
            Site newSite = GetSiteTemplate();
            newSite.Name = name;
            var list = ReadData();
            int newId = list.Max(x => x.Id) + 1;
            newSite.Id = newId;
            list.Add(newSite);
            SaveData(list);
            int firstId = list.First().Id;
            Helpers.CopyDirectory(Path.Combine(RootPath,firstId.ToString()), Path.Combine(RootPath,newId.ToString()));
        }

        //从远程服务器更新数据
        public static void UpdateDataFromRemote(string rootPath)
        {
            SiteSvc svc = new SiteSvc(rootPath);
            List<Site> sites = svc.ReadData();
            foreach (Site site in sites)
            {
                using (SwitchInfo.SwitchReader reader = new SwitchInfo.SwitchReader($"{Properties.Settings.Default.rootPath}/{site.Id}"))
                {
                    reader.BatchGet();
                }
            }

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

        public List<Site> GetDataTemplate()
        {
            List<Site> site = new List<Site>()
            {
                GetSiteTemplate()
            };
            return site;
        }

        private Site GetSiteTemplate()
        {
            return new Site
            {
                Id = 1,
                Name = "",
                X = 100,
                Y = 100,
                Values = new List<KV>{
                new KV{ Key="输入带宽",Value=""},
                new KV{ Key="在线用户",Value=""},
                new KV{ Key="访问用户SL2",Value=""},
                new KV{ Key="访问用户SL3",Value=""},
                new KV{ Key="访问用户SL4",Value=""}
            }
            };
        }

        //从原始数据处更新数据-全部
        private void UpdateData(List<Site> list)
        {
                foreach (Site site in list)
                {
                    UpdateData(site);
                }
        }

        //更新单个数据
        private  void UpdateData(Site site)
        {
            string path = $"{RootPath}\\{site.Id}";
            if (Directory.Exists(path))
            {
                SwitchInfo.SwitchReader swReader = new SwitchInfo.SwitchReader(path);
                swReader.BatchGetValues();
                foreach (KV kv in site.Values)
                {
                    kv.Value = swReader.Values[kv.Key];
                }
            }
        }
    }
}