using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Services;
using DevExpress.LookAndFeel;

namespace DXWindows
{
    public partial class frmMessage : DevExpress.XtraEditors.XtraUserControl
    {
        private ICommentService _commentService;

        public frmMessage()
        {
            InitializeComponent();
            _commentService = new CommentService();
        }

        public void GetData()
        {
            IEnumerable<Model.Message> message = _commentService.GetAllMessages();
            gridMessage.DataSource = message;
            gridView1.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            gridView1.ExpandAllGroups();
        }
        private void frmMessage_Load(object sender, EventArgs e)
        {
            GetData();            
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount > 0)
            {
                string content = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Content") + "";
                string answer = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Answer") + "";
                string caid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CAId") + "";

                txtContent.Text = content;
                txtAnswer.Text = answer;

                if (!(string.IsNullOrEmpty(caid) || caid.Equals("0")))
                {
                    _commentService.UpdateFBAStatus(caid, "1");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CAStatus", "1");
                }
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string castatus = gridView1.GetRowCellValue(e.RowHandle, "CAStatus") + "";
            string caid = gridView1.GetRowCellValue(e.RowHandle, "CAId") + "";

            if (castatus.Equals("1"))
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
            }
            else
            {
                if (caid.Equals("0"))
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                else
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }
    }
}