using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Data;
using SRLog.Data.Settings;
using SRLog.Entities.SRLog.ViewModels;
using SRLog.Entities.Settings.ViewModels;

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
    }
}
