namespace AuthUtility.Models.UtilityModel
{
    public class DCMasterProvider
    {
        public int DCid { get; set; }
        public int id { get; set; }
        public int parentId { get; set; }
        public int entityType { get; set; }
        public int e_Hierarchy_Group { get; set; }
        public int e_Hierarchy_Sub_Group { get; set; }
        public string fullName { get; set; }
        public string primaryAddress { get; set; }
        public string pinCode { get; set; }
        public int countryId { get; set; }
        public int stateId { get; set; }
        public int cityId { get; set; }
        public int districtId { get; set; }
        public int locationId { get; set; }
        public string landlineNumber { get; set; }
        public string emailAddress { get; set; }
        public string primaryContactName { get; set; }
        public string primaryContactNumber { get; set; }
        public string secondaryContactName { get; set; }
        public string secondaryContactNumber { get; set; }
        public string website { get; set; }
        public string primaryLocationLatitude { get; set; }
        public string primaryLocationLongitude { get; set; }
        public bool isLegal { get; set; }
        public bool isactive { get; set; }
        public string pan { get; set; }
        public string panHolderName { get; set; }
        public string primaryContactEmail { get; set; }
        public string secondaryContactEmail { get; set; }
        public string shortName { get; set; }
        public string contactSTDCode { get; set; }
        public string mobileNumber { get; set; }
        public string landmark { get; set; }
    }
}