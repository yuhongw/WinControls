using SwitchInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            string rootPath = Helpers.GetRootPath();
            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
            svc = new SiteSvc(rootPath,Helpers.GetOriginToolPath());
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
            frmNew frm = frmNew.GetForm(svc);
            frm.ShowDialog();
            if (frm.data!=null)
            {
                svc.AddNewSite(frm.data);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要删除吗?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (gv.CurrentRow.DataBoundItem != null)
                {
                    svc.DeleteSite(gv.CurrentRow.DataBoundItem as Site);
                    BindData();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(textBox1.Text, out val) && val>0)
            {
                Properties.Settings.Default.interval = val;
            }
        }
    }
}
