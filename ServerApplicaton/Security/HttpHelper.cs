//using System;
//using System.Net.Http;
//using System.Web.Script.Serialization;

//namespace HRM.Security
//{
//    public class HttpHelper
//    {
//        public static T Post<T, TU>(TU request, string url)
//        {
//            using (var client = new HttpClient())
//            {
//                try
//                {
//                    var javaScriptSerializer = new JavaScriptSerializer();

//                    var resultContent = client.PostAsJsonAsync(url, request).Result;

//                    var msg = resultContent.Content.ReadAsStringAsync().Result;

//                    var data = javaScriptSerializer.Deserialize<T>(msg);
//                    return data;
//                }
//                catch (Exception e)
//                {
//                    throw e;
//                }
//            }
//        }

//        public static void Post<TU>(TU request, string url)
//        {
//            using (var client = new HttpClient())
//            {
//                try
//                {
//                    var javaScriptSerializer = new JavaScriptSerializer();
//                    var resultContent = client.PostAsJsonAsync(url, request).Result;
//                    var msg = resultContent.Content.ReadAsStringAsync().Result;
//                }
//                catch (Exception e)
//                {
//                    throw e;
//                }
//            }
//        }
//    }
//}