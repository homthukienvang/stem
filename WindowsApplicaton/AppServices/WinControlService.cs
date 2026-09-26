using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppServices
{
    public interface IWinControlService
    {
        void picMouseHover(object sender, EventArgs e);
        void picMouseLeave(object sender, EventArgs e);

        void picMouseHover2(object sender, EventArgs e);
        void picMouseLeave2(object sender, EventArgs e);

        void picMouseHover3(object sender, EventArgs e);
        void picMouseLeave3(object sender, EventArgs e);

        void ctlMouseHover(object sender, EventArgs e);
        void ctlMouseLeave(object sender, EventArgs e);
    }

    public class WinControlService : IWinControlService
    {
        public void picMouseHover(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        public void picMouseLeave(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        public void picMouseHover2(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
        }

        public void picMouseLeave2(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        public void picMouseHover3(object sender, EventArgs e)
        {
            PictureEdit pic = ((PictureEdit)sender);
            pic.SendToBack();
            pic.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        public void picMouseLeave3(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        public void ctlMouseHover(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (sender.GetType() == typeof(PictureEdit) || sender.GetType() == typeof(LabelControl))
                {
                    XtraUserControl ctl = (XtraUserControl)((Control)sender).Parent;
                    ctl.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        public void ctlMouseLeave(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (sender.GetType() == typeof(PictureEdit) || sender.GetType() == typeof(LabelControl))
                {
                    XtraUserControl ctl = (XtraUserControl)((Control)sender).Parent;
                    ctl.BorderStyle = BorderStyle.None;
                }
            }
        }
    }
}
