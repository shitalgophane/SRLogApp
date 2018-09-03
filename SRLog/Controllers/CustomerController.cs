using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Filters;
using SRLog.Data.Customer;
using SRLog.Entities;
using SRLog.Entities.Customer.ViewModels;
using SRLog.Data.SRLog;
using SRLog.Data.Activity;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SRLog.Common;
using SRLog.Data.Settings;

namespace SRLog.Controllers
{
    [AdminFilter]
    public class CustomerController : Controller
    {
        //
        // GET: /BidLog/
        CustomerRepository _repository = new CustomerRepository();

        [ExceptionHandler]
        public ActionResult Index(string id)
        {
            _repository = new CustomerRepository();

            if (id != null)
            {
                var cust = _repository.GetCustomerDetails(Convert.ToInt32(id));
                ViewBag.Adddisable = false;
                ViewBag.CustomerId = id;
                ViewBag.Deletedisable = false;
                return View(cust);
            }
            else
            {
                ViewBag.Adddisable = true;
                ViewBag.Deletedisable = true;
                CustomerViewModel cust = new CustomerViewModel();
                return View(cust);
            }
        }

        [ExceptionHandler]
        public ActionResult IndexWithCustomer(string id)
        {
            _repository = new CustomerRepository();

            if (id != null)
            {
                SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
                var customer = (from c in db.tblCustomers
                                where c.CustomerName == id
                                select c).ToList();
                if (customer.Count() <= 0)
                {
                    ViewBag.Adddisable = true;
                    ViewBag.Deletedisable = true;
                    CustomerViewModel cust = new CustomerViewModel();
                    cust.CustomerName = id;
                    return View("Index", cust);
                }
                else
                {
                    int customerid = customer[0].CustomerId;
                    var cust = _repository.GetCustomerDetails(Convert.ToInt32(customerid));
                    ViewBag.Adddisable = false;
                    ViewBag.CustomerId = customerid;
                    ViewBag.Deletedisable = false;
                    return View("Index", cust);
                }


            }
            else
            {
                ViewBag.Adddisable = true;
                ViewBag.Deletedisable = true;
                CustomerViewModel cust = new CustomerViewModel();
                return View("Index", cust);
            }


        }


        [ExceptionHandler]
        [HttpPost]
        public ActionResult Index(CustomerViewModel cust)
        {
            return View(cust);
        }



        [ExceptionHandler]
        [HttpPost]
        public ActionResult Save(CustomerViewModel cust)
        {
            ModelState.Remove("CustomerId");
            int CustomerId = 0;
            CustomerId = cust.CustomerId;
            if (ModelState.IsValid)
            {
                // string jobsite = Convert.ToString(form["hdnJobSiteAddress"]);
                string Customer = cust.CustomerName.ToUpper();
                cust.CustomerName = cust.CustomerName.ToUpper();
                if (string.IsNullOrEmpty(cust.Notes) == false)
                    cust.Notes = cust.Notes.ToUpper();

                SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();


                if (cust.CustomerId != 0)
                {
                    var editcust = db.tblCustomers.Where(x => x.CustomerName.Trim().ToUpper() == Customer.Trim().ToUpper() && x.CustomerId != cust.CustomerId).FirstOrDefault();
                    if (editcust != null)
                    {
                        cust.CustomerId = cust.CustomerId;
                        ViewBag.CustomerId = cust.CustomerId;
                        ViewBag.Message = "This Customer already present. Please add different name And Try Again.";
                        ViewBag.Adddisable = true;
                        ViewBag.Deletedisable = true;
                        var customerexist = _repository.GetCustomerDetails(Convert.ToInt32(CustomerId));
                        cust.lstCustAddress = customerexist.lstCustAddress;
                        cust.lstCustContact = customerexist.lstCustContact;
                        return View("Index", cust);
                    }
                }
                else
                {
                    var existcust = db.tblCustomers.Where(x => x.CustomerName.Trim().ToUpper() == Customer.Trim().ToUpper()).FirstOrDefault();
                    if (existcust != null)
                    {
                        cust.CustomerId = cust.CustomerId;
                        ViewBag.CustomerId = cust.CustomerId;
                        ViewBag.Message = "This Customer already present. Please add different name And Try Again.";
                        ViewBag.Adddisable = true;
                        ViewBag.Deletedisable = true;
                        var customerexist2 = _repository.GetCustomerDetails(Convert.ToInt32(CustomerId));
                        cust.lstCustAddress = customerexist2.lstCustAddress;
                        cust.lstCustContact = customerexist2.lstCustContact;
                        return View("Index", cust);
                    }

                }


                CustomerRepository objdata = new CustomerRepository();

                // CustomerId = objdata.AddUpdateCustomer(cust, "Add");
                CustomerId = objdata.AddUpdateCustomer(cust);
                // Call SaveActivityLog("frmAddCustomer", "Add Customer", "Customer " + UCase(txtCustomerName.Text) + " added by user " & UserInfo.UserName & ".")
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Add Customer", "Save", "Customer " + cust.CustomerName.ToUpper() + " added by user " + Convert.ToString(Session["User"]) + ".");

                if (cust.CustomerId == 0)
                {
                    SendCustomerMailSMTP(Convert.ToInt32(CustomerId), cust.CustomerName);
                }

                cust.CustomerId = CustomerId;
                ViewBag.CustomerId = CustomerId;
                ViewBag.Adddisable = false;
                ViewBag.Deletedisable = false;
                ViewBag.Message = "Customer details saved successfully.";

            }
            else
            {
                ViewBag.Adddisable = true;
            }

            var customer = _repository.GetCustomerDetails(Convert.ToInt32(CustomerId));

           

            return View("Index", customer);
        }

        [ExceptionHandler]
        public void SendCustomerMailSMTP(int CustomerId, string CustomerName)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            SettingsRepository setting = new SettingsRepository();
            string rcptCustInfoTo = "";

            string sbody = "";
            List<Customer_Info_Mail> lstcustmailto = setting.GetCustomerInfoMail();

            foreach (var i in lstcustmailto)
            {
                rcptCustInfoTo += i.CustomerInfoMail + ";";
            }

            sbody = "<html><head><meta http-equiv=Content-Type content='text/html; charset=us-ascii'><meta name=Generator content='Microsoft Word 12 (filtered medium)'></head>";
            sbody = sbody + "<P style='color:#1F497D';><body lang=EN-US link=blue vlink=purple><div class=Section1><p class=MsoNormal><span style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''><span style='color:#1F497D'>";
            sbody = sbody + "<b> The Following Customer is added in database by user : " + Convert.ToString(Session["User"]) + "</b><br><br>";
            List<tblCustAddress> custAddressDetails = db.tblCustAddresses.Where(x => x.CustomerId == CustomerId).ToList();
            sbody = sbody + "<table style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''>";
            sbody = sbody + "<tr><td><b>CustomerName<b></td><td>: </td><td>" + CustomerName + "</td></tr>";

            if (custAddressDetails != null)
            {
                if (custAddressDetails.Count > 0)
                {
                    for (int i = 0; i < custAddressDetails.Count(); i++)
                    {
                        sbody = sbody + "<tr><td><b>Jobsite Address<b></td><td>: </td><td>" + custAddressDetails[i].Address1 + "</td></tr>";
                    }
                }
            }

            sbody = sbody + "</table>";
            sbody = sbody + "<br />";
            sbody = sbody + "<b>This E-mail is automatically sent by SR Log Database.<b></body></html>";


            MailSend m = new MailSend();
            m.sendMail(rcptCustInfoTo, "SR Log - New Customer Created", sbody, "", true);

        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            var custcontact = (from c in db.tblCustContacts
                               where c.CustomerId == id
                               select c).ToList();
            foreach (var c in custcontact)
            {
                int ContactId = c.Id;
                tblCustContact contact = db.tblCustContacts.Where(x => x.Id == ContactId).FirstOrDefault();
                db.tblCustContacts.Remove(contact);
                db.SaveChanges();
            }

            var custaddress = (from c in db.tblCustAddresses
                               where c.CustomerId == id
                               select c).ToList();
            foreach (var a in custaddress)
            {
                int AddressId = a.Id;
                tblCustAddress address = db.tblCustAddresses.Where(x => x.Id == AddressId).FirstOrDefault();
                db.tblCustAddresses.Remove(address);
                db.SaveChanges();
            }


            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == id).FirstOrDefault();
            db.tblCustomers.Remove(customer);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);


        }

        #region Customer Address Details
        [ExceptionHandler]
        [HttpPost]
        public ActionResult CreateCustomerAddress(CustomerAddressViewModel custaddress)
        {


            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (custaddress.CustomerId != 0)
            {
                var existcustcontact = db.tblCustAddresses.Where(x => x.Address1.Trim().ToUpper() == custaddress.Address1.Trim().ToUpper() && x.CustomerId == custaddress.CustomerId && x.SiteName.ToUpper() == custaddress.SiteName).FirstOrDefault();
                if (existcustcontact != null)
                {
                    ViewBag.Message = "This address is already added for this site. Please choose a different site name.";

                    return Json(custaddress, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CustomerRepository objdata = new CustomerRepository();
                    if (string.IsNullOrEmpty(custaddress.Address1) == false)
                        custaddress.Address1 = custaddress.Address1.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.Address2) == false)
                        custaddress.Address2 = custaddress.Address2.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.City) == false)
                        custaddress.City = custaddress.City.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.Country) == false)
                        custaddress.Country = custaddress.Country.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.State) == false)
                        custaddress.State = custaddress.State.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.ZipCode) == false)
                        custaddress.ZipCode = custaddress.ZipCode.ToUpper();

                    if (string.IsNullOrEmpty(custaddress.SiteName) == false)
                        custaddress.SiteName = custaddress.SiteName.ToUpper();

                    objdata.AddUpdateCustomerAddress(custaddress, "Add");

                }

            }
            //CustomerRepository _repository = new CustomerRepository();
            //var cust = _repository.GetCustomerDetails(custaddress.CustomerId);

            tblCustAddress cust = new tblCustAddress();

            return Json(cust, JsonRequestBehavior.AllowGet);


        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult UpdateCustomerAddress(CustomerAddressViewModel custaddress)
        {

            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (custaddress.CustomerId != 0)
            {
                CustomerRepository objdata = new CustomerRepository();
                if (string.IsNullOrEmpty(custaddress.Address1) == false)
                    custaddress.Address1 = custaddress.Address1.ToUpper();

                if (string.IsNullOrEmpty(custaddress.Address2) == false)
                    custaddress.Address2 = custaddress.Address2.ToUpper();

                if (string.IsNullOrEmpty(custaddress.City) == false)
                    custaddress.City = custaddress.City.ToUpper();

                if (string.IsNullOrEmpty(custaddress.Country) == false)
                    custaddress.Country = custaddress.Country.ToUpper();

                if (string.IsNullOrEmpty(custaddress.State) == false)
                    custaddress.State = custaddress.State.ToUpper();

                if (string.IsNullOrEmpty(custaddress.ZipCode) == false)
                    custaddress.ZipCode = custaddress.ZipCode.ToUpper();

                if (string.IsNullOrEmpty(custaddress.SiteName) == false)
                    custaddress.SiteName = custaddress.SiteName.ToUpper();

                objdata.AddUpdateCustomerAddress(custaddress, "Update");
                tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == custaddress.CustomerId).FirstOrDefault();
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Edit address", "UpdateCustomerAddress", "Addresses for customer " + customer.CustomerName.ToUpper() + " edited by " + Convert.ToString(Session["User"]) + ".");
            }


            //CustomerRepository _repository = new CustomerRepository();
            //var cust = _repository.GetCustomerDetails(custaddress.CustomerId);

            tblCustAddress cust = new tblCustAddress();
            return Json(cust, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public JsonResult DeleteCustomerAddress(Int32 Id)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            tblCustAddress custaddress = db.tblCustAddresses.Where(x => x.Id == Id).FirstOrDefault();
            int customerId = custaddress.CustomerId;

            db.tblCustAddresses.Remove(custaddress);
            db.SaveChanges();

            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == customerId).FirstOrDefault();



            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete address", "DeleteCustomerAddress", "Address " + custaddress.Address1 + " of customer " + customer.CustomerName + " deleted by user " + Convert.ToString(Session["User"]) + ".");
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        [ExceptionHandler]
        [HttpGet]
        public PartialViewResult CreateCustomerAddress(int customerid)
        {
            CustomerAddressViewModel custaddress = new CustomerAddressViewModel();
            SRLogRepository objdata = new SRLogRepository();
            custaddress.CustomerId = customerid;
            ViewBag.CustomerId = customerid;
            ViewBag.CustomerAddressId = 0;
            custaddress.IsPrimaryAddress = false;
            custaddress.ProjectManagerList = objdata.GetGroupUser();
            return PartialView(custaddress);
        }

        [ExceptionHandler]
        [HttpGet]
        public PartialViewResult UpdateCustomerAddress(Int32 id)
        {
            SRLogRepository objdata = new SRLogRepository();
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            tblCustAddress custaddress = db.tblCustAddresses.Where(x => x.Id == id).FirstOrDefault();
            CustomerAddressViewModel address = new CustomerAddressViewModel();
            address.Address1 = custaddress.Address1;
            address.Address2 = custaddress.Address2;
            address.City = custaddress.City;
            address.State = custaddress.State;
            address.ZipCode = custaddress.ZipCode;
            address.Country = custaddress.Country;
            address.SiteName = custaddress.SiteName;
            if (custaddress.IsPrimaryAddress == true)
            {
                address.IsPrimaryAddress = true;
            }
            else
            {
                address.IsPrimaryAddress = false;
            }
            //address.IsPrimaryAddress = custaddress.IsPrimaryAddress;
            address.ProjectManagerList = objdata.GetGroupUser();
            address.ProjectManager = custaddress.ProjectManager;
            address.Id = custaddress.Id;
            address.CustomerId = custaddress.CustomerId;

            ViewBag.Adddisable = false;
            ViewBag.CustomerId = custaddress.CustomerId;
            ViewBag.CustomerAddressId = custaddress.Id;
            return PartialView("CreateCustomerAddress", address);
        }
        #endregion

        #region Customer Contact Details

        [ExceptionHandler]
        [HttpPost]
        public ActionResult CreateCustomerContact(CustomerContactViewModel custconact)
        {


            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (custconact.CustomerId != 0)
            {
                var existcustcontact = db.tblCustContacts.Where(x => x.CustomerContact.Trim().ToUpper() == custconact.CustomerContact.Trim().ToUpper() && x.CustomerId == custconact.CustomerId).FirstOrDefault();
                if (existcustcontact != null)
                {
                    ViewBag.Message = "This Contact already present. Please add different name And Try Again.";
                    return Json(custconact, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CustomerRepository objdata = new CustomerRepository();
                    if (string.IsNullOrEmpty(custconact.ContactEmail) == false)
                        custconact.ContactEmail = custconact.ContactEmail.ToUpper();

                    if (string.IsNullOrEmpty(custconact.ContactPhone) == false)
                        custconact.ContactPhone = custconact.ContactPhone.ToUpper();

                    if (string.IsNullOrEmpty(custconact.CustomerContact) == false)
                        custconact.CustomerContact = custconact.CustomerContact.ToUpper();


                    objdata.AddUpdateCustomerContact(custconact, "Add");

                }

            }

            //CustomerRepository _repository = new CustomerRepository();
            //var cust = _repository.GetCustomerDetails(custconact.CustomerId);
            tblCustContact cust = new tblCustContact();
            return Json(cust, JsonRequestBehavior.AllowGet);



            //db.Products.InsertOnSubmit(product);
            //db.SubmitChanges();
            //return Json(cust, JsonRequestBehavior.AllowGet);

            //CustomerRepository _repository = new CustomerRepository();
            //var cust = _repository.GetCustomerDetails(2);
            //// parts.Add(new Part() {PartName="crank arm", PartId=1234});
            //cust.lstCustContact.Add(new tblCustContact() { Id = 0, CustomerId = 0, CustomerContact = product.CustomerContact, ContactEmail = product.ContactEmail, IsPrimaryContact = product.IsPrimaryContact });
            //return View("Index", cust);

        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult UpdateCustomerContact(CustomerContactViewModel custconact)
        {

            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (custconact.CustomerId != 0)
            {
                CustomerRepository objdata = new CustomerRepository();
                if (string.IsNullOrEmpty(custconact.ContactEmail) == false)
                    custconact.ContactEmail = custconact.ContactEmail.ToUpper();

                if (string.IsNullOrEmpty(custconact.ContactPhone) == false)
                    custconact.ContactPhone = custconact.ContactPhone.ToUpper();

                if (string.IsNullOrEmpty(custconact.CustomerContact) == false)
                    custconact.CustomerContact = custconact.CustomerContact.ToUpper();

                objdata.AddUpdateCustomerContact(custconact, "Update");

                tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == custconact.CustomerId).FirstOrDefault();
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Edit Contact", "UpdateCustomerContact", "Contacts for customer " + customer.CustomerName.ToUpper() + " edited by " + Convert.ToString(Session["User"]) + ".");
                //Call SaveActivityLog("frmAddCustomer", "Edit Contact", "Contacts for customer " & UCase(txtCustomerName.Text) & " edited by " & UserInfo.UserName)
            }


            //CustomerRepository _repository = new CustomerRepository();
            //var cust = _repository.GetCustomerDetails(custconact.CustomerId);
            tblCustContact cust = new tblCustContact();
            return Json(cust, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public JsonResult DeleteCustomerContact(Int32 Id)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            tblCustContact custcontact = db.tblCustContacts.Where(x => x.Id == Id).FirstOrDefault();
            int CustomerId = custcontact.CustomerId;
            string contact = custcontact.CustomerContact;
            db.tblCustContacts.Remove(custcontact);
            db.SaveChanges();


            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == CustomerId).FirstOrDefault();

            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete contact", "DeleteCustomerContact", "Contact " + contact + " of customer " + customer.CustomerName + " deleted by user " + Convert.ToString(Session["User"]) + ".");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpGet]
        public PartialViewResult CreateCustomerContact(int customerid)
        {
            CustomerContactViewModel custcontact = new CustomerContactViewModel();
            custcontact.CustomerId = customerid;
            ViewBag.CustomerId = customerid;
            ViewBag.CustomerContactId = 0;
            return PartialView(custcontact);
        }

        [ExceptionHandler]
        [HttpGet]
        public PartialViewResult UpdateCustomerContact(Int32 id)
        {

            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            tblCustContact custcontact = db.tblCustContacts.Where(x => x.Id == id).FirstOrDefault();
            CustomerContactViewModel contact = new CustomerContactViewModel();
            contact.CustomerContact = custcontact.CustomerContact;
            contact.ContactEmail = custcontact.ContactEmail;
            contact.ContactPhone = custcontact.ContactPhone;
            contact.Id = custcontact.Id;
            contact.CustomerId = custcontact.CustomerId;

            if (custcontact.IsPrimaryContact == true)
            {
                contact.IsPrimaryContact = true;
            }
            else
            {
                contact.IsPrimaryContact = false;
            }
            ViewBag.Adddisable = false;
            ViewBag.CustomerId = custcontact.CustomerId;
            ViewBag.CustomerContactId = custcontact.Id;
            return PartialView("CreateCustomerContact", contact);
        }
        #endregion


        #region Manage Customer Address Details

        [ExceptionHandler]
        public ActionResult ManageCustomerAddress()
        {
            return View();
        }

        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetManageCustAddressData()
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            //var address = (from a in db.tblCustAddresses
            //               join c in db.tblCustomers on a.CustomerId equals c.CustomerId

            //               select new
            //               {
            //                   Id = a.Id,
            //                   CustomerId = a.CustomerId,
            //                   Customer = c.CustomerName,
            //                   Address1 = a.Address1,
            //                   Address2 = a.Address2,
            //                   City = a.City,
            //                   State = a.State,
            //                   ZipCode = a.ZipCode,
            //                   Country = a.Country,
            //                   IsPrimaryAddress = a.IsPrimaryAddress,
            //                   SiteName = a.SiteName,
            //                   ProjectManager = a.ProjectManager,
            //                   IsActive = c.IsInActive
            //               }).OrderBy(x => x.Customer).ToList();



            var address =
              from a in db.tblCustomers
              join c in db.tblCustAddresses on a.CustomerId equals c.CustomerId into ps
              from p in ps.DefaultIfEmpty()
              select new
              {
                  Id = (p == null ? 0 : p.Id),
                  CustomerId = a.CustomerId,
                  Customer = a.CustomerName,
                  Address1 = (p == null ? "" : p.Address1),
                  Address2 = (p == null ? "" : p.Address2),
                  City = (p == null ? "" : p.City),
                  State = (p == null ? "" : p.State),
                  ZipCode = (p == null ? "" : p.ZipCode),
                  Country = (p == null ? "" : p.Country),
                  IsPrimaryAddress = (p == null ? false : p.IsPrimaryAddress),
                  SiteName = (p == null ? "" : p.SiteName),
                  ProjectManager = (p == null ? "" : p.ProjectManager),
                  IsActive = a.IsInActive

              };


            var jsonData = new
            {
                data = address
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);




        }


        [ExceptionHandler]
        public JsonResult SetInActive(Int32 Id, Int32 customerId, bool setdefault)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            // if (setdefault == true)
            // {

            tblCustomer cust = db.tblCustomers.Where(x => x.CustomerId == customerId).FirstOrDefault();
            cust.IsInActive = setdefault;
            db.SaveChanges();
            //}
            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == customerId).FirstOrDefault();

            var act = new ActivityRepository();
            if (setdefault == true)
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update Customer Status", "SetInActive", "Customer " + customer.CustomerName + " status is set as inactive by user " + Convert.ToString(Session["User"]) + ".");
            }
            else
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update Customer Status", "SetInActive", "Customer " + customer.CustomerName + " status is set as active by user " + Convert.ToString(Session["User"]) + ".");
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public JsonResult SetPrimaryAddress(Int32 Id, Int32 customerId, bool setdefault)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (setdefault == true)
            {
                var cust = (from c in db.tblCustAddresses
                            where c.CustomerId == customerId && c.Id != Id
                            select c).ToList();

                foreach (var c in cust)
                {
                    tblCustAddress address = db.tblCustAddresses.Where(x => x.Id == c.Id).FirstOrDefault();
                    address.IsPrimaryAddress = false;
                    db.SaveChanges();

                }
            }
            tblCustAddress custaddress = db.tblCustAddresses.Where(x => x.Id == Id).FirstOrDefault();

            custaddress.IsPrimaryAddress = setdefault;
            db.SaveChanges();


            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == customerId).FirstOrDefault();

            var act = new ActivityRepository();
            if (setdefault == true)
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update default address", "SetPrimaryAddress", "Default address set to " + custaddress.Address1 + " for customer " + customer.CustomerName + " by user " + Convert.ToString(Session["User"]) + ".");
            }
            else
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update default address", "SetPrimaryContact", "Default contact " + custaddress.Address1 + " removed for customer " + customer.CustomerName + " by user " + Convert.ToString(Session["User"]) + ".");
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult ImportCustomerAddress(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/ImportCustomerAddress/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    DataTable dtToExport = new DataTable();
                    int cnt = 0;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            //string[] col = sr.ReadLine().Split(',');
                            string[] col = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                            DataRow dr = dtToExport.NewRow();
                            if (col.Length > 0)
                            {
                                for (int i = 0; i < col.Length; i++)
                                {
                                    if (cnt == 0)
                                    {
                                        DataColumn dc = new DataColumn();
                                        dc.ColumnName = col[i].ToString();
                                        dtToExport.Columns.Add(dc);
                                    }
                                    else
                                    {
                                        dr[i] = col[i].ToString();
                                    }
                                }

                                if (cnt > 0)
                                    dtToExport.Rows.Add(dr);
                                if (cnt == 0)
                                    cnt++;


                            }
                        }
                    }
                    string[] columnNames = dtToExport.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                    if (columnNames[0] != "Customer Name" && columnNames[1] != "Street 1" && columnNames[2] != "Street 2" && columnNames[3] != "City" && columnNames[4] != "State" && columnNames[5] != "ZIP Code" && columnNames[6] != "Country" && columnNames[7] != "Site Name")
                    {
                        ViewBag.Message = "CSV file not in correct format. Please select valid CSV file and try again!";
                        return View("ManageCustomerAddress");
                    }
                    CustomerRepository _respository = new CustomerRepository();
                    for (int i = 0; i < dtToExport.Rows.Count; i++)
                    {
                        CustomerViewModel cst = new CustomerViewModel();
                        //cst.CustomerName = dtToExport.Rows[i][0].ToString();
                        cst.CustomerName = dtToExport.Rows[i][0].ToString().Replace(@"""", "");
                        // string cust = cst.CustomerName.Replace("\"", "");
                        cst.InActive = false;
                        cst.Notes = "";
                        int CustomerId;
                        CustomerId = _respository.AddUpdateCustomerImport(cst);
                        // Call SaveActivityLog("frmImportCustomers", "Import Customer", "Customer " + cst.CustomerName + " added from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".")
                        var act = new ActivityRepository();
                        act.AddActivityLog(Convert.ToString(Session["User"]), "Import Customer", "ImportCustomer", "Customer " + cst.CustomerName + " added from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".");
                        if (dtToExport.Rows[i]["Street 1"].ToString() != "")
                        {
                            CustomerAddressViewModel cstAdd = new CustomerAddressViewModel();
                            cstAdd.CustomerId = CustomerId;
                            cstAdd.Address1 = dtToExport.Rows[i][1].ToString();
                            cstAdd.Address2 = dtToExport.Rows[i][2].ToString();
                            cstAdd.City = dtToExport.Rows[i][3].ToString();
                            cstAdd.State = dtToExport.Rows[i][4].ToString();
                            cstAdd.ZipCode = dtToExport.Rows[i][5].ToString();
                            cstAdd.Country = dtToExport.Rows[i][6].ToString();
                            cstAdd.SiteName = dtToExport.Rows[i][7].ToString();
                            _respository.AddUpdateCustomerAddress(cstAdd, "Add");
                            act = new ActivityRepository();
                            act.AddActivityLog(Convert.ToString(Session["User"]), "Import Customer Address", "ImportCustomer", "Address added for customer " + cst.CustomerName + " from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".");

                            //Call SaveActivityLog("frmImportCustomers", "Import Customer Address", "Address added for customer " & fileColumns(0) & " from csv file " & cmdDialog.FileTitle & " by user " & UserInfo.UserName & ".")
                        }

                    }

                }
                else
                {
                    ViewBag.Message = "Please select file to import";
                }

                return View("ManageCustomerAddress");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "CSV file not in correct format. Please select valid CSV file and try again!";
                return View("ManageCustomerAddress");
            }
        }


        #endregion

        #region Manage Customer Contacts
        [ExceptionHandler]
        public ActionResult ManageCustomerContacts()
        {
            return View();
        }

        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetManageCustContactsData()
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            //var contacts = (from a in db.tblCustContacts
            //                join c in db.tblCustomers on a.CustomerId equals c.CustomerId

            //                select new
            //                {
            //                    Id = a.Id,
            //                    CustomerId = a.CustomerId,
            //                    Customer = c.CustomerName,
            //                    CustomerContact = a.CustomerContact,
            //                    ContactPhone = a.ContactPhone,
            //                    ContactEmail = a.ContactEmail,
            //                    IsPrimaryContact = a.IsPrimaryContact,

            //                }).OrderBy(x => x.Customer).ToList();




            var contacts = (from a in db.tblCustomers
                            join c in db.tblCustContacts on a.CustomerId equals c.CustomerId into ps
                            from c in ps.DefaultIfEmpty()
                            select new
                            {
                                Id = (c == null ? 0 : c.Id),
                                CustomerId = a.CustomerId,
                                Customer = a.CustomerName,
                                CustomerContact = (c == null ? "" : c.CustomerContact),
                                ContactPhone = (c == null ? "" : c.ContactPhone),
                                ContactEmail = (c == null ? "" : c.ContactEmail),
                                IsPrimaryContact = (c == null ? false : c.IsPrimaryContact),

                            }).OrderBy(x => x.Customer).ToList();

            var jsonData = new
            {
                data = contacts
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);




        }

        [ExceptionHandler]
        public JsonResult SetPrimaryContact(Int32 Id, Int32 customerId, bool setdefault)
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            if (setdefault == true)
            {
                var cust = (from c in db.tblCustContacts
                            where c.CustomerId == customerId && c.Id != Id
                            select c).ToList();

                foreach (var c in cust)
                {
                    tblCustContact contact = db.tblCustContacts.Where(x => x.Id == c.Id).FirstOrDefault();
                    contact.IsPrimaryContact = false;
                    db.SaveChanges();

                }
            }

            tblCustContact custcontact = db.tblCustContacts.Where(x => x.Id == Id).FirstOrDefault();
            custcontact.IsPrimaryContact = setdefault;
            db.SaveChanges();


            tblCustomer customer = db.tblCustomers.Where(x => x.CustomerId == customerId).FirstOrDefault();

            var act = new ActivityRepository();
            if (setdefault == true)
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update default contact", "SetPrimaryContact", "Default contact set to " + custcontact.CustomerContact + " for customer " + customer.CustomerName + " by user " + Convert.ToString(Session["User"]) + ".");
            }
            else
            {
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update default contact", "SetPrimaryContact", "Default contact " + custcontact.CustomerContact + " removed for customer " + customer.CustomerName + " by user " + Convert.ToString(Session["User"]) + ".");
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult ImportCustomerContacts(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/ImportCustomerContacts/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    DataTable dtToExport = new DataTable();
                    int cnt = 0;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            //string[] col = sr.ReadLine().Split(',');
                            string[] col = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                            DataRow dr = dtToExport.NewRow();
                            if (col.Length > 0)
                            {
                                for (int i = 0; i < col.Length; i++)
                                {
                                    if (cnt == 0)
                                    {
                                        DataColumn dc = new DataColumn();
                                        dc.ColumnName = col[i].ToString();
                                        dtToExport.Columns.Add(dc);
                                    }
                                    else
                                    {
                                        dr[i] = col[i].ToString();
                                    }
                                }

                                if (cnt > 0)
                                    dtToExport.Rows.Add(dr);
                                if (cnt == 0)
                                    cnt++;


                            }
                        }
                    }


                    string[] columnNames = dtToExport.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                    if (columnNames[0] != "Customer Name" && columnNames[1] != "Customer Contact" && columnNames[2] != "Contact Phone" && columnNames[3] != "Contact Email")
                    {
                        ViewBag.Message = "CSV file not in correct format. Please select valid CSV file and try again!";
                        return View("ManageCustomerContacts");
                    }
                    CustomerRepository _respository = new CustomerRepository();
                    for (int i = 0; i < dtToExport.Rows.Count; i++)
                    {
                        CustomerViewModel cst = new CustomerViewModel();
                        cst.CustomerName = dtToExport.Rows[i][0].ToString().Replace(@"""", "");

                        cst.InActive = false;
                        cst.Notes = "";

                        int CustomerId;
                        CustomerId = _respository.AddUpdateCustomerImport(cst);
                        // Call SaveActivityLog("frmImportCustomers", "Import Customer", "Customer " + cst.CustomerName + " added from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".")
                        var act = new ActivityRepository();
                        act.AddActivityLog(Convert.ToString(Session["User"]), "Import Customer", "ImportCustomerContacts", "Customer " + cst.CustomerName + " added from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".");
                        if (dtToExport.Rows[i][1].ToString() != "" && dtToExport.Rows[i][2].ToString() != " && dtToExport.Rows[i][3].ToString() != ")
                        {
                            CustomerContactViewModel cstAdd = new CustomerContactViewModel();
                            cstAdd.CustomerId = CustomerId;
                            cstAdd.CustomerContact = dtToExport.Rows[i][1].ToString();
                            cstAdd.ContactPhone = dtToExport.Rows[i][2].ToString();
                            cstAdd.ContactEmail = dtToExport.Rows[i][3].ToString();
                            cstAdd.IsPrimaryContact = false;
                            // cstAdd.DateAdded = System.DateTime.Now;
                            CommonFunctions c = new CommonFunctions();
                            cstAdd.DateAdded = c.GetCurrentDate();

                            _respository.AddUpdateCustomerContact(cstAdd, "Add");
                            act = new ActivityRepository();
                            act.AddActivityLog(Convert.ToString(Session["User"]), "Import Customer Contacts", "ImportCustomer", "Contacts added for customer " + cst.CustomerName + " from csv file " + postedFile.FileName + " by user " + Convert.ToString(Session["User"]) + ".");
                        }

                    }

                }
                else
                {
                    ViewBag.Message = "Please select file to import";
                }

                return View("ManageCustomerContacts");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "CSV file not in correct format. Please select valid CSV file and try again!";
                return View("ManageCustomerContacts");
            }
        }



        #endregion





    }
}
