using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spirion.Automation.Framework.Elements
{
    public class JavascriptElement : WebElement
    {
        IJavaScriptExecutor scriptDriver = Browser.Instance.WebDriver as IJavaScriptExecutor;
        public JavascriptElement(IWebElement element, string elementName) : base(element,elementName)
        {

        }

        public void ClickUsingJS()
        {
            scriptDriver.ExecuteScript("arguments[0].click();", Element);
            Logger.LogInfo("${ elementName} Clicked using JS");
        }

        public void ScrollElementToView()
        {
            scriptDriver.ExecuteScript("arguments[0].scrollIntoView(true);", Element);
            Logger.LogInfo("${ elementName} Clicked using JS");
        }

        public void SendKeysUsingJS()
        {

        }



    }
}
