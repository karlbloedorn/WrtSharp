using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrtSharp.Models;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using WrtSharp;
using System.IO;

namespace WrtSharp
{
    public class WirelessNetworks
    {

      public static WirelessNetworkResponse Nearby(string input)
      {
        var res = JsonConvert.DeserializeObject<WirelessNetworkResponse>(input); 
 
        return res;
      }

      public static void Graph(List<Network> networks, Stream graphstream)
      {
        var temp = new PlotModel("Nearby Networks");

        foreach (var result in networks)
        {
          var ls = new LineSeries("" + result.ssid);
          ls.Points.Add(new DataPoint(result.channel-2, -100));

          ls.Points.Add(new DataPoint(result.channel, result.signal));

          ls.Points.Add(new DataPoint(result.channel+2, -100));



          temp.Series.Add(ls);
        }
   
        temp.Axes.Add(new LinearAxis(AxisPosition.Left, -100, -30));
        temp.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0,15));

          OxyPlot.PdfExporter.Export(temp, graphstream,1200, 800);
        
     }
    }
}
