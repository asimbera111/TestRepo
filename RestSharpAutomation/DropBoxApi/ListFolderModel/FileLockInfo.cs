using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.DropBoxApi.ListFolderModel
{
    public class FileLockInfo
    {
        public bool is_lockholder { get; set; }
        public string lockholder_name { get; set; }
        public DateTime created { get; set; }
    }
}
