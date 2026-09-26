using System.Collections.Generic;

namespace Model
{
    public class ClientImport
    {
        public List<Client> Clients { set; get; }
        public string Error { get; set; }
    }
}