using OpenQA.Selenium;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests.Pages.Settings
{
    public class ScriptRepository : SettingsPage
    {
        public ScriptRepositoryPageElement.SRCustomScriptPage SRCustomScriptPage => ScriptRepositoryPageElement.srCustomScriptPage;
        public ScriptRepositoryPageElement.SRMainPage SrMainPage => ScriptRepositoryPageElement.srMainPage;

        public void UploadScript(string name, string description, string path)
        {
            SRCustomScriptPage.CustomScriptName.EnterText(name);
            SRCustomScriptPage.CustomScriptDescription.EnterText(description);
            SRCustomScriptPage.FileUpload.UploadFile(path);
            SRCustomScriptPage.Save.Click();
        }

        public bool IsScriptFound(string scriptName)
        {
            SrMainPage.TxtSearch.ClearText();
            SrMainPage.TxtSearch.EnterText(scriptName);
            SrMainPage.SubmitBtn.Click();
            WaitForGridToLoad();
            return IsRowFoundAfterFilter(scriptName);
        }

        public void AddScript()
        {
            SrMainPage.ActionButton.Click();
            SrMainPage.AddScript.Click();
        }

        public bool IsScriptRepositoryPageOpened()
        {
            return SrMainPage.ScriptRepositoryHeader.IsDisplayed();
        }

        public void SetStatus(string searchName)
        {
            if (GetStatus(searchName) == false)
            {
                SrMainPage.Status(searchName).Click();
            }
        }

        private bool GetStatus(string searchName)
        {
            try
            {
                return Convert.ToBoolean(SrMainPage.StatusInput(searchName).GetAttribute("aria-checked"));
            }
            catch
            {
                return true;
            }
        }
    }

    public class ScriptRepositoryPageElement : SettingsPageElement
    {
        public static SRMainPage srMainPage => new SRMainPage();
        public static SRCustomScriptPage srCustomScriptPage => new SRCustomScriptPage();
        public class SRMainPage: ScriptRepositoryPageElement
        {
            #region Locators
            private By _actionButton = By.CssSelector("#scan-landing-componentActionsBtn");
            private By _addScript = By.XPath("//div[text()='Add Script']");
            private By _txtSearch = By.CssSelector("#scan-landing-componentSearchbarSearchInput");
            private By _submitBtn = By.CssSelector("#scan-landing-componentSearchbarSearchButton");
            private By _scriptRepositoryHeader = By.CssSelector("#scan-landing-componentPageTitle");
            private By _statusBtn(string search) => By.XPath($"//tr[td[contains(.,'{search}')]]/td//div[@class='v-input--selection-controls__ripple']");
            private By _getStatus(string search) => By.XPath($"//tr[td[contains(.,'{search}')]]/td//div[@class='v-input--selection-controls__ripple']/preceding-sibling::input");
            #endregion

            #region Elements
            public WebElement ActionButton => browser.FindWebElement(_actionButton);
            public WebElement AddScript => browser.FindWebElement(_addScript);
            public TextBoxElement TxtSearch => browser.FindTextBox(_txtSearch);
            public WebElement SubmitBtn => browser.FindWebElement(_submitBtn);
            public WebElement ScriptRepositoryHeader => browser.FindWebElement(_scriptRepositoryHeader);
            public WebElement Status(string search) => browser.FindWebElement(_statusBtn(search));
            public WebElement StatusInput(string search) => browser.FindHiddenElement(_getStatus(search));
            #endregion
        }

        public class SRCustomScriptPage: ScriptRepositoryPageElement
        {
            #region Locators
            private By _customScriptName = By.CssSelector("#EditCustomScriptDialogScriptName");
            private By _customScriptDescription = By.CssSelector("#EditCustomScriptDialogDescription");
            private By _fileUpload = By.CssSelector("input[type='file']");
            private By _save = By.CssSelector("#EditCustomScriptDialogSave");
            private By _cancel = By.XPath("//button[contains(.,'Cancel')]");
            #endregion

            #region PageElements

            public WebElement FileUpload => browser.FindHiddenElement(_fileUpload);
            public WebElement Save => browser.FindWebElement(_save);
            public WebElement Cancel => browser.FindWebElement(_cancel);
            public TextBoxElement CustomScriptName => browser.FindTextBox(_customScriptName);
            public TextBoxElement CustomScriptDescription => browser.FindTextBox(_customScriptDescription);

            #endregion

        }
    }
}

