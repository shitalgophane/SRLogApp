using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.ActivityLog.ViewModels
{
    public class ActivityLogViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public string FormName { get; set; }
        public string Action { get; set; }
        public string Comment { get; set; }
    }
}
