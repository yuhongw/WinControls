using SwitchInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class _Data : System.Web.UI.Page
    {
        protected List<Site> Data{ get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSvc siteSvc = new SiteSvc(Properties.Settings.Default.DataPath, "");
            this.Data = siteSvc.ReadData();
            /*
            string siteIdstr = Request.QueryString["id"];
            int siteId = 0;
            if (int.TryParse(siteIdstr, out siteId))
            {
                this.site = siteSvc.GetSiteById(siteId);
            }*/
            
        }
    }
}