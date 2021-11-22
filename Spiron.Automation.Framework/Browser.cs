using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Spirion.Automation.Framework.Elements;
using System.Threading;

namespace Spirion.Automation.Framework
{
    public class Browser
    {
        public static ThreadLocal<IWebDriver> WebDriverThreaded = new ThreadLocal<IWebDriver>();
        public IWebDriver WebDriver 
        {
            get
            {
                return WebDriverThreaded.Value;
            }
            set
            {
                WebDriverThreaded.Value = value;
            }
        }
        private static readonly object padlock = new object();
        private static Browser instance = null;
        public int TimeOutInSeconds = 10;
        public By by;
        public Wait WaitHelper => new Wait();
        public IWebElement ParentElement;

        public List<string> ElementsText = new List<string>();


        public void InitWebDriver(string browserName)
        {
            WebDriver = new DriverFactory().SetUpDriver(browserName);
        }

        public static void KillProcess()
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("chrome.exe"))
                {
                    proc.Kill();
                }
                foreach (Process proc in Process.GetProcessesByName("ChromeWebDriver"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                throw ex;

                //Logger.WriteError(ex.Message, ex);
            }
        }
        public void CloseWebDriver()
        {
            Instance.WebDriver.Dispose();
            KillProcess();
        }

        public void NavigateToURL(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        private Browser()
        {

        }


        public static Browser Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Browser();
                    }
                    return instance;
                }
            }
        }

        public WebElements FindRefreshedWebElements(By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(TimeOutInSeconds, IsElementVisible))
            {
                WaitHelper.WaitFor(2, HasElementTextChanged);
                IList<WebElement> webElements = Elements.Select(x => new WebElement(x, nameof(x))).ToList();
                return new WebElements(webElements, nameof(selector));
            }
            return new WebElements(null, nameof(selector));
        }

        

        public WebElements FindWebElements(By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(TimeOutInSeconds, IsElementVisible))
            {
                IList<WebElement> webElements = Elements.Select(x => new WebElement(x, nameof(x))).ToList();
                return new WebElements(webElements, nameof(selector));
            }
            return new WebElements(null, nameof(selector));
        }

        public WebElement FindChildElement(IWebElement parentElement, By selector)
        {
            ParentElement = parentElement;
            by = selector;
            return new WebElement(Element, nameof(selector));
        }

        public WebElements FindChildElements(IWebElement parentElement, By selector)
        {
            ParentElement = parentElement;
            by = selector;
            IList<WebElement> webElements = Elements.Select(x => new WebElement(x, nameof(x))).ToList();
            return new WebElements(webElements, nameof(selector));
        }

       
        public WebElement FindWebElementWithoutWait(By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(0, IsElementVisible))
                return new WebElement(WebDriver.FindElement(selector), nameof(selector));
            else return new WebElement(null, nameof(selector));

        }

        public void WaitUntilLoaded()
        {
            WebDriver.WaitUntilLoaded();
        }

        public void ScrollToElement(int x, int y)
        {

        }

        public WebElement FindStaleElement(By selector)
        {
            int tries = 0;
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            while (Element != null && tries++ < 25)
            {
                WaitHelper.WaitFor(TimeOutInSeconds, IsElementVisible);
            }
            if (WaitHelper.WaitFor(TimeOutInSeconds, IsElementVisible))
                return new WebElement(WebDriver.FindElement(selector), nameof(selector));
            else
                return new WebElement(null, nameof(selector));
        }

        public WebElement FindWebElement(By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(TimeOutInSeconds, IsElementVisible))
                return new WebElement(WebDriver.FindElement(selector), nameof(selector));
            else return new WebElement(null, nameof(selector));
        }

        public TextBoxElement FindTextBox(By selector)
        {
            return FindWebElement(selector).ToTextBox();
        }

        public WebElement FindWebElementWithExplicitWait(int timeout, By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(0, IsElementVisible))
                return new WebElement(WebDriver.FindElement(selector), nameof(selector));
            else return new WebElement(null, nameof(selector));
        }

        public void WaitUntilElementDisappears(By selector)
        {
            FindElementUsingRetry(selector);
            while (WaitHelper.WaitFor(0, IsElementVisible))
            {

            }

        }

        public void DragAndDropToOffset(By selector, int xAxis, int yAxis)
        {
            try
            {
                FindElementUsingRetry(selector);
                (new Actions(WebDriver)).DragAndDropToOffset(Element, xAxis, yAxis).Perform();
            }
            catch
            {

            }
        }

        public void SwitchToFrame(By frameSelector)
        {
            FindElementUsingRetry(frameSelector);
            WebDriver.SwitchTo().Frame(Element);
        }

        public void SwitchToFrame(WebElement webElement)
        {
            WebDriver.SwitchTo().Frame(webElement.Element);
        }

        public void SwitchToNewTab()
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.Last());
        }

        public void SwitchToOriginalTab()
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.First());
        }

        public void HitRefresh()
        {
            WebDriver.Navigate().Refresh();
        }
        public void SwitchToDefault()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public WebElement FindHiddenElement(By selector)
        {
            WebDriver.WaitUntilLoaded();
            FindElementUsingRetry(selector);
            if (WaitHelper.WaitFor(TimeOutInSeconds, DoesElementExist))
                return new WebElement(WebDriver.FindElement(selector), nameof(selector));
            else return new WebElement(null, nameof(selector));
        }

       
        public void SetFocusOnBrowser()
        {
            ((IJavaScriptExecutor)WebDriver).ExecuteScript("window.focus();");

        }

        public string PageSource()
        {
            return WebDriver.PageSource;
        }


        private void FindElementUsingRetry(By selector)
        {
            ParentElement = null;
            by = selector;
            Log.Logger.Information($"Finding {nameof(selector)} Using {selector}");
        }

        public List<IWebElement> Elements
        {
            get
            {
                try
                {
                    if (ParentElement != null)
                    {
                        return ParentElement.FindElements(by).ToList();
                    }
                    else
                    {
                        return WebDriver.FindElements(by).ToList();
                    }
                }
                catch (Exception)
                {
                    return new List<IWebElement>();
                }
            }
        }

        public IWebElement Element
        {
            get
            {
                try
                {
                    if (ParentElement != null)
                    {
                        return ParentElement.FindElement(by);
                    }
                    else
                    {
                        return WebDriver.FindElement(by);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool AllElementsWithText()
        {
            bool result = false;
            try
            {
                result = Elements.All(x => string.IsNullOrEmpty(x.Text) == false);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool HasElementTextChanged()
        {
            ElementsText = new List<string>();
            bool result = true;
            int tries = 0;
            while (tries++ < 3)
            {
                try
                {
                    ElementsText.Add(Element.Text);
                    result = result && (ElementsText.Count > 1 && ElementsText.Any(x => x != ElementsText[0]) == false);
                    if (result) return result;
                }
                catch
                {
                }
            }
            return result;
        }

     
        public bool IsElementVisible()
        {
            bool result = false;
            try
            {
                result = ((Element != null) && (Element.Displayed));
                if (Element != null)
                {
                    string attribute = Element.GetAttribute("outerHTML");
                    return string.IsNullOrEmpty(attribute) == false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public bool DoesElementExist()
        {
            bool result = false;
            try
            {
                result = Element != null;
            }
            catch
            {
                result = false;
            }
            return result;
        }

       
        public void ScrollToBottom()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        public void ScrollToTop()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            js.ExecuteScript("window.scrollBy(0,-500)", "");
        }

        public bool WaitForUrlToContains(string url)
        {
            int tries = 0;
            while (tries++ < 60)
            {
                string WebDriverUrl = WebDriver.Url;
                if (WebDriverUrl.Contains(url))
                    return true;
            }
            return false;
        }
        public string GetUrl()
        {
            return WebDriver.Url;
        }

     

    }

    //public static class WebDriverExtension
    //{
    //    public static WebElement FindWebElement(this IWebDriver WebDriver,By selector)
    //    {
    //        return Browser.Instance.FindWebElement(selector, WebDriver);
    //    }

    //    public static TextBoxElement FindTextBox(this IWebDriver WebDriver, By selector)
    //    {
    //        return Browser.Instance.FindWebElement(selector,WebDriver).ToTextBox();
    //    }

    //    public static WebElement FindWebElementWithExplicitWait(this IWebDriver WebDriver, int timeout, By selector)
    //    {
    //        return Browser.Instance.FindWebElementWithExplicitWait(timeout, selector, WebDriver);
    //    }
    //    public static DropdownElement FindDropdown(this IWebDriver WebDriver, By selector)
    //    {
    //        return Browser.Instance.FindWebElement(selector,WebDriver).ToDropDown();
    //    }

    //    public static WebElements FindWebElements(this IWebDriver WebDriver, By selector)
    //    {
    //        return Browser.Instance.FindWebElements(selector, WebDriver);
    //    }

    //    public static WebElement FindHiddenElement(this IWebDriver WebDriver, By selector)
    //    {
    //        return Browser.Instance.FindHiddenElement(selector, WebDriver);
    //    }

    //    public static void WaitUntilElementDisappears(this IWebDriver WebDriver,By selector)
    //    {
    //        Browser.Instance.WaitUntilElementDisappears(selector, WebDriver);
    //    }
    //}
}
