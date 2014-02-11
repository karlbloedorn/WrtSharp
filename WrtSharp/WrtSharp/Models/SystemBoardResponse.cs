using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
  public class SystemBoardResponse
  {
    public string kernel { get; set; }
    public string hostname { get; set; }
    public string system { get; set; }
    public Release release { get; set; }
  }
  public class Release
  {
    public string distribution { get; set; }
    public string version { get; set; }
    public string revision { get; set; }
    public string codename { get; set; }
    public string target { get; set; }
    public string description { get; set; }
  }
}
