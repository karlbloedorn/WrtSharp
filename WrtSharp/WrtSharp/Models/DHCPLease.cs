using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WrtSharp.Models
{
    public class DHCPLease
    {
        public DHCPLease(Double expires, string ip, string mac, string host, string client_identifier)
        {
            if (expires != 0)
            {
                this.expires = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expires);
            }
            this.host = host  == "*" ? null : host;
            this.client_identifier = client_identifier == "*" ? null : client_identifier;
            this.ip = ip;
            this.mac = mac;
        }
        public readonly DateTime expires;
        public readonly string ip;
        public readonly string mac;
        public readonly string host;
        public readonly string client_identifier;
        public TimeSpan timeLeft
        {
            get
            {
                return expires - DateTime.UtcNow;
            }
        }
    }

    public class UCIDHCPReservationResponse : UCIBlob
    {
        public string name { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
    }
}
