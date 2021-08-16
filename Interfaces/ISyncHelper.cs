using AuthUtility.Common;
using AuthUtility.Models;
using System.Threading.Tasks;

namespace AuthUtility.Interfaces
{
    public interface ISyncHelper
    {
        ElasticSyncResponse SyncToElastic(string UserIds);
        BaseResponse RemoveBaseUserProfile(int UserId);
    }
}
