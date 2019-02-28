using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hsp.PowerShell.Utility
{

  public class ConsolePsUserInterface : IPsUserInterface
  {

    private void WriteMessage(string type, string message)
    {
      WriteMessage($"{type.ToUpper()}: {message}");
    }

    public void WriteError(string message, object context)
    {
      WriteMessage("Error", message);
    }

    public void WriteDebug(string message)
    {
      WriteMessage("Debug", message);
    }

    public void WriteVerbose(string message)
    {
      WriteMessage("Verbose", message);
    }

    public void WriteWarning(string message)
    {
      WriteMessage("Warning", message);
    }

    public void WriteMessage(string message)
    {
      Console.WriteLine(message);
    }

    public void ReportProgress(long id, ProgressRecord record)
    {
      // not supported
    }

    public PsChoiceItem Prompt(string caption, string message, IEnumerable<PsChoiceItem> items, int defaultIndex)
    {
      // not supported
      throw new NotSupportedException();
    }

    public NetworkCredential PromptForCredential(string caption, string message, string userName, string targetName)
    {
      // not supported
      throw new NotSupportedException();
    }

  }

}
