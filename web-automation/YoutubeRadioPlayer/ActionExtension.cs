using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeRadioPlayer.Extension
{
    [Obsolete]
    public static class ActionExtension
    {
        const string RefreshMarker = "unrefreshed";
        /* To avoid conflict
        public static void WithJs(this WebDriverWait wait)
        {
            wait.Until(d => GetDocumentReadyState(d).Equals("complete"));
        }
        */
        public static string GetDocumentReadyState(IWebDriver driver)
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString();
        }

        public static IWebElement ElementExists(this IWebDriver driver, By locator)
        {
            try
            {
                var e = driver.FindElement(locator);
                return e;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static IWebElement UntilClickable(this WebDriverWait wait, By locator)
        {
            return wait.Until(d =>
            {
                try
                {
                    var e = d.FindElement(locator);
                    if (e.Displayed && e.Enabled)
                    {
                        return e;
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public static void AddRefreshMarker(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                $"document.querySelector('body').className+='{RefreshMarker}'");
        }

        public static bool InvisibilityOfRefreshMarker(this IWebDriver d)
        {
            return d.FindElements(By.ClassName(RefreshMarker)).Any();
        }

        public static void BlurAllInputs(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "document.querySelectorAll(\"input:focus\").forEach(function(entry) {entry.blur();});");
        }

        public static IAlert AlertIsPresent(this IWebDriver driver)
        {
            try
            {
                return driver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        }
        /* To avoid conflict
        public static void ClearAndSendKeys(this IWebElement element, string input)
        {
            element.Clear();
            element.SendKeys(input);
        }
        */

    }
}
