﻿using DATA;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyPOS.Controllers
{
    public class InvoiceController : ApiController
    {
        public List<Invoice> ReadAll(int StoreID, int SaleID)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    var InvoiceList = database.Invoice.Where(x => x.StoreID == StoreID && x.SaleID == SaleID).ToList();

                    return InvoiceList;
                }
            }
            catch (Exception ex)
            {
                return null;
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public void Create(int StoreID, int SaleID, DateTime Date, string PaymentMethod, string TotalPrice)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    database.Invoice.Add(new Invoice()
                    {
                        StoreID = StoreID
                        ,
                        SaleID = SaleID
                        , 
                        Date = Date
                        ,
                        PaymentMethod = PaymentMethod
                    });


                    database.SaveChanges();


                    //    var message = Request.CreateResponse(HttpStatusCode.OK, data);
                    //    return message;
                    //}
                    //else
                    //{
                    //    var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
                    //    return message;
                    //}
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public void Delete(int InvoiceID, int StoreID)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    var Invoice = database.Invoice.FirstOrDefault(x => x.ID == InvoiceID && x.StoreID == StoreID);

                    if (Invoice != null)
                    {
                        database.Invoice.Remove(Invoice);
                        database.SaveChanges();
                    }

                    //    var message = Request.CreateResponse(HttpStatusCode.OK, data);
                    //    return message;
                    //}
                    //else
                    //{
                    //    var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
                    //    return message;
                    //}
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public void Update(int InvoiceID, int StoreID, DateTime Date, string PaymentMethod, string TotalPrice)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    var Invoice = database.Invoice.FirstOrDefault(x => x.ID == InvoiceID && x.StoreID == StoreID);


                    if (Invoice != null)
                    {
                        Invoice.Date = Date;
                        Invoice.PaymentMethod = PaymentMethod;
                        //Update
                        database.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Invoice
        // GET: api/Invoice
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Invoice/5
        //public HttpResponseMessage Get(string token, int id)
        //{
        //    try
        //    {
        //        using (CompanyPOSEntities database = new CompanyPOSEntities())
        //        {
        //            SessionController sessionController = new SessionController();
        //            Session session = sessionController.Autenticate(token);

        //            if (session != null)
        //            {
        //                //Validate storeID and InvoiceID
        //                var data = database.Invoice.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
        //                if (data != null)
        //                {
        //                    //Save last  update
        //                    session.LastUpdate = DateTime.Now;
        //                    database.SaveChanges();

        //                    var message = Request.CreateResponse(HttpStatusCode.OK, data);
        //                    return message;
        //                }
        //                else
        //                {
        //                    var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
        //                    return message;
        //                }
        //            }
        //            else
        //            {
        //                var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
        //                return message;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        //// POST: api/Invoice
        ////CREATE
        //public HttpResponseMessage Post([FromBody]Invoice Invoice, string token)
        //{
        //    try
        //    {
        //        using (CompanyPOSEntities database = new CompanyPOSEntities())
        //        {
        //            SessionController sessionController = new SessionController();
        //            Session session = sessionController.Autenticate(token);

        //            if (session != null)
        //            {
        //                //Save last  update
        //                session.LastUpdate = DateTime.Now;

        //                var currentInvoice = database.Invoice.ToList().FirstOrDefault(x => (x. == Invoice.MenuID) && (x.hPos == Invoice.hPos) && (x.vPos == Invoice.vPos) && (x.StoreID == session.StoreID));
        //                if (currentInvoice != null)
        //                {
        //                    database.SaveChanges();
        //                    var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Invoice with this name");
        //                    return message;
        //                }
        //                else
        //                {
        //                    Invoice.StoreID = session.StoreID;
        //                    database.Invoice.Add(Invoice);
        //                    //SAVE ACTIVITY
        //                    database.UserActivity.Add(new UserActivity()
        //                    {
        //                        StoreID = session.StoreID
        //                        ,
        //                        UserID = session.UserID
        //                        ,
        //                        Activity = "CREATE ItemPos"
        //                    });
        //                    database.SaveChanges();

        //                    var message = Request.CreateResponse(HttpStatusCode.OK, "Create Success");
        //                    return message;
        //                }
        //            }
        //            else
        //            {
        //                var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
        //                return message;
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}",
        //                                        validationError.PropertyName,
        //                                        validationError.ErrorMessage);
        //            }
        //        }
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        //// PUT: api/Invoice/5
        ////UPDATE
        //public HttpResponseMessage Put(int id, [FromBody]Invoice Invoice, string token)
        //{
        //    try
        //    {
        //        using (CompanyPOSEntities database = new CompanyPOSEntities())
        //        {
        //            SessionController sessionController = new SessionController();
        //            Session session = sessionController.Autenticate(token);

        //            if (session != null)
        //            {
        //                //Save last  update
        //                session.LastUpdate = DateTime.Now;

        //                var currentInvoice = database.Invoice.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

        //                if (currentInvoice != null)
        //                {
        //                    currentInvoice.hPos = Invoice.hPos;
        //                    currentInvoice.vPos = Invoice.Page;
        //                    currentInvoice.ItemID = Invoice.ItemID;
        //                    currentInvoice.Page = Invoice.Page;

        //                    //SAVE ACTIVITY
        //                    database.UserActivity.Add(new UserActivity()
        //                    {
        //                        StoreID = session.StoreID
        //                        ,
        //                        UserID = session.UserID
        //                        ,
        //                        Activity = "CREATE ItPagePos"
        //                    });

        //                    database.SaveChanges();
        //                    var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
        //                    return message;
        //                }
        //                else
        //                {
        //                    var message = Request.CreateResponse(HttpStatusCode.OK, "Invoice Not found");
        //                    return message;
        //                }
        //            }
        //            else
        //            {
        //                var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
        //                return message;
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}",
        //                                        validationError.PropertyName,
        //                                        validationError.ErrorMessage);
        //            }
        //        }
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        //// DELETE: api/Invoice/5
        ////DELETE
        //public HttpResponseMessage Delete(int id, string token)
        //{
        //    try
        //    {
        //        using (CompanyPOSEntities database = new CompanyPOSEntities())
        //        {
        //            SessionController sessionController = new SessionController();
        //            Session session = sessionController.Autenticate(token);

        //            if (session != null)
        //            {
        //                //Save last  update
        //                session.LastUpdate = DateTime.Now;

        //                var Invoice = database.Invoice.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

        //                if (Invoice == null)
        //                {
        //                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //                            "Invoice with Id = " + id.ToString() + " not found to delete");
        //                }
        //                else
        //                {

        //                    database.Invoice.Remove(Invoice);
        //                    //SAVE ACTIVITY
        //                    database.UserActivity.Add(new UserActivity()
        //                    {
        //                        StoreID = session.StoreID
        //                        ,
        //                        UserID = session.UserID
        //                        ,
        //                        Activity = "DELETE ItPagPos"
        //                    });

        //                    database.SaveChanges();
        //                    var message = Request.CreateResponse(HttpStatusCode.OK, "Delete Success");
        //                    return message;
        //                }
        //            }
        //            else
        //            {
        //                var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
        //                return message;
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Trace.TraceInformation("Property: {0} Error: {1}",
        //                                        validationError.PropertyName,
        //                                        validationError.ErrorMessage);
        //            }
        //        }
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}
    }
}