using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AuthUtility.Controllers
{
    [ApiController]
    public class RestCallController : ControllerBase
    {
        private readonly IDBProvider _dbProvider;
        private readonly ILogger _logger;

        public RestCallController(IDBProvider dbProvider, ILogger<RestCallController> logger)
        {
            _dbProvider = dbProvider;
            _logger = logger;
        }
        [Route("SaveCorporateAlias")]
        [HttpPost]
        public BaseResponse SaveCorporateAlias(AliasMapping Model)
        {
            if (Model.IsNull() || !Model.IsValid())
                return new BaseResponse() { Message = MsgConstants.ParametersNotFound };

            try
            {
                Model.ActionPerformedBy = 111;
                var resp = _dbProvider.SaveCorporateAlias(Model);
                if (resp.IsNotNull() && resp.IsSuccess)
                    resp.Message = null;

                return resp;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in SaveCorporateAlias :" + ex.Message + ex.StackTrace);

                return new BaseResponse() { Message = ex.Message };
            }
        }
    }
}