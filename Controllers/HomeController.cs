using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace AuthUtility.Controllers
{
    [AuthorizationFilter("9227")]
    public class HomeController : Controller
    {
        private readonly IDBProvider _dbProvider;
        private readonly ILogger _logger;
        private readonly ISyncHelper _syncHelper;
        public HomeController(ILogger<HomeController> logger, IDBProvider dbProvider, ISyncHelper syncHelper)
        {
            this._dbProvider = dbProvider;
            this._logger = logger;
            this._syncHelper = syncHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChangeEmail(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateEmail");

            return View("UpdateEmail", new UpdateEmail() { Message = Message });
        }

        [HttpPost]
        public IActionResult ChangeEmail(UpdateEmail Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.ChangeEmail(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateEmail", Model);
                }

                return RedirectToAction("ChangeEmail", "Home", new { @message = result.Message });

            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeEmail :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateEmail", Model);
        }

        public IActionResult UpdateFirstLastName(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateFirstLastName");

            return View("UpdateFirstLastName", new UpdateFirstLastName() { Message = Message });
        }

        [HttpPost]
        public IActionResult UpdateFirstLastName(UpdateFirstLastName Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdateFirstLastName(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateFirstLastName", Model);
                }

                return RedirectToAction("UpdateFirstLastName", "Home", new { @message = result.Message });

            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateFirstLastName :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateFirstLastName", Model);
        }
        public IActionResult ChangeMob(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateMob");

            return View("UpdateMob", new UpdateMob() { Message = Message });
        }

        [HttpPost]
        public IActionResult ChangeMob(UpdateMob Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.ChangeMob(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateMob", Model);
                }

                return RedirectToAction("ChangeMob", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeMob :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateMob", Model);
        }
        public IActionResult UpdatePassword(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdatePassword");

            return View("UpdatePassword", new UpdatePassword() { Message = Message });
        }

        [HttpPost]
        public IActionResult UpdatePassword(UpdatePassword Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.ChangePassword(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdatePassword", Model);
                }

                return RedirectToAction("UpdatePassword", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdatePassword :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdatePassword", Model);
        }
        public IActionResult UpdateLoginname(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateLoginname");

            return View("UpdateLoginname", new UpdateLoginname() { Message = Message });
        }

        [HttpPost]
        public IActionResult UpdateLoginname(UpdateLoginname Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdateLoginname(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateLoginname", Model);
                }

                return RedirectToAction("UpdateLoginname", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateLoginname :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateLoginname", Model);
        }

        public IActionResult UpdateEntityId(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateEntityId");

            return View("UpdateEntityId", new UpdateEntityId() { Message = Message });
        }

        [HttpPost]
        public IActionResult UpdateEntityId(UpdateEntityId Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdateEntityId(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateEntityId", Model);
                }

                return RedirectToAction("UpdateEntityId", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateEntityId :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateEntityId", Model);
        }
        public IActionResult UnlockUsers(string Message)
        {

            if (string.IsNullOrEmpty(Message))
                return View("UnlockUsers");

            return View("UnlockUsers", new UnlockUsers() { Message = Message });
        }

        [HttpPost]
        public IActionResult UnlockUsers(UnlockUsers Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UnlockUsers(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UnlockUsers", Model);
                }

                return RedirectToAction("UnlockUsers", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UnlockUsers :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UnlockUsers", Model);
        }

        public IActionResult ChangeActiveStatus(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateActiveStatus");

            return View("UpdateActiveStatus", new UpdateActiveStatus() { Message = Message });
        }

        [HttpPost]
        public IActionResult ChangeActiveStatus(UpdateActiveStatus Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.ChangeActiveStatus(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateActiveStatus", Model);
                }

                return RedirectToAction("ChangeActiveStatus", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeActiveStatus :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateActiveStatus", Model);
        }
        public IActionResult UpdateIsOTPVerified(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("UpdateIsOTPVerified");

            return View("UpdateIsOTPVerified", new UpdateIsOTPVerified() { Message = Message });
        }

        [HttpPost]
        public IActionResult UpdateIsOTPVerified(UpdateIsOTPVerified Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdateIsOTPVerified(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateIsOTPVerified", Model);
                }

                return RedirectToAction("UpdateIsOTPVerified", "Home", new { @message = result.Message });

            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateIsOTPVerified :" + ex.Message + ex.StackTrace);
                Model.Message = ex.Message;
            }
            return View("UpdateIsOTPVerified", Model);
        }
        public IActionResult UpdateRole(string Message)
        {
            UpdateRole viewResponse = new UpdateRole();
            viewResponse.AllRoles = _dbProvider.GetAllRoles();
            if (!string.IsNullOrEmpty(Message))
                viewResponse.Message = Message;
            return View("UpdateRole", viewResponse);
        }

        [HttpPost]
        public IActionResult UpdateRole(UpdateRole Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                Model.AllRoles = _dbProvider.GetAllRoles();
                if (ModelState.IsValid && Model.SelectedRolesIds.Count > 0 && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdateRolesinAuth(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdateRole", Model);
                }

                return RedirectToAction("UpdateRole", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateRole :" + ex.Message + ex.StackTrace);
            }
            return View("UpdateRole");
        }

        public IActionResult UpdatePasswordBulk(string Message)
        {
            UpdatePasswordBulk viewResponse = new UpdatePasswordBulk();
            if (!string.IsNullOrEmpty(Message))
                viewResponse.Message = Message;
            return View("UpdatePasswordBulk", viewResponse);
        }

        [HttpPost]
        public IActionResult UpdatePasswordBulk(UpdatePasswordBulk Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid && Model.Validate())
                {
                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    result = _dbProvider.UpdatePasswordBulk(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("UpdatePasswordBulk", Model);
                }

                return RedirectToAction("UpdatePasswordBulk", "Home", new { @message = result.Message });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdatePasswordBulk :" + ex.Message + ex.StackTrace);
            }
            return View("UpdatePasswordBulk");
        }


        //public IActionResult UpdateFromCMS(string Message)
        //{
        //    if (string.IsNullOrEmpty(Message))
        //        return View("UpdateFromCMS");

        //    return View("UpdateFromCMS", new UpdateFromCMS() { Message = Message });

        //}

        //[HttpPost]
        //public IActionResult UpdateFromCMS(UpdateFromCMS Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid && ValidEntry.IsValid(Model.TauIds))
        //        {
        //            Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
        //            result = _dbProvider.UpdateFromCMS(Model);
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("UpdateFromCMS", Model);
        //        }

        //        return RedirectToAction("UpdateFromCMS", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateFromCMS :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("UpdateFromCMS");
        //}

        //public IActionResult BrokerLogin(string Message)
        //{
        //    if (string.IsNullOrEmpty(Message))
        //        return View("BrokerLogin");

        //    return View("BrokerLogin", new BrokerLogin() { Message = Message });

        //}

        //[HttpPost]
        //public IActionResult BrokerLogin(BrokerLogin Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid)
        //            result = _dbProvider.CreateBrokerLogin(Model);

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("BrokerLogin", Model);
        //        }

        //        return RedirectToAction("BrokerLogin", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in BrokerLogin :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("BrokerLogin");
        //}
        //public IActionResult UpdateProclaimRole(string Message)
        //{
        //    UpdateProclaimRole viewResponse = new UpdateProclaimRole();
        //    viewResponse.AllRoles = _dbProvider.GetAllRoles(true);
        //    if (!string.IsNullOrEmpty(Message))
        //        viewResponse.Message = Message;
        //    return View("UpdateProclaimRole", viewResponse);
        //}

        //[HttpPost]
        //public IActionResult UpdateProclaimRole(UpdateProclaimRole Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        Model.AllRoles = _dbProvider.GetAllRoles(true);
        //        if (ModelState.IsValid && Model.SelectedRolesIds.Count > 0 && ValidEntry.IsValid(Model.UserIds) && Model.AllRoles != null && Model.AllRoles.Keys.Count > 0)
        //        {
        //            Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
        //            result = _dbProvider.UpdateProclaimRole(Model);
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("UpdateProclaimRole", Model);
        //        }

        //        return RedirectToAction("UpdateProclaimRole", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateProclaimRole :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("UpdateProclaimRole");
        //}

        //public IActionResult UpdateShortName(string Message)
        //{
        //    if (string.IsNullOrEmpty(Message))
        //        return View("UpdateShortName");

        //    return View("UpdateShortName", new UpdateShortName() { Message = Message });
        //}

        //[HttpPost]
        //public IActionResult UpdateShortName(UpdateShortName Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid)
        //        {
        //            Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
        //            result = _dbProvider.UpdateShortName(Model);
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("UpdateShortName", Model);
        //        }

        //        return RedirectToAction("UpdateShortName", "Home", new { @message = result.Message });

        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateShortName :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("UpdateShortName");
        //}

        //public IActionResult ResetPassword(string Message)
        //{
        //    if (string.IsNullOrEmpty(Message))
        //        return View("ResetPassword");

        //    return View("ResetPassword", new ResetPassword() { Message = Message });
        //}
        //[HttpPost]
        //public IActionResult ResetPassword(ResetPassword Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid && !string.IsNullOrEmpty(GetCorporateName(int.Parse(Model.ProviderMasterEntityId))))
        //        {
        //            bool validUserIds = true;
        //            if (!string.IsNullOrEmpty(Model.UserIds))
        //                validUserIds = ValidEntry.IsValid(Model.UserIds);

        //            if (validUserIds)
        //            {
        //                Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
        //                result = _dbProvider.ResetPassword(Model);
        //            }
        //        }
        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("ResetPassword", Model);
        //        }
        //        return RedirectToAction("ResetPassword", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ResetPassword :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("ResetPassword");
        //}

        //public string GetCorporateName(int EntityId)
        //{
        //    if (!(EntityId > default(int)))
        //        return null;

        //    return _dbProvider.GetNameFromEntityId(EntityId);
        //}

        //public IActionResult SyncToElastic(string Message)
        //{

        //    if (string.IsNullOrEmpty(Message))
        //        return View("SyncToElastic");

        //    return View("SyncToElastic", new SyncToElastic() { Message = Message });
        //}

        //[HttpPost]
        //public IActionResult SyncToElastic(SyncToElastic Model)
        //{
        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid && ValidEntry.IsValid(Model.TauIds))
        //        {
        //            ElasticSyncResponse syncResponse = _syncHelper.SyncToElastic(Model.TauIds);
        //            if (syncResponse != null)
        //                result.IsSuccess = syncResponse.IsSuccess;
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View("SyncToElastic", Model);
        //        }

        //        return RedirectToAction("SyncToElastic", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in SyncToElastic :" + ex.Message + ex.StackTrace);
        //    }
        //    return View("SyncToElastic");
        //}

        //public ActionResult SaveCorporateAlias(long PolCorporateId, string Message)
        //{
        //    if (!string.IsNullOrEmpty(Message))
        //        return View(new AliasMapping() { Message = Message });
        //    else if (PolCorporateId > 0)
        //        return View(_dbProvider.GetAliasMappingForUi(PolCorporateId));

        //    return View(new AliasMapping());
        //}

        //[HttpPost]
        //public ActionResult SaveCorporateAlias(AliasMapping Model)
        //{
        //    if (Model.IsNull() || !Model.IsValid())
        //    {
        //        Model.Message = MsgConstants.ParametersNotFound;
        //        return View(Model);
        //    }

        //    try
        //    {
        //        BaseResponse result = new BaseResponse();
        //        if (ModelState.IsValid)
        //        {
        //            Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
        //            result = _dbProvider.SaveCorporateAlias(Model);
        //        }

        //        if (!result.IsSuccess)
        //        {
        //            Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
        //            return View(Model);
        //        }

        //        return RedirectToAction("SaveCorporateAlias", "Home", new { @message = result.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_logger != null)
        //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in SaveCorporateAlias :" + ex.Message + ex.StackTrace);

        //        Model.Message = ex.Message;
        //        return View(Model);
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}