using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXWindows.Helper
{
    public class MyWebClient : System.Net.WebClient
    {
        //time in milliseconds
        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        public MyWebClient()
        {
            this.timeout = 60000;
        }

        public MyWebClient(int timeout)
        {
            this.timeout = timeout;
        }

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = this.timeout;
            return result;
        }
    }
}
