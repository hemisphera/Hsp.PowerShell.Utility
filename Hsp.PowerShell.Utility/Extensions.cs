using System;
using System.Management.Automation;
using System.Reflection;

namespace Hsp.PowerShell.Utility
{

  public static class Extensions
  {

    public static string GetExceptionMessage(this Exception ex)
    {
      if (ex is AggregateException)
        return String.Join(Environment.NewLine, ex.GetExceptionMessage());
      if (ex.InnerException != null)
        return String.Join(Environment.NewLine, ex.Message, ex.InnerException.GetExceptionMessage());
      return ex.Message;
    }

    public static void WriteEntry(this IPsUserInterface logger, PsStreamEntry entry)
    {
      switch (entry.Type)
      {
        case PsStreamEntryType.Information:
          logger.WriteMessage(entry.Message);
          break;
        case PsStreamEntryType.Warning:
          logger.WriteWarning(entry.Message);
          break;
        case PsStreamEntryType.Error:
          logger.WriteError(entry.Message, null);
          break;
        case PsStreamEntryType.Verbose:
          logger.WriteVerbose(entry.Message);
          break;
        case PsStreamEntryType.Debug:
          logger.WriteDebug(entry.Message);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static string GetCmdletName<T>() where T : Cmdlet
    {
      var attr = typeof(T).GetCustomAttribute<CmdletAttribute>();
      return attr == null ? null : $"{attr.VerbName}-{attr.NounName}";
    }

  }

}
