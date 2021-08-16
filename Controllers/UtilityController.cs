using AuthUtility.Common;
using AuthUtility.Interfaces;
using AuthUtility.Models.UtilityModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AuthUtility.Controllers
{
    public class UtilityController : Controller
    {
        private readonly IUtilityProvider _utilityProvider;
        private readonly IDBProvider _dbProvider;
        private readonly ICacheProvider _cacheProvider;
        public UtilityController(IUtilityProvider utilityProvider, IDBProvider dbProvider, ICacheProvider cacheProvider)
        {
            _utilityProvider = utilityProvider;
            _dbProvider = dbProvider;
            _cacheProvider = cacheProvider;
        }
        public ActionResult GetRolesForGroup(string groupKey)
        {
            var roles = _utilityProvider.GetLoginTypeRoles(groupKey);
            var obj = (roles == null || roles.Count == 0) ? null : roles.Select(x => new { id = x.Id, name = x.Name }).ToList();
            return Json(obj);
        }

        public ActionResult GetRoles()
        {
            var roles = _dbProvider.GetAllRoles();
            var obj = (roles == null || roles.Count == 0) ? null : roles.Select(x => new { id = x.Key, name = x.Value }).ToList();
            return Json(obj);
        }

        public ActionResult Contracts()
        {
            var eType = _cacheProvider.GetContractDetails();
            return Json(eType?.OrderBy(x => x.name));
        }

        public ActionResult EntityRelation(int contractId)
        {
            var erType = _utilityProvider.GetERDetails(contractId);
            var response = erType
                          .OrderBy(x => x.EntityFullName)
                          .Select(x => new { id = x.EntityId, name = x.EntityFullName + "," + x.EntityPinCode + " => " + x.EntityId })
                          .ToList();
            return Json(response);

        }

        public ActionResult DCcontractEntity(string SearchTerm)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return new EmptyResult();

            List<DCMasterProvider> dcMaster = _cacheProvider.GetDCMaster();

            int id = 0;
            int.TryParse(SearchTerm, out id);

            List<DCMasterProvider> response = null;

            if (dcMaster != null && dcMaster.Count > 0)
            {
                response = id > 0 ? dcMaster.Where(x => x.DCid.ToString().StartsWith(SearchTerm)).ToList() :
                                    dcMaster.Where(x => x.fullName.Trim().ToLower().Contains(SearchTerm.ToLower())).ToList();
            }

            return Json(response);
        }

        public ActionResult FetchEntity(string SearchTerm)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return new EmptyResult();

            var entities = _cacheProvider.GetEntities();
            if (!entities.HasRecords())
                return new EmptyResult();

            var response = entities.Where(x => x.Value.ToLower().Contains(SearchTerm.ToLower())).Select(x => new { id = x.Key, entity = x.Value }).ToList();
            return Json(response);
        }

        public ActionResult GetAppBasedProfile(int UserId, string GroupKey)
        {
            var appBasedProfile = _dbProvider.GetPreviousAppBasedProfile(UserId, GroupKey);
            return Json(appBasedProfile);
        }

        public ActionResult RefreshCache()
        {
            _cacheProvider.RefreshCache();

            return Json(true);
        }


    }
}