using Newtonsoft.Json;

namespace AuthUtility.Models.Elastic
{
    public class ElasticDeleteResponse
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public int total { get; set; }
        public int deleted { get; set; }
        public int batches { get; set; }
        public int version_conflicts { get; set; }
        public int noops { get; set; }
        public Retries retries { get; set; }
        public int throttled_millis { get; set; }
        //public int requests_per_second { get; set; }
        public int throttled_until_millis { get; set; }
    }
    public class Retries
    {
        public int bulk { get; set; }
        public int search { get; set; }
    }

    public class Match
    {
        [JsonProperty("ProfileDataObj.EntityId")]
        public int EntityId { get; set; }
    }

    public class Query
    {
        public Match match { get; set; }
    }

    public class ElasticDeleteRequest
    {
        public Query query { get; set; }

    }
}
