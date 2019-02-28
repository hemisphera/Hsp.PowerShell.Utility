using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hsp.PowerShell.Utility.Test
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var psr = new PsRunner(new ConsolePsUserInterface()))
      {
        var files = 
          psr.StartPipeline()
            .AddCommand("Get-ChildItem")
            .AddParameter("Path", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddParameterIf(args.Length > 0, "Filter", args.FirstOrDefault())
            .AddParameter("Verbose", true)
            .Invoke<object>();

        psr.StartPipeline()
          .AddCommand("Copy-Item")
          .AddParameter("Path", files.FirstOrDefault())
          .AddParameter("Destination", files.FirstOrDefault() + ".test")
          .AddParameter("Verbose", true)
          .Invoke<object>(true);

        foreach (var file in files)
        {
          Console.WriteLine(file);
        }
      }

      Console.ReadLine();
    }
  }
}
