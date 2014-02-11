using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrtSharp.Models;

namespace WrtSharp
{
  public class Host
  {
    private string endpoint;
    private string session;
    private JsonRPC rpc;
    public Host(string endpoint)
    {
      this.endpoint = endpoint;
      rpc = new JsonRPC();
      rpc.baseUrl = endpoint;
    }

    public async Task<bool> Login(string username, string password)
    {

      var res =  await rpc.Invoke<LoginResponse>(
                                "call",
                                string.Concat(Enumerable.Repeat("0", 32)),
                                "session",
                                "login",
                                new { username = username, password = password }
                                );

      switch (res.code)
      {
        case 0:
          break;
        case 6:
        default:
          session = null;
          return false;
      }
      session = res.result.ubus_rpc_session;
      return true;
    }

    public async Task<SystemBoardResponse> BoardInfo()
    {
      var res = await rpc.Invoke<SystemBoardResponse>( "call", session, "system", "board", new { } );
      return res.result;
    }

    public async Task<SystemInfoResponse> SystemInfo()
    {
      var res = await rpc.Invoke<SystemInfoResponse>("call", session, "system", "info", new { });
      return res.result;
    }



  }
}
