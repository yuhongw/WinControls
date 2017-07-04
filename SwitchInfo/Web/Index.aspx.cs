using SwitchInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Index : System.Web.UI.Page
    {
        protected List<Site> Data;
        protected int Interval;
        protected int IntervalUpdate;       //从交换机更新数据的间隔
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSvc siteSvc = new SiteSvc(Properties.Settings.Default.DataPath,"");
            this.Data = siteSvc.ReadData();
            Interval = Properties.Settings.Default.IntervalClient;
            IntervalUpdate = Properties.Settings.Default.IntervalUpdate;
        }
    }
}