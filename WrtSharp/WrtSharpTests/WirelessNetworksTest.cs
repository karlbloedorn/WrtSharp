using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrtSharp;
using System.IO;
using System.Linq;
using WrtSharp.Models;
using System.Collections.Generic;

namespace WrtSharpTests
{
  [TestClass]
  public class WirelessNetworksTest
  {
    [TestMethod]
    public void TestNearby()
    {
      var expectedNetwork = new Network{
          bssid = "C0:3F:0E:7F:15:09",
          channel = 5,
          mode = "Master",
          quality = 33,
          quality_max = 70,
          signal = -77,
          ssid = "das Internet"
      };

      string json  = File.ReadAllText("TestAssets/wirelessnetworks.json");
      var res = WirelessNetworks.Nearby(json);

      Assert.IsNotNull(res, "Result object was null");
      Assert.IsTrue(res.results.Any(), "There were no results");
      Assert.IsTrue(res.results.Any(x => PublicInstancePropertiesEqual(x, expectedNetwork, "encryption")),"das Internet was not found or did not match");
   
    }

    public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
    {
      if (self != null && to != null)
      {
        Type type = typeof(T);
        List<string> ignoreList = new List<string>(ignore);
        foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
          if (!ignoreList.Contains(pi.Name))
          {
            object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
            object toValue = type.GetProperty(pi.Name).GetValue(to, null);

            if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
            {
              return false;
            }
          }
        }
        return true;
      }
      return self == to;
    }
  }
}
