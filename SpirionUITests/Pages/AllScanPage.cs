using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System.Threading;
using System.Linq;

namespace SpirionUITests.Pages
{
    public class AllScanPage : GridPage
    {

        #region Locators

        private By _scanTextSearch = By.CssSelector("#scan-landing-componentSearchbarSearchInput");
        private By _scanSearchBtn = By.CssSelector("#scan-landing-componentSearchbarSearchButton");
        private By _scanOptions = By.CssSelector("[id*='ScanLandingComponentMoreBtn']");
        private By _runScanNow = By.XPath("//div[text()='Run Scan Now']");
        private By _deleteScan = By.XPath("//div[text()='Delete Scan']");
        private By _scanStatus = By.CssSelector("table[class*='data-table'] td:nth-child(3)");
        private By _playBookAnchor = By.CssSelector("table tbody tr:nth-child(1) td:nth-child(6) a");
        
        public TextBoxElement ScanTextSearch => browser.FindTextBox(_scanTextSearch);
        public WebElement ScanSearchBtn => browser.FindWebElement(_scanSearchBtn);
        public WebElement ScanMoreOptions => browser.FindWebElementWithExplicitWait(120,_scanOptions);
        public WebElement RunScanNow => browser.FindWebElement(_runScanNow);
        public WebElement ScanStatus => browser.FindWebElement(_scanStatus);
        public WebElement DeleteScan => browser.FindWebElement(_deleteScan);
        public WebElement PlayBookAnchor => browser.FindWebElement(_playBookAnchor);
        #endregion

        

        #region Methods
        public void WaitForAllScansToLoad()
        {
            WaitForGridToLoad();
        }
        
        public void EnterSearchTextForScan(string scan)
        {
            ScanTextSearch.EnterText(scan);
            ScanSearchBtn.Click();
        }

        public void ClickPlayBook()
        {
            PlayBookAnchor.Click();
        }

        public void RunScan()
        {
            ScanMoreOptions.Click();
            RunScanNow.Click();
        }

        public bool WaitForScanToComplete(string searchName)
        {
            int tries = 0;
            while(GetFilteredValueFromGrid("Status").Contains("Done, View Results") == false && tries++ < 50)
            {
                Thread.Sleep(15 * 1000);
                Browser.Instance.HitRefresh();
                WaitForAllScansToLoad();
                EnterSearchTextForScan(searchName);
            }
            if (tries < 50)
                return true;
            return false;
        }

        public void OpenSearchResultOfScan()
        {
            ScanStatus.Click();
        }

        public void DeleteExistingSearch(string search)
        {
            WaitForAllScansToLoad();
            EnterSearchTextForScan(search);
            ScanMoreOptions.Click();
            DeleteScan.Click();
            CllickConfirmDelete();
        }

        #endregion
   
    }

}
