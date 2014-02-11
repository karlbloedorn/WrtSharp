using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
    public class JSONRPCResponse<T>
    {
        public string jsonrpc { get; set; }
        public T result { get; set; }
        public string id { get; set; }
    }

  public class UbusRPCResponse<T> : JSONRPCResponse<T>
  {
    [JsonProperty("result")]
    private JArray raw_result { get; set; }

    [JsonIgnore]
    public int code
    {
      get
      {
        JToken responseCode = (JToken)raw_result[0];
        int returnCode = responseCode.ToObject<int>();
        return returnCode;
      }
    }

    [JsonIgnore]
    public new T result
    {
      get
      {
        JObject responseResult = (JObject)raw_result[1];
        var convertedResult = responseResult.ToObject<T>();
        return convertedResult;
      }
    }
   

  }
}
