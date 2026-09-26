using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DXWindows.Properties;
using Extensions;
using Model;
using Services;

namespace DXWindows.UserControl
{
    public partial class uListLesson : XtraUserControl
    {
        private ILessonService _lessonService;
        private ISubjectService _subjectService;
        private IRoomService _roomService;
        private IDocumentService _documentService;
        public static Lesson CurrenLesson;

        public uListLesson()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            _lessonService = new LessonService();
            _subjectService = new SubjectService();
            _roomService = new RoomService();
            _documentService = new DocumentService();
            InitData();
        }

        public void InitData()
        {
            var subs = _subjectService.GetAllSubject().ToList();
            var rooms = _roomService.GetAllRoom();
            var lesson = _lessonService.GetAllLesson();
            treeList1.BeginUnboundLoad();
            var nid = 0;
            for (int id = 0; id < subs.Count; id++)
            {
                var sub = subs[id];
               
                var rooms1 = rooms.Where(x => x.SubjectId == sub.SubjectId).ToList();
                if (rooms1.Any())
                {
                    var hadRoom = false;
                    for (int r = 0; r < rooms1.Count(); r++)
                    {
                        hadRoom = lesson.Any(x => x.RoomId == rooms1[r].RoomId && x.IsDeleted == true);
                        if (hadRoom)
                        {
                            break;
                        }
                    }
                    if (hadRoom)
                    {
                        treeList1.Nodes.Add(sub.Name, null);
                        var rr = 0;
                        for (var r = 0; r < rooms1.Count(); r++)
                        {
                            var lesson1 = lesson.Where(x => x.RoomId == rooms1[r].RoomId && x.IsDeleted == true).ToList();
                            if (lesson1.Any())
                            {
                                treeList1.Nodes[nid].Nodes.Add(rooms1[r].Name, null);
                                for (int l = 0; l < lesson1.Count; l++)
                                {
                                    treeList1.Nodes[nid].Nodes[rr].Nodes.Add(lesson1[l].Name, lesson1[l].LessonId);
                                }
                                rr++;
                            }
                        }
                        nid++;
                    }
                }
               
                
            }
            treeList1.EndUnboundLoad();
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            var tree = sender as TreeList;
            var idlesson = tree.Selection[0].GetDisplayText(1);
            tileGroup1.Items.Clear();
            if (!string.IsNullOrEmpty(idlesson))
            {
                CurrenLesson = _lessonService.GetLessonById(Convert.ToInt32(idlesson));
                lblLessonName.Text = CurrenLesson.Name;
                lblDes.Text = CurrenLesson.Description;
                var docs = _documentService.GetAllDocumentByLesson(Convert.ToInt32(idlesson));
                foreach (var doc in docs)
                {
                    TileItem tileItem1 = new TileItem();
                    tileItem1.Id = doc.DocumentId;
                    TileItemElement tileItemElement1 = new TileItemElement();
                    var extension = Path.GetExtension(doc.FileName);
                    if (extension == null)
                    {
                        extension = "";
                    }
                    if (extension.Equals(Constants.FlashExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.flash_icon;
                    }

                    if (extension.Equals(Constants.PdfFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.Adobe_PDF_Document_icon;
                    }
                    if (extension.Equals(Constants.Mp3FileExtension, StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(Constants.Mp4FileExtension, StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(Constants.AviFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.Video_File_icon;
                    }

                    tileItemElement1.Text = doc.DocumentName;
                    tileItem1.Elements.Add(tileItemElement1);

                    tileItem1.ItemClick += tileItem1_ItemClick;
                    tileGroup1.Items.Add(tileItem1);
                }
            }
        }

        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            var id = (sender as TileItem).Id;
            var doc = _documentService.GetDocumentById(id);
            if (doc == null || string.IsNullOrEmpty(doc.FileName))
            {
                MessageBox.Show("Không tìm thấy tài liệu này?");
                return;
            }
            OpenDocument(doc.FileName, doc.DocumentName, true);
        }

        public void OpenDocument(string fileName, string title, bool allowPrint)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    MessageBox.Show("Không tìm thấy tài liệu này?");
                    return;
                }
                
                var form2 = new frmPrinter();
                form2.AllowPrint = allowPrint;

                form2.pdfViewer1.Visible = false;
                form2.axWindowsMediaPlayer1.Visible = false;
                var selectedFileName = fileName;
                var extension = Path.GetExtension(selectedFileName);

                var filePathToOpen = Application.StartupPath + @"\App_data\" + fileName.Trim().ToLower().MD5Hash();
                if (extension.Equals(Constants.PdfFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    form2.pdfViewer1.Enabled = true;
                    form2.pdfViewer1.Visible = true;
                    form2.pdfViewer1.DocumentFilePath = filePathToOpen;
                }
                if (extension.Equals(Constants.Mp3FileExtension, StringComparison.OrdinalIgnoreCase) ||
                    extension.Equals(Constants.Mp4FileExtension, StringComparison.OrdinalIgnoreCase) ||
                    extension.Equals(Constants.AviFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    form2.axWindowsMediaPlayer1.Enabled = true;
                    form2.axWindowsMediaPlayer1.URL = filePathToOpen;
                    form2.axWindowsMediaPlayer1.Show();
                }

                form2.Text = title;
                form2.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể mở tài liệu này?");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDocument(CurrenLesson.GuideFile, CurrenLesson.Name, false);
            }
            catch (Exception)
            {
            }
        }
    }
}