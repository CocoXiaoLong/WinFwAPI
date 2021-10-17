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
            foreach (var rule in Firewall.GetRules())
            {
                Console.Write($"{rule.Action} {rule.Name} ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{rule.Direction}|{rule.Protocol}]");
                Console.ResetColor();
            }
            Console.ReadKey();
        }
    }
}
