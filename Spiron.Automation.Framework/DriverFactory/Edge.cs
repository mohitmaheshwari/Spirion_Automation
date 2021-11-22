using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;

namespace Spirion.Automation.Framework
{
    public class Edge : BaseDriver
    {
        public override IWebDriver CreateDriver()
        {
            var options = new EdgeOptions { UseInPrivateBrowsing = true };
            //options.AddArgument("headless");
            var service = EdgeDriverService.CreateDefaultService(Environment.CurrentDirectory, "msedgedriver.exe");
            service.UseVerboseLogging.Equals(true);
            var driver = new EdgeDriver(service, options);
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            return driver;
        }
    }
}
