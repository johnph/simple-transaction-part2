using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.WebApi.Models
{
    public class UserAccount
    {
        public string Name { get; set; }
        public int AccountNumber { get; set; }        
        public string Currency { get; set; }
    }
}
