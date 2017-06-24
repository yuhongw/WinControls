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
        List<Site> data;
        public string Result { get; set; }
        public frmNew()
        {
            InitializeComponent();
        }

        public static frmNew GetForm(List<Site> data)
        {
            frmNew frm = new frmNew();
            frm.data = data;
            return frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (name.Length > 0)
            {
                if (data.Count(x => x.Name == name) == 0)
                {
                    Result = name;
                    Close();
                }
                else
                {
                    Helpers.ShowError($"地点[{name}]已存在");
                }
            }
            else
            {
                Helpers.ShowError("地点错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
