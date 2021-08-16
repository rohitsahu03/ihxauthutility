using AuthUtility.Constants;

namespace AuthUtility.Models
{
    public class RemoveElasticBaseProfile : BaseResponse
    {
        public int EntityId { get; set; }
        public int ActionPerformedBy { get; set; }

        public bool IsValid()
        {
            bool response = true;
            response = response && EntityId > 0;
            response = response && EntityId != UtilityConstants.RetailEntityId;
            return response;
        }
    }
}
