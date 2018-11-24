using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeRadioPlayer.Extension;
using WebAutomation.YoutubeAdSkipper.Utility;
using static WebAutomation.YoutubeAdSkipper.Forms.Prompts;

namespace WebAutomation.YoutubeAdSkipper
{
    class Program
    {

        static void Main(string[] args)
        {
            new YoutubeAdSkipperLauncher().Run();
        }
    }
}
