using SpirionUITests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using OpenQA.Selenium;
using Spirion.Automation.Framework.Elements;

namespace SpirionUITests.Pages
{
    public class BasePage
    {
      
        public Browser browser => Browser.Instance;
        public CommonElements.LeftSideBar MainLeftSideBar => CommonElements.leftSideBar;
      
        #region Methods
        public void GoToSpironUrl(string url)
        {
            Browser.Instance.NavigateToURL(url);
        }

        public void ClickNotification()
        {
            MainLeftSideBar.Notifications.Click();
        }

        public void ClickScan()
        {
            MainLeftSideBar.ScanLink.Click();
        }

        public bool AreYouOnScanLink()
        {
            return MainLeftSideBar.ScanLink.Exists();
        }

        public void HitRefresh()
        {
            Browser.Instance.HitRefresh();
        }

        public void CreateNewScan()
        {
            MainLeftSideBar.CreateNewScan.Click();
        }

        public void ClickScanPlayBooks()
        {
            MainLeftSideBar.CreatePlayBooks.Click();
        }

        public void GoToSettingsPage()
        {
            MainLeftSideBar.Settings.Click();
        }

        public void ClickScanResults()
        {
            MainLeftSideBar.ScanResults.Click();
        }

        public void ClickAllScans()
        {
            MainLeftSideBar.AllScans.Click();
        }

        public void GoBack()
        {
            MainLeftSideBar.Back.Click();
        }

        public void SwitchtoNewTab()
        {
            browser.SwitchToNewTab();
        }
      
        public void ClickDataAssetInventoryMenu()
        {
            MainLeftSideBar.DataAssetInventoryMenu.Click();
        }

        #endregion
    }

    public class CommonElements 
    {
        public Browser browser => Browser.Instance;
        public static LeftSideBar leftSideBar => new LeftSideBar();
       
        public class LeftSideBar : CommonElements
        {
            #region Locators
            private By _back = By.XPath("//div[text()='BACK']");
            public By _ScanMenu = By.CssSelector("#MenuItemScans");
            private By _createNewScan = By.XPath("//a[contains(.,'Create New Scan')]");
            private By _scanPlayBooks = By.XPath("//a[contains(.,'Scan Playbooks')]");
            private By _allScans = By.XPath("//div[contains(@class,'navbar')]//a[contains(.,'All Scans')]");
            private By _scanResults = By.XPath("//a[contains(.,'Scan Results')]");
            private By _settings = By.XPath("//div[contains(.,'Settings')][@class='v-list-item__title']");
            private By _notifications = By.XPath("//div[div[contains(@class,'cover')]]/following-sibling::span/button");
            private By _dataAssetInventoryMenu = By.CssSelector("#MenuItemDataAssetInventory");

            #endregion

            #region Page Elements

            public WebElement ScanLink => browser.FindWebElement(_ScanMenu);
            public WebElement CreateNewScan => browser.FindWebElement(_createNewScan);
            public WebElement CreatePlayBooks => browser.FindWebElement(_scanPlayBooks);
            public WebElement ScanResults => browser.FindWebElement(_scanResults);
            public WebElement AllScans => browser.FindWebElement(_allScans);
            public WebElement Settings => browser.FindWebElement(_settings);
            public WebElement Back => browser.FindWebElement(_back);
            public WebElement Notifications => browser.FindWebElement(_notifications);
            public WebElement DataAssetInventoryMenu => browser.FindWebElement(_dataAssetInventoryMenu);
            
            #endregion
        }

       


    }
}
