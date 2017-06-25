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
    }
}
