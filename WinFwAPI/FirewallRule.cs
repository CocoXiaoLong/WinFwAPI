using NetFwTypeLib;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace WinFwAPI
{
    public class FirewallRule
    {
        private bool import;
        private string name;

        public string Name
        {
            get => name; 
            set
            {
                if (!import) if (Firewall.HasRule(value)) throw new FirewallException("Rule already exists");
                name = value;
            }
        }
        public string Description { get; set; }
        public string ApplicationName { get; set; }
        public string ServiceName { get; set; }
        public FirewallProtocol Protocol { get; set; }
        public string LocalPorts { get; set; }
        public string RemotePorts { get; set; }
        public string LocalAddresses { get; set; }
        public string RemoteAddresses { get; set; }
        public FirewallDirection Direction { get; set; }
        public bool Enabled { get; set; }
        public FirewallAction Action { get; set; }


        public FirewallRule()
        {

        }
        private FirewallRule(bool imported)
        {
            this.import = imported;
        }
        internal static FirewallRule FromFwAPI(INetFwRule rule)
        {
            return new FirewallRule(true)
            {
                Name = rule.Name,
                Description = rule.Description,
                ApplicationName = rule.ApplicationName,
                ServiceName = rule.serviceName,
                Protocol = (FirewallProtocol)rule.Protocol,
                LocalPorts = rule.LocalPorts,
                RemotePorts = rule.RemotePorts,
                LocalAddresses = rule.LocalAddresses,
                RemoteAddresses = rule.RemoteAddresses,
                Direction = (FirewallDirection)rule.Direction,
                Enabled = rule.Enabled,
                Action = (FirewallAction)rule.Action
            };
        }
    }

    public enum FirewallAction
    {
        BLOCK, ALLOW, MAX
    }
    public enum FirewallProtocol
    {
        HOPOPT = 0,
        ICMPv4 = 1,
        IGMP = 2,
        TCP = 6,
        UDP = 17,
        IPv6 = 41,
        IPv6_Route = 43,
        IPv6_Frag = 44,
        GRE = 47,
        ICMPv6 = 58,
        IPv6_NoNxt = 59,
        IPv6_Opts = 60,
        VRRP = 112,
        PGM = 113,
        L2TP = 115,
        Any = 256
    }
    public enum FirewallDirection
    {
        IN = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN,
        OUT = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,
        MAX = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX
    }
}