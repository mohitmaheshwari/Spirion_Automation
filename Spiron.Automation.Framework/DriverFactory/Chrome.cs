using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Spirion.Automation.Framework
{
    public class Chrome : BaseDriver
    {
        public override IWebDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AcceptInsecureCertificates = true;
            options.AddArgument("--start-maximized");
            options.AddArgument("--lang=en");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-dev-shm-usage");
            //options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--enable-logging");
            var driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
