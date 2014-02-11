using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Exceptions
{
  public class RPCException : Exception
  {
    public RPCException(string message) : base(message)
    {
    }
  }
}
