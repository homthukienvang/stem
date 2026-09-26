using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FeedbackAnswer
    {
        public int FBAnswerId { get; set; }
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Answer { get; set; }
        public Nullable<System.DateTime> AnswerTime { get; set; }
        public int Status { get; set; }
        public int TotalRowCount { get; set; }
    }
}
