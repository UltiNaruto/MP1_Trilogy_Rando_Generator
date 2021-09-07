using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    class DarkComboBox_ : DarkComboBox
    {
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (this.SelectedIndex != -1)
                e.Graphics.DrawString((String)this.Items[this.SelectedIndex].ToString(), this.Font, new SolidBrush(this.ForeColor), e.Bounds, StringFormat.GenericDefault);
        }
    }
}
