using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using Spirion.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;

namespace Spirion.Automation.Framework
{
    public sealed class Report
    {

        public static ExtentReports extent;
        public static ExtentTest test;
        public static ExtentHtmlReporter htmlReporter;
        string projectPath;
        //string reportPath;
        private string reportPath;
        private string reportFolderLocation;

        private DateTime startTime;
        private DateTime endTime;
        private static readonly object padlock = new object();
        public readonly string base64ImageTag = "<img style=\"width:100%;\" src='{0}'>";
        private static Report instance = null;
        public static Report Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Report();
                    }
                    return instance;
                }
            }
        }
        private Report()
        {

        }
        public void Initialize(string reportTitle, string reportheader, string reportFolder,
             string Url, string ArchiveFolder, string Archivetype)
        {
            reportFolderLocation = reportFolder;

            //To obtain the current solution path/project path
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;

            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));

            projectPath = new Uri(actualPath).LocalPath;
            reportPath = projectPath;
            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }
            if (!Directory.Exists(reportPath + reportFolder))
            {
                Directory.CreateDirectory(reportPath + reportFolder);
            }
            ArchiveFiles(reportFolder, ArchiveFolder, Archivetype);

            //string logFilePath = Path.Combine(projectPath + "\\Logs\\Log_" + DateTime.Now.ToString("MM_dd_yyyy") + ".txt");

            //Logger.log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            //ChangeFilePath("MyRollingFileAppender", logFilePath);

            //Append the html report file to current project path
            //reportPath = projectPath + "Reports\\TestRunReport_" + DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss") + ".html";
            htmlReporter = new ExtentHtmlReporter(reportPath + reportFolder);
            htmlReporter.LoadConfig(Environment.CurrentDirectory + "\\Report\\Extent-Config.xml");
            htmlReporter.Config.ReportName = reportheader;
            htmlReporter.Config.DocumentTitle = reportTitle;


            startTime = DateTime.Now;
            //Boolean value for replacing exisisting report
            extent = new ExtentReports();

            //Add QA system info to html report

            extent.AddSystemInfo("Application URL : ", Url);

            //Adding config.xml file
            extent.AttachReporter(htmlReporter);

        }

        public ExtentTest CreateTest(string testName)
        {
            try
            {
                test = extent.CreateTest(testName);
                return test;
            }
            catch (Exception ex)
            {
                throw ex;

                //Logger.WriteError(ex.Message, ex);
            }

        }

        public void LogStepInfo(string message)
        {
            test.Log(Status.Info, message);
        }

        public void LogTestResult(string status)
        {
            LogTestValidation(status, "Test ended with " + status);
        }

        public void LogTestValidation(string status, string errorMessage)
        {
            try
            {
                Status logstatus;
                switch (status)
                {
                    case "Failed":
                        logstatus = Status.Fail;
                        string screenShotPath = TakeScreenshot(DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss"));
                        test.Log(logstatus, errorMessage);
                        test.Log(logstatus, "Snapshot below:");
                        test.Log(logstatus, string.Format(base64ImageTag, screenShotPath));
                        break;
                    case "Skipped":
                        logstatus = Status.Skip;
                        test.Log(logstatus, "Skipped");
                        break;
                    default:
                        logstatus = Status.Pass;
                        //screenShotPath = TakeScreenshot(DateTime.Now.ToString("MM_dd_yyyy HH-mm-ss"));
                        test.Log(logstatus, errorMessage);
                        //test.Log(logstatus, "Snapshot below: " + test.AddScreenCaptureFromPath(screenShotPath,"Passed"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Logger.WriteError(ex.Message, ex);
            }

        }

        public void Close()
        {
            endTime = DateTime.Now;

            extent.Flush();

            //String strEmailSubject = ConfigurationManager.AppSettings["Environment"].ToString() + ConfigurationManager.AppSettings["EmailSubject"].ToString();

            //SendEmail(reportPath, ConfigurationManager.AppSettings["EmailRecivers"].ToString(), strEmailSubject);


        }
        public string TakeScreenshot(string SSName)
        {
            try
            {
                //string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;

                //string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));

                //string projectPath = new Uri(actualPath).LocalPath;

                //Append the html report file to current project path
                string path = reportPath + reportFolderLocation + "\\Screenshots\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = (path + SSName) + "." + ScreenshotImageFormat.Png;
                Screenshot ss = ((ITakesScreenshot)Browser.Instance.WebDriver).GetScreenshot();
                ss.SaveAsFile(path);
                return path;
            }
            catch (Exception ex)
            {
                //Logger.WriteError(ex.Message, ex);
                throw ex;
            }
        }

        //private void SendEmail(string filePath, string email, string email_subject)
        //{

        //    MailMessage mail = new MailMessage();
        //    mail.IsBodyHtml = true;
        //    SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
        //    SmtpServer.Port = 25;
        //    mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"].ToString());
        //    mail.To.Add(email.Trim());

        //    mail.Subject = email_subject;

        //    mail.Body = "Hi All,<br> <br>For your reference the summary report of automation execution in below environment is attached<br>";
        //    //mail.Body = mail.Body + "----------------------------------------------------------------------------------------------------------------------------------------------"+Environment.NewLine;
        //    mail.Body = mail.Body + "<br><u><b>Execution Notes</b></u><br>Execution Start : " + startTime.ToString() + "<br>Execution End: " + endTime.ToString();
        //    mail.Body = mail.Body + "<br>Testing Environment :" + ConfigurationManager.AppSettings["Environment"].ToString().Replace(":", "");
        //    mail.Body = mail.Body + "<br> URL : ";
        //    mail.Body = mail.Body + "<br><br><br>Thanks";
        //    mail.Body = mail.Body + "<br>QA Team";
        //    AttachFiles(filePath, mail);

        //    SmtpServer.Send(mail);
        //}

        private static void AttachFiles(string filePath, MailMessage mail)
        {
            //foreach (string file in files)
            //{
            Attachment attachment = new Attachment(filePath)
            {
                Name = Path.GetFileName(filePath)
            };
            mail.Attachments.Add(attachment);
            //}
        }

        private void ArchiveFiles(string reportFolder, string ArchiveFolder, string Archivetype)
        {
            try
            {
                List<string> lstFileToDelete = new List<string>();
                //DateTime dateToArchieve = DateTime.Now.Date.AddDays(-Convert.ToDouble(Config.ArchiveDuration));

                foreach (string file in Directory.GetFiles(reportPath + reportFolder))
                {
                    // DateTime fileDate = File.GetCreationTime(file).Date;
                    //if (fileDate.CompareTo(dateToArchieve) <= -1)
                    lstFileToDelete.Add(file);
                }
                //foreach (string file in Directory.GetFiles(projectPath + "Logs\\"))
                //{
                //    DateTime fileDate = File.GetCreationTime(file).Date;
                //    if (fileDate.CompareTo(dateToArchieve) <= -1)
                //        lstFileToDelete.Add(file);
                //}
                //foreach (string file in Directory.GetFiles(projectPath + "Screenshots\\"))
                //{
                //    DateTime fileDate = File.GetCreationTime(file).Date;
                //    if (fileDate.CompareTo(dateToArchieve) < -1)
                //        lstFileToDelete.Add(file);
                //}
                if (lstFileToDelete.Count > 0)
                {
                    foreach (string strFile in lstFileToDelete)
                    {
                        switch (Archivetype)
                        {
                            case "Delete":
                                File.Delete(strFile);
                                //Logger.WriteInfo("File deleted successfuly.");
                                break;
                            case "Archive":
                                if (!Directory.Exists(reportPath + ArchiveFolder))
                                    Directory.CreateDirectory(reportPath + ArchiveFolder);
                                File.Move(strFile, reportPath + ArchiveFolder + "\\TestReportRun " + File.GetCreationTime(strFile).ToString("MM_dd_yyyy HH-mm-ss") + ".html", true);
                                //Logger.WriteInfo("File removed successfuly.");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteError(ex.Message, ex);
                throw ex;
            }
        }
    }

}
