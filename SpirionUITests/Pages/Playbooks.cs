using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System.Linq;
using SpirionUITests.Models;

namespace SpirionUITests.Pages
{
    public class Playbooks : GridPage
    {
        public static PageElementsOnPlaybook.PlaybooksUI PlaybooksUI => PageElementsOnPlaybook.playbooksUI;
        public static PageElementsOnPlaybook.PlaybooksDecisionUI PlaybookDecisionUI => PageElementsOnPlaybook.playbooksDecisionUI;

        public static PageElementsOnPlaybook.PlaybooksDecisionLogicUI PlaybooksDecisionLogicUI => PageElementsOnPlaybook.playbooksDecisionLogicUI;
        public void SetPlayBookName(string name)
        {
            PlaybooksUI.PlayBookNameTxt.EnterText(name);
        }

        public void SetPlaybookDescription(string description)
        {
            PlaybooksUI.PlayBookDescriptionText.EnterText(description);
        }

        public void SearchExistingPlayBook(string playbook)
        {
            Logger.LogInfo($"PlayBook Name : {playbook}");
            PlaybooksUI.SearchBox.EnterText(playbook);
            PlaybooksUI.SearchButton.Click();
        }

        public void CancelRecoveryPopupIfFound()
        {
            if (PlaybooksUI.CancelRecoveryPopup.IsDisplayed())
                PlaybooksUI.CancelRecoveryPopup.Click();
        }

        public bool IsPlayBookFound(string playBook)
        {
            return IsRowFoundAfterFilter(playBook);
        }

        public void AddPlayBook()
        {
            PlaybooksUI.ActionButton.Click();
            PlaybooksUI.AddPlaybook.Click();
        }

        public void ClickNext()
        {
            PlaybooksUI.NextBtn.Click();
        }

        public void SetLeftDecision(string option1)
        {
            PlaybookDecisionUI.SelectAction.Click();
            PlaybookDecisionUI.SelectMainActionFirst(option1).Click();
            if (option1 == "User Action")
            {
                PlaybookDecisionUI.UserActionNote.EnterText("Note producted by UI Automation");
            }
            else if (option1 == "Classification")
            {
                SelectClassificationValues(1);
                PlaybookDecisionUI.AutomatiedActionCheckFirst.Click();
            }
            else
            {
                PlaybookDecisionUI.AutomatiedActionCheckFirst.Click();
            }
            PlaybookDecisionUI.ActionNodeFirst.Click();
            PlaybookDecisionUI.CompleteActionNode.Click();
        }



        public void SetRightDecision(string option2)
        {
            PlaybookDecisionUI.SelectAction.Click();
            PlaybookDecisionUI.SelectMainActionSecond(option2).Click();
            if (option2 == "Shred")
            {
                PlaybookDecisionUI.AutomatiedActionCheckFirst.Click();
            }
            else if (option2 == "Quarantine" || option2 == "Redact" || option2 == "Take No Action" || option2 == "Ignore")
            {
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            else if (option2 == "Classification")
            {
                SelectClassificationValues(2);
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            else if (option2 == "User Action")
            {
                PlaybookDecisionUI.UserActionNote.EnterText("If SSN found delete it");
            }
            else if (option2 == "Execute Script")
            {
                SelectExecuteScript(ScannedData.Instance.ScriptName, 2);
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            else if (option2 == "Notify")
            {
                SelectNotifyDetails(2);
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            else if (option2 == "Assign")
            {
                SelectAssignAdmin(ScannedData.Instance.AssignedUser);
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            else if (option2 == "Restrict Access")
            {
                SelectRestrictedAccess();
                PlaybookDecisionUI.AutomatedActionCheckSecond.Click();
            }
            PlaybookDecisionUI.ActionNodeFirst.Click();
            PlaybookDecisionUI.CompleteActionNode.Click();
        }

        public string GetSelectedMiddleDropdownOption(int index)
        {
            return PlaybooksDecisionLogicUI.StepLogicFilterMiddleDropdownValue(index).GetText();
        }

        private void SelectNotifyDetails(int index)
        {
            PlaybookDecisionUI.CustomNotificationTemplate.Click();
            PlaybookDecisionUI.CustomNotificationTemplate.ToAction().SendKeys(ScannedData.Instance.TemplateName);
            PlaybookDecisionUI.TxtEmail.EnterText("john@test.com");
            PlaybookDecisionUI.Dropdown(index).ToList()[1].Click();
            PlaybookDecisionUI.AdminCheck.Click();
        }

        private void SelectAssignAdmin(string user)
        {
            PlaybookDecisionUI.Dropdown(2).ToList()[0].Click();
            PlaybookDecisionUI.Option(user).Click();
        }

        public void SetNewPlayBookDetails(string playBookName, string playBookDescription)
        {
            SetPlayBookName(playBookName);
            SetPlaybookDescription(playBookDescription);
            ClickContinue();
        }

        public void SavePlayBook()
        {
            PlaybookDecisionUI.SaveBtn.Click();
        }

        public void SelectExecuteScript(string scriptName, int index)
        {
            PlaybookDecisionUI.Dropdown(index).ToList()[0].Click();
            PlaybookDecisionUI.Option(scriptName).Click();
        }

        private void SelectClassificationValues(int index)
        {
            PlaybookDecisionUI.ClassificationActionDropdown(index).Click();
            PlaybookDecisionUI.ClassificationActionOption.Click();
            PlaybookDecisionUI.ClassificationSelectDropodown(index).Click();
            PlaybookDecisionUI.ClassificationSelectDropodown(index).ToAction().SendKeys("S");
            PlaybookDecisionUI.ClassificationSelectOption.Click();
            PlaybookDecisionUI.ClassificationTypeDropdown(index).Click();
            PlaybookDecisionUI.ClassificationTypeOption.Click();

        }

        public void ClickLogicButton()
        {
            PlaybookDecisionUI.LogicDecisionBtn.Click();
        }

        public void ClickContinue()
        {
            PlaybooksUI.Continue.Click();
        }
        public void EnterLogicName(string name)
        {
            PlaybooksDecisionLogicUI.StepLogicTxt.EnterText(name);
        }

        public void SelectLeftDropdownOptionFromLogic(string option, int index)
        {
            PlaybooksDecisionLogicUI.StepLogicFilterLeftDropdown(index).Click();
            PlaybooksDecisionLogicUI.StepLogicFilterDropdownOption(index, option).Click();

        }

        public string GetMiddleDropdownOptionFromLogic(int index)
        {
            return PlaybooksDecisionLogicUI.StepLogicFilterMiddleDropdownValue(index).GetText();
        }
        public void SelectMiddleDropdownOptionFromLogic(string option, int index)
        {
            PlaybooksDecisionLogicUI.StepLogicFilterMiddleDropdown(index).Click();
            if(option.Contains("Last X"))
            {
                string tempOption = "Last X";
                PlaybooksDecisionLogicUI.StepLogicFilterDropdownOption(index, tempOption).ScrollToElement();
            }

            else if(option.Contains("Older Than"))
            {
                string tempOption = "Older Than";
                PlaybooksDecisionLogicUI.StepLogicFilterMiddleDropdown(index).ToAction().SendKeys("Older");
                PlaybooksDecisionLogicUI.StepLogicFilterDropdownOption(index, tempOption).ScrollToElement();
            }

            PlaybooksDecisionLogicUI.StepLogicFilterDropdownOption(index, option).Click();
        }

        public void ClickAppendIcon(int index)
        {
            PlaybooksDecisionLogicUI.AppendIcon(index).Click();
        }

        public void SelectRightSideValueInLogic(string logicType, string scanItem, string logicTypeList, int index)
        {
            if (logicType.Equals("Data Types"))
            {
                ClickAppendIcon(index);
                List<string> scanItems = scanItem.Split(';').ToList();
                foreach (var item in scanItems)
                {
                    SelectAppendText(index, item);
                }
                ClickOK();
            }

            else if ((logicType.Equals("Access Date") || (logicType.Equals("Create Date")) || (logicType.Equals("Modify Date"))))
            {
                if (logicTypeList.Contains("X"))
                {
                    SetValuesForX();
                }
                else
                {
                    SetCalendar();
                }
            }
        }

        public void SelectAppendText(int index, string item)
        {
            PlaybooksDecisionLogicUI.SelectAppendItem(index, item).ScrollToElement();
            PlaybooksDecisionLogicUI.SelectAppendItem(index, item).Click();
        }

        public void ClickOK()
        {
            PlaybooksDecisionLogicUI.OkButton.Click();
        }

        public void ClickSaveLogic()
        {
            PlaybooksDecisionLogicUI.SaveBtn.Click();
        }

        public void WaitForPlayBooksToLoad()
        {
            WaitForGridToLoad();
        }

        public void ExeuteAction()
        {
            PlaybookDecisionUI.ExecuteAction.Click();
            PlaybookDecisionUI.Queue.Click();
            if (PlaybookDecisionUI.Exit.Exists())
            {
                PlaybookDecisionUI.Exit.Click();
            }
        }

        public void ClickAddRowLogic()
        {
            PlaybooksDecisionLogicUI.AddLogic.Click();
        }
        public void SelectRestrictedAccess()
        {
            PlaybookDecisionUI.Button_Dropdown(2).Click();
            PlaybookDecisionUI.Option(ScannedData.Instance.AssignedUser).Click();
        }

        public void SetValuesForX()
        {
            PlaybooksDecisionLogicUI.InputForXValues.EnterText("4");
        }

        public void SetCalendar()
        {
            if (PlaybooksDecisionLogicUI.CalendarInput.Exists())
            {
                PlaybooksDecisionLogicUI.CalendarInput.Click();
                PlaybooksDecisionLogicUI.CurrentDate.Click();
                PlaybooksDecisionLogicUI.CurrentTimeOk.Click();
            }
        }


        public void ClickLogicalOperator()
        {
            PlaybooksDecisionLogicUI.LogicalOperator.Click();
        }

        public void DeleteExistingPlaybook(string playbook)
        {
            WaitForPlayBooksToLoad();
            SearchExistingPlayBook(playbook);
            PlaybooksUI.PlayBookMoreOptions.Click();
            PlaybooksUI.DeletePlaybook.Click();
            CllickConfirmDelete();
        }
        

    }

    public class PageElementsOnPlaybook : GridLocators
    {
        public static PlaybooksUI playbooksUI => new PlaybooksUI();
        public static PlaybooksDecisionUI playbooksDecisionUI => new PlaybooksDecisionUI();
        public static PlaybooksDecisionLogicUI playbooksDecisionLogicUI => new PlaybooksDecisionLogicUI();
        public class PlaybooksUI : PageElementsOnPlaybook
        {
            #region Locators
            private By _actionButton = By.CssSelector("#PlaybookListComponentActionsBtn");
            private By _search = By.Id("PlaybookListComponentSearchbarSearchInput");
            private By _addPlayBook = By.CssSelector("#PlaybookListComponentActionsBtnAddPlaybook");
            private By _playBookNameTxt = By.CssSelector("#PlaybookNameField");
            private By _playBookDescriptionText = By.CssSelector("#PlaybookDescriptionField");
            private By _nextBtn = By.XPath("//button[contains(.,'Next')]");
            private By _continueBtn = By.XPath("//button[contains(.,'Continue')]");
            private By _searchButton = By.CssSelector("#PlaybookListComponentSearchbarSearchButton");
            private By _PlayBookTable = By.CssSelector("#PlaybookListComponentGrid");
            private By _cancelRecoveryPopUp = By.XPath("//div[contains(@class,'v-dialog--active')][contains(.,'Playbook Recovery')]//button[contains(.,'Cancel')]");
            private By _playbookMoreOptions = By.CssSelector("button[id*='PlaybookListMoreBtn']");
            private By _deletePlaybooks = By.XPath("//div[text()='Delete Playbook']");
            #endregion

            #region WebElements

            public WebElement ActionButton => browser.FindWebElement(_actionButton);
            public WebElement AddPlaybook => browser.FindWebElement(_addPlayBook);
            public TextBoxElement PlayBookNameTxt => browser.FindTextBox(_playBookNameTxt);
            public TextBoxElement PlayBookDescriptionText => browser.FindTextBox(_playBookDescriptionText);
            public WebElement Continue => browser.FindWebElement(_continueBtn);
            public WebElement NextBtn => browser.FindWebElement(_nextBtn);
            public TextBoxElement SearchBox => browser.FindTextBox(_search);
            public WebElement SearchButton => browser.FindWebElement(_searchButton);
            public WebElement PlayBookTable => browser.FindWebElement(_PlayBookTable);
            public WebElement PlayBookMoreOptions => browser.FindWebElement(_playbookMoreOptions);
            public WebElement DeletePlaybook => browser.FindWebElement(_deletePlaybooks);
            public WebElement CancelRecoveryPopup => browser.FindWebElement(_cancelRecoveryPopUp);
            #endregion
        }
        public class PlaybooksDecisionUI : PageElementsOnPlaybook
        {
            #region Page Locators
            private By _selectionAction = By.XPath("(//button[contains(.,'Select Action')])[1]");
            private By _selectFirstAction(string actionType) => By.XPath($"//div[contains(@class,'v-item-group')]/div[contains(.,'{actionType}')]");
            private By _selectSecondAction(string actionType) => By.XPath($"(//div[contains(@class,'v-item-group')]/div[contains(.,'{actionType}')])[2]");
            private By _userActionNote => By.CssSelector("textarea[id*='input']");
            private By _actionNodeFirst = By.XPath("(//div[contains(@class,'playbook__action-node')]/button)[1]");
            private By _actionNodeSecond = By.XPath("(//div[contains(@class,'playbook__action-node')]/button)[2]");
            private By _completeActionNode = By.XPath("//div[contains(@class,'v-list-item--link')]/div[contains(.,'Completed')]");
            private By _automatedActionCheckFirst = By.XPath("//label[contains(.,'Automated Action')]/preceding-sibling::div/input");
            private By _automationCheckSecond = By.XPath("(//label[contains(.,'Automated Action')]/preceding-sibling::div/input)[2]");
            private By _savePlaybookBtn = By.CssSelector("#PlaybookToolbarSaveBtn");
            private By _logicDecisionBtn = By.CssSelector("button.definition-name");
            private By _classificationDropdowns(int index) => By.XPath($"(//div[contains(@class,'node-action-wrapper')])[{index}]//div[contains(@class,'v-input__slot')][@role='button']");
            private By _classificationActionOption = By.XPath("//div[contains(@class,'v-list-item__title')][contains(.,'Perform Action on File and Database')]");
            private By _classificationTypeOption = By.XPath("//div[contains(@class,'v-list-item__title')][contains(.,'Add Classification')]");
            private By _classifcationSelectOption = By.XPath("//div[text()='Secret'][contains(@class,'v-list-item__title')]");
            private By _Dropdowns(int index) => By.XPath($"(//div[contains(@class,'node-action-wrapper')])[{index}]//div[contains(@class,'v-input__slot')][@role='combobox']");
            private By _Button_down(int index) => By.XPath($"(//div[contains(@class,'node-action-wrapper')])[{index}]//div[contains(@class,'v-input__slot')][@role='button']");
            private By _Option(string option) => By.XPath($"//div[contains(@class,'v-list-item__title')][contains(.,'{option}')]");
            private By _txtEmail = By.XPath("//label[contains(.,'Enter Email Address')]/following-sibling::div//input[@type='text']");
            private By _adminCheck = By.XPath("//div[@role='option'][contains(.,'Admin')]//div[@class='v-simple-checkbox']");
            private By _executeAction = By.XPath("//button[contains(.,'Execute Action')]");
            private By _queue = By.XPath("//button[contains(.,'Queue')]");
            private By _exit = By.XPath("//button[contains(.,'Exit')]");
            #endregion

            #region Page Elements

            public WebElement SelectAction => browser.FindWebElement(_selectionAction);

            public WebElement SelectMainActionFirst(string actionType) => browser.FindWebElement(_selectFirstAction(actionType));
            public WebElement SelectMainActionSecond(string actionType) => browser.FindWebElement(_selectSecondAction(actionType));
            public TextBoxElement UserActionNote => browser.FindTextBox(_userActionNote);
            public WebElement ActionNodeFirst => browser.FindWebElement(_actionNodeFirst);
            public WebElement ActionNodeSecond => browser.FindWebElement(_actionNodeSecond);
            public WebElement CompleteActionNode => browser.FindWebElement(_completeActionNode);
            public WebElement AutomatiedActionCheckFirst => browser.FindHiddenElement(_automatedActionCheckFirst);
            public WebElement AutomatedActionCheckSecond => browser.FindHiddenElement(_automationCheckSecond);
            public WebElement SaveBtn => browser.FindWebElement(_savePlaybookBtn);
            public WebElement LogicDecisionBtn => browser.FindWebElement(_logicDecisionBtn);
            public WebElement ClassificationActionDropdown(int index) => browser.FindWebElements(_classificationDropdowns(index)).ToList()[0];
            public WebElement ClassificationTypeDropdown(int index) => browser.FindWebElements(_classificationDropdowns(index)).ToList()[1];
            public WebElement ClassificationSelectDropodown(int index) => browser.FindWebElements(_classificationDropdowns(index)).ToList()[2];
            public WebElement ClassificationActionOption => browser.FindWebElement(_classificationActionOption);
            public WebElement ClassificationTypeOption => browser.FindWebElement(_classificationTypeOption);
            public WebElement ClassificationSelectOption => browser.FindWebElement(_classifcationSelectOption);
            public WebElements Dropdown(int index) => browser.FindWebElements(_Dropdowns(index));
            public WebElement ExecuteAction => browser.FindWebElement(_executeAction);
            public WebElement Queue => browser.FindWebElement(_queue);
            public WebElement Exit => browser.FindWebElement(_exit);
            public WebElement Option(string option) => browser.FindWebElement(_Option(option));
            public TextBoxElement TxtEmail => browser.FindTextBox(_txtEmail);
            public WebElement AdminCheck => browser.FindWebElement(_adminCheck);
            public WebElement CustomNotificationTemplate => browser.FindWebElement(_classificationDropdowns(2));
            public WebElement Button_Dropdown(int index) => browser.FindWebElement(_Button_down(index));

            #endregion
        }

        public class PlaybooksDecisionLogicUI : PageElementsOnPlaybook
        {
            #region Locators

            private By _stepLogicName = By.CssSelector("#ruleName");
            private By _StepLogicFilterLeftDropdown = By.XPath("(.//div[contains(@class,'v-select__selections')])[1]");
            private By _StepLogicFilterMiddleDropdown = By.XPath("(.//div[contains(@class,'v-select__selections')])[2]");
            private By _StepLogicFilterMiddleDropdownValue = By.XPath("(.//div[contains(@class,'v-select__selections')])[2]/div");
            private By _StepLogicFilterLeftDropdownOption(string option) => By.XPath($"//div[contains(@class,'v-list-item--link')][contains(.,'{option}')]");
            private By _appendIcon = By.XPath("//button[@aria-label='append icon']");
            private By _selectAppendedItem(string item) => By.XPath($"//td[contains(.,'{item}')]/following-sibling::td//button");
            private By _okButton = By.XPath("//button[contains(.,'OK')]");
            private By _saveBtn = By.XPath("//button[contains(.,'Save')]");
            private By _addLogic = By.XPath("(.//button)[3]");
            private By _filterRow(int index) => By.XPath($"(//div[@class='filter-item'])[{index}]");
            private By _calendarInput = By.XPath("//div[contains(@class,'calendar-input')]//input[@readonly]");
            private By xValuesInput = By.CssSelector("div.filter-item input[type='number']");
            private By _currentDate = By.CssSelector("button[class*='current']");
            private By _currentTimeOk = By.XPath("//button[contains(.,'OK')]");
            private By _logicalOperator = By.XPath("//button[contains(.,'And')]");

            #endregion
            #region Page Elements

            public TextBoxElement StepLogicTxt => browser.FindTextBox(_stepLogicName);
            public WebElement StepLogicFilterLeftDropdown(int index) => FilerRow(index).FindChildElement(_StepLogicFilterLeftDropdown);
            public WebElement StepLogicFilterMiddleDropdownValue(int index) => FilerRow(index).FindChildElement(_StepLogicFilterMiddleDropdownValue);
            public WebElement StepLogicFilterDropdownOption(int index, string option) => FilerRow(index).FindChildElement(_StepLogicFilterLeftDropdownOption(option));
            public WebElement AppendIcon(int index) => FilerRow(index).FindChildElement(_appendIcon);
            public WebElement SelectAppendItem(int index, string item) => FilerRow(index).FindChildElement(_selectAppendedItem(item));
            public WebElement OkButton => browser.FindWebElement(_okButton);
            public WebElement SaveBtn => browser.FindWebElement(_saveBtn);
            public WebElement StepLogicFilterMiddleDropdown(int index) => FilerRow(index).FindChildElement(_StepLogicFilterMiddleDropdown);
            public WebElement FilerRow(int index) => browser.FindWebElement(_filterRow(index));
            public WebElement AddLogic => FilerRow(1).FindChildElement(_addLogic);
            public WebElement CalendarInput => browser.FindWebElementWithoutWait(_calendarInput);
            public WebElement CurrentDate => browser.FindWebElement(_currentDate);
            public WebElement CurrentTimeOk => browser.FindWebElement(_currentTimeOk);
            public TextBoxElement InputForXValues => browser.FindTextBox(xValuesInput);
            public WebElement LogicalOperator => browser.FindWebElement(_logicalOperator);

            #endregion
        }

    }
}
