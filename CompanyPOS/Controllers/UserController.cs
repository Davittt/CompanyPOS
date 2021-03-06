﻿using DATA;
using DATA.Models;
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
	public class UserController : ApiController
	{
		//class simplePair
		//{
		//    public User user { get; set; }
		//    public Session session { get; set; }
		//}

		//public HttpResponseMessage Getall()
		//{
		//	try
		//	{
		//		using (CompanyPosDBContext database = new CompanyPosDBContext())
		//		{
		//			List<User> listUsers = database.Users.ToList();
		//			List<Session> listSession = database.Sessions.ToList();
		//			var message = Request.CreateResponse(HttpStatusCode.OK, listUsers);
		//			return message;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
		//	}
		//}

		public HttpResponseMessage GetStoreUsers(string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

					if (session != null)
					{
						if (SessionUser.UserLevel.ToLower() == "admin")
						{
							//Validate storeID and UserID
							List<User> userList = database.Users.Where(x => (x.StoreID == session.StoreID)).ToList();
							if (userList != null)
							{
								//Save last  update
								session.LastUpdate = DateTime.Now;
								database.SaveChanges();

								var message = Request.CreateResponse(HttpStatusCode.OK, userList);
								return message;
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "Users not found");
								return message;
							}
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You don't have privileges");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// GET: api/Store/5
		//READ
		//It should have permissions
		public HttpResponseMessage GetCompanyUsers(string token, int companyID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);


					var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

					if (session != null)
					{
						if (SessionUser.UserLevel.ToLower() == "admin")
						{
							//Validate storeID and UserID
							List<User> userList = database.Users.Where(x => (x.CompanyID == companyID)).ToList();
							if (userList != null)
							{
								//Save last  update
								session.LastUpdate = DateTime.Now;
								database.SaveChanges();

								var message = Request.CreateResponse(HttpStatusCode.OK, userList);
								return message;
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "Users not found");
								return message;
							}
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You don't have privileges");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// GET: api/Store/5
		//READ
		public HttpResponseMessage Get(string token, int id)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

						//Validate storeID and UserID
						User user = database.Users.ToList().FirstOrDefault(x => (x.ID == id)
							&& ((x.StoreID == session.StoreID) || ((SessionUser.Type.ToLower() == "owner") && SessionUser.CompanyID == x.CompanyID))
							);
						if (user != null)
						{
							//Save last  update
							session.LastUpdate = DateTime.Now;

							database.SaveChanges();

							var message = Request.CreateResponse(HttpStatusCode.OK, user);
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// POST: api/User
		//CREATE
		public HttpResponseMessage Post([FromBody]User user, string token, int? storeID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					
					if (session != null)
					{
						var sessionUser = database.Users.ToList().FirstOrDefault(x => (x.ID == session.UserID));

						if (user.UUID == null || user.UUID.Trim().Equals(""))
						{
							return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "UUID not specified");
						}

						//Save last  update
						session.LastUpdate = DateTime.Now;

						var currentUser = database.Users.ToList().FirstOrDefault(x => x.UUID == user.UUID && (x.StoreID == session.StoreID));
						if (currentUser != null)
						{
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "There is a user with this UUID");
							return message;
						}
						else
						{
							if (database.Users.ToList().Any(x => (x.ClerkNum == user.ClerkNum) && (x.StoreID == session.StoreID)))
							{
								return Request.CreateResponse(HttpStatusCode.OK, "ClerkNum Already exists");
							}

							//Save last  update
							var currentCompanyID = database.Stores.FirstOrDefault(x => x.ID == session.StoreID);

							if (currentCompanyID == null)
							{
								return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Wrong StoreID" + " SessionId: " + session.ID);
							}
							else
							{
								user.CompanyID = currentCompanyID.CompanyID;
							}
							if (storeID == null)
							{
								user.StoreID = session.StoreID;
							}
							else
							{
								var newStore = database.Companies.FirstOrDefault(x => x.Id == user.CompanyID).Stores.First(y => y.ID == storeID);
								if (newStore != null)
								{
									user.StoreID = newStore.ID;
								}
								else
								{
									return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Wrong StoreID");
								}
							}
							
							if (sessionUser.Type != "OWNER")
							{
								database.Users.Add(user);
								//SAVE ACTIVITY
								//database.UserActivities.Add(new UserActivity()
								//{
								//	StoreID = session.StoreID
								//	,
								//	UserID = session.UserID
								//	,
								//	Activity = "CREATE USER",
								//	Date = DateTime.Now
								//});
								database.SaveChanges();

								var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
								return message;
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You cannot create an Owner");
								return message;
							}
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx + "    " + dbEx.Message);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex + "    " + ex.Message);
			}
		}

		// PUT: api/User/5
		//UPDATE
		public HttpResponseMessage Put(int id, [FromBody]User user, string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						var sessionUser = database.Users.ToList().FirstOrDefault(x => (x.ID == session.UserID));

						if (user.UUID == null || user.UUID.Trim().Equals(""))
						{
							return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "UUID not specified");
						}

						//Save last  update
						var currentCompanyID = database.Stores.FirstOrDefault(x => x.ID == session.StoreID);
						//Save last  update
						session.LastUpdate = DateTime.Now;

						var currentUser = database.Users.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentUser != null)
						{
							if ( database.Users.ToList().Any(x => (x.ClerkNum == user.ClerkNum) && (x.StoreID == session.StoreID) && (x.ID != id))){
							 return  Request.CreateResponse(HttpStatusCode.OK, "ClerkNum Already exists");
							}

							currentUser.LastName = user.LastName;
							currentUser.UserLevel = user.UserLevel;
							currentUser.Username = user.Username;
							currentUser.Email = user.Email;
							currentUser.FirstName = user.FirstName;
							currentUser.Phone = user.Phone;
							currentUser.Status = user.Status;
							currentUser.UUID = user.UUID;
							currentUser.Active = user.Active;
							currentUser.ClerkNum =  user.ClerkNum;

							if ((sessionUser.UserLevelNum < 5) && (currentUser.UserLevelNum != user.UserLevelNum))
							{
								return Request.CreateResponse(HttpStatusCode.OK, "You don't have priviledges to edit the user access level");
							}
							else {
								currentUser.UserLevelNum = user.UserLevelNum;
							}

							currentUser.CompanyID = currentUser.CompanyID ?? Int32.Parse(currentCompanyID.CompanyID.ToString());

							if (user.Password != null)
							{
								currentUser.Password = user.Password;
							}
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								, UserID = session.UserID
								, Activity = "UPDATE USER",
								Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "User Not found");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// PUT: api/User/5
		//UPDATE
		public HttpResponseMessage Put(string uuid, [FromBody]User user, string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						if (user.UUID == null || user.UUID.Trim().Equals(""))
						{
							return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "UUID not specified");
						}
						var currentCompanyID = database.Stores.FirstOrDefault(x => x.ID == session.StoreID);
						//Save last  update
						session.LastUpdate = DateTime.Now;
						var currentUser = database.Users.ToList().FirstOrDefault(x => x.UUID == uuid && (x.StoreID == session.StoreID));

						if (currentUser != null)
						{
							currentUser.LastName = user.LastName;
							currentUser.UserLevel = user.UserLevel;
							currentUser.Username = user.Username;
							currentUser.Email = user.Email;
							currentUser.FirstName = user.FirstName;
							currentUser.Phone = user.Phone;
							currentUser.Status = user.Status;
							currentUser.UUID = user.UUID;
							currentUser.CompanyID = currentUser.CompanyID ?? Int32.Parse(currentCompanyID.CompanyID.ToString());

							if (user.Password != null)
							{
								currentUser.Password = user.Password;
							}
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "CREATE USER",
								Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "User Not found");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// DELETE: api/User/5
		//DELETE
		public HttpResponseMessage Delete(int id, string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						//Save last  update
						session.LastUpdate = DateTime.Now;

						var user = database.Users.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (user == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"User with Id = " + id.ToString() + " not found to delete");
						}
						else
						{

							database.Users.Remove(user);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE USER",
								Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Delete Success");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// DELETE: api/User/5
		//DELETE
		public HttpResponseMessage Delete(string uuid, string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						//Save last  update
						session.LastUpdate = DateTime.Now;

						var user = database.Users.ToList().FirstOrDefault(x => x.UUID == uuid && (x.StoreID == session.StoreID));

						if (user == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"User with UUID = " + uuid + " not found to delete");
						}
						else
						{

							database.Users.Remove(user);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE USER",
								Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Delete Success");
							return message;
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
						return message;
					}
				}
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
	}
}
