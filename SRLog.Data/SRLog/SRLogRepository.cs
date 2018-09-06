using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SRLog.Entities;
using SRLog.Data.Settings;
using SRLog.Entities.SRLog.ViewModels;
using SRLog.Entities.Settings.ViewModels;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data;
using System.Data.Objects;
using SRLog.Common;

namespace SRLog.Data.SRLog
{
    public class SRLogRepository
    {
        SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

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

        public List<SRLogViewModel> GetSRLogsList(int UserId, string sorting1, string sorting2, string fromdate, string todate, int startIndex, int count, string sorting)
        {
            //IEnumerable<tblSR_Log> query = db.tblSR_Logs;//.OrderByDescending(x =>  x.SRNumber);
            db = new SR_Log_DatabaseSQLEntities();
            var query = (from x in db.tblSR_Logs
                         select new SRLogViewModel
                         {
                             SRNumber = x.SRNumber,
                             Customer = x.Customer,
                             Owner = x.Owner,
                             ProjectDescription = x.ProjectDescription,
                             Division = x.Division,

                             InactiveJob = (x.InactiveJob == null || x.InactiveJob == false ? false : true),
                             ProjectManager = x.ProjectManager,
                             Estimator = x.Estimator,
                             CreationDate = x.CreationDate,
                             JobOrQuote = x.JobOrQuote,
                             ContactEmail = x.ContactEmail,
                             SiteForeman = x.SiteForeman,
                             CustomerContact = x.CustomerContact,
                             FileFolder = x.FileFolder,
                             PW = x.PW,
                             ChemFeed = x.ChemFeed,
                             ContactPhone = x.ContactPhone,
                             QuoteDue = x.QuoteDue,
                             WhoJobWalk = x.WhoJobWalk,
                             NotQuoted = x.NotQuoted,
                             Closed = x.Closed,
                             BidAsPrimeOrSub = x.BidAsPrimeOrSub,
                             Bonding = x.Bonding,
                             BondingMailSent = x.BondingMailSent,
                             PrevailingMailSent = x.PrevailingMailSent,
                             Notes = x.Notes,
                             JobsiteAddress = x.JobsiteAddress,
                             ServerJobFolder = x.ServerJobFolder
                         });


            if (fromdate != "" && todate != "")
            {
                DateTime date1 = Convert.ToDateTime(fromdate);
                DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                DateTime date2 = Convert.ToDateTime(todate);
                DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);

                query = query.Where(x => x.CreationDate >= startdate && x.CreationDate <= enddate);
            }

            if (sorting1 != "" && sorting2 != "")
            {
                string sort = sorting1 + "," + sorting2;
                query = query.OrderBy(sort);
            }
            else if (sorting1 != "" && sorting2 == "")
            {

                query = query.OrderBy(sorting1);
            }
            else if (sorting1 == "" && sorting2 != "")
            {
                query = query.OrderBy(sorting2);
            }
            else
            {
                query = query.OrderByDescending(x => x.SRNumber);
            }


            //return query.ToList();
            return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging

        }


        //public IEnumerable<tblSR_Log> GetSRLogsList(int UserId, string sorting1, string sorting2, string fromdate, string todate)
        //{
        //    //IEnumerable<tblSR_Log> query = db.tblSR_Logs;//.OrderByDescending(x =>  x.SRNumber);
        //    //if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
        //    //{
        //    //    DateTime date1 = Convert.ToDateTime(fromdate);
        //    //    DateTime date2 = Convert.ToDateTime(todate);
        //    //    query = query.Where(x => x.CreationDate >= date1 && x.CreationDate <= date2);
        //    //}

        //    //if (!string.IsNullOrEmpty(sorting1) && !string.IsNullOrEmpty(sorting2))
        //    //{
        //    //    string sort = sorting1 + "," + sorting2;
        //    //    query = query.OrderBy(sort);
        //    //}
        //    //else if (!string.IsNullOrEmpty(sorting1) && string.IsNullOrEmpty(sorting2))
        //    //{
        //    //    query = query.OrderBy(sorting1);
        //    //}
        //    //else if (string.IsNullOrEmpty(sorting1) && !string.IsNullOrEmpty(sorting2))
        //    //{
        //    //    query = query.OrderBy(sorting2);
        //    //}
        //    //else
        //    //{
        //    //    query = query.OrderByDescending(x => x.SRNumber);
        //    //}

        //    //return query.ToList();


        //    var query = from s in db.tblSR_Logs
        //                select s;
        //    if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
        //    {
        //        DateTime date1 = Convert.ToDateTime(fromdate);
        //        DateTime date2 = Convert.ToDateTime(todate);
        //        query = query.Where(x => x.CreationDate >= date1 && x.CreationDate <= date2);
        //    }

        //    if (!string.IsNullOrEmpty(sorting1) && !string.IsNullOrEmpty(sorting2))
        //    {
        //        string sort = sorting1 + "," + sorting2;
        //        query = query.OrderBy(sort);
        //    }
        //    else if (!string.IsNullOrEmpty(sorting1) && string.IsNullOrEmpty(sorting2))
        //    {
        //        query = query.OrderBy(sorting1);
        //    }
        //    else if (string.IsNullOrEmpty(sorting1) && !string.IsNullOrEmpty(sorting2))
        //    {
        //        query = query.OrderBy(sorting2);
        //    }
        //    else
        //    {
        //        query = query.OrderByDescending(x => x.SRNumber);
        //    }

        //    return query.Take(500);


        //}

        public int GetSRLogcount(string fromdate, string todate)
        {
            if (fromdate == "" && todate == "")
            {
                return db.tblSR_Logs.Count();
            }
            else
            {
                //DateTime date1 = Convert.ToDateTime(fromdate);
                //DateTime date2 = Convert.ToDateTime(todate);

                DateTime date1 = Convert.ToDateTime(fromdate);
                DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                DateTime date2 = Convert.ToDateTime(todate);
                DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
                return db.tblSR_Logs.Where(x => x.CreationDate >= startdate && x.CreationDate <= enddate).Count();
            }
        }

        public List<tblSR_Log> GetSRLogsListByFilter(string keyword, string sorting1, string sorting2, string fromdate, string todate, int UserId, int startIndex, int count, string sorting)
        {
            IEnumerable<tblSR_Log> query = db.tblSR_Logs;
            if (fromdate != "" && todate != "")
            {
                //DateTime date1 = Convert.ToDateTime(fromdate);
                //DateTime date2 = Convert.ToDateTime(todate);

                DateTime date1 = Convert.ToDateTime(fromdate);
                DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                DateTime date2 = Convert.ToDateTime(todate);
                DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);


                query = db.tblSR_Logs.Where(x =>
                       (SqlFunctions.PatIndex("%" + keyword + "%", SqlFunctions.StringConvert(x.SRNumber)) > 0
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
                        || SqlFunctions.PatIndex("%" + keyword + "%", x.Bonding) > 0)
                        && (x.CreationDate >= startdate && x.CreationDate <= enddate)
                   );//.OrderByDescending(x => x.SRNumber);
            }
            else
            {
                query = db.tblSR_Logs.Where(x =>
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
                   );//.OrderByDescending(x => x.SRNumber);


                //query = db.tblSR_Logs.Where(x =>
                //       SqlFunctions.StringConvert(x.SRNumber).Contains(keyword)
                //       || x.Customer.Contains(keyword)
                //       || x.CustomerContact.Contains(keyword)
                //       || x.ProjectDescription.Contains(keyword)
                //       || x.ContactEmail.Contains(keyword)
                //       || x.ContactPhone.Contains(keyword)
                //       || x.WhoJobWalk.Contains(keyword)
                //       || x.Estimator.Contains(keyword)
                //       || x.Notes.Contains(keyword)
                //       || x.Owner.Contains(keyword)
                //       || x.JobsiteAddress.Contains(keyword)
                //       || x.Bonding.Contains(keyword)
                //  );//.OrderByDescending(x => x.SRNumber);

            }
            if (sorting1 != "" && sorting2 != "")
            {
                string sort = sorting1 + "," + sorting2;
                query = query.OrderBy(sort);
            }
            else if (sorting1 != "" && sorting2 == "")
            {
                query = query.OrderBy(sorting1);
            }
            else if (sorting1 == "" && sorting2 != "")
            {
                query = query.OrderBy(sorting2);
            }
            else
            {
                query = query.OrderByDescending(x => x.SRNumber);
            }

            return count > 0
                        ? query.Skip(startIndex).Take(count).ToList() //Paging
                        : query.ToList(); //No paging

        }

        public int GetSRLogcountByFilter(string keyword, string fromdate, string todate)
        {
            if (fromdate == "" && todate == "")
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
            else
            {
                //DateTime date1 = Convert.ToDateTime(fromdate);
                //DateTime date2 = Convert.ToDateTime(todate);

                DateTime date1 = Convert.ToDateTime(fromdate);
                DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                DateTime date2 = Convert.ToDateTime(todate);
                DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);

                return db.tblSR_Logs.Where(x =>
                       (SqlFunctions.PatIndex("%" + keyword + "%", SqlFunctions.StringConvert(x.SRNumber)) > 0
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
                       || SqlFunctions.PatIndex("%" + keyword + "%", x.Bonding) > 0)
                        && (x.CreationDate >= startdate && x.CreationDate <= enddate)
                  ).Count();
            }
        }

        public IEnumerable<tblSR_Log> GetSRLogsList(int UserId)
        {
            db = new SR_Log_DatabaseSQLEntities();
            IEnumerable<tblSR_Log> query = db.tblSR_Logs.Take(500);//.OrderByDescending(x =>  x.SRNumber);   
            // IEnumerable<tblSR_Log> query = db.tblSR_Logs;//.OrderByDescending(x =>  x.SRNumber);  
            return query;

        }
        public List<tblCustomer> GetCustomer()
        {
            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustomer> customer = (db.tblCustomers.Where(c => c.IsInActive == false)).OrderBy(c => c.CustomerName).ToList();
            return customer;
        }

        public List<tblCustContact> GetCustomerContact()
        {
            db = new SR_Log_DatabaseSQLEntities();
            List<tblCustContact> custcontact = db.tblCustContacts.ToList();
            return custcontact;
        }
        public List<tblGroupUser> GetGroupUserForEstimator()
        {
            db = new SR_Log_DatabaseSQLEntities();
            List<tblGroupUser> groupUsers = db.tblGroupUsers.ToList().OrderBy(x => x.UserName).ToList();
            return groupUsers;
        }
        public List<tblGroupUser> GetGroupUser()
        {
            db = new SR_Log_DatabaseSQLEntities();
            List<tblGroupUser> groupUsers = db.tblGroupUsers.ToList().OrderBy(x => x.UserName).ToList();
            return groupUsers;
        }
        public List<tblCustAddress> GetJobsiteAddress(int CustomerId)
        {
            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustAddress> jobsiteadd = (db.tblCustAddresses.Where(c => c.CustomerId == CustomerId)).ToList();
            return jobsiteadd;
        }
        public List<tblCustAddress> GetJobsiteAddressWithName(string CustomerName)
        {
            db = new SR_Log_DatabaseSQLEntities();
            var cust = (from c in db.tblCustomers
                        where c.CustomerName.Trim().ToUpper() == CustomerName.Trim().ToUpper()
                        select c).ToList();
            int CustomerId = 0;
            foreach (var i in cust)
            {
                CustomerId = i.CustomerId;
            }
            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustAddress> jobsiteadd = (db.tblCustAddresses.Where(c => c.CustomerId == CustomerId)).ToList();
            return jobsiteadd;
        }
        public List<tblCustContact> GetCustomerContact(int CustomerId)
        {
            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustContact> customercontact = (db.tblCustContacts.Where(c => c.CustomerId == CustomerId)).ToList();
            return customercontact;
        }


        public List<tblCustContact> GetCustomerContactWithName(string CustomerName)
        {
            db = new SR_Log_DatabaseSQLEntities();
            var cust = (from c in db.tblCustomers
                        where c.CustomerName.Trim().ToUpper() == CustomerName.Trim().ToUpper()
                        select c).ToList();
            int CustomerId = 0;
            if (cust.Count > 0)
            {
                CustomerId = cust[0].CustomerId;
            }

            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustContact> customercontact = (db.tblCustContacts.Where(c => c.CustomerId == CustomerId)).ToList();
            return customercontact;
        }

        public tblCustContact GetCustomerContactEmailWithName(string CustomerName, string ContactName)
        {
            db = new SR_Log_DatabaseSQLEntities();
            var cust = (from c in db.tblCustomers
                        where c.CustomerName.Trim().ToUpper() == CustomerName.Trim().ToUpper()
                        select c).ToList();
            int CustomerId = 0;
            if (cust.Count > 0)
            {
                CustomerId = cust[0].CustomerId;
            }

            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            tblCustContact customercontact = (db.tblCustContacts.Where(c => c.CustomerId == CustomerId && c.CustomerContact == ContactName)).FirstOrDefault();
            return customercontact;
        }

        public List<tblCustomer> GetCustomer(int CustomerId)
        {
            db = new SR_Log_DatabaseSQLEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerId == CustomerId)).ToList();
            return customer;
        }

        public int GetLastSrNumber()
        {
            db = new SR_Log_DatabaseSQLEntities();
            int LastSrNUmber = 0;
            var srlog = (from s in db.tblSR_Logs
                         orderby s.Id descending
                         select s).Take(1);
            foreach (var s in srlog)
            {
                LastSrNUmber = Convert.ToInt32(s.SRNumber) + 1;


            }
            return LastSrNUmber;
        }

        public double GetMaxSrNumber()
        {
            db = new SR_Log_DatabaseSQLEntities();
            double LastSrNUmber = 0;
            var srlog = (from s in db.tblSR_Logs
                         orderby s.Id descending
                         select s).Take(1);
            foreach (var s in srlog)
            {
                LastSrNUmber = Convert.ToDouble(s.SRNumber);


            }
            return LastSrNUmber;
        }


        public double GetFirstSrNumber()
        {
            db = new SR_Log_DatabaseSQLEntities();
            double FirstSrNUmber = 0;
            var srlog = (from s in db.tblSR_Logs
                         orderby s.Id
                         select s).Take(1);
            foreach (var s in srlog)
            {
                FirstSrNUmber = Convert.ToDouble(s.SRNumber);


            }
            return FirstSrNUmber;
        }


        public bool GetAddressDetails(string CustomerName, string jobsiteaddress)
        {
            string[] arraddress = jobsiteaddress.Split(',');
            string address = "";
            //if (arraddress.Count() == 3)
            //{
            //    address = arraddress[0];
            //}
            //else if (arraddress.Count() == 4)
            //{
            //    address = arraddress[1];
            //}
            //else if (arraddress.Count() == 0)
            //{
            //    address = arraddress[0];
            //}
            //else if (arraddress.Count() == 2)
            //{
            //    address = arraddress[0];
            //}
            //else
            //{
            //    address = jobsiteaddress;
            //}

            if (arraddress.Count() - 1 == 3)
            {
                address = arraddress[0];
            }
            else if (arraddress.Count() - 1 == 4)
            {
                address = arraddress[1];
            }
            else if (arraddress.Count() - 1 == 0)
            {
                address = arraddress[0];
            }
            else if (arraddress.Count() - 1 == 2)
            {
                address = arraddress[0];
            }
            else
            {
                address = jobsiteaddress;
            }

            db = new SR_Log_DatabaseSQLEntities();
            var customer = (from c in db.tblCustomers
                            where c.CustomerName == CustomerName
                            select c.CustomerId).ToList();
            int CustomerId = 0;
            if (customer.Count > 0)
            {
                CustomerId = customer[0];
                var srlog = (from s in db.tblCustAddresses
                             where s.CustomerId == CustomerId && s.Address1.ToUpper().Trim() == address.ToUpper().Trim()
                             select s).ToList();
                if (srlog.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }


        public string AddSRLog(SRLogViewModel model, string Flag)
        {
            string SRNumber = "";
            //using (SqlConnection conn = new SqlConnection("name=SR_Log_DatabaseSQLEntities"))
            //{


            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //    conn.Open();
            //    // Create an EntityCommand.

            //    string Joborquote = "";
            //    if (model.Job == true)
            //    { Joborquote = "1"; }
            //    else if (model.Quote == true)
            //    { Joborquote = "2"; }


            //    bool? bPW=null;
            //    if (model.PrevailingWageYes == true)
            //    {
            //        bPW = true;
            //    }
            //    else if (model.PrevailingWageNo == true)
            //    {
            //        bPW = false;
            //    }


            //    string ProjectType = "";
            //    if (model.ProjectTypeLumpType)
            //    {
            //        ProjectType = "1";
            //    }
            //    else if (model.ProjectTypeTAndM)
            //    {
            //        ProjectType = "2";
            //    }
            //    else if (model.ProjectTypeTAndMNTE)
            //    {
            //        ProjectType = "3";
            //    }



            //    bool bNewCustomer = false;
            //    if (model.NewCustomerYes == true)
            //    {
            //        bNewCustomer = true;
            //    }
            //    else
            //    {
            //        bNewCustomer = false;
            //    }


            //    string Customer = "NA";
            //    if (model.CustomerContact != null)
            //    {
            //        Customer = model.CustomerContact;
            //    }



            //    string Division = "";
            //    if (model.DivisionConcord == true)
            //    {
            //        Division = "Concord";
            //    }
            //    if (model.DivisionHanford == true)
            //    {
            //        Division = "Hanford";
            //    }
            //    if (model.DivisionSacramento == true)
            //    {
            //        Division = "Sacramento";
            //    }

            //    bool bActive = false;
            //    if (model.InactiveJob == true)
            //    {
            //        bActive = true;
            //    }
            //    else
            //    {
            //        bActive = false;
            //    }


            //    decimal? dcSR = 0;
            //    if (model.SRNumber != null)
            //    {
            //        dcSR = model.SRNumber;
            //    }

            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "SR_Log_DatabaseSQLEntities.USP_TT_InsertUpdateSRLog";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        SqlParameter param = new SqlParameter();

            //        param.Value = Joborquote;
            //        param.ParameterName = "JobOrQuote";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.PrevailingWageTBD;
            //        param.ParameterName = "PrevailingWageTBD";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();

            //        param.Value = bPW;

            //        param.ParameterName = "PW";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = ProjectType;
            //        param.ParameterName = "ProjectType";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = bNewCustomer;
            //        param.ParameterName = "NewCustomer";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "Billing";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.JobsiteAddress;
            //        param.ParameterName = "JobsiteAddress";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "QuoteTypeElectrical";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "QuoteTypeIC";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "QuoteTypeNoBid";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        //  param.Value = Convert.ToDateTime("1/1/1753").ToString("dd-MMM-yyyy");
            //        param.Value = Convert.ToDateTime("1/1/1753");
            //        param.ParameterName = "QuoteDate";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        //  param.Value = Convert.ToDateTime(System.DateTime.Now).ToString("hh:mm:ss");
            //        param.Value = Convert.ToDateTime(System.DateTime.Now);
            //        param.ParameterName = "QuoteTime";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = dcSR;
            //        param.ParameterName = "SRNumber";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.Customer;
            //        param.ParameterName = "Customer";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.ProjectDescription;
            //        param.ParameterName = "ProjectDescription";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.Customer;
            //        param.ParameterName = "CustomerContact";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.ContactPhone;
            //        param.ParameterName = "ContactPhone";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.ContactEmail;
            //        param.ParameterName = "ContactEmail";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.Estimator;
            //        param.ParameterName = "Estimator";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = Division;
            //        param.ParameterName = "Division";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        // param.Value = Convert.ToDateTime(model.CreationDate).ToString("dd-MMM-yyyy");
            //        param.Value = Convert.ToDateTime(System.DateTime.Now);
            //        param.ParameterName = "CreationDate";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        //dtpQuoteDueDate.MinDate not known
            //        // param.Value = Convert.ToDateTime(model.QuoteDate).ToString("dd-MMM-yyyy");
            //        param.Value = Convert.ToDateTime(model.QuoteDate);
            //        param.ParameterName = "QuoteDue";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        // param.Value = Convert.ToDateTime("1/1/1753").ToString("dd-MM-yyyy HH:mm");
            //        param.Value = Convert.ToDateTime("1/1/1753");
            //        param.ParameterName = "JobWalkDate";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "MandatoryJobWalk";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        // param.Value = model.Bonding;
            //        param.Value = "";
            //        param.ParameterName = "Bonding";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.ProjectManager;
            //        param.ParameterName = "ProjectManager";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = (model.Notes == null ? "" : model.Notes);
            //        param.ParameterName = "Notes";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.CreatedBy;
            //        param.ParameterName = "CreatedBy";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = 0;
            //        param.ParameterName = "FollowUp";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = bActive;
            //        param.ParameterName = "Active";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = "";
            //        param.ParameterName = "BidType";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.Owner;
            //        param.ParameterName = "Owner";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        //mindate not understand
            //        // param.Value = Convert.ToDateTime(model.AdvertiseDate).ToString("dd-MMM-yyyy");
            //        param.Value = Convert.ToDateTime(model.AdvertiseDate);
            //        param.ParameterName = "AdvertiseDate";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = model.NotifyPM;
            //        param.ParameterName = "NotifyPM";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.ServerJobFolder;
            //        param.ParameterName = "ServerJobFolder";
            //        cmd.Parameters.Add(param);

            //        param = new SqlParameter();
            //        param.Value = model.SiteForeman;
            //        param.ParameterName = "SiteForeman";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = Flag;
            //        param.ParameterName = "FlgAddEdit";
            //        cmd.Parameters.Add(param);


            //        param = new SqlParameter();
            //        param.Value = "";
            //        param.ParameterName = "result";
            //        cmd.Parameters.Add(param);



            //        // Execute the command.

            //        //using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
            //        //{

            //        //    // Read the results returned by the stored procedure.
            //        //    while (rdr.Read())
            //        //    {
            //        //        SRNumber = rdr[0].ToString();

            //        //        // Console.WriteLine("ID: {0} Grade: {1}", rdr["result"], rdr["Grade"]);
            //        //    }
            //        //    if (!rdr.IsClosed)
            //        //    {
            //        //        rdr.Close();
            //        //    }
            //        //}
            //    }
            //    conn.Close();
            //}


            string Joborquote = "";
            if (model.Job == true)
            { Joborquote = "1"; }
            else if (model.Quote == true)
            { Joborquote = "2"; }


            bool? bPW = null;
            if (model.PrevailingWageYes == true)
            {
                bPW = true;
            }
            else if (model.PrevailingWageNo == true)
            {
                bPW = false;
            }


            string ProjectType = "";
            if (model.ProjectTypeLumpType)
            {
                ProjectType = "1";
            }
            else if (model.ProjectTypeTAndM)
            {
                ProjectType = "2";
            }
            else if (model.ProjectTypeTAndMNTE)
            {
                ProjectType = "3";
            }



            bool bNewCustomer = false;
            if (model.NewCustomerYes == true)
            {
                bNewCustomer = true;
            }
            else
            {
                bNewCustomer = false;
            }


            string Customer = "NA";
            if (model.CustomerContact != null)
            {
                Customer = model.CustomerContact.ToUpper();
            }



            string Division = "";
            if (model.DivisionConcord == true)
            {
                Division = "Concord";
            }
            if (model.DivisionHanford == true)
            {
                Division = "Hanford";
            }
            if (model.DivisionSacramento == true)
            {
                Division = "Sacramento";
            }

            bool bActive = false;
            if (model.InactiveJob == true)
            {
                bActive = true;
            }
            else
            {
                bActive = false;
            }


            decimal? dcSR = 0;
            if (model.SRNumber != null)
            {
                dcSR = model.SRNumber;
            }

            DateTime AdvertiseDate = Convert.ToDateTime("1/1/1753");
            if (model.AdvertiseDate != null)
            {
                AdvertiseDate = Convert.ToDateTime(model.AdvertiseDate);
            }

            DateTime QuoteDate = Convert.ToDateTime("1/1/1753");
            if (model.QuoteDate != null)
            {
                QuoteDate = Convert.ToDateTime(model.QuoteDate);
            }

            //        param = new SqlParameter();
            //        //mindate not understand
            //        // param.Value = Convert.ToDateTime(model.AdvertiseDate).ToString("dd-MMM-yyyy");
            //        param.Value = Convert.ToDateTime(model.AdvertiseDate);
            //        param.ParameterName = "AdvertiseDate";
            //        cmd.Parameters.Add(param);




            //int CustomerId = Convert.ToInt32(model.Customer);
            //List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerId == CustomerId)).ToList();


            //string strcustomer="";
            //foreach (var c in customer)
            //{
            //    strcustomer = c.CustomerName;
            //}

            CommonFunctions c = new CommonFunctions();
            DateTime currentdate = c.GetCurrentDate();
            if (Flag == "E")
            {
                if (model.CreationDate != null)
                {
                    currentdate = Convert.ToDateTime(model.CreationDate);
                }
            }

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                ObjectParameter srnumber = new ObjectParameter("result", typeof(string));

                var sr = context.USP_TT_InsertUpdateSRLog(Joborquote, model.PrevailingWageTBD, bPW, ProjectType, bNewCustomer, false, model.JobsiteAddress, false, false, false, Convert.ToDateTime("1/1/1753"), currentdate,
                   dcSR, model.Customer, model.ProjectDescription, model.CustomerContact, model.ContactPhone, model.ContactEmail, model.Estimator, Division,
                 model.CreationDate, QuoteDate, Convert.ToDateTime("1/1/1753"), false,
                   "", model.ProjectManager, model.Notes, model.CreatedBy, false, bActive, "", model.Owner, AdvertiseDate, model.NotifyPM,
                    model.ServerJobFolder, model.SiteForeman, model.EditedBy, model.EditedDate, Flag, srnumber).ToList();
                SRNumber = sr[0].ToString();
                // SRNumber = Convert.ToString(InsertedId.Value);
            }



            return SRNumber;

        }

        public string AddQuoteLog(SRLogViewModel model, string Flag)
        {
            string result = "";

            string Division = "";
            if (model.DivisionConcord == true)
            {
                Division = "Concord";
            }
            if (model.DivisionHanford == true)
            {
                Division = "Hanford";
            }
            if (model.DivisionSacramento == true)
            {
                Division = "Sacramento";
            }
            DateTime QuoteDate = Convert.ToDateTime("1/1/1753");
            if (model.QuoteDate != null)
            {
                QuoteDate = Convert.ToDateTime(model.QuoteDate);
            }

            CommonFunctions c = new CommonFunctions();


            DateTime currentdate = c.GetCurrentDate();

            using (var context = new SR_Log_DatabaseSQLEntities())
            {

                ObjectParameter returnresult = new ObjectParameter("result", typeof(string));
                var quote = context.USP_TT_InsertUpdateQuote(currentdate, "", model.Customer, model.ProjectDescription, 1, model.Estimator,
                     Convert.ToInt32(model.SRNumber), Convert.ToInt32(model.SRNumber), model.Notes, "0", Division, false, Convert.ToDateTime("1/1/1753"), QuoteDate, "",
                     QuoteDate, "", "", "", Flag, "F", returnresult).ToList();
                result = quote[0].ToString();

            }



            return result;

        }

        public string AddQuoteLogFromSRGenetated(SRLogViewModel model, string Flag, short? lastAddendumRecvd, int UId, string EngineersEstimate, DateTime? QADeadLineDateTime)
        {
            string result = "";

            string Division = "";
            if (model.DivisionConcord == true)
            {
                Division = "Concord";
            }
            if (model.DivisionHanford == true)
            {
                Division = "Hanford";
            }
            if (model.DivisionSacramento == true)
            {
                Division = "Sacramento";
            }
            DateTime QuoteDate = Convert.ToDateTime("1/1/1753");
            if (model.QuoteDate != null)
            {
                QuoteDate = Convert.ToDateTime(model.QuoteDate);
            }



            using (var context = new SR_Log_DatabaseSQLEntities())
            {

                ObjectParameter returnresult = new ObjectParameter("result", typeof(string));
                var quote = context.USP_TT_InsertUpdateQuote(QADeadLineDateTime, model.QuoteType, model.Customer, model.ProjectDescription, lastAddendumRecvd, model.Estimator,
                     Convert.ToInt32(model.SRNumber), UId, model.Notes, EngineersEstimate, Division, model.MandatoryJobWalk, model.JobWalkDate, QADeadLineDateTime, "",
                    QADeadLineDateTime, "", "", "", Flag, "T", returnresult).ToList();
                result = quote[0].ToString();
            }



            return result;

        }


        public string AddSRLogFromSRGenerated(SRLogViewModel model, string Flag, string BiddingAs)
        {
            string SRNumber = "";




            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                ObjectParameter srnumber = new ObjectParameter("result", typeof(string));

                var sr = context.USP_TT_InsertUpdateSRLog(model.JobOrQuote, model.PrevailingWageTBD, model.PW, model.ProjectType, model.NewCustomer, model.Billing, model.JobsiteAddress, model.QuoteTypeI_C, model.QuoteTypeElectrical, model.QuoteTypeNoBid, model.QuoteDate, model.QuoteTime,
                    model.SRNumber, model.Customer, model.ProjectDescription, model.CustomerContact, model.ContactPhone, model.ContactEmail, model.Estimator, model.Division,
                 model.CreationDate, model.QuoteDue, model.JobWalkDate, model.MandatoryJobWalk,
                   model.Bonding, model.ProjectManager, model.Notes, model.CreatedBy, model.FollowUp, model.InactiveJob, BiddingAs, model.Owner, model.AdvertiseDate, model.NotifyPM,
                     model.ServerJobFolder, model.SiteForeman, model.EditedBy, model.EditedDate, Flag, srnumber).ToList();
                SRNumber = sr[0].ToString();
                // SRNumber = Convert.ToString(InsertedId.Value);
            }
            return SRNumber;

        }

        public SRLogViewModel GetSrLogRecords(string Flag, decimal SRNumber)
        {
            SRLogViewModel vwSR = new SRLogViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {
                var srlog = (from s in db.tblSR_Logs
                             where s.SRNumber == SRNumber
                             select s).FirstOrDefault();
                int Id = srlog.Id;

                if (Flag == "F")
                {
                    srlog = (from s in db.tblSR_Logs
                             select s).OrderBy(x => x.Id).FirstOrDefault();
                }
                else if (Flag == "L")
                {
                    srlog = (from s in db.tblSR_Logs
                             select s).OrderByDescending(x => x.Id).FirstOrDefault();
                }
                else if (Flag == "N")
                {
                    Id = Id + 1;
                    srlog = (from s in db.tblSR_Logs
                             where s.Id == Id
                             select s).FirstOrDefault();
                }
                else if (Flag == "P")
                {
                    Id = Id - 1;
                    srlog = (from s in db.tblSR_Logs
                             where s.Id == Id
                             select s).FirstOrDefault();
                }


                string Customer = Convert.ToString(srlog.Customer);
                //List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerName == Customer)).ToList();


                //int ncustomer = 0;
                //foreach (var c in customer)
                //{
                //    ncustomer = c.CustomerId;
                //}

                // vwSR.Customer =Convert.ToString( ncustomer);
                vwSR.Customer = Convert.ToString(Customer);

                if (srlog.JobOrQuote == "1")
                {
                    vwSR.Quote = false;
                    vwSR.Job = true;
                }
                else
                {
                    vwSR.Quote = true;
                    vwSR.Job = false;
                }
                if (srlog.NewCustomer == true)
                {
                    vwSR.NewCustomerYes = true;
                    vwSR.NewCustomerNo = false;
                }
                else
                {
                    vwSR.NewCustomerYes = false;
                    vwSR.NewCustomerNo = true;
                }


                if (srlog.ProjectType == "1")
                {
                    vwSR.ProjectTypeLumpType = true;
                }
                else if (srlog.ProjectType == "2")
                {
                    vwSR.ProjectTypeTAndM = true;
                }
                else if (srlog.ProjectType == "3")
                {
                    vwSR.ProjectTypeTAndMNTE = true;
                }



                if (srlog.Division == "Concord")
                {
                    vwSR.DivisionConcord = true;
                }
                if (srlog.Division == "Hanford")
                {
                    vwSR.DivisionHanford = true;

                }
                if (srlog.Division == "Sacramento")
                {
                    vwSR.DivisionSacramento = true;
                }

                vwSR.FileFolder = srlog.FileFolder;
                vwSR.ChemFeed = srlog.ChemFeed;
                if (srlog.InactiveJob == true)
                {
                    vwSR.InactiveJob = true;
                }
                else
                {
                    vwSR.InactiveJob = false;
                }


                if (srlog.PW == true)
                {
                    vwSR.PrevailingWageYes = true;
                    vwSR.PrevailingWageNo = false;
                }
                else
                {
                    vwSR.PrevailingWageYes = false;
                    vwSR.PrevailingWageNo = true;
                }

                vwSR.NotQuoted = srlog.NotQuoted;
                vwSR.Closed = srlog.Closed;
                vwSR.SRNumber = srlog.SRNumber;
                //vwSR.Customer = i.Customer;
                vwSR.ProjectDescription = srlog.ProjectDescription;
                vwSR.CustomerContact = srlog.CustomerContact;
                vwSR.ContactPhone = srlog.ContactPhone;
                vwSR.ContactEmail = srlog.ContactEmail;
                vwSR.Estimator = srlog.Estimator;
                vwSR.CreationDate = srlog.CreationDate;
                vwSR.QuoteDue = srlog.QuoteDue;
                vwSR.JobWalkDate = srlog.JobWalkDate;
                vwSR.MandatoryJobWalk = srlog.MandatoryJobWalk;
                vwSR.WhoJobWalk = srlog.WhoJobWalk;
                vwSR.BidAsPrimeOrSub = srlog.BidAsPrimeOrSub;
                vwSR.Bonding = srlog.Bonding;
                vwSR.BondingMailSent = srlog.BondingMailSent;
                vwSR.PrevailingMailSent = srlog.PrevailingMailSent;
                vwSR.Notes = srlog.Notes;
                vwSR.ProjectManager = srlog.ProjectManager;
                vwSR.CreatedBy = srlog.CreatedBy;
                vwSR.JobOrQuote = srlog.JobOrQuote;
                if (srlog.PrevailingWageTBD == true)
                {
                    vwSR.PrevailingWageTBD = true;
                }
                else
                {
                    vwSR.PrevailingWageTBD = false;
                }
                vwSR.JobsiteAddress = srlog.JobsiteAddress;
                vwSR.Billing = srlog.Billing;
                vwSR.QuoteDate = srlog.QuoteDate;
                vwSR.QuoteTime = srlog.QuoteTime;
                vwSR.QuoteTypeI_C = srlog.QuoteTypeI_C;
                vwSR.QuoteTypeElectrical = srlog.QuoteTypeElectrical;
                vwSR.QuoteTypeNoBid = srlog.QuoteTypeNoBid;
                vwSR.FollowUp = srlog.FollowUp;
                vwSR.QuoteType = srlog.QuoteType;
                vwSR.Owner = srlog.Owner;
                vwSR.AdvertiseDate = srlog.AdvertiseDate;
                if (srlog.NotifyPM == true)
                {
                    vwSR.NotifyPM = true;
                }
                else
                {
                    vwSR.NotifyPM = false;
                }
                vwSR.ServerJobFolder = srlog.ServerJobFolder;
                vwSR.SiteForeman = srlog.SiteForeman;
                vwSR.Id = srlog.Id;



            }
            return vwSR;
        }



        public SRLogViewModel GetSrRecords(string Flag, decimal SRNumber)
        {
            SRLogViewModel vwSR = new SRLogViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                bool ExistRecord = false;
                var srList = db.USP_TT_GetNextPrevLastFirst(SRNumber, Flag);

                foreach (var i in srList)
                {
                    ExistRecord = true;
                    string Customer = Convert.ToString(i.Customer);
                    //List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerName == Customer)).ToList();


                    //int ncustomer = 0;
                    //foreach (var c in customer)
                    //{
                    //    ncustomer = c.CustomerId;
                    //}

                    // vwSR.Customer =Convert.ToString( ncustomer);
                    vwSR.Customer = Convert.ToString(Customer);

                    if (i.JobOrQuote == "1")
                    {
                        vwSR.Quote = false;
                        vwSR.Job = true;
                    }
                    else
                    {
                        vwSR.Quote = true;
                        vwSR.Job = false;
                    }
                    if (i.NewCustomer == true)
                    {
                        vwSR.NewCustomerYes = true;
                        vwSR.NewCustomerNo = false;
                    }
                    else
                    {
                        vwSR.NewCustomerYes = false;
                        vwSR.NewCustomerNo = true;
                    }



                    if (i.ProjectType == "1")
                    {
                        vwSR.ProjectTypeLumpType = true;
                    }
                    else if (i.ProjectType == "2")
                    {
                        vwSR.ProjectTypeTAndM = true;
                    }
                    else if (i.ProjectType == "3")
                    {
                        vwSR.ProjectTypeTAndMNTE = true;
                    }



                    if (i.Division == "Concord")
                    {
                        vwSR.DivisionConcord = true;
                    }
                    if (i.Division == "Hanford")
                    {
                        vwSR.DivisionHanford = true;

                    }
                    if (i.Division == "Sacramento")
                    {
                        vwSR.DivisionSacramento = true;
                    }

                    vwSR.FileFolder = i.FileFolder;
                    vwSR.ChemFeed = i.ChemFeed;
                    if (i.InactiveJob == true)
                    {
                        vwSR.InactiveJob = true;
                    }
                    else
                    {
                        vwSR.InactiveJob = false;
                    }


                    if (i.PW == true)
                    {
                        vwSR.PrevailingWageYes = true;
                        vwSR.PrevailingWageNo = false;
                    }
                    else
                    {
                        vwSR.PrevailingWageYes = false;
                        vwSR.PrevailingWageNo = true;
                    }

                    vwSR.NotQuoted = i.NotQuoted;
                    vwSR.Closed = i.Closed;
                    vwSR.SRNumber = i.SRNumber;
                    //vwSR.Customer = i.Customer;
                    vwSR.ProjectDescription = i.ProjectDescription;
                    vwSR.CustomerContact = i.CustomerContact;
                    vwSR.ContactPhone = i.ContactPhone;
                    vwSR.ContactEmail = i.ContactEmail;
                    vwSR.Estimator = i.Estimator;

                    vwSR.CreationDate = i.CreationDate;
                    vwSR.EditedDate = i.EditedDate;

                    vwSR.QuoteDue = i.QuoteDue;
                    vwSR.JobWalkDate = i.JobWalkDate;
                    vwSR.MandatoryJobWalk = i.MandatoryJobWalk;
                    vwSR.WhoJobWalk = i.WhoJobWalk;
                    vwSR.BidAsPrimeOrSub = i.BidAsPrimeOrSub;
                    vwSR.Bonding = i.Bonding;
                    vwSR.BondingMailSent = i.BondingMailSent;
                    vwSR.PrevailingMailSent = i.PrevailingMailSent;
                    vwSR.Notes = i.Notes;
                    vwSR.ProjectManager = i.ProjectManager;

                    vwSR.CreatedBy = i.CreatedBy;
                    vwSR.EditedBy = i.EditedBy;

                    vwSR.JobOrQuote = i.JobOrQuote;
                    if (i.PrevailingWageTBD == true)
                    {
                        vwSR.PrevailingWageTBD = true;
                    }
                    else
                    {
                        vwSR.PrevailingWageTBD = false;
                    }
                    vwSR.JobsiteAddress = i.JobsiteAddress;
                    vwSR.Billing = i.Billing;
                    vwSR.QuoteDate = i.QuoteDate;
                    vwSR.QuoteTime = i.QuoteTime;
                    vwSR.QuoteTypeI_C = i.QuoteTypeI_C;
                    vwSR.QuoteTypeElectrical = i.QuoteTypeElectrical;
                    vwSR.QuoteTypeNoBid = i.QuoteTypeNoBid;
                    vwSR.FollowUp = i.FollowUp;
                    vwSR.QuoteType = i.QuoteType;
                    vwSR.Owner = i.Owner;
                    vwSR.AdvertiseDate = i.AdvertiseDate;
                    if (i.NotifyPM == true)
                    {
                        vwSR.NotifyPM = true;
                    }
                    else
                    {
                        vwSR.NotifyPM = false;
                    }
                    vwSR.ServerJobFolder = i.ServerJobFolder;
                    vwSR.SiteForeman = i.SiteForeman;
                    vwSR.Id = i.Id;
                }
                if (ExistRecord == false)
                {
                    if (Flag == "N")
                    {
                        var NextRecord = (from n in db.tblSR_Logs
                                          where n.SRNumber > SRNumber
                                          select n).OrderBy(x => x.SRNumber).Take(1);

                        foreach (var i in NextRecord)
                        {

                            string Customer = Convert.ToString(i.Customer);
                            //List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerName == Customer)).ToList();


                            //int ncustomer = 0;
                            //foreach (var c in customer)
                            //{
                            //    ncustomer = c.CustomerId;
                            //}

                            // vwSR.Customer =Convert.ToString( ncustomer);
                            vwSR.Customer = Convert.ToString(Customer);

                            if (i.JobOrQuote == "1")
                            {
                                vwSR.Quote = false;
                                vwSR.Job = true;
                            }
                            else
                            {
                                vwSR.Quote = true;
                                vwSR.Job = false;
                            }
                            if (i.NewCustomer == true)
                            {
                                vwSR.NewCustomerYes = true;
                                vwSR.NewCustomerNo = false;
                            }
                            else
                            {
                                vwSR.NewCustomerYes = false;
                                vwSR.NewCustomerNo = true;
                            }


                            if (i.ProjectType == "1")
                            {
                                vwSR.ProjectTypeLumpType = true;
                            }
                            else if (i.ProjectType == "2")
                            {
                                vwSR.ProjectTypeTAndM = true;
                            }
                            else if (i.ProjectType == "3")
                            {
                                vwSR.ProjectTypeTAndMNTE = true;
                            }



                            if (i.Division == "Concord")
                            {
                                vwSR.DivisionConcord = true;
                            }
                            if (i.Division == "Hanford")
                            {
                                vwSR.DivisionHanford = true;

                            }
                            if (i.Division == "Sacramento")
                            {
                                vwSR.DivisionSacramento = true;
                            }

                            vwSR.FileFolder = i.FileFolder;
                            vwSR.ChemFeed = i.ChemFeed;
                            if (i.InactiveJob == true)
                            {
                                vwSR.InactiveJob = true;
                            }
                            else
                            {
                                vwSR.InactiveJob = false;
                            }


                            if (i.PW == true)
                            {
                                vwSR.PrevailingWageYes = true;
                                vwSR.PrevailingWageNo = false;
                            }
                            else
                            {
                                vwSR.PrevailingWageYes = false;
                                vwSR.PrevailingWageNo = true;
                            }

                            vwSR.NotQuoted = i.NotQuoted;
                            vwSR.Closed = i.Closed;
                            vwSR.SRNumber = i.SRNumber;
                            //vwSR.Customer = i.Customer;
                            vwSR.ProjectDescription = i.ProjectDescription;
                            vwSR.CustomerContact = i.CustomerContact;
                            vwSR.ContactPhone = i.ContactPhone;
                            vwSR.ContactEmail = i.ContactEmail;
                            vwSR.Estimator = i.Estimator;
                            vwSR.CreationDate = i.CreationDate;
                            vwSR.QuoteDue = i.QuoteDue;
                            vwSR.JobWalkDate = i.JobWalkDate;
                            vwSR.MandatoryJobWalk = i.MandatoryJobWalk;
                            vwSR.WhoJobWalk = i.WhoJobWalk;
                            vwSR.BidAsPrimeOrSub = i.BidAsPrimeOrSub;
                            vwSR.Bonding = i.Bonding;
                            vwSR.BondingMailSent = i.BondingMailSent;
                            vwSR.PrevailingMailSent = i.PrevailingMailSent;
                            vwSR.Notes = i.Notes;
                            vwSR.ProjectManager = i.ProjectManager;
                            vwSR.CreatedBy = i.CreatedBy;
                            vwSR.JobOrQuote = i.JobOrQuote;
                            if (i.PrevailingWageTBD == true)
                            {
                                vwSR.PrevailingWageTBD = true;
                            }
                            else
                            {
                                vwSR.PrevailingWageTBD = false;
                            }
                            vwSR.JobsiteAddress = i.JobsiteAddress;
                            vwSR.Billing = i.Billing;
                            vwSR.QuoteDate = i.QuoteDate;
                            vwSR.QuoteTime = i.QuoteTime;
                            vwSR.QuoteTypeI_C = i.QuoteTypeI_C;
                            vwSR.QuoteTypeElectrical = i.QuoteTypeElectrical;
                            vwSR.QuoteTypeNoBid = i.QuoteTypeNoBid;
                            vwSR.FollowUp = i.FollowUp;
                            vwSR.QuoteType = i.QuoteType;
                            vwSR.Owner = i.Owner;
                            vwSR.AdvertiseDate = i.AdvertiseDate;
                            if (i.NotifyPM == true)
                            {
                                vwSR.NotifyPM = true;
                            }
                            else
                            {
                                vwSR.NotifyPM = false;
                            }
                            vwSR.ServerJobFolder = i.ServerJobFolder;
                            vwSR.SiteForeman = i.SiteForeman;
                            vwSR.Id = i.Id;
                        }



                    }
                    else if (Flag == "P")
                    {
                        var PreviousRecord = (from n in db.tblSR_Logs
                                              where n.SRNumber < SRNumber
                                              select n).OrderByDescending(x => x.SRNumber).Take(1);

                        foreach (var i in PreviousRecord)
                        {

                            string Customer = Convert.ToString(i.Customer);
                            //List<tblCustomer> customer = (db.tblCustomers.Where(c => c.CustomerName == Customer)).ToList();


                            //int ncustomer = 0;
                            //foreach (var c in customer)
                            //{
                            //    ncustomer = c.CustomerId;
                            //}

                            // vwSR.Customer =Convert.ToString( ncustomer);
                            vwSR.Customer = Convert.ToString(Customer);

                            if (i.JobOrQuote == "1")
                            {
                                vwSR.Quote = false;
                                vwSR.Job = true;
                            }
                            else
                            {
                                vwSR.Quote = true;
                                vwSR.Job = false;
                            }
                            if (i.NewCustomer == true)
                            {
                                vwSR.NewCustomerYes = true;
                                vwSR.NewCustomerNo = false;
                            }
                            else
                            {
                                vwSR.NewCustomerYes = false;
                                vwSR.NewCustomerNo = true;
                            }


                            if (i.ProjectType == "1")
                            {
                                vwSR.ProjectTypeLumpType = true;
                            }
                            else if (i.ProjectType == "2")
                            {
                                vwSR.ProjectTypeTAndM = true;
                            }
                            else if (i.ProjectType == "3")
                            {
                                vwSR.ProjectTypeTAndMNTE = true;
                            }



                            if (i.Division == "Concord")
                            {
                                vwSR.DivisionConcord = true;
                            }
                            if (i.Division == "Hanford")
                            {
                                vwSR.DivisionHanford = true;

                            }
                            if (i.Division == "Sacramento")
                            {
                                vwSR.DivisionSacramento = true;
                            }

                            vwSR.FileFolder = i.FileFolder;
                            vwSR.ChemFeed = i.ChemFeed;
                            if (i.InactiveJob == true)
                            {
                                vwSR.InactiveJob = true;
                            }
                            else
                            {
                                vwSR.InactiveJob = false;
                            }


                            if (i.PW == true)
                            {
                                vwSR.PrevailingWageYes = true;
                                vwSR.PrevailingWageNo = false;
                            }
                            else
                            {
                                vwSR.PrevailingWageYes = false;
                                vwSR.PrevailingWageNo = true;
                            }

                            vwSR.NotQuoted = i.NotQuoted;
                            vwSR.Closed = i.Closed;
                            vwSR.SRNumber = i.SRNumber;
                            //vwSR.Customer = i.Customer;
                            vwSR.ProjectDescription = i.ProjectDescription;
                            vwSR.CustomerContact = i.CustomerContact;
                            vwSR.ContactPhone = i.ContactPhone;
                            vwSR.ContactEmail = i.ContactEmail;
                            vwSR.Estimator = i.Estimator;
                            vwSR.CreationDate = i.CreationDate;
                            vwSR.QuoteDue = i.QuoteDue;
                            vwSR.JobWalkDate = i.JobWalkDate;
                            vwSR.MandatoryJobWalk = i.MandatoryJobWalk;
                            vwSR.WhoJobWalk = i.WhoJobWalk;
                            vwSR.BidAsPrimeOrSub = i.BidAsPrimeOrSub;
                            vwSR.Bonding = i.Bonding;
                            vwSR.BondingMailSent = i.BondingMailSent;
                            vwSR.PrevailingMailSent = i.PrevailingMailSent;
                            vwSR.Notes = i.Notes;
                            vwSR.ProjectManager = i.ProjectManager;
                            vwSR.CreatedBy = i.CreatedBy;
                            vwSR.JobOrQuote = i.JobOrQuote;
                            if (i.PrevailingWageTBD == true)
                            {
                                vwSR.PrevailingWageTBD = true;
                            }
                            else
                            {
                                vwSR.PrevailingWageTBD = false;
                            }
                            vwSR.JobsiteAddress = i.JobsiteAddress;
                            vwSR.Billing = i.Billing;
                            vwSR.QuoteDate = i.QuoteDate;
                            vwSR.QuoteTime = i.QuoteTime;
                            vwSR.QuoteTypeI_C = i.QuoteTypeI_C;
                            vwSR.QuoteTypeElectrical = i.QuoteTypeElectrical;
                            vwSR.QuoteTypeNoBid = i.QuoteTypeNoBid;
                            vwSR.FollowUp = i.FollowUp;
                            vwSR.QuoteType = i.QuoteType;
                            vwSR.Owner = i.Owner;
                            vwSR.AdvertiseDate = i.AdvertiseDate;
                            if (i.NotifyPM == true)
                            {
                                vwSR.NotifyPM = true;
                            }
                            else
                            {
                                vwSR.NotifyPM = false;
                            }
                            vwSR.ServerJobFolder = i.ServerJobFolder;
                            vwSR.SiteForeman = i.SiteForeman;
                            vwSR.Id = i.Id;
                        }
                    }
                }

                return vwSR;
            }

        }

        public DataTable GetSRLogsReportDetails(string sorting1, string sorting2, string fromdate, string todate, string keyword)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

            DataTable dt = new DataTable();

            dt.Columns.Add("SRNumber");
            dt.Columns.Add("Customer");
            dt.Columns.Add("CustomerContact");
            dt.Columns.Add("ContactPhone");
            dt.Columns.Add("ContactEmail");
            dt.Columns.Add("ProjectManager");
            dt.Columns.Add("ProjectDescription");
            dt.Columns.Add("SiteForeman");
            dt.Columns.Add("Estimator");
            dt.Columns.Add("JobOrQuote");
            dt.Columns.Add("Division");
            dt.Columns.Add("Owner");

            dt.Columns.Add("Notes");
            dt.Columns.Add("NewCustomer");
            dt.Columns.Add("ProjectType");
            dt.Columns.Add("QuoteDue");
            dt.Columns.Add("PrevailingWageTBD");
            dt.Columns.Add("Bonding");
            dt.Columns.Add("PW");

            IEnumerable<tblSR_Log> query = db.tblSR_Logs;
            if (keyword != "")
            {

                if (fromdate != "" && todate != "")
                {
                    //DateTime date1 = Convert.ToDateTime(fromdate);
                    //DateTime date2 = Convert.ToDateTime(todate);
                    DateTime date1 = Convert.ToDateTime(fromdate);
                    DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                    DateTime date2 = Convert.ToDateTime(todate);
                    DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);


                    query = db.tblSR_Logs.Where(x =>
                           (SqlFunctions.PatIndex("%" + keyword + "%", SqlFunctions.StringConvert(x.SRNumber)) > 0
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
                            || SqlFunctions.PatIndex("%" + keyword + "%", x.Bonding) > 0)
                            && (x.CreationDate >= startdate && x.CreationDate <= enddate)
                       );//.OrderByDescending(x => x.SRNumber);
                }
                else
                {
                    query = db.tblSR_Logs.Where(x =>
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
                       );//.OrderByDescending(x => x.SRNumber);
                }
                if (sorting1 != "" && sorting2 != "")
                {
                    string sort = sorting1 + "," + sorting2;
                    query = query.OrderBy(sort);
                }
                else if (sorting1 != "" && sorting2 == "")
                {
                    query = query.OrderBy(sorting1);
                }
                else if (sorting1 == "" && sorting2 != "")
                {
                    query = query.OrderBy(sorting2);
                }
                else
                {
                    query = query.OrderByDescending(x => x.SRNumber);
                }


            }
            else
            {

                db = new SR_Log_DatabaseSQLEntities();



                if (fromdate != "" && todate != "")
                {
                    //DateTime date1 = Convert.ToDateTime(fromdate);
                    //DateTime date2 = Convert.ToDateTime(todate);

                    DateTime date1 = Convert.ToDateTime(fromdate);
                    DateTime startdate = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);

                    DateTime date2 = Convert.ToDateTime(todate);
                    DateTime enddate = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);

                    query = query.Where(x => x.CreationDate >= startdate && x.CreationDate <= enddate);
                }

                if (sorting1 != "" && sorting2 != "")
                {
                    string sort = sorting1 + "," + sorting2;
                    query = query.OrderBy(sort);
                }
                else if (sorting1 != "" && sorting2 == "")
                {

                    query = query.OrderBy(sorting1);
                }
                else if (sorting1 == "" && sorting2 != "")
                {
                    query = query.OrderBy(sorting2);
                }
                else
                {
                    query = query.OrderByDescending(x => x.SRNumber);
                }


            }

            foreach (var i in query)
            {
                DataRow dr = dt.NewRow();

                dr["SRNumber"] = i.SRNumber;
                dr["Customer"] = i.Customer;
                dr["CustomerContact"] = i.CustomerContact;
                dr["ContactPhone"] = i.ContactPhone;
                dr["ContactEmail"] = i.ContactEmail;
                dr["ProjectManager"] = i.ProjectManager;
                dr["ProjectDescription"] = i.ProjectDescription;
                dr["SiteForeman"] = i.SiteForeman;
                dr["Estimator"] = i.Estimator;
                dr["JobOrQuote"] = i.JobOrQuote;
                dr["Division"] = i.Division;
                dr["Owner"] = i.Owner;

                dr["Notes"] = i.Notes;
                dr["NewCustomer"] = i.NewCustomer;
                dr["ProjectType"] = i.ProjectType;
                dr["QuoteDue"] = i.QuoteDue;
                dr["PrevailingWageTBD"] = i.PrevailingWageTBD;
                dr["Bonding"] = i.Bonding;
                dr["PW"] = i.PW;
                dt.Rows.Add(dr);
            }

            //string srselect = sr.Substring(0, sr.Length - 1);
            //string[] SrNUmbers = srselect.Split(';');

            //for (int j = 0; j < SrNUmbers.Count(); j++)
            //{
            //    decimal nSR =Convert.ToDecimal( SrNUmbers[j]);
            //    var srlog = (from s in db.tblSR_Logs
            //                 where s.SRNumber == nSR
            //                 select s).ToList();

            //    foreach (var i in srlog)
            //    {
            //        DataRow dr = dt.NewRow();

            //        dr["SRNumber"] = i.SRNumber;
            //        dr["Customer"] = i.Customer;
            //        dr["CustomerContact"] = i.CustomerContact;
            //        dr["ContactPhone"] = i.ContactPhone;
            //        dr["ContactEmail"] = i.ContactEmail;
            //        dr["ProjectManager"] = i.ProjectManager;
            //        dr["ProjectDescription"] = i.ProjectDescription;
            //        dr["SiteForeman"] = i.SiteForeman;
            //        dr["Estimator"] = i.Estimator;
            //        dr["JobOrQuote"] = i.JobOrQuote;
            //        dr["Division"] = i.Division;
            //        dr["Owner"] = i.Owner;

            //        dr["Notes"] = i.Notes;
            //        dr["NewCustomer"] = i.NewCustomer;
            //        dr["ProjectType"] = i.ProjectType;
            //        dr["QuoteDue"] = i.QuoteDue;
            //        dr["PrevailingWageTBD"] = i.PrevailingWageTBD;
            //        dr["Bonding"] = i.Bonding;
            //        dr["PW"] = i.PW;
            //        dt.Rows.Add(dr);
            //    }

            //}




            return dt;

        }


        //public List<tblCustAddress> GetPM(int jobsiteaddressId, int CustomerId)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var jobadd = (db.tblCustAddresses.Where(c => c.Id == jobsiteaddressId)).ToList();
        //    string Address1="";
        //    string SiteName="";
        //    List<tblCustAddress> pm;
        //    foreach(var i in jobadd)
        //    {
        //        Address1=i.Address1;
        //        SiteName=Convert.ToString(i.SiteName);
        //    }
        //    if (SiteName != "")
        //    {
        //       pm = (from c in db.tblCustAddresses
        //             where c.CustomerId == CustomerId && c.Address1 == Address1 && c.SiteName == SiteName && c.ProjectManager != null && c.ProjectManager != ""
        //                                   select c).ToList();

        //    }
        //    else
        //    {
        //        pm = (from c in db.tblCustAddresses
        //                                   where c.CustomerId == CustomerId && c.Address1 == Address1 && c.ProjectManager != null && c.ProjectManager !=""
        //                                   select c).ToList();
        //    }


        //    return pm;
        //}
    }
}
