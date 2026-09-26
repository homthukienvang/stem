using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Message
    {
        public DateTime CreatedDate { get; set; }
        public string ShortCreatedDate { get; set; }
        public string ShortConent { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int FeedbackId { get; set; }
        public string UserName { get; set; }
        public string Answer { get; set; }
        public DateTime AnswerTime { get; set; }
        public int CAStatus { get; set; }
        public int CAId { get; set; }
    }
}
