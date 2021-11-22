using OpenQA.Selenium;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpirionUITests.Pages
{
    public class GridPage : BasePage
    {
        public GridLocators _gridLocators => new GridLocators();
        #region Method

        public List<string> GetAllValuesFromColumn(string columnaName)
        {
            int tries = 0;
            List<string> headers = _gridLocators.TableHeaders.Text();
            int index = headers.FindIndex(x => x.Contains(columnaName)) + 1;
            return _gridLocators.TableColumn(index).Text();
        }

        public string GetFilteredValueFromGrid(string columnName)
        {
            List<string> headers = _gridLocators.TableHeaders.Text();
            int index = headers.FindIndex(x => x.Contains(columnName)) + 1;
            return _gridLocators.TableColumn(index).Text().First();
        }

        public void WaitForGridToLoad()
        {
            browser.WaitUntilElementDisappears(_gridLocators._gridLoader);
        }

        public bool IsRowFoundAfterFilter(string row)
        {
            if (_gridLocators.MatchingRow(row).Exists() == false)
                return _gridLocators.NoMatchingRecords.Exists() == false;
            return true;
        }

        public void CllickConfirmDelete()
        {
            _gridLocators.ConfirmDelete.Click();
        }

        #endregion

    }

    public class GridLocators : CommonElements
    {
        #region Locators
        private By _tableHeaders = By.CssSelector("table th");
        private By _tableColumn(int index) => By.CssSelector($"table td:nth-child({index})");
        public By _gridLoader = By.CssSelector("div.loader--wrapper");
        private By _matchingRow(string value) => By.XPath($"//table[contains(.,'{value}')]");
        private By _NoMatchingRecords = By.XPath("//table[contains(.,'No matching records')] | //table[contains(.,'No data available')]");
        private By _confirmDelete = By.CssSelector("#AlpineMessageDialogConfirmOk");
        #endregion

        #region Elements
        public WebElements TableHeaders => browser.FindWebElements(_tableHeaders);
        public WebElements TableColumn(int index) => browser.FindWebElements(_tableColumn(index));
        public WebElement MatchingRow(string row) => browser.FindWebElementWithExplicitWait(0, _matchingRow(row));
        public WebElement NoMatchingRecords => browser.FindWebElementWithExplicitWait(0, _NoMatchingRecords);
        public WebElement ConfirmDelete => browser.FindWebElement(_confirmDelete);

        #endregion
    }
}
