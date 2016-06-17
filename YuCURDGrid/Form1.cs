using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestData;

namespace CURDGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*
            gv.SetColDataAgent(
                this.cityDataGridViewTextBoxColumn.DataPropertyName,
                (x) => FrmRef.ShowList(
                    () => TestData.KeyValueFac.GetCityList(),
                    (r) => {(bs.Current as Student).City = (r as KeyValue).Value; }
                    ));
             * */
            gv.SetColDataAgent("Gender", TestData.KeyValueFac.GetGenderList());
            gv.SetColDataAgent("City", TestData.KeyValueFac.GetCityList());

            this.txtCity.SetDataAgent(TestData.KeyValueFac.GetCityList());
            this.txtGender.SetDataAgent(TestData.KeyValueFac.GetGenderList());
            this.txtCity2.SetDataAgent(TestData.KeyValueFac.GetCityList());
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bs.DataSource = TestData.Student.GetList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

    }
}
