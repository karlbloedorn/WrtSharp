using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
  public class LoginResponse
  {
    public string ubus_rpc_session { get; set; }
    public int timeout { get; set; }
    public int expires { get; set; }
  }
}
