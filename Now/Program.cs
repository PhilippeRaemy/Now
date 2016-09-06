using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Now
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if(args.Any(a=>a=="/?" || a== "-?"))
            {
                Console.WriteLine("Usage is now [--pipe] [--fmt=yyyy-MM-dd] \"message text\" where fmt is any valid dotnet DateTime format string and --pipe indicates to read from sysin.");
                return;
            }
            var fmt = args
                .Where(a => a.StartsWith("--fmt=", StringComparison.InvariantCultureIgnoreCase))
                .Select(a => a.Split(new[] {'='}, 2).Skip(1).FirstOrDefault())
                .FirstOrDefault()
                      ?? "yyyy-MM-dd HH:mm:ss";

            var msg=new StringBuilder();
            var first = true;
            var re=new Regex(@"^--\w+=?",RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            foreach (var s in args
                .Where(a => !re.Match(a).Success)
            )
            {
                msg.Append(first ? " -- " : " ");
                first = false;
                msg.Append(s);
            }
            
            if(args.Any(a => a.Equals("--pipe", StringComparison.InvariantCultureIgnoreCase)))
            {
                string line;
                while ((line=Console.In.ReadLine())!=null)
                {
                    Console.WriteLine($"{DateTime.Now.ToString(fmt)}{msg} -- {line}");
                }
            }
            else Console.WriteLine($"{DateTime.Now.ToString(fmt)}{msg}");
        }
    }
}
