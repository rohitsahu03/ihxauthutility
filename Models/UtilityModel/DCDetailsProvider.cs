using System.Collections.Generic;

namespace AuthUtility.Models.UtilityModel
{
    public class LookUpData
    {
        public int lookUpId { get; set; }
        public string lookUpType { get; set; }
        public string lookUpValue { get; set; }
        public int lookUpParent { get; set; }
        public int sortOrder { get; set; }
    }

    public class RootObject
    {
        public bool isSuccess { get; set; }
        public int timeInMS { get; set; }
        public List<LookUpData> lookUpData { get; set; }
    }
}