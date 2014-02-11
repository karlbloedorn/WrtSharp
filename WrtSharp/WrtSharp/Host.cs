using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrtSharp.Models;

namespace WrtSharp
{
    public class Host
    {
        private string endpoint;
        private string session;
        private UbusRPC rpc;
        public Host(string endpoint)
        {
            this.endpoint = endpoint;
            rpc = new UbusRPC();
            rpc.baseUrl = endpoint;
            session = string.Concat(Enumerable.Repeat("0", 32));
            rpc.sessionId = session;
        }

        public async Task<string[]> NetworkInterfaces()
        {
            var x = await rpc.List("network.interface.*");
            Debug.WriteLine(x);
            return x;
        }

        public async Task<string[]> WirelessAps()
        {
            var x = await rpc.List("hostapd.*");
            Debug.WriteLine(x);
            return x;     
        }

        public async Task<bool> Login(string username, string password)
        {

            var res = await rpc.Call<LoginResponse>(
                                      "session",
                                      "login",
                                      new { username = username, password = password }
                                      );

            switch (res.code)
            {
                case 0:
                    break;
                case 6:
                default:
                    session = null;
                    return false;
            }
            session = res.result.ubus_rpc_session;
            rpc.sessionId = session;
            return true;
        }

        public async Task<SystemBoardResponse> BoardInfo()
        {
            var res = await rpc.Call<SystemBoardResponse>("system", "board", new { });
            return res.result;
        }

        public async Task<List<DHCPLease>> DevicesInfo()
        {
            string leasesString = null;
            var leasefile = await rpc.Call<UCIResponseSingle<string>>("uci", "get", new
            {
                config = "dhcp",
                section = "@dnsmasq[0]",
                option = "leasefile"
            });
            var res = await rpc.Call<FileResult>("file", "read", new { path = leasefile.result.value, data = new { } });
            if (res.code == 0)
            {
                leasesString = res.result.data;
            }
            else
            {
                throw new Exception("Failed to read lease file");
            }
            var lines = leasesString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var leases = lines.Select(x =>
            {
                var items = x.Split(' ');
                return new DHCPLease
                (
                   expires: Double.Parse(items[0]),
                   mac: items[1],
                   ip: items[2],
                   host: items[3],
                   client_identifier: items[4]
                );
            }).ToList();

            var staticLeasesResponse = await rpc.Call<UCIResponseMultiple<Dictionary<string, UCIDHCPReservationResponse>>>("uci", "get", new {config= "dhcp", type= "host"});

            var staticReservations = staticLeasesResponse.result.values.Values.ToList().Select(x =>
            {
                return new DHCPLease
                (
                    expires: 0,
                    mac: x.mac,
                    ip: x.ip,
                    host: x.name,
                    client_identifier: "*"
                );
            });

            leases.AddRange(staticReservations);
            return leases;
        }

        public async Task<SystemInfoResponse> SystemInfo()
        {
            var res = await rpc.Call<SystemInfoResponse>("system", "info", new {});
            return res.result;
        }



    }
}
