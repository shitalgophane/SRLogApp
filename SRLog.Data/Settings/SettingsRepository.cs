using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Entities;
using SRLog.Entities.Settings.ViewModels;
using SRLog.Data.Activity;
namespace SRLog.Data.Settings
{
    public class SettingsRepository
    {
        SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
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
            //List<string> setting = (from u in db.tblSettings
            //                        where u.UserId == userid
            //                        && u.SectionName == "ReportQuery"
            //                        select u.Value).ToList();

            //List<SRLogDisplayViewModel> srlogs = new List<SRLogDisplayViewModel>();

            //foreach (string s in setting)
            //{
            //    string dispname = (from u in db.tblSRLogColumns
            //                       where u.FieldName == s
            //                       select u.DisplayName).FirstOrDefault();

            //    SRLogDisplayViewModel disp = new SRLogDisplayViewModel();
            //    disp.FieldName = s;
            //    disp.DisplayName = dispname;
            //    srlogs.Add(disp);
            //}
            //return srlogs;

            List<string> settingfixedcol = (from u in db.tblSettings
                                            where u.UserId == userid
                                            && u.SectionName == "ReportQuery" && u.IsFixedInGrid == true
                                            select u.Value).ToList();


            List<string> settingothercol = (from u in db.tblSettings
                                            where u.UserId == userid && u.IsFixedInGrid == false
                                            && u.SectionName == "ReportQuery"
                                            select u.Value).ToList();




            List<SRLogDisplayViewModel> srlogs = new List<SRLogDisplayViewModel>();


            foreach (var s in settingfixedcol)
            {
                string dispname = (from u in db.tblSRLogColumns
                                   where u.FieldName == s
                                   select u.DisplayName).FirstOrDefault();


                SRLogDisplayViewModel disp = new SRLogDisplayViewModel();
                disp.FieldName = s;
                disp.DisplayName = dispname;
                disp.Visible = true;
                disp.fixedcolumn = true;
                srlogs.Add(disp);
            }
            foreach (var s in settingothercol)
            {
                string dispname = (from u in db.tblSRLogColumns
                                   where u.FieldName == s
                                   select u.DisplayName).FirstOrDefault();


                SRLogDisplayViewModel disp = new SRLogDisplayViewModel();
                disp.FieldName = s;
                disp.DisplayName = dispname;
                disp.Visible = true;
                disp.fixedcolumn = false;
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

        public List<tblSRLogColumn> GetSortableColumnNames(int userid)
        {
            List<tblSRLogColumn> dispnames = (from u in db.tblSRLogColumns
                                              where u.IsSortable == true
                                              orderby u.FieldName
                                              select u).ToList();


            return dispnames;
        }

        public string GetSort(string SectionName, string key,int userid)
        {
            string sortby = (from u in db.tblSettings
                             where u.SectionName == SectionName && u.Key == key && u.UserId==userid
                             select u.Value).FirstOrDefault();

            return sortby;
        }

        #region Manage User
        public List<tblUser> GetUser()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<tblUser> user = (db.tblUsers.Where(c => c.User_Name != "Admin")).OrderBy(c => c.User_Name).ToList();
            return user;
        }
        public List<tblUser> GetUserById(string UserId)
        {
            db = new SR_Log_DatabaseSQLEntities();
            int nUser = Convert.ToInt32(UserId);
            List<tblUser> user = (db.tblUsers.Where(c => c.UserId == nUser)).OrderBy(c => c.User_Name).ToList();
            return user;
        }
        public int AddUpdateUser(UserViewModel model, string Flag)
        {
            db = new SR_Log_DatabaseSQLEntities();
            int UserId = 0;
            if (Flag == "A")
            {
                tblUser user = new tblUser();
                user.User_Name = model.User_Name;
                user.User_Password = model.User_Password;
                if (model.SRlogReadonly == true)
                {
                    model.SR_Log_ReadOnly = true;
                }
                else
                {
                    model.SR_Log_ReadOnly = false;
                }
                user.SR_Log_ReadOnly = model.SR_Log_ReadOnly;
                if (model.BidlogReadonly == true)
                {
                    model.BidlogReadonly = true;
                }
                else
                {
                    model.BidlogReadonly = false;
                }
                
                user.Bid_Log_ReadOnly = model.BidlogReadonly;
                user.Admin_Rights = model.Admin_Rights;
                user.Accounting_Rights = model.Accounting_Rights;
                user.DatabaseUpdate_Rights = model.DatabaseUpdate_Rights;

                using (var context = new SR_Log_DatabaseSQLEntities())
                {

                    context.tblUsers.Add(user);
                    context.SaveChanges();
                    UserId = user.UserId;
                }
            }
            else
            {
                tblUser user = (from c in db.tblUsers
                                where c.UserId == model.UserId
                                select c).SingleOrDefault();


                user.User_Name = model.User_Name;
                user.User_Password = model.User_Password;
                if (model.SRlogReadonly == true)
                {
                    model.SR_Log_ReadOnly = true;
                }
                else
                {
                    model.SR_Log_ReadOnly = false;
                }
                user.SR_Log_ReadOnly = model.SR_Log_ReadOnly;
                if (model.BidlogReadonly == true)
                {
                    model.BidlogReadonly = true;
                }
                else
                {
                    model.BidlogReadonly = false;
                }
                user.Bid_Log_ReadOnly = model.BidlogReadonly;
                user.Admin_Rights = model.Admin_Rights;
                user.Accounting_Rights = model.Accounting_Rights;
                user.DatabaseUpdate_Rights = model.DatabaseUpdate_Rights;

                db.SaveChanges();
                UserId = user.UserId;
            }

            return UserId;
        }
        #endregion

        #region Manage Project Manager
        public List<tblGroupUser> GetProjectManager()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<tblGroupUser> user = (db.tblGroupUsers.Where(c => c.Group_Name == "TimesheetProjectMgr")).OrderBy(c => c.UserName).ToList();
            return user;
        }

        public string AddUpdateProjectManager(ProjectManagerViewModel model)
        {
            db = new SR_Log_DatabaseSQLEntities();
            string UserId = "";

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                tblGroupUser user = new tblGroupUser();
                user.Userid = model.Userid;
                user.UserName = model.UserName;
                user.Group_Name = "TimesheetProjectMgr";
                context.tblGroupUsers.Add(user);
                context.SaveChanges();
                UserId = user.Userid;
            }



            return UserId;

        }
        #endregion

        #region Manage Mail List

        public List<Bidlog_Result_Mail> GetBidLogResultMail()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<Bidlog_Result_Mail> bidlogresultmail = (db.Bidlog_Result_Mail).ToList();
            return bidlogresultmail;
        }

        public List<Customer_Info_Mail> GetCustomerInfoMail()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<Customer_Info_Mail> customerinfomail = (db.Customer_Info_Mail).ToList();
            return customerinfomail;
        }



        public List<Bonding_Mail_TO> GetBondingMailTo()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<Bonding_Mail_TO> bondingmailto = (db.Bonding_Mail_TO).ToList();
            return bondingmailto;
        }

        public List<Bonding_Mail_TO> GetBondingMailTo()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<Bonding_Mail_TO> bondingmailto = (db.Bonding_Mail_TO).ToList();
            return bondingmailto;
        }
        public List<Bonding_Mail_CC> GetBondingMailCC()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<Bonding_Mail_CC> bondingmailcc = (db.Bonding_Mail_CC).ToList();
            return bondingmailcc;
        }
        public List<EMail_Info> GetEmailPerson()
        {
            db = new SR_Log_DatabaseSQLEntities();

            List<EMail_Info> emailinfo = (db.EMail_Infoes).OrderBy(c => c.Name).ToList();
            return emailinfo;
        }

        public void SaveMailInfo(string[] bonding, string[] bidresult, string[] customerinfo, string User)
        {


            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            List<Bonding_Mail_TO> allEmails = (from p in db.Bonding_Mail_TO select p).ToList();
            foreach (Bonding_Mail_TO t in allEmails)
            {
                db.Bonding_Mail_TO.Remove(t);
                db.SaveChanges();
            }
            var act = new ActivityRepository();
            act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "All existing bonding mails deleted by " + User);
            //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Delete all bonding mails", "All existing bonding mails deleted by " & UserInfo.UserName)
            for (int i = 0; i < bonding.Length; i++)
            {
                if (bonding[i].ToString().Trim() != "")
                {
                    Bonding_Mail_TO objbonding = new Bonding_Mail_TO();
                    objbonding.BondingMailTO = bonding[i].ToString();
                    db.Bonding_Mail_TO.Add(objbonding);
                    db.SaveChanges();
                    act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "Bonding mail " + bonding[i].ToString() + " added in bonding mails by " + User);
                }
                //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Insert bonding mail", "Bonding mail " & lstTo.List(j) & " added in bonding mails by " & UserInfo.UserName)
            }



            List<Customer_Info_Mail> allEmailsofcustomer = (from p in db.Customer_Info_Mail select p).ToList();
            foreach (Customer_Info_Mail t in allEmailsofcustomer)
            {
                db.Customer_Info_Mail.Remove(t);
                db.SaveChanges();
            }
            act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "All existing customer info mails deleted by " + User);
            //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Delete all customer info mails", "All existing customer info mails deleted by " & UserInfo.UserName)
            for (int i = 0; i < customerinfo.Length; i++)
            {
                if (customerinfo[i].ToString().Trim() != "")
                {
                    Customer_Info_Mail objcustomer = new Customer_Info_Mail();
                    objcustomer.CustomerInfoMail = customerinfo[i].ToString();
                    db.Customer_Info_Mail.Add(objcustomer);
                    db.SaveChanges();
                    act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "Customer info mail " + customerinfo[i].ToString() + " added in customer info mails by " + User);

                    //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Insert customer info mail", "Customer info mail " & lstCustTo.List(j) & " added in customer info mails by " & UserInfo.UserName)
                }
            }



            List<Bidlog_Result_Mail> allEmailsbidresult = (from p in db.Bidlog_Result_Mail select p).ToList();
            foreach (Bidlog_Result_Mail t in allEmailsbidresult)
            {
                db.Bidlog_Result_Mail.Remove(t);
                db.SaveChanges();
            }
            act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "All existing bidlog result mails deleted by " + User);
            //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Delete all bidlog result mails", "All existing bidlog result mails deleted by " & UserInfo.UserName)
            for (int i = 0; i < bidresult.Length; i++)
            {
                if (bidresult[i].ToString().Trim() != "")
                {
                    Bidlog_Result_Mail objbidresult = new Bidlog_Result_Mail();
                    objbidresult.BidlogResultMail = bidresult[i].ToString();
                    db.Bidlog_Result_Mail.Add(objbidresult);
                    db.SaveChanges();
                    act.AddActivityLog(User, "ManageMailList", "SaveMailInfo", "Bidlog mail  " + bidresult[i].ToString() + " added in bidlog results mails by " + User);
                    //Call SaveActivityLog("Frm_Bonding_prevailing_Email", "Insert bidlog result mail", "Bidlog mail " & lstBidTo.List(j) & " added in bidlog results mails by " & UserInfo.UserName)
                }
            }

        }



        public int AddUpdateEmailInfo(string Name, string Email, string Flag,int id)
        {
            int EmailInfoId = 0;
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (Flag == "A")
            {
                EMail_Info e = new EMail_Info();
                e.Name = Name;
                e.Email = Email;
                db.EMail_Infoes.Add(e);
                db.SaveChanges();
                EmailInfoId = e.Id;
            }
            else if (Flag == "E")
            {
                EMail_Info ei =( from e in db.EMail_Infoes
                                where e.Id == id
                                select e).FirstOrDefault();
                ei.Name = Name;
                ei.Email = Email;
                db.SaveChanges();

                EmailInfoId = id;
            }

            return EmailInfoId;
        }
        #endregion

        #region Manage Scheduled Maintenance
        public ScheduledMaintenanceViewModel GetScheduledMaintenance()
        {
            db = new SR_Log_DatabaseSQLEntities();

            ScheduledMaintenanceViewModel rec = new ScheduledMaintenanceViewModel();
            
            tblScheduledMaintenance tblrec= db.tblScheduledMaintenances.FirstOrDefault ();
            if(tblrec !=null)
            {
                rec.FromDate = tblrec.FromDate;
                rec.ToDate = tblrec.ToDate;
                rec.Message = tblrec.Message;
            }

            return rec;
        }

        public string AddUpdateScheduledMaintenance(ScheduledMaintenanceViewModel model)
        {
            db = new SR_Log_DatabaseSQLEntities();
            string UserId = "";

            List<tblScheduledMaintenance> setting = (from u in db.tblScheduledMaintenances 
                                        select u).ToList();
            //Delete all existing schedules

            if (setting.Count > 0)
            {
                foreach (tblScheduledMaintenance t in setting)
                {
                    db.tblScheduledMaintenances.Remove(t);
                    db.SaveChanges();
                }
            }

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                tblScheduledMaintenance rec = new tblScheduledMaintenance();
                rec.FromDate = model.FromDate;
                rec.ToDate = model.ToDate;
                rec.Message = model.Message;
                context.tblScheduledMaintenances.Add(rec);
                context.SaveChanges();
                UserId = "Scheduled Maintenance Set Successfully";
            }



            return UserId;

        }
        #endregion
    }
}
