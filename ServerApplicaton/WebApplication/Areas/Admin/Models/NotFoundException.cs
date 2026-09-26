using System;
using System.Net;
using System.Web;

namespace WebApplication.Areas.Admin.Models
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
            throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
        }

        public NotFoundException(string message) : base(message)
        {
            throw new HttpException((int)HttpStatusCode.NotFound, message);
        }
    }
}
