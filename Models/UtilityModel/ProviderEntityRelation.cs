using System;
using System.Collections.Generic;

namespace AuthUtility.Models.UtilityModel
{
    public class ProviderEntityRelation
    {
        public int id { get; set; }
        public int erTypeId { get; set; }
        public string erTypeName { get; set; }
        public int serviceBrokerId { get; set; }
        public string serviceBrokerName { get; set; }
        public int entityId { get; set; }
        public string entityName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int documentType { get; set; }
        public bool isActive { get; set; }
        public int entityTypeId { get; set; }
        public int stateId { get; set; }
        public int districtId { get; set; }
        public int cityId { get; set; }
        public int locationId { get; set; }
        public string pinCode { get; set; }
        public List<object> attachDocIds { get; set; }
        public bool isBreakfast { get; set; }
        public bool isMammogram { get; set; }
        public bool isSoftcopy { get; set; }
    }
}