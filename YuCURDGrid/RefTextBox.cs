using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestData;

namespace Yu.Win.Controls
{
    public class RefTextBox:TextBox
    {
        private string oldEditingControlText;
        FrmPopupCURDGrid refForm;
        private Action<object> RetAction;
        private Func<string, IEnumerable<CodeValue>> PopupFunc;
        public RefTextBox()
        {
            refForm = FrmPopupCURDGrid.GetForm();
            refForm.Owner = FindForm();
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (this.PopupFunc != null)
            {
                //e.KeyCode!=Keys.Enter 条件可防止Enter回填时引发本过程，导致Ref窗体再次出现
                if (this.Text.Length > 0 && this.Text != oldEditingControlText && e.KeyCode != Keys.Enter)
                {
                    oldEditingControlText = this.Text;
                    refForm.RetAction = this.RetAction;
                    if (refForm.Visible == false)
                    {
                        refForm.Visible = true;
                    }
                    Point formLocation = this.PointToScreen(new Point(0,0));
                    refForm.Location = new Point(formLocation.X, formLocation.Y + this.Height);
                    refForm.RefreshList(PopupFunc(this.Text));
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (refForm != null && refForm.Visible && (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Escape || keyData == Keys.Enter))
            {
                refForm.ProcessKey(keyData);
                return true;
            }
            if (keyData == Keys.Enter && !refForm.Visible)
            {
                SendKeys.Send("{TAB}");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SetDataAgent(IEnumerable<CodeValue> popupData, Action<object> retAction = null)
        {
            SetDataAgent(x => popupData.Where(y => y.Code.Contains(x.ToUpper())).ToList(), retAction);
        }

        //public void SetDataAgent(IEnumerable<KeyValue> popupData, Action<object> retAction = null)
        //{
        //    SetDataAgent(x => popupData.Where(y => y.Key.ToString().Contains(x.ToUpper())).ToList(), retAction);
        //}

        public void SetDataAgent(Func<string,IEnumerable<CodeValue>> popupFunc,Action<object>retAction=null)
        {
            this.PopupFunc = popupFunc;
            this.RetAction = retAction;
            if (RetAction == null)
            {
                this.RetAction =
                    r =>
                    {
                        if (r is CodeValue)
                            this.Text = (r as CodeValue).Code;
                        else if (r is ValueOnly)
                            this.Text = (r as ValueOnly).Value;
                        oldEditingControlText = this.Text.ToString();
                    };
            }

            if (this.DataBindings["Text"] != null)
            {
                this.DataBindings["Text"].Format -= RefTextBox_Format;
                this.DataBindings["Text"].Format += RefTextBox_Format;
            }
        }

        void RefTextBox_Format(object sender, ConvertEventArgs e)
        {
            CodeValue first = PopupFunc(e.Value.ToString()).FirstOrDefault();
            if (first !=null)
            {
                e.Value = first.Code + "-" + first.Value;
            }
            
        }
    }
}
