using System.Collections.Generic;
using System.Web.Http;
using Model;
using Services;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System;
using System.Linq;
using System.Web.Hosting;

namespace WebApplication.Api
{
    public class PublicApiController : ApiController
    {
        private IPublicApiService apiService;

        private readonly ISubjectService _subjectService;
        private IRoomService _roomService;
        private ILessonService _lessonService;
        private IDocumentService _documentService;
        private IClientService _clientService;
        private IFeedbackService _feedbackService;
        private IFeedbackAnswerService _feedbackAnswerService;
        private IAdvertisementService _advertisementService;
        private INewsService _newsService;
        public PublicApiController()
        {
            _newsService = new NewsService();
            _feedbackService = new FeedbackService();
            _subjectService = new SubjectService();
            _roomService = new RoomService();
            _lessonService = new LessonService();
            _documentService = new DocumentService();
            _clientService = new ClientService();
            _feedbackAnswerService = new FeedbackAnswerService();
            _advertisementService = new AdvertisementService();
        }
        public SynDataViewModel GetUpdateData(int clientId, int version)
        {
            var model = new SynDataViewModel
            {
                ClientId = clientId,
                Subjects = _subjectService.GetUpdateSubjectsV2(version),
                Rooms = _roomService.GetUpdateRoomsV2(version),
                Lessons = _lessonService.GetUpdateLessonsV2(clientId, version),
                //Documents = _documentService.GetUpdateDocumentsV2(clientId, version),
                ClientLessons = _clientService.GetClientLessonsByClientIdV2(clientId),
                NewInfo = _newsService.GetNewsById(1),
                UserGuide = _newsService.GetNewsById(2),
                SoftInfo = _newsService.GetNewsById(3),
                HotlineInfo = _newsService.GetNewsById(4),
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

        public SynDataViewModel GetFeedback(int client)
        {
            var model = new SynDataViewModel
            {
                Feedbacks = _feedbackService.GetFeedbackByClient(client),
                FeedbackAnswers = _feedbackService.GetFeedbackAnswerByClient(client)
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

        //public IEnumerable<Feedback> GetFeedback(int client)
        //{
        //    return _feedbackService.GetFeedbackByClient(client);
        //}

        //public IEnumerable<FeedbackAnswer> GetFeedbackAnswer(int client)
        //{
        //    return _feedbackService.GetFeedbackAnswerByClient(client);
        //}

        public int GetLastVersion()
        {
            return _lessonService.GetLastVersion();
        }

        public News GetNews()
        {
            return _newsService.GetNewsById(1);
        }
        public News GetNewsInfo()
        {
            return _newsService.GetNewsById(1);
        }

        public News GetUserGuide()
        {
            return _newsService.GetNewsById(2);
        }

        public Client GetLoginToApp(string userName, string passWord, string macid)
        {
            return _clientService.LoginToApp(userName.ToLower(), passWord, macid);
        }

        [HttpPost]
        public Client PostClient([FromBody] string userName, string passWord, string macid)
        {
            return _clientService.LoginToApp(userName.ToLower(), passWord, macid);
        }

        [HttpPost]
        public Client PostLogin([FromBody] Client client)
        {
            return _clientService.LoginToAppV2(client.UserName.ToLower(), client.PassWork, client.MacId);
        }
        [HttpPost]
        [HttpGet]
        public Client PostChangePassword(string userName, string passWord)
        {
            return _clientService.ChangePass(userName.ToLower(), passWord);
        }
        [HttpPost]
        public Response PostFeedback([FromBody] Feedback feedback)
        {
            return _feedbackService.Create(feedback);
        }
        [HttpPost]
        public Response PostClientLessonView([FromBody] ClientLessonView lesson)
        {
            return _clientService.UpdateLessonViewCount(lesson);
        }
        [HttpPost]
        [HttpGet]
        public Response PostUpdateInfo(string ClientId, string Email, string Phone)
        {
            return _clientService.PostUpdateInfo(ClientId, Email, Phone);
        }

        public SynDataViewModel GetAdvertisement()
        {
            var model = new SynDataViewModel
            {
                Advertisements = _advertisementService.GetAllAdvertisment()
            };
            return model;
        }

        [HttpPost]
        public async Task<object> UploadFiles()
        {
            var filePath = @"/upload/" ;
            string path = HttpContext.Current.Server.MapPath(filePath);
            var file = await Request.Content.ReadAsByteArrayAsync();
            var fileName = Request.Headers.GetValues("fileName").FirstOrDefault();
            try
            {
                File.WriteAllBytes(path + fileName, file);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return null;
        }

        [HttpPost]
        [HttpGet]
        public Response PostAdvertisment(int clientid, int advid)
        {
            return _advertisementService.UpdateCount(clientid, advid);
        }
    }
}