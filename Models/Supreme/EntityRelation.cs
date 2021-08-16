using System;
using System.Collections.Generic;

namespace AuthUtility.Models.Supreme
{
    public class EntityRelationRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool EntityRelationByEntityRelationId { set; get; }
        public bool EntityRelationByEntityRelationTypeId { set; get; }
        public bool EntityRelationByPrimaryId { set; get; }
        public bool EntityRelationBySecondaryId { set; get; }
        public bool EntityRelationByPrimaryTypeId { set; get; }
        public bool GetPrimaryDetail { set; get; }
        public bool GetSecondaryDetail { set; get; }
        public bool GetEntityProperty { set; get; }
        public List<int> EntityRelationIds { set; get; }
        public List<int> EntityRelationTypeIds { set; get; }
        public List<long> PrimaryIds { set; get; }
        public List<long> SecondaryIds { set; get; }
        public List<long> PrimaryTypeIds { set; get; }
        public bool EntityListByStateId { get; set; }
        public bool EntityListByCityId { get; set; }
        public bool EntityListByDistrictId { get; set; }
        public bool EntityListByLocationId { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> LocationIds { get; set; }
        public Operand Operation { get; set; }

    }
    public class EntityRelationResponse 
    {
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
        public List<EntityRelation> EntityRelationDetails { set; get; }
        public long TotalRecords { get; set; }
    }
    public class EntityRelation
    {
        public int EntityRelationId { get; set; }
        public long EntityRelationPrimaryId { get; set; }
        public string EntityRelationPrimaryEntityLocation { get; set; }
        public long EntityRelationSecondaryID { get; set; }
        public string EntityRelationSecondaryEntityLocation { get; set; }
        public int EntityRelationType { get; set; }
        public string EntityRelationTypeName { get; set; }
        public DateTime? EntityRelationStartDate { get; set; }
        public DateTime? EntityRelationEndDate { get; set; }
        public int EntityRelationDocumentType { get; set; }
        public string EntityRelationDocumentLink { get; set; }
        public string EntityRelationRemarks { get; set; }
        public bool EntityRelationIsActive { get; set; }
        public int EntityRelationAddUser { get; set; }
        public DateTime EntityRelationCreateOn { get; set; }
        public int EntityRelationModifiedUser { get; set; }
        public DateTime EntityRelationModifiedOn { get; set; }
        public EntityDTO PrimaryEntityDetail { set; get; }
        public EntityDTO SecondaryEntityDetail { set; get; }
    }
}
