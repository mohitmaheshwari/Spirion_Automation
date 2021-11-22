using OpenQA.Selenium;
using Spiron.Automation.Framework.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Spirion.Automation.Framework.Elements
{
    public class WebElement
    {
        private IWebElement _element = null;
        public string elementName = null;
        public WebElement(IWebElement element, string _elementName)
        {
            _element = element;
            elementName = _elementName;
        }

        public CheckboxElement ToCheckBox()
        {
            return new CheckboxElement(_element, elementName);
        }

        public TextBoxElement ToTextBox()
        {
            return new TextBoxElement(_element, elementName);
        }

        public DropdownElement ToDropDown()
        {
            return new DropdownElement(_element, elementName);
        }

        public RadioButtonElement ToRadioButton()
        {
            return new RadioButtonElement(_element, elementName);
        }

        public JavascriptElement ToJavaScript()
        {
            return new JavascriptElement(_element, elementName);
        }

        public string GetValue()

        {
            try
            {
                Logger.LogInfo($"Getting Value for {elementName}: " + _element.Text);
                return _element.GetAttribute("value");
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetText()
        {
            try
            {
                Logger.LogInfo($"Getting Text for {elementName}: " + _element.Text);
                return _element.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsEnabled()
        {
            bool result;
            if (_element == null)
            {
                Logger.LogInfo(elementName + " is not found");
                return false;
            }
            try
            {
                result = _element.Enabled;
                Logger.LogInfo(elementName + " is Enabled.");
            }
            catch (Exception)
            {
                Logger.LogError(elementName + " is not Enabled.");
                result = false;
            }

            return result;
        }



        public bool Exists()
        {

            if (_element == null)
            {
                Logger.LogInfo(elementName + " is not found");
                return false;
            }
            Logger.LogInfo(elementName + "is found");
            return true;
        }

        public bool IsDisplayed()
        {
            bool result;
            if (_element == null)
            {
                Logger.LogInfo(elementName + " is not found");
                return false;
            }
            try
            {
                result = _element.Displayed;
                Logger.LogInfo(elementName + " is Displayed.");
            }
            catch (Exception)
            {
                Logger.LogError(elementName + " is not Displayed.");
                result = false;
            }

            return result;
        }



        public Point CoOrdinates()
        {
            var location = _element.Location;
            return location;
        }

        public void UploadFile(string path)
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
            _element.SendKeys(path);
        }

        public void Click()
        {
            try
            {
                _element.Click();
                Logger.LogInfo($"{elementName} is clicked");
            }
            catch
            {
                try
                {
                    new JavascriptElement(_element, "").ClickUsingJS();
                    Logger.LogInfo($"{elementName} is clicked");

                }
                catch (Exception ex)
                {
                    try
                    {
                        new WebElement(_element, "").ToAction().Click();
                        Logger.LogInfo($"{elementName} is clicked");
                    }
                    catch (Exception ex2)
                    {
                        Logger.LogError($"Throwing exception");
                        throw new Exception(ex2.Message);
                    }
                }
            }

        }

        public void ScrollToElement()
        {
            try
            {
                new JavascriptElement(_element, nameof(_element)).ScrollElementToView();
            }
            catch
            {

            }
        }

        public bool IsCheckBox()
        {
            string a = _element.GetAttribute("outerHTML");
            return _element.GetAttribute("outerHTML").Contains("check");
        }

        public int GetElementPointX()
        {
            return _element.Location.X;
        }

        public int GetElementPointY()
        {
            return _element.Location.Y;
        }

        public string GetAttribute(string attributeValue)
        {
            string value = _element.GetAttribute(attributeValue);
            Logger.LogInfo($"{elementName} {attributeValue} is value");
            return value;
        }


        public WebElement FindChildElement(By selector)
        {
            return new WebElement(_element.FindElement(selector), elementName);
        }

        public ActionHandler ToAction()
        {
            return new ActionHandler(_element);
        }

        public IWebElement Element => _element;

    }
    public class WebElements : IEnumerable<WebElement>
    {
        public string ElementName;
        public IList<WebElement> elements = null;
        public WebElements(IList<WebElement> _elements, string elementName)
        {
            elements = _elements;
            ElementName = elementName;
        }

        public bool AllHaveText()
        {
            return elements.All(x => string.IsNullOrEmpty(x.GetText()) == false);
        }

        public List<string> Text()
        {
            if (elements == null)
                return new List<string>();
            List<string> text = new List<string>();
            foreach (var element in elements)
            {
                Logger.LogInfo($"Text : {element.GetText()}");
                text.Add(element.GetText());
            }
            return text;
        }

        public List<string> Value()
        {
            if (elements == null)
                return new List<string>();
            return elements.Select(x => x.GetValue()).ToList();
        }

        public bool IsDisplayed()
        {
            bool result;
            try
            {
                result = elements.All(el => el.IsDisplayed());
                Logger.LogInfo(ElementName + " is Displayed.");
            }
            catch (Exception)
            {
                Logger.LogError(ElementName + " is not Displayed.");
                result = false;
            }

            return result;

        }

        public IEnumerator<WebElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public RadioButtonElements ToRadioButtons()
        {
            return new RadioButtonElements(elements, nameof(elements));
        }

        public CheckboxElements ToCheckBoxes()
        {
            return new CheckboxElements(elements, nameof(elements));
        }

    }
}


