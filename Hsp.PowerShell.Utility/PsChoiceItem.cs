namespace Hsp.PowerShell.Utility
{

  public class PsChoiceItem
  {

    public int Index { get; }

    public string Label { get; }

    public bool DefaultChoice { get; }


    public PsChoiceItem(int index, string label, bool defaultChoice = false)
    {
      Index = index;
      Label = label;
      DefaultChoice = defaultChoice;
    }

  }

}
