using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;

namespace Hsp.PowerShell.Utility
{

  internal class SimplePsHostUserInterface : PSHostUserInterface
  {

    public override PSHostRawUserInterface RawUI { get; } = new SimplePsHostRawUserInterface();

    public IPsUserInterface Ui { get; }


    public SimplePsHostUserInterface(IPsUserInterface ui)
    {
      Ui = ui;
    }


    public override string ReadLine()
    {
      return "";
    }

    public override SecureString ReadLineAsSecureString()
    {
      return new SecureString();
    }

    public override void Write(string value)
    {
      if (!String.IsNullOrEmpty(value))
        Ui.WriteMessage(value);
    }

    public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
    {
      if (!String.IsNullOrEmpty(value))
        Ui.WriteMessage(value);
    }

    public override void WriteLine(string value)
    {
      if (!String.IsNullOrEmpty(value))
        Ui.WriteMessage(value);
    }

    public override void WriteErrorLine(string value)
    {
    }

    public override void WriteDebugLine(string message)
    {
    }

    public override void WriteProgress(long sourceId, ProgressRecord record)
    {
    }

    public override void WriteVerboseLine(string message)
    {
    }

    public override void WriteWarningLine(string message)
    {
    }

    public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
    {
      throw new NotSupportedException();
    }

    public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
    {
      var networkCreds = Ui.PromptForCredential(caption, message, userName, targetName);
      return networkCreds == null 
        ? null 
        : new PSCredential(networkCreds.UserName, networkCreds.SecurePassword);
    }

    public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
    {
      return PromptForCredential(caption, message, userName, targetName);
    }

    public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
    {
      var items = choices?.Select((c, idx) =>
      {
        var isDefaultChoice = idx == defaultChoice;
        var item = new PsChoiceItem(idx, c.Label, isDefaultChoice);
        return item;
      }) ?? new PsChoiceItem[] { };

      var result = Ui.Prompt(caption, message, items, defaultChoice);
      return result?.Index ?? defaultChoice;
    }

  }

}