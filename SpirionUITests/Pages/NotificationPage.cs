using OpenQA.Selenium;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests.Pages
{
    public class NotificationPage : GridPage
    {
        public NotificationPageElement notificationPageElement => new NotificationPageElement();
        public void DismissNotification()
        {
            notificationPageElement.MoreBtn.Click();
            notificationPageElement.DimissNotification.Click();
        }
    }


    public class NotificationPageElement : CommonElements
    {
        #region Selectors
        private By _moreBtn = By.CssSelector("#MoreBtn");
        private By _dismissNotification = By.XPath("//div[text()='Dismiss Notification']");
        private By _dismissBtn = By.XPath("//button[contains(.,'Dismiss')]");
        #endregion

        #region Elements
        public WebElement MoreBtn => browser.FindWebElement(_moreBtn);
        public WebElement DimissNotification => browser.FindWebElement(_dismissNotification);
        public WebElement DismissBtn => browser.FindWebElement(_dismissBtn);
        #endregion
    }

}
