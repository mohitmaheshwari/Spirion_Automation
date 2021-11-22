using NUnit.Framework;
using SpirionUITests.Fixtures;
using SpirionUITests.Models;
using SpirionUITests.Pages;
using SpirionUITests.Pages.Settings;
using SpirionUITests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpirionUITests.Steps
{
    [Binding]
    public class DataAssetInventorySteps
    {

        private EnvironmentFixture environmentFixture;
        private DataAssetInventory dataAssetInventory;

        public DataAssetInventorySteps(EnvironmentFixture _environmentFixture, DataAssetInventory _dataAssetInventory)
        {

            environmentFixture = _environmentFixture;
            dataAssetInventory = _dataAssetInventory;
        }

        [Then(@"User add a new remote target agent")]
        public void ThenUserAddANewRemoteTargetAgent(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            dataAssetInventory.ClickDataAssetInventoryMenu();
            dataAssetInventory.WaitForTargetGridToLoad();
            dataAssetInventory.SelectTarget();
            dataAssetInventory.EnterSearchText(data.TargetName);
            dataAssetInventory.ClickSearchButton();
            bool isRemoteAgentFound = dataAssetInventory.IsRemoteAgentFound(data.TargetName);
            if (isRemoteAgentFound)
            {
                ScannedData.Instance.TargetMachine = data.TargetName;
                dataAssetInventory.GoBack();
                return;
            }
            dataAssetInventory.ClickActionButton();
            dataAssetInventory.ClickAddTarget();
            dataAssetInventory.EnterTargetName(data.TargetName);
            dataAssetInventory.SelectTargetType(data.TargetType);
            dataAssetInventory.SelectAddressType(data.AddressType);
            dataAssetInventory.EnterAddress(environmentFixture.Environment.RemoteTarget.MachineIP);
            dataAssetInventory.EnterUsername(environmentFixture.Environment.RemoteTarget.Username);
            dataAssetInventory.EnterPassword(environmentFixture.Environment.RemoteTarget.Password);
            dataAssetInventory.ClickSaveButton();
            AssertExt.IsTrue(dataAssetInventory.IsRemoteAgentFound(data.TargetName), $"Verify remot agent found with name {data.TargetName}");
            ScannedData.Instance.TargetMachine = data.TargetName;
            dataAssetInventory.GoBack();
        }

        [Then(@"User add a New Dropbox Cloud target agent")]
        public void ThenUserAddANewDropboxCloudTargetAgent(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            var dropBox = environmentFixture.Environment.Cloud.DropBox;
            dataAssetInventory.ClickDataAssetInventoryMenu();
            dataAssetInventory.WaitForTargetGridToLoad();
            dataAssetInventory.SelectTarget();
            dataAssetInventory.EnterSearchText(data.TargetName);
            dataAssetInventory.ClickSearchButton();
            dataAssetInventory.ClickSearchButton();
            bool isRemoteAgentFound = dataAssetInventory.IsRemoteAgentFound(data.TargetName);
            if (isRemoteAgentFound)
            {
                ScannedData.Instance.TargetMachine = data.TargetName;
                dataAssetInventory.GoBack();
                return;
            }
            dataAssetInventory.ClickActionButton();
            dataAssetInventory.ClickAddTarget();
            dataAssetInventory.EnterTargetName(data.TargetName);
            dataAssetInventory.SelectTargetType(data.TargetType);
            dataAssetInventory.SelectDropboxCloudType();
            dataAssetInventory.EnterAdminUserAccountName(dropBox.AdminAccount);
            dataAssetInventory.ClickAgentsCheckBoxes();
            dataAssetInventory.EnterAuthenticationCode(dropBox.AdminAccount, dropBox.Password);
            dataAssetInventory.EnterAppKey(dropBox.AppKey);
            dataAssetInventory.EnterAppSecret(dropBox.AppSecret);
            dataAssetInventory.EnterAccessToken(dropBox.AccessToken);
            dataAssetInventory.ClickSaveBtnDropboxPopup();
            string alert = dataAssetInventory.GetAlertText();
            if (dataAssetInventory.IsRemoteCloudAgentSaved(alert) == false)
            {
                string targetName = dataAssetInventory.GetOldTargetName(alert);
                dataAssetInventory.ClickCancel();
                dataAssetInventory.GoBack();
                dataAssetInventory.ClickDataAssetInventoryMenu();
                dataAssetInventory.SelectTarget();
                dataAssetInventory.WaitForTargetGridToLoad();
                dataAssetInventory.EnterSearchText(targetName);
                dataAssetInventory.ClickSearchButton();
                isRemoteAgentFound = dataAssetInventory.IsRemoteAgentFound(targetName);
                AssertExt.IsTrue(isRemoteAgentFound, $"Verify remot agent found with name {targetName}");
                dataAssetInventory.EditTarget();
                dataAssetInventory.ClickAgentsCheckBoxes();
                dataAssetInventory.ClickSaveBtnEditPopup();
                alert = dataAssetInventory.GetAlertText();
                AssertExt.IsTrue(dataAssetInventory.IsRemoteCloudAgentSaved(alert), "Verify Remote Cloud is saved");
                ScannedData.Instance.TargetMachine = targetName;
            }
          
        }



    }
}
