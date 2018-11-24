using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAutomation.MITOCWCourseNotesDownloader;

namespace MITOCWCourseNotesDownloader
{
    class Program
    {
        static async Task Main()
        {
            await new MITOCWCourseNotesDownloaderLauncher().RunAsync();
        }
    }
}
