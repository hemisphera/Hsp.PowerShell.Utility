using System.Collections.Generic;
using System.Management.Automation;
using System.Net;

namespace Hsp.PowerShell.Utility
{

  public class NullInterface : IPsUserInterface
  {

    private static NullInterface _instance;

    public static NullInterface Instance => _instance ?? (_instance = new NullInterface());


    private NullInterface()
    {
    }


    public void WriteError(string message, object context)
    {
    }

    public void WriteDebug(string message)
    {
    }

    public void WriteVerbose(string message)
    {
    }

    public void WriteWarning(string message)
    {
    }

    public void WriteMessage(string message)
    {
    }

    public void ReportProgress(long id, ProgressRecord record)
    {
    }

    public PsChoiceItem Prompt(string caption, string message, IEnumerable<PsChoiceItem> items, int defaultIndex)
    {
      return null;
    }

    public NetworkCredential PromptForCredential(string caption, string message, string userName, string targetName)
    {
      return null;
    }

  }

}