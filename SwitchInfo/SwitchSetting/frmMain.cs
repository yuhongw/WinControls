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
    public partial class frmMain : Form
    {
        SiteSvc svc;
        public frmMain()
        {
            InitializeComponent();
            timer.Interval =(int) Properties.Settings.Default.interval*1000;
            timer.Start();
            listBox1.Items.Add("Start updating..");
            svc = new SiteSvc(Helpers.GetRootPath(),Helpers.GetOriginToolPath());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出?" ,"Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            new frmSetting().ShowDialog();
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                svc.UpdateDataFromRemote();
                Invoke(new Action(() =>
                {
                    listBox1.Items.Add($"Updated:{DateTime.Now}");
                    while (listBox1.Items.Count > 15) { listBox1.Items.RemoveAt(0); }
                }));
            }
            catch (Exception ex)
            {
                listBox1.Items.Add($"***Error: {ex.Message}***");
            }
        }
    }
}
