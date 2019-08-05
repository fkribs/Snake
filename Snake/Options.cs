using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Options
    {
        [Option(SetName ="FrameWait", Required = false, HelpText = "The milliseconds to wait before processing.",Default = 100)]
        public int FrameWait { get; set; }
    }
}
