using System.Collections.Generic;
using System.Management.Automation;
using System.Net;

namespace Hsp.PowerShell.Utility
{

  public interface IPsUserInterface
  {

    void WriteError(string message, object context);

    void WriteDebug(string message);

    void WriteVerbose(string message);

    void WriteWarning(string message);

    void WriteMessage(string message);

    void ReportProgress(long id, ProgressRecord record);
    
    PsChoiceItem Prompt(string caption, string message, IEnumerable<PsChoiceItem> items, int defaultIndex);

    NetworkCredential PromptForCredential(string caption, string message, string userName, string targetName);

  }

}