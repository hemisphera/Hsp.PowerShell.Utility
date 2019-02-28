using System;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation.Host;
using System.Threading;

namespace Hsp.PowerShell.Utility
{

  public class SimplePsHost : PSHost
  {

    public override string Name { get; }

    public override Version Version
    {
      get
      {
        var fvi = FileVersionInfo.GetVersionInfo(typeof(SimplePsHost).Assembly.Location);
        return new Version(fvi.FileVersion);
      }
    }

    public override Guid InstanceId { get; }

    public override PSHostUserInterface UI { get; }

    public override CultureInfo CurrentCulture => Thread.CurrentThread.CurrentCulture;

    public override CultureInfo CurrentUICulture => Thread.CurrentThread.CurrentUICulture;

    public IPsUserInterface PsUserInterface { get; }


    public SimplePsHost(IPsUserInterface psUserInterface, string name = "")
    {
      if (String.IsNullOrEmpty(name))
        name = nameof(SimplePsHost);
      Name = name;
      PsUserInterface = psUserInterface;
      InstanceId = Guid.NewGuid();
      UI = new SimplePsHostUserInterface(psUserInterface);
    }


    public override void SetShouldExit(int exitCode)
    {
    }

    public override void EnterNestedPrompt()
    {
    }

    public override void ExitNestedPrompt()
    {
    }

    public override void NotifyBeginApplication()
    {
    }

    public override void NotifyEndApplication()
    {
    }

  }

}