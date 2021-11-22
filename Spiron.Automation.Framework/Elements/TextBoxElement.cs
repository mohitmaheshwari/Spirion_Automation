using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spirion.Automation.Framework.Elements
{
    public class TextBoxElement : WebElement
    {
        public TextBoxElement(IWebElement element, string elementName) : base(element, elementName)
        {

        }

        public void EnterTextAndPressEnter(string text)
        {
            if (Element.GetAttribute("readonly") == null)
            {
                Logger.LogInfo($"Enter Text { text} on {elementName}");
                Element.SendKeys(text+ Keys.Return);
                
            }
            else
            {
                Logger.LogInfo($"{elementName} is Readonly");
            }
        }

        public void EnterText(string text, bool clearExistingText = true)
        {
            if (Element.GetAttribute("readonly") == null)
            {
                Logger.LogInfo($"Enter Text { text} on {elementName}");
                if (clearExistingText) Element.Clear();
                Element.SendKeys(text);
            }
            else
            {
                Logger.LogInfo($"{elementName} is Readonly");
            }
        }

        /// <summary>
        /// Clear the text from textbox
        /// </summary>
        public void ClearText()
        {
            Logger.LogInfo("Clearing Text");
            Element.Clear();
        }
    }

    public class TextBoxElements : WebElements
    {
        public IList<IWebElement> _webElements = null;
        public TextBoxElements(IList<WebElement> Elements, string elementName) : base(Elements, elementName)
        {
            _webElements = Elements.Select(a => a.Element).ToList();
        }
    }
}
