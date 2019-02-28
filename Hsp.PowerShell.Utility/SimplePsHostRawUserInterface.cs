using System;
using System.Management.Automation.Host;

namespace Hsp.PowerShell.Utility
{

  internal class SimplePsHostRawUserInterface : PSHostRawUserInterface
  {

    public override ConsoleColor ForegroundColor { get; set; }

    public override ConsoleColor BackgroundColor { get; set; }

    public override Coordinates CursorPosition { get; set; }

    public override Coordinates WindowPosition { get; set; }

    public override int CursorSize { get; set; }

    public override Size BufferSize { get; set; }

    public override Size WindowSize { get; set; }

    public override Size MaxWindowSize { get; }

    public override Size MaxPhysicalWindowSize { get; }

    public override bool KeyAvailable { get; }

    public override string WindowTitle { get; set; }


    public override KeyInfo ReadKey(ReadKeyOptions options)
    {
      return new KeyInfo();
    }

    public override void FlushInputBuffer()
    {
    }

    public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
    {
    }

    public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
    {
    }

    public override BufferCell[,] GetBufferContents(Rectangle rectangle)
    {
      return new BufferCell[1,1];
    }

    public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
    {
    }

  }

}