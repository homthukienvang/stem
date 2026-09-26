using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Model
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int ClientId { get; set; }
        public int CommentType { get; set; }
        public string Content { get; set; }
        public int LessonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        public int FeedbackId { get; set; }
    }
}
