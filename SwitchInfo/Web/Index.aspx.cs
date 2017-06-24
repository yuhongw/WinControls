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
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSvc siteSvc = new SiteSvc(HttpContext.Current.Server.MapPath(@"~\Data"));
            this.Data = siteSvc.ReadData();
        }

        
    }
}