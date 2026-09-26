using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DataModel.Base;
using Extensions;
using Model;
using OfficeOpenXml;
using Security;
using Services;
using WebApplication.Helper;

namespace WebApplication.Areas.Admin.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private ISubjectService _subjectService;
        private IRoomService _roomService;
        private ISchoolService _schoolService;

        public ClientController(IClientService clientService, ISubjectService subjectService, IRoomService roomService, ISchoolService schoolService)
        {
            _clientService = clientService;
            _subjectService = subjectService;
            _roomService = roomService;
            _schoolService = schoolService;
        }

        private PagedData<Client> GetModel(int pageIndex, string tclass, string schoolCode, string cityCode, string districtCode, string fullName, string deploymethod, int approve)
        {
            var model = new PagedData<Client>();
            int totalRow;
            string usercitycode = Session["CityCode"] + "";
            string userdistrictcode = Session["DistrictCode"] + "";

            model.Data = _clientService.GetClientByPage(pageIndex, tclass, schoolCode, cityCode, districtCode, fullName, deploymethod, approve, usercitycode, userdistrictcode, GlobalSession.PageSize, out totalRow);
            model.Pagination = new PaginationModel
            {
                CurrentPage = pageIndex,
                PageSize = GlobalSession.PageSize,
                NumberOfRows = totalRow
            };
            return model;
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult Index()
        {
            var model = GetModel(0, "", "", "", "", "", "", -1);
            return View(model);
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult Simple()
        {
            var model = GetModel(0, "", "", "", "", "", "", -1);
            return View(model);
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult ReActive()
        {
            var model = GetModel(0, "", "", "", "", "", "", -1);
            return View(model);
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult ClientLessonView()
        {
            var model = GetModel(0, "", "", "", "", "", "", -1);
            return View(model);
        }
        [RequiredLogin]
        [RequiredRight]
        public ActionResult Approve()
        {
            var model = GetModel(0, "", "", "", "", "", "", 0);
            return View(model);
        }
        [RequiredRight]
        public ActionResult ImportClient()
        {
            return View();
        }

        public ActionResult ListClientLessonView(int? pageIndex, string tclass, string schoolCode, string cityCode, string districtCode, string fullName, int approve)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, tclass, schoolCode, cityCode, districtCode, fullName, "", approve);
            return PartialView(model);
        }

        public ActionResult List(int? pageIndex, string tclass, string schoolCode, string cityCode, string districtCode, string fullName, string deploymethod, int approve, string view = "List")
        {
            TempData["pageIndex"] = pageIndex;
            TempData["tclass"] = tclass;
            TempData["schoolCode"] = schoolCode;
            TempData["cityCode"] = cityCode;
            TempData["districtCode"] = districtCode;
            TempData["fullName"] = fullName;
            TempData["approve"] = approve;
            TempData["deploymethod"] = deploymethod;

            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, tclass, schoolCode, cityCode, districtCode, fullName, deploymethod, approve);
            return PartialView(view, model);
        }

        public ActionResult ListReactive(int? pageIndex, string tclass, string schoolCode, string cityCode, string districtCode, string fullName, int approve)
        {
            TempData["pageIndex"] = pageIndex;
            TempData["tclass"] = tclass;
            TempData["schoolCode"] = schoolCode;
            TempData["cityCode"] = cityCode;
            TempData["districtCode"] = districtCode;
            TempData["fullName"] = fullName;
            TempData["approve"] = approve;
            TempData["deploymethod"] = "";

            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, tclass, schoolCode, cityCode, districtCode, fullName, "", approve);
            return PartialView(model);
        }

        public ActionResult ListApprove(int? pageIndex, string tclass, string schoolCode, string cityCode, string districtCode, string fullName, int approve)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, tclass, schoolCode, cityCode, districtCode, fullName, "", approve);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            GetViewBag();
            var model = new Client()
            {
            };
            string html = this.RenderPartialView("Create", model);
            return Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportClient(ClientImport model)
        {
            if (Request.Files.Count <= 0)
            {
                model.Error = "Không tìm thấy file import!!!";
                return View(model);
            }
            model.Error = "Hoàn thành Import <br/>";
            var file = Request.Files[0];

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(path);

                string sexcelconnectionstring = "";

                //Connection String to Excel Workbook

                //Use this if .xls
                // sexcelconnectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                //Use this if .xlsx
                sexcelconnectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=2\"";
                using (OleDbConnection conn = new OleDbConnection(sexcelconnectionstring))
                {
                    conn.Open();
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string sheetname = dt.Rows[0]["Table_Name"].ToString();
                    string query = "SELECT * FROM [" + sheetname + "]";
                    OleDbCommand ocmd = new OleDbCommand(query, conn);
                    OleDbDataAdapter adp = new OleDbDataAdapter(ocmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    Random random = new Random();
                    int activateCode = random.Next(100000, 999999999);
                    DataRow row = ds.Tables[0].Rows[0];
                    ds.Tables[0].Rows.Remove(row);

                    string fileNameSave = DateTime.Now.ToString("imported_dd_MM_yyyy_hh_mm_ss") + ".xlsx";
                    string filePath = Server.MapPath("/Images/" + fileNameSave);
                    var fileInfo = new FileInfo(filePath);
                    using (ExcelPackage excel = new ExcelPackage(fileInfo))
                    {

                        var sheet = excel.Workbook.Worksheets.Add("imported");

                        sheet.Column(1).Width = 10;
                        sheet.Column(2).Width = 20;
                        sheet.Column(3).Width = 20;
                        sheet.Column(4).Width = 30;
                        sheet.Column(5).Width = 20;
                        sheet.Column(6).Width = 20;
                        sheet.Column(7).Width = 20;
                        sheet.Column(8).Width = 20;
                        sheet.Column(9).Width = 50;
                        sheet.Column(10).Width = 50;
                        sheet.Column(11).Width = 20;
                        sheet.Cell(1, 1).Value = "STT";
                        sheet.Cell(1, 2).Value = "Họ tên";
                        sheet.Cell(1, 3).Value = "Tên đăng nhập";
                        sheet.Cell(1, 4).Value = "Mật khẩu";
                        sheet.Cell(1, 5).Value = "Lớp";
                        sheet.Cell(1, 6).Value = "SĐT";
                        sheet.Cell(1, 7).Value = "Mã trường";
                        sheet.Cell(1, 8).Value = "Mã tỉnh thành";
                        sheet.Cell(1, 9).Value = "Mã quận huyện";
                        sheet.Cell(1, 10).Value = "Địa chỉ";
                        sheet.Cell(1, 11).Value = "Sử dụng toàn bộ bài học";

                        int rowCount = 2; // start row (in row 1 are header cells)

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            if (item[1] != null && !string.IsNullOrEmpty(item[1].ToString()))
                            {
                                var client = new Client();
                                var pass = "123456";
                                client.FullName = item[1].ToString();
                                if (item[2] != null && !string.IsNullOrEmpty(item[2].ToString()))
                                {
                                    client.UserName = item[2].ToString();
                                }
                                else
                                {
                                    client.UserName = StringHelper.FormatUserName(item[1].ToString());
                                }

                                if (item[3] != null && !string.IsNullOrEmpty(item[3].ToString()))
                                {
                                    pass = item[3].ToString();
                                    client.PassWork = item[3].ToString().MD5Hash();
                                }
                                else
                                {
                                    client.PassWork = "123456".MD5Hash();
                                }
                                if (item[4] != null && !string.IsNullOrEmpty(item[4].ToString()))
                                {
                                    client.Email = item[4].ToString();
                                }
                                if (item[5] != null && !string.IsNullOrEmpty(item[5].ToString()))
                                {
                                    client.Phone = item[5].ToString();
                                }
                                if (item[6] != null && !string.IsNullOrEmpty(item[6].ToString()))
                                {
                                    client.SchoolCode = item[6].ToString();
                                }
                                if (item[7] != null && !string.IsNullOrEmpty(item[7].ToString()))
                                {
                                    client.CityCode = item[7].ToString();
                                }
                                if (item[8] != null && !string.IsNullOrEmpty(item[8].ToString()))
                                {
                                    client.DistrictCode = item[8].ToString();
                                }
                                if (item[9] != null && !string.IsNullOrEmpty(item[9].ToString()))
                                {
                                    client.Address = item[9].ToString();
                                }
                                if (item[10] != null && !string.IsNullOrEmpty(item[10].ToString()))
                                {
                                    client.FullLesson = item[10].ToString().ToLower() == "y";
                                }
                                else
                                {
                                    client.FullLesson = false;
                                }

                                var result = _clientService.Create(client);
                                if (!result.Success)
                                {
                                    model.Error += "STT " + item[0].ToString() + "(" + client.FullName + "):" + result.Message + "<br/>";

                                    try
                                    {
                                        sheet.Cell(rowCount, 1).Value = "#";
                                        sheet.Cell(rowCount, 2).Value = client.FullName;
                                        sheet.Cell(rowCount, 3).Value = result.Message;
                                        rowCount++;
                                    }
                                    catch
                                    {
                                        rowCount++;
                                    }

                                }
                                else
                                {
                                    try
                                    {
                                        sheet.Cell(rowCount, 1).Value = (rowCount - 1).ToString();
                                        sheet.Cell(rowCount, 2).Value = client.FullName;
                                        sheet.Cell(rowCount, 3).Value = client.UserName;
                                        sheet.Cell(rowCount, 4).Value = pass;
                                        sheet.Cell(rowCount, 5).Value = client.Email;
                                        sheet.Cell(rowCount, 6).Value = client.Phone;
                                        sheet.Cell(rowCount, 7).Value = client.SchoolCode;
                                        sheet.Cell(rowCount, 8).Value = client.CityCode;
                                        sheet.Cell(rowCount, 9).Value = client.DistrictCode;
                                        sheet.Cell(rowCount, 10).Value = client.Address;
                                        sheet.Cell(rowCount, 11).Value = client.FullLesson ? "Y" : "N";

                                        rowCount++;
                                    }
                                    catch
                                    {
                                        rowCount++;
                                    }
                                }

                            }
                        }

                        excel.Save();
                    }
                    return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Client model)
        {
            try
            {
                model.IsDeleted = false;
                model.IsActived = true;
                model.PassWork = model.PassWork.MD5Hash();
                var response = _clientService.Create(model);

                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create Client");
                return Json(new { Success = false, Message = "Không thể tạo môn học" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _clientService.GetClientById(id);
            model.ConfirmPassword = model.PassWork;
            string html = this.RenderPartialView("Edit", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Client model)
        {
            string pageIndex = TempData["pageIndex"] + "";
            string tclass = TempData["tclass"] + "";
            string schoolCode = TempData["schoolCode"] + "";
            string cityCode = TempData["cityCode"] + "";
            string districtCode = TempData["districtCode"] + "";
            string fullName = TempData["fullName"] + "";
            string approve = TempData["approve"] + "";
            string deploymethod = TempData["deploymethod"] + "";

            try
            {
                var response = _clientService.Update(model);
                string html = "";
                if (response.Success)
                {
                    int page = (string.IsNullOrEmpty(pageIndex) || int.Parse(pageIndex) < 0) ? 0 : int.Parse(pageIndex);
                    int app = int.TryParse(approve, out app) ? app : -1;
                    var lst = GetModel(page, tclass, schoolCode, cityCode, districtCode, fullName, deploymethod, app);

                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        public ActionResult ClientPayment(int id)
        {
            var model = _clientService.GetClientById(id);
            model.ClientPayments = _clientService.GetClientPayment(id);
            string html = this.RenderPartialView("ClientPayment", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClientPayment(Client model)
        {
            try
            {
                var response = _clientService.CreateClientPayment(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        public ActionResult ClientLesson(int id, string view= "ClientLesson")
        {
            var model = _clientService.GetClientById(id);
            model.Subjects = _subjectService.GetAllSubject();
            model.Rooms = _roomService.GetAllRoom();
            model.ClientLessons = _clientService.GetClientLessons(id);
            string html = this.RenderPartialView(view, model);
            var jsonResult = Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ClientLessonViewDetail(int id)
        {
            var model = _clientService.GetClientById(id);
            model.Subjects = _subjectService.GetAllSubject();
            model.Rooms = _roomService.GetAllRoom();
            model.ClientLessons = _clientService.GetClientLessonViewDetail(id);
            string html = this.RenderPartialView("ClientLessonViewDetail", model);
            var jsonresults = Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
            jsonresults.MaxJsonLength = int.MaxValue;
            return jsonresults;
        }
        public ActionResult ListClientLesson()
        {
            var model = new Client();
            model.Subjects = _subjectService.GetAllSubject();
            model.Rooms = _roomService.GetAllRoom();
            model.ClientLessons = _clientService.GetListLessons();
            string html = this.RenderPartialView("ListClientLesson", model);
            var jsonresults = Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
            jsonresults.MaxJsonLength = int.MaxValue;
            return jsonresults;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClientLesson(Client model)
        {
            try
            {
                var response = _clientService.CreateClientLesson(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                var jsonresults = Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message,

                });
                jsonresults.MaxJsonLength = int.MaxValue;
                return jsonresults;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ListClientLesson(Client model)
        {
            try
            {
                if (model.EndDate.HasValue)
                {
                    foreach (var id in model.ListId)
                    {

                        var res = _clientService.CreateClientPayment(new Client
                        {
                            ClientId = id,
                            Amount = 1000,
                            PaymentDate = DateTime.Now,
                            PaymentCode = "000",
                            Description = "",
                            BeginDate = DateTime.Now,
                            EndDate = model.EndDate,
                            CreatedUserName = "STEM+"
                        });

                    }
                }
                var response = _clientService.CreateListClientLesson(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                var jsonresults = Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                }, JsonRequestBehavior.AllowGet);
                jsonresults.MaxJsonLength = int.MaxValue;
                return jsonresults;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var response = _clientService.Delete(Id);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                var jsonresults = Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                }, JsonRequestBehavior.AllowGet);
                jsonresults.MaxJsonLength = int.MaxValue;
                return jsonresults;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "_ClientService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        protected void GetViewBag()
        {
            ViewBag.SchoolCodes = _schoolService.GetAllCode();
        }

        [HttpGet]
        public ActionResult GetSchool(string schoolcode)
        {
            School school = _schoolService.GetByCode(schoolcode);
            return Json(new
            {
                Data = school,
                Success = school != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult Approve(Client model)
        {
            try
            {
                User identity = System.Web.HttpContext.Current.Session["UserSession"] as User;
                if (identity != null) model.ApproverId = identity.UserId;
                var response = _clientService.ApproveListClient(model);

                return Json(new
                {
                    Html = "",
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult LockClients(Client model)
        {
            try
            {
                var response = _clientService.LockListClient(model);

                return Json(new
                {
                    Html = "",
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Lock Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult ReactiveClients(Client model)
        {
            try
            {
                var response = _clientService.ReactiveListClient(model);

                return Json(new
                {
                    Html = "",
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Reactive Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult DeleteClients(Client model)
        {
            try
            {
                var response = _clientService.DeleteListClient(model);

                return Json(new
                {
                    Html = "",
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        [HttpGet]
        [RequiredLogin]
        public ActionResult ResetPassword(int id)
        {
            var model = _clientService.GetClientById(id);
            model.ConfirmPassword = "";
            model.PassWork = "";
            string html = this.RenderPartialView("ResetPassword", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResetPassword(Client model)
        {
            try
            {
                var response = _clientService.ResetPassword(model);
                string html = "";
                if (response.Success)
                {
                    //var lst = GetModel(0);
                    var lst = GetModel(0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ResetPassword client");
                return Json(new { Success = false, error = "Unable to add ResetPassword client at this time." });
            }
        }

        //Export
        public ActionResult ExportClient(string tclass, string schoolCode, string cityCode, string districtCode, string fullName, string deploymentmethod, int approve)
        {
            int totalRow = 0;
            string usercitycode = Session["CityCode"] + "";
            string userdistrictcode = Session["DistrictCode"] + "";

            var data = _clientService.GetClientByPage(0, tclass, schoolCode, cityCode, districtCode, fullName, deploymentmethod, approve, usercitycode, userdistrictcode, 100000000, out totalRow);
            string fileNameSave = "client_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            string filePath = Server.MapPath("/Images/" + fileNameSave);

            var fileInfo = new FileInfo(filePath);
            using (ExcelPackage excel = new ExcelPackage(fileInfo))
            {

                var sheet = excel.Workbook.Worksheets.Add("exported");
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 50;
                sheet.Column(10).Width = 50;
                sheet.Column(11).Width = 50;
                sheet.Cell(1, 1).Value = "STT";
                sheet.Cell(1, 2).Value = "Họ tên";
                sheet.Cell(1, 3).Value = "Tên đăng nhập";
                sheet.Cell(1, 4).Value = "Lớp";
                sheet.Cell(1, 5).Value = "SĐT";
                sheet.Cell(1, 6).Value = "Mã trường";
                sheet.Cell(1, 7).Value = "Mã tỉnh thành";
                sheet.Cell(1, 8).Value = "Mã quận huyện";
                sheet.Cell(1, 9).Value = "Địa chỉ";
                sheet.Cell(1, 10).Value = "Sử dụng toàn bộ bài học";
                sheet.Cell(1, 11).Value = "Mã máy";
                sheet.Cell(1, 12).Value = "Đang khóa";

                int rowCount = 2; // start row (in row 1 are header cells)

                foreach (Client client in data)
                {
                    {

                        {
                            try
                            {
                                sheet.Cell(rowCount, 1).Value = (rowCount - 1).ToString();
                                sheet.Cell(rowCount, 2).Value = client.FullName + "";
                                sheet.Cell(rowCount, 3).Value = client.UserName + "";
                                sheet.Cell(rowCount, 4).Value = client.Email + "";
                                sheet.Cell(rowCount, 5).Value = client.Phone + "";
                                sheet.Cell(rowCount, 6).Value = client.SchoolCode + "";
                                sheet.Cell(rowCount, 7).Value = client.CityCode + "";
                                sheet.Cell(rowCount, 8).Value = client.DistrictCode + "";
                                sheet.Cell(rowCount, 9).Value = client.Address + "";
                                sheet.Cell(rowCount, 10).Value = client.FullLesson ? "Y" : "N";
                                sheet.Cell(rowCount, 11).Value = client.MacIp + "";
                                sheet.Cell(rowCount, 12).Value = client.IsLock ? "Y" : "N";
                                rowCount++;
                            }
                            catch
                            {
                                rowCount++;
                            }
                        }

                    }
                }

                excel.Save();
            }

            return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

        }

        public ActionResult ExportSchool(string schoolCode, string cityCode, string districtCode)
        {
            int totalRow = 0;
            string usercitycode = Session["CityCode"] + "";
            string userdistrictcode = Session["DistrictCode"] + "";

            var data = _clientService.GetClientByPage(0, "", schoolCode, cityCode, districtCode, "", "", -1, usercitycode, userdistrictcode, 100000000, out totalRow);
            string fileNameSave = "school_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            string filePath = Server.MapPath("/Images/" + fileNameSave);

            var fileInfo = new FileInfo(filePath);
            using (ExcelPackage excel = new ExcelPackage(fileInfo))
            {

                var sheet = excel.Workbook.Worksheets.Add("school");
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 50;
                sheet.Column(10).Width = 50;
                sheet.Column(11).Width = 20;
                sheet.Cell(1, 1).Value = "STT";
                sheet.Cell(1, 2).Value = "Mã trường";
                sheet.Cell(1, 3).Value = "Tên trường";
                sheet.Cell(1, 4).Value = "Mã tỉnh thành";
                sheet.Cell(1, 5).Value = "Mã quận huyện";
                sheet.Cell(1, 6).Value = "Hình thức triển khai";
                sheet.Cell(1, 7).Value = "Số học sinh";
                sheet.Cell(1, 8).Value = "Số giáo viên";
                sheet.Cell(1, 9).Value = "In giáo án";
                sheet.Cell(1, 10).Value = "Giấy chứng nhận";

                int rowCount = 2; // start row (in row 1 are header cells)
                int rank = 1;
                var listSchool = data.Select(p => p.SchoolCode).Distinct();

                foreach (var school in listSchool)
                {
                    try
                    {
                        var objschool = _schoolService.GetByCode(school);

                        sheet.Cell(rowCount, 1).Value = (rank).ToString();
                        sheet.Cell(rowCount, 2).Value = objschool.Code + "";
                        sheet.Cell(rowCount, 3).Value = objschool.Name + "";
                        sheet.Cell(rowCount, 4).Value = objschool.CityCode + "";
                        sheet.Cell(rowCount, 5).Value = objschool.DistrictCode + "";
                        sheet.Cell(rowCount, 6).Value = objschool.DeploymentMethod + "";
                        sheet.Cell(rowCount, 7).Value = objschool.NumberOfStudent + "";
                        sheet.Cell(rowCount, 8).Value = objschool.NumberOfTeacher + "";
                        sheet.Cell(rowCount, 9).Value = objschool.PrintDocument + "";
                        sheet.Cell(rowCount, 10).Value = objschool.Certificate + "";

                        IEnumerable<Client> list = data.Where(p => p.SchoolCode == school);
                        if (list.Any())
                        {
                            int rankc = 1;
                            rowCount++;
                            sheet.Cell(rowCount, 2).Value = "STT";
                            sheet.Cell(rowCount, 3).Value = "Họ tên";
                            sheet.Cell(rowCount, 4).Value = "Tên đăng nhập";
                            sheet.Cell(rowCount, 5).Value = "Lớp";
                            sheet.Cell(rowCount, 6).Value = "SĐT";
                            sheet.Cell(rowCount, 7).Value = "Địa chỉ";
                            sheet.Cell(rowCount, 8).Value = "Sử dụng toàn bộ bài học";

                            rowCount++;
                            foreach (Client client in list)
                            {
                                sheet.Cell(rowCount, 2).Value = "" + rankc;
                                sheet.Cell(rowCount, 3).Value = client.FullName + "";
                                sheet.Cell(rowCount, 4).Value = client.UserName + "";
                                sheet.Cell(rowCount, 5).Value = client.Email + "";
                                sheet.Cell(rowCount, 6).Value = client.Phone + "";
                                sheet.Cell(rowCount, 7).Value = client.Address + "";
                                sheet.Cell(rowCount, 8).Value = client.FullLesson ? "Y" : "N";

                                rankc++;
                                rowCount++;
                            }
                        }
                    }
                    catch
                    {
                    }
                    rowCount++;
                    rank++;
                }

                excel.Save();
            }

            return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

        }

        //Export
        public ActionResult ExportClientLesson(string tclass, string schoolCode, string cityCode, string districtCode, string fullName, int approve)
        {
            int totalRow = 0;
            string usercitycode = Session["CityCode"] + "";
            string userdistrictcode = Session["DistrictCode"] + "";

            var data = _clientService.GetClientByPage(0, tclass, schoolCode, cityCode, districtCode, fullName, "", approve, usercitycode, userdistrictcode, 100000000, out totalRow);
            string fileNameSave = "clientlesson_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            string filePath = Server.MapPath("/Images/" + fileNameSave);

            var fileInfo = new FileInfo(filePath);
            using (ExcelPackage excel = new ExcelPackage(fileInfo))
            {

                var sheet = excel.Workbook.Worksheets.Add("clientlesson");
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 50;
                sheet.Column(10).Width = 50;
                sheet.Column(11).Width = 20;
                sheet.Cell(1, 1).Value = "STT";
                sheet.Cell(1, 2).Value = "Họ tên";
                sheet.Cell(1, 3).Value = "Tên đăng nhập";
                sheet.Cell(1, 4).Value = "Lớp";
                sheet.Cell(1, 5).Value = "SĐT";
                sheet.Cell(1, 6).Value = "Mã trường";
                sheet.Cell(1, 7).Value = "Mã tỉnh thành";
                sheet.Cell(1, 8).Value = "Mã quận huyện";

                int rowCount = 2; // start row (in row 1 are header cells)
                int rank = 1;
                foreach (Client client in data)
                {
                    try
                    {
                        sheet.Cell(rowCount, 1).Value = (rank).ToString();
                        sheet.Cell(rowCount, 2).Value = client.FullName + "";
                        sheet.Cell(rowCount, 3).Value = client.UserName + "";
                        sheet.Cell(rowCount, 4).Value = client.Email + "";
                        sheet.Cell(rowCount, 5).Value = client.Phone + "";
                        sheet.Cell(rowCount, 6).Value = client.SchoolCode + "";
                        sheet.Cell(rowCount, 7).Value = client.CityCode + "";
                        sheet.Cell(rowCount, 8).Value = client.DistrictCode + "";

                        IEnumerable<Lesson> list = _clientService.GetClientLessonViewDetail(client.ClientId);
                        if (list.Any())
                        {
                            int rankc = 1;
                            rowCount++;
                            sheet.Cell(rowCount, 2).Value = "STT";
                            sheet.Cell(rowCount, 3).Value = "Mã bài học";
                            sheet.Cell(rowCount, 4).Value = "Tên bài học";
                            sheet.Cell(rowCount, 5).Value = "Lượt download";
                            sheet.Cell(rowCount, 6).Value = "Lượt sử dụng";
                            sheet.Cell(rowCount, 7).Value = "Sử dụng lần cuối";
                            rowCount++;
                            foreach (Lesson lesson in list)
                            {
                                sheet.Cell(rowCount, 2).Value = "" + rankc;
                                sheet.Cell(rowCount, 3).Value = lesson.Code + "";
                                sheet.Cell(rowCount, 4).Value = lesson.Name + "";
                                sheet.Cell(rowCount, 5).Value = lesson.OpenCount + "";
                                sheet.Cell(rowCount, 6).Value = lesson.DownloadCount + "";
                                sheet.Cell(rowCount, 7).Value = string.IsNullOrEmpty(lesson.LastUse) ? lesson.LastOpen.Value.ToString("dd/MM/yyyy hh:mm:ss") : lesson.LastUse + "";

                                rankc++;
                                rowCount++;
                            }
                        }

                    }
                    catch
                    {
                    }

                    rowCount++;
                    rank++;
                }

                excel.Save();
            }

            return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

        }

    }
}