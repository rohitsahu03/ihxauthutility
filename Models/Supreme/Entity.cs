using System;
using System.Collections.Generic;

namespace AuthUtility.Models.Supreme
{
    public class EntityListRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool EntityListByEntityId { get; set; }
        public bool EntityListByStateId { get; set; }
        public bool EntityListByCityId { get; set; }
        public bool EntityListByDistrictId { get; set; }
        public bool EntityListByLocationId { get; set; }
        public bool EntityListByEntityType { get; set; }
        public bool EntityListByEntityName { get; set; }
        public bool EntityListByParentId { set; get; }
        public bool GetEntityProperty { get; set; }
        public bool GetAllDetails { get; set; }
        public bool GetInactiveEntities { get; set; }
        public bool GetEntityByCMSID { get; set; }

        public bool GetEntityByPinCodes { get; set; }
        public List<long> EntityIds { get; set; }
        public List<int> EntityTypes { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> LocationIds { get; set; }
        public List<string> EntityNames { get; set; }
        public List<long> ParentIds { get; set; }
        public List<int> CMSIds { get; set; }
        public List<string> PinCodes { get; set; }
        public Operand Operation { get; set; }

    }
    public class EntityListResponse
    {
        public long TotalRecords { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
        public List<EntityDTO> EntitiesFullDetails { get; set; }
        public List<EntityDetail> Entities { get; set; }
    }
    public class EntityDetail
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public bool UserExist { get; set; }
        public List<EntityProperty> PropertyList { get; set; }
    }
    public class EntityDTO
    {
        public long EntityId { get; set; }
        public long EntityParentId { get; set; }
        public int EntityEntityType { get; set; }
        public int EntityHierarchyGroup { get; set; }
        public int EntityHierarchySubGroup { get; set; }
        public string EntityFullName { get; set; }
        public string EntityDisplayName { get; set; }
        public string EntityPrimaryAddress { get; set; }
        public string EntityLandmark { get; set; }
        public string EntityPinCode { get; set; }
        public int EntityCountryId { get; set; }
        public string EntityCountryName { get; set; }
        public int EntityStateId { get; set; }
        public string EntityStateName { get; set; }
        public int EntityCityId { get; set; }
        public string EntityCityName { get; set; }
        public int EntityDistrictId { get; set; }
        public string EntityDistrictName { get; set; }
        public string EntityLandlineNumber { get; set; }
        public string EntityFaxNumber { get; set; }
        public string EntityMobileNumber { get; set; }
        public string EntityEmailAddress { get; set; }
        public string EntityContactName { get; set; }
        public string EntityContactNumber { get; set; }
        public string EntityContactEmail { get; set; }
        public string EntityWebsite { get; set; }
        public decimal EntityLocationLatitude { get; set; }
        public decimal EntityLocationLongitude { get; set; }
        public string EntityPanNumber { get; set; }
        public string EntityPanHolderName { get; set; }
        public bool EntityPanStatus { get; set; }
        public string EntityPanRemarks { get; set; }
        public string EntityRating { get; set; }
        public bool EntityIsActive { get; set; }
        public int EntityAddUser { get; set; }
        public DateTime EntityCreatedOn { get; set; }
        public int EntityModifiedUser { get; set; }
        public DateTime EntityModifiedOn { get; set; }
        public string EntityLocationName { get; set; }
        public int EntityLocationId { get; set; }
        public bool EntityLatLongVerified { get; set; }
        public string EntityTollfreeNo { get; set; }
        public List<EntityProperty> PropertyList { get; set; }

    }
    public class EntityProperty
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int GroupId { get; set; }
        public int LookUpId { get; set; }
        public string GroupName { get; set; }
    }
}
