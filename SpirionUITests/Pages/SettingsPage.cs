using OpenQA.Selenium;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests.Pages
{
    public class SettingsPage : GridPage
    {
        public SettingsPageElement.SettingsleftSideBar settingsleftSide => SettingsPageElement.settingslefftSieBar;

        public void OpenScriptRepository()
        {
            settingsleftSide.ScriptRepository.Click();

        }

        public void OpenApplicationSettings()
        {
            settingsleftSide.ApplicationSettings.Click();
        }
    }

    public class SettingsPageElement : CommonElements
    {
        public static SettingsleftSideBar settingslefftSieBar => new SettingsleftSideBar();
        public class SettingsleftSideBar: CommonElements
        {
            #region Locators
            private By _scriptRepository = By.XPath("//div[contains(.,'Script Repository')][@class='v-list-item__title']");
            private By _applicationSettings = By.XPath("//div[contains(.,'Application Settings')][@class='v-list-item__title']");
            #endregion

            #region WebElements 
            public WebElement ScriptRepository => browser.FindWebElement(_scriptRepository);
            public WebElement ApplicationSettings => browser.FindWebElement(_applicationSettings);
            #endregion

        }
    }
}
