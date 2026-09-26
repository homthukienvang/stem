using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Model;
using Services;

namespace WebApplication.Api
{
    public class SynDataViewModel
    {
        public int ClientId { set; get; }
        public IEnumerable<Subject> Subjects { set; get; }
        public IEnumerable<Room> Rooms { set; get; }
        public IEnumerable<Document> Documents { set; get; }
        public IEnumerable<ClientLesson> ClientLessons { set; get; }
        public IEnumerable<Lesson> Lessons { set; get; }

        public IEnumerable<Feedback> Feedbacks { set; get; }
        public IEnumerable<FeedbackAnswer> FeedbackAnswers { set; get; }

        public IEnumerable<Advertisement> Advertisements { set; get; }

        public Lesson Lesson { set; get; }
        public News NewInfo { get; set; }
        public News UserGuide { get; set; }
        public News HotlineInfo { get; set; }
        public News SoftInfo { get; set; }
    }

    public class SynDataController : ApiController
    {
        private readonly ISubjectService _subjectService;
        private IRoomService _roomService;
        private ILessonService _lessonService;
        private IDocumentService _documentService;
        private IClientService _clientService;

        public SynDataController()
        {
            _subjectService = new SubjectService();
            _roomService = new RoomService();
            _lessonService = new LessonService();
            _documentService = new DocumentService();
            _clientService = new ClientService();
        }

        public SynDataViewModel GetUpdateData(int clientId ,int version)
        {
            var model = new SynDataViewModel
            {
                ClientId = clientId,
                Subjects = _subjectService.GetUpdateSubjects(version),
                Rooms = _roomService.GetUpdateRooms(version),
                Lessons = _lessonService.GetUpdateLessons(version),
                Documents = _documentService.GetUpdateDocuments(version),
                ClientLessons = _clientService.GetClientLessonsByClientId(clientId)
            };
            return model;
        }

        public SynDataViewModel GetUpdateDocByLessons(int lessonId)
        {
            var model = new SynDataViewModel
            {
                Lesson = _lessonService.GetLessonById(lessonId),
                Documents = _documentService.GetDocumentByLesson(lessonId),
            };
            return model;
        }
        public IEnumerable<Subject> GetUpdateSubjects(int version)
        {
            return _subjectService.GetUpdateSubjects(version);
        }

        public IEnumerable<Room> GetUpdateRooms(int version)
        {
            return _roomService.GetUpdateRooms(version);
        }

        public IEnumerable<Lesson> GetUpdateLessons(int version)
        {
            return _lessonService.GetUpdateLessons(version);
        }

        public IEnumerable<Document> GetUpdateDocuments(int version)
        {
            return _documentService.GetUpdateDocuments(version);
        }

        public int GetLastVersion()
        {
            return _lessonService.GetLastVersion();
        }
        public News GetNews()
        {
            return _clientService.GetNews();
        }

        public Client GetLoginToApp(string userName, string passWord, string macid)
        {
            return _clientService.LoginToApp(userName.ToLower(), passWord, macid)  ;
        }
        [HttpPost]
        public Client PostClient([FromBody] string userName, string passWord, string macid)
        {
            return _clientService.LoginToApp(userName.ToLower(), passWord, macid)  ;
        }
        [HttpPost]
        public Client PostOne([FromBody] Client client)
        {
            return _clientService.LoginToApp(client.UserName.ToLower(), client.PassWork, client.MacIp);
        }
        public Client GetChangePassword(string userName, string passWord)
        {
            return _clientService.ChangePass(userName.ToLower(), passWord);
        }
    }
}