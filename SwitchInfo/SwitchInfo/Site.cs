﻿using System;
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
        public string Ip { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public int X { get; set; }              //显示位置
        public int Y { get; set; }

        public List<KV> Values { get; set; }
    }

    public static class SiteValidator
    {
        public static bool IsValid(this Site site,IEnumerable<Site> data, out string validatedInfo)
        {
            
            if (string.IsNullOrEmpty(site.Ip.Trim()))
            {
                validatedInfo = "Ip 是必填项";
                return false;
            }

            if (string.IsNullOrEmpty(site.Name.Trim()))
            {
                validatedInfo = "[名称]是必填项";
                return false;
            }

            if (string.IsNullOrEmpty(site.UserName.Trim()))
            {
                validatedInfo = "[用户名]是必填项";
                return false;
            }

            if (string.IsNullOrEmpty(site.Pwd.Trim()))
            {
                validatedInfo = "[密码]是必填项";
                return false;
            }


            if (data.Any(x=>x.Ip == site.Ip && x.Id != site.Id))
            {
                validatedInfo = "Ip 不能重复";
                return false;
            }

            if (data.Any(x => x.Name == site.Name && x.Id != site.Id))
            {
                validatedInfo = "Name 不能重复";
                return false;
            }
            validatedInfo = "";
            return true;
        }
    }

    public class SiteSvc
    {
        private List<string> err;
        public List<string> Err { get { return this.err; } }

        

        private string RootPath;
        private string OriginToolPath;
        public SiteSvc(string path,string originToolPath)
        {
            RootPath = path;
            OriginToolPath = originToolPath;
        }
       
        public List<Site> ReadData()
        {
            string fn = $"{RootPath}/sites.xml";
            if (File.Exists(fn))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Site>));
                using (var fs = System.IO.File.OpenRead(fn))
                {
                    var list = ser.Deserialize(fs) as List<Site>;
                    UpdateData(list);
                    return list;
                }
            }
            else
            {
                return new List<Site>();
            }
        }

        public void AddNewSite(Site site)
        {
            
            var list = ReadData();
            int newId = 1;
            if (list.Count > 0)
            {
                newId = list.Max(x => x.Id) + 1;
            }
            site.Id = newId;
            list.Add(site);
            SaveData(list);
            CopyFilesAndReplaceAddr( Path.Combine(RootPath,newId.ToString()),site);
        }

        private void CopyFilesAndReplaceAddr(string newPath, Site site)
        {
            var files = Directory.GetFiles(OriginToolPath);
            foreach (string f in files)
            {
                string fn = Path.GetFileName(f);
                string destFn = Path.Combine(newPath, fn);
                if (!Directory.Exists(Path.GetDirectoryName(destFn))) Directory.CreateDirectory(Path.GetDirectoryName(destFn));
                if (fn.StartsWith("script_"))
                {
                    string txt = File.ReadAllText(f)
                        .Replace("<IP>", site.Ip)
                        .Replace("<username>",site.UserName)
                        .Replace("<pwd>",site.Pwd);
                    File.WriteAllText(destFn, txt);
                }
                else
                {
                    File.Copy(f, destFn);
                }
            }
        }

        //从远程服务器更新数据
        public void UpdateDataFromRemote()
        {
            List<Site> sites = ReadData();
            foreach (Site site in sites)
            {
                using (SwitchInfo.SwitchReader reader = new SwitchInfo.SwitchReader($"{RootPath}/{site.Id}"))
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

        public Site GetDataSample()
        {
            Site site = GetSiteTemplate();
            site.Name = "农安";
            site.Ip = "222.168.94.36";
            site.UserName = "huawei";
            site.Pwd = "Huawei@123";
            return site;
        }

        public Site GetSiteTemplate()
        {
            return new Site
            {
                Id = 1,
                Name = "",
                X = 100,
                Y = 100,
                Values = new List<KV>{
                new KV{ Key="输入带宽",Value=""},
                new KV{ Key="输出带宽",Value=""},
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

        public void DeleteSite(Site site)
        {
            List<Site> list = ReadData();
            list.Remove(list.First(x=>x.Id == site.Id));
            SaveData(list);
        }
    }
}