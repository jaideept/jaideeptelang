using Newtonsoft.Json;
using System;

namespace ContactApi.Models
{
    public class FilterModel : FilterModelBase
    {
        public Boolean IncludeInactive { get; set; }
        public string Term { get; set; }

        public FilterModel() : base()
        {
            this.Limit = 3;
        }

        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }
    }
}
