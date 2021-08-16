namespace AuthUtility.Interfaces
{
    public interface IRestClient
    {
        V MakePostRestCall<K, V>(K request, string absolutepath, string serviceUri, bool isElastic = false);
        V MakeGetRestCall<V>(string absolutepath, string serviceUri);
    }
}
