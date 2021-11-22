using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Serilog;
using Spirion.Automation.Framework.Elements;

namespace Spirion.Automation.Framework
{
    public static class Extensions
    {
        public static void Focus(this IWebDriver webDriver)
        {
            webDriver.SwitchTo().Window(webDriver.CurrentWindowHandle);
            Thread.Sleep(1000);
        }

        public static void ScrollBy(this IWebDriver webDriver)
        {
            Thread.Sleep(500);
            IJavaScriptExecutor executor = (IJavaScriptExecutor)webDriver;
            executor.ExecuteScript("window.scrollBy(0,500)", "");
            Thread.Sleep(500);
            webDriver.SwitchTo().Window(webDriver.CurrentWindowHandle);
        }

        public static void WaitUntilLoaded(this IWebDriver webDiver)
        {
            WebDriverWait wait = new WebDriverWait(webDiver, TimeSpan.FromMinutes(1));
            wait.Until(w => ((IJavaScriptExecutor)webDiver).ExecuteScript("return document.readyState").ToString().Equals("complete"));

        }

    }
}