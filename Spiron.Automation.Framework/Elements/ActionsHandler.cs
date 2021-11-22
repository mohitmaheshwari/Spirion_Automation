using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spiron.Automation.Framework.Elements
{
    public class ActionHandler 
    {
        public Actions actions = new Actions(Browser.Instance.WebDriver);
        public IWebElement _element = null;
        public ActionHandler(IWebElement element)
        {
            _element = element;
        }

        public void SendKeys(string text)
        {
            actions.Click(_element).SendKeys(text).Build().Perform();
        }

        public void Click()
        {
            actions.MoveToElement(_element).Click().Build().Perform();
        }
    }
}
