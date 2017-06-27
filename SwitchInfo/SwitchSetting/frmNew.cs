using SwitchInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SwitchSetting
{
    public partial class frmNew : Form
    {
        public Site data;
        SiteSvc siteSvc;
        public frmNew()
        {
            InitializeComponent();
        }

        public static frmNew GetForm(SiteSvc svc)
        {
            frmNew frm = new frmNew();
            frm.siteSvc = svc;
            return frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Site newSite = this.siteSvc.GetSiteTemplate();
            newSite.Ip = txtIp.Text.Trim();
            newSite.Name = txtName.Text.Trim();
            newSite.UserName = txtUserName.Text;
            newSite.Pwd = txtPwd.Text;

            string vInfo = "";
            if (newSite.IsValid( this.siteSvc.ReadData(),out vInfo))
            {
                //数据在data里
                data = newSite;
                Close();
            }
            else
            {
                Helpers.ShowError(vInfo);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Site site = this.siteSvc.GetDataSample();
            txtIp.Text = site.Ip;
            txtName.Text = site.Name;
            txtUserName.Text = site.UserName;
            txtPwd.Text = site.Pwd;
        }
    }
}
