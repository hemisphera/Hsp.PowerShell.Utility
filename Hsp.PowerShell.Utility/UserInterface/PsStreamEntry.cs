using System;

namespace Hsp.PowerShell.Utility
{

  public class PsStreamEntry
  {

    public PsStreamEntryType Type { get; }

    public string Message { get; }

    public object Context { get; }

    public DateTime Timestamp { get; }


    public PsStreamEntry(string message, PsStreamEntryType type = PsStreamEntryType.Information, object context = null)
    {
      Message = message;
      Type = type;
      Context = context;
      Timestamp = DateTime.Now;
    }

  }

}