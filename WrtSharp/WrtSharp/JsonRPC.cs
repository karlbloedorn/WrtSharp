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
  public class JsonRPC
  {
    public string baseUrl;
    private HttpClient client;

    public JsonRPC()
    {
      client = new HttpClient();
      client.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<JsonRPCResponse<T>> Invoke<T>(string methodName, params object[] parameters)
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
          var results = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<JsonRPCResponse<T>>(result_data));
          return results;
        }
        catch (Exception e)
        {
          Debug.WriteLine("Error: " + e.ToString());
        }
        return null;
    }
  }
}
