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
        protected List<List<KeyValuePair<string, string>>> data;
        protected void Page_Load(object sender, EventArgs e)
        {
            var data = SiteSvc.GetSample();
            SiteSvc.SaveData(data);
            var list = SiteSvc.ReadData();
        }

        
    }
}