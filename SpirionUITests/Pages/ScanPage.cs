using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;

namespace SpirionUITests.Pages
{
    public class ScanPage : BasePage
    {
        public PageElementsOnScan.ScanUI ScanUI => PageElementsOnScan.scanUI;
        public PageElementsOnScan.ScanFooterUI ScanFooterUI => PageElementsOnScan.scanFooterUI;

        public void SetScanName(string scanName)
        {
            ScanUI.ScanNameTxt.EnterText(scanName);
            Logger.LogInfo($"Scan Name :{scanName}");
        }

        public void SetScanDescription(string scanDescription)
        {
            ScanUI.ScanDescriptionTxt.EnterText(scanDescription);
        }

        public void ClickNext()
        {
            ScanFooterUI.NextBtn.Click();
        }

        public void SelectMetaDataScan()
        {
            ScanUI.DiscoveryMetaDataScan.Click();
        }

        public void SelectSensitiveDataScan()
        {
            ScanUI.SensitiveDataScan.Click();
        }

        public void SelectDiscoveryScan()
        {

        }
        public void SelectPlayBook(string playBook)
        {
            ScanUI.SearchPlayBookTxt.EnterText(playBook);
            ScanUI.SearchPlaybookBtn.Click();
            ScanUI.SelectPlayBook(playBook).Click();
        }

        public void SelectScanTargetType(string targetType)
        {
            ScanUI.ScanTarget(targetType).Click();
        }

        public void SelectCloud(string cloud)
        {
            ScanUI.ScanCloud(cloud).Click();
        }

        public void SelectTargetType(string targetType)
        {
            ScanUI.ScanTarget(targetType).Click();
        }

        public void SearchTargetToScan(string target)
        {
            ScanUI.TargetToScanTxt.EnterText(target);
            int tries = 0;
            while (tries++ < 3)
            {
                ScanUI.TargetToScanSearchBtn.Click();
            }
            ScanUI.SelectTargetFromTree(target).Click();
        }

        public void SelectAgentToScanRemoteMachine(string agentname)
        {
            ScanUI.TxtOnPermAgent.EnterText(agentname);
            ScanUI.TxtSearchPermAgent.Click();
            ScanUI.SelectTargetFromTree(agentname).Click();
        }

        public void SearchLocationType(string locationType)
        {
            ScanUI.ScanTarget(locationType).Click();
        }

        public void IncludePath(string path)
        {
            ScanUI.Include.EnterText(path);
        }

        public void EnterCloudAccount(string account)
        {
            ScanUI.InputScanAccount.EnterTextAndPressEnter(account);
        }

        public void ClickFinish()
        {
            ScanFooterUI.Finish.Click();
        }

        public void SelectOnPRemAgent()
        {
            ScanUI.OnPremAgent.Click();
        }
    }

    public class PageElementsOnScan : CommonElements
    {
        public static ScanUI scanUI => new ScanUI();
        public static ScanFooterUI scanFooterUI => new ScanFooterUI();
        public class ScanUI : PageElementsOnScan
        {
            #region Locators
            private By _allScans = By.CssSelector("#scan-landing-component div.v-tab.v-tab--active");
            private By _search = By.Id("scan-landing-componentSearchbarSearchInput");
            private By _discoveryScans = By.CssSelector("");
            private By _sensitiveData = By.CssSelector("");
            private By _addScan = By.CssSelector("#scan-landing-componentActionsBtnAddScan");
            private By _scanNameTxt = By.CssSelector("#scanConfigName");
            private By _scanDescription = By.CssSelector("#descriptionInputScanConfig");
            private By _discoveryMetaDataScan = By.XPath("//button[contains(.,'Discovery')]");
            private By _sensitiveDataScan = By.XPath("//button[contains(.,'Sensitive Data Scan')]");
            private By _searchPlayBookTxt = By.CssSelector("#PaneScanSDRSearchInput");
            private By _searchPlaybookBtn = By.CssSelector("#PaneScanSDRSearchButton");
            private By _selectPlayBook(string playbook) => By.XPath($"//tr[td[contains(.,'{playbook}')]]");
            private By _scanTarget(string target) => By.XPath($"//button[contains(.,'{target}')]");
            private By _scanCloudType(string type) => By.CssSelector($"#PaneCloudSource div[aria-label*='{type}']");
            private By _targetToScanTxt = By.CssSelector("#PaneScanEndpointSearchInput");
            private By _targetToScanSearchBtn = By.CssSelector("#PaneScanEndpointSearchButton");
            private By _selectTargetFromTree(string targetName) => By.XPath($"//div[@class='v-treeview-node__root'][contains(.,'{targetName}')]//button");
            private By _include = By.CssSelector("#include");
            private By _txtOnPermAgent = By.CssSelector("#scanPaneAgentsSearchbarSearchInput");
            private By _searchBtnPermAgent = By.CssSelector("#scanPaneAgentsSearchbarSearchButton");
            private By _inputScanAccount = By.CssSelector("[id*='user-input-endpoint']");
            private By _selectOnPremAgent = By.XPath("//*[@id='PaneScanAgent']//button[contains(.,'On-Prem')]");
            
            #endregion

            #region WebElements

            public WebElements AllScans => browser.FindWebElements(_allScans);
            public TextBoxElement SearchPlayBookTxt => browser.FindTextBox(_searchPlayBookTxt);
            public TextBoxElement TargetToScanTxt => browser.FindTextBox(_targetToScanTxt);
            public WebElement TargetToScanSearchBtn => browser.FindWebElement(_targetToScanSearchBtn);
            public WebElement SelectTargetFromTree(string targetName) => browser.FindWebElement(_selectTargetFromTree(targetName));
            public WebElement SearchPlaybookBtn => browser.FindWebElement(_searchPlaybookBtn);
            public WebElement AddScan => browser.FindWebElement(_addScan);
            public TextBoxElement ScanNameTxt => browser.FindTextBox(_scanNameTxt);
            public TextBoxElement ScanDescriptionTxt => browser.FindTextBox(_scanDescription);
            public WebElement DiscoveryMetaDataScan => browser.FindWebElement(_discoveryMetaDataScan);
            public WebElement SensitiveDataScan => browser.FindWebElement(_sensitiveDataScan);
            public TextBoxElement InputScanAccount => browser.FindTextBox(_inputScanAccount);
            public WebElement SelectPlayBook(string playBook) => browser.FindWebElement(_selectPlayBook(playBook));
            public WebElement ScanTarget(string target) => browser.FindWebElement(_scanTarget(target));
            public WebElement ScanCloud(string cloud) => browser.FindWebElement(_scanCloudType(cloud));
            public TextBoxElement Include => browser.FindTextBox(_include);
            public TextBoxElement TxtOnPermAgent => browser.FindTextBox(_txtOnPermAgent);
            public WebElement TxtSearchPermAgent => browser.FindWebElement(_searchBtnPermAgent);
            public WebElement OnPremAgent => browser.FindWebElement(_selectOnPremAgent);
            #endregion
        }

        public class ScanFooterUI : PageElementsOnScan
        {
            #region Footer
            private By _nextBtn = By.XPath("//button[contains(.,'Next')]");
            private By _finish = By.XPath("//button[contains(.,'Finish')]");
            public WebElement Finish => browser.FindWebElement(_finish);
            public WebElement NextBtn => browser.FindWebElement(_nextBtn);
            #endregion
        }



    }
}
