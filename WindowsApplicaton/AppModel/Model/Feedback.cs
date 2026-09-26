using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int ClientId { get; set; }
        public int LessonId { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LessonName { get; set; }
        public string RoomName { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentTime { get; set; }
        public bool Viewed { get; set; }
        public int TotalRowCount { get; set; }


        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Answer { get; set; }
        public IEnumerable<FeedbackAnswer> FeedbackAnswers { get; set; }

        public IEnumerable<int> ListId { get; set; }
    }
}
