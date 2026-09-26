using System;

namespace Model
{
    public partial class DownloadDocument
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int LessonId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
