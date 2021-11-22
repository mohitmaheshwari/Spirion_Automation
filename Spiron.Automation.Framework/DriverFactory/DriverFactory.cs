using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Spirion.Automation.Framework
{
    public class DriverFactory
    {
        public IWebDriver SetUpDriver(string browserName)
        {
            switch (browserName)
            {
                case "Chrome":
                     new DriverManager().SetUpDriver(new ChromeConfig());
                    return new Chrome().CreateDriver();
                case "Firefox":
                     new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new FirefoxDriver();
                default:
                    return null;
            }
        }
    }
}
