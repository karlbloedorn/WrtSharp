using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
  class JsonRPCRequest
  {
      public string jsonrpc = "2.0";
      public string method { get; set; }

      [JsonProperty("params")]
      public object[] @params { get; set; }
      public string id { get; set; }
  }
}
