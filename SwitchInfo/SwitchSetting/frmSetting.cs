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
    public partial class frmSetting : Form
    {
        List<Site> data;
        SiteSvc svc;
        public frmSetting()
        {
            InitializeComponent();
            svc = new SiteSvc(Properties.Settings.Default.rootPath);
            BindData();

        }

        private void BindData()
        {
           
            gv.AutoGenerateColumns = false;
            data = svc.ReadData();
            gv.DataSource = null;
            gv.DataSource = data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmNew frm = frmNew.GetForm(data);
            frm.ShowDialog();
            if (!string.IsNullOrEmpty(frm.Result))
            {
                svc.AddNewSite(frm.Result);
                BindData();
            }
            else
            {
                Helpers.ShowError("已取消");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
