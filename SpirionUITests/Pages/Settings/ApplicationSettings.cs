using OpenQA.Selenium;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using Spiron.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests.Pages.Settings
{

    public class ApplicationSettings : SettingsPage
    {
        public ApplicationSettingsPageElement.MainPage mainPage => ApplicationSettingsPageElement.mainPage;
        public ApplicationSettingsPageElement.CustomNotificationPopup CustomNotificationPopup => ApplicationSettingsPageElement.customNotificationPopup;
        public void ClickOnNotifications()
        {
            mainPage.Notifications.Click();
        }

        public void ClickOnNewTemplateButton()
        {
            mainPage.NewTemplateButton.Click();
        }

        private void SetNotificationName(string name)
        {
            CustomNotificationPopup.Name.EnterText(name);
        }

        private void SetNotificationSubject(string subject)
        {
            CustomNotificationPopup.Subject.EnterText(subject);
        }

        public void ClickSaveButton()
        {
            CustomNotificationPopup.SaveButton.Click();
        }

        public void SelectAllInPagination()
        {
            mainPage.PaginationDropdown.Click();
            mainPage.PaginationDropdownOption.Click();
        }

        public bool IsNotificationAlreadyCreated(string notificationName)
        {
            return mainPage.NotificationFoundInTable(notificationName).Exists();
        }

        public void CreateNewTemplate(string templateName, string subject,string body)
        {
            ClickOnNewTemplateButton();
            SetNotificationName(templateName);
            SetNotificationSubject(subject);
            CustomNotificationPopup.Body.SendKeys(body);
            ClickSaveButton();
        }
    }

    public class ApplicationSettingsPageElement : SettingsPageElement
    {
        public static MainPage mainPage => new MainPage();
        public static CustomNotificationPopup customNotificationPopup => new CustomNotificationPopup();
        public class MainPage : ApplicationSettingsPageElement
        {
            #region Locators
            private By _notifications = By.XPath("//button[contains(.,'Notifications')]");
            private By _newTemplateBtn = By.XPath("//button[contains(.,'New Template')]");
            private By _paginationDropdown = By.XPath("//div[@class='v-select__selection v-select__selection--comma']");
            private By _paginationDropdownOption = By.XPath("//div[@class='v-list-item__title'][contains(.,'All')]");
            private By _notificationFoundinTable(string notification) => By.XPath($"//table[contains(.,'{notification}')]");
            #endregion

            #region Element
            public WebElement Notifications => browser.FindWebElement(_notifications);
            public WebElement NewTemplateButton => browser.FindWebElement(_newTemplateBtn);
            public WebElement PaginationDropdown => browser.FindWebElement(_paginationDropdown);
            public WebElement PaginationDropdownOption => browser.FindWebElement(_paginationDropdownOption);
            public WebElement NotificationFoundInTable(string notification) => browser.FindHiddenElement(_notificationFoundinTable(notification));
            #endregion
        }

        public class CustomNotificationPopup : ApplicationSettingsPageElement
        {
            #region Locator

            private By _name = By.CssSelector("#ManageCustomNotificationsDialogName");
            private By _subject = By.CssSelector("#ManageCustomNotificationsDialogSubject");
            private By _saveBtn = By.XPath("//button[contains(.,'Save')]");
            private By _body = By.CssSelector("div.ProseMirror");
          
            #endregion

            #region Element

            public TextBoxElement Name => browser.FindTextBox(_name);
            public TextBoxElement Subject => browser.FindTextBox(_subject);
            public WebElement SaveButton => browser.FindWebElement(_saveBtn);
            public ActionHandler Body => browser.FindWebElement(_body).ToAction();
            #endregion


        }



    }

}
