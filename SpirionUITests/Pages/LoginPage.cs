using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;

namespace SpirionUITests.Pages
{
    public class LoginPage : BasePage
    {

        #region Locators

        private By _skipBtn = By.CssSelector("button[value='skip']");
        private By _UserName = By.CssSelector("#Username");
        private By _Password = By.CssSelector("#Password");
        private By _loginBtn = By.CssSelector("button[value='login']");

        #endregion

        #region Page Elements

        private WebElement Skip => browser.FindWebElement(_skipBtn);
        private TextBoxElement Username => browser.FindTextBox(_UserName);
        private TextBoxElement Password => browser.FindTextBox(_Password);
        private WebElement LoginBtn => browser.FindWebElement(_loginBtn);

        #endregion

        #region Methods

        public void EnterUserName(string username)
        {
            Username.EnterText(username);
        }

        public void EnterPassword(string password)
        {
            Password.EnterText(password);
        }

        public void ClickLoginButton()
        {
            LoginBtn.Click();
        }

        public void ClickSkipButton()
        {
            if (Skip.Exists())
                Skip.Click();
        }

        public bool AreYouOnLoginPage()
        {
            return LoginBtn.Exists();
        }

        #endregion

    }

}
