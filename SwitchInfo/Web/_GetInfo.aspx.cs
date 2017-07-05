﻿using SwitchInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class _GetInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            SiteSvc siteSvc = new SiteSvc(Properties.Settings.Default.DataPath, "");
            string cmd = Request.QueryString["id"];
            if (cmd == "lastUpdateTime")
            {
                DirectoryInfo dir = new DirectoryInfo(Properties.Settings.Default.DataPath);
                var dirs =  dir.GetDirectories();
                if (dirs.Count() > 0)
                {
                     FileInfo finfo = dirs.First().GetFiles("out*.txt").First();
                    Response.Write(finfo.LastWriteTime);
                }
            }
            Response.End();
        }
    }
}