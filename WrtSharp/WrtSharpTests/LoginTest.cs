using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WrtSharp;
using System.Threading.Tasks;
using WrtSharp.Models;

namespace WrtSharpTests
{
  /// <summary>
  /// Summary description for HostTest
  /// </summary>
  public class LoginTestHelper
  {
    protected const string endpoint = "http://10.8.0.147/ubus";
    protected Host testHost;
    public string username = "root";
    public string password = "";


    public virtual void Initialize()
    {
      testHost = new Host(endpoint);
    }

    public bool Login(string test_username = null, string test_password = null)
    {
      if (test_password == null)
      {
        test_password = password;
      }
      if (test_username == null)
      {
        test_username = username;
      }
      Task<bool> task = testHost.Login(test_username, test_password);
      task.Wait();
      return task.Result;
    }
  }

  [TestClass]
  public class LoginTest : LoginTestHelper
  {
    [TestInitialize()]
    public override void Initialize()
    {
      base.Initialize();
    }

    [TestMethod]
    public void LoginSuccess()
    {
      Assert.IsTrue(Login());
    }

    [TestMethod]
    public void LoginFail()
    {
      Assert.IsFalse(Login(username, password + "bad_wolf"));
      Assert.IsFalse(Login(username + "bad_wolf", password));
    }
  }

  [TestClass] 
  public class HostTest : LoginTestHelper
  {

    [TestInitialize()]
    public override void Initialize()
    {
      base.Initialize();
      Assert.IsTrue(Login());
    }

    [TestMethod]
    public void BoardInfo()
    {
     
      Task<SystemBoardResponse> boardTask = testHost.BoardInfo();
      boardTask.Wait();
      var info = boardTask.Result;
      Assert.IsNotNull(info);
    }

    [TestMethod]
    public void SystemInfo()
    {
      Task<SystemInfoResponse> infoTask = testHost.SystemInfo();
      infoTask.Wait();
      var info = infoTask.Result;
      Assert.IsNotNull(info);
    }
  }
}
