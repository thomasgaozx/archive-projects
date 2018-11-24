using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAutomation.Core;
using WebAutomation.Core.Support;
using YoutubeRadioPlayer.Extension;
using static WebAutomation.YoutubeAdSkipper.Forms.Prompts;

namespace WebAutomation.YoutubeAdSkipper
{
    public class YoutubeAdSkipperLauncher : ZActionBase
    {
        [ZAction]
        public void LaunchYoutubeAdSkipper()
        {
            #region OneTimeSetUp

            User user = PromptPswdProcessor();

            while (user == null)
            {
                Console.WriteLine("Try again. ");
                user = PromptPswdProcessor();
            }

            Console.WriteLine("\n\nThank you, execution continues ... \n\n");

            LaunchDriver();

            var refreshHandler = new ZRefreshMarkerHandler(this);

            #endregion

            #region Local Functions and Utilities

            Action<By> ClickAndWait = locator =>
            {
                
                refreshHandler.AddRefreshMarker();
                driver.BlurAllInputs();
                Clickable(locator).Click();
                refreshHandler.WaitUntilInvisibilityOfRefreshMarker();
            };

            Func<bool> InvalidFieldExists =
                () => driver.FindElements(By.CssSelector("[aria-live='assertive'][aria-atomic='true']")).FirstOrDefault(e => e.Displayed && !String.IsNullOrEmpty(e.Text)) != null;

            By SkipAdLocator = By.CssSelector("button.videoAdUiSkipButton");
            Func<bool> AdExists = () => driver.FindElements(SkipAdLocator).Any();

            #endregion

            #region Act

            driver.Navigate().GoToUrl("https://www.youtube.com/");
            wait.WithJs();
            Clickable(By.CssSelector("#buttons > ytd-button-renderer > a")).Click();
            wait.WithJs();

            Clickable(By.Id("identifierId")).SendKeys(user.UserName);
            ClickAndWait(By.Id("identifierNext"));

            while (InvalidFieldExists()) // check success status
            {

                Console.WriteLine("\n\nBad email :( ... ReEnter information in the form");

                do
                {
                    Console.WriteLine("\n\nBad email ... ReEnter information in the form");
                    user.UserName = ReEnterInformation();
                } while (user.UserName == null || EmailPattern.Match(user.UserName).Value == "");

                Clickable(By.Id("identifierId")).ClearAndSendKeys(user.UserName);
                ClickAndWait(By.Id("identifierNext"));

            }

            Clickable(By.CssSelector("#password input[type='password']")).SendKeys(user.PassWord);
            ClickAndWait(By.Id("passwordNext"));

            while (InvalidFieldExists()) // check success status
            {
                Console.WriteLine("\n\nBad Password :( ... ReEnter information in the form");
                do
                {
                    Console.WriteLine("\n\nBad Password ... ReEnter information in the form");
                    user.PassWord = ReEnterInformation(true);
                } while (user.PassWord == null);

                Clickable(By.CssSelector("#password input[type='password']")).ClearAndSendKeys(user.PassWord);
                ClickAndWait(By.Id("passwordNext"));
            }

            wait.Until(d => d.Url.Contains("https://www.youtube.com/"));
            //HandleAlert(driver);
            /*
            using (PowerShell ps1 = PowerShell.Create())
            {
                ps1.AddScript(System.IO.File.ReadAllText(@"PowerShellControl\ScheduleRadioShutdown.ps1"));
                ps1.BeginInvoke();
            }
            */

            wait.WithJs();
            driver.FindElements(By.Id("guide-icon")).FirstOrDefault(e => e.Displayed && e.Enabled).Click();
            Clickable(By.CssSelector("a[title='Conversations']")).Click();
            Clickable(By.CssSelector("a#thumbnail.ytd-playlist-thumbnail")).Click();
            Clickable(By.CssSelector("button[aria-label='Shuffle playlist']")).Click();

            while (driver.WindowHandles.Count > 0)
            {
                if (AdExists())
                {
                    wait.Timeout = TimeSpan.FromSeconds(30);
                    Clickable(SkipAdLocator).Click();
                    wait.Timeout = TimeSpan.FromSeconds(15);
                }
                Thread.Sleep(TimeSpan.FromSeconds(3));

            }


            #endregion

        }

    }
}
