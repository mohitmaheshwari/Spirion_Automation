using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System.Text.RegularExpressions;
using System.Linq;

namespace SpirionUITests.Pages
{
    public class DataAssetInventory : GridPage
    {
        public DataAssetInventoryPageElement.TargetMainPage targetMain = DataAssetInventoryPageElement.targetMain;
        public DataAssetInventoryPageElement.NewActionPopup newAction = DataAssetInventoryPageElement.newAction;
        public DataAssetInventoryPageElement.NewDropboxActionPopup dropboxActionPopup = DataAssetInventoryPageElement.dropboxActionPopup;
        public DataAssetInventoryPageElement.NewDropboxAuthentication dropboxAuthentication = DataAssetInventoryPageElement.dropboxAuthentication;
        public DataAssetInventoryPageElement.LeftSide leftSide = DataAssetInventoryPageElement.leftSide;


        #region Method

        public void SelectTarget()
        {
            targetMain.ClickTarget.Click();
        }

        public void EnterSearchText(string text)
        {
            targetMain.EnterSearchText.ClearText();
            targetMain.EnterSearchText.ToAction().SendKeys(text);
        }

        public void ClickSearchButton()
        {
            targetMain.SearchButton.Click();
        }

        public void EditTarget()
        {
            targetMain.MoreBtn.Click();
            targetMain.EditTargetBtn.Click();
        }

        public void ClickActionButton()
        {
            targetMain.ActionButton.Click();
        }

        public void ClickAddTarget()
        {
            targetMain.AddTarget.Click();
        }

        public void EnterTargetName(string name)
        {
            newAction.TargetName.EnterText(name);
        }

        public void SelectTargetType(string type)
        {
            newAction.TargetType(type).Click();
        }

        public bool IsRemoteAgentFound(string agent)
        {
            WaitForGridToLoad();
            return IsRowFoundAfterFilter(agent);
        }

        public void WaitForTargetGridToLoad()
        {
            WaitForGridToLoad();
        }


        public void SelectAddressType(string type)
        {
            newAction.AddressType.Click();
            newAction.AddressTypeOptions(type).Click();
        }

        public void EnterAddress(string address)
        {
            newAction.Address.EnterText(address);
        }

        public void EnterUsername(string username)
        {
            newAction.Username.EnterText(username);
        }

        public void EnterPassword(string password)
        {
            newAction.Password.EnterText(password);
        }

        public void ClickSaveButton()
        {
            newAction.SaveButton.Click();
        }

        public void SelectDropboxCloudType()
        {
            dropboxActionPopup.CloudSourceDropbox.Click();
        }

        public void EnterAdminUserAccountName(string accountName)
        {
            dropboxActionPopup.AdminAccountName.EnterText(accountName);
        }

        public void ClickAgentsCheckBoxes()
        {
            if (dropboxActionPopup.CloudAgentCheckbox.ToCheckBox().GetCheckBoxState() == false)
                dropboxActionPopup.CloudAgentCheckbox.Click();
            if (dropboxActionPopup.OnPremAgentCheckBox.ToCheckBox().GetCheckBoxState() == false)
                dropboxActionPopup.OnPremAgentCheckBox.Click();
        }

        public bool IsRemoteCloudAgentSaved(string text)
        {
            return text.Contains("successful");
        }

        public string GetAlertText()
        {
            return dropboxActionPopup.AlertPopup.GetText();
        }

        public void EnterAppKey(string key)
        {
            dropboxActionPopup.DropboxAppKey.EnterText(key);
        }

        public void EnterAppSecret(string secret)
        {
            dropboxActionPopup.DropboxAppSecret.EnterText(secret);
        }

        public void EnterAccessToken(string token)
        {
            dropboxActionPopup.DropboxAccessToken.EnterText(token);
        }

        public void ClickSaveBtnDropboxPopup()
        {
            dropboxActionPopup.SaveButtonDropBoxPopUp.ToList().First().Click();
        }

        public bool IsSaveButtonEditPopupDisabled()
        {
            return dropboxActionPopup.SaveButtonDropBoxPopUp.Last().GetAttribute("disabled").Equals("true");
        }

        public void ClickSaveBtnEditPopup()
        {
            dropboxActionPopup.SaveButtonDropBoxPopUp.ToList().Last().Click();
        }

        public void ClickBackBtn()
        {
            dropboxActionPopup.BackBtn.Click();
        }


        public void ClickCancel()
        {
            dropboxActionPopup.Cancel.Click();
        }

        public void EnterAuthenticationCode(string emailID, string password)
        {
            dropboxActionPopup.AuthenticationButton.Click();
            browser.SwitchToNewTab();
            browser.WaitUntilLoaded();
            dropboxAuthentication.Email.EnterText(emailID);
            dropboxAuthentication.ContinueBtn.ToAction().Click();
            dropboxAuthentication.EmailForSignIn.EnterText(emailID);
            dropboxAuthentication.NextBtn.ToAction().Click();
            dropboxAuthentication.Password.EnterText(password);
            dropboxAuthentication.SignInBtn.ToAction().Click();
            dropboxAuthentication.StaySignedIn.ToAction().Click();
            dropboxAuthentication.AllowBtn.ToAction().Click();
            string code = dropboxAuthentication.AuthenticationCode.GetValue();
            browser.SwitchToOriginalTab();
            dropboxActionPopup.AuthenticationCodeTextBox.EnterText(code);
        }

        internal string GetOldTargetName(string text)
        {
            Regex regex = new Regex("\'.*?\'");
            return regex.Match(text).Value.Trim('\'');
        }

        #endregion
    }

    public class DataAssetInventoryPageElement : CommonElements
    {
        public static TargetMainPage targetMain = new TargetMainPage();
        public static NewActionPopup newAction = new NewActionPopup();
        public static NewDropboxActionPopup dropboxActionPopup = new NewDropboxActionPopup();
        public static NewDropboxAuthentication dropboxAuthentication = new NewDropboxAuthentication();
        public static LeftSide leftSide = new LeftSide();
        public class TargetMainPage : DataAssetInventoryPageElement
        {
            #region Locator

            private By _clickTarget = By.XPath("//div[contains(.,' Targets ')][@role='tab']");
            private By _enterSearchText = By.XPath("(//input[@id='AlpinePageToolbarSearchbarSearchInput'])[2]");
            private By _searchButton = By.XPath("(//button[@id='AlpinePageToolbarSearchbarSearchButton'])[2]");
            private By _actionBtn = By.CssSelector("#AlpinePageToolbarActionsBtn");
            private By _addTarget = By.CssSelector("#AlpinePageToolbarActionsBtnAddTarget");
            private By _moreBtn = By.CssSelector("#dataTargetsTaggingGrid #MoreBtn");
            private By _editTarget = By.XPath("//div[contains(.,'Edit Target')][contains(@class,'title')]");
            #endregion

            #region Elements

            public WebElement ClickTarget => browser.FindWebElement(_clickTarget);
            public TextBoxElement EnterSearchText => browser.FindTextBox(_enterSearchText);
            public WebElement SearchButton => browser.FindWebElement(_searchButton);
            public WebElement ActionButton => browser.FindWebElement(_actionBtn);
            public WebElement AddTarget => browser.FindWebElement(_addTarget);
            public WebElement EditTargetBtn => browser.FindWebElement(_editTarget);
            public WebElement MoreBtn => browser.FindWebElement(_moreBtn);

            #endregion
        }

        public class NewActionPopup : DataAssetInventoryPageElement
        {
            #region Locator

            private By _targetName = By.CssSelector("#CardEndpoint-EndpointName");
            private By _targetType(string targettype) => By.XPath($"//button[contains(.,'{targettype}')]");
            private By _addressType => By.CssSelector("#AddRemoteMachineEndpointDialog-AddressType");
            private By _addressTypeOptions(string addresstype) => By.XPath($"//div[@role='option'][contains(.,'{addresstype}')]");
            private By _address = By.CssSelector("#AddRemoteMachineEndpointDialog-Address");
            private By _username = By.CssSelector("#AddRemoteMachineEndpointDialog-Username");
            private By _password = By.CssSelector("#AddRemoteMachineEndpointDialog-Password");
            private By _saveBtn = By.CssSelector("#AddRemoteMachineEndpointDialog-Save");
            #endregion

            #region Elements

            public TextBoxElement TargetName => browser.FindTextBox(_targetName);
            public WebElement TargetType(string targettype) => browser.FindWebElement(_targetType(targettype));
            public WebElement AddressType => browser.FindWebElement(_addressType);
            public WebElement AddressTypeOptions(string type) => browser.FindWebElement(_addressTypeOptions(type));
            public TextBoxElement Address => browser.FindTextBox(_address);
            public TextBoxElement Username => browser.FindTextBox(_username);
            public TextBoxElement Password => browser.FindTextBox(_password);
            public WebElement SaveButton => browser.FindWebElement(_saveBtn);

            #endregion
        }

        public class NewDropboxActionPopup : DataAssetInventoryPageElement
        {
            #region Locator

            private By _clouldSourceDropbox = By.XPath("(//div[contains(@class,'v-dialog')]//div[contains(@class,'v-image__image')])[1]");
            private By _adminAccountName = By.CssSelector("#AddCloudEndpointDialog-AdminAccountName");
            private By _onPremAgentCheckbox = By.XPath("(//input[@type='checkbox'])[1]");
            private By _cloudAgentCheckbox = By.XPath("(//input[@type='checkbox'])[2]");
            private By _authenticateBtn = By.CssSelector("#AddCloudEndpointDialog-DuelAuthModeAuthButton");
            private By _authenticationCodeTextBox = By.CssSelector("#AddCloudEndpointDialog-DuelAuthModeAuthCode");
            private By _dropboxAppKey = By.CssSelector("#AddCloudEndpointDialog-DropboxAppKey");
            private By _dropboxAppSecret = By.CssSelector("#AddCloudEndpointDialog-DropboxAppSecret");
            private By _dropboxAccessToken = By.CssSelector("#AddCloudEndpointDialog-DropboxAccessToken");
            private By _saveBtnDropboxPopup = By.XPath("//*[@id='AddCloudEndpointDialog-Save']");
            private By _backbtn = By.XPath("//div[contains(@id,'AddCloudEndpointDialogVCard')]//button[contains(.,'Back')]");
            private By _alertPopup = By.CssSelector("div[role='alert']");
            private By _btnCancel = By.XPath("//*[@id='AddCloudEndpointDialog-Save']/preceding-sibling::button[contains(.,'Cancel')]");
            #endregion

            #region Elements

            public TextBoxElement AdminAccountName => browser.FindTextBox(_adminAccountName);
            public WebElement CloudSourceDropbox => browser.FindStaleElement(_clouldSourceDropbox);
            public WebElement OnPremAgentCheckBox => browser.FindHiddenElement(_onPremAgentCheckbox);
            public WebElement CloudAgentCheckbox => browser.FindHiddenElement(_cloudAgentCheckbox);
            public WebElement AuthenticationButton => browser.FindWebElement(_authenticateBtn);
            public TextBoxElement AuthenticationCodeTextBox => browser.FindTextBox(_authenticationCodeTextBox);
            public TextBoxElement DropboxAppKey => browser.FindTextBox(_dropboxAppKey);
            public TextBoxElement DropboxAppSecret => browser.FindTextBox(_dropboxAppSecret);
            public TextBoxElement DropboxAccessToken => browser.FindTextBox(_dropboxAccessToken);
            public WebElements SaveButtonDropBoxPopUp => browser.FindWebElements(_saveBtnDropboxPopup);
            public WebElement BackBtn => browser.FindWebElement(_backbtn);
            public WebElement AlertPopup => browser.FindWebElement(_alertPopup);
            public WebElement Cancel => browser.FindWebElement(_btnCancel);
            #endregion

        }

        public class NewDropboxAuthentication : DataAssetInventoryPageElement
        {
            #region Locator

            private By _email = By.CssSelector("div.login-email input");
            private By _continueBtn = By.XPath("//button[contains(.,'Continue')]");
            private By _enterEmailForSignIn = By.CssSelector("input.form-control");
            private By _nextBtn = By.CssSelector("#idSIButton9");
            private By _password = By.XPath("//input[@type='password']");
            private By _signInBtn = By.CssSelector("#idSIButton9");
            private By _allowBtn = By.XPath("//button[contains(.,'Allow')]");
            private By _authenticationCode = By.CssSelector("input.auth-box");
            private By _staySignedIn = By.CssSelector("input[type = 'submit']");

            #endregion

            #region Elements

            public TextBoxElement Email => browser.FindTextBox(_email);
            public WebElement ContinueBtn => browser.FindStaleElement(_continueBtn);
            public TextBoxElement EmailForSignIn => browser.FindStaleElement(_enterEmailForSignIn).ToTextBox();
            public WebElement NextBtn => browser.FindStaleElement(_nextBtn);
            public TextBoxElement Password => browser.FindStaleElement(_password).ToTextBox();
            public WebElement SignInBtn => browser.FindWebElement(_signInBtn);
            public WebElement AllowBtn => browser.FindStaleElement(_allowBtn);
            public WebElement AuthenticationCode => browser.FindStaleElement(_authenticationCode);
            public WebElement StaySignedIn => browser.FindStaleElement(_staySignedIn);

            #endregion


        }



        public class LeftSide
        {
            #region Locator


            #endregion

            #region Elements


            #endregion
        }



    }

}



