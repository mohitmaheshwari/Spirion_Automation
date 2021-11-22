using SpirionUITests.Fixtures;
using SpirionUITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

using TechTalk.SpecFlow;

namespace SpirionUITests.Steps
{
    [Binding]
    public class LeftSideBarCommonSteps
    {
        private BasePage basePage;
        private EnvironmentFixture environmentFixture;

        public LeftSideBarCommonSteps(BasePage _basePage, EnvironmentFixture _environmentFixture)
        {
            basePage = _basePage;
            environmentFixture = _environmentFixture;
        }

        [Then(@"User selects to click on new scan")]
        public void ThenUserSelectsToClickOnNewScan()
        {
            basePage.ClickScan();
        }

        [Then(@"User selects to click on Notifications")]
        public void ThenUserSelectsToClickOnNotifications()
        {
            basePage.ClickNotification();
        }


    }
}
