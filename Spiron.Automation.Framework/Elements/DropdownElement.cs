using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spirion.Automation.Framework.Elements
{
    public class DropdownElement : WebElement
    {

        public DropdownElement(IWebElement element, string elementName) : base(element,elementName)
        {

        }

        public void SelectDropdownByText(string text)
        {
            SelectElement oSelect = new SelectElement(Element);
            oSelect.SelectByText(text);
            Logger.LogInfo(text + " text selected on " + elementName);
        }

        public void SelectDropdownByIndex(int index)
        {
            SelectElement oSelect = new SelectElement(Element);
            oSelect.SelectByIndex(index);
            Logger.LogInfo(index + " index selected on " + elementName);
        }

        public void SelectDropdownByValue(string value)
        {
            SelectElement oSelect = new SelectElement(Element);
            oSelect.SelectByValue(value);
            Logger.LogInfo(value + " value selected on " + elementName);
        }

        public string GetSelectedItemText()
        {
            string text = string.Empty;
            SelectElement oSelect = new SelectElement(Element);
            text =  oSelect.SelectedOption.Text;
            Logger.LogInfo(text + " value selected on " + elementName);
            return text;
        }

        public int OptionsCount
        {
            get
            {
                SelectElement oSelect = new SelectElement(Element);
                return oSelect.Options.Count;
            }
        }

       


    }
}
