using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
    public class UCIResponseSingle<T>
    {
        public T value;
    }

    public class UCIResponseMultiple<T>
    {
        public T values;
    }

    public abstract class UCIBlob
    {
            [JsonProperty(".anonymous")]
            public bool anonymous { get; set; }
            [JsonProperty(".type")]
            public string type { get; set; }
            [JsonProperty(".name")]
            public string name { get; set; }
            [JsonProperty(".index")]
            public int index { get; set; }
    }
}
