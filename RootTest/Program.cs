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
            var exiRule = Firewall.GetAPIRules().First();
            var cRule = new FirewallRule
            {
                Name = "OneRule",
                Action = FirewallAction.BLOCK,
                ApplicationName = @"",
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
            var newRule = cRule.ToAPIRule();

            Firewall.AddRule(cRule);

            Console.ReadKey();
        }
    }
}
