using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFwAPI;

namespace RootTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cRule = new FirewallRule
            {
                Name = "OneRule",
                Action = FirewallAction.BLOCK,
                ApplicationName = @"C:\DevResources\MicrosoftEdgeSetup.exe",
                Description = "Block connections",
                Direction = FirewallDirection.IN,
                Grouping = "OneWay",
                Enabled = true,
                LocalAddresses = "*",
                LocalPorts = "*",
                Protocol = FirewallProtocol.TCP,
                RemoteAddresses = "*",
                RemotePorts = "*"
            };

            Firewall.AddRule(cRule);

            Console.ReadKey();
        }
    }
}
