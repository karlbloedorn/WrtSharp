using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrtSharp.Models
{
  public class Memory
  {
    public int total { get; set; }
    public int free { get; set; }
    public int shared { get; set; }
    public int buffered { get; set; }
  }

  public class Swap
  {
    public int total { get; set; }
    public int free { get; set; }
  }

  public class SystemInfoResponse
  {
    public int uptime { get; set; }
    public int localtime { get; set; }
    public List<int> load { get; set; }
    public Memory memory { get; set; }
    public Swap swap { get; set; }
  }
}
