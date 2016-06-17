using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Yu.Win.Controls
{

    public partial class FrmPopupCURDGrid : Form
    {
        string RefText;
        public Action<object> RetAction;
        private static FrmPopupCURDGrid ThisForm;

        private const int WM_LBUTTONDOWN = 513;   //   0x0201   
        private const int WM_LBUTTONUP = 514;   //   0x0202   
        
        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);


        public FrmPopupCURDGrid()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public static void ShowModal(string refText)
        {
            using (FrmPopupCURDGrid frm = new FrmPopupCURDGrid())
            {
                frm.RefText = refText;
                frm.ShowDialog();
            }
        }

        //public static void ShowList(Func<IEnumerable> GetListFunc, Action<object> okAction = null, int X = 0, int Y = 0)
        //{
        //    using (FrmPopupCURDGrid frm = new FrmPopupCURDGrid())
        //    {
        //        frm.RefreshList(GetListFunc());
        //        frm.RetAction = okAction;
        //        frm.ShowDialog();
        //    }
        //}

        //public static void ShowList(IEnumerable data, Action<object> okAction = null)
        //{
        //    using (FrmPopupCURDGrid frm = new FrmPopupCURDGrid())
        //    {
        //        frm.RetAction = okAction;
        //        frm.gv.DataSource = data;
        //        frm.ShowDialog();
        //    }
        //}



        private void returnValue()
        {
            if (this.RetAction != null)
            {
                this.RetAction(gv.CurrentRow.DataBoundItem);
            }
            this.Hide();
        }

        private void gv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            returnValue();
        }

        internal void RefreshList(IEnumerable data)
        {
            gv.DataSource = data;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        public void ProcessKey(Keys keyData)
        {
            int row;
            if (gv.Rows.Count>0)
            {
                if (keyData == Keys.Up || keyData == Keys.Down)
                {
                    row = gv.CurrentRow.Index;
                    if (keyData == Keys.Up)
                    {
                        row--;
                        if (row < 0) row = gv.Rows.Count - 1;
                    }
                    if (keyData == Keys.Down)
                    {
                        row++;
                        if (row >= gv.Rows.Count) row = 0;
                    }
                    gv.CurrentCell = gv[0, row];
                }
                else if (keyData == Keys.Escape)
                {
                    this.Hide();
                }
                else if (keyData == Keys.Enter)
                {
                    returnValue();
                }
            }
            
        }

        public static FrmPopupCURDGrid GetForm()
        {
            if (ThisForm == null) ThisForm = new FrmPopupCURDGrid();
            return ThisForm;
        }
    }
}
