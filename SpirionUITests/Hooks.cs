using Spirion.Automation.Framework;
using SpirionUITests.Fixtures;
using SpirionUITests.Models;
using SpirionUITests.Pages;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
//[assembly: Parallelizable(ParallelScope.Fixtures)]
//[assembly: LevelOfParallelism(2)]
namespace SpirionUITests
{
    [Binding]
    public class Hooks
    {
        public static ExtentTest feature;
        public static ExtentTest scenario;

        private static EnvironmentFixture environmentFixture;
        private static ReportFixture reportFixture;
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioInfo)
        {
            scenario = feature.CreateNode<Feature>(scenarioInfo.ScenarioInfo.Title);
            Report.test = scenario;
            Browser.Instance.InitWebDriver(environmentFixture.Environment.Browser);

            ScannedData.Instance.DiscoveryFiles = new System.Collections.Generic.List<string>();
            ScannedData.Instance.SSN = new System.Collections.Generic.List<string>();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureInfo)
        {
            feature = Report.Instance.CreateTest(featureInfo.FeatureInfo.Title);
        }



        [BeforeTestRun]
        public static void RunBeforeAllTests()
        {
            environmentFixture = new EnvironmentFixture();
            reportFixture = new ReportFixture();
            Logger.InitLogging(environmentFixture.Environment.LogFile);
            Report.Instance.Initialize(reportFixture.ReportDetails.ReportTitle, reportFixture.ReportDetails.ReportName
                , reportFixture.ReportDetails.ReportPath, environmentFixture.Environment.TestUrl, reportFixture.ReportDetails.ArchivePath
                , reportFixture.ReportDetails.ArchiveType);
            ScannedData.RemoteFlag = false;

        }

        [AfterScenario]
        public static void TearDown(ScenarioContext scenarioInfo)
        {
            string status = scenarioInfo.TestError == null ? "Pass" : "Fail";
            Report.Instance.LogTestResult(status);
            try
            {
                if (environmentFixture.Environment.DeleteSearchAfterTestRun)
                {
                    if (new AllScanPage().AreYouOnScanLink())
                    {
                        new AllScanPage().ClickScan();
                        new AllScanPage().DeleteExistingSearch(ScannedData.Instance.ScanName);
                    }
                }
            }
            catch
            {
                Logger.LogError("Search not deleted");
            }
            try
            {
                List<string> keywords = new List<string> { "On", "Before", "After" };
                if (keywords.Any(x => ScannedData.Instance.PlaybookName.Contains(x)))
                {
                    if (new AllScanPage().AreYouOnScanLink())
                    {
                        new AllScanPage().ClickScan();
                    }
                    new BasePage().ClickScanPlayBooks();
                    new Playbooks().DeleteExistingPlaybook(ScannedData.Instance.PlaybookName);

                }
            }
            catch
            {

            }

            finally
            {
                var scenarioDispose = scenarioInfo as IDisposable;
                scenarioDispose.Dispose();
                Browser.Instance.CloseWebDriver();
            }
        }


        [AfterFeature]
        public static void AfterFeature(FeatureContext feature)
        {


        }

        [AfterTestRun]
        public static void AfterTestRu()
        {
            Report.Instance.Close();
            Browser.Instance.CloseWebDriver();
        }
    }
}
