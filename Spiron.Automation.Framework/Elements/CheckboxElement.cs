using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spirion.Automation.Framework.Elements
{
    public class CheckboxElement : WebElement
    {
        public string ElementName = null;
        public CheckboxElement(IWebElement element, string ElementName) : base(element,ElementName)
        {

        }

        public void SetState(bool state)
        {
            if (Element.Selected != state)
                new WebElement(Element,"").Click();
            Logger.LogInfo($"{elementName} is set to {state} state");
        }

        public bool GetCheckBoxState()
        {
            Logger.LogInfo($"{elementName} state : {Element.Selected}");
            //return Element.GetAttribute("outerHTML").Contains("checked");
            return Element.Selected;
        }

    }

    public class CheckboxElements :  IEnumerable<CheckboxElement>
    {
        public IList<IWebElement> _webElements = null;
        public CheckboxElements(IList<WebElement> Elements, string elementName)
        {
            _webElements = Elements.Select(a => a.Element).ToList();
        }

        public List<string> GetSelectedCheckBoxesValueFromGroup()
        {
            Logger.LogInfo($"Getting selected CheckBox Buttons from Group");
            var elements = _webElements.Where(a => new CheckboxElement(a, nameof(a)).GetCheckBoxState() == true);
            return elements.Select(x => new CheckboxElement(x, nameof(x)).GetValue()).ToList();
        }

        public bool AreAllCheckBoxesCheckedInGroup()
        {
            return _webElements.All(a => new CheckboxElement(a, nameof(a)).GetCheckBoxState() == true);
        }

        public IEnumerator<CheckboxElement> GetEnumerator()
        {
            var checkboxElements = _webElements.Select(x => new CheckboxElement(x, nameof(x)));
            return checkboxElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
