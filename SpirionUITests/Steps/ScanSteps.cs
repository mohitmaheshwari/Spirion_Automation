using NUnit.Framework;
using SpirionUITests.Fixtures;
using SpirionUITests.Models;
using SpirionUITests.Pages;
using SpirionUITests.Pages.Settings;
using SpirionUITests.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpirionUITests.Steps
{
    [Binding]
    public class ScanSteps
    {
        private ScanPage scan;
        private Playbooks playbooks;
        private EnvironmentFixture environmentFixture;
        private AllScanPage allScan;
        private ScriptRepository scriptRepository;
        private ApplicationSettings applicationSettings;
        private ScanResults scanResults;
        public ScanSteps(ScanPage _scan, Playbooks _playbooks, AllScanPage _allScan, ScriptRepository _scriptRepository, ApplicationSettings _applicationSettings, ScanResults _scanResults, EnvironmentFixture _environmentFixture)
        {
            scan = _scan;
            playbooks = _playbooks;
            environmentFixture = _environmentFixture;
            allScan = _allScan;
            scanResults = _scanResults;
            scriptRepository = _scriptRepository;
            applicationSettings = _applicationSettings;
        }

        [Then(@"User finds the created playbook or create new one '(.*)','(.*)','(.*)', '(.*)','(.*)','(.*)' ,'(.*)' and '(.*)'")]
        public void ThenUserFindsTheCreatedPlaybookOrCreateNewOneAnd(string playBookName, string playBookDescription, string LogicName, string LogicType, string LogicTypeOptions, string LeftDecision, string RightDecision, string scanItem)
        {
            if (RightDecision.Contains("Assign"))
            {
                ScannedData.Instance.AssignedUser = RightDecision.Contains("User") ? "ClientAdmin@spirion.com" : "Admin";
                RightDecision = "Assign";
            }
            else if (RightDecision.Contains("Restrict"))
            {
                ScannedData.Instance.AssignedUser = RightDecision.Contains("Administrator") ? "Administrator" : "File Owner";
                RightDecision = "Restrict Access";
            }

            scan.ClickScanPlayBooks();
            playbooks.WaitForPlayBooksToLoad();
            playbooks.SearchExistingPlayBook(playBookName);
            bool isPlayBookFound = playbooks.IsPlayBookFound(playBookName);
            ScannedData.Instance.PlaybookName = playBookName;
            List<string> keywords = new List<string> { "On", "Before", "After" };
            if (isPlayBookFound)
            {
                if (keywords.Any(x => ScannedData.Instance.PlaybookName.Contains(x)))
                {

                    playBookName = playBookName + Guid.NewGuid().ToString().Substring(0, 6);
                    ScannedData.Instance.PlaybookName = playBookName;
                }
                else return;
            }
            playbooks.AddPlayBook();
            playbooks.CancelRecoveryPopupIfFound();
            playbooks.SetNewPlayBookDetails(playBookName, playBookDescription);
            playbooks.ClickLogicButton();
            playbooks.EnterLogicName(LogicName);
            List<string> LogicTypes = LogicType.Split(';').ToList();
            List<string> logictypeOptionList = LogicTypeOptions.Split(';').ToList();
            for (int index = 1; index <= LogicTypes.Count; index++)
            {
                playbooks.SelectLeftDropdownOptionFromLogic(LogicTypes[index - 1], index);
                if (logictypeOptionList[index - 1] != playbooks.GetSelectedMiddleDropdownOption(index))
                    playbooks.SelectMiddleDropdownOptionFromLogic(logictypeOptionList[index - 1], index);
                playbooks.SelectRightSideValueInLogic(LogicTypes[index - 1], scanItem, logictypeOptionList[index - 1], index);
                if (index < LogicTypes.Count)
                {
                    playbooks.ClickAddRowLogic();

                }
            }
            if (playBookName.Contains("Or"))
            {
                playbooks.ClickLogicalOperator();
            }
            playbooks.ClickSaveLogic();
            playbooks.SetLeftDecision(LeftDecision);
            playbooks.SetRightDecision(RightDecision);
            playbooks.SavePlayBook();
            playbooks.WaitForPlayBooksToLoad();
            playbooks.SearchExistingPlayBook(playBookName);
            AssertExt.IsTrue(playbooks.IsPlayBookFound(playBookName), "Verify Playbook is created");
        }




        [Then(@"User creates a new Discovery scan with '(.*)', '(.*)', '(.*)','(.*)', '(.*)', '(.*)'")]
        public void ThenUserCreatesANewDiscoveryScanWith(string name, string description, string scanType, string targetType, string locationType, string folderPath)
        {
            ScannedData.Instance.ScanName = name + Guid.NewGuid().ToString().Substring(0, 6);
            scan.CreateNewScan();
            scan.SetScanName(ScannedData.Instance.ScanName);
            scan.SetScanDescription(description);
            scan.ClickNext();
            scan.SelectMetaDataScan();
            if (scanType.Contains("Cloud"))
            {
                List<string> scanInfo = scanType.Split(';').ToList();
                scan.SelectScanTargetType(scanInfo.First());
                scan.SelectCloud(scanInfo.Last());
            }
            else
            {
                scan.SelectScanTargetType(scanType);
            }
            if (scanType.Contains("Cloud") == false)
                scan.SelectScanTargetType(targetType);
            scan.SearchTargetToScan(string.IsNullOrEmpty(ScannedData.Instance.TargetMachine) ? environmentFixture.Environment.LocalAgent.MachineName : ScannedData.Instance.TargetMachine);
            scan.ClickNext();
            if (scanType.Contains("Cloud") == false)
            {
                scan.SearchLocationType(locationType);
                if (targetType.Contains("Remote"))
                    folderPath = string.Format(environmentFixture.Environment.RemoteTarget.RemoteBasePath, environmentFixture.Environment.RemoteTarget.MachineIP) + folderPath;
                else
                    folderPath = $"{environmentFixture.Environment.BasePath}{folderPath}";
                scan.IncludePath(folderPath);
                ScannedData.Instance.DiscoveryFiles = ScannedData.Instance.DiscoveryFiles.Select(x => Path.Combine(folderPath, Path.GetFileName(x))).ToList();
            }
            else
            {
                scan.EnterCloudAccount(environmentFixture.Environment.Cloud.DropBox.UserAccount);
            }
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            if (scanType.Contains("Cloud"))
            {
                scan.SelectOnPRemAgent();
                scan.SelectAgentToScanRemoteMachine(environmentFixture.Environment.LocalAgent.MachineName);
            }

            if (targetType.Contains("Remote"))
            {
                scan.SelectAgentToScanRemoteMachine(environmentFixture.Environment.LocalAgent.MachineName);
            }
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickFinish();
        }


        [Then(@"User creates a new SSD scan with '(.*)', '(.*)', '(.*)', '(.*)','(.*)', '(.*)', '(.*)'")]
        public void ThenUserCreatesANewScanWith(string name, string description, string playbook, string scanType, string targetType, string locationType, string folderPath)
        {
            ScannedData.Instance.ScanName = name + Guid.NewGuid().ToString().Substring(0, 6);
            scan.CreateNewScan();
            scan.SetScanName(ScannedData.Instance.ScanName);
            scan.SetScanDescription(description);
            scan.ClickNext();
            scan.SelectSensitiveDataScan();
            scan.SelectPlayBook(ScannedData.Instance.PlaybookName);
            scan.ClickNext();
            if (scanType.Contains("Cloud"))
            {
                List<string> scanInfo = scanType.Split(';').ToList();
                scan.SelectScanTargetType(scanInfo.First());
                scan.SelectCloud(scanInfo.Last());
            }
            else
            {
                scan.SelectScanTargetType(scanType);
                scan.SelectScanTargetType(targetType);
            }
            scan.SearchTargetToScan(string.IsNullOrEmpty(ScannedData.Instance.TargetMachine) ? environmentFixture.Environment.LocalAgent.MachineName : ScannedData.Instance.TargetMachine);
            scan.ClickNext();
            if (scanType.Contains("Cloud") == false)
            {
                scan.SearchLocationType(locationType);
                if (targetType.Contains("Remote"))
                    folderPath = string.Format(environmentFixture.Environment.RemoteTarget.RemoteBasePath, environmentFixture.Environment.RemoteTarget.MachineIP) + folderPath;
                else
                    folderPath = $"{environmentFixture.Environment.BasePath}{folderPath}";
                ScannedData.Instance.DiscoveryFiles = ScannedData.Instance.DiscoveryFiles.Select(x => Path.Combine(folderPath,Path.GetFileName(x))).ToList();
                scan.IncludePath(folderPath);
            }
            else
            {
                scan.EnterCloudAccount(environmentFixture.Environment.Cloud.DropBox.UserAccount);
            }
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            if (scanType.Contains("Cloud"))
            {
                scan.SelectOnPRemAgent();
                scan.SelectAgentToScanRemoteMachine(environmentFixture.Environment.LocalAgent.MachineName);
            }
            if (targetType.Contains("Remote"))
            {
                scan.SelectAgentToScanRemoteMachine(environmentFixture.Environment.LocalAgent.MachineName);
            }
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickNext();
            scan.ClickFinish();
        }

        [Then(@"User uploads script to the repositiory")]
        public void ThenUserUploadsScriptToTheRepositiory(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            scriptRepository.GoToSettingsPage();
            scriptRepository.OpenScriptRepository();
            AssertExt.IsTrue(scriptRepository.IsScriptRepositoryPageOpened(), "Verify Script Repository page is opened");
            if (scriptRepository.IsScriptFound(data.ScriptName) == false)
            {
                scriptRepository.AddScript();
                scriptRepository.UploadScript(data.ScriptName, data.ScriptDescription, data.Path);
                AssertExt.IsTrue(scriptRepository.IsScriptFound(data.ScriptName), "Verify Script Exists in reporsitory");
            }
            scriptRepository.SetStatus(data.ScriptName);
            scriptRepository.GoBack();
            ScannedData.Instance.ScriptName = data.ScriptName;
        }

        [Then(@"User creates a custom notification template")]
        public void ThenUserCreatesACustomNotificationTemplate(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            applicationSettings.GoToSettingsPage();
            applicationSettings.OpenApplicationSettings();
            applicationSettings.ClickOnNotifications();
            applicationSettings.SelectAllInPagination();
            if (applicationSettings.IsNotificationAlreadyCreated(data.TemplateName) == false)
            {
                applicationSettings.CreateNewTemplate(data.TemplateName, data.Subject, data.Body);
                AssertExt.IsTrue(applicationSettings.IsNotificationAlreadyCreated(data.TemplateName), "Verify template is created");
            }
            applicationSettings.GoBack();
            applicationSettings.GoBack();
            ScannedData.Instance.TemplateName = data.TemplateName;
            ScannedData.Instance.Subject = data.Subject;
        }



        [Then(@"Run the scan and wait for scan to complete")]
        public void ThenRunTheScanAndWaitForScanToComplete()
        {
            allScan.EnterSearchTextForScan(ScannedData.Instance.ScanName);
            allScan.WaitForAllScansToLoad();
            allScan.RunScan();
            AssertExt.IsTrue(allScan.WaitForScanToComplete(ScannedData.Instance.ScanName), "Verify Scan is completed");
        }

        [Then(@"Open Search Scan Results and verify all files are discovered")]
        public void ThenOpenSearchScanResultsAndVerifyAllFilesAreDiscovered()
        {
            int tries = 0;
            allScan.OpenSearchResultOfScan();
            scanResults.WaitForGridToLoad();
            if (scanResults.ColumnCount < 17)
            {
                scanResults.CheckAllColumns();
            }
            while (allScan.GetFilteredValueFromGrid("Playbook Status").Equals("User Action Required") == false && tries++ < 3)
            {
                Thread.Sleep(5000);
                allScan.HitRefresh();
                scanResults.EnterSearchText(ScannedData.Instance.ScanName);
            }
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Location").Count == ScannedData.Instance.DiscoveryFiles.Count, "Verify Record Count matches");
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Playbook Status").All(x => x.Equals("User Action Required")), "Verify Playbook Status is User Action Required");
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Playbook(s)").All(x => x.Equals("Discovery")), "Verify Playbook Status is User Action Required");
            List<string> difference = allScan.GetAllValuesFromColumn("Location").Except(ScannedData.Instance.DiscoveryFiles, StringComparer.OrdinalIgnoreCase).ToList();
            AssertExt.IsTrue(difference.Count == 0, "Verify Location matches");

        }

        [Then(@"Open Search Scan Results and verify all details '(.*)'")]

        public void ThenOpenSearchScanResultsAndVerifyAllDetails(string resolution)
        {
            int tries = 0;
            allScan.OpenSearchResultOfScan();
            scanResults.WaitForGridToLoad();
            if (scanResults.ColumnCount < 17)
            {
                scanResults.CheckAllColumns();
            }
            while (allScan.GetFilteredValueFromGrid("Playbook Status").Equals("Complete") == false)
            {
                Thread.Sleep(5000);
                allScan.HitRefresh();
            }
            bool resolutionMatch = allScan.GetFilteredValueFromGrid("Resolution").Contains(resolution);
            while (resolutionMatch == false && tries++ < 10)
            {
                Thread.Sleep(30 * 1000);
                allScan.HitRefresh();
                scanResults.EnterSearchText(ScannedData.Instance.ScanName);
                allScan.WaitForGridToLoad();
                resolutionMatch = allScan.GetFilteredValueFromGrid("Resolution").Contains(resolution);
            }
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Match").Equals(ScannedData.Instance.SSN.First()), "Verify matching with SSN");
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Resolution").Contains(resolution), "Verify Resolution is correct");
        }

        [Then(@"Open Search Scan Results and verify assign details")]
        public void ThenOpenSearchScanResultsAndVerifyAssignDetails()
        {
            allScan.OpenSearchResultOfScan();
            allScan.WaitForGridToLoad();
            while (allScan.GetFilteredValueFromGrid("Playbook Status").Equals("Complete") == false)
            {
                Thread.Sleep(5000);
                allScan.HitRefresh();
                scanResults.EnterSearchText(ScannedData.Instance.ScanName);
            }
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Match").Equals(ScannedData.Instance.SSN.First()), "Verify matching with SSN");
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Assignee").Equals(ScannedData.Instance.AssignedUser), "Verify Assigned User is correct");
        }


        [Then(@"Open Search Scan Results and verify user action details")]
        public void ThenOpenSearchScanResultsAndVerifyUserActionDetails()
        {
            int tries = 0;
            allScan.OpenSearchResultOfScan();
            allScan.WaitForGridToLoad();
            while (allScan.GetFilteredValueFromGrid("Playbook Status").Equals("User Action Required") == false && tries++ < 3)
            {
                Thread.Sleep(5000);
                allScan.HitRefresh();
                scanResults.EnterSearchText(ScannedData.Instance.ScanName);
            }
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Playbook Status").Equals("User Action Required"), "Verify Playbook Status is User Action Required");
            allScan.ClickPlayBook();
            playbooks.ExeuteAction();
            scan.ClickScan();
            allScan.WaitForAllScansToLoad();
            allScan.EnterSearchTextForScan(ScannedData.Instance.ScanName);
            allScan.OpenSearchResultOfScan();
            tries = 0;
            while (allScan.GetFilteredValueFromGrid("Playbook Status").Equals("Complete") == false && tries++ < 3)
            {
                Thread.Sleep(5000);
                allScan.HitRefresh();
            }
            AssertExt.IsTrue(allScan.GetFilteredValueFromGrid("Playbook Status").Equals("Complete"), "Verify Playbook status is complete");
        }


        [Then(@"Open Search Scan Results and verify all details for Multiple Classification '(.*)'")]
        public void ThenOpenSearchScanResultsAndVerifyAllDetailsForMultipleClassification(string resolution)
        {
            allScan.OpenSearchResultOfScan();
            allScan.WaitForGridToLoad();
            List<string> dataTypeFromGrid = allScan.GetAllValuesFromColumn("Data Type");
            List<string> matches = allScan.GetAllValuesFromColumn("Match");
            for (int count = 0; count < dataTypeFromGrid.Count; count++)
            {
                string dataTypeValue = dataTypeFromGrid[count].Replace(" ", "");
                if (dataTypeValue == DataTypes.SocialSecurityNumber.ToString())
                {
                    AssertExt.IsTrue(matches.Contains(ScannedData.Instance.SSN.First()), "Verify SSN Matches");
                }
            }
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Classification(s)").Any(x => x.Equals("Secret")), "Verify All Classifications are Secret");
        }



        [Then(@"Open Search Scan Results and verify playbook OR logic details are correct '(.*)' '(.*)'")]
        public void ThenOpenSearchScanResultsAndVerifyPlaybookORLogicDetailsAreCorrect(string ssnType, bool scenario)
        {
            allScan.OpenSearchResultOfScan();
            allScan.WaitForGridToLoad();
            List<string> matches = allScan.GetAllValuesFromColumn("Match");
            List<string> locationsFromGrid = allScan.GetAllValuesFromColumn("Location").OrderBy(x => x).ToList();
            List<string> fileLocations = ScannedData.Instance.DiscoveryFiles.OrderBy(x => x).ToList();
            List<string> classifications = allScan.GetAllValuesFromColumn("Classification(s)");
            AssertExt.IsTrue(locationsFromGrid.Count == 1, "Verify all datatypes are populated");
            for (int count = 0; count < locationsFromGrid.Count; count++)
            {
                if (ssnType == "SocialSecurityNumber")
                    AssertExt.IsTrue(ScannedData.Instance.SSN[count].Equals(matches[count]), "Verify SSN matches");
                AssertExt.IsTrue(locationsFromGrid[count].ToLower().Equals(fileLocations[count].ToLower()), "Verify location matches");
                if (scenario || ssnType == "SocialSecurityNumber")
                {
                    AssertExt.IsTrue(classifications[count].Equals("Secret"), "Verify Classification is secret");
                }
                else
                {
                    AssertExt.IsTrue(classifications[count].Equals("N/A"), "Verify Clasification is N/A");
                }

            }
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Resolution").All(x => x.Equals("No Action Taken")), "Verify All Resolution are Secret");
        }


        [Then(@"Open Search Scan Results and verify playbook OR logic details are correct")]
        public void ThenOpenSearchScanResultsAndVerifyPlaybookORLogicDetailsAreCorrect()
        {
        }


        [Then(@"Open Search Scan Results and verify playbook logic details are correct")]
        public void ThenOpenSearchScanResultsAndVerifyPlaybookLogicDetailsAreCorrect()
        {
            allScan.OpenSearchResultOfScan();
            allScan.WaitForGridToLoad();
            List<string> matches = allScan.GetAllValuesFromColumn("Match");
            List<string> locationsFromGrid = allScan.GetAllValuesFromColumn("Location").OrderBy(x => x).ToList();
            List<string> fileLocations = ScannedData.Instance.DiscoveryFiles.OrderBy(x => x).ToList();
            List<string> classifications = allScan.GetAllValuesFromColumn("Classification(s)");
            AssertExt.IsTrue(matches.Count == 2, "Verify all datatypes are populated");
            for (int count = 0; count < matches.Count; count++)
            {
                AssertExt.IsTrue(ScannedData.Instance.SSN[count].Equals(matches[count]), "Verify SSN matches");
                AssertExt.IsTrue(locationsFromGrid[count].ToLower().Equals(fileLocations[count].ToLower()), "Verify Location Matches");
                if (ScannedData.Instance.DiscoveryFiles[0].ToLower() == locationsFromGrid[count].ToLower())
                {
                    AssertExt.IsTrue(classifications[count].Equals("Secret"), "Verify classification is secret");
                }
                else
                {
                    AssertExt.IsTrue(classifications[count].Equals("N/A"), "Verify classification is N/A");
                }

            }
            AssertExt.IsTrue(allScan.GetAllValuesFromColumn("Resolution").All(x => x.Equals("No Action Taken")), "Verify All Resolution are Secret");

        }

    }
}
