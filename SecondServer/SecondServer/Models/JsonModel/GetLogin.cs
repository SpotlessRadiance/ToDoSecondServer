using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Models.JsonModel
{
    public class GetLogin
    {
        public string Login { get; set; }
        public bool Authorized { get; set; }
        public long Id { get; set; }
        public string Role { get; set; }
    }
}
