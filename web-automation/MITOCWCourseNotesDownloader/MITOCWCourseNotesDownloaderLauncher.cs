using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAutomation.Core;
using WebAutomation.Core.Support;

namespace WebAutomation.MITOCWCourseNotesDownloader
{
    public class MITOCWCourseNotesDownloaderLauncher : ZActionBase
    {
        static string GetDownloadPath()
        {
            Console.WriteLine("Please Enter the Directory to have the files downloaded.");
            var path = Console.ReadLine();
            while (!Directory.Exists(path))
            {
                Console.WriteLine("The specified path does not exist!\nTry again.");
                path = Console.ReadLine();
            }
            return path;
        }

        static async Task<string> GetUrlAsync()
        {
            Console.WriteLine("Please Enter the absolute url of the target page");
            string url = Console.ReadLine();
            while (!(await ZEndpoint.LinkIsValidAsync(url)))
            {
                Console.WriteLine("Url is invalid! Please Enter again below: ");
                url = Console.ReadLine();
            }
            return url;
        }

        #region Dummy Test Properties
        static string DummyPath => @"C:\Users\DSNYSVP13213\Downloads";
        static string DummyUrl => "https://ocw.mit.edu/courses/mathematics/18-01-single-variable-calculus-fall-2006/lecture-notes/";

        #endregion

        [ZAction]
        public async Task LaunchMITOCWCourseNotesDownloader()
        {
            #region Arrange

            // Arrange
            string downloadPath = DummyPath;
            string url = DummyUrl;

            // Launch Driver
            LaunchDriver();

            #endregion

            #region Local Functions

            Func<IWebElement> GetDownloadButton = () =>
            {
                return (driver as IJavaScriptExecutor).ExecuteScript(
                "function(){var toolbar=document.getElementById('toolbar');" +
                "toolbar.setAttribute('style','transform:none');" +
                "return toolbar.shadowRoot.getElementById('download');}") as IWebElement;
            };

            #endregion

            #region Act

            driver.Navigate().GoToUrl(url);
            wait.WithJs();

            string directoryNameExtraction = Regex.Matches(driver.Title, @"(?<=\| )[^\|]+(?= \|)")[0].Value;

            var downloadDir = new DirectoryInfo($"{downloadPath}\\{directoryNameExtraction}");
            if (!downloadDir.Exists)
            {
                downloadDir.Create();
            }

            var processedDir = new DirectoryInfo(downloadDir.FullName + "_processed");
            if (!processedDir.Exists)
            {
                processedDir.Create();
            }
            var help = driver.FindElements(By.CssSelector("div.help.slide-bottom"));
            if (help.Any())
            {
                (driver as IJavaScriptExecutor).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", help[0]);
            }

            string standardSelector = "#course_inner_section td a";
            string tempSelector = "#course_inner_section tr td:nth-child(4) a";
            var downloadUrls = driver.FindElements(By.CssSelector(standardSelector)).Select(e => $"{e.GetAttribute("href")}");

            int i = 0;
            foreach (var downloadUrl in downloadUrls)
            {
                var fileName = $"{(++i).ToString()}.pdf";
                var filePath = $"{downloadDir.FullName}\\{fileName}";
                new WebClient().DownloadFile(downloadUrl, filePath);
                ZPdfUtil.TrimFirstPage(filePath, $"{processedDir.FullName}\\{fileName}");
            }

            ZPdfUtil.MergeAll(processedDir.FullName, $"{downloadPath}\\{directoryNameExtraction}.pdf");
            // Open all links as new tabs
            /*
            foreach (var pdfLink in pdfs)
            {
                new Actions(driver).MoveToElement(pdfLink).Perform();
                await Task.Delay(400);
                new Actions(driver).KeyDown(Keys.Control).Click().KeyUp(Keys.Control).Perform();
                await Task.Delay(200);
            }

            driver.Close();
            var handles = driver.WindowHandles;

            while (handles.Count > 0)
            {
                driver.SwitchTo().Window(handles[0]);
                WaitWithJs();
                
                var toolbar = Visible(By.Id("toolbar"));
                var js = driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].setAttribute('style','transform:none')",toolbar);
                IWebElement downloadButton=js.ExecuteScript("return arguments[0].shadowRoot.getElementById('download')", toolbar) as IWebElement;
                downloadButton.Click();
               //GetDownloadButton().Click();

                await Task.Delay(100);
                driver.Close();
                handles = driver.WindowHandles;
            }
            */
            Console.WriteLine("Download is complete");

            #endregion
        }
    }
}
