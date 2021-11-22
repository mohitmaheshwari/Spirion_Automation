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
    public class ScanResults : GridPage
    {
        public ScanResultsElementOnPage scanResultsElement => new ScanResultsElementOnPage();

        private bool AreAllColumnChecked()
        {
            return scanResultsElement.ColumnCheckboxes.AreAllCheckBoxesCheckedInGroup();
        }

        public int ColumnCount => _gridLocators.TableHeaders.Count();

        public void CheckAllColumns()
        {
            OpenCustomizeColumnBox();
            if (AreAllColumnChecked())
                return;
            int count = scanResultsElement.ColumnCheckboxes.Count();
            for(int index=0;index<count;index++)
            {
                scanResultsElement.ColumnCheckboxes.ToList()[index].SetState(true);
            }
            ClickCustomize();
        }

        public void EnterSearchText(string scan)
        {
            scanResultsElement.SearchBox.EnterText(scan);
        }

        private void ClickCustomize()
        {
            scanResultsElement.CustomizeFooterBtn.Click();
        }

        private void OpenCustomizeColumnBox()
        {
            scanResultsElement.ActionsBtn.Click();
            scanResultsElement.CustomizeMenu.Click();
        }

    }

    public class ScanResultsElementOnPage : GridLocators
    {
        #region Locators
        private By _actionsBtn = By.CssSelector("#SearchResultsComponentActionsBtn");
        private By _customizeMenuBtn = By.XPath("//div[text()='Customize Columns']");
        private By _columnChecks = By.CssSelector("div.v-card__text input");
        private By _customizeFooterBtn = By.XPath("//button[contains(.,'Customize')]");
        private By _search = By.CssSelector("#SearchResultsComponentSearchbarSearchInput");

        #endregion
        #region PageElements

        public WebElement ActionsBtn => browser.FindWebElement(_actionsBtn);
        public WebElement CustomizeMenu => browser.FindWebElement(_customizeMenuBtn);
        public CheckboxElements ColumnCheckboxes => browser.FindWebElements(_columnChecks).ToCheckBoxes();
        public WebElement CustomizeFooterBtn => browser.FindWebElement(_customizeFooterBtn);
        public TextBoxElement SearchBox => browser.FindTextBox(_search);
        #endregion
    }

}
