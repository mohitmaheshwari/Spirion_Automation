using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spirion.Automation.Framework
{
    public abstract class BaseDriver
    {
        public abstract IWebDriver CreateDriver();
    }
}
