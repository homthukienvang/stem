using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;
using DevExpress.Pdf;
using DevExpress.XtraEditors;

namespace DXWindows
{
    public partial class MyPrint : DevExpress.XtraEditors.XtraForm
    {

        frmViewDoc frmF;
        public MyPrint(frmViewDoc frmOpen)
        {
            frmF = frmOpen;
            InitializeComponent();
        }
        public MyPrint()
        {
            InitializeComponent();
        }

        private void MyPrint_Load(object sender, EventArgs e)
        {
            List<string> items = new List<string>() { };

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (!printer.ToUpper().Contains("VIEWER") && !printer.ToUpper().Contains("FOXIT") && !printer.ToUpper().Contains("ADOBE") && !printer.ToUpper().Contains("FAX") && !printer.ToUpper().Contains("PDF") && !printer.ToUpper().Contains("READ")
                    && !printer.ToUpper().Contains("ONENOTE") && !printer.ToUpper().Contains("OFFICE") && !printer.ToUpper().Contains("XPS") && !printer.ToUpper().Contains("MICROSOFT") && !printer.ToUpper().Contains("WRITER"))
                    items.Add(printer);
            }
            comboBox1.DataSource = items;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidPages(int pagecount)
        {
            bool result = true;
            string pages = txtPageNumber.Text;
            if (string.IsNullOrEmpty(pages)) return false;

            if (pages.IndexOf('-') > 0)
            {
                string[] _pages = pages.Split('-');
                if (_pages.Length != 2) return false;
                if (!IsInt(_pages[0], pagecount) || !IsInt(_pages[1], pagecount)) return false;
            }
            else if (pages.IndexOf(',') > 0)
            {
                string[] _pages = pages.Split(',');
                if (_pages.Length < 2) return false;
                for (int i = 0; i < _pages.Length; i++)
                {
                    if (!IsInt(_pages[i], pagecount)) return false;
                }
            }
            else if (!IsInt(pages, pagecount))
            {
                return false;
            }
            return result;
        }

        private bool IsInt(String strNumber, int pagecount)
        {
            int val;
            if (string.IsNullOrEmpty(strNumber)) return false;

            int.TryParse(strNumber, out val);
            if (val <= 0 || val > pagecount) return false;

            return val > 0 && val <= pagecount;
        }

        private int[] GetPages(string pagelist, int pagecount)
        {
            var pages = new List<int>();

            if (!string.IsNullOrEmpty(pagelist))
            {
                if (pagelist.IndexOf('-') > 0)
                {
                    string[] _pages = pagelist.Split('-');
                    int from = int.Parse(_pages[0]);
                    int to = int.Parse(_pages[1]);
                    for (int i = from; i <= to; i++)
                        pages.Add(i);
                }
                else if (pagelist.IndexOf(',') > 0)
                {
                    string[] _pages = pagelist.Split(',');
                    for (int i = 0; i < _pages.Length; i++)
                    {
                        pages.Add(int.Parse(_pages[i]));
                    }
                }
                else if (IsInt(pagelist, pagecount))
                {
                    pages.Add(int.Parse(pagelist));
                }
            }
            return pages.ToArray();
        }

        private int[] GetPages(int pagecount, bool isodd)
        {
            var pages = new List<int>();
            if (isodd)
                for (int i = 1; i <= pagecount; i++)
                {
                    if (i % 2 == 0) continue;
                    pages.Add(i);
                }
            else
                for (int i = 1; i <= pagecount; i++)
                {
                    if (i % 2 == 1) continue;
                    pages.Add(i);
                }
            return pages.ToArray();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show(this, @"Vui lòng chọn máy in.", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrinterSettings printerSettings = new PrinterSettings();
            printerSettings.PrinterName = comboBox1.Text;
            printerSettings.PrintToFile = false;

            // Declare the PDF printer settings.
            // If required, pass the system settings to the PDF printer settings constructor.
            PdfPrinterSettings pdfPrinterSettings = new PdfPrinterSettings(printerSettings);

            // Specify the PDF printer settings.
            pdfPrinterSettings.PageOrientation = PdfPrintPageOrientation.Portrait;
            //pdfPrinterSettings.PageNumbers = new int[] { 1, 3, 4, 5 };

            pdfPrinterSettings.ScaleMode = PdfPrintScaleMode.CustomScale;
            pdfPrinterSettings.Scale = 100;
            int pagecount = frmF.pdfViewer1.PageCount;
            if (pagecount <= 0) return;
            switch (rdoPrint.SelectedIndex)
            {
                case 0:
                    pdfPrinterSettings.Settings.Duplex = Duplex.Vertical;
                    printerSettings.PrintRange = PrintRange.AllPages;
                    break;
                case 1:
                    pdfPrinterSettings.PageNumbers = GetPages(pagecount, false);
                    printerSettings.PrintRange = PrintRange.SomePages;
                    break;
                case 2:
                    pdfPrinterSettings.PageNumbers = GetPages(pagecount, true);
                    printerSettings.PrintRange = PrintRange.SomePages;
                    break;
                case 3:
                    if (!ValidPages(pagecount))
                    {
                        XtraMessageBox.Show("Số trang chưa đúng. Vui lòng kiểm tra lại. Tổng số trang là: " + pagecount);
                        txtPageNumber.Focus();
                        return;
                    }
                    pdfPrinterSettings.PageNumbers = GetPages(txtPageNumber.Text, pagecount);
                    printerSettings.PrintRange = PrintRange.SomePages;
                    break;
            }
            // Print the document using the specified printer settings.
            int total = 1;
            if (int.TryParse(textQuantity.Text, out total))
            {
            }
            else
            {
                XtraMessageBox.Show("Vui lòng điền số bản in hợp lệ");
            }

            for (int i = 0; i < total; i++)
            {
                frmF.pdfViewer1.Print(pdfPrinterSettings);
            }
        }
    }
}
