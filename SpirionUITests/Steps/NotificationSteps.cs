using NUnit.Framework;
using SpirionUITests.Fixtures;
using SpirionUITests.Models;
using SpirionUITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpirionUITests.Steps
{
    [Binding]
    public class NotificationSteps
    {
        public NotificationPage notificationPage;
        public NotificationSteps(NotificationPage _notificationPage)
        {
            notificationPage = _notificationPage;
        }

        [Then(@"Verify User gets the notification once the scan is completed")]
        public void ThenVerifyUserGetsTheNotificationOnceTheScanIsCompleted()
        {
            notificationPage.WaitForGridToLoad();
            AssertExt.IsTrue(notificationPage.GetFilteredValueFromGrid("Subject").Equals(ScannedData.Instance.Subject), "Verify Subject matches");
         }

        [Then(@"Delete the notification from system")]
        public void ThenDeleteTheNotificationFromSystem()
        {
            notificationPage.DismissNotification();
        }


    }
}
