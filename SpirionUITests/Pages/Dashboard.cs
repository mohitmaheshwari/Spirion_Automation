using SpirionUITests.Fixtures;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Spirion.Automation.Framework;
using Spirion.Automation.Framework.Elements;

namespace SpirionUITests.Pages
{
    public class Dashboard : GridPage
    {
        
        #region Method

        public void WaitForDashboardToLoad()
        {
            WaitForGridToLoad();
        }
        #endregion
    }

    public class PageElementsDashboard : CommonElements
    {
        public static DashboardMain dashboardMain => new DashboardMain();
        
        public class DashboardMain:PageElementsDashboard
        {
            #region Locators

            public  By _systemOverview = By.CssSelector("tspan.dataLabel");
            public  By _powerBIFrame = By.CssSelector("iframe[src*='powerbi']");

            #endregion

            #region PageElement
            public WebElement Frame => browser.FindWebElement(_powerBIFrame);
            public WebElements SystemOverView => browser.FindWebElements(_systemOverview);
        
            #endregion
        }



    }

}
