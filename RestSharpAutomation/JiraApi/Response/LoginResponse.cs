using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.JiraApi.Response
{
    public class LoginResponse
    {
        public Session session { get; set; }
        public LoginInfo loginInfo { get; set; }
    }
}
