using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrtSharp.Models
{
  public class WirelessNetworkResponse
  {
    public List<Network> results { get; set; }
  }
  public class Encryption
  {
    public bool enabled { get; set; }
    public List<int> wpa { get; set; }
    public List<string> authentication { get; set; }
    public List<string> ciphers { get; set; }
  }

  public class Network
  {
    public string ssid { get; set; }
    public string bssid { get; set; }
    public string mode { get; set; }
    public int channel { get; set; }
    public int signal { get; set; }
    public int quality { get; set; }
    public int quality_max { get; set; }
    public Encryption encryption { get; set; }
  }
}
