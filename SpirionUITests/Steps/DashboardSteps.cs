using SpirionUITests.Fixtures;
using SpirionUITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpirionUITests.Steps
{
    [Binding]
    public class DashboardSteps
    {
        private Dashboard dashboard;
        private EnvironmentFixture environmentFixture;
        public DashboardSteps(Dashboard _dashboard, EnvironmentFixture _environmentFixture)
        {
            dashboard = _dashboard;
            environmentFixture = _environmentFixture;
        }

        [Then(@"User is waiting for the dashboard to load")]
        public void GivenUserIsWaitingForTheDashboardToLoad()
        {
            dashboard.WaitForDashboardToLoad();
        }

    }
}
