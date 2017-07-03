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
            txtRootPath.Text = rootPath;
            txtInterval.Text = Properties.Settings.Default.interval.ToString();
            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
            svc = new SiteSvc(rootPath,Helpers.GetOriginToolPath());
            data = svc.ReadData();
            BindData();
        }

        private void BindData()
        {
            gv.AutoGenerateColumns = false;
            gv.DataSource = null;
            gv.DataSource = data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmNew frm = frmNew.GetForm(svc);
            //frm.ShowDialog();
            var site = svc.GetDataSample();
            site.Id = data.Count==0?1:data.Max(x => x.Id) + 1;
            data.Add(site);
            //svc.AddNewSite(site);
            BindData();
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
                    //svc.DeleteSite(gv.CurrentRow.DataBoundItem as Site);
                    data.Remove(data.First(x => x.Id == (gv.CurrentRow.DataBoundItem as Site).Id));
                    BindData();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            svc.SaveData(data);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(txtInterval.Text, out val) && val > 0)
            {
                Properties.Settings.Default.interval = val;
            }
            else
            {
                txtInterval.Text = Properties.Settings.Default.interval.ToString();
                Helpers.ShowError("更新时间间隔设置错误。");
                
            }
        }
    }
}
