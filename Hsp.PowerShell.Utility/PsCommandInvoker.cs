using System.Collections;
using System.Linq;
using System.Management.Automation;

namespace Hsp.PowerShell.Utility
{

  public class PsCommandInvoker
  {

    public static PSObject[] InvokeCommand(string commandName, Hashtable parameters)
    {
      var sb = ScriptBlock.Create("param($Command, $Params) & $Command @Params");
      return sb.Invoke(commandName, parameters).ToArray();
    }

    public static PSObject[] InvokeCommand<T>(Hashtable parameters) where T : Cmdlet
    {
      return InvokeCommand(Extensions.GetCmdletName<T>(), parameters);
    }

    public static PsCommandInvoker Create(string cmdletName)
    {
      return new PsCommandInvoker(cmdletName);
    }

    public static PsCommandInvoker Create<T>() where T : Cmdlet
    {
      return new PsCommandInvoker(Extensions.GetCmdletName<T>());
    }


    private Hashtable Parameters { get; }


    public string CmdletName { get; }

    public PSObject[] LastResult { get; private set; }


    private PsCommandInvoker(string cmdletName)
    {
      CmdletName = cmdletName;
      Parameters = new Hashtable();
    }


    public PsCommandInvoker AddArgument(string name, object value)
    {
      Parameters.Add(name, value);
      return this;
    }

    public PsCommandInvoker AddArgument(string name)
    {
      Parameters.Add(name, null);
      return this;
    }

    public PsCommandInvoker AddArgumentIf(bool expression, string name)
    {
      return !expression ? this : AddArgument(name);
    }

    public PsCommandInvoker AddArgumentIf(bool expression, string name, object value)
    {
      return !expression ? this : AddArgument(name, value);
    }

    public PSObject[] Invoke()
    {
      LastResult = InvokeCommand(CmdletName, Parameters);
      Parameters.Clear();
      return LastResult;
    }

  }
}
