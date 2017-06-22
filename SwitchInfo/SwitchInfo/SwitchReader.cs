using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchInfo
{
    public class SwitchReader
    {

        public string Get1_BandWithInputValue()
        {
            return GetValueAfterColumn(Get1_Bandwith(true), 0);
        }

        public string Get1_BandWithOutputValue()
        {
            return GetValueAfterColumn(Get1_Bandwith(true), 1);
        }

        public string Get2_onlineUsersValue()
        {
            return GetValueAfterColumn(Get2_onlineUsers(true), 0);
        }

        public string Get3_accessuser_sl2Value()
        {
            return GetValueAfterColumn(Get3_accessuser_sl2(true), 0);
        }

        public string Get3_accessuser_sl3Value()
        {
            return GetValueAfterColumn(Get3_accessuser_sl3(true), 0);
        }

        public string Get3_accessuser_sl4Value()
        {
            return GetValueAfterColumn(Get3_accessuser_sl4(true), 0);
        }

        //从串数组中选择指定的索引的行，并取出冒号后面的串
        private string GetValueAfterColumn(List<string> list, int index)
        {
            string result = "";
            if (list.Count > index)
            {
                result = GetStringAfterColumn(list[index]);
            }
            return result;
        }

        private string GetStringAfterColumn(string v)
        {
            var values = v.Split(':');
            if (values.Count() > 1)
            {
                return values[1];
            }
            else
                return "";
        }

        /// <summary>
        /// 批处理取得数据，保存成文件
        /// </summary>
        public void BatchGet()
        {
            Get1_Bandwith();
            Get2_onlineUsers();
            Get3_accessuser_sl2();
            Get3_accessuser_sl3();
            Get3_accessuser_sl4();
        }

        public List<string> Get1_Bandwith(bool fromCache=false)
        {
            string[] dests = {
                "Input bandwidth utilization",
                "Output bandwidth utilization"
            };

            return GetInfoByParams("script_1_bandwith.txt", "out_1.txt", dests,fromCache);
        }

        public List<string> Get2_onlineUsers(bool fromCache = false)
        {
            string[] dests = {
                "Max online users since startup"
            };

            return GetInfoByParams("script_2_onlineusers.txt", "out_2.txt", dests, fromCache);
        }


        public List<string> Get3_accessuser_sl2(bool fromCache = false)
        {
            string[] dests = {
                "Total users"
            };

            return GetInfoByParams("script_3_accessuser_sl2.txt", "out_3_sl2.txt", dests, fromCache);
        }

        public List<string> Get3_accessuser_sl3(bool fromCache = false)
        {
            string[] dests = {
                "Total users"
            };

            return GetInfoByParams("script_3_accessuser_sl3.txt", "out_3_sl3.txt", dests,fromCache);
        }

        public List<string> Get3_accessuser_sl4(bool fromCache=false)
        {
            string[] dests = {
                "Total users"
            };

            return GetInfoByParams("script_3_accessuser_sl4.txt", "out_3_sl4.txt", dests,fromCache);
        }

        public List<string> GetInfoByParams(string scriptFile, string outFile, string[] dests, bool fromCache)
        {
            List<string> result = new List<string>();
            string rootPath = Path.GetDirectoryName(Properties.Settings.Default.tst10);
            outFile = $"{rootPath}\\{outFile}";

            if (!fromCache)
            {
                //刷新本地文件
                string cmd = $"/r:{rootPath}\\{scriptFile} /o:{outFile}";
                GetInfoTelnet(cmd, outFile);
            }

            var lines = File.ReadAllLines(outFile);
            foreach (string dest in dests) FindAndAppend(lines, dest, result);
            return result;
        }

        private void FindAndAppend(string[] lines, string seek, List<string> dest)
        {
            foreach (var line in lines)
            {
                if (line.Contains(seek))
                {
                    dest.Add(line);
                    break;
                }
            }
        }

        public bool GetInfoTelnet(string cmd, string outFile)
        {
            bool result = false;
            //启动telnet,连接
            string telnetTool = Properties.Settings.Default.tst10;

            Process p = Process.Start(telnetTool, cmd);
            if (p.WaitForExit(5000))
            {
                //success 
                if (File.Exists(outFile))
                {
                    result = true;
                }
            }
            else
            {
                p.Kill();
            }
            return result;
        }
    }
}
