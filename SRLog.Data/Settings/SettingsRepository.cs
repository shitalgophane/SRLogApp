using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Data;
using SRLog.Entities.Settings.ViewModels;

namespace SRLog.Data.Settings
{
    public class SettingsRepository
    {
        SR_Log_DatabaseSQLEntities1 db = new SR_Log_DatabaseSQLEntities1();
        public bool IsSettingsExistsForUser(int userid)
        {
            var setting = (from u in db.tblSettings
                           where u.UserId == userid
                           select u).ToList();

            if (setting.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<string> GetExistingConfigurableColumns(int userid)
        {
            List<string> setting = (from u in db.tblSettings
                                    where u.UserId == userid
                                    && u.SectionName == "ReportQuery" && u.IsFixedInGrid == false
                                    select u.Value).ToList();
            return setting;
        }

        public List<string> GetExistingColumns(int userid)
        {
            List<string> setting = (from u in db.tblSettings
                                    where u.UserId == userid
                                    && u.SectionName == "ReportQuery"
                                    select u.Value).ToList();

            return setting;
        }

        public List<string> GetExistingColumnsWidth(int userid)
        {
            List<string> setting = (from u in db.tblSettings
                                    where u.UserId == userid
                                    && u.SectionName == "Grid_Dsp_COLUMNSETTINGS1"
                                    && u.Key != "Col_1"
                                    select u.Value).ToList();

            return setting;
        }

        public List<SRLogDisplayViewModel> GetExistingColumnsWithDisplayNames(int userid)
        {
            List<string> setting = (from u in db.tblSettings
                                    where u.UserId == userid
                                    && u.SectionName == "ReportQuery"
                                    select u.Value).ToList();

            List<SRLogDisplayViewModel> srlogs = new List<SRLogDisplayViewModel>();

            foreach (string s in setting)
            {
                string dispname = (from u in db.tblSRLogColumns
                                   where u.FieldName == s
                                   select u.DisplayName).FirstOrDefault();

                SRLogDisplayViewModel disp = new SRLogDisplayViewModel();
                disp.FieldName = s;
                disp.DisplayName = dispname;
                srlogs.Add(disp);
            }
            return srlogs;
        }

        public void AddSetting(int userid, string section, string key, string value, bool IsFixed = false)
        {

            tblSetting setting = new tblSetting();
            setting.UserId = userid;
            setting.SectionName = section;
            setting.Key = key;
            setting.Value = value;
            setting.IsFixedInGrid = IsFixed;
            db.tblSettings.Add(setting);

            db.SaveChanges();
        }

        public void UpdateSetting(int userid, string section, string key, string value)
        {
            tblSetting setting = (from u in db.tblSettings
                                  where u.UserId == userid && u.SectionName == section && u.Key == key
                                  select u).FirstOrDefault();

            if (setting != null)
            {
                setting.Value = value;
                db.SaveChanges();
            }
            else
            {
                AddSetting(userid, section, key, value, false);
            }
        }
        public void SaveColumnOrdering(int userid, List<string> columns)
        {
            List<tblSetting> setting = (from u in db.tblSettings
                                        where u.UserId == userid
                                        && u.SectionName == "ReportQuery" && u.IsFixedInGrid == false
                                        select u).ToList();
            //Delete all existing settings

            if (setting.Count > 0)
            {
                foreach (tblSetting t in setting)
                {
                    db.tblSettings.Remove(t);
                    db.SaveChanges();
                }
            }
            //Save new settings
            if (columns.Count > 0)
            {
                foreach (string c in columns)
                {
                    if (c != "Id")
                    {
                        AddSetting(userid, "ReportQuery", c, c, false);
                    }
                }
            }

        }
    }
}
