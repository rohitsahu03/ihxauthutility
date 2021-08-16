namespace AuthUtility.Constants
{
    public class UtilityConstants
    {
        public const string AuthUtilityAppId = "9231";
        public const string UserSession = "UserContext";
        public const string IsEmailVerfied = "IsEmailVerfied";
        public const string IsOTPVerified = "IsOTPVerified";
        public const string TypePropertyMerge = "dbo.tblSourcePropertyMerge";
        public const string TypeRoleMerge = "dbo.tblSourceRoleMerge";
        public const string TypeUpdateUserName = "dbo.tblUpdateUserName";
        public const string EmployeeCodeFormat = "EmpCode";
        public const string OtherFormat = "Other";
        public const int MaxUserIdCount = 500;
        public const string EmpIdLoginType = "EMPID";
        public const string MediMarketAppId = "5385";
        public const string ProcheckAppId = "9203";
        public const string GroupKeys = "MA_Role,DC_Role,Insurer_Role,Corp_Role";
        public const string OldUserName = "OldUserName";
        public const string NewUserName = "NewUserName";
        public const string EmployeeId = "EmployeeId";
        public const string EntityId = "EntityId";
        public const string UserId = "UserId";
        public const string TypeBulkUpdateUserDetails = "dbo.tblBulkUpdateUserDetails";
        public const int RetailEntityId = 1018900;
        public const string NumberRegex = "^[0-9]+$";
    }
    public class MsgConstants
    {
        public const string SuccessMsg = "Success !!!";
        public const string SuccessMsgWithCount = "Updated For {0} User(s)";
        public const string ErrorMsg = "Error !!!";
        public const string InvalidPolIdMsg = "Invalid Policy ID !!!";
        public const string InvalidData = "Invalid Data!!!";
        public const string InvalidBroker = "Broker Doesn't Exists";
        public const string ErrorInGettingBrokers = "Error Fetch Broker Details";
        public const string ErrorInSavingAlias = "Error occured while saving alias";
        public const string NoAliasMappingFound = "Alias Mapping Not Found";
        public const string IncorrectMapping = "Incorrect Alias Mapping";
        public const string LoginFailedMsg = "Login Failed";
        public const string DuplicateUsersError = "{0} Duplicate Users Found";
        public const string InvalidEntityId = "Invalid New ProviderMasterEntityId";
        public const string ErrorInExcel = "Invalid Data. Download Sample Excel.";
        public const string NoDataFound = "No Data Found";
        public const string DuplicateData = "Duplicate Values => {0}";
        public const string UsersAlreadyPresent = "Already Present => {0}";
        public const string ServerError = "Server Error";
        public const string RequestCannotBeNull = "Request Cannot Be Null";
        public const string ErrorGettingToken = "Error Generating AccessToken";
        public const string InvalidEntityIdUserCreation = "EntityId cannot be negative";
        public const string MARoleError = "ManagerEmployeeId is mandatory for MA_Role";
        public const string AlreadyPresent = "Already Present";
        public const string InvalidLoginType = "Invalid LoginType";
        public const string UnableToGetAppId = "Unable to fetch AppId";
        public const string UnableToGetAuthenticationKey = "Unable to fetch AuthenticationKey";
        public const string UserNameCannotBeNull = "UserName cannot be null";
        public const string Done = "Done !!!";
        public const string SameOldAndNewUserName = "Same Old And New UserNames => {0}";
        public const string ErrorInReadingFile = "Error in reading file";
        public const string MultipleUsers = "Multiple Users Found";
        public const string ErrorProcessingData = "Error Processing Data";
        public const string UserIdUserNameMismatch = "UserId-UserName mismatch => {0}";
        public const string UnableToGetUserId = "Unable to retrieve userid";
        public const string UserNotFound = "User Not Found";
        public const string UnableToUpdateUName = "Error in Updating UserName";
        public const string FailedForUsers = "Failed for users => {0}";
        public const string NullResponse_RestCall = "Null response from rest call";
        public const string ParametersNotFound = "Parameters Not Found";
        public const string DataUpdationFailed = "Data Updation Failed";
        public const string EmpIdMandatory = "EmployeeId is Mandatory";
    }

    public class UserPropertyConstants
    {
        public const string DOB = "DOB";
        public const string Gender = "Gender";
        public const string IWP_EmpID = "IWP_EmpID";
        public const string MAID = "MAID";
        public const string DateOfMarriage = "DateOfMarriage";
        public const string DateOfJoining = "DateOfJoining";
    }
    public class CachingConstants
    {
        public const string UserRolesProclaim = "UserRolesProclaim";
        public const string UserRoles = "UserRoles";
        public const string Cities = "Cities";
        public const string States = "States";
        public const string Locations = "Locations";
        public const string DC = "DC";
        public const string DCMaster = "DCMaster";
        public const string ContractDetails = "ContractDetails";
        public const string Entities = "Entities";
    }

    public class APIConstants
    {
        public const string SignInWithPassword = "/SignIn/Password";
        public const string SyncToElastic = "/UserDetails/SyncFromDatabase";
        public const string GetCities = "/masterdata/lookup/city";
        public const string GetStates = "/masterdata/lookup/state";
        public const string GetLocations = "/masterdata/lookup/location";
        public const string ContractDetails = "/contractlookup/contracts";
        public const string EntityRelationDetails = "EntityRelation/search/{0}.json";
        public const string DCEntity = "/diagnosticCenters";
        public const string UpdateBaseProfilePath = "/UserDetails/UpdateBaseUserProfile";
        public const string UpdateAppBasedProfilePath = "/UserDetails/UpdateAppBasedProfile";
        public const string SignInWithAppKey = "/SignIn/ApplicationKey";
        public const string CreateRetaileUser = "/UserCreation/RetailUser";
        public const string CreateCorporateUser = "/UserCreation/CorporateUser";
        public const string RemoveBaseProfile = "/Custom/RemoveBaseUserProfileElastic";
        public const string LookUpRequest = "/IHXSupreme/LookUpRequest";
        public const string EntityListRequest = "/IHXSupreme/EntityList";
        public const string EntityRelationListRequest = "/IHXSupreme/EntityRelationList";
    }

    //Do not add new variables into it
    //Using all values with reflection
    public class UserCreationConstants
    {
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string UserName = "UserName";
        public const string Email = "Email";
        public const string PhoneNumber = "PhoneNumber";
        public const string Gender = "Gender";
        public const string DOB = "DateOfBirth";
        public const string Password = "Password";
        public const string EntityId = "EntityId";
        //public const string EmpId = "EmployeeId";
        //public const string MAID = "MAID";
    }

    public class DBConstants
    {
        public const string MediAuth = "MediAuth";
    }
}