using AuthUtility.Constants;
using LinqToDB.Data;

namespace AuthUtility.MediBuddyDBFactory
{
    public class MediAuthConnection : DataConnection
    {
        public MediAuthConnection() : base(DBConstants.MediAuth) { }
    }
}
