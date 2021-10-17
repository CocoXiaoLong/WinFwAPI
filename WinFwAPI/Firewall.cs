using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WinFwAPI
{
    public static class Firewall
    {
        /// <summary>
        /// Indicates if Firewall-Class was initialized
        /// </summary>
        private static bool inited = false;

        /// <summary>
        /// The Windows Firewall Policy Object
        /// </summary>
        private static INetFwPolicy2 policy;

        /// <summary>
        /// Gets the current firewall rules
        /// </summary>
        /// <returns>An Array of the Rules</returns>
        public static FirewallRule[] GetRules()
        {
            TryInit();
            return GetAPIRules().Select(i => FirewallRule.FromFwAPI(i)).ToArray();
        }
        public static INetFwRule[] GetAPIRules()
        {
            TryInit();
            return policy.Rules.Cast<INetFwRule>().ToArray();
        }


        /// <param name="name"></param>
        /// <returns>true if rule with that name exists, otherwise false</returns>
        public static bool HasRule(string name)
        {
            TryInit();
            return policy.Rules.Cast<INetFwRule>().Any(i => i.Name == name);
        }

        /// <param name="name">the name of the Rule</param>
        /// <returns>Rule with that name</returns>
        /// <exception cref="FirewallException"/>
        public static FirewallRule GetRule(string name)
        {
            TryInit();
            try
            {
                return policy.Rules.Cast<INetFwRule>().Select(i => FirewallRule.FromFwAPI(i)).Single(i => i.Name == name);
            }
            catch (Exception)
            {
                throw new FirewallException($"'{name}' was not found");
            }
        }


        private static void TryInit()
        {
            if (!inited) InitializeController();
        }
        /// <exception cref="FirewallException"/>
        private static void InitializeController()
        {
            // Getting FirewallPolicy-Object
            policy = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2")) as INetFwPolicy2;
            if (policy == null) throw new FirewallException("Firewall konnte nicht geöffnet werden");
        }

        internal static INetFwRule CreateNew()
        {
            try
            {
                Type type = Type.GetTypeFromProgID("HNetCfg.FWRule");
                if (type == null) throw new ArgumentNullException("RuleType", "Initialization gave null back");
                return Activator.CreateInstance(type) as INetFwRule;
            }
            catch (Exception err)
            {
                throw new FirewallException("Error while initializing RuleType", err);
            }
        }


        /// <summary>
        /// Adds a rule to the local firewall, provided that there isn't a rule with the same name
        /// </summary>
        /// <param name="rule">The Rule that's being added</param>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="FirewallException"/>
        [SecurityCritical]
        public static void AddRule(FirewallRule rule)
        {
            if (!IsAdmin()) throw new UnauthorizedAccessException("You need to be Admin to add firewall rules");
            if (HasRule(rule.Name)) throw new FirewallException("Rule with that name already exists, consider using 'Firewall.UpdateRule(FirewallRule)'");
            try
            {
                policy.Rules.Add(rule.ToAPIRule());
            }
            catch (Exception err)
            {
                throw new FirewallException("An error occoured while adding the rule", err);
            }
        }

        /// <summary>
        /// Adds a rule to the local firewall, provided that there is a rule with that name
        /// </summary>
        /// <param name="rule">The Rule that's being added</param>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="FirewallException"/>
        [SecurityCritical]
        public static void UpdateRule(FirewallRule rule)
        {
            if (!IsAdmin()) throw new UnauthorizedAccessException("You need to be Admin to add firewall rules");
            if (!HasRule(rule.Name)) throw new FirewallException("Rule with that name doesn't exists, consider using 'Firewall.AddRule(FirewallRule)'");

            var backup = FirewallRule.FromFwAPI(policy.Rules.Item(rule.Name));

            try
            {
                policy.Rules.Remove(rule.Name);
                policy.Rules.Add(rule.ToAPIRule());
            }
            catch (Exception err)
            {
                if (HasRule(rule.Name))
                    policy.Rules.Remove(rule.Name);

                policy.Rules.Add(backup.ToAPIRule());
                throw new FirewallException("There was an Error updating the Rule", err);
            }
        }

        /// <summary>
        /// This resets the Firewall to defaults
        /// </summary>
        [SecurityCritical]
        public static void Reset()
        {
            if (IsAdmin()) policy.RestoreLocalFirewallDefaults();
            else throw new UnauthorizedAccessException("You need to be Admin to reset the firewall");
        }


        /// <returns>Is the user admin?</returns>
        private static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        
    }
}
