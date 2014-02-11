using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WrtSharp.Models;

namespace WrtSharp
{
  public class UbusRPC
  {
    public string baseUrl;
    private HttpClient client;
    public string sessionId;

    public UbusRPC()
    {
      client = new HttpClient();
      client.Timeout = TimeSpan.FromSeconds(5);
    }

    protected async Task<T> Invoke<T>(string methodName, params object[] parameters)
    {
        var request = new JsonRPCRequest{
          id = Guid.NewGuid().ToString(),
          method = methodName,
          @params = parameters
        };
        try
        {
          var jsonstring = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(request));
          var content = new StringContent(jsonstring);
          content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          HttpResponseMessage res = await client.PostAsync(new Uri(baseUrl), content);
          String result_data = await res.Content.ReadAsStringAsync();
          var results = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(result_data));
          return results;
        }
        catch (Exception e)
        {
          Debug.WriteLine("Error: " + e.ToString());
        }
        return default(T);
    }
    public async Task<UbusRPCResponse<T>> Call<T>(string @object, string @function, object parameters)
    {
        const string methodName = "call";
        UbusRPCResponse<T> result = await Invoke<UbusRPCResponse<T>>(methodName, sessionId, @object, @function, parameters);
        return result;
    }
    public async Task<string[]> List(string matches)
    {
        const string methodName = "list";
        var items = await Invoke<JSONRPCResponse<Dictionary<string, object>>>(methodName, sessionId, matches);
        return items.result.Keys.ToArray();
    }
  }
}
