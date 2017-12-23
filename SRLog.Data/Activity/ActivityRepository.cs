using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Data;
using SRLog.Entities.ActivityLog.ViewModels;

namespace SRLog.Data.Activity
{
    public class ActivityRepository
    {
        SR_Log_DatabaseSQLEntities1 db = new SR_Log_DatabaseSQLEntities1();

        public void AddActivityLog(string UserName, string strFormName, string strAction, string strComment)
        {
            db.USP_TT_InsertActivityLog(UserName, DateTime.Now, strFormName, strAction, strComment);            
            db.SaveChanges();
        }
    }
}
