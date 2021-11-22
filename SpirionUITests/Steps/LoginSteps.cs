using SpirionUITests.Fixtures;
using SpirionUITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Spirion.Automation.Framework;

namespace SpirionUITests.Steps
{
    [Binding]
    public class LoginSteps
    {

        private LoginPage loginPage;
        private EnvironmentFixture environmentFixture;
        public LoginSteps(LoginPage _loginPage,EnvironmentFixture _environmentFixture)
        {
            loginPage = _loginPage;
            environmentFixture = _environmentFixture;
        }



        [Given(@"User is logged in")]
        public void GivenUserIsLoggedIn()
        {
            loginPage.GoToSpironUrl(environmentFixture.Environment.TestUrl);
            if (loginPage.AreYouOnLoginPage())
            {
                loginPage.EnterUserName(environmentFixture.Environment.User.Username);
                loginPage.EnterPassword(environmentFixture.Environment.User.Password);
                loginPage.ClickLoginButton();
                loginPage.ClickSkipButton();
            }
        }


    }
}
