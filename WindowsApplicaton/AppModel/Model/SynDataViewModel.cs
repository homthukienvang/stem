using System;
using System.Collections.Generic;

namespace Model
{
    public partial class News
    {
        public int NewsId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NewsType { get; set; }
        public string ImageUrl { get; set; }
    }
    public class SynDataViewModel
    {
        public int ClientId { set; get; }
        public List<Subject> Subjects { set; get; }
        public List<Room> Rooms { set; get; }
        public List<Document> Documents { set; get; }
        public List<ClientLesson> ClientLessons { set; get; }
        public List<Lesson> Lessons { set; get; }

        public List<Feedback> Feedbacks { set; get; }
        public List<FeedbackAnswer> FeedbackAnswers { set; get; }

        public List<Advertisement> Advertisements { set; get; }

        public Lesson Lesson { set; get; }
        public News NewInfo { get; set; }
        public News UserGuide { get; set; }
        public News HotlineInfo { get; set; }
        public News SoftInfo { get; set; }

        public int GetSyncTotalItems()
        {
            var totalRecord = 0;
            if (NewInfo != null)
                totalRecord += 1;
            if (UserGuide != null)
                totalRecord += 1;
            if (HotlineInfo != null)
                totalRecord += 1;
            if (SoftInfo != null)
                totalRecord += 1;

            if (Subjects != null)
                totalRecord += Subjects.Count;
            if (Rooms != null)
                totalRecord += Rooms.Count;
            if (Documents != null)
                totalRecord += Documents.Count;
            if (Lessons != null)
                totalRecord += Lessons.Count;
            if (ClientLessons != null)
                totalRecord += 1;
            return totalRecord;
        }
    }
}