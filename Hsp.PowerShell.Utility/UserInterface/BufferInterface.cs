using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;

namespace Hsp.PowerShell.Utility
{

  /// <summary>
  /// A logger that captures all output into lists and provides methods to dump them elsewhere
  /// </summary>
  public class BufferInterface : IPsUserInterface
  {

    public List<PsStreamEntry> Entries { get; }

    public IPsUserInterface OutputLogger { get; }


    public bool ThrowAllErrors { get; set; }


    public BufferInterface(IPsUserInterface outputLogger)
    {
      ThrowAllErrors = true;
      Entries = new List<PsStreamEntry>();
      OutputLogger = outputLogger;
    }


    public void WriteError(string message, object context)
    {
      Entries.Add(new PsStreamEntry(message, PsStreamEntryType.Error, context));
    }

    public void WriteDebug(string message)
    {
      Entries.Add(new PsStreamEntry(message, PsStreamEntryType.Verbose));
    }

    public void WriteVerbose(string message)
    {
      Entries.Add(new PsStreamEntry(message, PsStreamEntryType.Verbose));
    }

    public void WriteWarning(string message)
    {
      Entries.Add(new PsStreamEntry(message, PsStreamEntryType.Warning));
    }

    public void WriteMessage(string message)
    {
      Entries.Add(new PsStreamEntry(message));
    }

    public void ReportProgress(long id, ProgressRecord record)
    {
    }

    public PsChoiceItem Prompt(string caption, string message, IEnumerable<PsChoiceItem> items, int defaultIndex)
    {
      throw new NotSupportedException();
    }
    
    public NetworkCredential PromptForCredential(string caption, string message, string userName, string targetName)
    {
      throw new NotSupportedException();
    }

    
    public void Clear()
    {
      Entries.Clear();
    }

    public void Dump(IPsUserInterface otherLogger = null)
    {
      if (otherLogger == null)
        otherLogger = OutputLogger;

      foreach (var entry in Entries.OrderBy(e => e.Timestamp))
        otherLogger.WriteEntry(entry);

      if (ThrowAllErrors)
        ThrowErrors();
      Clear();
    }

    public void ThrowErrors()
    {
      var errors = Entries
        .Where(e => e.Type == PsStreamEntryType.Error)
        .Select(e => e.Message)
        .ToArray();
      if (errors.Any())
        throw new Exception(String.Join(Environment.NewLine, errors));
    }

  }

}