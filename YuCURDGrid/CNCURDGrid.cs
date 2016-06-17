using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using TestData;

namespace Yu.Win.Controls
{
    public class CNCURDGrid : DataGridView
    {

        //弹出窗体有时候需要固定的列表数据，有时需要动态生成数据
        //固定列表数据存储在colRefData,一般具有CodeValue类型。
        Dictionary<string, Func<string, IEnumerable>> ColPopupFuncs;
        Dictionary<string, Action<object>> ColRetActions;
        Dictionary<string, Dictionary<string, string>> ColRefData;
        string oldEditingControlText = "";
        Bitmap ImgDrowDown;
        FrmPopupCURDGrid refForm;
        public CNCURDGrid()
        {
            ColPopupFuncs = new Dictionary<string, Func<string, IEnumerable>>();
            ColRetActions = new Dictionary<string, Action<object>>();
            ColRefData = new Dictionary<string, Dictionary<string, string>>();
            ImgDrowDown = Properties.Resources.dropDown;
            ImgDrowDown.MakeTransparent(Color.White);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                EndEdit();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (refForm != null && refForm.Visible && ( keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Escape || keyData == Keys.Enter))
            {
                refForm.ProcessKey(keyData);
                return true;
            }
            if (keyData ==Keys.Enter)
            {
                if (CurrentCell.ColumnIndex < Columns.Count - 1) CurrentCell = this[CurrentCell.ColumnIndex+1, CurrentCell.RowIndex];
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            string colName = Columns[e.ColumnIndex].DataPropertyName;
            if (ColRefData.ContainsKey(colName) && e.Value != null)
            {
                if (ColRefData[colName].ContainsKey(e.Value.ToString()))
                    e.Value = e.Value + "-" + ColRefData[colName][e.Value.ToString()];
            }
            base.OnCellFormatting(e);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // && e.RowIndex == CurrentRow.Index)
            {
                if (IsRefCol(e.ColumnIndex))
                {
                    e.Handled = true;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                    e.Graphics.DrawImage(ImgDrowDown, e.CellBounds.X + e.CellBounds.Width - 18, e.CellBounds.Y + 3);
                }
            }
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseClick(e);
            var c = this[e.ColumnIndex, e.RowIndex];
            if (e.X > c.Size.Width - 20 && e.X < c.Size.Width)
            {
                Message msg = new Message();
                ProcessCmdKey(ref msg, Keys.Insert);
            }
        }



        private bool IsRefCol(int colIndex)
        {
            return ColPopupFuncs.ContainsKey(Columns[colIndex].DataPropertyName);
        }

        public void SetColDataAgent(string colName, Func<string, IEnumerable> refAction)
        {
            ColPopupFuncs.Add(colName, refAction);
            ColRetActions.Add(colName,
                    r =>
                    {
                        if (r is CodeValue)
                            CurrentCell.Value = (r as CodeValue).Code;
                        else if (r is ValueOnly)
                            CurrentCell.Value = (r as ValueOnly).Value;
                        //else if (r is CodeValueInt)
                        //    CurrentCell.Value = (r as CodeValueInt).CodeInt.ToString();
                        oldEditingControlText = CurrentCell.Value.ToString();
                    });
        }

        public void SetColDataAgent(string colName, IEnumerable listData)
        {
            if (listData is IEnumerable<CodeValue>)
            {
                Dictionary<string, string> dictData = new Dictionary<string, string>();
                foreach (CodeValue item in listData) dictData.Add(item.Code, item.Value);
                ColRefData.Add(colName, dictData);
            }
            if (listData is IEnumerable<CodeValue>)
            {
                ColPopupFuncs.Add(colName, x => ((IEnumerable<CodeValue>)listData).Where(y=>y.Code.Contains(x.ToUpper())).ToList());
            }
            else
            {
                ColPopupFuncs.Add(colName, x => listData);
            }
            ColRetActions.Add(colName,
                    r =>
                    {
                        if (r is CodeValue)
                            CurrentCell.Value = (r as CodeValue).Code;
                        else if (r is ValueOnly)
                            CurrentCell.Value = (r as ValueOnly).Value;
                        //else if (r is CodeValueInt)
                        //    CurrentCell.Value = (r as CodeValueInt).CodeInt.ToString();
                        oldEditingControlText = CurrentCell.Value.ToString();
                    });
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            base.OnEditingControlShowing(e);
            e.Control.KeyUp -= new KeyEventHandler(Control_KeyUp);
            if (IsRefCol(CurrentCell.ColumnIndex))
            {
                oldEditingControlText = EditingControl.Text;
                e.Control.KeyUp += new KeyEventHandler(Control_KeyUp);
            }
        }

        void Control_KeyUp(object sender, KeyEventArgs e)
        {
            Rectangle rectCell = GetCellDisplayRectangle(CurrentCell.ColumnIndex, CurrentCell.RowIndex, true);
            string colName = Columns[CurrentCell.ColumnIndex].DataPropertyName;
            string controlText = (sender as Control).Text;

            //e.KeyCode!=Keys.Enter 条件可防止Enter回填时引发本过程，导致Ref窗体再次出现
            if (controlText.Length > 0 && EditingControl.Text!=oldEditingControlText && e.KeyCode!=Keys.Enter)
            {
                oldEditingControlText = EditingControl.Text;

                if (refForm == null)
                {
                    refForm = new FrmPopupCURDGrid();
                    refForm.Owner = FindForm();

                }
                refForm.RetAction = ColRetActions[colName];
                if (refForm.Visible == false)
                {
                    refForm.Visible = true;
                }
                Point formLocation = this.PointToScreen(rectCell.Location);
                refForm.Location = new Point(formLocation.X, formLocation.Y + rectCell.Height);
                refForm.RefreshList(ColPopupFuncs[colName](controlText));
            }
        }
    }
}
