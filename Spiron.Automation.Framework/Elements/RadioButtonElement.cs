using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
namespace Spirion.Automation.Framework.Elements
{
    public class RadioButtonElement : WebElement
    {
        public RadioButtonElement(IWebElement element, string elementName) : base(element, elementName)
        {

        }

        public void SelectRadioButton()
        {
            Element.Click();
            Logger.LogInfo($"{elementName} is clicked");
        }

        public bool IsRadioButtonSelected()
        {
            Logger.LogInfo($"{elementName} select status is {Element.Selected}");
            return Element.Selected;
        }
    }

    public class RadioButtonElements : WebElements
    {
        public IList<IWebElement> _webElements = null;
        public RadioButtonElements(IList<WebElement> Elements, string elementName) : base(Elements, elementName)
        {
            _webElements = Elements.Select(a => a.Element).ToList();
        }

        public string GetSelectedRadioButtonNameFromGroup()
        {
            Logger.LogInfo($"Getting selected Radio Button from Group");
            var element = _webElements.Single(a => new RadioButtonElement(a, nameof(a)).IsRadioButtonSelected());
            return new RadioButtonElement(element, nameof(element)).GetValue();
        }
    }
}
