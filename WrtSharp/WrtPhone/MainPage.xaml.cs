using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WrtPhone.Resources;
using System.Diagnostics;
using WrtSharp;
using System.Threading.Tasks;
using WrtSharp.Models;

namespace WrtPhone
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();

      // Sample code to localize the ApplicationBar
      //BuildLocalizedApplicationBar();
    }

    private async void ConnectButtonTapped(object sender, System.Windows.Input.GestureEventArgs e)
    {
      Debug.WriteLine("" + usernameField.Text  + " " + passwordField.Password + " " + hostnameField.Text + " " + rememberField.IsChecked );
      var host = new Host("http://"+hostnameField.Text + "/ubus");

      bool success =  await host.Login(usernameField.Text, passwordField.Password);
      if (success)
      {
        var start = DateTime.UtcNow;
        var tasks = new List<Task< List<DHCPLease>>>();
        int count = 10;
        for( int i = 0; i < count; i++){
          var d = host.DevicesInfo();
          tasks.Add(d);
        }
        await Task.WhenAll(tasks.ToArray());

        var result = tasks[0].Result;

        var end = DateTime.UtcNow;
        double ms =  (end - start).TotalMilliseconds;
        Debug.WriteLine("count: " + count + ", total "  + ms + " ms, average " + ms/(count*1.0) );
      }
      else
      {
        MessageBox.Show("failed to login" + " took: ");
      }
    }
  }
}