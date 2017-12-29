using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SRLog.Data;
using SRLog.Data.Settings;
using SRLog.Entities.SRLog.ViewModels;
using SRLog.Entities.Settings.ViewModels;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;

namespace SRLog.Data.SRLog
{
    public class SRLogRepository
    {
        SR_Log_DatabaseSQLEntities1 db = new SR_Log_DatabaseSQLEntities1();

        public List<string> GetColumns(int userid)
        {
            SettingsRepository _repository = new SettingsRepository();
            List<string> existingcolumns = _repository.GetExistingColumns(userid);

            List<string> columnnames = (from t in typeof(tblSR_Log).GetProperties() select t.Name).ToList();

            columnnames.RemoveAll(x => existingcolumns.Contains(x));

            return columnnames;
        }

        public List<string> GetConfiguredSRLogColumns(int userid)
        {
            SettingsRepository _repository = new SettingsRepository();
            List<string> existingcolumns = _repository.GetExistingConfigurableColumns(userid);


            return existingcolumns;
        }

        public List<tblSR_Log> GetSRLogsList(int UserId, int startIndex, int count, string sorting)
        {
            IEnumerable<tblSR_Log> query = db.tblSR_Logs.OrderByDescending(x => x.SRNumber);

            return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging

        }

        public int GetSRLogcount()
        {
            return db.tblSR_Logs.Count();
        }

        public List<tblSR_Log> GetSRLogsListByFilter(string keyword, int UserId, int startIndex, int count, string sorting)
        {
            IEnumerable<tblSR_Log> query = db.tblSR_Logs.Where(x =>
                    SqlFunctions.PatIndex("%" + keyword + "%", SqlFunctions.StringConvert(x.SRNumber)) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Customer) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.CustomerContact) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ProjectDescription) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ContactEmail) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ContactPhone) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.WhoJobWalk) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Estimator) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Notes) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Owner) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.JobsiteAddress) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Bonding) > 0
               ).OrderByDescending(x => x.SRNumber);

            return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging

        }

        public int GetSRLogcountByFilter(string keyword)
        {
            return db.tblSR_Logs.Where(x =>
                    SqlFunctions.PatIndex("%" + keyword + "%", SqlFunctions.StringConvert(x.SRNumber)) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Customer) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.CustomerContact) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ProjectDescription) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ContactEmail) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.ContactPhone) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.WhoJobWalk) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Estimator) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Notes) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Owner) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.JobsiteAddress) > 0
                    || SqlFunctions.PatIndex("%" + keyword + "%", x.Bonding) > 0
               ).Count();
        }
    }
}
