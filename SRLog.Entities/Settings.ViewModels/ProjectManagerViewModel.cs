using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.Settings.ViewModels
{
 public class ProjectManagerViewModel
    {
       public string Userid { get; set; }
       public string UserName { get; set; }
       public string GroupName { get; set; }

       public List<tblGroupUser> ManagerList;
         
    }
}
