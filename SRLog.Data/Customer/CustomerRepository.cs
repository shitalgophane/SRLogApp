using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Entities;
using SRLog.Entities.Customer.ViewModels;
using System.Data.Objects;
using SRLog.Common;
namespace SRLog.Data.Customer
{
    public class CustomerRepository
    {

        SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

        public CustomerViewModel GetCustomerDetails(int Id)
        {
            db = new SR_Log_DatabaseSQLEntities();
            CustomerViewModel cust = new CustomerViewModel();
            if (Id != 0)
            {
                var customer = from c in db.tblCustomers
                               where c.CustomerId == Id
                               select c;
                foreach (var c in customer)
                {
                    cust.CustomerName = c.CustomerName;
                    cust.Notes = c.Notes;
                    if (c.IsInActive == true)
                    {
                        cust.InActive = true;
                    }
                    else
                    {
                        cust.InActive = false;
                    }
                    cust.CustomerId = c.CustomerId;

                }

                List<tblCustAddress> custAdd = db.tblCustAddresses.Where(x => x.CustomerId == Id).ToList<tblCustAddress>();

                cust.lstCustAddress = custAdd;

                List<tblCustContact> custContact = db.tblCustContacts.Where(x => x.CustomerId == Id).ToList<tblCustContact>();

                cust.lstCustContact = custContact;
            }

            return cust;

        }

        public int AddUpdateCustomerImport(CustomerViewModel model)
        {
            int customerId = 0;

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                ObjectParameter custId = new ObjectParameter("result", typeof(string));
                var customer = context.USP_TT_InsertUpdateCustomer(model.CustomerName.ToString().ToUpper(), model.Notes, model.InActive, custId).ToList();
                customerId = Convert.ToInt32(customer[0].ToString());
               
            }
            return customerId;
        }

        public int AddUpdateCustomer(CustomerViewModel model)
        {
            int customerId=0;
           
            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                //ObjectParameter custId = new ObjectParameter("result", typeof(string));
                //var customer = context.USP_TT_InsertUpdateCustomer(model.CustomerName.ToString().ToUpper(), model.Notes, model.InActive, custId).ToList();
                //customerId = Convert.ToInt32(customer[0].ToString());
                if (model.CustomerId != 0)
                {
                    tblCustomer result = (from c in context.tblCustomers
                                             where c.CustomerId == model.CustomerId
                                             select c).SingleOrDefault();

                    if (result != null)
                    {
                        result.CustomerName = model.CustomerName;
                        result.IsInActive = model.InActive;
                        result.Notes = model.Notes;                     
                        context.SaveChanges();
                        customerId = result.CustomerId;
                    }

                }
                else
                {
                    CommonFunctions com = new CommonFunctions();
                    tblCustomer cust = new tblCustomer();
                    cust.CustomerName = model.CustomerName;
                    cust.DateAdded = com.GetCurrentDate();
                    cust.IsInActive = model.InActive;
                    cust.Notes = model.Notes;
                    context.tblCustomers.Add(cust);
                    context.SaveChanges();
                    customerId = cust.CustomerId;
                }
            }
            return customerId;

        }

        public void AddUpdateCustomerContact(CustomerContactViewModel model, string Flag)
        {

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                if (Flag == "Add")
                {
                    if (model.IsPrimaryContact == true)
                    {
                        var ExistCust = (from c in context.tblCustContacts
                                         where c.CustomerId == model.CustomerId
                                         select c).ToList();
                        if (ExistCust.Count > 0)
                        {
                            foreach (var cust in ExistCust)
                            {
                                if (cust.IsPrimaryContact == true)
                                {
                                    tblCustContact result = (from c in context.tblCustContacts
                                                             where c.Id == cust.Id
                                                             select c).SingleOrDefault();
                                    if (result != null)
                                    {
                                        result.IsPrimaryContact = false;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    var customer = context.USP_TT_InsertCustContacts(Convert.ToInt32(model.CustomerId), model.CustomerContact, model.ContactPhone, model.ContactEmail, model.IsPrimaryContact);
                }
                else
                {
                    if (model.IsPrimaryContact == true)
                    {
                        var ExistCust = (from c in context.tblCustContacts
                                         where c.CustomerId == model.CustomerId
                                         select c).ToList();
                        if (ExistCust.Count > 0)
                        {
                            foreach (var cust in ExistCust)
                            {
                                if (cust.IsPrimaryContact == true)
                                {
                                    tblCustContact r = (from c in context.tblCustContacts
                                                             where c.Id == cust.Id && c.Id != model.Id
                                                             select c).SingleOrDefault();
                                    if (r != null)
                                    {
                                        r.IsPrimaryContact = false;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    tblCustContact result = (from c in context.tblCustContacts
                                             where c.Id == model.Id
                                             select c).SingleOrDefault();

                    result.CustomerContact = model.CustomerContact;
                    result.ContactPhone = model.ContactPhone;
                    result.ContactEmail = model.ContactEmail;
                    result.IsPrimaryContact = model.IsPrimaryContact;
                    context.SaveChanges();
                }

            }





        }

        public void AddUpdateCustomerAddress(CustomerAddressViewModel model, string Flag)
        {

            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                if (Flag == "Add")
                {
                    if (model.IsPrimaryAddress == true)
                    {
                        var ExistCust = (from c in context.tblCustAddresses
                                         where c.CustomerId == model.CustomerId
                                         select c).ToList();
                        if (ExistCust.Count > 0)
                        {
                            foreach (var cust in ExistCust)
                            {
                                if (cust.IsPrimaryAddress == true)
                                {
                                    tblCustAddress result = (from c in context.tblCustAddresses
                                                             where c.Id == cust.Id
                                                             select c).FirstOrDefault();
                                    if (result != null)
                                    {
                                        result.IsPrimaryAddress = false;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    var customer = context.USP_TT_InsertCustAddress(model.CustomerId, model.Address1, model.Address2, model.City, model.State, model.ZipCode, model.Country, model.SiteName, model.IsPrimaryAddress, model.ProjectManager);
                }
                else
                {
                    if (model.IsPrimaryAddress == true)
                    {
                        var ExistCust = (from c in context.tblCustAddresses
                                         where c.CustomerId == model.CustomerId
                                         select c).ToList();
                        if (ExistCust.Count > 0)
                        {
                            foreach (var cust in ExistCust)
                            {
                                if (cust.IsPrimaryAddress == true)
                                {
                                    tblCustAddress r = (from c in context.tblCustAddresses
                                                             where c.Id == cust.Id && c.Id !=model.Id
                                                             select c).SingleOrDefault();
                                    if (r != null)
                                    {
                                        r.IsPrimaryAddress = false;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    tblCustAddress result = (from c in context.tblCustAddresses
                                             where c.Id == model.Id
                                             select c).SingleOrDefault();

                    result.Address1 = model.Address1;
                    result.Address2 = model.Address2;
                    result.City = model.City;
                    result.State = model.State;
                    result.ZipCode = model.ZipCode;
                    result.Country = model.Country;
                    result.IsPrimaryAddress = model.IsPrimaryAddress;
                    result.SiteName = model.SiteName;
                    result.ProjectManager = model.ProjectManager;
                    context.SaveChanges();
                }

            }





        }


    }
}
