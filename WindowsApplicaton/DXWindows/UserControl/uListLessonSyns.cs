using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DXWindows.Properties;
using Extensions;
using Model;
using Repositories.Implementations;
using Services;

namespace DXWindows.UserControl
{
    public partial class uListLessonSyns : XtraUserControl
    {
        private ILessonService _lessonService;
        private ISubjectService _subjectService;
        private IRoomService _roomService;
        private IDocumentService _documentService;
        public static Lesson CurrenLesson;

        public uListLessonSyns()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            _lessonService = new LessonService();
            _subjectService = new SubjectService();
            _roomService = new RoomService();
            _documentService = new DocumentService();
            InitData();
            LoadLoction();
        }

        public void InitData()
        {
            var subs = _subjectService.GetAllSubject().ToList();
            var rooms = _roomService.GetAllRoom();
            var lesson = _lessonService.GetAllLesson();
            treeList1.BeginUnboundLoad();
            for (int i = 0; i < subs.Count; i++)
            {
                var sub = subs[i];
                treeList1.Nodes.Add(sub.Name, null);
                var rooms1 = rooms.Where(x => x.SubjectId == sub.SubjectId).ToList();
                var rr = 0;
                for (var r = 0; r < rooms1.Count(); r++)
                {
                    var lesson1 = lesson.Where(x => x.RoomId == rooms1[r].RoomId && x.IsDeleted == true).ToList();
                    if (lesson1.Any())
                    {
                        treeList1.Nodes[i].Nodes.Add(rooms1[r].Name, null);
                        for (int l = 0; l < lesson1.Count; l++)
                        {
                            treeList1.Nodes[i].Nodes[rr].Nodes.Add(lesson1[l].Name, lesson1[l].LessonId);
                        }
                        rr++;
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

        /// <summary>
        /// ////////
        /// </summary>

        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void Copy(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            var newLesonsService = new LessonService(new CommonRepository("Data Source=\"" + targetDir + "\\App_data\\stem_plus.sdf\""));
            var newDocumentService = new DocumentService(new CommonRepository("Data Source=\"" + targetDir + "\\App_data\\stem_plus.sdf\""));
            var newlesson = newLesonsService.GetLessonById(CurrenLesson.LessonId);
            CurrenLesson.IsDeleted = true;
            if (newlesson == null || newlesson.LessonId == 0)
            {
                var resp = newLesonsService.Create(CurrenLesson);
                newlesson = newLesonsService.GetLessonById(CurrenLesson.LessonId);
            }
            else
            {
                newLesonsService.Update(CurrenLesson);
            }
            string guide = @"\App_data\" + CurrenLesson.GuideFile.Trim().ToLower().MD5Hash();
            if (File.Exists(targetDir + guide))
            {
                File.Delete(targetDir + guide);
            }
            File.Copy(sourceDir + guide, targetDir + guide);
            var docs = _documentService.GetAllDocumentByLesson(CurrenLesson.LessonId);
            foreach (var doc in docs)
            {
                var newDoc = newDocumentService.GetDocumentById(doc.DocumentId);

                if (newDoc == null || newDoc.LessonId == 0)
                {
                    var resp = newDocumentService.Create(doc);
                }
                else
                {
                    newDocumentService.Update(doc);
                }
                string file = @"\App_data\" + doc.FileName.Trim().ToLower().MD5Hash();
                if (File.Exists(targetDir + file))
                {
                    File.Delete(targetDir + file);
                }
                File.Copy(sourceDir + file, targetDir + file);
            }
            newLesonsService.UpdateDowloadStatus(CurrenLesson);
        }

        private void LoadLoction()
        {
            var is64 = InternalCheckIsWow64();
            if (is64)
            {
                var path = @"C:\Program Files (x86)\Stem+";
                r64.Checked = true;
                textBox1.Text = path;
            }
            else
            {
                var paths = @"C:\Program Files\Stem+";                
                r32.Checked = true;
                textBox1.Text = paths;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var newLesonsService = new LessonService(new CommonRepository(""));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CurrenLesson == null)
            {
                MessageBox.Show("Vui lòng chọn bài học cụ thể");
                return;
            }

            if (!File.Exists(textBox1.Text.Trim() + @"\App_data\stem_plus.sdf"))
            {
                MessageBox.Show("Vui lòng chọn đường dẫn thư mục cài STEM+");
            }
            else
            {
                try
                {
                    //var listSub = _subjectService.GetAllSubject();
                    //if (!listSub.Any())
                    {
                        SynSubject(AppDomain.CurrentDomain.BaseDirectory, textBox1.Text);
                    }
                    Copy(AppDomain.CurrentDomain.BaseDirectory, textBox1.Text);
                    MessageBox.Show("Cập nhật thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra vui lòng kiểm tra lại: " + ex.Message);
                    return;
                }

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text.Trim() + @"\App_data\stem_plus.sdf"))
            {
                MessageBox.Show("Vui lòng chọn đường dẫn thư mục cài STEM+");
                return;
            }
            try
            {
                SynSubject(AppDomain.CurrentDomain.BaseDirectory, textBox1.Text);
                MessageBox.Show("Cập nhật thành công");
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng kiểm tra lại");
                return;
            }
        }

        private void SynSubject(string sourceDir, string targetDir)
        {
            var newSubjectService = new SubjectService(new CommonRepository("Data Source=\"" + targetDir + "\\App_data\\stem_plus.sdf\""));
            var newRoomService = new RoomService(new CommonRepository("Data Source=\"" + targetDir + "\\App_data\\stem_plus.sdf\""));
            var listSub = _subjectService.GetAllSubject();
            foreach (var subject in listSub)
            {
                var sub = newSubjectService.GetSubjectById(subject.SubjectId);
                if (sub == null)
                {
                    newSubjectService.Create(subject);
                }
                else
                {
                    newSubjectService.Update(subject);
                }
            }
            var listRoom = _roomService.GetAllRoom();
            foreach (var room in listRoom)
            {
                var ro = newRoomService.GetRoomById(room.RoomId);
                if (ro == null)
                {
                    newRoomService.Create(room);
                }
                else
                {
                    newRoomService.Update(room);
                }
            }
        }
    }
}