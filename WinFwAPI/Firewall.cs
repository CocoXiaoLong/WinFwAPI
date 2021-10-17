using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFwAPI
{
    public static class Firewall
    {
        private static bool inited = false;
        private static INetFwPolicy2 policy;
        public static FirewallRule[] GetRules()
        {
            TryInit();
            return policy.Rules.Cast<INetFwRule>().Select(i => FirewallRule.FromFwAPI(i)).ToArray();
        }

        public static bool HasRule(string name)
        {
            TryInit();
            return policy.Rules.Cast<INetFwRule>().Any(i => i.Name == name);
        }

        public static FirewallRule GetRule(string name)
        {
            TryInit();
            return policy.Rules.Cast<INetFwRule>().Select(i => FirewallRule.FromFwAPI(i)).Single(i => i.Name == name);
        }



        private static void TryInit()
        {
            if (!inited) InitializeController();
        }
        private static void InitializeController()
        {
            policy = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2")) as INetFwPolicy2;
            if (policy == null) throw new FirewallException("Firewall konnte nicht geöffnet werden");
        }
    }
}
