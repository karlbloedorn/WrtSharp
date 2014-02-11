using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WrtSharp;
using System.Threading.Tasks;
using WrtSharp.Models;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace WrtSharpTests
{
    /// <summary>
    /// Summary description for HostTest
    /// </summary>
    public class LoginTestHelper
    {
        protected Host testHost;
        public static string username;
        public static string password;
        public static string endpoint;


        public virtual void Initialize()
        {
            testHost = new Host(endpoint);
        }

        public static Dictionary<string, string> loadConfig()
        {
            Dictionary<string, string> items;
            using (StreamReader r = new StreamReader("TestAssets/credentials.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            return items;
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
 
        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            var config = loadConfig();
            username = config["username"];
            password = config["password"];
            endpoint = config["endpoint"];
        }

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
        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            var config = loadConfig();
            username = config["username"];
            password = config["password"];
            endpoint = config["endpoint"];
        }

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

        [TestMethod]
        public void DeviceInfo()
        {
            var deviceInfoTask = testHost.DevicesInfo();
            deviceInfoTask.Wait();
            Debug.WriteLine(deviceInfoTask.Result.ToList());
        }
    }
}
