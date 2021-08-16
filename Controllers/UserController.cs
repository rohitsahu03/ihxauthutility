using AuthUtility.Interfaces;
using AuthUtility.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AuthUtility.Common;
using System.Linq;
using AuthUtility.Constants;

namespace AuthUtility.Controllers
{
    [AuthorizationFilter("9227,9226")]
    public class UserController : Controller
    {
        private IDBProvider _dbProvider = null;
        public UserController(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        public ActionResult ManagerCreation(string LoginName, int UserId, string EmailId, string PhoneNo)
        {
            if (!string.IsNullOrEmpty(LoginName) || UserId != 0 || !string.IsNullOrEmpty(EmailId) || !string.IsNullOrEmpty(PhoneNo))
                return View(_dbProvider.GetUserForManager(LoginName, UserId, EmailId, PhoneNo));

            return View(new ManagerCreationDTO());
        }

        [HttpPost]
        public ActionResult ManagerCreation(ManagerCreationDTO model)
        {
            model.UserProperties = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.PropertiesString);

            if (ModelState.IsValid && (model.Id != 0 || !string.IsNullOrEmpty(model.Password)))
            {
                if (!model.UserProperties.Any(x => x.Key == "EmployeeId"))
                {
                    model.ErrorMsg = MsgConstants.EmpIdMandatory;
                    return View(model);
                }

                model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                BaseResponse response = model.Id > 0 ? _dbProvider.UpdateUser(model) : _dbProvider.CreateUser(model);
                if (response.IsNull() || !response.IsSuccess)
                {
                    model.ErrorMsg = response.IsNull() ? MsgConstants.ServerError : response.Message;
                    return View(model);
                }

                return RedirectToAction("ManagerCreation");
            }
            else
            {
                ModelState.AddModelError("", "Please provide valid data");
                return View(model);
            }
        }
    }
}