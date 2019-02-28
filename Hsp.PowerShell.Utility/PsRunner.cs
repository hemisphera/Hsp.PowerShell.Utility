using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace Hsp.PowerShell.Utility
{

  public class PsRunner : IDisposable
  {

    private SimplePsHost Host { get; }

    private System.Management.Automation.PowerShell PsInstance { get; }

    protected IPsUserInterface Logger { get; set; }

    public bool DiscardOutput { get; set; }

    public bool AutoDumpBufferLogger { get; set; }

    public bool VerboseEnabled { get; private set; }


    public event EventHandler<PSInvocationStateChangedEventArgs> InvocationStateChanged;


    public event ErrorEventHandler ErrorOccurred;


    public PsRunner(IPsUserInterface psUserInterface)
    {
      Logger = psUserInterface;
      DiscardOutput = false;

      Host = new SimplePsHost(psUserInterface);
      var rs = RunspaceFactory.CreateRunspace(Host);
      PsInstance = System.Management.Automation.PowerShell.Create();
      PsInstance.Runspace = rs;
      PsInstance.InvocationStateChanged += (s, e) => InvocationStateChanged?.Invoke(this, e);
      rs.Open();

      PsInstance.Streams.Debug.DataAdded += delegate(object s, DataAddedEventArgs e)
      {
        Logger.WriteDebug(PsInstance.Streams.Debug[e.Index].Message);
      };
      PsInstance.Streams.Error.DataAdded += delegate (object s, DataAddedEventArgs e)
      {
        var er = PsInstance.Streams.Error[e.Index];
        Logger.WriteError(er.Exception.GetExceptionMessage(), er.TargetObject);
      };
      PsInstance.Streams.Progress.DataAdded += delegate (object s, DataAddedEventArgs e)
      {
        var pr = PsInstance.Streams.Progress[e.Index];
        Logger.ReportProgress(pr.ActivityId, pr);
      };
      PsInstance.Streams.Verbose.DataAdded += delegate (object s, DataAddedEventArgs e)
      {
        Logger.WriteVerbose(PsInstance.Streams.Verbose[e.Index].Message);
      };
      PsInstance.Streams.Warning.DataAdded += delegate (object s, DataAddedEventArgs e)
      {
        Logger.WriteWarning(PsInstance.Streams.Warning[e.Index].Message);
      };
    }


    public void ImportModule(string path, bool disableNameCheck = true)
    {
      StartPipeline()
        .AddCommand("Import-Module")
        .AddParameter("Name", path)
        .AddParameterIf(disableNameCheck, "DisableNameChecking")
        .Invoke();
    }

    public void RegisterVariable(string name, object value)
    {
      PsInstance
        .AddCommand("Set-Variable")
        .AddParameter("Name", name)
        .AddParameter("Value", value);
      Invoke();
    }


    public PsRunner StartPipeline()
    {
      PsInstance.Commands.Clear();
      return this;
    }

    public PsRunner AddCommand(string cmdletName)
    {
      PsInstance.AddCommand(cmdletName);
      return this;
    }

    public PsRunner AddCommand<T>() where T: Cmdlet
    {
      var cmdletName = Extensions.GetCmdletName<T>();
      return AddCommand(cmdletName);
    }

    public PsRunner AddParameter(string parameterName, object value)
    {
      PsInstance.AddParameter(parameterName, value);
      return this;
    }

    public PsRunner AddParameter(string parameterName)
    {
      PsInstance.AddParameter(parameterName);
      return this;
    }

    public PsRunner AddParameterIf(bool expression, string parameterName, object value)
    {
      if (expression)
        PsInstance.AddParameter(parameterName, value);
      return this;
    }

    public PsRunner AddParameterIf(bool expression, string parameterName)
    {
      if (expression)
        PsInstance.AddParameter(parameterName);
      return this;
    }
    
    public void AddScript(string script)
    {
      PsInstance.AddScript(script);
    }


    public void SetVerboseEnabled(bool isEnabled)
    {
      StartPipeline()
        .AddCommand("Set-Variable")
        .AddParameter("Name", "VerbosePreference")
        .AddParameter("Value", isEnabled ? "Continue" : "SilentlyContinue")
        .Invoke();
      VerboseEnabled = isEnabled;
    }

    public void Abort()
    {
      PsInstance.Stop();
    }


    public async Task<Collection<PSObject>> InvokeAsync()
    {
      return await Task.Run(() => Invoke());
    }

    public Collection<PSObject> Invoke(bool discardOutput = false)
    {
      discardOutput = discardOutput || DiscardOutput;
      if (discardOutput) 
        PsInstance.AddCommand("Out-Null");

      Collection<PSObject> result;
      try
      {
        result = PsInstance.Invoke();
      }
      catch (Exception e)
      {
        Logger.WriteError(e.GetExceptionMessage(), null);
        ErrorOccurred?.Invoke(this, new ErrorEventArgs(e));
        throw;
      }
      finally
      {
        PsInstance.Commands.Clear();
      }

      if (AutoDumpBufferLogger && Logger is BufferInterface bl)
        bl.Dump();

      return result;
    }

    public IEnumerable<T> Invoke<T>(bool discardOutput = false)
    {
      var result = Invoke(discardOutput);
      return result.Select(r => (T) r.BaseObject).ToArray();
    }


    public void Dispose()
    {
      PsInstance?.Dispose();
    }

  }

}