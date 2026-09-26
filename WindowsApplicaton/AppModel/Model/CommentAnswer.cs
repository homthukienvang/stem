using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CommentAnswer
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Answer { get; set; }
        public DateTime AnswerTime { get; set; }
        public int Status { get; set; }
        public int FeedbackId { get; set; }

        public int FBAnswerId { get; set; }
        
    }
}
